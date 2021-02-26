using System;

namespace pili_sdk.pili_common
{
	// Token: 0x0200000B RID: 11
	public class UrlSafeBase64
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public static string encodeToString(string data)
		{
			try
			{
				return UrlSafeBase64.encodeToString(data.GetBytes("UTF-8"));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				Console.Write(ex.StackTrace);
			}
			return null;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D2C File Offset: 0x00000F2C
		public static string encodeToString(byte[] data)
		{
			return Base64.encodeToString(data, 10);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002D48 File Offset: 0x00000F48
		public static byte[] decode(string data)
		{
			return Base64.decode(data, 10);
		}
	}
}
