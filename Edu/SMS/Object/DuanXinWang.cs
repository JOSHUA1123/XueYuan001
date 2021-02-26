using System;
using System.IO;
using System.Net;
using System.Text;
using Common.Param.Method;

namespace SMS.Object
{
	/// <summary>
	/// 短信王的短信发送类
	/// </summary>
	// Token: 0x0200000E RID: 14
	public class DuanXinWang : ISMS
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002945 File Offset: 0x00000B45
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000294D File Offset: 0x00000B4D
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

		/// <summary>
		/// 用户的账号
		/// </summary>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002956 File Offset: 0x00000B56
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00002963 File Offset: 0x00000B63
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

		/// <summary>
		/// 用户的密码
		/// </summary>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002971 File Offset: 0x00000B71
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000297E File Offset: 0x00000B7E
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

		// Token: 0x06000055 RID: 85 RVA: 0x0000298C File Offset: 0x00000B8C
		public SmsState Send(string mobiles, string context)
		{
			return this.Send(mobiles, context, DateTime.Now);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000299C File Offset: 0x00000B9C
		public SmsState Send(string mobiles, string context, DateTime time)
		{
			string user = this.Current.User;
			string text = this.Current.Password;
			text = new ConvertToAnyValue(text).MD5;
			string str = "http://223.4.201.174/tx/";
			string text2 = "user={0}&pass={1}&mobile={2}&content={3}&time={4}&encode=utf8";
			context = new ConvertToAnyValue(context).UrlEncode;
			text2 = string.Format(text2, new object[]
			{
				user,
				text,
				mobiles,
				context,
				time.ToString()
			});
			Encoding.UTF8.GetBytes(text2);
			string a = "";
			using (WebClient webClient = new WebClient())
			{
				using (Stream stream = webClient.OpenRead(str + "?" + text2))
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))
					{
						a = streamReader.ReadToEnd();
						streamReader.Close();
					}
					stream.Close();
				}
			}
			SmsState smsState = new SmsState();
			smsState.Success = (a == "100");
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
			foreach (string text3 in array)
			{
				string text4 = text3.Substring(0, text3.IndexOf("|"));
				string description = text3.Substring(text3.IndexOf("|") + 1);
				if (a == text4)
				{
					smsState.Result = text4;
					smsState.Description = description;
				}
			}
			return smsState;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002BDC File Offset: 0x00000DDC
		public int Query()
		{
			string user = this.Current.User;
			string password = this.Current.Password;
			return this.Query(user, password);
		}

		/// <summary>
		/// 查询剩余的短信条数
		/// </summary>
		/// <param name="user">账号</param>
		/// <param name="pw">密码</param>
		/// <returns></returns>
		// Token: 0x06000058 RID: 88 RVA: 0x00002C0C File Offset: 0x00000E0C
		public int Query(string user, string pw)
		{
			pw = new ConvertToAnyValue(pw).MD5;
			string text = "http://223.4.201.174/mm/?user={0}&pass={1}";
			text = string.Format(text, user, pw);
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
			if (text2.IndexOf("|") > -1)
			{
				string a = text2.Substring(0, text2.IndexOf("|"));
				if (a == "100")
				{
					string value = text2.Substring(text2.IndexOf("-") + 1);
					try
					{
						return Convert.ToInt32(value);
					}
					catch
					{
						return -1;
					}
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D3C File Offset: 0x00000F3C
		public string ReceiveSMS(DateTime from, string readflag)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		// Token: 0x04000011 RID: 17
		private SmsItem _current;
	}
}
