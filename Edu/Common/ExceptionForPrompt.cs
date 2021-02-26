using System;

namespace Common
{
	/// <summary>
	/// 作为提示项的异常
	/// </summary>
	// Token: 0x02000053 RID: 83
	public class ExceptionForPrompt : Exception
	{
		// Token: 0x06000262 RID: 610 RVA: 0x0000A081 File Offset: 0x00008281
		public ExceptionForPrompt(string message) : base(message)
		{
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000920B File Offset: 0x0000740B
		public ExceptionForPrompt()
		{
		}
	}
}
