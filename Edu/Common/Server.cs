using System;
using System.Configuration;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using Microsoft.Win32;

namespace Common
{
	/// <summary>
	/// 服务器信息
	/// </summary>
	// Token: 0x0200004F RID: 79
	public class Server
	{
		/// <summary>
		/// 服务器IP
		/// </summary>       
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00015620 File Offset: 0x00013820
		public static string IP
		{
			get
			{
				if (Server.Domain.Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
				{
					return "127.0.0.1";
				}
				HttpContext httpContext = HttpContext.Current;
				try
				{
					if (httpContext != null && httpContext.Request != null && httpContext.Request.ServerVariables != null)
					{
						return HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
					}
				}
				catch
				{
				}
				IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
				if (hostEntry == null)
				{
					return "";
				}
				if (hostEntry.AddressList.Length < 1)
				{
					return "";
				}
				string result;
				try
				{
					IPAddress ipaddress = hostEntry.AddressList[1];
					result = ipaddress.ToString().Trim();
				}
				catch
				{
					result = "";
				}
				return result;
			}
		}

		/// <summary>
		/// 是否是本机IP
		/// </summary>
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000156EC File Offset: 0x000138EC
		public static bool IsLocalIP
		{
			get
			{
				string ip = Server.IP;
				return ip == "127.0.0.1" || Server.Domain.ToLower().Trim() == "localhost";
			}
		}

		/// <summary>
		/// 是否是内网IP
		/// </summary>
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0001572C File Offset: 0x0001392C
		public static bool IsIntranetIP
		{
			get
			{
				if (Server.IsLocalIP)
				{
					return true;
				}
				string ip = Server.IP;
				return ip.Substring(0, 3) == "10." || ip.Substring(0, 7) == "192.168" || ip.Substring(0, 7) == "172.16.";
			}
		}

		/// <summary>
		/// 服务器访问端口
		/// </summary>
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001FF RID: 511 RVA: 0x00015788 File Offset: 0x00013988
		public static string Port
		{
			get
			{
				string text = "80";
				string result;
				try
				{
					if (HttpContext.Current != null)
					{
						text = HttpContext.Current.Request.Url.Port.ToString();
					}
					if (text == "443")
					{
						text = "80";
					}
					result = text;
				}
				catch
				{
					result = text;
				}
				return result;
			}
		}

		/// <summary>
		/// 站点的访问域名
		/// </summary>
		/// <returns></returns>
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000157EC File Offset: 0x000139EC
		public static string Domain
		{
			get
			{
				try
				{
					if (HttpContext.Current != null)
					{
						return HttpContext.Current.Request.Url.Host.ToString();
					}
				}
				catch
				{
					return "";
				}
				return "";
			}
		}

		/// <summary>
		/// 站点的访问域名带端口，如:http://www.xx.com/
		/// </summary>
		/// <returns></returns>
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00015840 File Offset: 0x00013A40
		public static string DomainPath
		{
			get
			{
				string result = string.Empty;
				if (HttpContext.Current != null)
				{
					result = "http://" + Server.Domain + ":" + Server.Port;
				}
				return result;
			}
		}

		/// <summary>
		/// 根域名
		/// </summary>
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00015878 File Offset: 0x00013A78
		public static string RootDomain
		{
			get
			{
				string domain = Server.Domain;
				string mainName = Server.MainName;
				if (domain.Length < mainName.Length)
				{
					return string.Empty;
				}
				return "";
			}
		}

		/// <summary>
		/// 当前站点的主域（来自db.config设置）
		/// </summary>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00013A4C File Offset: 0x00011C4C
		public static string MainName
		{
			get
			{
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
				return connectionStringSettings.Name;
			}
		}

		/// <summary>
		/// 服务器操作系统
		/// </summary>  
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000158AC File Offset: 0x00013AAC
		public static string OS
		{
			get
			{
				OperatingSystem osversion = Environment.OSVersion;
				switch (osversion.Platform)
				{
				case PlatformID.Win32Windows:
				{
					int minor = osversion.Version.Minor;
					if (minor == 0)
					{
						return "Win95 ";
					}
					if (minor != 10)
					{
						if (minor == 90)
						{
							return "WinMe ";
						}
					}
					else
					{
						if (osversion.Version.Revision.ToString() == "2222A ")
						{
							return "Win98r2";
						}
						return "Win98 ";
					}
					break;
				}
				case PlatformID.Win32NT:
					switch (osversion.Version.Major)
					{
					case 3:
						return "WindNT 3.51 ";
					case 4:
						return "WinNT 4.0 ";
					case 5:
						switch (osversion.Version.Minor)
						{
						case 0:
							return "Win2000";
						case 1:
							return "WinXP ";
						case 2:
							return "Win2003 ";
						}
						break;
					case 6:
						switch (osversion.Version.Minor)
						{
						case 0:
							return "WinVista ";
						case 1:
							return "Win7 ";
						}
						break;
					}
					break;
				}
				return osversion.VersionString;
			}
		}

		/// <summary>
		/// IIS版本
		/// </summary>
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000159CC File Offset: 0x00013BCC
		public static string IISVersion
		{
			get
			{
				string result = string.Empty;
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("software\\microsoft\\inetstp");
				if (registryKey != null)
				{
					result = Convert.ToInt32(registryKey.GetValue("majorversion", -1)).ToString();
				}
				return result;
			}
		}

		/// <summary>
		/// CPU个数
		/// </summary>
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00015A14 File Offset: 0x00013C14
		public static int CPUCount
		{
			get
			{
				int result;
				try
				{
					string environmentVariable = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
					result = (int)Convert.ToInt16(environmentVariable);
				}
				catch
				{
					result = 0;
				}
				return result;
			}
		}

		/// <summary>
		/// CPU主频，单位 GHz
		/// </summary>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00015A4C File Offset: 0x00013C4C
		public static string CPUHz
		{
			get
			{
				string result;
				try
				{
					ManagementClass managementClass = new ManagementClass("Win32_Processor");
					ManagementObjectCollection instances = managementClass.GetInstances();
					int count = instances.Count;
					ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
					string value = "";
					foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						value = managementObject.Properties["CurrentClockSpeed"].Value.ToString();
					}
					double num = Convert.ToDouble(value);
					result = (Math.Round(num / 100.0) / 10.0).ToString();
				}
				catch
				{
					result = "";
				}
				return result;
			}
		}

