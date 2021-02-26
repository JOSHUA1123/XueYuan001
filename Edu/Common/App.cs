using System;
using Common.Param.Method;

namespace Common
{
	/// <summary>
	/// 系统参数
	/// </summary>
	// Token: 0x020000B6 RID: 182
	public class App
	{
		/// <summary>
		/// 获取参数
		/// </summary>
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0000B332 File Offset: 0x00009532
		public static App Get
		{
			get
			{
				return App._get;
			}
		}

		/// <summary>
		/// 应用程序版本号
		/// </summary>
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00023F14 File Offset: 0x00022114
		public static string Version
		{
			get
			{
				PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode = PlatformInfoHandler.Node("App");
				return siteInfoHandlerParaNode.GetAttr("version");
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000925C File Offset: 0x0000745C
		private App()
		{
		}

		// Token: 0x17000178 RID: 376
		public ConvertToAnyValue this[string key]
		{
			get
			{
				return new ConvertToAnyValue(PlatformInfoHandler.Node("App").ItemValue(key));
			}
		}

		// Token: 0x040001F8 RID: 504
		private static readonly App _get = new App();
	}
}
