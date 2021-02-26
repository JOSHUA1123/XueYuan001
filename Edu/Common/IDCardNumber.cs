using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Xml;

namespace Common
{
	/// <summary>
	/// 身份证号码解析与生成
	/// </summary>
	// Token: 0x020000B4 RID: 180
	public class IDCardNumber
	{
		/// <summary>
		/// 所在省份信息
		/// </summary>
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0000B254 File Offset: 0x00009454
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x0000B25C File Offset: 0x0000945C
		public string Province
		{
			get
			{
				return this._province;
			}
			set
			{
				this._province = value;
			}
		}

		/// <summary>
		/// 所在地区信息
		/// </summary>
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0000B265 File Offset: 0x00009465
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0000B26D File Offset: 0x0000946D
		public string Area
		{
			get
			{
				return this._area;
			}
			set
			{
				this._area = value;
			}
		}

		/// <summary>
		/// 所在区县信息
		/// </summary>
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0000B276 File Offset: 0x00009476
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0000B27E File Offset: 0x0000947E
		public string City
		{
			get
			{
				return this._city;
			}
			set
			{
				this._city = value;
			}
		}

		/// <summary>
		/// 出生日期
		/// </summary>
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0000B287 File Offset: 0x00009487
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x0000B28F File Offset: 0x0000948F
		public DateTime Birthday
		{
			get
			{
				return this._birthday;
			}
			set
			{
				this._birthday = value;
			}
		}

		/// <summary>
		/// 年龄
		/// </summary>
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0002336C File Offset: 0x0002156C
		public int Age
		{
			get
			{
				return DateTime.Now.Year - this.Birthday.Year;
			}
		}

		/// <summary>
		/// 性别，1为男，2为女
		/// </summary>
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0000B298 File Offset: 0x00009498
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x0000B2A0 File Offset: 0x000094A0
		public short Sex
		{
			get
			{
				return this._sex;
			}
			set
			{
				this._sex = value;
			}
		}

		/// <summary>
		/// 身份证号码
		/// </summary>
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0000B2A9 File Offset: 0x000094A9
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x0000B2B1 File Offset: 0x000094B1
		public string CardNumber
		{
			get
			{
				return this._cardnumber;
			}
			set
			{
				this._cardnumber = value;
			}
		}

		/// <summary>
		/// 生成Javascript对象；
		/// </summary>
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0000B2BA File Offset: 0x000094BA
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x0000B2C2 File Offset: 0x000094C2
		public string Json
		{
			get
			{
				return this._json;
			}
			set
			{
				this._json = value;
			}
		}

		/// <summary>
		/// 获取区域信息
		/// </summary>
		// Token: 0x060004CE RID: 1230 RVA: 0x00023398 File Offset: 0x00021598
		private static void FillAreas()
		{
			XmlDocument xmlDocument = new XmlDocument();
			string filename = HostingEnvironment.MapPath("~/App_Data/AreaCodeInfo.xml");
			xmlDocument.Load(filename);
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("area");
			foreach (object obj in elementsByTagName)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string value = xmlNode.Attributes["code"].Value;
				string value2 = xmlNode.Attributes["name"].Value;
				IDCardNumber.Areas.Add(new string[]
				{
					value,
					value2
				});
			}
		}

		/// <summary>
		/// 解析身份证信息
		/// </summary>
		/// <param name="idCardNumber"></param>
		// Token: 0x060004CF RID: 1231 RVA: 0x00023460 File Offset: 0x00021660
		public static IDCardNumber Get(string idCardNumber)
		{
			if (IDCardNumber.Areas.Count < 1)
			{
				IDCardNumber.FillAreas();
			}
			if (!IDCardNumber.CheckIDCardNumber(idCardNumber))
			{
				throw new Exception("非法的身份证号码");
			}
			return new IDCardNumber(idCardNumber);
		}

		/// <summary>
		/// 校验身份证号码是否合法
		/// </summary>
		/// <param name="idCardNumber"></param>
		/// <returns></returns>
		// Token: 0x060004D0 RID: 1232 RVA: 0x0002349C File Offset: 0x0002169C
		public static bool CheckIDCardNumber(string idCardNumber)
		{
			if (idCardNumber == null)
			{
				return false;
			}
			Regex regex = new Regex("^\\d{17}(\\d|X)$");
			Match match = regex.Match(idCardNumber);
			if (!match.Success)
			{
				return false;
			}
			string b = idCardNumber.Substring(17, 1);
			double num = 0.0;
			for (int i = 2; i <= 18; i++)
			{
				num += (double)int.Parse(idCardNumber[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2.0, (double)(i - 1)) % 11.0);
			}
			string[] array = new string[]
			{
				"1",
				"0",
				"X",
				"9",
				"8",
				"7",
				"6",
				"5",
				"4",
				"3",
				"2"
			};
			string a = array[(int)num % 11];
			return !(a != b);
		}

		/// <summary>
		/// 随机生成一个身份证号
		/// </summary>
		/// <returns></returns>
		// Token: 0x060004D1 RID: 1233 RVA: 0x000235BC File Offset: 0x000217BC
		public static IDCardNumber Radom()
		{
			long ticks = DateTime.Now.Ticks;
			return new IDCardNumber(IDCardNumber._radomCardNumber((int)ticks));
		}

