using System;
using System.Text.RegularExpressions;
using System.Web;

namespace VTemplate.Engine
{
	/// <summary>
	/// 解析器的帮助类
	/// </summary>
	// Token: 0x02000027 RID: 39
	internal static class ParserHelper
	{
		/// <summary>
		/// 读取某个偏移位置的字符.如果超出则返回特殊字符"\0x0"
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		// Token: 0x060001F6 RID: 502 RVA: 0x0000950B File Offset: 0x0000770B
		internal static char ReadChar(string text, int offset)
		{
			if (offset < text.Length)
			{
				return text[offset];
			}
			return '\0';
		}

		/// <summary>
		/// 判断c是否是c1,c2中的一个
		/// </summary>
		/// <param name="c"></param>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		// Token: 0x060001F7 RID: 503 RVA: 0x0000951F File Offset: 0x0000771F
		private static bool IsChars(char c, char c1, char c2)
		{
			return c == c1 || c == c2;
		}

		/// <summary>
		/// 判断是否是变量标签的开始
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		// Token: 0x060001F8 RID: 504 RVA: 0x0000952B File Offset: 0x0000772B
		internal static bool IsVariableTagStart(string text, int offset)
		{
			return ParserHelper.ReadChar(text, offset) == '{' && ParserHelper.ReadChar(text, offset + 1) == '$';
		}

