using System;

namespace Common
{
	/// <summary>
	/// 如果没有登录
	/// </summary>
	// Token: 0x02000057 RID: 87
	public class ExceptionForNoLogin : Exception
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0000A081 File Offset: 0x00008281
		public ExceptionForNoLogin(string message) : base(message)
		{
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000920B File Offset: 0x0000740B
		public ExceptionForNoLogin()
		{
		}
	}
}
