using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	// Token: 0x02000036 RID: 54
	public class ListElseTag : Tag
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000B394 File Offset: 0x00009594
		internal ListElseTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000B39D File Offset: 0x0000959D
		public override string TagName
		{
			get
			{
				return "listelse";
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000B3A4 File Offset: 0x000095A4
		public override string EndTagName
		{
			get
			{
				return "list";
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000B3AB File Offset: 0x000095AB
		internal override bool IsSingleTag
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000B3B0 File Offset: 0x000095B0
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (!(container is ListTag))
			{
				throw new ParserException(string.Format("未找到和{0}标签对应的{1}标签", this.TagName, this.EndTagName));
			}
			ListTag listTag = (ListTag)container;
			if (listTag.Else != null)
			{
				throw new ParserException(string.Format("{0}标签不能定义多个{1}标签", this.EndTagName, this.TagName));
			}
			listTag.Else = this;
			return true;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000B414 File Offset: 0x00009614
		internal override Element Clone(Template ownerTemplate)
		{
			ListElseTag listElseTag = new ListElseTag(ownerTemplate);
			this.CopyTo(listElseTag);
			return listElseTag;
		}
	}
}
