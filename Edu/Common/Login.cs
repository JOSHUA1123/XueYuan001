using System;

namespace Common
{
	/// <summary>
	/// 系统参数
	/// </summary>
	// Token: 0x02000073 RID: 115
	public class Login
	{
		/// <summary>
		/// 获取参数
		/// </summary>
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000A41E File Offset: 0x0000861E
		public static Login Get
		{
			get
			{
				return Login._get;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000925C File Offset: 0x0000745C
		private Login()
		{
		}

		// Token: 0x170000D0 RID: 208
		public LoginItem this[string key]
		{
			get
			{
				return new LoginItem(PlatformInfoHandler.Node("Login")[key]);
			}
		}

		// Token: 0x0400011E RID: 286
		private static readonly Login _get = new Login();
	}
}
