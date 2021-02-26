using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 表达式接口
	/// </summary>
	// Token: 0x02000025 RID: 37
	public interface IExpression : ICloneableElement<IExpression>
	{
		/// <summary>
		/// 获取表达式的值
		/// </summary>
		/// <returns></returns>
		// Token: 0x060001E2 RID: 482
		object GetValue();
	}
}
