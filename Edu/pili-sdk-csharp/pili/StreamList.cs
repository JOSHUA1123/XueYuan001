using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace pili_sdk.pili
{
	// Token: 0x02000013 RID: 19
	public class StreamList
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000045A8 File Offset: 0x000027A8
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000045B0 File Offset: 0x000027B0
		public string Marker { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000045B9 File Offset: 0x000027B9
		// (set) Token: 0x0600008E RID: 142 RVA: 0x000045C1 File Offset: 0x000027C1
		public bool End { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000045CA File Offset: 0x000027CA
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000045D2 File Offset: 0x000027D2
		public IList<Stream> Streams { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000045DB File Offset: 0x000027DB
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000045E3 File Offset: 0x000027E3
		public JObject JsonObject { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000045EC File Offset: 0x000027EC
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000045F4 File Offset: 0x000027F4
		public string JsonString { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000045FD File Offset: 0x000027FD
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004605 File Offset: 0x00002805
		public string XMLString { get; private set; }

		// Token: 0x06000097 RID: 151 RVA: 0x00004610 File Offset: 0x00002810
		public StreamList(JObject jsonObj, ICredentials auth)
		{
			this.Marker = jsonObj["marker"].ToString();
			this.End = Convert.ToBoolean(jsonObj["end"].ToString());
			try
			{
				JToken jtoken = jsonObj["items"];
				this.Streams = new List<Stream>();
				foreach (JToken jtoken2 in ((IEnumerable<JToken>)jtoken))
				{
					JObject jobject = (JObject)jtoken2;
					this.Streams.Add(new Stream(JObject.Parse(jobject.ToString()), auth));
				}
			}
			catch (InvalidCastException ex)
			{
				throw ex;
			}
		}
	}
}
