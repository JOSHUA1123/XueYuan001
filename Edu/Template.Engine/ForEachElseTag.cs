using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// ForEachElse标签.如:&lt;vt:foreachelse&gt;...&lt;/vt:foreach&gt;
	/// </summary>
	// Token: 0x02000024 RID: 36
	public class ForEachElseTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x060001DC RID: 476 RVA: 0x000091AC File Offset: 0x000073AC
		internal ForEachElseTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000091B5 File Offset: 0x000073B5
		public override string TagName
		{
			get
			{
				return "foreachelse";
			}
		}

		/// <summary>
		/// 返回标签的结束标签名称.
		/// </summary>
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000091BC File Offset: 0x000073BC
		public override string EndTagName
		{
			get
			{
				return "foreach";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000091C3 File Offset: 0x000073C3
		internal override bool IsSingleTag
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// 开始解析标签数据
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="container">标签的容器</param>
		/// <param name="tagStack">标签堆栈</param>
		/// <param name="text"></param>
		/// <param name="match"></param>
		/// <param name="isClosedTag">是否闭合标签</param>
		/// <returns>如果需要继续处理EndTag则返回true.否则请返回false</returns>
		// Token: 0x060001E0 RID: 480 RVA: 0x000091C8 File Offset: 0x000073C8
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (!(container is ForEachTag))
			{
				throw new ParserException(string.Format("未找到和{0}标签对应的{1}标签", this.TagName, this.EndTagName));
			}
			ForEachTag forEachTag = (ForEachTag)container;
			if (forEachTag.Else != null)
			{
				throw new ParserException(string.Format("{0}标签不能定义多个{1}标签", this.EndTagName, this.TagName));
			}
			forEachTag.Else = this;
			return true;
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060001E1 RID: 481 RVA: 0x0000922C File Offset: 0x0000742C
		internal override Element Clone(Template ownerTemplate)
		{
			ForEachElseTag forEachElseTag = new ForEachElseTag(ownerTemplate);
			this.CopyTo(forEachElseTag);
			return forEachElseTag;
		}
	}
}
