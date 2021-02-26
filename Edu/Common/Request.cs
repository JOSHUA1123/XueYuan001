using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Common.Method;
using Common.Param.Method;

namespace Common
{
	/// <summary>
	/// 获取各种参数，如QueryString、Form、Cookies、Session等
	/// </summary>
	// Token: 0x020000CE RID: 206
	public class Request
	{
		/// <summary>
		///  获取 HTTP 查询字符串变量集合
		/// </summary>    
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0000B891 File Offset: 0x00009A91
		public static Request QueryString
		{
			get
			{
				return Request._queryString;
			}
		}

		/// <summary>
		/// 获取窗体变量集合
		/// </summary>
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0000B898 File Offset: 0x00009A98
		public static Request Form
		{
			get
			{
				return Request._form;
			}
		}

		/// <summary>
		///  获取客户端发送的 cookie 的集合
		/// </summary>
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0000B89F File Offset: 0x00009A9F
		public static Request Cookies
		{
			get
			{
				return Request._cookies;
			}
		}

		/// <summary>
		/// 获取 ASP.NET 提供的当前 Session 对象
		/// </summary>
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0000B8A6 File Offset: 0x00009AA6
		public static Request Session
		{
			get
			{
				return Request._session;
			}
		}

		/// <summary>
		/// 获取当前页面的信息
		/// </summary>
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0000B8AD File Offset: 0x00009AAD
		public static PageInfo Page
		{
			get
			{
				return new PageInfo();
			}
		}

		/// <summary>
		/// 获取属性的值
		/// </summary>
		/// <typeparam name="T">类型名称</typeparam>
		/// <param name="t">对象</param>
		/// <param name="propertyname">属性</param>
		/// <returns></returns>
		// Token: 0x060005B1 RID: 1457 RVA: 0x00027A98 File Offset: 0x00025C98
		public static string GetObjectPropertyValue<T>(T t, string propertyname)
		{
			Type type = t.GetType();
			PropertyInfo property = type.GetProperty(propertyname);
			if (property == null)
			{
				return string.Empty;
			}
			object value = property.GetValue(t, null);
			if (value == null)
			{
				return string.Empty;
			}
			return value.ToString();
		}

