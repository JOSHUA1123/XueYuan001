using System;

namespace pili_sdk
{
	// Token: 0x02000003 RID: 3
	public interface ICredentials
	{
		// Token: 0x0600000B RID: 11
		string signRequest(Uri url, string method, byte[] body, string contentType);

		// Token: 0x0600000C RID: 12
		string sign(string secret, string data);
	}
}
