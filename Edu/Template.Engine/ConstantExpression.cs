using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 常数表达式.
	/// </summary>
	// Token: 0x02000038 RID: 56
	public class ConstantExpression : IExpression, ICloneableElement<IExpression>
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		// Token: 0x06000294 RID: 660 RVA: 0x0000BB10 File Offset: 0x00009D10
		public ConstantExpression(object value)
		{
			this.Value = value;
		}

		/// <summary>
		/// 常数值
		/// </summary>
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000BB1F File Offset: 0x00009D1F
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000BB27 File Offset: 0x00009D27
		private object Value { get; set; }

		/// <summary>
		/// 获取常数的值
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000297 RID: 663 RVA: 0x0000BB30 File Offset: 0x00009D30
		public object GetValue()
		{
			return this.Value;
		}

		/// <summary>
		/// 输出为字符串
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000298 RID: 664 RVA: 0x0000BB38 File Offset: 0x00009D38
		public override string ToString()
		{
			if (this.Value != null)
			{
				return this.Value.ToString();
			}
			return string.Empty;
		}

		/// <summary>
		/// 克隆表达式
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x06000299 RID: 665 RVA: 0x0000BB53 File Offset: 0x00009D53
		public IExpression Clone(Template ownerTemplate)
		{
			return this;
		}
	}
}
