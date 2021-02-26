using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	// Token: 0x02000009 RID: 9
	public class RepeatTag : Tag
	{
		// Token: 0x06000063 RID: 99 RVA: 0x000035BA File Offset: 0x000017BA
		internal RepeatTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000035C3 File Offset: 0x000017C3
		public override string TagName
		{
			get
			{
				return "repeat";
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000035CA File Offset: 0x000017CA
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000035D0 File Offset: 0x000017D0
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

		// Token: 0x06000067 RID: 103 RVA: 0x0000361C File Offset: 0x0000181C
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

		// Token: 0x06000068 RID: 104 RVA: 0x0000366C File Offset: 0x0000186C
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000036BC File Offset: 0x000018BC
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000036C4 File Offset: 0x000018C4
		public VariableExpression From { get; protected set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000036CD File Offset: 0x000018CD
		// (set) Token: 0x0600006C RID: 108 RVA: 0x000036D5 File Offset: 0x000018D5
		public object DataSourse { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000036DE File Offset: 0x000018DE
		// (set) Token: 0x0600006E RID: 110 RVA: 0x000036E6 File Offset: 0x000018E6
		public VariableIdentity Item { get; protected set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000036EF File Offset: 0x000018EF
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000036F7 File Offset: 0x000018F7
		public VariableIdentity Variable { get; protected set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003700 File Offset: 0x00001900
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00003708 File Offset: 0x00001908
		public VariableIdentity Index { get; protected set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003711 File Offset: 0x00001911
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00003719 File Offset: 0x00001919
		public VariableExpression Sort { get; protected set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003722 File Offset: 0x00001922
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000372A File Offset: 0x0000192A
		public int Count { get; protected set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003733 File Offset: 0x00001933
		public Attribute GroupSize
		{
			get
			{
				return base.Attributes["GroupSize"];
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003745 File Offset: 0x00001945
		// (set) Token: 0x06000079 RID: 121 RVA: 0x0000374D File Offset: 0x0000194D
		public RepeatElseTag Else
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

		// Token: 0x0600007A RID: 122 RVA: 0x00003760 File Offset: 0x00001960
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
				if (name == "sort")
				{
					VariableExpression variableExpression2 = item.Value as VariableExpression;
					if (variableExpression2 == null && this.OwnerDocument.DocumentConfig.CompatibleMode)
					{
						variableExpression2 = ParserHelper.CreateVariableExpression(base.OwnerTemplate, item.Text, false);
					}
					this.Sort = variableExpression2;
					return;
				}
				if (name == "item")
				{
					this.Item = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
					return;
				}
				if (name == "var")
				{
					this.Variable = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
					return;
				}
				if (name == "index")
				{
					this.Index = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
					return;
				}
				if (!(name == "count"))
				{
					return;
				}
				int num = 0;
				int.TryParse(item.Text, out num);
				this.Count = ((num <= 0) ? int.MaxValue : num);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000038B4 File Offset: 0x00001AB4
		protected override void RenderTagData(TextWriter writer)
		{
			IEnumerable enumerable = null;
			if (this.From != null)
			{
				enumerable = Utility.GetResolvedDataSource(this.From.GetValue());
			}
			if (this.From == null && this.DataSourse != null)
			{
				enumerable = Utility.GetResolvedDataSource(this.DataSourse);
			}
			int num = 0;
			LoopIndex loopIndex = new LoopIndex(0m);
			if (this.Index != null)
			{
				this.Index.Value = loopIndex;
			}
			if (enumerable != null)
			{
				if (this.Variable != null)
				{
					this.Variable.Value = enumerable;
				}
				IEnumerator enumerator = enumerable.GetEnumerator();
				int num2 = (this.GroupSize == null) ? 0 : Utility.ConverToInt32(this.GroupSize.GetTextValue());
				if (num2 > 1)
				{
					enumerator = Utility.SplitToGroup(enumerator, num2);
				}
				enumerator.Reset();
				int num3 = (this.Count > 0) ? this.Count : int.MaxValue;
				if (enumerator.MoveNext())
				{
					loopIndex.IsLast = false;
					while (!loopIndex.IsLast && num < num3)
					{
						object value = enumerator.Current;
						loopIndex.Value = ++num;
						loopIndex.IsFirst = (num == 1);
						loopIndex.IsLast = !enumerator.MoveNext();
						if (this.Index != null)
						{
							this.Index.Reset();
						}
						if (this.Item != null)
						{
							this.Item.Value = value;
						}
						base.RenderTagData(writer);
					}
				}
			}
			if (num == 0 && this.Else != null)
			{
				this.Else.Render(writer);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003A19 File Offset: 0x00001C19
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003A2C File Offset: 0x00001C2C
		internal override Element Clone(Template ownerTemplate)
		{
			RepeatTag repeatTag = new RepeatTag(ownerTemplate);
			this.CopyTo(repeatTag);
			repeatTag.Else = ((this.Else == null) ? null : ((RepeatElseTag)this.Else.Clone(ownerTemplate)));
			repeatTag.From = ((this.From == null) ? null : ((VariableExpression)this.From.Clone(ownerTemplate)));
			repeatTag.Sort = ((this.Sort == null) ? null : ((VariableExpression)this.Sort.Clone(ownerTemplate)));
			repeatTag.Index = ((this.Index == null) ? null : this.Index.Clone(ownerTemplate));
			repeatTag.Item = ((this.Item == null) ? null : this.Item.Clone(ownerTemplate));
			repeatTag.Count = this.Count;
			repeatTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			return repeatTag;
		}

		// Token: 0x04000015 RID: 21
		private RepeatElseTag _Else;
	}
}
