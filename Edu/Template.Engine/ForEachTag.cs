using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// ForEach标签.如:&lt;vt:foreach from="collection" item="variable"  index="i"&gt;...&lt;/vt:foreach&gt;
	/// </summary>
	// Token: 0x0200003A RID: 58
	public class ForEachTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x060002A0 RID: 672 RVA: 0x0000BBF0 File Offset: 0x00009DF0
		internal ForEachTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000BC04 File Offset: 0x00009E04
		internal ForEachTag(Template ownerTemplate, string endTag) : base(ownerTemplate)
		{
			this._EndTagName = endTag;
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000BC1F File Offset: 0x00009E1F
		public override string TagName
		{
			get
			{
				return "foreach";
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000BC26 File Offset: 0x00009E26
		public override string EndTagName
		{
			get
			{
				if (!string.IsNullOrEmpty(this._EndTagName))
				{
					return this._EndTagName;
				}
				return "foreach";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000BC41 File Offset: 0x00009E41
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 根据Id获取某个子元素标签
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		// Token: 0x060002A5 RID: 677 RVA: 0x0000BC44 File Offset: 0x00009E44
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

		/// <summary>
		/// 根据name获取所有同名的子元素标签
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		// Token: 0x060002A6 RID: 678 RVA: 0x0000BC90 File Offset: 0x00009E90
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

		/// <summary>
		/// 根据标签名获取所有同标签名的子元素标签
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		// Token: 0x060002A7 RID: 679 RVA: 0x0000BCE0 File Offset: 0x00009EE0
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

		/// <summary>
		/// 来源数据的变量
		/// </summary>
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000BD30 File Offset: 0x00009F30
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x0000BD38 File Offset: 0x00009F38
		public VariableExpression From { get; protected set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000BD41 File Offset: 0x00009F41
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000BD49 File Offset: 0x00009F49
		public object DataSourse { get; set; }

		/// <summary>
		/// 当前项变量
		/// </summary>
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000BD52 File Offset: 0x00009F52
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000BD5A File Offset: 0x00009F5A
		public VariableIdentity Item { get; protected set; }

		/// <summary>
		/// 索引变量
		/// </summary>
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000BD63 File Offset: 0x00009F63
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000BD6B File Offset: 0x00009F6B
		public VariableIdentity Index { get; protected set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000BD74 File Offset: 0x00009F74
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000BD7C File Offset: 0x00009F7C
		public int Count { get; protected set; }

		/// <summary>
		/// 分组大小
		/// </summary>
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000BD85 File Offset: 0x00009F85
		public Attribute GroupSize
		{
			get
			{
				return base.Attributes["GroupSize"];
			}
		}

		/// <summary>
		/// ForEachElse节点
		/// </summary>
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000BD97 File Offset: 0x00009F97
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000BD9F File Offset: 0x00009F9F
		public ForEachElseTag Else
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

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x060002B5 RID: 693 RVA: 0x0000BDB4 File Offset: 0x00009FB4
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
				if (name == "item")
				{
					this.Item = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
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

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x060002B6 RID: 694 RVA: 0x0000BE8C File Offset: 0x0000A08C
		protected override void RenderTagData(TextWriter writer)
		{
			IEnumerable enumerable = null;
			if (this.From != null)
			{
				enumerable = Utility.GetResolvedDataSource(this.From.GetValue());
			}
			if (this.DataSourse != null)
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
		// Token: 0x060002B7 RID: 695 RVA: 0x0000BFD5 File Offset: 0x0000A1D5
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060002B8 RID: 696 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		internal override Element Clone(Template ownerTemplate)
		{
			ForEachTag forEachTag = new ForEachTag(ownerTemplate);
			this.CopyTo(forEachTag);
			forEachTag.Else = ((this.Else == null) ? null : ((ForEachElseTag)this.Else.Clone(ownerTemplate)));
			forEachTag.From = ((this.From == null) ? null : ((VariableExpression)this.From.Clone(ownerTemplate)));
			forEachTag.Index = ((this.Index == null) ? null : this.Index.Clone(ownerTemplate));
			forEachTag.Item = ((this.Item == null) ? null : this.Item.Clone(ownerTemplate));
			forEachTag.Count = this.Count;
			return forEachTag;
		}

		// Token: 0x0400009E RID: 158
		private string _EndTagName = string.Empty;

		/// <summary>
		/// ForEachElse节点
		/// </summary>
		// Token: 0x0400009F RID: 159
		private ForEachElseTag _Else;
	}
}
