using System;
using Newtonsoft.Json.Linq;

namespace pili_sdk.pili
{
	// Token: 0x02000012 RID: 18
	public class SnapshotResponse
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00004500 File Offset: 0x00002700
		public SnapshotResponse(JObject jsonObj)
		{
			this.targetUrl = jsonObj["targetUrl"].ToString();
			this.persistentId = ((jsonObj.GetValue("persistentId") == null) ? null : jsonObj["persistentId"].ToString());
			this.mJsonString = jsonObj.ToString();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004560 File Offset: 0x00002760
		public virtual string TargetUrl
		{
			get
			{
				return this.targetUrl;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00004578 File Offset: 0x00002778
		public virtual string PersistentId
		{
			get
			{
				return this.persistentId;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004590 File Offset: 0x00002790
		public override string ToString()
		{
			return this.mJsonString;
		}

		// Token: 0x0400002C RID: 44
		private string targetUrl;

		// Token: 0x0400002D RID: 45
		private string persistentId;

		// Token: 0x0400002E RID: 46
		private string mJsonString;
	}
}
