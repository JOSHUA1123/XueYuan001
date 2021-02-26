using System;

namespace Common
{
	/// <summary>
	/// 没有操作权限
	/// </summary>
	// Token: 0x02000058 RID: 88
	public class ExceptionForNoPurview : Exception
	{
		// Token: 0x0600026C RID: 620 RVA: 0x0000A081 File Offset: 0x00008281
		public ExceptionForNoPurview(string message) : base(message)
		{
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000920B File Offset: 0x0000740B
		public ExceptionForNoPurview()
		{
		}
	}
}
