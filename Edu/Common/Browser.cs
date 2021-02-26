using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Common
{
	/// <summary>
	/// 浏览器信息
	/// </summary>
	// Token: 0x02000080 RID: 128
	public class Browser
	{
		/// <summary>
		/// 当前浏览器是否是手机浏览器
		/// </summary>
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0001B5C4 File Offset: 0x000197C4
		public static bool IsMobile
		{
			get
			{
				try
				{
					HttpContext httpContext = HttpContext.Current;
					string text = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
					Regex regex = new Regex("android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\\\/|plucker|pocket|psp|symbian|treo|up\\\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
					Regex regex2 = new Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\\\\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\\\\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\\\\-(n|u)|c55\\\\/|capi|ccwa|cdm\\\\-|cell|chtm|cldc|cmd\\\\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\\\\-s|devi|dica|dmob|do(c|p)o|ds(12|\\\\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\\\\-|_)|g1 u|g560|gene|gf\\\\-5|g\\\\-mo|go(\\\\.w|od)|gr(ad|un)|haie|hcit|hd\\\\-(m|p|t)|hei\\\\-|hi(pt|ta)|hp( i|ip)|hs\\\\-c|ht(c(\\\\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\\\\-(20|go|ma)|i230|iac( |\\\\-|\\\\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\\\\/)|klon|kpt |kwc\\\\-|kyo(c|k)|le(no|xi)|lg( g|\\\\/(k|l|u)|50|54|e\\\\-|e\\\\/|\\\\-[a-w])|libw|lynx|m1\\\\-w|m3ga|m50\\\\/|ma(te|ui|xo)|mc(01|21|ca)|m\\\\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\\\\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\\\\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\\\\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\\\\-2|po(ck|rt|se)|prox|psio|pt\\\\-g|qa\\\\-a|qc(07|12|21|32|60|\\\\-[2-7]|i\\\\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\\\\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\\\\-|oo|p\\\\-)|sdk\\\\/|se(c(\\\\-|0|1)|47|mc|nd|ri)|sgh\\\\-|shar|sie(\\\\-|m)|sk\\\\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\\\\-|v\\\\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\\\\-|tdg\\\\-|tel(i|m)|tim\\\\-|t\\\\-mo|to(pl|sh)|ts(70|m\\\\-|m3|m5)|tx\\\\-9|up(\\\\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\\\\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\\\\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\\\\-|2|g)|yas\\\\-|your|zeto|zte\\\\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
					if (string.IsNullOrWhiteSpace(text))
					{
						return false;
					}
					if (regex.IsMatch(text))
					{
						return true;
					}
					if (text.Length >= 4 && regex2.IsMatch(text.Substring(0, 4)))
					{
						return true;
					}
				}
				catch
				{
					return false;
				}
				return false;
			}
		}

		/// <summary>
		/// 当前浏览器是否是微信浏览器
		/// </summary>
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0001B65C File Offset: 0x0001985C
		public static bool IsWeixin
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
				return text.ToLower().Contains("micromessenger");
			}
		}

		/// <summary>
		/// 前端浏览器是否是微信小程序
		/// </summary>
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0001B69C File Offset: 0x0001989C
		public static bool IsWeixinApp
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
				return text.ToLower().Contains("miniprogram");
			}
		}

		/// <summary>
		/// 前端浏览器是否是桌面应用
		/// </summary>
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0001B6DC File Offset: 0x000198DC
		public static bool IsDestopApp
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string input = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
				Regex regex = new Regex("DeskApp\\(.[^\\)]*\\)");
				return regex.IsMatch(input);
			}
		}

		/// <summary>
		/// 是否处于Apicloud打包的APP中
		/// </summary>
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0001B71C File Offset: 0x0001991C
		public static bool IsAPICloud
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
				return text.ToLower().Contains("apicloud");
			}
		}

		/// <summary>
		/// 当前浏览器是否来自苹果手机
		/// </summary>
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0001B75C File Offset: 0x0001995C
		public static bool IsIPhone
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string input = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
				Regex regex = new Regex("ip(hone|od)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				return regex.IsMatch(input);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0001B7A0 File Offset: 0x000199A0
		public static bool IsIPad
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string input = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
				Regex regex = new Regex("ipad", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				return regex.IsMatch(input);
			}
		}

		/// <summary>
		/// 客户端IP
		/// </summary>       
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0001B7E4 File Offset: 0x000199E4
		public static string IP
		{
			get
			{
				string text = string.Empty;
				text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (text != null && text != string.Empty)
				{
					if (text.IndexOf(".") == -1)
					{
						text = null;
					}
					else if (text.IndexOf(",") != -1)
					{
						text = text.Replace(" ", "").Replace("\"", "");
						string[] array = text.Split(",;".ToCharArray());
						for (int i = 0; i < array.Length; i++)
						{
							if (Browser.IsIPAddress(array[i]) && array[i].Substring(0, 3) != "10." && array[i].Substring(0, 7) != "192.168" && array[i].Substring(0, 7) != "172.16.")
							{
								return array[i];
							}
						}
					}
					else
					{
						if (Browser.IsIPAddress(text))
						{
							return text;
						}
						text = null;
					}
				}
				if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null || !(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != string.Empty))
				{
					string text2 = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}
				else
				{
					string text3 = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				}
				if (text == null || text == string.Empty)
				{
					text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}
				if (text == null || text == string.Empty)
				{
					text = HttpContext.Current.Request.UserHostAddress;
				}
				return text;
			}
		}

		/// <summary>    
		/// 判断是否是IP地址格式 0.0.0.0    
		/// </summary>    
		/// <param name="str1">待判断的IP地址</param>    
		/// <returns>true or false</returns>    
		// Token: 0x0600034E RID: 846 RVA: 0x0001B9AC File Offset: 0x00019BAC
		private static bool IsIPAddress(string str1)
		{
			if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15)
			{
				return false;
			}
			string pattern = "^\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}$";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			return regex.IsMatch(str1);
		}

		/// <summary>
		/// 客户端的浏览器类型
		/// </summary>
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0001B9F4 File Offset: 0x00019BF4
		public static string Type
		{
			get
			{
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				return browser.Type;
			}
		}

		/// <summary>
		/// 客户端的浏览器的名称
		/// </summary>
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0001BA18 File Offset: 0x00019C18
		public static string Name
		{
			get
			{
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				return browser.Browser;
			}
		}

		/// <summary>
		/// 客户端的浏览器的版本号
		/// </summary>
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0001BA3C File Offset: 0x00019C3C
		public static string Version
		{
			get
			{
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				return browser.Version;
			}
		}

		/// <summary>
		/// 获取客户端的操作系统名称
		/// </summary>
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0001BA60 File Offset: 0x00019C60
		public static string OS
		{
			get
			{
				string text = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
				if (text.IndexOf("NT 4.0") > 0)
				{
					return "Windows NT ";
				}
				if (text.IndexOf("NT 5.0") > 0)
				{
					return "Windows 2000";
				}
				if (text.IndexOf("NT 5.1") > 0)
				{
					return "Windows XP";
				}
				if (text.IndexOf("NT 5.2") > 0)
				{
					return "Windows 2003";
				}
				if (text.IndexOf("NT 6.0") > 0)
				{
					return "Windows Vista";
				}
				if (text.IndexOf("NT 6.1") > 0)
				{
					return "Windows 7";
				}
				if (text.IndexOf("WindowsCE") > 0)
				{
					return "Windows CE";
				}
				if (text.IndexOf("NT") > 0)
				{
					return "Windows NT ";
				}
				if (text.IndexOf("9x") > 0)
				{
					return "Windows ME";
				}
				if (text.IndexOf("98") > 0)
				{
					return "Windows 98";
				}
				if (text.IndexOf("95") > 0)
				{
					return "Windows 95";
				}
				if (text.IndexOf("Win32") > 0)
				{
					return "Win32";
				}
				if (text.IndexOf("Linux") > 0)
				{
					return "Linux";
				}
				if (text.IndexOf("SunOS") > 0)
				{
					return "SunOS";
				}
				if (text.IndexOf("Mac") > 0)
				{
					return "Mac";
				}
				if (text.IndexOf("Linux") > 0)
				{
					return "Linux";
				}
				if (text.IndexOf("Windows") > 0)
				{
					return "Windows";
				}
				return "未知类型";
			}
		}

		/// <summary>
		/// 获取手机操作系统；
		/// </summary>
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0001BBE0 File Offset: 0x00019DE0
		public static string MobileOS
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string input = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
				Regex regex = new Regex("android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\\\/|plucker|pocket|psp|symbian|treo|up\\\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				MatchCollection matchCollection = regex.Matches(input);
				if (matchCollection.Count < 1)
				{
					return Browser.OS;
				}
				return matchCollection[0].Value;
			}
		}

		/// <summary>
		/// 获取手机号码（如果是手机访问），但是大多数通信商受限，无法获取
		/// </summary>
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0001BC38 File Offset: 0x00019E38
		public static string PhoneNumber
		{
			get
			{
				string text = "";
				HttpContext httpContext = HttpContext.Current;
				if (httpContext.Request.ServerVariables["DEVICEID"] != null)
				{
					text = httpContext.Request.ServerVariables["DEVICEID"].ToString();
				}
				if (httpContext.Request.ServerVariables["HTTP_X_UP_subno"] != null)
				{
					text = httpContext.Request.ServerVariables["HTTP_X_UP_subno"].ToString();
					text = text.Substring(3, 11);
				}
				if (httpContext.Request.ServerVariables["HTTP_X_NETWORK_INFO"] != null)
				{
					text = httpContext.Request.ServerVariables["HTTP_X_NETWORK_INFO"].ToString();
				}
				if (httpContext.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"] != null)
				{
					text = httpContext.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"].ToString();
				}
				return text;
			}
		}
	}
}
