using System;
using System.Configuration;
using System.Xml;

namespace Common
{
	/// <summary>
	/// 系统平台的信息
	/// </summary>
	// Token: 0x0200007F RID: 127
	public class Platform
	{
		/// <summary>
		/// 平台名称
		/// </summary>
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0001B4EC File Offset: 0x000196EC
		public static string Name
		{
			get
			{
				string result;
				try
				{
					object section = ConfigurationManager.GetSection("Platform");
					XmlElement xmlElement = section as XmlElement;
					if (xmlElement.Attributes["name"] == null)
					{
						result = "";
					}
					else
					{
						result = xmlElement.Attributes["name"].Value;
					}
				}
				catch
				{
					result = "unknown";
				}
				return result;
			}
		}

		/// <summary>
		/// 版本号
		/// </summary>       
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0001B558 File Offset: 0x00019758
		public static string Version
		{
			get
			{
				string result;
				try
				{
					object section = ConfigurationManager.GetSection("Platform");
					XmlElement xmlElement = section as XmlElement;
					if (xmlElement.Attributes["version"] == null)
					{
						result = "";
					}
					else
					{
						result = xmlElement.Attributes["version"].Value;
					}
				}
				catch
				{
					result = "-1";
				}
				return result;
			}
		}

		/// <summary>
		/// 授权码
		/// </summary>       
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000A713 File Offset: 0x00008913
		public static License License
		{
			get
			{
				return License.Value;
			}
		}
	}
}
