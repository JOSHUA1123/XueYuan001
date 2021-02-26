using System;

namespace VTemplate.Engine.Evaluator
{
	/// <summary>
	/// 表达式节点的类型
	/// </summary>
	// Token: 0x0200003C RID: 60
	public enum ExpressionNodeType
	{
		/// <summary>
		/// 未知
		/// </summary>
		// Token: 0x040000A7 RID: 167
		Unknown,
		/// <summary>
		/// +
		/// </summary>
		// Token: 0x040000A8 RID: 168
		Plus,
		/// <summary>
		/// -
		/// </summary>
		// Token: 0x040000A9 RID: 169
		Subtract,
		/// <summary>
		/// *
		/// </summary>
		// Token: 0x040000AA RID: 170
		MultiPly,
		/// <summary>
		/// /
		/// </summary>
		// Token: 0x040000AB RID: 171
		Divide,
		/// <summary>
		/// (
		/// </summary>
		// Token: 0x040000AC RID: 172
		LParentheses,
		/// <summary>
		/// )
		/// </summary>
		// Token: 0x040000AD RID: 173
		RParentheses,
		/// <summary>
		/// % (求模,取余)
		/// </summary>
		// Token: 0x040000AE RID: 174
		Mod,
		/// <summary>
		/// ^ (次幂)
		/// </summary>
		// Token: 0x040000AF RID: 175
		Power,
		/// <summary>
		/// &amp; (按位与)
		/// </summary>
		// Token: 0x040000B0 RID: 176
		BitwiseAnd,
		/// <summary>
		/// | (按位或)
		/// </summary>
		// Token: 0x040000B1 RID: 177
		BitwiseOr,
		/// <summary>
		/// &amp;&amp; (逻辑与)
		/// </summary>
		// Token: 0x040000B2 RID: 178
		And,
		/// <summary>
		/// || (逻辑或)
		/// </summary>
		// Token: 0x040000B3 RID: 179
		Or,
		/// <summary>
		/// ! (逻辑非)
		/// </summary>
		// Token: 0x040000B4 RID: 180
		Not,
		/// <summary>
		/// == (相等)
		/// </summary>
		// Token: 0x040000B5 RID: 181
		Equal,
		/// <summary>
		/// != 或 &lt;&gt; (不等于)
		/// </summary>
		// Token: 0x040000B6 RID: 182
		Unequal,
		/// <summary>
		/// &gt; (大于)
		/// </summary>
		// Token: 0x040000B7 RID: 183
		GT,
		/// <summary>
		/// &lt; (小于)
		/// </summary>
		// Token: 0x040000B8 RID: 184
		LT,
		/// <summary>
		/// &gt;= (大于等于)
		/// </summary>
		// Token: 0x040000B9 RID: 185
		GTOrEqual,
		/// <summary>
		/// &lt;= (小于等于)
		/// </summary>
		// Token: 0x040000BA RID: 186
		LTOrEqual,
		/// <summary>
		/// &lt;&lt; (左移位)
		/// </summary>
		// Token: 0x040000BB RID: 187
		LShift,
		/// <summary>
		/// &gt;&gt; (右移位)
		/// </summary>
		// Token: 0x040000BC RID: 188
		RShift,
		/// <summary>
		/// 数值
		/// </summary>
		// Token: 0x040000BD RID: 189
		Numeric
	}
}
