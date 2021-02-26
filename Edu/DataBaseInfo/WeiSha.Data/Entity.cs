using Microsoft.JScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Xml;
using DataBaseInfo.Design;
namespace DataBaseInfo
{
	[Serializable]
	public abstract class Entity : EntityBase, IEntity, ICloneable
	{
		public void Attach(params Field[] removeFields)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.isUpdate = true;
				this.RemoveFieldsToUpdate(removeFields);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public void Detach(params Field[] removeFields)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.isUpdate = false;
				this.RemoveFieldsToInsert(removeFields);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public string ToJson()
		{
			return this.ToJson(null, null);
		}
		public string ToJson(string only, string wipe)
		{
			return this.ToJson(only, wipe, null);
		}
		public string ToJson(string only, string wipe, Dictionary<string, object> addParas)
		{
			string[] array = null;
			string[] array2 = null;
			if (!string.IsNullOrWhiteSpace(only))
			{
				array = only.Split(new char[]
				{
					','
				});
			}
			if (!string.IsNullOrWhiteSpace(wipe))
			{
				array2 = wipe.Split(new char[]
				{
					','
				});
			}
			string text = "{";
			Type type = base.GetType();
			PropertyInfo[] properties = type.GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				object value = type.GetProperty(propertyInfo.Name).GetValue(this, null);
				if ((array == null || array.Length <= 0 || this._tojson_isExist(propertyInfo.Name, array)) && (array2 == null || array2.Length <= 0 || !this._tojson_isExist(propertyInfo.Name, array2)))
				{
					Type underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
					string typename = (underlyingType != null) ? underlyingType.Name : propertyInfo.PropertyType.Name;
					text = text + this._tojson_property(typename, propertyInfo.Name, value) + ",";
				}
			}
			if (addParas != null && addParas.Count > 0)
			{
				foreach (KeyValuePair<string, object> current in addParas)
				{
					if (current.Value != null)
					{
						text = text + this._tojson_property(current.Value.GetType().Name, current.Key, current.Value) + ",";
					}
					else
					{
						text = text + this._tojson_property("String", current.Key, current.Value) + ",";
					}
				}
			}
			if (text.Length > 0 && text.Substring(text.Length - 1, 1) == ",")
			{
				text = text.Substring(0, text.Length - 1);
			}
			text += "}";
			return text;
		}
		private bool _tojson_isExist(string piname, string[] arr)
		{
			bool result = false;
			if (arr != null && arr.Length > 0)
			{
				for (int i = 0; i < arr.Length; i++)
				{
					string text = arr[i];
					if (text.Trim() == piname)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
		private string _tojson_property(string typename, string name, object value)
		{
			string text = "\"";
			if (typename != null)
			{
				if (typename == "DateTime")
				{
					DateTime d = DateTime.Now;
					if (value != null)
					{
						d = System.Convert.ToDateTime(value);
					}
					DateTime d2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
					long num = (long)(d - d2).TotalMilliseconds;
					string str = string.Format("eval('new ' + eval('/Date({0})/').source)", num);
					text = text + name + "\":" + str;
					return text;
				}
				if (typename == "String")
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						name,
						"\":\"",
						(value == null) ? "" : GlobalObject.escape(value.ToString()),
						"\""
					});
					return text;
				}
				if (typename == "Boolean")
				{
					text = text + name + "\":" + value.ToString().ToLower();
					return text;
				}
			}
			string text3 = text;
			text = string.Concat(new string[]
			{
				text3,
				name,
				"\":\"",
				(value == null) ? "" : GlobalObject.escape(value.ToString()),
				"\""
			});
			return text;
		}
		public string ToXML()
		{
			Type type = base.GetType();
			XmlDocument xmlDocument = new XmlDocument();
			XmlNode xmlNode = xmlDocument.CreateElement(type.Name);
			xmlDocument.AppendChild(xmlNode);
			PropertyInfo[] properties = type.GetProperties();
			int i = 0;
			while (i < properties.Length)
			{
				PropertyInfo propertyInfo = properties[i];
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, propertyInfo.Name, null);
				XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("type");
				xmlAttribute.Value = propertyInfo.PropertyType.FullName;
				xmlNode2.Attributes.SetNamedItem(xmlAttribute);
				object value = propertyInfo.GetValue(this, null);
				string fullName;
				if ((fullName = propertyInfo.PropertyType.FullName) == null)
				{
					goto IL_119;
				}
				if (!(fullName == "System.String"))
				{
					if (!(fullName == "System.DateTime"))
					{
						if (!(fullName == "System.Boolean"))
						{
							goto IL_119;
						}
						xmlNode2.InnerText = value.ToString().ToLower();
					}
					else
					{
						xmlNode2.InnerText = System.Convert.ToDateTime(value).ToString();
					}
				}
				else
				{
					XmlCDataSection newChild = xmlDocument.CreateCDataSection((value != null) ? value.ToString() : "");
					xmlNode2.AppendChild(newChild);
				}
				IL_132:
				xmlNode.AppendChild(xmlNode2);
				i++;
				continue;
				IL_119:
				xmlNode2.InnerText = ((value != null) ? value.ToString() : "");
				goto IL_132;
			}
			return xmlDocument.OuterXml;
		}
		public void FromXML(string xml)
		{
			Type type = base.GetType();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNodeList childNodes = xmlDocument.FirstChild.ChildNodes;
			foreach (XmlNode xmlNode in childNodes)
			{
				if (!string.IsNullOrWhiteSpace(xmlNode.InnerText))
				{
					PropertyInfo property = type.GetProperty(xmlNode.Name);
					if (!(property == null))
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						string attribute;
						if ((attribute = xmlElement.GetAttribute("type")) != null && attribute == "System.DateTime")
						{
							DateTime dateTime = System.Convert.ToDateTime(xmlNode.InnerText);
							type.GetProperty(xmlNode.Name).SetValue(this, dateTime, null);
						}
						else
						{
							object value = System.Convert.ChangeType(xmlNode.InnerText, property.PropertyType);
							type.GetProperty(xmlNode.Name).SetValue(this, value, null);
						}
					}
				}
			}
		}
		public void AttachAll(params Field[] removeFields)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.AddFieldsToUpdate(this.GetFields());
				this.Attach(removeFields);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public void DetachAll(params Field[] removeFields)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.removeinsertlist.Clear();
				this.Detach(removeFields);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public void AttachSet(params Field[] setFields)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.updatelist.Clear();
				this.AddFieldsToUpdate(setFields);
				this.Attach(new Field[0]);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		public void DetachSet(params Field[] setFields)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.removeinsertlist.Clear();
				List<Field> fields = new List<Field>(setFields);
				List<Field> list = new List<Field>(this.GetFields());
				list.RemoveAll((Field f) => fields.Any((Field p) => p.Name == f.Name));
				this.RemoveFieldsToInsert(list);
				this.Detach(new Field[0]);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		protected void OnPropertyValueChange(Field field, object oldValue, object newValue)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				if (this.isFromDB)
				{
					if (oldValue != null)
					{
						if (!oldValue.Equals(newValue))
						{
							this.AddFieldsToUpdate(field);
						}
					}
					else
					{
						if (newValue != null && !newValue.Equals(oldValue))
						{
							this.AddFieldsToUpdate(field);
						}
					}
				}
				else
				{
					this.AddFieldsToUpdate(field);
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		private void AddFieldsToUpdate(IList<Field> fields)
		{
			if (fields == null || fields.Count == 0)
			{
				return;
			}
			foreach (Field current in fields)
			{
				this.AddFieldsToUpdate(current);
			}
		}
		private void AddFieldsToUpdate(Field field)
		{
			if (!this.updatelist.Exists((Field p) => p.Name == field.Name))
			{
				lock (this.updatelist)
				{
					this.updatelist.Add(field);
				}
			}
		}
		private void RemoveFieldsToUpdate(IList<Field> fields)
		{
			if (fields == null || fields.Count == 0)
			{
				return;
			}
			foreach (Field field in fields)
			{
				lock (this.updatelist)
				{
					this.updatelist.RemoveAll((Field p) => p.Name == field.Name);
				}
			}
		}
		private void RemoveFieldsToInsert(IList<Field> fields)
		{
			if (fields == null || fields.Count == 0)
			{
				return;
			}
			foreach (Field field in fields)
			{
				if (!this.removeinsertlist.Exists((Field p) => p.Name == field.Name))
				{
					lock (this.removeinsertlist)
					{
						this.removeinsertlist.Add(field);
					}
				}
			}
		}
		public object Clone()
		{
			return base.MemberwiseClone();
		}
		public T Clone<T>() where T : Entity
		{
			Type type = base.GetType();
			PropertyInfo[] properties = type.GetProperties();
			object obj = type.InvokeMember("", BindingFlags.CreateInstance, null, this, null);
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				if (propertyInfo.CanWrite)
				{
					object value = propertyInfo.GetValue(this, null);
					propertyInfo.SetValue(obj, value, null);
				}
			}
			return (T)((object)obj);
		}
		public T DeepClone<T>() where T : Entity
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			T result;
			using (stream)
			{
				formatter.Serialize(stream, this);
				stream.Seek(0L, SeekOrigin.Begin);
				result = (T)((object)formatter.Deserialize(stream));
			}
			return result;
		}
	}
}