		/// <summary>
		/// 物理内存大小
		/// </summary>
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00015B2C File Offset: 0x00013D2C
		public static double RamSize
		{
			get
			{
				double result;
				try
				{
					double num = 0.0;
					ManagementClass managementClass = new ManagementClass("Win32_PhysicalMemory");
					ManagementObjectCollection instances = managementClass.GetInstances();
					foreach (ManagementBaseObject managementBaseObject in instances)
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						num += Math.Round((double)(long.Parse(managementObject.Properties["Capacity"].Value.ToString()) / 1024L / 1024L) / 1024.0, 1);
					}
					instances.Dispose();
					managementClass.Dispose();
					result = num;
				}
				catch
				{
					result = 0.0;
				}
				return result;
			}
		}

		/// <summary>
		/// .Net FramwWork版本号
		/// </summary>
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00015C04 File Offset: 0x00013E04
		public static string DotNetVersion
		{
			get
			{
				string text = Environment.Version.ToString();
				if (text.IndexOf('.') < 0)
				{
					return text;
				}
				return text.Substring(0, 3);
			}
		}

		/// <summary>
		/// 获取CPU的序列号，由于某些原因，可能获取不到
		/// </summary>
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00015C34 File Offset: 0x00013E34
		public static string CPU_ID
		{
			get
			{
				string text = null;
				try
				{
					ManagementClass managementClass = new ManagementClass("win32_Processor");
					ManagementObjectCollection instances = managementClass.GetInstances();
					foreach (ManagementBaseObject managementBaseObject in instances)
					{
						ManagementObject managementObject = (ManagementObject)managementBaseObject;
						if (managementObject.Properties["Processorid"] != null)
						{
							text = managementObject.Properties["Processorid"].Value.ToString();
							break;
						}
					}
				}
				catch
				{
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					text = "notGetCPU";
				}
				return text;
			}
		}

