using System;
using pili_sdk.pili_qiniu;

namespace pili_sdk.pili
{
	// Token: 0x02000011 RID: 17
	public class Hub
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004478 File Offset: 0x00002678
		public Hub(Credentials credentials, string hubName)
		{
			bool flag = hubName == null;
			if (flag)
			{
				throw new ArgumentException("FATAL EXCEPTION: hubName is null!");
			}
			bool flag2 = credentials == null;
			if (flag2)
			{
				throw new ArgumentException("FATAL EXCEPTION: credentials is null!");
			}
			this.mCredentials = credentials;
			this.mHubName = hubName;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000044C3 File Offset: 0x000026C3
		public Hub(string accessKey, string secretKey, string hubName)
		{
			this.mCredentials = new Credentials(accessKey, secretKey);
			this.mHubName = hubName;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000044E4 File Offset: 0x000026E4
		public static Hub Create(string accessKey, string secretKey, string hubName)
		{
			return new Hub(accessKey, secretKey, hubName);
		}

		// Token: 0x0400002A RID: 42
		private Credentials mCredentials;

		// Token: 0x0400002B RID: 43
		private string mHubName;
	}
}
