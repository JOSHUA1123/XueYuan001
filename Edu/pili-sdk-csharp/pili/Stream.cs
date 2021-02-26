using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace pili_sdk.pili
{
	// Token: 0x02000016 RID: 22
	public class Stream
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004925 File Offset: 0x00002B25
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x0000492D File Offset: 0x00002B2D
		public string StreamID { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004936 File Offset: 0x00002B36
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000493E File Offset: 0x00002B3E
		public string HubName { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004947 File Offset: 0x00002B47
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x0000494F File Offset: 0x00002B4F
		public string CreatedAt { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004958 File Offset: 0x00002B58
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00004960 File Offset: 0x00002B60
		public string UpdatedAt { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004969 File Offset: 0x00002B69
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00004971 File Offset: 0x00002B71
		public string Title { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000497A File Offset: 0x00002B7A
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004982 File Offset: 0x00002B82
		public string PublishKey { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000498B File Offset: 0x00002B8B
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00004993 File Offset: 0x00002B93
		public string PublishSecurity { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000499C File Offset: 0x00002B9C
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000049A4 File Offset: 0x00002BA4
		public bool Disabled { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000049AD File Offset: 0x00002BAD
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000049B5 File Offset: 0x00002BB5
		public string PlaybackHttpHost { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000049BE File Offset: 0x00002BBE
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000049C6 File Offset: 0x00002BC6
		public string PlayRtmpHost { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000049CF File Offset: 0x00002BCF
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000049D7 File Offset: 0x00002BD7
		public string LiveHdlHost { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000049E0 File Offset: 0x00002BE0
		// (set) Token: 0x060000CB RID: 203 RVA: 0x000049E8 File Offset: 0x00002BE8
		public string PlayHttpHost { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000049F1 File Offset: 0x00002BF1
		// (set) Token: 0x060000CD RID: 205 RVA: 0x000049F9 File Offset: 0x00002BF9
		public string LiveHttpHost { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004A02 File Offset: 0x00002C02
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00004A0A File Offset: 0x00002C0A
		public string LiveHlsHost { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004A13 File Offset: 0x00002C13
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004A1B File Offset: 0x00002C1B
		public string[] Profiles { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004A24 File Offset: 0x00002C24
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00004A2C File Offset: 0x00002C2C
		public string PublishRtmpHost { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004A35 File Offset: 0x00002C35
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00004A3D File Offset: 0x00002C3D
		public string LiveRtmpHost { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004A46 File Offset: 0x00002C46
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00004A4E File Offset: 0x00002C4E
		public JObject JsonObject { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004A57 File Offset: 0x00002C57
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00004A5F File Offset: 0x00002C5F
		public string JsonString { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004A68 File Offset: 0x00002C68
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00004A70 File Offset: 0x00002C70
		public string XMLString { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004A79 File Offset: 0x00002C79
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00004A81 File Offset: 0x00002C81
		public ICredentials Credentials { get; private set; }

		// Token: 0x060000DE RID: 222 RVA: 0x00004A8C File Offset: 0x00002C8C
		public Stream(JObject jsonObj)
		{
			this.StreamID = jsonObj["id"].ToString();
			this.HubName = jsonObj["hub"].ToString();
			this.CreatedAt = jsonObj["createdAt"].ToString();
			this.UpdatedAt = jsonObj["updatedAt"].ToString();
			this.Title = jsonObj["title"].ToString();
			this.PublishKey = jsonObj["publishKey"].ToString();
			this.PublishSecurity = jsonObj["publishSecurity"].ToString();
			this.Disabled = (bool)jsonObj["disabled"];
			bool flag = jsonObj["profiles"] != null;
			if (flag)
			{
				this.Profiles = JsonConvert.DeserializeAnonymousType<string[]>(jsonObj["profiles"].ToString(), this.Profiles);
			}
			bool flag2 = jsonObj["hosts"]["publish"] != null;
			if (flag2)
			{
				this.PublishRtmpHost = jsonObj["hosts"]["publish"]["rtmp"].ToString();
			}
			bool flag3 = jsonObj["hosts"]["live"] != null;
			if (flag3)
			{
				this.LiveRtmpHost = jsonObj["hosts"]["live"]["rtmp"].ToString();
				this.LiveHdlHost = jsonObj["hosts"]["live"]["hdl"].ToString();
				this.LiveHlsHost = jsonObj["hosts"]["live"]["hls"].ToString();
				this.LiveHttpHost = jsonObj["hosts"]["live"]["hls"].ToString();
			}
			bool flag4 = jsonObj["hosts"]["playback"] != null;
			if (flag4)
			{
				this.PlaybackHttpHost = jsonObj["hosts"]["playback"]["hls"].ToString();
			}
			bool flag5 = jsonObj["hosts"]["play"] != null;
			if (flag5)
			{
				this.PlayHttpHost = jsonObj["hosts"]["play"]["http"].ToString();
				this.PlayRtmpHost = jsonObj["hosts"]["play"]["rtmp"].ToString();
			}
			this.JsonObject = jsonObj;
			this.JsonString = jsonObj.ToString();
			this.XMLString = JsonConvert.DeserializeXNode(this.JsonString, "Stream", true).ToString();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004D96 File Offset: 0x00002F96
		public Stream(JObject jsonObject, ICredentials credentials) : this(jsonObject)
		{
			this.Credentials = credentials;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004DAC File Offset: 0x00002FAC
		public StreamStatus Status()
		{
			return Pili.API<IStream>().Status(this);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004DCC File Offset: 0x00002FCC
		public Stream Enable(Stream stream)
		{
			return Pili.API<IStream>().Enable(this);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004DEC File Offset: 0x00002FEC
		public Stream Disable(Stream stream)
		{
			return Pili.API<IStream>().Disable(this);
		}

		// Token: 0x04000042 RID: 66
		public const string ORIGIN = "ORIGIN";
	}
}
