using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
	/// <summary>
	/// 激活码管理
	/// </summary>
	/// <![CDATA[通过激活码申请注册]]>
	// Token: 0x02000042 RID: 66
	public class Activationcode
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x0000925C File Offset: 0x0000745C
		private Activationcode()
		{
		}

		/// <summary>
		/// 通过CPU串号生成激活码
		/// </summary>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00009BC9 File Offset: 0x00007DC9
		public static string CodeForCPU
		{
			get
			{
				return Activationcode.RegistCode(1, Server.CPU_ID);
			}
		}

		/// <summary>
		/// 通过硬盘串号生成激活码
		/// </summary>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009BD6 File Offset: 0x00007DD6
		public static string CodeForHardDisk
		{
			get
			{
				return Activationcode.RegistCode(2, Server.HardDiskID);
			}
		}

		/// <summary>
		/// 通过IP生成激活码
		/// </summary>
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00013A78 File Offset: 0x00011C78
		public static string CodeForIP
		{
			get
			{
				string text = Server.IP;
				text = text + ":" + Server.Port;
				return Activationcode.RegistCode(3, text);
			}
		}

		/// <summary>
		/// 通过域名生成激活码
		/// </summary>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00013AA4 File Offset: 0x00011CA4
		public static string CodeForDomain
		{
			get
			{
				string text = Server.Domain;
				text = text + ":" + Server.Port;
				return Activationcode.RegistCode(4, text);
			}
		}

		/// <summary>
		/// 通过主域名生成激活码
		/// </summary>
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00013AD0 File Offset: 0x00011CD0
		public static string CodeForRoot
		{
			get
			{
				string text = Server.MainName;
				text = text + ":" + Server.Port;
				return Activationcode.RegistCode(5, text);
			}
		}

		/// <summary>
		/// 加密字符串
		/// </summary>
		/// <param name="type">加密类型，1为cup串号，2为硬盘串号，3为IP+端口，4为域名+端口</param>
		/// <param name="value">需加密的字符串</param>
		/// <returns></returns>
		// Token: 0x060001B7 RID: 439 RVA: 0x00013AFC File Offset: 0x00011CFC
		public static string RegistCode(int type, string value)
		{
			string text = value;
			value = string.Concat(new string[]
			{
				text,
				"$",
				Platform.Name,
				"$",
				Platform.Version
			});
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
			string result;
			try
			{
				string text2 = Request.Random(8, 6);
				byte[] bytes = Encoding.UTF8.GetBytes(text2.ToLower());
				DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
				byte[] bytes2 = Encoding.UTF8.GetBytes(value);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(bytes2, 0, bytes2.Length);
				cryptoStream.FlushFinalBlock();
				string text3 = Convert.ToBase64String(memoryStream.ToArray());
				string text4 = "";
				for (int i = 0; i < text3.Length; i++)
				{
					if (i < text2.Length)
					{
						text4 = text4 + text3.Substring(i, 1) + text2.Substring(i, 1);
					}
					else
					{
						text4 += text3.Substring(i, 1);
					}
				}
				result = type + text4;
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}
	}
}
