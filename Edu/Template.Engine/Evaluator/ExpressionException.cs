using System;

namespace VTemplate.Engine.Evaluator
{
	/// <summary>
	/// 表达式错误
	/// </summary>
	// Token: 0x02000008 RID: 8
	public class ExpressionException : Exception
	{
		/// <summary>
		///
		/// </summary>
		// Token: 0x06000060 RID: 96 RVA: 0x0000359F File Offset: 0x0000179F
		public ExpressionException()
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="message"></param>
		// Token: 0x06000061 RID: 97 RVA: 0x000035A7 File Offset: 0x000017A7
		public ExpressionException(string message) : base(message)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		// Token: 0x06000062 RID: 98 RVA: 0x000035B0 File Offset: 0x000017B0
		public ExpressionException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
