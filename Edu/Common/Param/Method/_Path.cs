using System;
using System.IO;

namespace Common.Param.Method
{
	/// <summary>
	/// 用于处理上传路径的类
	/// </summary>
	// Token: 0x020000C2 RID: 194
	public class _Path
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="root">根目录</param>
		/// <param name="path"></param>
		// Token: 0x06000562 RID: 1378 RVA: 0x000269B0 File Offset: 0x00024BB0
		public _Path(string root, string path)
		{
			path = (string.IsNullOrWhiteSpace(path) ? "" : path);
			this._value = path;
			if (root == null)
			{
				root = "/";
			}
			if (root.Length < 1)
			{
				root = "/";
			}
			string a = root.Substring(root.Length - 1, 1);
			if (a != "\\" && a != "/")
			{
				root += "/";
			}
			this._path = root + path;
			this._virtualPath = root + path;
			if (!Directory.Exists(this.Physics))
			{
				Directory.CreateDirectory(this.Physics);
			}
		}

		/// <summary>
		/// 物理路径
		/// </summary>
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0000B615 File Offset: 0x00009815
		public string Physics
		{
			get
			{
				return Server.MapPath(this._virtualPath);
			}
		}

		/// <summary>
		/// 虚拟路径
		/// </summary>
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0000B622 File Offset: 0x00009822
		public string Virtual
		{
			get
			{
				return Server.VirtualPath(this._virtualPath);
			}
		}

		/// <summary>
		/// 原始值，即直接从web.config读取来的，未处理的值
		/// </summary>
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0000B62F File Offset: 0x0000982F
		public string String
		{
			get
			{
				return this._value;
			}
		}

		/// <summary>
		/// 获取文件夹大小
		/// </summary>
		/// <param name="path">文件夹路径</param>
		/// <returns>返回文件夹大小 ,单位字节</returns>
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0000B637 File Offset: 0x00009837
		public ulong Length
		{
			get
			{
				return this.getLength(this._path);
			}
		}

		/// <summary>
		/// 获取文件夹大小
		/// </summary>
		/// <param name="path">文件夹路径</param>
		/// <returns>返回文件夹大小 ,单位字节</returns>
		// Token: 0x06000567 RID: 1383 RVA: 0x00026A84 File Offset: 0x00024C84
		private ulong getLength(string path)
		{
			if (!Directory.Exists(path))
			{
				return 0UL;
			}
			ulong num = 0UL;
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				num += (ulong)fileInfo.Length;
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				num += this.getLength(directoryInfo2.FullName);
			}
			return num;
		}

		/// <summary>
		/// 获取模版的大小,转换单位mb或kb
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		// Token: 0x06000568 RID: 1384 RVA: 0x00026B04 File Offset: 0x00024D04
		public string GetSize(ulong size)
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
		/// 删除当前文件夹下的文件，如果有缩略图，同时删除缩略图
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>上传成功，则返回1；否则返回0</returns>
		// Token: 0x06000569 RID: 1385 RVA: 0x00026BC4 File Offset: 0x00024DC4
		public int DeleteFile(string filename)
		{
			string physics = this.Physics;
			if (!Directory.Exists(physics))
			{
				return 0;
			}
			string text = physics + filename;
			string path = text;
			if (text.IndexOf(".") > -1)
			{
				string str = text.Substring(text.LastIndexOf("."));
				path = text.Substring(0, text.LastIndexOf(".")) + "_small" + str;
			}
			try
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return 1;
		}

		// Token: 0x0400022B RID: 555
		private string _path = "";

		// Token: 0x0400022C RID: 556
		private string _virtualPath = "";

		// Token: 0x0400022D RID: 557
		private string _value = "";
	}
}
