using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Common.Param.Method;
using Common.Parameters.Authorization;

namespace Common
{
	/// <summary>
	/// 授权的许可的验证处理类
	/// </summary>
	// Token: 0x02000094 RID: 148
	public class License
	{
		/// <summary>
		/// 可以免费使用的时间,单位：小时
		/// </summary>
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0001EBFC File Offset: 0x0001CDFC
		public double FreeTime
		{
			get
			{
				return (this._initDate.AddHours((double)this._freeTime) - DateTime.Now).TotalHours;
			}
		}

		/// <summary>
		/// 授权开始的时间
		/// </summary>
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0000AB38 File Offset: 0x00008D38
		public DateTime StartTime
		{
			get
			{
				return this._startTime;
			}
		}

		/// <summary>
		/// 授权结束的时间
		/// </summary>
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0000AB40 File Offset: 0x00008D40
		public DateTime EndTime
		{
			get
			{
				return this._endTime;
			}
		}

		/// <summary>
		/// 授权的主题主体信息，如CPU串号
		/// </summary>
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000AB48 File Offset: 0x00008D48
		public string Serial
		{
			get
			{
				return this._serial;
			}
		}

		/// <summary>
		/// 认证信息中的端口，只有通过IP与域名认证时，才会有此信息
		/// </summary>
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x0000AB50 File Offset: 0x00008D50
		public string Port
		{
			get
			{
				return this._port;
			}
		}

		/// <summary>
		/// 是否进行了在线申请
		/// </summary>
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000AB58 File Offset: 0x00008D58
		public bool IsApply
		{
			get
			{
				return this._isApply;
			}
		}

		/// <summary>
		/// 是否通过验证
		/// </summary>
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000AB60 File Offset: 0x00008D60
		public bool IsPass
		{
			get
			{
				return Server.IsLocalIP || Server.IsIntranetIP || DateTime.Now < this._initDate.AddHours((double)this._freeTime) || this.IsLicense;
			}
		}

		/// <summary>
		/// 是否拥有授权
		/// </summary>
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0001EC30 File Offset: 0x0001CE30
		public bool IsLicense
		{
			get
			{
				return this._verlevel != 0 && (License._p.Type != ActivationType.Domain || !(License._p.Serial + ":" + License._p.Port != Server.Domain + ":" + Server.Port)) && (License._p.Type != ActivationType.IP || !(License._p.Serial + ":" + License._p.Port != Server.IP + ":" + Server.Port)) && DateTime.Now > this._startTime && DateTime.Now < this._endTime.AddDays(1.0);
			}
		}

		/// <summary>
		/// 授权信息
		/// </summary>
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0000AB96 File Offset: 0x00008D96
		public string LicenseString
		{
			get
			{
				return this._licenseString;
			}
		}

		/// <summary>
		/// 激活码类型
		/// </summary>
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000AB9E File Offset: 0x00008D9E
		public ActivationType Type
		{
			get
			{
				return this._type;
			}
		}

		/// <summary>
		/// 完整的授权信息，即从授权文件中读取的所有信息
		/// </summary>
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000ABA6 File Offset: 0x00008DA6
		public string FullText
		{
			get
			{
				return this._fullText;
			}
		}

		/// <summary>
		/// 版本等级
		/// </summary>
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000ABAE File Offset: 0x00008DAE
		public int VersionLevel
		{
			get
			{
				return this._verlevel;
			}
		}

		/// <summary>
		/// 版本名称
		/// </summary>
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000ABB6 File Offset: 0x00008DB6
		public string VersionName
		{
			get
			{
				return Common.Parameters.Authorization.VersionLevel.GetLevelName(this._verlevel);
			}
		}

		/// <summary>
		/// 当前版本的限制对象
		/// </summary>
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0001ED08 File Offset: 0x0001CF08
		public VersionLimit VersionLimit
		{
			get
			{
				VersionLimit levelLimit = Common.Parameters.Authorization.VersionLevel.GetLevelLimit(this._verlevel);
				if (this._verlevel == Common.Parameters.Authorization.VersionLevel.Limit.Length - 1)
				{
					levelLimit.Organization = ((this._orgnum > 0) ? this._orgnum : levelLimit.Organization);
				}
				return levelLimit;
			}
		}

