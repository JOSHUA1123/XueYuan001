using System;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// 解析器的正则表达式库
	/// </summary>
	// Token: 0x02000032 RID: 50
	internal static class ParserRegex
	{
		/// <summary>
		///
		/// </summary>
		// Token: 0x0600027A RID: 634 RVA: 0x0000B124 File Offset: 0x00009324
		static ParserRegex()
		{
			string str = "(?:#(?<prefix>#|[\\-\\w]*)\\.)?(?<name>(?!\\d)\\w+)(?:\\.(?<field>\\d+|(?!\\d)\\w+)(?<method>(?:\\( *\\))?))*";
			ParserRegex.VarTagRegex = new Regex("\\G{\\$\\:?\\s*" + str + "(\\s+(?<attrname>[\\-\\w]+)(\\s*=\\s*\"(?<attrval>[^\"]*)\"|\\s*=\\s*'(?<attrval>[^']*)'|\\s*=\\s*(?<attrval>[^\\s=}]*)|(?<attrval>\\s*?)))*\\s*}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			ParserRegex.VarExpRegex = new Regex("^\\s*" + str + "\\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			ParserRegex.VarIdRegex = new Regex("^\\s*\\$?(?:#(?<prefix>#|[\\-\\w]*)\\.)?(?<name>(?!\\d)\\w+)\\s*$", RegexOptions.Compiled);
		}

		/// <summary>
		/// 标签的正则表达.如标签: &lt;vt:for from="1" to="100" index="i"&gt;
		/// </summary>
		// Token: 0x0400008C RID: 140
		internal static readonly Regex TagRegex = new Regex("\\G<vt\\:\\s*(?<tagname>[\\-\\w]+)(\\s+(?<attrname>[\\-\\w]+)(\\s*=\\s*\"(?<attrval>[^\"]*)\"|\\s*=\\s*'(?<attrval>[^']*)'|\\s*=\\s*(?<attrval>[^\\s=>]*)|(?<attrval>\\s*?)))*\\s*(?<closed>/)?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// 结束标签的正则表达式.如标签: &lt;/vt:for&gt;
		/// </summary>
		// Token: 0x0400008D RID: 141
		internal static readonly Regex EndTagRegex = new Regex("\\G</vt\\:\\s*(?<tagname>[\\-\\w]+)(\\s+(?<attrname>[\\-\\w]+)(\\s*=\\s*\"(?<attrval>[^\"]*)\"|\\s*=\\s*'(?<attrval>[^']*)'|\\s*=\\s*(?<attrval>[^\\s=>]*)|(?<attrval>\\s*?)))*\\s*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// 变量标签的正则表达式.如: {$:member.name} 或带前缀与属性的变量标签: {$:#.member.name htmlencode='true'}
		/// 前缀必须与#号开始.并且与.号结束.
		/// 如变量没有前缀.则表明此变量是根级模板的变量.
		/// 如果变量只有前缀"#."(如#.member.name)则表示当前模板的变量
		/// 如果变量只有前缀"##."(如##.member.name)则表示是当前模板的父级模板的变量.如果不存在父级模板.则归为当前模板.
		/// 如果变量是前缀"#"加其它字符.(如(#t1.member.name).则表示此变量是属于Id为"t1"的模板.如果不存在Id为"t1"的模板.则将产生解析错误.
		/// 注: 3.7版本以上，可以不写“:”号，如“{$member.name}”与“{$:member.name}”表示相同
		/// </summary>
		// Token: 0x0400008E RID: 142
		internal static readonly Regex VarTagRegex;

		/// <summary>
		/// 变量表达的正则表达式.如变量表达式: member.name 或带前缀的变量表达式: #.member.name
		/// </summary>
		// Token: 0x0400008F RID: 143
		internal static readonly Regex VarExpRegex;

		/// <summary>
		/// 变量标识的正则表达式.如变量:member 或带前缀 #.member。
		/// 注：3.7版本以上支持前缀“$”，如“$member”与“member”表示相同的变量标识
		/// </summary>
		// Token: 0x04000090 RID: 144
		internal static readonly Regex VarIdRegex;
	}
}
