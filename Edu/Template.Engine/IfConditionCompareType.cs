using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// If条件的比较类型
	/// </summary>
	// Token: 0x02000033 RID: 51
	public enum IfConditionCompareType
	{
		/// <summary>
		/// 相等比较"="或"=="
		/// </summary>
		// Token: 0x04000092 RID: 146
		Equal,
		/// <summary>
		/// 小于比较"&lt;"
		/// </summary>
		// Token: 0x04000093 RID: 147
		LT,
		/// <summary>
		/// 小于或等于比较"&lt;="
		/// </summary>
		// Token: 0x04000094 RID: 148
		LTAndEqual,
		/// <summary>
		/// 大于比较"&gt;"
		/// </summary>
		// Token: 0x04000095 RID: 149
		GT,
		/// <summary>
		/// 大于或等于比较"&gt;="
		/// </summary>
		// Token: 0x04000096 RID: 150
		GTAndEqual,
		/// <summary>
		/// 不等于比较"&lt;&gt;"或"!="
		/// </summary>
		// Token: 0x04000097 RID: 151
		UnEqual,
		/// <summary>
		/// 是否以某些值开始"^="
		/// </summary>
		// Token: 0x04000098 RID: 152
		StartWith,
		/// <summary>
		/// 是否以某些值结束"$="
		/// </summary>
		// Token: 0x04000099 RID: 153
		EndWith,
		/// <summary>
		/// 是否包含某些值"*="
		/// </summary>
		// Token: 0x0400009A RID: 154
		Contains
	}
}