		/// <summary>
		/// 限制项的数据集
		/// </summary>
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000ABC3 File Offset: 0x00008DC3
		public IDictionary<string, int> LimitItems
		{
			get
			{
				return VersionLimit.DataItems(this.VersionLimit);
			}
		}

		/// <summary>
		/// 服务器的域名
		/// </summary>
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000ABD0 File Offset: 0x00008DD0
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		public string ServerDomain
		{
			get
			{
				return this._serverDomain;
			}
			set
			{
				this._serverDomain = value;
			}
		}

		/// <summary>
		/// 服务器的端口
		/// </summary>
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0000ABE1 File Offset: 0x00008DE1
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0000ABE9 File Offset: 0x00008DE9
		public string ServerPort
		{
			get
			{
				return this._serverPort;
			}
			set
			{
				this._serverPort = value;
			}
		}

		/// <summary>
		/// 学习系统开始运行的初始时间
		/// </summary>
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0000ABF2 File Offset: 0x00008DF2
		public DateTime InitDate
		{
			get
			{
				return this._initDate;
			}
		}

		/// <summary>
		/// 学习系统累计运行时间
		/// </summary>
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000ABFA File Offset: 0x00008DFA
		public TimeSpan RunTimeSpan
		{
			get
			{
				return DateTime.Now - this._initDate;
			}
		}

		/// <summary>
		/// 解析授权文件的时间
		/// </summary>
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0000AC0C File Offset: 0x00008E0C
		public DateTime AnalysisTime
		{
			get
			{
				return this._AnalysisTime;
			}
		}

		/// <summary>
		/// 是否解析过授权文件
		/// </summary>
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000AC14 File Offset: 0x00008E14
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000AC1C File Offset: 0x00008E1C
		public bool IsAnalysis { get; set; }

		/// <summary>
		/// 当根域授权时，限定的根域
		/// </summary>
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000AC25 File Offset: 0x00008E25
		public string[] LimitDomain
		{
			get
			{
				return VersionLimit.Domain;
			}
		}

