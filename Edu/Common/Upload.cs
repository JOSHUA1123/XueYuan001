using System;
using Common.Param.Method;

namespace Common
{
	/// <summary>
	/// 系统上传功能的各模块上传到服务器端的路径
	/// </summary>
	// Token: 0x0200007E RID: 126
	public class Upload
	{
		/// <summary>
		/// 获取参数
		/// </summary>
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000A6CE File Offset: 0x000088CE
		public static Upload Get
		{
			get
			{
				return Upload._get;
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000A6D5 File Offset: 0x000088D5
		private Upload()
		{
			Upload.rootPath = PlatformInfoHandler.Node("Upload").GetAttr("path");
		}

		/// <summary>
		/// 上传到根路径
		/// </summary>
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000A6F6 File Offset: 0x000088F6
		public _Path Root
		{
			get
			{
				return new _Path(Upload.rootPath, "");
			}
		}

		// Token: 0x170000EB RID: 235
		public _Path this[string key]
		{
			get
			{
				if (key == string.Empty || key == null || key.Trim() == "")
				{
					return new _Path(Upload.rootPath, "");
				}
				return new _Path(Upload.rootPath, PlatformInfoHandler.Node("Upload").ItemValue(key));
			}
		}

		// Token: 0x0400013A RID: 314
		private static readonly Upload _get = new Upload();

		// Token: 0x0400013B RID: 315
		private static string rootPath;
	}
}
