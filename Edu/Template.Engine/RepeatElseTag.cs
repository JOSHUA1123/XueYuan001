using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	// Token: 0x02000039 RID: 57
	public class RepeatElseTag : Tag
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000BB56 File Offset: 0x00009D56
		internal RepeatElseTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000BB5F File Offset: 0x00009D5F
		public override string TagName
		{
			get
			{
				return "repeatelse";
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000BB66 File Offset: 0x00009D66
		public override string EndTagName
		{
			get
			{
				return "repeat";
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000BB6D File Offset: 0x00009D6D
		internal override bool IsSingleTag
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000BB70 File Offset: 0x00009D70
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (!(container is RepeatTag))
			{
				throw new ParserException(string.Format("未找到和{0}标签对应的{1}标签", this.TagName, this.EndTagName));
			}
			RepeatTag repeatTag = (RepeatTag)container;
			if (repeatTag.Else != null)
			{
				throw new ParserException(string.Format("{0}标签不能定义多个{1}标签", this.EndTagName, this.TagName));
			}
			repeatTag.Else = this;
			return true;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000BBD4 File Offset: 0x00009DD4
		internal override Element Clone(Template ownerTemplate)
		{
			RepeatElseTag repeatElseTag = new RepeatElseTag(ownerTemplate);
			this.CopyTo(repeatElseTag);
			return repeatElseTag;
		}
	}
}
