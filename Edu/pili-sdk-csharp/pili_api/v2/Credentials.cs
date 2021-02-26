using System;
using System.Security.Cryptography;
using System.Text;
using pili_sdk.pili_common;

namespace pili_sdk.pili_api.v2
{
	// Token: 0x0200000D RID: 13
	public class Credentials : ICredentials
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00002F00 File Offset: 0x00001100
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

		// Token: 0x0600005F RID: 95 RVA: 0x00002F70 File Offset: 0x00001170
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

		// Token: 0x06000060 RID: 96 RVA: 0x00003088 File Offset: 0x00001288
		private byte[] digest(string secret, string data)
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

		// Token: 0x06000061 RID: 97 RVA: 0x0000310C File Offset: 0x0000130C
		public string sign(string secret, string data)
		{
			return UrlSafeBase64.encodeToString(this.digest(secret, data));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000312C File Offset: 0x0000132C
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

		// Token: 0x06000063 RID: 99 RVA: 0x0000319C File Offset: 0x0000139C
		private static HMACSHA1 createMac(HMACSHA1 secretKeySpec)
		{
			return secretKeySpec;
		}

		// Token: 0x04000021 RID: 33
		private const string DIGEST_AUTH_PREFIX = "Qiniu";

		// Token: 0x04000022 RID: 34
		private HMACSHA1 mSkSpec;

		// Token: 0x04000023 RID: 35
		private string mAccessKey;

		// Token: 0x04000024 RID: 36
		private string mSecretKey;
	}
}