		/// <summary>
		/// 判断是否是某种标签的开始
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		// Token: 0x060001F9 RID: 505 RVA: 0x00009548 File Offset: 0x00007748
		internal static bool IsTagStart(string text, int offset)
		{
			return ParserHelper.ReadChar(text, offset) == '<' && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 1), 'v', 'V') && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 2), 't', 'T') && ParserHelper.ReadChar(text, offset + 3) == ':';
		}

		/// <summary>
		/// 判断是否是某种结束标签的开始
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		// Token: 0x060001FA RID: 506 RVA: 0x00009598 File Offset: 0x00007798
		internal static bool IsCloseTagStart(string text, int offset)
		{
			return ParserHelper.ReadChar(text, offset) == '<' && ParserHelper.ReadChar(text, offset + 1) == '/' && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 2), 'v', 'V') && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 3), 't', 'T') && ParserHelper.ReadChar(text, offset + 4) == ':';
		}

		/// <summary>
		/// 判断是否是VT表达式的开始. vt="&lt;vt: 或者 vt='&lt;vt:
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		// Token: 0x060001FB RID: 507 RVA: 0x000095F4 File Offset: 0x000077F4
		internal static bool IsVTExpressionStart(string text, int offset)
		{
			return ParserHelper.IsChars(ParserHelper.ReadChar(text, offset), 'v', 'V') && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 1), 't', 'T') && ParserHelper.ReadChar(text, offset + 2) == '=' && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 3), '"', '\'') && ParserHelper.IsTagStart(text, offset + 4);
		}

		/// <summary>
		/// 判断是否是注解标签的开始.注解标签的定义: &lt;!--vt[.....]--&gt;
		/// </summary>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		// Token: 0x060001FC RID: 508 RVA: 0x00009654 File Offset: 0x00007854
		internal static bool IsCommentTagStart(string text, int offset)
		{
			return ParserHelper.ReadChar(text, offset) == '<' && ParserHelper.ReadChar(text, offset + 1) == '!' && ParserHelper.ReadChar(text, offset + 2) == '-' && ParserHelper.ReadChar(text, offset + 3) == '-' && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 4), 'v', 'V') && ParserHelper.IsChars(ParserHelper.ReadChar(text, offset + 5), 't', 'T') && ParserHelper.ReadChar(text, offset + 6) == '[';
		}

		/// <summary>
		/// 解析元素的属性列表
		/// </summary>
		/// <param name="element"></param>
		/// <param name="match"></param>
		// Token: 0x060001FD RID: 509 RVA: 0x000096CC File Offset: 0x000078CC
		internal static void ParseElementAttributes(IAttributesElement element, Match match)
		{
			CaptureCollection captures = match.Groups["attrname"].Captures;
			CaptureCollection captures2 = match.Groups["attrval"].Captures;
			for (int i = 0; i < captures.Count; i++)
			{
				string value = captures[i].Value;
				string text = HttpUtility.HtmlDecode(captures2[i].ToString());
				if (!string.IsNullOrEmpty(value))
				{
					element.Attributes.Add(value, text);
				}
			}
		}

		/// <summary>
		/// 构建文本节点元素
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="container">标签的容器</param>
		/// <param name="text"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		// Token: 0x060001FE RID: 510 RVA: 0x0000974C File Offset: 0x0000794C
		internal static void CreateTextNode(Template ownerTemplate, Tag container, string text, int offset, int length)
		{
			if (length > 0)
			{
				string text2 = text.Substring(offset, length);
				if (ownerTemplate.OwnerDocument.DocumentConfig != null && ownerTemplate.OwnerDocument.DocumentConfig.CompressText)
				{
					text2 = Utility.CompressText(text2);
				}
				if (text2.Length > 0 && text2.TrimStart(new char[]
				{
					'\r',
					'\n',
					'\t'
				}).Length != 0)
				{
					container.AppendChild(new TextNode(ownerTemplate, text2));
				}
			}
		}

		/// <summary>
		/// 从匹配项中建构建变量实例
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="match"></param>
		/// <param name="prefix"></param>
		/// <returns></returns>
		// Token: 0x060001FF RID: 511 RVA: 0x000097C8 File Offset: 0x000079C8
		internal static Variable CreateVariable(Template ownerTemplate, Match match, out string prefix)
		{
			prefix = (match.Groups["prefix"].Success ? match.Groups["prefix"].Value : null);
			string value = match.Groups["name"].Value;
			ownerTemplate = Utility.GetOwnerTemplateByPrefix(ownerTemplate, prefix);
			if (ownerTemplate == null)
			{
				throw new ParserException(string.Format("变量的宿主模板#{0}不存在", prefix));
			}
			return Utility.GetVariableOrAddNew(ownerTemplate, value);
		}

		/// <summary>
		/// 从文本(如#.name或name)中构建变量标识对象
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		// Token: 0x06000200 RID: 512 RVA: 0x00009844 File Offset: 0x00007A44
		internal static VariableIdentity CreateVariableIdentity(Template ownerTemplate, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			Match match = ParserRegex.VarIdRegex.Match(text);
			if (match.Success)
			{
				string prefix;
				Variable variable = ParserHelper.CreateVariable(ownerTemplate, match, out prefix);
				return new VariableIdentity(ownerTemplate, variable, prefix);
			}
			throw new ParserException(string.Format("变量标识\"{0}\"的定义格式错误", text));
		}

		/// <summary>
		/// 构建变量的字段列表
		/// </summary>
		/// <param name="variableId"></param>
		/// <param name="match"></param>
		/// <param name="needCacheData"></param>
		/// <returns></returns>
		// Token: 0x06000201 RID: 513 RVA: 0x00009894 File Offset: 0x00007A94
		internal static VariableExpression CreateVariableExpression(VariableIdentity variableId, Match match, bool needCacheData)
		{
			VariableExpression variableExpression = new VariableExpression(variableId, needCacheData);
			CaptureCollection captures = match.Groups["field"].Captures;
			CaptureCollection captures2 = match.Groups["method"].Captures;
			VariableExpression parentExp = variableExpression;
			for (int i = 0; i < captures.Count; i++)
			{
				string value = captures[i].Value;
				VariableExpression variableExpression2 = new VariableExpression(parentExp, value, !string.IsNullOrEmpty(captures2[i].Value));
				parentExp = variableExpression2;
			}
			return variableExpression;
		}

		/// <summary>
		/// 从变量表达式文本(如:name.age)中构建变量表达式
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="expressionText"></param>
		/// <param name="needCacheData"></param>
		/// <returns></returns>
		// Token: 0x06000202 RID: 514 RVA: 0x00009920 File Offset: 0x00007B20
		internal static VariableExpression CreateVariableExpression(Template ownerTemplate, string expressionText, bool needCacheData)
		{
			if (string.IsNullOrEmpty(expressionText))
			{
				return null;
			}
			Match match = ParserRegex.VarExpRegex.Match(expressionText);
			if (match.Success)
			{
				string prefix;
				Variable variable = ParserHelper.CreateVariable(ownerTemplate, match, out prefix);
				VariableIdentity variableId = new VariableIdentity(ownerTemplate, variable, prefix);
				return ParserHelper.CreateVariableExpression(variableId, match, needCacheData);
			}
			throw new ParserException(string.Format("变量表达式\"{0}\"的定义格式错误", expressionText));
		}

		/// <summary>
		/// 从表达式文本中构造表达式.如果表达式是以$字符开头.并且不是以$$字符开头.则认为是变量表达式.否则则认为是常量表达式
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="expressionText"></param>
		/// <returns></returns>
		// Token: 0x06000203 RID: 515 RVA: 0x00009978 File Offset: 0x00007B78
		internal static IExpression CreateExpression(Template ownerTemplate, string expressionText)
		{
			if (string.IsNullOrEmpty(expressionText))
			{
				return new ConstantExpression(expressionText);
			}
			if (expressionText.StartsWith("$") && !"$=".Equals(expressionText))
			{
				expressionText = expressionText.Remove(0, 1);
				if (expressionText.StartsWith("$"))
				{
					return new ConstantExpression(expressionText);
				}
				return ParserHelper.CreateVariableExpression(ownerTemplate, expressionText, false);
			}
			else
			{
				if (!Utility.IsInteger(expressionText))
				{
					return new ConstantExpression(expressionText);
				}
				return new ConstantExpression(Utility.ConverToInt32(expressionText));
			}
		}

		/// <summary>
		/// 构建变量标签元素
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="container">标签的容器</param>
		/// <param name="match"></param>
		// Token: 0x06000204 RID: 516 RVA: 0x000099F4 File Offset: 0x00007BF4
		internal static VariableTag CreateVariableTag(Template ownerTemplate, Tag container, Match match)
		{
			string prefix;
			Variable variable = ParserHelper.CreateVariable(ownerTemplate, match, out prefix);
			VariableIdentity variableId = new VariableIdentity(ownerTemplate, variable, prefix);
			VariableExpression varExp = ParserHelper.CreateVariableExpression(variableId, match, true);
			VariableTag variableTag = new VariableTag(ownerTemplate, varExp);
			ParserHelper.ParseElementAttributes(variableTag, match);
			container.AppendChild(variableTag);
			return variableTag;
		}

		/// <summary>
		/// 构建标签元素
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="match"></param>
		/// <param name="isClosedTag">是否是自闭合标签</param>
		/// <returns></returns>
		// Token: 0x06000205 RID: 517 RVA: 0x00009A38 File Offset: 0x00007C38
		internal static Tag CreateTag(Template ownerTemplate, Match match, out bool isClosedTag)
		{
			string value = match.Groups["tagname"].Value;
			isClosedTag = match.Groups["closed"].Success;
			Tag tag = TagFactory.FromTagName(ownerTemplate, value);
			if (tag == null)
			{
				throw new ParserException(string.Format("不能识别的元素标签\"{0}\"", value));
			}
			ParserHelper.ParseElementAttributes(tag, match);
			return tag;
		}

		/// <summary>
		/// 注释标签的起始标记
		/// </summary>
		// Token: 0x04000065 RID: 101
		public const string CommentTagStart = "<!--vt[";

		/// <summary>
		/// 注释标签的结束标记
		/// </summary>
		// Token: 0x04000066 RID: 102
		public const string CommentTagEnd = "]-->";

		/// <summary>
		/// VT表达头的标记
		/// </summary>
		// Token: 0x04000067 RID: 103
		public const string VTExpressionHead = "vt=";
	}
}
