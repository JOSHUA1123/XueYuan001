using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace Common.Param.Method
{
	/// <summary>
	/// 将某个值转换为任意数据类型
	/// </summary>
	// Token: 0x02000051 RID: 81
	public class ConvertToAnyValue
	{
		/// <summary>
		/// 参数的原始值
		/// </summary>
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00009E6A File Offset: 0x0000806A
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00009E77 File Offset: 0x00008077
		public string ParaValue
		{
			get
			{
				return this._paravlue.ToString();
			}
			set
			{
				this._paravlue = value;
			}
		}

		/// <summary>
		/// 参数的键名
		/// </summary>
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00009E80 File Offset: 0x00008080
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00009E88 File Offset: 0x00008088
		public string ParaKey
		{
			get
			{
				return this._parakey;
			}
			set
			{
				this._parakey = value;
			}
		}

		/// <summary>
		/// 参数的单位
		/// </summary>
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00009E91 File Offset: 0x00008091
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00009E99 File Offset: 0x00008099
		public string Unit
		{
			get
			{
				return this._unit;
			}
			set
			{
				this._unit = value;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009EA2 File Offset: 0x000080A2
		public ConvertToAnyValue()
		{
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009EC0 File Offset: 0x000080C0
		public ConvertToAnyValue(object para)
		{
			this._paravlue = para;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009EE5 File Offset: 0x000080E5
		public ConvertToAnyValue(string para)
		{
			this._paravlue = (string.IsNullOrWhiteSpace(para) ? "" : para);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00009EC0 File Offset: 0x000080C0
		public ConvertToAnyValue(string para, string unit)
		{
			this._paravlue = para;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009F19 File Offset: 0x00008119
		public static ConvertToAnyValue Get(string para)
		{
			return new ConvertToAnyValue(para);
		}

		/// <summary>
		/// 参数的Int16类型值，如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00016928 File Offset: 0x00014B28
		public int? Int16
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				int? result;
				try
				{
					result = new int?((int)Convert.ToInt16(this._paravlue));
				}
				catch
				{
					Regex regex = new Regex("\\d+");
					Match match = regex.Match(this.String);
					if (match.Success)
					{
						string value = match.Groups[0].Value;
						result = new int?((int)Convert.ToInt16(value));
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		/// <summary>
		/// 参数的Int32类型值，如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000236 RID: 566 RVA: 0x000169BC File Offset: 0x00014BBC
		public int? Int32
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				int? result;
				try
				{
					result = new int?(Convert.ToInt32(this._paravlue));
				}
				catch
				{
					Regex regex = new Regex("\\d+");
					Match match = regex.Match(this.String);
					if (match.Success)
					{
						string value = match.Groups[0].Value;
						result = new int?(Convert.ToInt32(value));
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		/// <summary>
		/// 参数的Int64类型值，如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00016A50 File Offset: 0x00014C50
		public long? Int64
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				long? result;
				try
				{
					result = new long?(Convert.ToInt64(this._paravlue));
				}
				catch
				{
					Regex regex = new Regex("\\d+");
					Match match = regex.Match(this.String);
					if (match.Success)
					{
						string value = match.Groups[0].Value;
						result = new long?(Convert.ToInt64(value));
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		/// <summary>
		/// 参数的Double类型值，如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00016AE4 File Offset: 0x00014CE4
		public double? Double
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				double? result;
				try
				{
					result = new double?(Convert.ToDouble(this._paravlue));
				}
				catch
				{
					result = null;
				}
				return result;
			}
		}

		/// <summary>
		/// 参数的String类型值，如果参数不存在或异常，则返回空字符串，非Null;
		/// </summary>
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00009F21 File Offset: 0x00008121
		public string String
		{
			get
			{
				if (this._paravlue != null)
				{
					return this._paravlue.ToString().Trim();
				}
				return "";
			}
		}

		/// <summary>
		/// 参数文本类型值，自动去除html标签
		/// </summary>
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00016B34 File Offset: 0x00014D34
		public string Text
		{
			get
			{
				string @string = this.String;
				if (string.IsNullOrWhiteSpace(@string))
				{
					return @string;
				}
				string input = Regex.Replace(@string, "<[^>]+>", "");
				input = Regex.Replace(input, "&[^;]+;", "");
				return @string;
			}
		}

		/// <summary>
		/// 参数的值,如果没有内容，返回Null
		/// </summary>
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00016B78 File Offset: 0x00014D78
		public string Value
		{
			get
			{
				string @string = this.String;
				if (string.IsNullOrWhiteSpace(@string))
				{
					return null;
				}
				return @string;
			}
		}

		/// <summary>
		/// 参数的Boolean类型值，如果参数不存在或异常，则返回true;
		/// </summary>
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00016B98 File Offset: 0x00014D98
		public bool? Boolean
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				bool? result;
				try
				{
					result = new bool?(Convert.ToBoolean(this._paravlue));
				}
				catch
				{
					result = null;
				}
				return result;
			}
		}

		/// <summary>
		/// 参数的DateTime类型值，如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00016BE8 File Offset: 0x00014DE8
		public DateTime? DateTime
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				DateTime? result;
				try
				{
					if (this._paravlue is long)
					{
						long num = 0L;
						long.TryParse(this._paravlue.ToString(), out num);
						DateTime value = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds((double)num);
						result = new DateTime?(value);
					}
					else
					{
						result = new DateTime?(Convert.ToDateTime(this._paravlue));
					}
				}
				catch
				{
					string text = this._paravlue.ToString();
					foreach (char c in text)
					{
						text += ((c >= '0' && c <= '9') ? c : '-');
					}
					string text3 = text;
					Regex regex = new Regex("\\-{1,}", RegexOptions.IgnorePatternWhitespace);
					text3 = regex.Replace(text3, "-");
					regex = new Regex("^\\-{1,}(\\w)", RegexOptions.IgnorePatternWhitespace);
					text3 = regex.Replace(text3, "$1");
					regex = new Regex("(\\w)\\-{1,}$", RegexOptions.IgnorePatternWhitespace);
					text3 = regex.Replace(text3, "$1");
					if (text3.IndexOf('-') > -1)
					{
						string text4 = text3.Substring(0, text3.IndexOf('-'));
						text4 = ((text4.Length == 1) ? ("0" + text4) : text4);
						text4 = ((text4.Length == 2) ? ("19" + text4) : text4);
						text4 = ((text4.Length > 4) ? text4.Substring(0, 4) : text4);
						text3 = text4 + "-" + text3.Substring(text3.IndexOf('-') + 1);
					}
					else
					{
						text3 += "-1-1";
					}
					try
					{
						result = new DateTime?(Convert.ToDateTime(text3));
					}
					catch
					{
						result = null;
					}
				}
				return result;
			}
		}

		/// <summary>
		/// 参数的MD5加密值(小写)，如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00016E14 File Offset: 0x00015014
		public string MD5
		{
			get
			{
				if (this._paravlue == null)
				{
					return "";
				}
				if (string.IsNullOrWhiteSpace(this._paravlue.ToString()))
				{
					return "";
				}
				return FormsAuthentication.HashPasswordForStoringInConfigFile(this._paravlue.ToString(), "MD5").ToLower();
			}
		}

		/// <summary>
		/// 参数的SHA1加密值，如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00009F41 File Offset: 0x00008141
		public string SHA1
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				return FormsAuthentication.HashPasswordForStoringInConfigFile(this._paravlue.ToString(), "SHA1");
			}
		}

		/// <summary>
		/// 参数的字符串进行 URL 解码并返回已解码的字符串。如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00009F62 File Offset: 0x00008162
		public string UrlDecode
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				return HttpUtility.UrlDecode(this._paravlue.ToString().Trim());
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00009F83 File Offset: 0x00008183
		public string UrlEncode
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				return HttpUtility.UrlEncode(this._paravlue.ToString().Trim());
			}
		}

		/// <summary>
		/// 对经过HTML 编码的参数进行解码，并返回已解码的字符串。如果参数不存在或异常，则返回null;
		/// </summary>
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00016E64 File Offset: 0x00015064
		public string HtmlDecode
		{
			get
			{
				if (this._paravlue == null)
				{
					return null;
				}
				HttpContext httpContext = HttpContext.Current;
				return httpContext.Server.HtmlDecode(this._paravlue.ToString());
			}
		}

		/// <summary>
		/// 转换为物理路径
		/// </summary>
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00009FA4 File Offset: 0x000081A4
		public string MapPath
		{
			get
			{
				return Server.MapPath(this._paravlue.ToString());
			}
		}

		/// <summary>
		/// 转换虚拟路径
		/// </summary>
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00009FB6 File Offset: 0x000081B6
		public string VirtualPath
		{
			get
			{
				return Server.VirtualPath(this._paravlue.ToString());
			}
		}

		/// <summary>
		/// 将字符串分拆成数组
		/// </summary>
		/// <param name="split">用于分拆的字符</param>
		/// <returns></returns>
		// Token: 0x06000245 RID: 581 RVA: 0x00016E98 File Offset: 0x00015098
		public string[] Split(char split)
		{
			return this._paravlue.ToString().Split(new char[]
			{
				split
			});
		}

		/// <summary>
		/// 将C#时间转换成Javascript的时间
		/// </summary>
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00016EC4 File Offset: 0x000150C4
		[Obsolete]
		public string JavascriptTime
		{
			get
			{
				if (this._paravlue == null)
				{
					return "";
				}
				string result;
				try
				{
					DateTime dateTime = this.DateTime ?? TimeZone.CurrentTimeZone.ToLocalTime(System.DateTime.Now);
					string format = "ddd MMM d HH:mm:ss 'UTC'zz'00' yyyy";
					CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");
					string text = dateTime.ToString(format, provider);
					result = text;
				}
				catch
				{
					result = "";
				}
				return result;
			}
		}

		/// <summary>
		/// JavaScript时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数。
		/// </summary>
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00016F48 File Offset: 0x00015148
		public long TimeStamp
		{
			get
			{
				DateTime d = this.DateTime ?? System.DateTime.Now;
				DateTime d2 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
				return (long)(d - d2).TotalMilliseconds;
			}
		}

		/// <summary>
		/// 转为指定的数据库类型
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		// Token: 0x06000248 RID: 584 RVA: 0x00016FA0 File Offset: 0x000151A0
		public object ChangeType(Type type)
		{
			string fullName;
			object result;
			if ((fullName = type.FullName) != null && fullName == "System.DateTime")
			{
				result = this.DateTime;
			}
			else
			{
				result = Convert.ChangeType(this._paravlue, type);
			}
			return result;
		}

		/// <summary>
		/// 解密字符串,默认密钥为当前域名
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000249 RID: 585 RVA: 0x00016FE4 File Offset: 0x000151E4
		public ConvertToAnyValue Decrypt()
		{
			string para = string.Empty;
			para = DataConvert.DecryptForBase64(this.UrlDecode);
			return new ConvertToAnyValue(para);
		}

		// Token: 0x040000C1 RID: 193
		private object _paravlue;

		// Token: 0x040000C2 RID: 194
		private string _parakey = "";

		// Token: 0x040000C3 RID: 195
		private string _unit = "";
	}
}
