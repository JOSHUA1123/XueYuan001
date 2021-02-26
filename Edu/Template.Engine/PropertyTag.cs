using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// 属性或字段获取标签.如: &lt;vt:property var="time" field="Now" type="System.DateTime" /&gt;
	/// </summary>
	// Token: 0x0200000E RID: 14
	public class PropertyTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x06000093 RID: 147 RVA: 0x00003CEC File Offset: 0x00001EEC
		internal PropertyTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003CF5 File Offset: 0x00001EF5
		public override string TagName
		{
			get
			{
				return "property";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003CFC File Offset: 0x00001EFC
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 调用的属性或字段
		/// </summary>
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003CFF File Offset: 0x00001EFF
		public Attribute Field
		{
			get
			{
				return base.Attributes["Field"];
			}
		}

		/// <summary>
		/// 包含属性或字段的类型
		/// </summary>
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003D11 File Offset: 0x00001F11
		public new Attribute Type
		{
			get
			{
				return base.Attributes["Type"];
			}
		}

		/// <summary>
		/// 存储表达式结果的变量
		/// </summary>
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003D23 File Offset: 0x00001F23
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003D2B File Offset: 0x00001F2B
		public VariableIdentity Variable { get; protected set; }

		/// <summary>
		/// 是否输出此标签的结果值
		/// </summary>
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003D34 File Offset: 0x00001F34
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00003D3C File Offset: 0x00001F3C
		public bool Output { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x0600009C RID: 156 RVA: 0x00003D48 File Offset: 0x00001F48
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
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
		// Token: 0x0600009D RID: 157 RVA: 0x00003DA0 File Offset: 0x00001FA0
		protected override void RenderTagData(TextWriter writer)
		{
			object obj = (this.Type.Value is VariableExpression) ? this.Type.Value.GetValue() : Utility.CreateType(this.Type.Value.GetValue().ToString());
			bool flag;
			object obj2 = (obj == null) ? null : Utility.GetPropertyValue(obj, this.Field.GetTextValue(), out flag);
			if (this.Variable != null)
			{
				this.Variable.Value = obj2;
			}
			if (this.Output && obj2 != null)
			{
				writer.Write(obj2);
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
		// Token: 0x0600009E RID: 158 RVA: 0x00003E34 File Offset: 0x00002034
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Variable == null && !this.Output)
			{
				throw new ParserException(string.Format("{0}标签中如果未定义Output属性为true则必须定义var属性", this.TagName));
			}
			if (this.Field == null || string.IsNullOrEmpty(this.Field.Text))
			{
				throw new ParserException(string.Format("{0}标签中缺少field属性", this.TagName));
			}
			if (this.Type == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少type属性", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600009F RID: 159 RVA: 0x00003EC4 File Offset: 0x000020C4
		internal override Element Clone(Template ownerTemplate)
		{
			PropertyTag propertyTag = new PropertyTag(ownerTemplate);
			this.CopyTo(propertyTag);
			propertyTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			propertyTag.Output = this.Output;
			return propertyTag;
		}
	}
}
