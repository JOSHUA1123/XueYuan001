using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	// Token: 0x0200002D RID: 45
	public class DetailTag : Tag
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000A4E0 File Offset: 0x000086E0
		internal DetailTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000A4E9 File Offset: 0x000086E9
		public override string TagName
		{
			get
			{
				return "detail";
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000A4F0 File Offset: 0x000086F0
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000A4F4 File Offset: 0x000086F4
		public override Tag GetChildTagById(string id)
		{
			Tag tag = base.GetChildTagById(id);
			if (tag == null && this.Else != null)
			{
				if (id.Equals(this.Else.Id, StringComparison.InvariantCultureIgnoreCase))
				{
					tag = this.Else;
				}
				else
				{
					tag = this.Else.GetChildTagById(id);
				}
			}
			return tag;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000A540 File Offset: 0x00008740
		public override ElementCollection<Tag> GetChildTagsByName(string name)
		{
			ElementCollection<Tag> childTagsByName = base.GetChildTagsByName(name);
			if (this.Else != null)
			{
				if (name.Equals(this.Else.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					childTagsByName.Add(this.Else);
				}
				childTagsByName.AddRange(this.Else.GetChildTagsByName(name));
			}
			return childTagsByName;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000A590 File Offset: 0x00008790
		public override ElementCollection<Tag> GetChildTagsByTagName(string tagName)
		{
			ElementCollection<Tag> childTagsByTagName = base.GetChildTagsByTagName(tagName);
			if (this.Else != null)
			{
				if (tagName.Equals(this.Else.TagName, StringComparison.InvariantCultureIgnoreCase))
				{
					childTagsByTagName.Add(this.Else);
				}
				childTagsByTagName.AddRange(this.Else.GetChildTagsByTagName(tagName));
			}
			return childTagsByTagName;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000A5E0 File Offset: 0x000087E0
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000A5E8 File Offset: 0x000087E8
		public VariableExpression From { get; protected set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000A5F1 File Offset: 0x000087F1
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000A5F9 File Offset: 0x000087F9
		public object DataSourse { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000A602 File Offset: 0x00008802
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000A60A File Offset: 0x0000880A
		public VariableIdentity Item { get; protected set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000A613 File Offset: 0x00008813
		public Attribute GroupSize
		{
			get
			{
				return base.Attributes["GroupSize"];
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000A625 File Offset: 0x00008825
		// (set) Token: 0x06000242 RID: 578 RVA: 0x0000A62D File Offset: 0x0000882D
		public DetailElseTag Else
		{
			get
			{
				return this._Else;
			}
			internal set
			{
				if (value != null)
				{
					value.Parent = this;
				}
				this._Else = value;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000A640 File Offset: 0x00008840
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "from")
				{
					VariableExpression variableExpression = item.Value as VariableExpression;
					if (variableExpression == null && this.OwnerDocument.DocumentConfig.CompatibleMode)
					{
						variableExpression = ParserHelper.CreateVariableExpression(base.OwnerTemplate, item.Text, false);
					}
					this.From = variableExpression;
					return;
				}
				if (!(name == "item"))
				{
					return;
				}
				this.Item = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000A6C0 File Offset: 0x000088C0
		protected override void RenderTagData(TextWriter writer)
		{
			if (this.Item != null)
			{
				if (this.From != null)
				{
					this.Item.Value = this.From.GetValue();
				}
				if (this.From == null && this.DataSourse != null)
				{
					this.Item.Value = this.DataSourse;
				}
			}
			if (this.Item.Value != null)
			{
				base.RenderTagData(writer);
			}
			if (this.Item.Value == null && this.Else != null)
			{
				this.Else.Render(writer);
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000A749 File Offset: 0x00008949
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000A75C File Offset: 0x0000895C
		internal override Element Clone(Template ownerTemplate)
		{
			DetailTag detailTag = new DetailTag(ownerTemplate);
			this.CopyTo(detailTag);
			detailTag.Else = ((this.Else == null) ? null : ((DetailElseTag)this.Else.Clone(ownerTemplate)));
			detailTag.From = ((this.From == null) ? null : ((VariableExpression)this.From.Clone(ownerTemplate)));
			detailTag.Item = ((this.Item == null) ? null : this.Item.Clone(ownerTemplate));
			return detailTag;
		}

		// Token: 0x04000070 RID: 112
		private DetailElseTag _Else;
	}
}
