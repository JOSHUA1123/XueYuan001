using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// 变量赋值标签, 如:&lt;vt:set var="page" value="1" /&gt;
	/// </summary>
	// Token: 0x02000022 RID: 34
	public class SetTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x060001BD RID: 445 RVA: 0x00008A94 File Offset: 0x00006C94
		internal SetTag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.Values = new ElementCollection<IExpression>();
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00008AA8 File Offset: 0x00006CA8
		public override string TagName
		{
			get
			{
				return "set";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00008AAF File Offset: 0x00006CAF
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 变量的值
		/// </summary>
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00008AB2 File Offset: 0x00006CB2
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00008ABA File Offset: 0x00006CBA
		public ElementCollection<IExpression> Values { get; protected set; }

		/// <summary>
		/// 要对其赋值的变量
		/// </summary>
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00008AC3 File Offset: 0x00006CC3
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00008ACB File Offset: 0x00006CCB
		public VariableIdentity Variable { get; protected set; }

		/// <summary>
		/// 格式化
		/// </summary>
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public Attribute Format
		{
			get
			{
				return base.Attributes["Format"];
			}
		}

		/// <summary>
		/// 是否输出此标签的结果值
		/// </summary>
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00008AE6 File Offset: 0x00006CE6
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00008AEE File Offset: 0x00006CEE
		public bool Output { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x060001C7 RID: 455 RVA: 0x00008AF8 File Offset: 0x00006CF8
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "value")
				{
					this.Values.Add(item.Value);
					return;
				}
				if (name == "var")
				{
					this.Variable = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
					return;
				}
				if (!(name == "output"))
				{
					return;
				}
				this.Output = Utility.ConverToBoolean(item.Text);
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x060001C8 RID: 456 RVA: 0x00008B70 File Offset: 0x00006D70
		protected override void RenderTagData(TextWriter writer)
		{
			string text = (this.Format == null) ? string.Empty : this.Format.GetTextValue();
			object obj;
			if (string.IsNullOrEmpty(text))
			{
				obj = this.Values[0].GetValue();
			}
			else
			{
				List<object> list = new List<object>();
				foreach (IExpression expression in this.Values)
				{
					list.Add(expression.GetValue());
				}
				obj = string.Format(text, list.ToArray());
			}
			if (this.Variable != null)
			{
				this.Variable.Value = obj;
			}
			if (this.Output && obj != null)
			{
				writer.Write(obj);
			}
			base.RenderTagData(writer);
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
		// Token: 0x060001C9 RID: 457 RVA: 0x00008C40 File Offset: 0x00006E40
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Variable == null && !this.Output)
			{
				throw new ParserException(string.Format("{0}标签中如果未定义Output属性为true则必须定义var属性", this.TagName));
			}
			if (this.Values.Count < 1)
			{
				throw new ParserException(string.Format("{0}标签中缺少value属性", this.TagName));
			}
			if (this.Values.Count > 1 && this.Format == null)
			{
				throw new ParserException(string.Format("{0}标签如果已定义多个value属性,则也必须定义format属性", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060001CA RID: 458 RVA: 0x00008CD4 File Offset: 0x00006ED4
		internal override Element Clone(Template ownerTemplate)
		{
			SetTag setTag = new SetTag(ownerTemplate);
			this.CopyTo(setTag);
			setTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			setTag.Output = this.Output;
			foreach (IExpression expression in this.Values)
			{
				setTag.Values.Add(expression.Clone(ownerTemplate));
			}
			return setTag;
		}
	}
}
