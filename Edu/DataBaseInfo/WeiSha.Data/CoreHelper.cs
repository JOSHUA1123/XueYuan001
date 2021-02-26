using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DataBaseInfo.Converter;
namespace DataBaseInfo
{
	public static class CoreHelper
	{
		private static byte[] Keys = new byte[]
		{
			65,
			114,
			101,
			121,
			111,
			117,
			109,
			121,
			83,
			110,
			111,
			119,
			109,
			97,
			110,
			63
		};
		public static string GetClientIP()
		{
			if (HttpContext.Current == null)
			{
				return "localhost";
			}
			HttpRequest request = HttpContext.Current.Request;
			string text = request.ServerVariables["HTTP_CDN_SRC_IP"];
			if (string.IsNullOrEmpty(text))
			{
				text = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			}
			if (string.IsNullOrEmpty(text))
			{
				text = request.ServerVariables["REMOTE_ADDR"];
			}
			if (string.IsNullOrEmpty(text))
			{
				text = request.UserHostAddress;
			}
			return text;
		}
		public static string GetFullPath(string path)
		{
			path = path.Replace("/", "\\").TrimStart(new char[]
			{
				'\\'
			});
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
		}
		public static object GetTypeDefaultValue(Type type)
		{
			Type type2 = type;
			if (type.IsByRef)
			{
				type2 = type.GetElementType();
			}
			if (type == typeof(void))
			{
				return null;
			}
			return typeof(CoreHelper).GetMethod("DefaultValue", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				type2
			}).Invoke(null, null);
		}
		private static object DefaultValue<MemberType>()
		{
			return default(MemberType);
		}
		public static string RemoveSurplusSpaces(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			RegexOptions options = RegexOptions.None;
			Regex regex = new Regex("[ ]{2,}", options);
			return regex.Replace(value, " ").Trim();
		}
		public static bool CheckStructType(object value)
		{
			return CoreHelper.CheckStructType(value.GetType());
		}
		public static bool CheckStructType(Type type)
		{
			return type.IsValueType && !type.IsEnum && !type.IsPrimitive && string.Compare(type.Namespace, "system", true) != 0;
		}
		public static object CloneObject(object obj)
		{
			object result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj);
				memoryStream.Position = 0L;
				result = binaryFormatter.Deserialize(memoryStream);
			}
			return result;
		}
		public static T CreateInstance<T>()
		{
			return CoreHelper.CreateInstance<T>(typeof(T));
		}
		public static T CreateInstance<T>(Type type)
		{
			if (!type.IsPublic)
			{
				return (T)((object)Activator.CreateInstance(type));
			}
			return (T)((object)CoreHelper.GetFastInstanceCreator(type)());
		}
		public static FastCreateInstanceHandler GetFastInstanceCreator(Type type)
		{
			if (type.IsInterface)
			{
				throw new SNDataException("可实例化的对象类型不能是接口！");
			}
			return DynamicCalls.GetInstanceCreator(type);
		}
		public static FastInvokeHandler GetFastMethodInvoke(MethodInfo method)
		{
			return DynamicCalls.GetMethodInvoker(method);
		}
		public static void SetPropertyValue(object obj, PropertyInfo property, object value)
		{
			if (obj == null)
			{
				return;
			}
			if (!property.CanWrite)
			{
				return;
			}
			try
			{
				FastPropertySetHandler propertySetter = DynamicCalls.GetPropertySetter(property);
				value = CoreHelper.ConvertValue(property.PropertyType, value);
				propertySetter(obj, value);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public static void SetPropertyValue(object obj, string propertyName, object value)
		{
			if (obj == null)
			{
				return;
			}
			PropertyInfo property = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				CoreHelper.SetPropertyValue(obj, property, value);
			}
		}
		public static object GetPropertyValue(object obj, PropertyInfo property)
		{
			if (obj == null)
			{
				return null;
			}
			if (!property.CanRead)
			{
				return null;
			}
			object result;
			try
			{
				FastPropertyGetHandler propertyGetter = DynamicCalls.GetPropertyGetter(property);
				result = propertyGetter(obj);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		public static object GetPropertyValue(object obj, string propertyName)
		{
			if (obj == null)
			{
				return null;
			}
			PropertyInfo property = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				return CoreHelper.GetPropertyValue(obj, property);
			}
			return null;
		}
		public static T ConvertValue<T>(object value)
		{
			if (value == DBNull.Value || value == null)
			{
				return default(T);
			}
			if (value is T)
			{
				return (T)((object)value);
			}
			object obj = CoreHelper.ConvertValue(typeof(T), value);
			if (obj == null)
			{
				return default(T);
			}
			return (T)((object)obj);
		}
		public static object ConvertValue(Type type, object value)
		{
			if (value == DBNull.Value || value == null)
			{
				return null;
			}
			if (CoreHelper.CheckStructType(type))
			{
				return null;
			}
			Type type2 = value.GetType();
			if (type.IsAssignableFrom(type2))
			{
				return value;
			}
			if (type.IsEnum)
			{
				try
				{
					object result = Enum.ToObject(type, value);
					return result;
				}
				catch
				{
					object result = Enum.Parse(type, value.ToString(), true);
					return result;
				}
			}
			return CoreHelper.ChangeType(value, type);
		}
		public static object ConvertJsonValue(Type type, string jsonString)
		{
			object obj = null;
			if (type.IsByRef)
			{
				type = type.GetElementType();
			}
			Type type2 = type;
			if (type.IsEnum)
			{
				type = typeof(int);
			}
			if (!string.IsNullOrEmpty(jsonString))
			{
				Type primitiveType = CoreHelper.GetPrimitiveType(type);
				if (primitiveType.IsValueType || primitiveType == typeof(string))
				{
					if (type.IsArray)
					{
						if (!jsonString.StartsWith("[") || !jsonString.EndsWith("]"))
						{
							jsonString = string.Format("[{0}]", jsonString.Replace(",", "\",\""));
						}
					}
					else
					{
						if (type.IsGenericType)
						{
							Type genericTypeDefinition = type.GetGenericTypeDefinition();
							if ((typeof(IList<>).IsAssignableFrom(genericTypeDefinition) || typeof(List<>).IsAssignableFrom(genericTypeDefinition) || typeof(ICollection<>).IsAssignableFrom(genericTypeDefinition) || typeof(Collection<>).IsAssignableFrom(genericTypeDefinition) || typeof(IEnumerable<>).IsAssignableFrom(genericTypeDefinition)) && (!jsonString.StartsWith("[") || !jsonString.EndsWith("]")))
							{
								jsonString = string.Format("[{0}]", jsonString.Replace(",", "\",\""));
							}
						}
					}
				}
			}
			if (obj == null)
			{
				obj = CoreHelper.GetTypeDefaultValue(type);
			}
			return CoreHelper.ConvertValue(type2, obj);
		}
		public static PropertyInfo[] GetPropertiesFromType(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		}
		public static PropertyInfo[] GetPropertiesFromType<T>()
		{
			return CoreHelper.GetPropertiesFromType(typeof(T));
		}
		public static MethodInfo[] GetMethodsFromType(Type type)
		{
			return type.GetAllMethods().ToArray<MethodInfo>();
		}
		public static MethodInfo[] GetMethodsFromType<T>()
		{
			return CoreHelper.GetMethodsFromType(typeof(T));
		}
		public static MethodInfo GetMethodFromType<T>(string methodName)
		{
			return CoreHelper.GetMethodFromType(typeof(T), methodName);
		}
		public static MethodInfo GetMethodFromType(Type type, string methodName)
		{
			return (
				from p in CoreHelper.GetMethodsFromType(type)
				where p.ToString() == methodName
				select p).FirstOrDefault<MethodInfo>();
		}
		public static T[] GetMemberAttributes<T>(MemberInfo member)
		{
			Converter<object, T> converter = null;
			object[] customAttributes = member.GetCustomAttributes(typeof(T), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				List<object> arg_34_0 = new List<object>(customAttributes);
				if (converter == null)
				{
					converter = ((object p) => (T)((object)p));
				}
				return arg_34_0.ConvertAll<T>(converter).ToArray();
			}
			return null;
		}
		public static T GetMemberAttribute<T>(MemberInfo member)
		{
			object[] customAttributes = member.GetCustomAttributes(typeof(T), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				return (T)((object)customAttributes[0]);
			}
			return default(T);
		}
		public static T GetTypeAttribute<T>(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(T), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				return (T)((object)customAttributes[0]);
			}
			return default(T);
		}
		public static T[] GetTypeAttributes<T>(Type type)
		{
			Converter<object, T> converter = null;
			object[] customAttributes = type.GetCustomAttributes(typeof(T), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				List<object> arg_34_0 = new List<object>(customAttributes);
				if (converter == null)
				{
					converter = ((object p) => (T)((object)p));
				}
				return arg_34_0.ConvertAll<T>(converter).ToArray();
			}
			return null;
		}
		public static T ConvertTo<T>(string value, T defaultValue)
		{
			bool flag = false;
			Type type = typeof(T);
			if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				type = Nullable.GetUnderlyingType(type);
				flag = true;
			}
			if (ConverterFactory.Converters.ContainsKey(type))
			{
				if (string.IsNullOrEmpty(value) && flag)
				{
					return defaultValue;
				}
				bool flag2;
				object obj = ConverterFactory.Converters[type].ConvertTo(value, out flag2);
				if (flag2)
				{
					return (T)((object)obj);
				}
			}
			return defaultValue;
		}
		public static TOutput ConvertType<TInput, TOutput>(TInput obj)
		{
			if (obj == null)
			{
				return default(TOutput);
			}
			if (obj is TOutput && typeof(TOutput).IsInterface)
			{
				return (TOutput)((object)obj);
			}
			TOutput tOutput = default(TOutput);
			try
			{
				if (typeof(TOutput) == typeof(TInput))
				{
					tOutput = CoreHelper.CreateInstance<TOutput>(obj.GetType());
				}
				else
				{
					tOutput = CoreHelper.CreateInstance<TOutput>();
				}
			}
			catch (Exception inner)
			{
				throw new SNDataException(string.Format("创建类型对象【{0}】出错，可能不存在构造函数！", typeof(TOutput).FullName), inner);
			}
			PropertyInfo[] propertiesFromType = CoreHelper.GetPropertiesFromType<TOutput>();
			int i = 0;
			while (i < propertiesFromType.Length)
			{
				PropertyInfo propertyInfo = propertiesFromType[i];
				object obj2;
				if (obj is NameValueCollection)
				{
					NameValueCollection nameValueCollection = obj as NameValueCollection;
					if (nameValueCollection[propertyInfo.Name] != null)
					{
						obj2 = nameValueCollection[propertyInfo.Name];
						goto IL_199;
					}
				}
				else
				{
					if (obj is IDictionary)
					{
						IDictionary dictionary = obj as IDictionary;
						if (dictionary.Contains(propertyInfo.Name) && dictionary[propertyInfo.Name] != null)
						{
							obj2 = dictionary[propertyInfo.Name];
							goto IL_199;
						}
					}
					else
					{
						if (!(obj is DataRow))
						{
							obj2 = CoreHelper.GetPropertyValue(obj, propertyInfo.Name);
							goto IL_199;
						}
						DataRow dataRow = obj as DataRow;
						if (CoreHelper.Contains(dataRow, propertyInfo.Name) && !dataRow.IsNull(propertyInfo.Name))
						{
							obj2 = dataRow[propertyInfo.Name];
							goto IL_199;
						}
					}
				}
				IL_1A9:
				i++;
				continue;
				IL_199:
				if (obj2 != null)
				{
					CoreHelper.SetPropertyValue(tOutput, propertyInfo, obj2);
					goto IL_1A9;
				}
				goto IL_1A9;
			}
			return tOutput;
		}
		private static bool Contains(DataRow row, string name)
		{
			return row != null && row.Table.Columns.Contains(name);
		}
		public static string MakeUniqueKey(int length, string prefix)
		{
			if (prefix != null && prefix.Length >= length)
			{
				throw new ArgumentException("错误的前缀，传入的前缀长度大于总长度！");
			}
			int num = (prefix == null) ? 0 : prefix.Length;
			string text = "1234567890abcdefghijklmnopqrstuvwxyz";
			StringBuilder stringBuilder = new StringBuilder();
			if (num > 0)
			{
				stringBuilder.Append(prefix);
			}
			int num2 = 0;
			int num3 = 0;
			Random random = new Random(Guid.NewGuid().GetHashCode());
			for (int i = 0; i < length - num; i++)
			{
				int num4 = random.Next(0, 35);
				if (num4 == num3)
				{
					num2++;
				}
				stringBuilder.Append(text[num4]);
				num3 = num4;
			}
			if (num2 >= length - num - 2)
			{
				random = new Random(Guid.NewGuid().GetHashCode());
				return CoreHelper.MakeUniqueKey(length, prefix);
			}
			return stringBuilder.ToString();
		}
		private static object ChangeType(object value, Type conversionType)
		{
			if (value == null)
			{
				return null;
			}
			bool flag = false;
			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				conversionType = Nullable.GetUnderlyingType(conversionType);
				flag = true;
			}
			if (!(value.GetType() == typeof(string)))
			{
				return Convert.ChangeType(value, conversionType);
			}
			string value2 = value.ToString();
			if (string.IsNullOrEmpty(value2) && flag)
			{
				return null;
			}
			bool flag2;
			value = ConverterFactory.Converters[conversionType].ConvertTo(value2, out flag2);
			if (flag2)
			{
				return value;
			}
			throw new SNDataException(string.Format("【{0}】转换成数据类型【{1}】出错！", value, conversionType.Name));
		}
		public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
		{
			if (string.IsNullOrEmpty(p_SrcString))
			{
				return p_SrcString;
			}
			if (p_Length < 0)
			{
				return p_SrcString;
			}
			byte[] bytes = Encoding.Default.GetBytes(p_SrcString);
			if (bytes.Length <= p_Length)
			{
				return p_SrcString;
			}
			int num = p_Length;
			int[] array = new int[p_Length];
			int num2 = 0;
			for (int i = 0; i < p_Length; i++)
			{
				if (bytes[i] > 127)
				{
					num2++;
					if (num2 == 3)
					{
						num2 = 1;
					}
				}
				else
				{
					num2 = 0;
				}
				array[i] = num2;
			}
			if (bytes[p_Length - 1] > 127 && array[p_Length - 1] == 1)
			{
				num = p_Length + 1;
			}
			byte[] array2 = new byte[num];
			Array.Copy(bytes, array2, num);
			return Encoding.Default.GetString(array2) + p_TailString;
		}
		public static string Encrypt(string text, string key)
		{
			string result;
			try
			{
				key = key.PadRight(32, ' ');
				ICryptoTransform cryptoTransform = new RijndaelManaged
				{
					Key = Encoding.UTF8.GetBytes(key),
					IV = CoreHelper.Keys
				}.CreateEncryptor();
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
				result = Convert.ToBase64String(inArray);
			}
			catch
			{
				result = null;
			}
			return result;
		}
		public static string Decrypt(string text, string key)
		{
			string result;
			try
			{
				key = key.PadRight(32, ' ');
				ICryptoTransform cryptoTransform = new RijndaelManaged
				{
					Key = Encoding.UTF8.GetBytes(key),
					IV = CoreHelper.Keys
				}.CreateDecryptor();
				byte[] array = Convert.FromBase64String(text);
				byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
				result = Encoding.UTF8.GetString(bytes);
			}
			catch
			{
				result = null;
			}
			return result;
		}
		public static int Compare<T>(T value1, T value2)
		{
			int result;
			try
			{
				int num;
				if (value1 == null && value2 == null)
				{
					num = 0;
				}
				else
				{
					if (value1 == null)
					{
						num = -1;
					}
					else
					{
						if (value2 == null)
						{
							num = 1;
						}
						else
						{
							if (value1.GetType().IsGenericType && value1.GetType().GetGenericTypeDefinition() == typeof(Nullable<>) && value2.GetType().IsGenericType && value2.GetType().GetGenericTypeDefinition() == typeof(Nullable<>))
							{
								Type underlyingType = Nullable.GetUnderlyingType(value1.GetType());
								Type underlyingType2 = Nullable.GetUnderlyingType(value2.GetType());
								value1 = (T)((object)Convert.ChangeType(value1, underlyingType));
								value2 = (T)((object)Convert.ChangeType(value2, underlyingType2));
								num = ((IComparable)((object)value1)).CompareTo((IComparable)((object)value2));
							}
							else
							{
								num = ((IComparable)((object)value1)).CompareTo((IComparable)((object)value2));
							}
						}
					}
				}
				result = num;
			}
			catch (Exception ex)
			{
				throw new SNDataException("比较两个值大小时发生错误：" + ex.Message, ex);
			}
			return result;
		}
		public static bool CheckPrimitiveType(params Type[] types)
		{
			if (types == null || types.Length == 0)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				Type primitiveType = CoreHelper.GetPrimitiveType(type);
				if (!primitiveType.IsValueType && !primitiveType.IsEnum && !(primitiveType == typeof(string)))
				{
					result = false;
					break;
				}
			}
			return result;
		}
		public static string GetTypeName(Type type)
		{
			string text = type.Name;
			if (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericArguments();
				List<string> list = new List<string>();
				Type[] array = genericArguments;
				for (int i = 0; i < array.Length; i++)
				{
					Type type2 = array[i];
					list.Add(CoreHelper.GetTypeName(type2));
				}
				if (text.Contains("`" + genericArguments.Length))
				{
					text = text.Replace("`" + genericArguments.Length, "<" + string.Join(", ", list.ToArray()) + ">");
				}
			}
			return text;
		}
		public static Type GetPrimitiveType(Type type)
		{
			if (type.IsByRef)
			{
				type = type.GetElementType();
			}
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (typeof(IList<>).IsAssignableFrom(genericTypeDefinition) || typeof(List<>).IsAssignableFrom(genericTypeDefinition) || typeof(ICollection<>).IsAssignableFrom(genericTypeDefinition) || typeof(Collection<>).IsAssignableFrom(genericTypeDefinition) || typeof(IEnumerable<>).IsAssignableFrom(genericTypeDefinition))
				{
					Type[] genericArguments = type.GetGenericArguments();
					type = CoreHelper.GetPrimitiveType(genericArguments[0]);
				}
				else
				{
					if (typeof(IDictionary<, >).IsAssignableFrom(genericTypeDefinition) || typeof(Dictionary<, >).IsAssignableFrom(genericTypeDefinition))
					{
						Type[] genericArguments2 = type.GetGenericArguments();
						type = CoreHelper.GetPrimitiveType(genericArguments2[0]);
						if (type.IsValueType || type == typeof(string))
						{
							type = CoreHelper.GetPrimitiveType(genericArguments2[1]);
						}
					}
				}
			}
			return type;
		}
	}
}
