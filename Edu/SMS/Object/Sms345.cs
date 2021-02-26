using System;
using System.IO;
using System.Net;
using System.Text;
using Common.Param.Method;

namespace SMS.Object
{
	// Token: 0x02000015 RID: 21
	public class Sms345 : ISMS
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000036FD File Offset: 0x000018FD
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003705 File Offset: 0x00001905
		public SmsItem Current
		{
			get
			{
				return this._current;
			}
			set
			{
				this._current = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000370E File Offset: 0x0000190E
		// (set) Token: 0x0600009E RID: 158 RVA: 0x0000371B File Offset: 0x0000191B
		public string User
		{
			get
			{
				return this._current.User;
			}
			set
			{
				this._current.User = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003729 File Offset: 0x00001929
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003736 File Offset: 0x00001936
		public string Password
		{
			get
			{
				return this._current.Password;
			}
			set
			{
				this._current.Password = value;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003744 File Offset: 0x00001944
		public SmsState Send(string mobiles, string context)
		{
			return this.Send(mobiles, context, DateTime.Now);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003754 File Offset: 0x00001954
		public SmsState Send(string mobiles, string context, DateTime time)
		{
			string user = this.Current.User;
			string text = this.Current.Password;
			text = new ConvertToAnyValue(text).MD5;
			string text2 = this.Current.Domain;
			string text3 = "uid={0}&pwd={1}&mobile={2}&content={3}&encode=utf8";
			context = new ConvertToAnyValue(context).UrlEncode;
			time.ToString("yyyy-MM-dd HH:mm");
			text3 = string.Format(text3, new object[]
			{
				user,
				text,
				mobiles,
				context
			});
			Encoding.UTF8.GetBytes(text3);
			string text4 = "";
			using (WebClient webClient = new WebClient())
			{
				text2 = text2 + "tx/?" + text3;
				using (Stream stream = webClient.OpenRead(text2))
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))
					{
						text4 = streamReader.ReadToEnd();
						streamReader.Close();
					}
					stream.Close();
				}
			}
			SmsState smsState = new SmsState();
			smsState.Success = (text4 == "100");
			smsState.Result = text4;
			string[] array = new string[]
			{
				"100|发送成功",
				"101|验证失败",
				"102|短信不足",
				"103|操作失败",
				"104|非法字符",
				"105|内容过多",
				"106|号码过多",
				"107|频率过快",
				"108|号码内容空",
				"109|账号冻结",
				"110|禁止频繁单条发送",
				"111|系统暂定发送",
				"112|号码不正确",
				"120|系统升级"
			};
			foreach (string text5 in array)
			{
				string text6 = text5.Substring(0, text5.IndexOf("|"));
				string description = text5.Substring(text5.IndexOf("|") + 1);
				if (text4 == text6)
				{
					smsState.Result = text6;
					smsState.Description = description;
				}
			}
			return smsState;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000039A0 File Offset: 0x00001BA0
		public int Query()
		{
			string user = this.Current.User;
			string password = this.Current.Password;
			return this.Query(user, password);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000039D0 File Offset: 0x00001BD0
		public int Query(string user, string pw)
		{
			pw = new ConvertToAnyValue(pw).MD5;
			string text = this.Current.Domain;
			string str = "uid={0}&pwd={1}";
			text = string.Format(text + "mm/?" + str, user, pw);
			string text2 = "";
			using (WebClient webClient = new WebClient())
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))
					{
						text2 = streamReader.ReadToEnd();
						streamReader.Close();
					}
					stream.Close();
				}
			}
			if (text2 == "")
			{
				throw new Exception("获取失败！");
			}
			int result = -1;
			if (text2.IndexOf("|") > -1)
			{
				string a = text2.Substring(0, text2.IndexOf("|"));
				if (a == "100")
				{
					string s = text2.Substring(text2.LastIndexOf("|") + 1);
					int.TryParse(s, out result);
				}
			}
			return result;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003B0C File Offset: 0x00001D0C
		public string ReceiveSMS(DateTime from, string readflag)
		{
			return string.Empty;
		}

		// Token: 0x04000027 RID: 39
		private SmsItem _current;
	}
}
