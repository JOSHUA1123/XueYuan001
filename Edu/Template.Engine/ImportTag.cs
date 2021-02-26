using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// 类型导入标签, 如:&lt;vt:import var="math" type="System.Math" /&gt;
	/// </summary>
	// Token: 0x02000006 RID: 6
	public class ImportTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x06000048 RID: 72 RVA: 0x000031A9 File Offset: 0x000013A9
		internal ImportTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000031B2 File Offset: 0x000013B2
		public override string TagName
		{
			get
			{
				return "import";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000031B9 File Offset: 0x000013B9
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 要对其赋值的变量
		/// </summary>
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000031BC File Offset: 0x000013BC
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000031C4 File Offset: 0x000013C4
		public VariableIdentity Variable { get; protected set; }

		/// <summary>
		/// 要导入的类型名称
		/// </summary>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000031CD File Offset: 0x000013CD
		public new Attribute Type
		{
			get
			{
				return base.Attributes["Type"];
			}
		}

		/// <summary>
		/// 类型所在的程序集
		/// </summary>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000031DF File Offset: 0x000013DF
		public Attribute Assembly
		{
			get
			{
				return base.Attributes["Assembly"];
			}
		}

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x0600004F RID: 79 RVA: 0x000031F4 File Offset: 0x000013F4
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (!(name == "var"))
				{
					return;
				}
				this.Variable = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x06000050 RID: 80 RVA: 0x0000322C File Offset: 0x0000142C
		protected override void RenderTagData(TextWriter writer)
		{
			Type value = null;
			if (this.Type.Value is VariableExpression)
			{
				object value2 = this.Type.Value.GetValue();
				if (value2 != null)
				{
					value = ((value2 is Type) ? ((Type)value2) : value2.GetType());
				}
			}
			else
			{
				string assembly = (this.Assembly == null) ? string.Empty : this.Assembly.GetTextValue();
				value = Utility.CreateType(this.Type.GetTextValue(), assembly);
			}
			if (this.Variable != null)
			{
				this.Variable.Value = value;
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
		// Token: 0x06000051 RID: 81 RVA: 0x000032C4 File Offset: 0x000014C4
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Variable == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少var属性", this.TagName));
			}
			if (this.Type == null || string.IsNullOrEmpty(this.Type.Text))
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
		// Token: 0x06000052 RID: 82 RVA: 0x00003330 File Offset: 0x00001530
		internal override Element Clone(Template ownerTemplate)
		{
			ImportTag importTag = new ImportTag(ownerTemplate);
			this.CopyTo(importTag);
			importTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			return importTag;
		}
	}
}