		/// <summary>
		/// 批量生成身份证
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		// Token: 0x060004D2 RID: 1234 RVA: 0x000235E4 File Offset: 0x000217E4
		public static List<IDCardNumber> Radom(int count)
		{
			List<IDCardNumber> list = new List<IDCardNumber>();
			for (int i = 0; i < count; i++)
			{
				bool flag;
				string text;
				do
				{
					flag = false;
					int num = (int)DateTime.Now.Ticks;
					text = IDCardNumber._radomCardNumber(num * (i + 1));
					foreach (IDCardNumber idcardNumber in list)
					{
						if (idcardNumber.CardNumber == text)
						{
							flag = true;
							break;
						}
					}
				}
				while (flag);
				list.Add(new IDCardNumber(text));
			}
			return list;
		}

		/// <summary>
		/// 生成随身份证号
		/// </summary>
		/// <param name="seed">随机数种子</param>
		/// <returns></returns>
		// Token: 0x060004D3 RID: 1235 RVA: 0x00023684 File Offset: 0x00021884
		private static string _radomCardNumber(int seed)
		{
			if (IDCardNumber.Areas.Count < 1)
			{
				IDCardNumber.FillAreas();
			}
			Random random = new Random(seed);
			string text;
			do
			{
				text = IDCardNumber.Areas[random.Next(0, IDCardNumber.Areas.Count - 1)][0];
			}
			while (text.Substring(4, 2) == "00");
			DateTime dateTime = DateTime.Now.AddYears(-random.Next(16, 60)).AddMonths(-random.Next(0, 12)).AddDays((double)(-(double)random.Next(0, 31)));
			string str = random.Next(1000, 9999).ToString("####");
			string text2 = text + dateTime.ToString("yyyyMMdd") + str;
			double num = 0.0;
			for (int i = 2; i <= 18; i++)
			{
				num += (double)int.Parse(text2[18 - i].ToString(), NumberStyles.HexNumber) * (Math.Pow(2.0, (double)(i - 1)) % 11.0);
			}
			string[] array = new string[]
			{
				"1",
				"0",
				"X",
				"9",
				"8",
				"7",
				"6",
				"5",
				"4",
				"3",
				"2"
			};
			string str2 = array[(int)num % 11];
			return text2.Substring(0, 17) + str2;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000B2CB File Offset: 0x000094CB
		private IDCardNumber(string idCardNumber)
		{
			this._cardnumber = idCardNumber;
			this._analysis();
		}

		/// <summary>
		/// 解析身份证
		/// </summary>
		// Token: 0x060004D5 RID: 1237 RVA: 0x0002384C File Offset: 0x00021A4C
		private void _analysis()
		{
			string a = this._cardnumber.Substring(0, 2).PadRight(6, '0');
			string a2 = this._cardnumber.Substring(0, 4).PadRight(6, '0');
			string a3 = this._cardnumber.Substring(0, 6).PadRight(6, '0');
			for (int i = 0; i < IDCardNumber.Areas.Count; i++)
			{
				if (a == IDCardNumber.Areas[i][0])
				{
					this._province = IDCardNumber.Areas[i][1];
				}
				if (a2 == IDCardNumber.Areas[i][0])
				{
					this._area = IDCardNumber.Areas[i][1];
				}
				if (a3 == IDCardNumber.Areas[i][0])
				{
					this._city = IDCardNumber.Areas[i][1];
				}
				if (this._province != null && this._area != null && this._city != null)
				{
					break;
				}
			}
			string text = this._cardnumber.Substring(6, 8);
			try
			{
				int year = (int)Convert.ToInt16(text.Substring(0, 4));
				int month = (int)Convert.ToInt16(text.Substring(4, 2));
				int day = (int)Convert.ToInt16(text.Substring(6, 2));
				this._birthday = new DateTime(year, month, day);
			}
			catch
			{
				throw new Exception("非法的出生日期");
			}
			string value = this._cardnumber.Substring(14, 3);
			this._sex = (short)((Convert.ToInt16(value) % 2 == 0) ? 2 : 1);
			this._json = "prov:'{0}',area:'{1}',city:'{2}',year:{3},month:{4},day:{5},sex:{6},number:'{7}'";
			this._json = string.Format(this._json, new object[]
			{
				this._province,
				this._area,
				this._city,
				this._birthday.Year,
				this._birthday.Month,
				this._birthday.Day,
				this._sex,
				this._cardnumber
			});
			this._json = "{" + this._json + "}";
		}

		// Token: 0x040001EE RID: 494
		private string _province;

		// Token: 0x040001EF RID: 495
		private string _area;

		// Token: 0x040001F0 RID: 496
		private string _city;

		// Token: 0x040001F1 RID: 497
		private DateTime _birthday;

		// Token: 0x040001F2 RID: 498
		private short _sex;

		// Token: 0x040001F3 RID: 499
		private string _cardnumber;

		// Token: 0x040001F4 RID: 500
		private string _json;

		// Token: 0x040001F5 RID: 501
		private static readonly List<string[]> Areas = new List<string[]>();
	}
}
