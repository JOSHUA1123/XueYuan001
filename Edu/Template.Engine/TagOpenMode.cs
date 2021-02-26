using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 标签的开放模式
	/// </summary>
	// Token: 0x02000010 RID: 16
	public enum TagOpenMode
	{
		/// <summary>
		/// 简单的.不支持&lt;vt:datareader&gt;等标签
		/// </summary>
		// Token: 0x04000026 RID: 38
		Simple,
		/// <summary>
		/// 完全的.将支持所有标签
		/// </summary>
		// Token: 0x04000027 RID: 39
		Full
	}
}
