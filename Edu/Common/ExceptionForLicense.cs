using System;

namespace Common
{
	/// <summary>
	/// 如果没有获得授权
	/// </summary>
	// Token: 0x02000056 RID: 86
	public class ExceptionForLicense : Exception
	{
		// Token: 0x06000268 RID: 616 RVA: 0x0000A081 File Offset: 0x00008281
		public ExceptionForLicense(string message) : base(message)
		{
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000920B File Offset: 0x0000740B
		public ExceptionForLicense()
		{
		}
	}
}