		/// <summary>
		/// 返回QueryString参数
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		// Token: 0x170001AF RID: 431
		public ConvertToAnyValue this[string str]
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				ConvertToAnyValue convertToAnyValue = new ConvertToAnyValue();
				string text = "";
				string requestType;
				if ((requestType = this._requestType) != null)
				{
					if (!(requestType == "QueryString"))
					{
						if (!(requestType == "Form"))
						{
							if (!(requestType == "Cookies"))
							{
								if (requestType == "Session")
								{
									if (httpContext.Session != null && httpContext.Session[str] != null && httpContext.Session[str].ToString().Trim() != "")
									{
										text = httpContext.Session[str].ToString();
									}
								}
							}
							else if (httpContext.Request.Cookies[str] != null && httpContext.Request.Cookies[str].Value.Trim() != "")
							{
								text = httpContext.Request.Cookies[str].Value;
							}
						}
						else if (httpContext.Request.Form[str] != null && httpContext.Request.Form[str].Trim() != "")
						{
							text = httpContext.Request.Form[str];
						}
					}
					else if (httpContext.Request.QueryString[str] != null && httpContext.Request.QueryString[str].Trim() != "")
					{
						text = httpContext.Request.QueryString[str];
						text = HTML.ClearTag(text, 0);
					}
				}
				convertToAnyValue.ParaValue = text;
				return convertToAnyValue;
			}
		}

		/// <summary>
		/// 当前页的Web控件
		/// </summary>
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0000B8B4 File Offset: 0x00009AB4
		public static Request Controls
		{
			get
			{
				return Request._controls;
			}
		}

		// Token: 0x170001B1 RID: 433
		public ConvertToAnyValue this[WebControl ctrl]
		{
			get
			{
				ConvertToAnyValue convertToAnyValue = new ConvertToAnyValue();
				string paraValue = "";
				if (ctrl is TextBox)
				{
					paraValue = ((TextBox)ctrl).Text.Trim();
				}
				if (ctrl is Label)
				{
					paraValue = ((Label)ctrl).Text.Trim();
				}
				convertToAnyValue.ParaValue = paraValue;
				return convertToAnyValue;
			}
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0000B8BB File Offset: 0x00009ABB
		private Request()
		{
		}

		/// <summary>
		/// 构造字符串类的参数，如post,get,cookies,session等
		/// </summary>
		/// <param name="requestType"></param>
		// Token: 0x060005B6 RID: 1462 RVA: 0x0000B8CE File Offset: 0x00009ACE
		private Request(string requestType)
		{
			this._requestType = requestType;
		}

		/// <summary>
		/// 构造控件类的参数，如TextBox
		/// </summary>
		/// <param name="requestControlName"></param>
		// Token: 0x060005B7 RID: 1463 RVA: 0x0000B8E8 File Offset: 0x00009AE8
		private Request(WebControl requestControlName)
		{
			this._requestControlName = requestControlName;
		}

		/// <summary>
		/// 生成随机数
		/// </summary>
		/// <returns></returns>
		// Token: 0x060005B8 RID: 1464 RVA: 0x00027CFC File Offset: 0x00025EFC
		public static int Random()
		{
			Random random = new Random();
			int num = random.Next(0, 1000);
			DateTime now = DateTime.Now;
			string value = now.Minute.ToString() + now.Millisecond.ToString() + num.ToString();
			return Convert.ToInt32(value);
		}

		/// <summary>
		/// 生成随机字符串，可以选择长度与类型
		/// </summary>
		/// <param name="VcodeNum">随机字符串的长度</param>
		/// <param name="type">生成的随机数类型，0为数字与大小写字母，1为纯数字，2为纯小字母，3为纯大写字母，4为大小写字母，5数字加小写，6数字加大写，</param>
		/// <returns></returns>
		// Token: 0x060005B9 RID: 1465 RVA: 0x00027D58 File Offset: 0x00025F58
		public static string Random(int VcodeNum, int type)
		{
			string text = "0,1,2,3,4,5,6,7,8,9,";
			string text2 = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
			string text3 = text2.ToUpper();
			string text4;
			switch (type)
			{
			case 0:
				text4 = text + text2 + text3;
				break;
			case 1:
				text4 = text;
				break;
			case 2:
				text4 = text2;
				break;
			case 3:
				text4 = text3;
				break;
			case 4:
				text4 = text3 + text2;
				break;
			case 5:
				text4 = text + text2;
				break;
			case 6:
				text4 = text + text3;
				break;
			default:
				text4 = text + text2 + text3;
				break;
			}
			text4 = text4.Substring(0, text4.Length - 1);
			string[] array = text4.Split(new char[]
			{
				','
			});
			string text5 = "";
			Random random = new Random();
			for (int i = 1; i < VcodeNum + 1; i++)
			{
				text5 += array[random.Next(array.Length)];
			}
			return text5;
		}

		/// <summary>
		/// 生成唯一标识
		/// </summary>
		/// <returns></returns>
		// Token: 0x060005BA RID: 1466 RVA: 0x00027E40 File Offset: 0x00026040
		public static string UniqueID()
		{
			string password = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), Guid.NewGuid().ToString().Split(new char[]
			{
				'-'
			})[4]);
			return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToLower();
		}

		/// <summary>
		/// 路径的操作
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		// Token: 0x060005BB RID: 1467 RVA: 0x0000B902 File Offset: 0x00009B02
		public static _Path Path(string fullName)
		{
			return new _Path(fullName, "");
		}

		/// <summary>
		/// 当访问的IP信息，包括其地理信息
		/// </summary>
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00027EA4 File Offset: 0x000260A4
		public static Position IP
		{
			get
			{
				string ip = Browser.IP;
				return new Position(ip);
			}
		}

		/// <summary>
		/// 通过经纬度获取访问者的地理信息
		/// </summary>
		/// <param name="lng">经度</param>
		/// <param name="lat">纬度</param>
		/// <returns></returns>
		// Token: 0x060005BD RID: 1469 RVA: 0x0000B90F File Offset: 0x00009B0F
		public static Position Position(string lng, string lat)
		{
			return new Position(lng, lat);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000B918 File Offset: 0x00009B18
		public static Position Position(string address)
		{
			return new Position(address, 1);
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0000B921 File Offset: 0x00009B21
		public static Domain Domain
		{
			get
			{
				return new Domain();
			}
		}

		/// <summary>
		/// 获取模版的大小,转换单位mb或kb
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		// Token: 0x060005C0 RID: 1472 RVA: 0x00027EC0 File Offset: 0x000260C0
		public static string GetSize(ulong size)
		{
			string result = "";
			if (size < 1024UL)
			{
				result = size.ToString() + " Bit";
			}
			else
			{
				double num = size / 1024UL;
				num = Math.Round(num * 100.0) / 100.0;
				if (num < 1024.0)
				{
					result = num.ToString() + " Kb";
				}
				else
				{
					double num2 = num / 1024.0;
					num2 = Math.Round(num2 * 100.0) / 100.0;
					if (num2 < 1024.0)
					{
						result = num2.ToString() + " Mb";
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 获取网页的返回结果
		/// </summary>
		/// <param name="url">网址</param>
		/// <returns></returns>
		// Token: 0x060005C1 RID: 1473 RVA: 0x00027F80 File Offset: 0x00026180
		public static string HttpGet(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
			httpWebRequest.Method = "GET";
			httpWebRequest.MaximumAutomaticRedirections = 3;
			httpWebRequest.Timeout = 10000;
			httpWebRequest.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
			httpWebRequest.Accept = "*/*";
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
			Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			return result;
		}

		/// <summary>
		/// Post方式获取网页的返回结果
		/// </summary>
		/// <param name="url">网址</param>
		/// <param name="json">json格式参数</param>
		/// <returns></returns>
		// Token: 0x060005C2 RID: 1474 RVA: 0x0002801C File Offset: 0x0002621C
		public static string HttpPost(string url, string json)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/json";
			if (!string.IsNullOrWhiteSpace(json))
			{
				byte[] bytes = Encoding.ASCII.GetBytes(json);
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
			}
			HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
			return streamReader.ReadToEnd();
		}

		/// <summary>
		///  Post方式获取网页的返回结果
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		// Token: 0x060005C3 RID: 1475 RVA: 0x0000B928 File Offset: 0x00009B28
		public static string HttpPost(string url)
		{
			return Request.HttpPost(url, string.Empty);
		}

		/// <summary>
		/// 下载文件
		/// </summary>
		/// <param name="url">文件网址</param>
		/// <param name="filepath">本地路径</param>
		// Token: 0x060005C4 RID: 1476 RVA: 0x000280C0 File Offset: 0x000262C0
		public static void LoadFile(string url, string filepath)
		{
			using (WebClient webClient = new WebClient())
			{
				try
				{
					webClient.Headers.Add("User-Agent", "Chrome");
					webClient.DownloadFile(url, filepath);
				}
				catch (Exception ex)
				{
					string currentDirectory = Environment.CurrentDirectory;
					using (FileStream fileStream = new FileStream(currentDirectory + "error.txt", FileMode.Append))
					{
						StreamWriter streamWriter = new StreamWriter(fileStream);
						streamWriter.WriteLine(url);
						streamWriter.WriteLine(ex.Message);
						streamWriter.Flush();
						streamWriter.Close();
						fileStream.Close();
					}
				}
			}
		}

		/// <summary>
		/// 获取版本信息
		/// </summary>
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0000B935 File Offset: 0x00009B35
		public static Copyright<string, string> Copyright
		{
			get
			{
				return Copyright<string, string>.Items;
			}
		}

		// Token: 0x0400025D RID: 605
		private string _requestType = "";

		// Token: 0x0400025E RID: 606
		private static readonly Request _queryString = new Request("QueryString");

		// Token: 0x0400025F RID: 607
		private static readonly Request _form = new Request("Form");

		// Token: 0x04000260 RID: 608
		private static readonly Request _cookies = new Request("Cookies");

		// Token: 0x04000261 RID: 609
		private static readonly Request _session = new Request("Session");

		// Token: 0x04000262 RID: 610
		private WebControl _requestControlName;

		// Token: 0x04000263 RID: 611
		private static readonly Request _controls = new Request(new TextBox());
	}
}
