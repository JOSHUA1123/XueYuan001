using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 定义可包含属性的元素
	/// </summary>
	// Token: 0x02000003 RID: 3
	public interface IAttributesElement
	{
		/// <summary>
		/// 返回元素属性集合
		/// </summary>
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9
		AttributeCollection Attributes { get; }
	}
}
