using System;

namespace pili_sdk.pili_common
{
	// Token: 0x02000009 RID: 9
	public class Config
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002A7C File Offset: 0x00000C7C
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002AA6 File Offset: 0x00000CA6
		public static string API_VERSION
		{
			get
			{
				return string.IsNullOrWhiteSpace(Config._API_VERSION) ? "v1" : Config._API_VERSION;
			}
			private set
			{
				Config._API_VERSION = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002AB0 File Offset: 0x00000CB0
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public static string ACCESS_KEY
		{
			get
			{
				bool flag = string.IsNullOrWhiteSpace(Config._ACCESS_KEY);
				if (flag)
				{
					throw new Exception("ACCESS_KEY 不可为空");
				}
				return Config._ACCESS_KEY;
			}
			private set
			{
				Config._ACCESS_KEY = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002AEC File Offset: 0x00000CEC
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002B1C File Offset: 0x00000D1C
		public static string SECRET_KEY
		{
			get
			{
				bool flag = string.IsNullOrWhiteSpace(Config._SECRET_KEY);
				if (flag)
				{
					throw new Exception("SECRET_KEY 不可为空");
				}
				return Config._SECRET_KEY;
			}
			private set
			{
				Config._SECRET_KEY = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002B28 File Offset: 0x00000D28
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002B58 File Offset: 0x00000D58
		public static string HUB_NAME
		{
			get
			{
				bool flag = string.IsNullOrWhiteSpace(Config._HUB_NAME);
				if (flag)
				{
					throw new Exception("HUB_NAME 不可为空");
				}
				return Config._HUB_NAME;
			}
			private set
			{
				Config._HUB_NAME = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002B64 File Offset: 0x00000D64
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002B95 File Offset: 0x00000D95
		public static bool DEFAULT_USE_HTTPS
		{
			get
			{
				bool flag = Config._DEFAULT_USE_HTTPS == null;
				return !flag && Config._DEFAULT_USE_HTTPS.Value;
			}
			private set
			{
				Config._DEFAULT_USE_HTTPS = new bool?(value);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002BA4 File Offset: 0x00000DA4
		public static string API_BASE_URL
		{
			get
			{
				return string.Format("{0}://{1}/{2}", Config.DEFAULT_USE_HTTPS ? "https" : "http", "pili.qiniuapi.com", Config.API_VERSION);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002BDF File Offset: 0x00000DDF
		public static void SetVersion(string version)
		{
			Config._API_VERSION = version;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public static void SetHub(string hub)
		{
			Config._HUB_NAME = hub;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002BF1 File Offset: 0x00000DF1
		public static void SetKey(string access, string secret)
		{
			Config._ACCESS_KEY = access;
			Config._SECRET_KEY = secret;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C00 File Offset: 0x00000E00
		public static void SetKey(string access, string secret, string hub)
		{
			Config._ACCESS_KEY = access;
			Config._SECRET_KEY = secret;
			Config._HUB_NAME = hub;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002C15 File Offset: 0x00000E15
		public static void Set(string access, string secret, string hub, string version)
		{
			Config._ACCESS_KEY = access;
			Config._SECRET_KEY = secret;
			Config._HUB_NAME = hub;
			Config._API_VERSION = version;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002C30 File Offset: 0x00000E30
		public static void Set(string access, string secret, string hub, string version, bool usehttps)
		{
			Config._ACCESS_KEY = access;
			Config._SECRET_KEY = secret;
			Config._HUB_NAME = hub;
			Config._API_VERSION = version;
			Config._DEFAULT_USE_HTTPS = new bool?(usehttps);
		}

		// Token: 0x0400000D RID: 13
		public const string SDK_VERSION = "1.5.0";

		// Token: 0x0400000E RID: 14
		public const string USER_AGENT = "pili-sdk-csharp";

		// Token: 0x0400000F RID: 15
		public const string UTF8 = "UTF-8";

		// Token: 0x04000010 RID: 16
		public const int TITLE_MIN_LENGTH = 5;

		// Token: 0x04000011 RID: 17
		public const int TITLE_MAX_LENGTH = 200;

		// Token: 0x04000012 RID: 18
		public const string DEFAULT_API_HOST = "pili.qiniuapi.com";

		// Token: 0x04000013 RID: 19
		private static string _API_VERSION = "";

		// Token: 0x04000014 RID: 20
		private static string _ACCESS_KEY = "";

		// Token: 0x04000015 RID: 21
		private static string _SECRET_KEY = "";

		// Token: 0x04000016 RID: 22
		private static string _HUB_NAME = "";

		// Token: 0x04000017 RID: 23
		private static bool? _DEFAULT_USE_HTTPS = null;
	}
}
