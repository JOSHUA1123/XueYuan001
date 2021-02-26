using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	// Token: 0x02000005 RID: 5
	public class ListTag : Tag
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002C54 File Offset: 0x00000E54
		internal ListTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002C5D File Offset: 0x00000E5D
		public override string TagName
		{
			get
			{
				return "list";
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002C64 File Offset: 0x00000E64
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C68 File Offset: 0x00000E68
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

		// Token: 0x06000031 RID: 49 RVA: 0x00002CB4 File Offset: 0x00000EB4
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

		// Token: 0x06000032 RID: 50 RVA: 0x00002D04 File Offset: 0x00000F04
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002D54 File Offset: 0x00000F54
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002D5C File Offset: 0x00000F5C
		public VariableExpression From { get; protected set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002D65 File Offset: 0x00000F65
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002D6D File Offset: 0x00000F6D
		public object DataSourse { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002D76 File Offset: 0x00000F76
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002D7E File Offset: 0x00000F7E
		public VariableIdentity Item { get; protected set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002D87 File Offset: 0x00000F87
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00002D8F File Offset: 0x00000F8F
		public VariableIdentity Variable { get; protected set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002D98 File Offset: 0x00000F98
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public VariableIdentity Index { get; protected set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002DA9 File Offset: 0x00000FA9
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002DB1 File Offset: 0x00000FB1
		public VariableExpression Sort { get; protected set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002DBA File Offset: 0x00000FBA
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002DC2 File Offset: 0x00000FC2
		public int Count { get; protected set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002DCB File Offset: 0x00000FCB
		public Attribute GroupSize
		{
			get
			{
				return base.Attributes["GroupSize"];
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002DDD File Offset: 0x00000FDD
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002DE5 File Offset: 0x00000FE5
		public ListElseTag Else
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

		// Token: 0x06000044 RID: 68 RVA: 0x00002DF8 File Offset: 0x00000FF8
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

		// Token: 0x06000045 RID: 69 RVA: 0x00002F4C File Offset: 0x0000114C
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

		// Token: 0x06000046 RID: 70 RVA: 0x000030B1 File Offset: 0x000012B1
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000030C4 File Offset: 0x000012C4
		internal override Element Clone(Template ownerTemplate)
		{
			ListTag listTag = new ListTag(ownerTemplate);
			this.CopyTo(listTag);
			listTag.Else = ((this.Else == null) ? null : ((ListElseTag)this.Else.Clone(ownerTemplate)));
			listTag.From = ((this.From == null) ? null : ((VariableExpression)this.From.Clone(ownerTemplate)));
			listTag.Sort = ((this.Sort == null) ? null : ((VariableExpression)this.Sort.Clone(ownerTemplate)));
			listTag.Index = ((this.Index == null) ? null : this.Index.Clone(ownerTemplate));
			listTag.Item = ((this.Item == null) ? null : this.Item.Clone(ownerTemplate));
			listTag.Count = this.Count;
			listTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			return listTag;
		}

		// Token: 0x0400000A RID: 10
		private ListElseTag _Else;
	}
}
