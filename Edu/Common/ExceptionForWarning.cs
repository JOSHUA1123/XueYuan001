using System;

namespace Common
{
	/// <summary>
	/// 作为严重错误的异常
	/// </summary>
	// Token: 0x02000055 RID: 85
	public class ExceptionForWarning : Exception
	{
		// Token: 0x06000266 RID: 614 RVA: 0x0000A081 File Offset: 0x00008281
		public ExceptionForWarning(string message) : base(message)
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000920B File Offset: 0x0000740B
		public ExceptionForWarning()
		{
		}
	}
}
