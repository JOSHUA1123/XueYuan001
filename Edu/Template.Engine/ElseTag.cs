using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// Else标签..只适用于if标签内.如&lt;vt:if var="member.age" value="20" compare="&lt;="&gt;..&lt;vt:else&gt;..&lt;/vt:if&gt;
	/// </summary>
	// Token: 0x02000035 RID: 53
	public class ElseTag : IfConditionTag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x0600027F RID: 639 RVA: 0x0000B2F5 File Offset: 0x000094F5
		internal ElseTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000B2FE File Offset: 0x000094FE
		public override string TagName
		{
			get
			{
				return "else";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000B305 File Offset: 0x00009505
		internal override bool IsSingleTag
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// else节点不支持比较值
		/// </summary>
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000B308 File Offset: 0x00009508
		public override ElementCollection<IExpression> Values
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// else节点不支持条件变量
		/// </summary>
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000B30B File Offset: 0x0000950B
		public override VariableExpression VarExpression
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 永远返回true
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000284 RID: 644 RVA: 0x0000B30E File Offset: 0x0000950E
		internal override bool IsTestSuccess()
		{
			return true;
		}

		/// <summary>
		/// 解析标签数据
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="container">标签的容器</param>
		/// <param name="tagStack">标签堆栈</param>
		/// <param name="text"></param>
		/// <param name="match"></param>
		/// <param name="isClosedTag">是否闭合标签</param>
		/// <returns>如果需要继续处理EndTag则返回true.否则请返回false</returns>
		// Token: 0x06000285 RID: 645 RVA: 0x0000B314 File Offset: 0x00009514
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (!(container is IfTag))
			{
				throw new ParserException(string.Format("未找到和{0}标签对应的{1}标签", this.TagName, this.EndTagName));
			}
			IfTag ifTag = (IfTag)container;
			if (ifTag.Else != null)
			{
				throw new ParserException(string.Format("{0}标签不能定义多个{1}标签", this.EndTagName, this.TagName));
			}
			ifTag.Else = this;
			return true;
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x06000286 RID: 646 RVA: 0x0000B378 File Offset: 0x00009578
		internal override Element Clone(Template ownerTemplate)
		{
			ElseTag elseTag = new ElseTag(ownerTemplate);
			base.CopyTo(elseTag);
			return elseTag;
		}
	}
}
