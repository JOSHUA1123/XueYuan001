using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	// Token: 0x0200001A RID: 26
	public class DetailElseTag : Tag
	{
		// Token: 0x06000126 RID: 294 RVA: 0x000063CF File Offset: 0x000045CF
		internal DetailElseTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000063D8 File Offset: 0x000045D8
		public override string TagName
		{
			get
			{
				return "detailelse";
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000063DF File Offset: 0x000045DF
		public override string EndTagName
		{
			get
			{
				return "detail";
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000063E6 File Offset: 0x000045E6
		internal override bool IsSingleTag
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000063EC File Offset: 0x000045EC
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (!(container is DetailTag))
			{
				throw new ParserException(string.Format("未找到和{0}标签对应的{1}标签", this.TagName, this.EndTagName));
			}
			DetailTag detailTag = (DetailTag)container;
			if (detailTag.Else != null)
			{
				throw new ParserException(string.Format("{0}标签不能定义多个{1}标签", this.EndTagName, this.TagName));
			}
			detailTag.Else = this;
			return true;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006450 File Offset: 0x00004650
		internal override Element Clone(Template ownerTemplate)
		{
			DetailElseTag detailElseTag = new DetailElseTag(ownerTemplate);
			this.CopyTo(detailElseTag);
			return detailElseTag;
		}
	}
}
