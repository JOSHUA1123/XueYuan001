using System;
using System.IO;
using Newtonsoft.Json;

namespace pili_sdk.pili_common
{
	// Token: 0x0200000C RID: 12
	public class Utils
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00002D6C File Offset: 0x00000F6C
		public static string JsonEncode(object obj)
		{
			return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			});
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002D94 File Offset: 0x00000F94
		public static T ToObject<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002DAC File Offset: 0x00000FAC
		public static void Copy(Stream dst, Stream src)
		{
			long position = src.Position;
			byte[] buffer = new byte[Utils.bufferLen];
			for (;;)
			{
				int num = src.Read(buffer, 0, Utils.bufferLen);
				bool flag = num == 0;
				if (flag)
				{
					break;
				}
				dst.Write(buffer, 0, num);
			}
			src.Seek(position, SeekOrigin.Begin);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002E00 File Offset: 0x00001000
		public static void CopyN(Stream dst, Stream src, long numBytesToCopy)
		{
			long position = src.Position;
			byte[] buffer = new byte[Utils.bufferLen];
			long num;
			int num3;
			for (num = 0L; num < numBytesToCopy; num += (long)num3)
			{
				int num2 = Utils.bufferLen;
				bool flag = numBytesToCopy - num < (long)num2;
				if (flag)
				{
					num2 = (int)(numBytesToCopy - num);
				}
				num3 = src.Read(buffer, 0, num2);
				bool flag2 = num3 == 0;
				if (flag2)
				{
					break;
				}
				dst.Write(buffer, 0, num3);
			}
			src.Seek(position, SeekOrigin.Begin);
			bool flag3 = num != numBytesToCopy;
			if (flag3)
			{
				throw new Exception("StreamUtil.CopyN: nwritten not equal to ncopy");
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002E98 File Offset: 0x00001098
		public static string UserAgent
		{
			get
			{
				string str = "csharp";
				string str2 = "windows";
				string str3 = "pili-sdk-csharp1.5.0";
				return str3 + str2 + str;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002EC4 File Offset: 0x000010C4
		public static bool isArgNotEmpty(string arg)
		{
			return arg != null && arg.Trim().Length > 0;
		}

		// Token: 0x04000020 RID: 32
		public static int bufferLen = 32768;
	}
}
