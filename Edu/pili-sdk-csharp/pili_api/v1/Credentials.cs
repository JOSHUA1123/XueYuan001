using System;
using System.Security.Cryptography;
using System.Text;
using pili_sdk.pili_common;

namespace pili_sdk.pili_api.v1
{
	// Token: 0x0200000E RID: 14
	public class Credentials : ICredentials
	{
		// Token: 0x06000064 RID: 100 RVA: 0x000031B4 File Offset: 0x000013B4
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

		// Token: 0x06000065 RID: 101 RVA: 0x00003224 File Offset: 0x00001424
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

		// Token: 0x06000066 RID: 102 RVA: 0x0000333C File Offset: 0x0000153C
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

		// Token: 0x06000067 RID: 103 RVA: 0x000033C0 File Offset: 0x000015C0
		public string sign(string secret, string data)
		{
			return UrlSafeBase64.encodeToString(this.digest(secret, data));
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000033E0 File Offset: 0x000015E0
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

		// Token: 0x06000069 RID: 105 RVA: 0x00003450 File Offset: 0x00001650
		private static HMACSHA1 createMac(HMACSHA1 secretKeySpec)
		{
			return secretKeySpec;
		}

		// Token: 0x04000025 RID: 37
		private const string DIGEST_AUTH_PREFIX = "Qiniu";

		// Token: 0x04000026 RID: 38
		private HMACSHA1 mSkSpec;

		// Token: 0x04000027 RID: 39
		private string mAccessKey;

		// Token: 0x04000028 RID: 40
		private string mSecretKey;
	}
}
