using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 支持元素的深度克隆的接口定义
	/// </summary>
	// Token: 0x0200000A RID: 10
	public interface ICloneableElement<T>
	{
		/// <summary>
		/// 克隆元素
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600007E RID: 126
		T Clone(Template ownerTemplate);
	}
}
