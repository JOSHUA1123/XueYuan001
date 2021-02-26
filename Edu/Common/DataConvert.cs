using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Common.Param.Method;

namespace Common
{
	// Token: 0x0200008C RID: 140
	public class DataConvert
	{
		/// <summary>
		/// 将值转为指定的数据类型
		/// </summary>
		/// <param name="value"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		// Token: 0x06000397 RID: 919 RVA: 0x0001E160 File Offset: 0x0001C360
		public static object ChangeType(object value, Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				if (value == null || value.ToString().Length == 0)
				{
					return null;
				}
				NullableConverter nullableConverter = new NullableConverter(type);
				type = nullableConverter.UnderlyingType;
			}
			return Convert.ChangeType(value, type);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000A906 File Offset: 0x00008B06
		public static ConvertToAnyValue ChangeType(object value)
		{
			return new ConvertToAnyValue(value);
		}

		/// <summary>
		/// DES加密字符串
		/// </summary>
		/// <param name="encryptStr">需要加密的字符串</param>
		/// <param name="encryptKey">加密所用的Key</param>
		/// <returns></returns>
		// Token: 0x06000399 RID: 921 RVA: 0x0001E1B4 File Offset: 0x0001C3B4
		public static string EncryptForDES(string encryptStr, string encryptKey)
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
			string result;
			try
			{
				if (encryptKey.Length > 8)
				{
					encryptKey = encryptKey.Substring(0, 8);
				}
				byte[] bytes = Encoding.UTF8.GetBytes(encryptKey);
				DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
				byte[] bytes2 = Encoding.UTF8.GetBytes(encryptStr);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(bytes2, 0, bytes2.Length);
				cryptoStream.FlushFinalBlock();
				result = Convert.ToBase64String(memoryStream.ToArray());
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
		// Token: 0x0600039A RID: 922 RVA: 0x0001E25C File Offset: 0x0001C45C
		public static string DecryptForDES(string decryptStr, string decryptKey)
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
				if (decryptKey.Length > 8)
				{
					decryptKey = decryptKey.Substring(0, 8);
				}
				byte[] bytes = Encoding.UTF8.GetBytes(decryptKey);
				DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
				array = Convert.FromBase64String(decryptStr);
				MemoryStream memoryStream = new MemoryStream();
				CryptoStream cryptoStream = new CryptoStream(memoryStream, descryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
				cryptoStream.Write(array, 0, array.Length);
				cryptoStream.FlushFinalBlock();
				Encoding encoding = new UTF8Encoding();
				result = encoding.GetString(memoryStream.ToArray());
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 加密RSA字符串
		/// </summary>
		/// <param name="encryptStr">需要加密的字符串</param>
		/// <param name="encryptKey">加密所用的Key</param>
		/// <returns></returns>
		// Token: 0x0600039B RID: 923 RVA: 0x0001E314 File Offset: 0x0001C514
		public static string EncryptForRSA(string encryptStr, string encryptKey)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider(1024, new CspParameters
			{
				Flags = CspProviderFlags.UseMachineKeyStore
			});
			byte[] bytes = Encoding.Default.GetBytes(encryptStr);
			byte[] inArray = rsacryptoServiceProvider.Encrypt(bytes, false);
			return Convert.ToBase64String(inArray);
		}

		/// <summary>
		/// 解密RSA密码
		/// </summary>
		/// <param name="decryptStr">要解密的字符串</param>
		/// <param name="decryptKey">解密的Key</param>
		/// <returns></returns>
		// Token: 0x0600039C RID: 924 RVA: 0x0001E358 File Offset: 0x0001C558
		public static string DecryptForRSA(string decryptStr, string decryptKey)
		{
			if (string.IsNullOrWhiteSpace(decryptStr))
			{
				return string.Empty;
			}
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider(1024, new CspParameters
			{
				Flags = CspProviderFlags.UseMachineKeyStore
			});
			byte[] rgb = Convert.FromBase64String(decryptStr);
			byte[] bytes = rsacryptoServiceProvider.Decrypt(rgb, false);
			return Encoding.Default.GetString(bytes);
		}

		/// <summary>
		/// 加密字符为base64
		/// </summary>
		/// <param name="encryptStr"></param>
		/// <returns></returns>
		// Token: 0x0600039D RID: 925 RVA: 0x0001E3A8 File Offset: 0x0001C5A8
		public static string EncryptForBase64(string encryptStr)
		{
			byte[] bytes = Encoding.Default.GetBytes(encryptStr);
			return Convert.ToBase64String(bytes);
		}

		/// <summary>
		/// 加密字符为base64，并进行url编码
		/// </summary>
		/// <param name="encryptStr"></param>
		/// <returns></returns>
		// Token: 0x0600039E RID: 926 RVA: 0x0001E3C8 File Offset: 0x0001C5C8
		public static string EncryptForBase64UrlEncode(string encryptStr)
		{
			byte[] bytes = Encoding.Default.GetBytes(encryptStr);
			string str = Convert.ToBase64String(bytes);
			return HttpUtility.UrlEncode(str);
		}

		/// <summary>
		/// 解密base64字符
		/// </summary>
		/// <param name="decryptStr"></param>
		/// <returns></returns>
		// Token: 0x0600039F RID: 927 RVA: 0x0001E3F0 File Offset: 0x0001C5F0
		public static string DecryptForBase64(string decryptStr)
		{
			byte[] bytes = Convert.FromBase64String(decryptStr);
			return Encoding.Default.GetString(bytes);
		}
	}
}
