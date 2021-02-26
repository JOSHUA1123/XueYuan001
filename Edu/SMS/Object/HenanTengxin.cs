using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using SMS.sms1086;

namespace SMS.Object
{
	/// <summary>
	/// 河南腾信的短信开发接口
	/// </summary>
	// Token: 0x02000014 RID: 20
	public class HenanTengxin : ISMS
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003284 File Offset: 0x00001484
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000328C File Offset: 0x0000148C
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
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003295 File Offset: 0x00001495
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000032A2 File Offset: 0x000014A2
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
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000032B0 File Offset: 0x000014B0
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000032BD File Offset: 0x000014BD
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

		// Token: 0x06000093 RID: 147 RVA: 0x000032CB File Offset: 0x000014CB
		public SmsState Send(string mobiles, string context)
		{
			return this.Send(mobiles, context, DateTime.Now);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000032DC File Offset: 0x000014DC
		public SmsState Send(string mobiles, string content, DateTime time)
		{
			string text = HttpUtility.UrlEncode(this.Current.User, Encoding.UTF8);
			content = HttpUtility.UrlEncode(content, Encoding.UTF8);
			string requestUriString = string.Format("{0}Api/sendutf8.aspx?username={1}&password={2}&mobiles={3}&content={4}", new object[]
			{
				this.Current.Domain,
				text,
				this.Current.Password,
				mobiles,
				content
			});
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.ContentType = "text/HTML";
			httpWebRequest.MaximumAutomaticRedirections = 4;
			httpWebRequest.MaximumResponseHeadersLength = 4;
			httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
			string url = streamReader.ReadToEnd();
			httpWebResponse.Close();
			streamReader.Dispose();
			string s = this._getPara(url, "result");
			string text2 = this._getPara(url, "description");
			text2 = HttpUtility.UrlDecode(text2);
			SmsState smsState = new SmsState();
			smsState.Code = int.Parse(s);
			smsState.Success = (smsState.Code == 0);
			switch (smsState.Code)
			{
			case 1:
				smsState.Result = "提交参数不能为空";
				break;
			case 2:
				smsState.Result = "用户名或密码错误";
				break;
			case 3:
				smsState.Result = "账号未启用";
				break;
			case 4:
				smsState.Result = "计费账号无效";
				break;
			case 5:
				smsState.Result = "定时时间无效";
				break;
			case 6:
				smsState.Result = "业务未开通";
				break;
			case 7:
				smsState.Result = "权限不足";
				break;
			case 8:
				smsState.Result = "余额不足";
				break;
			case 9:
				smsState.Result = "号码中含有无效号码";
				break;
			default:
				smsState.Result = text2;
				break;
			}
			smsState.Description = text2;
			smsState.FailList = this._getPara(url, "faillist");
			return smsState;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000034E8 File Offset: 0x000016E8
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
		// Token: 0x06000096 RID: 150 RVA: 0x00003518 File Offset: 0x00001718
		public int Query(string user, string pw)
		{
			string text = this.Current.Domain;
			string str = "username={0}&password={1}";
			text = string.Format(text + "Api/Query.aspx?" + str, user, pw);
			string url = "";
			using (WebClient webClient = new WebClient())
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("gb2312")))
					{
						url = streamReader.ReadToEnd();
						streamReader.Close();
					}
					stream.Close();
				}
			}
			string a = this._getPara(url, "result");
			string value = this._getPara(url, "balance");
			string text2 = this._getPara(url, "description");
			if (!string.IsNullOrWhiteSpace(text2))
			{
				text2 = HttpUtility.UrlDecode(text2, Encoding.GetEncoding("GB2312"));
			}
			if (a == "0")
			{
				return Convert.ToInt32(value);
			}
			throw new Exception(text2);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000363C File Offset: 0x0000183C
		private string _getPara(string url, string key)
		{
			string result = string.Empty;
			if (url.IndexOf("?") > -1)
			{
				url = url.Substring(url.LastIndexOf("?") + 1);
			}
			string[] array = url.Split(new char[]
			{
				'&'
			});
			foreach (string text in array)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string[] array3 = text.Split(new char[]
					{
						'='
					});
					if (array3.Length >= 2 && string.Equals(array3[0], key, StringComparison.CurrentCultureIgnoreCase))
					{
						result = array3[1];
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000036DD File Offset: 0x000018DD
		public string ReceiveSMS(DateTime from, string readflag)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		// Token: 0x04000025 RID: 37
		private static readonly WsAPIs ObjWsAPIs = new WsAPIs();

		// Token: 0x04000026 RID: 38
		private SmsItem _current;
	}
}
