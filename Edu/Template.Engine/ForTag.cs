using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// For标签.如:&lt;vt:for from="1" to="100" step="1" index="i"&gt;...&lt;/vt:for&gt;
	/// </summary>
	// Token: 0x0200002B RID: 43
	public class ForTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x0600021A RID: 538 RVA: 0x00009FCD File Offset: 0x000081CD
		internal ForTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00009FD6 File Offset: 0x000081D6
		public override string TagName
		{
			get
			{
				return "for";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00009FDD File Offset: 0x000081DD
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 起始值
		/// </summary>
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00009FE0 File Offset: 0x000081E0
		public Attribute From
		{
			get
			{
				return base.Attributes["From"];
			}
		}

		/// <summary>
		/// 结束值
		/// </summary>
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00009FF2 File Offset: 0x000081F2
		public Attribute To
		{
			get
			{
				return base.Attributes["To"];
			}
		}

		/// <summary>
		/// 步长
		/// </summary>
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000A004 File Offset: 0x00008204
		public Attribute Step
		{
			get
			{
				return base.Attributes["Step"];
			}
		}

		/// <summary>
		/// 索引变量
		/// </summary>
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000A016 File Offset: 0x00008216
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000A01E File Offset: 0x0000821E
		public VariableIdentity Index { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x06000222 RID: 546 RVA: 0x0000A028 File Offset: 0x00008228
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (!(name == "index"))
				{
					return;
				}
				this.Index = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x06000223 RID: 547 RVA: 0x0000A060 File Offset: 0x00008260
		protected override void RenderTagData(TextWriter writer)
		{
			decimal num = Utility.ConverToDecimal(this.From.Value.GetValue());
			decimal num2 = (this.Step == null) ? 1m : Utility.ConverToDecimal(this.Step.Value.GetValue());
			decimal d = Utility.ConverToDecimal(this.To.Value.GetValue());
			decimal num3 = num;
			LoopIndex loopIndex = new LoopIndex(num3);
			if (this.Index != null)
			{
				this.Index.Value = loopIndex;
			}
			if (num2 >= 0m)
			{
				while (num3 <= d)
				{
					loopIndex.Value = num3;
					loopIndex.IsFirst = (num3 == num);
					loopIndex.IsLast = (num3 == d);
					if (this.Index != null)
					{
						this.Index.Variable.Reset();
					}
					base.RenderTagData(writer);
					num3 += num2;
				}
				return;
			}
			while (num3 >= d)
			{
				loopIndex.Value = num3;
				loopIndex.IsFirst = (num3 == num);
				loopIndex.IsLast = (num3 == d);
				if (this.Index != null)
				{
					this.Index.Variable.Reset();
				}
				base.RenderTagData(writer);
				num3 += num2;
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
		// Token: 0x06000224 RID: 548 RVA: 0x0000A19C File Offset: 0x0000839C
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.From == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少from属性", this.TagName));
			}
			if (this.To == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少to属性", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x06000225 RID: 549 RVA: 0x0000A1F4 File Offset: 0x000083F4
		internal override Element Clone(Template ownerTemplate)
		{
			ForTag forTag = new ForTag(ownerTemplate);
			base.CopyTo(forTag);
			forTag.Index = ((this.Index == null) ? null : this.Index.Clone(ownerTemplate));
			return forTag;
		}
	}
}
