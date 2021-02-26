using System;
using System.Collections.Generic;
using System.Reflection;
using pili_sdk.pili_common;
using Common;

namespace pili_sdk
{
	// Token: 0x02000005 RID: 5
	public class Pili
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000021FE File Offset: 0x000003FE
		public static void Initialization(string accesskey, string secretkey, string hubname)
		{
			Config.SetKey(accesskey, secretkey, hubname);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000220A File Offset: 0x0000040A
		public static void Initialization(string accesskey, string secretkey, string hubname, string version)
		{
			Config.SetKey(accesskey, secretkey, hubname);
			Config.SetVersion(version);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002220 File Offset: 0x00000420
		public static T API<T>()
		{
			return Pili.API<T>(null, null, null, null);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000223C File Offset: 0x0000043C
		public static T API<T>(string accesskey, string secretkey)
		{
			return Pili.API<T>(accesskey, secretkey, null, null);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002258 File Offset: 0x00000458
		public static T API<T>(string version)
		{
			return Pili.API<T>(null, null, null, version);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002274 File Offset: 0x00000474
		public static T API<T>(string accesskey, string secretkey, string hubname, string version)
		{
			License value = License.Value;
			bool flag = value.VersionLevel < 6;
			if (flag)
			{
				throw new Exception("直播功能需要更高的商业授权");
			}
			bool flag2 = !string.IsNullOrEmpty(version);
			if (flag2)
			{
				Config.SetVersion(version);
			}
			bool flag3 = !string.IsNullOrEmpty(accesskey) && !string.IsNullOrEmpty(secretkey);
			if (flag3)
			{
				Config.SetVersion(version);
			}
			bool flag4 = !string.IsNullOrEmpty(hubname);
			if (flag4)
			{
				Config.SetHub(hubname);
			}
			bool flag5 = !typeof(T).IsInterface;
			if (flag5)
			{
				throw new ArgumentException("泛型调用的类不是接口类型。");
			}
			object obj = Pili._GetObject(typeof(T));
			bool flag6 = obj != null;
			T result;
			if (flag6)
			{
				result = (T)((object)obj);
			}
			else
			{
				List<Type> list = new List<Type>();
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				foreach (Type type in executingAssembly.GetTypes())
				{
					Type[] interfaces = type.GetInterfaces();
					bool flag7 = interfaces.Length != 0;
					if (flag7)
					{
						foreach (Type type2 in interfaces)
						{
							bool flag8 = type2.FullName.Equals(typeof(T).FullName);
							if (flag8)
							{
								list.Add(type);
								break;
							}
						}
					}
				}
				string value2 = Config.API_VERSION + "." + typeof(T).Name.Substring(1) + "_Impl";
				Type type3 = null;
				foreach (Type type4 in list)
				{
					bool flag9 = type4.FullName.EndsWith(value2);
					if (flag9)
					{
						type3 = type4;
						break;
					}
				}
				bool flag10 = type3 == null;
				if (flag10)
				{
					result = default(T);
				}
				else
				{
					object obj2 = Activator.CreateInstance(type3);
					Pili._AddDictionary(typeof(T), obj2);
					result = (T)((object)obj2);
				}
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024A0 File Offset: 0x000006A0
		private static void _AddDictionary(Type type, object obj)
		{
			object @lock = Pili._lock;
			lock (@lock)
			{
				bool flag2 = !Pili.dic.ContainsKey(type);
				if (flag2)
				{
					Pili.dic.Add(type, obj);
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002500 File Offset: 0x00000700
		private static object _GetObject(Type type)
		{
			foreach (KeyValuePair<Type, object> keyValuePair in Pili.dic)
			{
				bool flag = keyValuePair.Key.FullName.Equals(type.FullName);
				if (flag)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x04000001 RID: 1
		private static Dictionary<Type, object> dic = new Dictionary<Type, object>();

		// Token: 0x04000002 RID: 2
		private static object _lock = new object();
	}
}
