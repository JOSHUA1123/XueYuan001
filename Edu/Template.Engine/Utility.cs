using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace VTemplate.Engine
{
	/// <summary>
	/// 实用类
	/// </summary>
	// Token: 0x02000015 RID: 21
	internal static class Utility
	{
		/// <summary>
		///
		/// </summary>
		// Token: 0x060000E7 RID: 231 RVA: 0x00004E3B File Offset: 0x0000303B
		static Utility()
		{
			Utility.TypeCache = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// 判断是否是空数据(null或DBNull)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000E8 RID: 232 RVA: 0x00004E6A File Offset: 0x0000306A
		internal static bool IsNothing(object value)
		{
			return value == null || value == DBNull.Value;
		}

		/// <summary>
		/// 判断是否是整数
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000E9 RID: 233 RVA: 0x00004E7C File Offset: 0x0000307C
		internal static bool IsInteger(string value)
		{
			int num;
			return !string.IsNullOrEmpty(value) && (value[0] == '-' || char.IsDigit(value[0])) && int.TryParse(value, out num);
		}

		/// <summary>
		/// 转移为某种类型
		/// </summary>
		/// <param name="value"></param>
		/// <param name="conversionType"></param>
		/// <returns></returns>
		// Token: 0x060000EA RID: 234 RVA: 0x00004EB8 File Offset: 0x000030B8
		internal static object ChangeType(object value, Type conversionType)
		{
			object result;
			try
			{
				result = Convert.ChangeType(value, conversionType);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 比较两个值
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <param name="success"></param>
		/// <returns></returns>
		// Token: 0x060000EB RID: 235 RVA: 0x00004EE8 File Offset: 0x000030E8
		internal static int CompareTo(object value1, object value2, out bool success)
		{
			Type type = value1.GetType();
			Type type2 = value2.GetType();
			bool flag = Type.GetTypeCode(type) != TypeCode.Object;
			bool flag2 = Type.GetTypeCode(type2) != TypeCode.Object;
			success = false;
			if (flag)
			{
				if (type != type2)
				{
					if (type.IsEnum)
					{
						value2 = Utility.ConvertTo(value2.ToString(), type);
					}
					else if (value2 is IConvertible)
					{
						value2 = Utility.ChangeType(value2, type);
					}
					else
					{
						value2 = null;
					}
				}
			}
			else if (flag2 && type != type2)
			{
				if (type2.IsEnum)
				{
					value1 = Utility.ConvertTo(value1.ToString(), type2);
				}
				else if (value1 is IConvertible)
				{
					value1 = Utility.ChangeType(value1, type2);
				}
				else
				{
					value1 = null;
				}
			}
			if (value1 != null && value1 is IComparable)
			{
				success = true;
				return ((IComparable)value1).CompareTo(value2);
			}
			if (value2 != null && value2 is IComparable)
			{
				success = true;
				return -((IComparable)value2).CompareTo(value1);
			}
			return 1;
		}

		/// <summary>
		/// XML编码
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000EC RID: 236 RVA: 0x00004FD0 File Offset: 0x000031D0
		internal static string XmlEncode(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
					{
						xmlTextWriter.WriteString(value);
						xmlTextWriter.Flush();
						value = stringWriter.ToString();
					}
				}
			}
			return value;
		}

		/// <summary>
		/// 文本编码
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000ED RID: 237 RVA: 0x0000503C File Offset: 0x0000323C
		internal static string TextEncode(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				value = HttpUtility.HtmlEncode(value);
				value = Regex.Replace(value, "  |\t", "&nbsp;&nbsp;");
				value = Regex.Replace(value, "\r\n|\r|\n", "<br />");
			}
			return value;
		}

		/// <summary>
		/// JS脚本编码
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000EE RID: 238 RVA: 0x00005074 File Offset: 0x00003274
		internal static string JsEncode(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				value = value.Replace("\\", "\\\\");
				value = value.Replace("\"", "\\\"");
				value = value.Replace("'", "\\'");
				value = value.Replace("\r", "\\r");
				value = value.Replace("\n", "\\n");
			}
			return value;
		}

		/// <summary>
		/// 压缩文本
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000EF RID: 239 RVA: 0x000050E4 File Offset: 0x000032E4
		internal static string CompressText(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				value = Regex.Replace(value, "[ \\t]*\\r?\\n[ \\t]*", "");
			}
			return value;
		}

		/// <summary>
		/// 删除HTML代码
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000F0 RID: 240 RVA: 0x00005101 File Offset: 0x00003301
		internal static string RemoveHtmlCode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			value = Regex.Replace(value, "<script[^>]*>.*?</script>|<style[^>]*>.*?</style>|<!--.*?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			return Regex.Replace(value, "</?\\s*[\\w:\\-]+(\\s*[\\w:\\-]+\\s*(=\\s*\"[^\"]*\"|=\\s*'[^']*'|=\\s*[^\\s=>]*))*\\s*/?>", "").Trim();
		}

		/// <summary>
		/// 将某个集合数据拆分为一组一组数据
		/// 例如: int[] datas = [1,2,3,4,5,6]; 当SplitToGroup(datas, 2)拆分后,将会拆分为类似以下的集合"[1,2],[3,4],[5,6]"
		/// </summary>
		/// <param name="list"></param>
		/// <param name="groupSize"></param>
		/// <returns></returns>
		// Token: 0x060000F1 RID: 241 RVA: 0x0000513C File Offset: 0x0000333C
		internal static IEnumerator SplitToGroup(IEnumerator list, int groupSize)
		{
			List<ArrayList> list2 = new List<ArrayList>();
			for (;;)
			{
				ArrayList arrayList = new ArrayList();
				int num = 0;
				while (num < groupSize && list.MoveNext())
				{
					arrayList.Add(list.Current);
					num++;
				}
				if (arrayList.Count <= 0)
				{
					break;
				}
				list2.Add(arrayList);
			}
			if (list2.Count > 0)
			{
				return list2.GetEnumerator();
			}
			return list;
		}

		/// <summary>
		/// 转换字符串为布尔值
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000F2 RID: 242 RVA: 0x0000519E File Offset: 0x0000339E
		internal static bool ConverToBoolean(string value)
		{
			return value == "1" || string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// 转换字符串为整型值
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000F3 RID: 243 RVA: 0x000051C0 File Offset: 0x000033C0
		internal static int ConverToInt32(string value)
		{
			int result;
			if (!int.TryParse(value, out result))
			{
				result = 0;
			}
			return result;
		}

		/// <summary>
		/// 转换字符串为数值
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000F4 RID: 244 RVA: 0x000051DC File Offset: 0x000033DC
		internal static decimal ConverToDecimal(object value)
		{
			if (value == null || value == DBNull.Value)
			{
				return 0m;
			}
			decimal result;
			try
			{
				result = Convert.ToDecimal(value);
			}
			catch
			{
				result = 0m;
			}
			return result;
		}

		/// <summary>
		/// 截取字符
		/// </summary>
		/// <param name="value">要截取的字符串</param>
		/// <param name="maxLength">最大大小</param>
		/// <param name="charset">采用的编码</param>
		/// <param name="appendText">附加字符</param>
		/// <returns></returns>
		// Token: 0x060000F5 RID: 245 RVA: 0x00005220 File Offset: 0x00003420
		internal static string CutString(string value, int maxLength, Encoding charset, string appendText)
		{
			StringBuilder stringBuilder = new StringBuilder(maxLength);
			int num = 0;
			int i;
			for (i = 0; i < value.Length; i++)
			{
				char c = value[i];
				num += charset.GetByteCount(new char[]
				{
					c
				});
				if (num > maxLength)
				{
					break;
				}
				stringBuilder.Append(c);
			}
			if (i < value.Length && !string.IsNullOrEmpty(appendText))
			{
				stringBuilder.Append(appendText);
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// 从字符集名称获取编码器
		/// </summary>
		/// <param name="charset"></param>
		/// <param name="defaultCharset"></param>
		/// <returns></returns>
		// Token: 0x060000F6 RID: 246 RVA: 0x00005294 File Offset: 0x00003494
		internal static Encoding GetEncodingFromCharset(string charset, Encoding defaultCharset)
		{
			Encoding result = defaultCharset;
			try
			{
				result = Encoding.GetEncoding(charset);
			}
			catch
			{
				result = defaultCharset;
			}
			return result;
		}

		/// <summary>
		/// 转换为某种数据类型
		/// </summary>
		/// <param name="value">要转换的字符串</param>
		/// <param name="type">最终的数据类型</param>
		/// <returns>如果转换失败返回null</returns>
		// Token: 0x060000F7 RID: 247 RVA: 0x000052C4 File Offset: 0x000034C4
		internal static object ConvertTo(string value, Type type)
		{
			object result = value;
			if (value != null)
			{
				try
				{
					if (type.IsEnum)
					{
						result = Enum.Parse(type, value, true);
					}
					else if (Type.GetTypeCode(type) == TypeCode.DateTime)
					{
						result = DateTime.Parse(value);
					}
					else
					{
						result = ((IConvertible)value).ToType(type, null);
					}
				}
				catch
				{
					result = null;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取条件的比较类型
		/// </summary>
		/// <param name="compareType"></param>
		/// <returns></returns>
		// Token: 0x060000F8 RID: 248 RVA: 0x00005324 File Offset: 0x00003524
		internal static IfConditionCompareType GetIfConditionCompareType(string compareType)
		{
			IfConditionCompareType result = IfConditionCompareType.Equal;
			if (!string.IsNullOrEmpty(compareType))
			{
				string key;
				switch (key = compareType.Trim())
				{
				case ">":
					return IfConditionCompareType.GT;
				case ">=":
					return IfConditionCompareType.GTAndEqual;
				case "<":
					return IfConditionCompareType.LT;
				case "<=":
					return IfConditionCompareType.LTAndEqual;
				case "<>":
				case "!=":
					return IfConditionCompareType.UnEqual;
				case "^=":
					return IfConditionCompareType.StartWith;
				case "$=":
					return IfConditionCompareType.EndWith;
				case "*=":
					return IfConditionCompareType.Contains;
				}
				result = IfConditionCompareType.Equal;
			}
			return result;
		}

		/// <summary>
		/// 获取某个对象对应的DbType
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060000F9 RID: 249 RVA: 0x00005430 File Offset: 0x00003630
		internal static DbType GetObjectDbType(object value)
		{
			if (value == null)
			{
				return DbType.Object;
			}
			switch (Type.GetTypeCode((value is Type) ? ((Type)value) : value.GetType()))
			{
			case TypeCode.Boolean:
				return DbType.Boolean;
			case TypeCode.Char:
			case TypeCode.String:
				return DbType.String;
			case TypeCode.SByte:
				return DbType.SByte;
			case TypeCode.Byte:
				return DbType.Byte;
			case TypeCode.Int16:
				return DbType.Int16;
			case TypeCode.UInt16:
				return DbType.UInt16;
			case TypeCode.Int32:
				return DbType.Int32;
			case TypeCode.UInt32:
				return DbType.UInt32;
			case TypeCode.Int64:
				return DbType.Int64;
			case TypeCode.UInt64:
				return DbType.UInt64;
			case TypeCode.Single:
				return DbType.Single;
			case TypeCode.Double:
				return DbType.Double;
			case TypeCode.Decimal:
				return DbType.Decimal;
			case TypeCode.DateTime:
				return DbType.DateTime;
			}
			return DbType.Object;
		}

		/// <summary>
		/// 获取某个属性的值
		/// </summary>
		/// <param name="container">数据源</param>
		/// <param name="propName">属性名</param>
		/// <param name="exist">是否存在此属性</param>
		/// <returns>属性值</returns>
		// Token: 0x060000FA RID: 250 RVA: 0x000054D0 File Offset: 0x000036D0
		internal static object GetPropertyValue(object container, string propName, out bool exist)
		{
			exist = false;
			object obj = null;
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			if (string.IsNullOrEmpty(propName))
			{
				throw new ArgumentNullException("propName");
			}
			if (Utility.IsInteger(propName))
			{
				int num = Utility.ConverToInt32(propName);
				if (container is IList)
				{
					IList list = (IList)container;
					if (list.Count > num)
					{
						exist = true;
						return list[num];
					}
					return obj;
				}
				else if (container is ICollection)
				{
					ICollection collection = (ICollection)container;
					if (collection.Count > num)
					{
						exist = true;
						IEnumerator enumerator = collection.GetEnumerator();
						int num2 = 0;
						while (num2++ <= num)
						{
							enumerator.MoveNext();
						}
						return enumerator.Current;
					}
					return obj;
				}
				else
				{
					PropertyInfo property = container.GetType().GetProperty("Item", new Type[]
					{
						typeof(int)
					});
					if (!(property != null))
					{
						return obj;
					}
					try
					{
						obj = property.GetValue(container, new object[]
						{
							num
						});
						exist = true;
						return obj;
					}
					catch
					{
						exist = false;
						return obj;
					}
				}
			}
			Type type = (container is Type) ? ((Type)container) : container.GetType();
			BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			if (!(container is Type))
			{
				bindingFlags |= BindingFlags.Instance;
			}
			FieldInfo field = type.GetField(propName, bindingFlags);
			if (field != null)
			{
				exist = true;
				obj = field.GetValue(container);
			}
			else
			{
				PropertyInfo property2 = type.GetProperty(propName, bindingFlags, null, null, Type.EmptyTypes, new ParameterModifier[0]);
				if (property2 != null)
				{
					exist = true;
					obj = property2.GetValue(container, null);
				}
				else if (container is ICustomTypeDescriptor)
				{
					ICustomTypeDescriptor customTypeDescriptor = (ICustomTypeDescriptor)container;
					PropertyDescriptor propertyDescriptor = customTypeDescriptor.GetProperties().Find(propName, true);
					if (propertyDescriptor != null)
					{
						exist = true;
						obj = propertyDescriptor.GetValue(container);
					}
				}
				else if (container is IDictionary)
				{
					IDictionary dictionary = (IDictionary)container;
					if (dictionary.Contains(propName))
					{
						exist = true;
						obj = dictionary[propName];
					}
				}
				else if (container is NameObjectCollectionBase)
				{
					NameObjectCollectionBase nameObjectCollectionBase = (NameObjectCollectionBase)container;
					MethodInfo method = nameObjectCollectionBase.GetType().GetMethod("BaseGet", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(string)
					}, new ParameterModifier[]
					{
						new ParameterModifier(1)
					});
					if (method != null)
					{
						obj = method.Invoke(container, new object[]
						{
							propName
						});
						exist = (obj != null);
					}
				}
				else
				{
					PropertyInfo property3 = type.GetProperty("Item", new Type[]
					{
						typeof(string)
					});
					if (property3 != null)
					{
						try
						{
							obj = property3.GetValue(container, new object[]
							{
								propName
							});
							exist = true;
						}
						catch
						{
							exist = false;
						}
					}
				}
			}
			return obj;
		}

		/// <summary>
		/// 获取方法的结果值
		/// </summary>
		/// <param name="container"></param>
		/// <param name="methodName"></param>
		/// <param name="exist"></param>
		/// <returns></returns>
		// Token: 0x060000FB RID: 251 RVA: 0x000057D8 File Offset: 0x000039D8
		internal static object GetMethodResult(object container, string methodName, out bool exist)
		{
			exist = false;
			Type type = (container is Type) ? ((Type)container) : container.GetType();
			MethodInfo method = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, new ParameterModifier[0]);
			object result = null;
			if (method != null)
			{
				exist = true;
				return method.Invoke(method.IsStatic ? null : container, null);
			}
			return result;
		}

		/// <summary>
		/// 返回数据源的枚举数
		/// </summary>
		/// <param name="dataSource">要处理的数据源</param>
		/// <returns>如果非IListSource与IEnumerable实例则返回null</returns>
		// Token: 0x060000FC RID: 252 RVA: 0x00005838 File Offset: 0x00003A38
		internal static IEnumerable GetResolvedDataSource(object dataSource)
		{
			if (dataSource != null)
			{
				if (dataSource is IListSource)
				{
					IListSource listSource = (IListSource)dataSource;
					IList list = listSource.GetList();
					if (!listSource.ContainsListCollection)
					{
						return list;
					}
					if (list != null && list is ITypedList)
					{
						PropertyDescriptorCollection itemProperties = ((ITypedList)list).GetItemProperties(new PropertyDescriptor[0]);
						if (itemProperties == null || itemProperties.Count == 0)
						{
							return null;
						}
						PropertyDescriptor propertyDescriptor = itemProperties[0];
						if (propertyDescriptor != null)
						{
							object component = list[0];
							object value = propertyDescriptor.GetValue(component);
							if (value != null && value is IEnumerable)
							{
								return (IEnumerable)value;
							}
						}
						return null;
					}
				}
				if (dataSource is IEnumerable)
				{
					return (IEnumerable)dataSource;
				}
			}
			return null;
		}

		/// <summary>
		/// 修正文件地址
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		// Token: 0x060000FD RID: 253 RVA: 0x000058DC File Offset: 0x00003ADC
		internal static string ResolveFilePath(Tag tag, string fileName)
		{
			if (!string.IsNullOrEmpty(fileName) && fileName.IndexOf(":") == -1 && !fileName.StartsWith("\\\\"))
			{
				string text = string.Empty;
				while (string.IsNullOrEmpty(text) && tag != null)
				{
					if (tag is Template)
					{
						text = ((Template)tag).File;
					}
					else if (tag is IncludeTag)
					{
						text = ((IncludeTag)tag).File;
					}
					tag = tag.Parent;
				}
				if (!string.IsNullOrEmpty(text))
				{
					fileName = Path.Combine(Path.GetDirectoryName(text), fileName);
				}
				fileName = Path.GetFullPath(fileName);
			}
			return fileName;
		}

		/// <summary>
		/// 统计行号与列号(x = 列号, y = 行号)
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		// Token: 0x060000FE RID: 254 RVA: 0x00005970 File Offset: 0x00003B70
		internal static Point GetLineAndColumnNumber(string text, int offset)
		{
			int num2;
			int num = num2 = 1;
			int num3 = 0;
			while (num3 < offset && num3 < text.Length)
			{
				char c = text[num3];
				if (c == '\r' || c == '\n')
				{
					if (c == '\r' && num3 < text.Length - 1 && text[num3 + 1] == '\n')
					{
						num3++;
					}
					num2++;
					num = 1;
				}
				else
				{
					num++;
				}
				num3++;
			}
			return new Point(num, num2);
		}

		/// <summary>
		/// 从模板中获取某个变量.如果不存在此变量则添加新的变量
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="varName"></param>
		/// <returns></returns>
		// Token: 0x060000FF RID: 255 RVA: 0x000059DC File Offset: 0x00003BDC
		internal static Variable GetVariableOrAddNew(Template ownerTemplate, string varName)
		{
			Variable variable = ownerTemplate.Variables[varName];
			if (variable == null)
			{
				variable = new Variable(ownerTemplate, varName);
				ownerTemplate.Variables.Add(variable);
			}
			return variable;
		}

		/// <summary>
		/// 根据前缀获取变量的模板所有者
		/// </summary>
		/// <param name="template"></param>
		/// <param name="prefix"></param>
		/// <returns>如果prefix值为null则返回template的根模板.如果为空值.则为template.如果为#则返回template的父模板.否则返回对应Id的模板</returns>
		// Token: 0x06000100 RID: 256 RVA: 0x00005A10 File Offset: 0x00003C10
		internal static Template GetOwnerTemplateByPrefix(Template template, string prefix)
		{
			if (prefix == string.Empty)
			{
				return template;
			}
			if (prefix == "#")
			{
				return template.OwnerTemplate ?? template;
			}
			while (template.OwnerTemplate != null)
			{
				template = template.OwnerTemplate;
			}
			if (prefix != null)
			{
				return template.GetChildTemplateById(prefix);
			}
			return template;
		}

		/// <summary>
		/// 建立某个类型
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
		// Token: 0x06000101 RID: 257 RVA: 0x00005A61 File Offset: 0x00003C61
		internal static Type CreateType(string typeName)
		{
			return Utility.CreateType(typeName, null);
		}

		/// <summary>
		/// 建立某个类型
		/// </summary>
		/// <param name="typeName">类型名称</param>
		/// <param name="assembly">程序集.如果为空.则表示当前程序域里的所有程序集</param>
		/// <returns></returns>
		// Token: 0x06000102 RID: 258 RVA: 0x00005A6C File Offset: 0x00003C6C
		internal static Type CreateType(string typeName, string assembly)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				return null;
			}
			Type type = null;
			bool flag = false;
			string key = typeName + "," + assembly;
			lock (Utility.TypeCache)
			{
				flag = Utility.TypeCache.TryGetValue(key, out type);
			}
			if (!flag)
			{
				if (string.IsNullOrEmpty(assembly))
				{
					type = Type.GetType(typeName, false, true);
					if (type == null)
					{
						Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
						foreach (Assembly assembly2 in assemblies)
						{
							type = assembly2.GetType(typeName, false, true);
							if (type != null)
							{
								break;
							}
						}
					}
				}
				else
				{
					Assembly assembly3;
					if (assembly.IndexOf(":") != -1)
					{
						assembly3 = Assembly.LoadFrom(assembly);
					}
					else
					{
						assembly3 = Assembly.Load(assembly);
					}
					if (assembly3 != null)
					{
						type = assembly3.GetType(typeName, false, true);
					}
				}
				lock (Utility.TypeCache)
				{
					if (!Utility.TypeCache.ContainsKey(key))
					{
						Utility.TypeCache.Add(key, type);
					}
				}
			}
			return type;
		}

		/// <summary>
		/// 获取解析器的实例
		/// </summary>
		/// <param name="renderInstance"></param>
		/// <returns></returns>
		// Token: 0x06000103 RID: 259 RVA: 0x00005BAC File Offset: 0x00003DAC
		private static object GetRenderInstance(string renderInstance)
		{
			if (string.IsNullOrEmpty(renderInstance))
			{
				return null;
			}
			string[] array = renderInstance.Split(new char[]
			{
				','
			}, 2);
			if (array.Length != 2)
			{
				return null;
			}
			string text = array[1].Trim();
			string text2 = array[0].Trim();
			string key = text2 + "," + text;
			bool flag = false;
			object obj;
			lock (Utility.RenderInstanceCache)
			{
				flag = Utility.RenderInstanceCache.TryGetValue(key, out obj);
			}
			if (!flag || obj == null)
			{
				obj = null;
				Assembly assembly;
				if (text.IndexOf(":") != -1)
				{
					assembly = Assembly.LoadFrom(text);
				}
				else
				{
					assembly = Assembly.Load(text);
				}
				if (assembly != null)
				{
					obj = assembly.CreateInstance(text2, false);
				}
				if (obj != null)
				{
					lock (Utility.RenderInstanceCache)
					{
						if (Utility.RenderInstanceCache.ContainsKey(key))
						{
							Utility.RenderInstanceCache[key] = obj;
						}
						else
						{
							Utility.RenderInstanceCache.Add(key, obj);
						}
					}
				}
			}
			return obj;
		}

		/// <summary>
		/// 预解析模板数据
		/// </summary>
		/// <param name="renderInstance">模板解析器实例的配置</param>
		/// <param name="template">要解析处理的模板</param>
		// Token: 0x06000104 RID: 260 RVA: 0x00005CE4 File Offset: 0x00003EE4
		internal static void PreRenderTemplate(string renderInstance, Template template)
		{
			ITemplateRender templateRender = Utility.GetRenderInstance(renderInstance) as ITemplateRender;
			if (templateRender != null)
			{
				templateRender.PreRender(template);
			}
		}

		/// <summary>
		/// 使用特性方法预解析模板数据
		/// </summary>
		/// <param name="renderInstance"></param>
		/// <param name="renderMethod"></param>
		/// <param name="template"></param>
		// Token: 0x06000105 RID: 261 RVA: 0x00005D08 File Offset: 0x00003F08
		internal static void PreRenderTemplateByAttributeMethod(string renderInstance, string renderMethod, Template template)
		{
			object renderInstance2 = Utility.GetRenderInstance(renderInstance);
			if (renderInstance2 != null)
			{
				MethodInfo methodInfo = null;
				MethodInfo[] methods = renderInstance2.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo2 in methods)
				{
                    TemplateRenderMethodAttribute templateRenderMethodAttribute = System.Attribute.GetCustomAttribute(methodInfo2, typeof(TemplateRenderMethodAttribute)) as TemplateRenderMethodAttribute;
					if (templateRenderMethodAttribute != null && renderMethod.Equals(methodInfo2.Name, StringComparison.InvariantCultureIgnoreCase))
					{
						methodInfo = methodInfo2;
						break;
					}
				}
				if (methodInfo != null)
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == typeof(Template))
					{
						methodInfo.Invoke(methodInfo.IsStatic ? null : renderInstance2, new object[]
						{
							template
						});
					}
				}
			}
		}

		/// <summary>
		/// 建立数据驱动工厂
		/// </summary>
		/// <param name="providerName"></param>
		// Token: 0x06000106 RID: 262 RVA: 0x00005DD0 File Offset: 0x00003FD0
		internal static DbProviderFactory CreateDbProviderFactory(string providerName)
		{
			if (string.IsNullOrEmpty(providerName))
			{
				return null;
			}
			bool flag = false;
			DbProviderFactory factory;
			lock (Utility.DbFactoriesCache)
			{
				flag = Utility.DbFactoriesCache.TryGetValue(providerName, out factory);
			}
			if (!flag || factory == null)
			{
				factory = DbProviderFactories.GetFactory(providerName);
				if (factory != null)
				{
					lock (Utility.DbFactoriesCache)
					{
						if (Utility.DbFactoriesCache.ContainsKey(providerName))
						{
							Utility.DbFactoriesCache[providerName] = factory;
						}
						else
						{
							Utility.DbFactoriesCache.Add(providerName, factory);
						}
					}
				}
			}
			return factory;
		}

		/// <summary>
		/// 类型的缓存
		/// </summary>
		// Token: 0x04000034 RID: 52
		private static Dictionary<string, Type> TypeCache;

		/// <summary>
		/// 存储模板解析器实例的缓存
		/// </summary>
		// Token: 0x04000035 RID: 53
		private static Dictionary<string, object> RenderInstanceCache = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// 数据驱动工厂实例的缓存
		/// </summary>
		// Token: 0x04000036 RID: 54
		private static Dictionary<string, DbProviderFactory> DbFactoriesCache = new Dictionary<string, DbProviderFactory>(StringComparer.InvariantCultureIgnoreCase);
	}
}
