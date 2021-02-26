using System;
using System.Security.Cryptography;
using System.Text;
using pili_sdk.pili_common;

namespace pili_sdk.pili_qiniu
{
	// Token: 0x02000007 RID: 7
	public class Credentials
	{
		// Token: 0x0600002F RID: 47 RVA: 0x0000259C File Offset: 0x0000079C
		public Credentials(string ak, string sk)
		{
			bool flag = ak == null || sk == null;
			if (flag)
			{
				throw new ArgumentException("Invalid accessKey or secretKey!!");
			}
			this.mAccessKey = ak;
			this.mSecretKey = sk;
			try
			{
				this.mSkSpec = new HMACSHA1(this.mSecretKey.GetBytes("UTF-8"));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000260C File Offset: 0x0000080C
		public virtual string signRequest(Uri url, string method, byte[] body, string contentType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = string.Format("{0} {1}", method, url.LocalPath);
			stringBuilder.Append(value);
			bool flag = url.Query != "";
			if (flag)
			{
				stringBuilder.Append(url.Query);
			}
			stringBuilder.Append(string.Format("\nHost: {0}", url.Host));
			bool flag2 = url.Port != 80;
			if (flag2)
			{
				stringBuilder.Append(string.Format(":{0}", url.Port));
			}
			bool flag3 = contentType != null;
			if (flag3)
			{
				stringBuilder.Append(string.Format("\nContent-Type: {0}", contentType));
			}
			stringBuilder.Append("\n\n");
			bool flag4 = body != null && contentType != null && !"application/octet-stream".Equals(contentType);
			if (flag4)
			{
				stringBuilder.Append(StringHelperClass.NewString(body));
			}
			return string.Format("{0} {1}:{2}", "Qiniu", this.mAccessKey, this.signData(stringBuilder.ToString()));
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002724 File Offset: 0x00000924
		private static byte[] digest(string secret, string data)
		{
			byte[] result;
			try
			{
				Encoding utf = Encoding.UTF8;
				byte[] bytes = utf.GetBytes(data);
				byte[] bytes2 = utf.GetBytes(secret);
				HMACSHA1 hmacsha = new HMACSHA1(bytes2);
				byte[] array = hmacsha.ComputeHash(bytes);
				result = array;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				Console.Write(ex.StackTrace);
				throw new Exception("Failed to digest: " + ex.Message);
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000027A8 File Offset: 0x000009A8
		public static string sign(string secret, string data)
		{
			return UrlSafeBase64.encodeToString(Credentials.digest(secret, data));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000027C8 File Offset: 0x000009C8
		private string signData(string data)
		{
			string result = null;
			try
			{
				HMACSHA1 hmacsha = Credentials.createMac(this.mSkSpec);
				Encoding utf = Encoding.UTF8;
				byte[] bytes = utf.GetBytes(data);
				byte[] data2 = hmacsha.ComputeHash(bytes);
				result = UrlSafeBase64.encodeToString(data2);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to generate HMAC : " + ex.Message);
			}
			return result;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002838 File Offset: 0x00000A38
		private static HMACSHA1 createMac(HMACSHA1 secretKeySpec)
		{
			return secretKeySpec;
		}

		// Token: 0x04000003 RID: 3
		private const string DIGEST_AUTH_PREFIX = "Qiniu";

		// Token: 0x04000004 RID: 4
		private HMACSHA1 mSkSpec;

		// Token: 0x04000005 RID: 5
		private string mAccessKey;

		// Token: 0x04000006 RID: 6
		private string mSecretKey;
	}
}
