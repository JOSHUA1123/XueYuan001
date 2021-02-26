using System;

namespace Common
{
	/// <summary>
	/// 作为警告项的异常
	/// </summary>
	// Token: 0x02000054 RID: 84
	public class ExceptionForAlert : Exception
	{
		// Token: 0x06000264 RID: 612 RVA: 0x0000A081 File Offset: 0x00008281
		public ExceptionForAlert(string message) : base(message)
		{
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000920B File Offset: 0x0000740B
		public ExceptionForAlert()
		{
		}
	}
}