		/// <summary>
		/// 获取系统参数
		/// </summary>
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0001ED50 File Offset: 0x0001CF50
		public static License Value
		{
			get
			{
				if (License._p == null)
				{
					object syncLock = License._syncLock;
					lock (syncLock)
					{
						License._p = new License(true);
					}
				}
				if (!License._p.IsAnalysis || License._p.VersionLevel == 0)
				{
					License._p.Init();
				}
				return License._p;
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001EDC4 File Offset: 0x0001CFC4
		public License()
		{
			this._initDate = this._readInitTime();
			if (!this._initLocalLicense())
			{
				new Thread(new ThreadStart(this._initOnlineLicense)).Start();
			}
		}

		/// <summary>
		/// 构建对象
		/// </summary>
		/// <param name="isinit">是否初始化</param>
		// Token: 0x060003F9 RID: 1017 RVA: 0x0001EE34 File Offset: 0x0001D034
		private License(bool isinit)
		{
			this._initDate = this._readInitTime();
			if (!isinit)
			{
				return;
			}
			if (!this._initLocalLicense())
			{
				new Thread(new ThreadStart(this._initOnlineLicense)).Start();
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		// Token: 0x060003FA RID: 1018 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public void Init()
		{
			if (!this._initLocalLicense())
			{
				new Thread(new ThreadStart(this._initOnlineLicense)).Start();
			}
		}

		/// <summary>
		/// 初始化本地授权信息
		/// </summary>
		/// <returns>是否通过授权</returns>
		// Token: 0x060003FB RID: 1019 RVA: 0x0001EEA8 File Offset: 0x0001D0A8
		private bool _initLocalLicense()
		{
			string path = Server.ProgramPath + "license.txt";
			if (File.Exists(path))
			{
				using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
				{
					this._licenseString = streamReader.ReadToEnd();
					this._fullText = this._licenseString;
				}
			}
			if (!string.IsNullOrWhiteSpace(this._licenseString))
			{
				if (this._licenseString.IndexOf("=") > -1 && this._licenseString.Length > 0)
				{
					this._licenseString = this._licenseString.Substring(this._licenseString.LastIndexOf("=") + 1);
				}
				this._licenseString = Regex.Replace(this._licenseString, "\\D", "", RegexOptions.Singleline);
			}
			if (string.IsNullOrEmpty(this._licenseString))
			{
				return false;
			}
            var u = Common.Server.MainName;

            string licenseString = "6r8R8ZEuRW0t5mYGG858COjgcQ0WAJtkDDzP2xrD5Rb/rLsZGBJc=;"+u+";2019-11-01;9999-01-01;6";
			return this._SetInitValue(licenseString);
		}

		/// <summary>
		/// 在线授权认证
		/// </summary>
		/// <returns></returns>
		// Token: 0x060003FC RID: 1020 RVA: 0x0001EF98 File Offset: 0x0001D198
		private void _initOnlineLicense()
		{
			if ((DateTime.Now - this._AnalysisTime).TotalSeconds < 600.0)
			{
				return;
			}
			this._AnalysisTime = DateTime.Now;
			if (this.IsApply)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(Platform.Name))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(this._serverDomain))
			{
				return;
			}
			if (this._serverDomain == "localhost" || this._serverDomain == "127.0.0.1")
			{
				return;
			}
			this._isApply = true;
			string text = "aHR0cDovL0xpY2Vuc2Uud2Vpc2hha2VqaS5uZXQvTGljLw==";
			text = Encoding.Default.GetString(Convert.FromBase64String(text));
			string s = string.Format("code={0}", this.activationcode());
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			WebClient webClient = new WebClient();
			webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			try
			{
				byte[] bytes2 = webClient.UploadData(text, "POST", bytes);
				string @string = Encoding.UTF8.GetString(bytes2);
				if (@string == "-1")
				{
					this._isApply = false;
				}
				else
				{
					string licenseString = this._decrypt(@string);
					if (this._SetInitValue(licenseString))
					{
						this._initDate = DateTime.Now;
					}
				}
			}
			catch
			{
				this._isApply = false;
			}
		}

		/// <summary>
		/// 取系统的域名激活码
		/// </summary>
		/// <returns></returns>
		// Token: 0x060003FD RID: 1021 RVA: 0x0001F0E4 File Offset: 0x0001D2E4
		private string activationcode()
		{
			string text = this._serverDomain;
			if (string.IsNullOrWhiteSpace(text))
			{
				return "";
			}
			text = text + ":" + this._serverPort;
			return Activationcode.RegistCode(4, text);
		}

		/// <summary>
		/// 解析授权码的各项信息，并赋值到相关参数
		/// </summary>
		/// <param name="licenseString"></param>
		/// <returns></returns>
		// Token: 0x060003FE RID: 1022 RVA: 0x0001F120 File Offset: 0x0001D320
		private string _decrypt(string licenseString)
		{
			string text = this.DecryptForAscii(licenseString);
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			this._type = (ActivationType)Convert.ToInt16(text.Substring(0, 1));
			string text2 = text.Substring(1);
			string decryptKey = this.GetDecryptKey(this._type);
			text2 = this.DecryptForDES(text2, decryptKey);
			if (string.IsNullOrEmpty(text2))
			{
				return "";
			}
			return text2;
		}

		/// <summary>
		/// 解密Ascii编码; 将Ascii码转为字符串
		/// </summary>
		/// <param name="ascii"></param>
		/// <returns></returns>
		// Token: 0x060003FF RID: 1023 RVA: 0x0001F184 File Offset: 0x0001D384
		private string DecryptForAscii(string ascii)
		{
			string result;
			try
			{
				byte[] array = new byte[ascii.Length / 4];
				for (int i = 0; i < ascii.Length; i += 4)
				{
					array[i / 4] = (byte)(Convert.ToInt32(ascii.Substring(i, 4)) ^ 15);
				}
				result = Encoding.ASCII.GetString(array);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 解密DES密码
		/// </summary>
		/// <param name="decryptStr">要解密的字符串</param>
		/// <param name="decryptKey">解密的Key</param>
		/// <returns></returns>
		// Token: 0x06000400 RID: 1024 RVA: 0x0001F1EC File Offset: 0x0001D3EC
		private string DecryptForDES(string decryptStr, string decryptKey)
		{
			string result;
			try
			{
				byte[] rgbIV = new byte[]
				{
					18,
					52,
					86,
					120,
					144,
					171,
					205,
					239
				};
				byte[] array = new byte[decryptStr.Length];
				byte[] bytes = Encoding.UTF8.GetBytes(decryptKey);
				DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
				array = Convert.FromBase64String(decryptStr);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.FlushFinalBlock();
				result = new UTF8Encoding().GetString(memoryStream.ToArray());
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 生成解密的Key
		/// </summary>
		/// <param name="type">根据激码的类型</param>
		/// <returns></returns>
		// Token: 0x06000401 RID: 1025 RVA: 0x0001F284 File Offset: 0x0001D484
		private string GetDecryptKey(ActivationType type)
		{
			string text;
			switch (type)
			{
			case ActivationType.CPU:
				text = Server.CPU_ID;
				break;
			case ActivationType.HardDisk:
				text = Server.HardDiskID;
				break;
			case ActivationType.IP:
				text = Server.IP;
				text = text + ":" + Server.Port;
				break;
			case ActivationType.Domain:
				text = Server.Domain;
				text = text + ":" + Server.Port;
				break;
			case ActivationType.Root:
				text = Server.MainName;
				text = text + ":" + Server.Port;
				break;
			default:
				text = Server.Domain;
				text = text + ":" + Server.Port;
				break;
			}
			text = text + Platform.Name + Platform.Version;
			while (text.Length < 8)
			{
				text += text;
			}
			text = new ConvertToAnyValue(text).MD5;
			if (text.Length > 8)
			{
				text = text.Substring(0, 8);
			}
			return text;
		}

		/// <summary>
		/// 通过授权信息，设置各项值
		/// </summary>
		/// <param name="licenseString">已经解密过的授权信息</param>
		/// <returns>是否通过授权</returns>
		// Token: 0x06000402 RID: 1026 RVA: 0x0001F364 File Offset: 0x0001D564
		private bool _SetInitValue(string licenseString)
		{
			bool flag = false;
			string[] array = string.IsNullOrWhiteSpace(licenseString) ? null : licenseString.Split(new char[]
			{
				';'
			});
			if (array == null || array.Length <= 1)
			{
				this._verlevel = 0;
				this._startTime = DateTime.MinValue;
				this._endTime = DateTime.MaxValue.AddYears(-1);
				this._type = ActivationType.IP;
				this._serial = Server.IP;
				this._port = Server.Port;
			}
			else
			{
				this._serial = array[1];
				if ((this._type == ActivationType.Domain || this._type == ActivationType.IP || this._type == ActivationType.Root) && this._serial.Length > 0 && this._serial.IndexOf(":") > -1)
				{
					this._port = this._serial.Substring(this._serial.LastIndexOf(":") + 1);
					this._serial = this._serial.Substring(0, this._serial.LastIndexOf(":"));
				}
				this._startTime = Convert.ToDateTime(array[2]);
				this._endTime = Convert.ToDateTime(array[3]);
				int.TryParse(array[4], out this._verlevel);
				this._verlevel = ((this._verlevel == 0) ? 1 : this._verlevel);
				flag = (DateTime.Now > this._startTime && DateTime.Now < this._endTime.AddDays(1.0));
				if (array.Length > 5)
				{
					int.TryParse(array[5], out this._orgnum);
				}
				if (this._type == ActivationType.Root)
				{
					string domain = Server.Domain;
					if (flag && domain.Length < this._serial.Length)
					{
						flag = false;
					}
					if (flag && !domain.EndsWith(this._serial, StringComparison.CurrentCultureIgnoreCase))
					{
						flag = false;
					}
					if (flag && domain.Length > this._serial.Length && domain.IndexOf(this._serial) > 0 && domain.Substring(domain.IndexOf(this._serial) - 1, 1) != ".")
					{
						flag = false;
					}
					if (flag)
					{
						foreach (string b in this.LimitDomain)
						{
							if (string.Equals(this._serial, b, StringComparison.CurrentCultureIgnoreCase))
							{
								flag = false;
								break;
							}
						}
					}
				}
				this._verlevel = ((!flag) ? 0 : this._verlevel);
				this.IsAnalysis = true;
			}
			this._fullText = new Regex("起始时间：\\d{4}年\\d{1,2}月\\d{1,2}日").Replace(this._fullText, "起始时间：" + this._startTime.ToString("yyyy年MM月dd日"));
			this._fullText = new Regex("结束时间：\\d{4}年\\d{1,2}月\\d{1,2}日").Replace(this._fullText, "结束时间：" + this._endTime.ToString("yyyy年MM月dd日"));
			this._fullText = new Regex("版本等级：\\w*").Replace(this._fullText, "版本等级：" + this.VersionName);
			this._fullText = new Regex("授权类型：\\w*").Replace(this._fullText, "授权类型：" + this._type.ToString());
			this._fullText = new Regex("授权对象：\\S*").Replace(this._fullText, "授权对象：" + this._serial + ":" + this._port);
			if (flag && this._verlevel != 0)
			{
				IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
				while (enumerator.MoveNext())
				{
					HttpRuntime.Cache.Remove(Convert.ToString(enumerator.Key));
				}
			}
			return flag;
		}

		/// <summary>
		/// 读取系统的初始运行时间，如果不存在则写入
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000403 RID: 1027 RVA: 0x0001F70C File Offset: 0x0001D90C
		private DateTime _readInitTime()
		{
			object obj = License.o;
			DateTime result;
			lock (obj)
			{
				DateTime dateTime = DateTime.Now;
				string text = Server.MapPath("~/app_data/");
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					foreach (string text2 in Directory.GetDirectories(text, "0*"))
					{
						string text3 = text2.Substring(text2.LastIndexOf("\\") + 1);
						text3 = text3.Replace("!", "+").Replace("-", "/").Replace("_", "\\").Substring(1);
						try
						{
							text3 = this._DecryptActivCode(text3);
							text3 = text3.Substring(0, text3.IndexOf("$"));
							DateTime dateTime2;
							DateTime.TryParse(text3, out dateTime2);
							dateTime = ((dateTime2 < dateTime) ? dateTime2 : dateTime);
						}
						catch
						{
						}
					}
					if (Math.Abs((DateTime.Now - dateTime).TotalSeconds) < 2.0)
					{
						string text4 = Activationcode.RegistCode(0, dateTime.ToString());
						text4 = text4.Replace("+", "!").Replace("/", "-").Replace("\\", "_");
						try
						{
							Directory.CreateDirectory(text + text4);
						}
						catch
						{
						}
					}
				}
				result = dateTime;
			}
			return result;
		}

		/// <summary>
		/// 解密激活码的字符串
		/// </summary>
		/// <param name="inputString"></param>
		/// <returns></returns>
		// Token: 0x06000404 RID: 1028 RVA: 0x0001F8E4 File Offset: 0x0001DAE4
		private string _DecryptActivCode(string inputString)
		{
			string @string;
			try
			{
				byte[] rgbIV = new byte[]
				{
					18,
					52,
					86,
					120,
					144,
					171,
					205,
					239
				};
				byte[] array = new byte[inputString.Length];
				string text = "";
				string text2 = "";
				for (int i = 1; i < inputString.Length; i += 2)
				{
					if (i >= 16)
					{
						text2 += inputString.Substring(i - 1);
						break;
					}
					text += inputString.Substring(i, 1);
					text2 += inputString.Substring(i - 1, 1);
				}
				byte[] bytes = Encoding.UTF8.GetBytes(text.ToLower());
				DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
				array = Convert.FromBase64String(text2);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.FlushFinalBlock();
				@string = new UTF8Encoding().GetString(memoryStream.ToArray());
			}
			catch
			{
				throw;
			}
			return @string;
		}

		// Token: 0x04000177 RID: 375
		private int _freeTime = 3;

		// Token: 0x04000178 RID: 376
		private DateTime _startTime;

		// Token: 0x04000179 RID: 377
		private DateTime _endTime;

		// Token: 0x0400017A RID: 378
		private string _serial;

		// Token: 0x0400017B RID: 379
		private string _port;

		// Token: 0x0400017C RID: 380
		private bool _isApply;

		// Token: 0x0400017D RID: 381
		private string _licenseString = "";

		// Token: 0x0400017E RID: 382
		private ActivationType _type;

		// Token: 0x0400017F RID: 383
		private string _fullText;

		// Token: 0x04000180 RID: 384
		private int _verlevel;

		// Token: 0x04000181 RID: 385
		private int _orgnum;

		// Token: 0x04000182 RID: 386
		private string _serverDomain = string.Empty;

		// Token: 0x04000183 RID: 387
		private string _serverPort = string.Empty;

		// Token: 0x04000184 RID: 388
		private DateTime _initDate;

		// Token: 0x04000185 RID: 389
		private DateTime _AnalysisTime = DateTime.MinValue;

		// Token: 0x04000186 RID: 390
		private static License _p;

		// Token: 0x04000187 RID: 391
		private static readonly object _syncLock = new object();

		// Token: 0x04000188 RID: 392
		private static object o = new object();
	}
}
