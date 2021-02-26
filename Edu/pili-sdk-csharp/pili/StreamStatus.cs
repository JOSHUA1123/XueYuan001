using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace pili_sdk.pili
{
	// Token: 0x02000014 RID: 20
	public class StreamStatus
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000046E0 File Offset: 0x000028E0
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000046E8 File Offset: 0x000028E8
		public string Addr { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000046F1 File Offset: 0x000028F1
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000046F9 File Offset: 0x000028F9
		public string Status { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004702 File Offset: 0x00002902
		// (set) Token: 0x0600009D RID: 157 RVA: 0x0000470A File Offset: 0x0000290A
		public string StartFrom { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004713 File Offset: 0x00002913
		// (set) Token: 0x0600009F RID: 159 RVA: 0x0000471B File Offset: 0x0000291B
		public float BytesPerSecond { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004724 File Offset: 0x00002924
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x0000472C File Offset: 0x0000292C
		public FramesPerSecond FramesPerSecond { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004735 File Offset: 0x00002935
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x0000473D File Offset: 0x0000293D
		public string StreamID { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004746 File Offset: 0x00002946
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000474E File Offset: 0x0000294E
		public Stream Stream { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004757 File Offset: 0x00002957
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x0000475F File Offset: 0x0000295F
		public JObject JsonObject { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004768 File Offset: 0x00002968
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00004770 File Offset: 0x00002970
		public string JsonString { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004779 File Offset: 0x00002979
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00004781 File Offset: 0x00002981
		public string XMLString { get; private set; }

		// Token: 0x060000AC RID: 172 RVA: 0x0000478C File Offset: 0x0000298C
		public StreamStatus(JObject jsonObj, Stream stream)
		{
			this.Stream = stream;
			this.StreamID = stream.StreamID;
			this.Addr = jsonObj["addr"].ToString();
			this.Status = jsonObj["status"].ToString();
			this.StartFrom = ((DateTime)jsonObj["startFrom"]).ToString("yyyy-MM-ddTHH:mm:ssZ");
			try
			{
				this.BytesPerSecond = (float)jsonObj["bytesPerSecond"];
				float audio = (float)jsonObj["framesPerSecond"]["audio"];
				float video = (float)jsonObj["framesPerSecond"]["video"];
				float data = (float)jsonObj["framesPerSecond"]["data"];
				this.FramesPerSecond = new FramesPerSecond(audio, video, data);
			}
			catch (NullReferenceException ex)
			{
				throw ex;
			}
			this.JsonObject = jsonObj;
			this.JsonString = jsonObj.ToString();
			this.XMLString = JsonConvert.DeserializeXNode(this.JsonString, "Status", true).ToString();
		}
	}
}
