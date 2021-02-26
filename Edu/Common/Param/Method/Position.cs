using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Common.Param.Method
{
	/// <summary>
	/// 获取访问者的地理位置，可以通过GPS与IP
	/// </summary>
	// Token: 0x020000AC RID: 172
	public class Position : IDisposable
	{
		/// <summary>
		/// 当前访问者的客户端IP地址
		/// </summary>
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0000B009 File Offset: 0x00009209
		public string IPAddress
		{
			get
			{
				return this._ip;
			}
		}

		/// <summary>
		/// 当前访问者所在的纬度
		/// </summary>
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0000B011 File Offset: 0x00009211
		public string Latitude
		{
			get
			{
				return this._latitude;
			}
		}

		/// <summary>
		/// 当前访问者所在的经度
		/// </summary>
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000B019 File Offset: 0x00009219
		public string Longitude
		{
			get
			{
				return this._longitude;
			}
		}

		/// <summary>
		/// 当前访问者所在的省份信息
		/// </summary>
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0000B021 File Offset: 0x00009221
		public string Province
		{
			get
			{
				return this._province;
			}
		}

		/// <summary>
		/// 当前访问者所在的城市信息
		/// </summary>
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0000B029 File Offset: 0x00009229
		public string City
		{
			get
			{
				return this._city;
			}
		}

		/// <summary>
		/// 当前访问者所在的区县信息
		/// </summary>
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0000B031 File Offset: 0x00009231
		public string District
		{
			get
			{
				return this._district;
			}
		}

		/// <summary>
		/// 当前访问者所在的街道
		/// </summary>
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x0000B039 File Offset: 0x00009239
		public string Street
		{
			get
			{
				return this._street;
			}
		}

		/// <summary>
		/// 当前访问者所在的门牌号
		/// </summary>
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0000B041 File Offset: 0x00009241
		public string StreetNumber
		{
			get
			{
				return this._streetNumber;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0000B049 File Offset: 0x00009249
		public string Address
		{
			get
			{
				return this._address;
			}
		}

		/// <summary>
		/// 通过IP获取地理位置的构造
		/// </summary>
		/// <param name="ip"></param>
		// Token: 0x0600048A RID: 1162 RVA: 0x0000B051 File Offset: 0x00009251
		public Position(string ip)
		{
			this._ip = ip;
			this._getPosiForIP(this._ip);
		}

		/// <summary>
		/// 通过GPS信息获取地理位置
		/// </summary>
		/// <param name="lng">百度经度坐标</param>
		/// <param name="lat">百度纬度坐标</param>
		// Token: 0x0600048B RID: 1163 RVA: 0x00022614 File Offset: 0x00020814
		public Position(string lng, string lat)
		{
			this._longitude = lng;
			this._latitude = lat;
			this._ip = Browser.IP;
			this._getPosiForGPS(lng, lat);
		}

		/// <summary>
		/// 通过地址，获取地理位置信息
		/// </summary>
		/// <param name="address"></param>
		/// <param name="type">结果类型,1为xml,2为json</param>
		// Token: 0x0600048C RID: 1164 RVA: 0x00022664 File Offset: 0x00020864
		public Position(string address, int type)
		{
			try
			{
				this._getPosiForAddress(address, type);
				this._getPosiForGPS(this._longitude, this._latitude);
			}
			catch
			{
			}
			this._address = address;
		}

		/// <summary>
		/// 通过IP获取地理位置的方法
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		// Token: 0x0600048D RID: 1165 RVA: 0x000226C8 File Offset: 0x000208C8
		private string _getPosiForIP(string ip)
		{
			string text = "http://api.map.baidu.com/location/ip?ak={0}&ip={1}&coor=bd09ll";
			text = string.Format(text, this.ak, ip);
			WebClient webClient = new WebClient();
			webClient.BaseAddress = text;
			string text2 = "";
			try
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					StreamReader streamReader = new StreamReader(stream, Encoding.Default);
					text2 = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			catch
			{
			}
			text2 = this._unicodeToGB(text2);
			this._province = this._getJson(text2, "province");
			this._city = this._getJson(text2, "city");
			this._district = this._getJson(text2, "district");
			this._street = this._getJson(text2, "street");
			this._streetNumber = this._getJson(text2, "street_number");
			this._latitude = this._getJson(text2, "y");
			this._longitude = this._getJson(text2, "x");
			this._address = this._getJson(text2, "address");
			this._address = this._clearNoChinise(this._address);
			return text2;
		}

		/// <summary>
		/// 去除非汉字
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		// Token: 0x0600048E RID: 1166 RVA: 0x000227F8 File Offset: 0x000209F8
		private string _clearNoChinise(string text)
		{
			char[] array = text.ToCharArray();
			string text2 = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] >= '一' && array[i] <= '龻')
				{
					text2 += array[i].ToString();
				}
			}
			return text2;
		}

		/// <summary>
		/// Unicode转GB字符串
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		// Token: 0x0600048F RID: 1167 RVA: 0x00022848 File Offset: 0x00020A48
		private string _unicodeToGB(string text)
		{
			MatchCollection matchCollection = Regex.Matches(text, "\\\\u([\\w]{4})");
			if (matchCollection != null && matchCollection.Count > 0)
			{
				foreach (object obj in matchCollection)
				{
					Match match = (Match)obj;
					string value = match.Value;
					string text2 = value.Substring(2);
					byte[] array = new byte[2];
					int num = Convert.ToInt32(text2.Substring(0, 2), 16);
					int num2 = Convert.ToInt32(text2.Substring(2), 16);
					array[0] = (byte)num2;
					array[1] = (byte)num;
					text = text.Replace(value, Encoding.Unicode.GetString(array));
				}
			}
			return text;
		}

		/// <summary>
		/// 解析json信息
		/// </summary>
		/// <param name="result"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		// Token: 0x06000490 RID: 1168 RVA: 0x00022918 File Offset: 0x00020B18
		private string _getJson(string result, string key)
		{
			string text = "\"{0}\":\"(?<value>[^\"]+)\"";
			text = string.Format(text, key);
			Regex regex = new Regex(text, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
			MatchCollection matchCollection = regex.Matches(result);
			string result2 = "";
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				result2 = match.Groups["value"].Value.Trim();
			}
			return result2;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000229B0 File Offset: 0x00020BB0
		private void _getPosiForGPS(string lng, string lat)
		{
			string text = "http://api.map.baidu.com/geocoder/v2/?ak={0}&location={1},{2}&output=xml&pois=0";
			text = string.Format(text, this.ak, lat, lng);
			WebClient webClient = new WebClient();
			webClient.BaseAddress = text;
			string xml = "";
			try
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
					xml = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			catch
			{
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("GeocoderSearchResponse/status");
				if (xmlNode.InnerText == "0")
				{
					XmlNode xmlNode2 = xmlDocument.SelectSingleNode("GeocoderSearchResponse/result/formatted_address");
					this._address = xmlNode2.InnerText;
					XmlNode xmlNode3 = xmlDocument.SelectSingleNode("GeocoderSearchResponse/result/addressComponent");
					this._province = xmlNode3.SelectSingleNode("province").InnerText;
					this._city = xmlNode3.SelectSingleNode("city").InnerText;
					this._district = xmlNode3.SelectSingleNode("district").InnerText;
					this._street = xmlNode3.SelectSingleNode("street").InnerText;
					this._streetNumber = xmlNode3.SelectSingleNode("streetNumber").InnerText;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00022B18 File Offset: 0x00020D18
		private void _getPosiForAddress(string address, int type)
		{
			string arg = (type == 1) ? "xml" : "json";
			address = HttpUtility.UrlEncode(address);
			string text = "http://api.map.baidu.com/geocoder/v2/?ak={0}&output={1}&address={2}";
			text = string.Format(text, this.ak, arg, address);
			WebClient webClient = new WebClient();
			webClient.BaseAddress = text;
			string xml = "";
			try
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
					xml = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("GeocoderSearchResponse/status");
				if (xmlNode.InnerText == "0")
				{
					XmlNode xmlNode2 = xmlDocument.SelectSingleNode("GeocoderSearchResponse/result/location");
					this._latitude = xmlNode2.SelectSingleNode("lat").InnerText;
					this._longitude = xmlNode2.SelectSingleNode("lng").InnerText;
				}
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000B087 File Offset: 0x00009287
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000B096 File Offset: 0x00009296
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00022C3C File Offset: 0x00020E3C
		~Position()
		{
			this.Dispose(false);
		}

		// Token: 0x040001D3 RID: 467
		private string ak = App.Get["BaiduLBS"].String;

		// Token: 0x040001D4 RID: 468
		private string _ip;

		// Token: 0x040001D5 RID: 469
		private string _latitude;

		// Token: 0x040001D6 RID: 470
		private string _longitude;

		// Token: 0x040001D7 RID: 471
		private string _province;

		// Token: 0x040001D8 RID: 472
		private string _city;

		// Token: 0x040001D9 RID: 473
		private string _district;

		// Token: 0x040001DA RID: 474
		private string _street;

		// Token: 0x040001DB RID: 475
		private string _streetNumber;

		// Token: 0x040001DC RID: 476
		private string _address;

		// Token: 0x040001DD RID: 477
		private bool disposed;
	}
}