		/// <summary>
		/// 取第一块硬盘物理串号；可能会由于某些原因取不到物理串号，会自动返回   
		/// </summary>
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00015CE4 File Offset: 0x00013EE4
		public static string HardDiskID
		{
			get
			{
				string text = null;
				try
				{
					ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
					using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							ManagementObject managementObject = (ManagementObject)enumerator.Current;
							text = managementObject["SerialNumber"].ToString().Trim();
						}
					}
				}
				catch
				{
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					try
					{
						ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher
						{
							Query = new SelectQuery("Win32_DiskDrive", "", new string[]
							{
								"PNPDeviceID",
								"Signature"
							})
						}.Get();
						ManagementObjectCollection.ManagementObjectEnumerator enumerator2 = managementObjectCollection.GetEnumerator();
						enumerator2.MoveNext();
						ManagementBaseObject managementBaseObject = enumerator2.Current;
						text = managementBaseObject.Properties["signature"].Value.ToString().Trim();
					}
					catch
					{
					}
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					try
					{
						new ManagementClass("Win32_NetworkAdapterConfiguration");
						ManagementObject managementObject2 = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
						managementObject2.Get();
						text = managementObject2.GetPropertyValue("VolumeSerialNumber").ToString();
					}
					catch
					{
					}
				}
				if (string.IsNullOrWhiteSpace(text))
				{
					text = "notGetHardDiskID";
				}
				return text;
			}
		}

		/// <summary>
		/// 当前应用程序的物理路径
		/// </summary>
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00009E28 File Offset: 0x00008028
		public static string ProgramPath
		{
			get
			{
				return HostingEnvironment.ApplicationPhysicalPath;
			}
		}

		/// <summary>
		/// 数据库类型
		/// </summary>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00015E58 File Offset: 0x00014058
		public static string DatabaseType
		{
			get
			{
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
				string providerName = connectionStringSettings.ProviderName;
				if (providerName.ToLower().IndexOf("access") > -1)
				{
					return "Access";
				}
				if (providerName.ToLower().IndexOf("sqlserver9") > -1)
				{
					return "SqlServer2005";
				}
				if (providerName.ToLower().IndexOf("sqlserver") > -1)
				{
					return "SqlServer2000";
				}
				if (providerName.ToLower().IndexOf("oracle") > -1)
				{
					return "Oracle";
				}
				return "";
			}
		}

		/// <summary>
		/// 数据库的完整物理路径名，包括路径与文件名；如果是Access数据库，则返回路径，如果是其它数据库则不返回
		/// </summary>
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00015EEC File Offset: 0x000140EC
		public static string DatabaseFilePath
		{
			get
			{
				if (Server.DatabaseType.ToLower().Trim() != "access")
				{
					return "";
				}
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
				string text = connectionStringSettings.ConnectionString;
				int num = text.LastIndexOf('=');
				int num2 = text.LastIndexOf(';');
				if (num < num2)
				{
					text = text.Substring(num + 1, num2 - num - 1);
				}
				else
				{
					text = text.Substring(num + 1);
				}
				text = text.Replace("|DataDirectory|", Server.ProgramPath + "\\App_Data\\");
				return Server.MapPath(text);
			}
		}

		/// <summary>
		/// 数据库所在的物理路径，不包括文件名；如果是Access数据库，则返回路径，如果是其它数据库则不返回
		/// </summary>
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00015F8C File Offset: 0x0001418C
		public static string DatabasePath
		{
			get
			{
				if (Server.DatabaseType.IndexOf("SqlServer") > -1)
				{
					return "";
				}
				if (Server.DatabaseType.ToLower().Trim() == "access")
				{
					string text = Server.DatabaseFilePath;
					if (text.IndexOf("\\") > -1)
					{
						text = text.Substring(0, text.LastIndexOf("\\") + 1);
					}
					return text;
				}
				return "";
			}
		}

		/// <summary>
		/// 获取某路径的物理路径，路径末尾带有\符号
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		// Token: 0x06000210 RID: 528 RVA: 0x00015FFC File Offset: 0x000141FC
		public static string MapPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return path;
			}
			string pattern = "^[a-zA-Z]:\\\\{1,2}.+(\\\\?)$";
			if (Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace))
			{
				path = path.Replace("/", "\\");
			}
			else
			{
				path = HostingEnvironment.MapPath(path);
			}
			if (string.IsNullOrWhiteSpace(path))
			{
				return path;
			}
			if (path.IndexOf("\\") > -1 && path.Substring(path.LastIndexOf("\\")).IndexOf(".") < 0 && path.Substring(path.Length - 1) != "\\")
			{
				path += "\\";
			}
			path = Regex.Replace(path, "\\\\+", "\\");
			return path;
		}

		/// <summary>
		/// 获取某路径相对网站根目录的虚拟路径，路径末尾带有/符号
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		// Token: 0x06000211 RID: 529 RVA: 0x000160B0 File Offset: 0x000142B0
		public static string VirtualPath(string path)
		{
			path = path.Replace("\\", "/");
			path = path.Replace("~/", HostingEnvironment.ApplicationVirtualPath);
			if (path.IndexOf("/") > -1 && path.Substring(path.LastIndexOf("/")).IndexOf(".") < 0 && path.Substring(path.Length - 1) != "/")
			{
				path += "/";
			}
			path = Regex.Replace(path, "\\/+", "/");
			return path;
		}

		/// <summary>
		/// 将文件名转换为合法的文件名
		/// </summary>
		/// <param name="filename">文件名</param>
		/// <returns></returns>
		// Token: 0x06000212 RID: 530 RVA: 0x00016148 File Offset: 0x00014348
		public static string LegalName(string filename)
		{
			string text = "\\/:*?<>|\"";
			string text2 = "＼／：★？〖〗｜＂";
			for (int i = 0; i < text.Length; i++)
			{
				string oldValue = text.Substring(i, 1);
				string newValue = text2.Substring(i, 1);
				filename = filename.Replace(oldValue, newValue);
			}
			return filename;
		}

		/// <summary>
		/// 页面重定向
		/// </summary>
		/// <param name="path"></param>
		// Token: 0x06000213 RID: 531 RVA: 0x00016194 File Offset: 0x00014394
		public static void Rewriter(string path)
		{
			HttpContext httpContext = HttpContext.Current;
			httpContext.RewritePath(path);
		}

		/// <summary>
		/// JavaScript时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数。
		/// </summary>
		// Token: 0x06000214 RID: 532 RVA: 0x00009E2F File Offset: 0x0000802F
		public static long getTime()
		{
			return Server.getTime(DateTime.Now);
		}

		/// <summary>
		///  JavaScript时间戳：是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数。
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		// Token: 0x06000215 RID: 533 RVA: 0x000161B0 File Offset: 0x000143B0
		public static long getTime(DateTime time)
		{
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			return (long)(time - d).TotalMilliseconds;
		}

		/// <summary>
		/// 系统部署运行的初始时间
		/// </summary>
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000216 RID: 534 RVA: 0x000161E8 File Offset: 0x000143E8
		public static DateTime InitDate
		{
			get
			{
				if (Server._initDate > DateTime.Now.AddYears(-100))
				{
					return Server._initDate;
				}
				Server._initDate = License.Value.InitDate;
				return Server._initDate;
			}
		}

		// Token: 0x040000BF RID: 191
		public static DateTime _initDate = DateTime.Now.AddYears(-200);
	}
}
