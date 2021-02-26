using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Common;
using Common.Templates;

namespace VTemplate.Engine
{
	/// <summary>
	/// 文件包含标签.如: &lt;vt:include file="include.html" charset="utf-8" /&gt;
	/// </summary>
	// Token: 0x0200002F RID: 47
	public class IncludeTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x06000254 RID: 596 RVA: 0x0000A8E1 File Offset: 0x00008AE1
		internal IncludeTag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.Charset = ownerTemplate.Charset;
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000A8F6 File Offset: 0x00008AF6
		public override string TagName
		{
			get
			{
				return "include";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000A8FD File Offset: 0x00008AFD
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 引用的文件
		/// </summary>
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000A900 File Offset: 0x00008B00
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000A908 File Offset: 0x00008B08
		public string File { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000A911 File Offset: 0x00008B11
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000A919 File Offset: 0x00008B19
		public string From { get; private set; }

		/// <summary>
		/// 文件编码
		/// </summary>
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000A922 File Offset: 0x00008B22
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000A92A File Offset: 0x00008B2A
		public Encoding Charset { get; private set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x0600025D RID: 605 RVA: 0x0000A934 File Offset: 0x00008B34
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "file")
				{
					this.File = item.Text;
					return;
				}
				if (name == "from")
				{
					this.From = item.Text;
					return;
				}
				if (!(name == "charset"))
				{
					return;
				}
				this.Charset = Utility.GetEncodingFromCharset(item.Text, base.OwnerTemplate.Charset);
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
		// Token: 0x0600025E RID: 606 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			container.AppendChild(this);
			if (!string.IsNullOrEmpty(this.File))
			{
				TemplateBank templateBank = null;
				if (this.From != null)
				{
                    if (ownerTemplate.File.StartsWith(Common.Template.ForWeb.Root.Physics))
					{
                        templateBank = Common.Template.ForWeb.GetTemplate(this.From);
					}
                    if (ownerTemplate.File.StartsWith(Common.Template.ForMobile.Root.Physics))
					{
                        templateBank = Common.Template.ForMobile.GetTemplate(this.From);
					}
				}
				if (templateBank != null)
				{
					this.File = TemplateDocument.RelativePath_ClearnPoint(templateBank.Path.Physics + this.File.Replace("/", "\\"));
				}
				if (templateBank == null)
				{
					this.File = Utility.ResolveFilePath(base.Parent, this.File);
				}
				if (System.IO.File.Exists(this.File))
				{
					base.OwnerTemplate.AddFileDependency(this.File);
					string text2 = System.IO.File.ReadAllText(this.File, this.Charset);
					if (!string.IsNullOrWhiteSpace(this.From))
					{
						text2 = TemplateDocument._replacePath(text2, ownerTemplate.File, this.File, "link|script|img");
					}
					else
					{
						text2 = TemplateDocument._replacePath(text2, ownerTemplate.File, this.File, "link|script|img|a");
					}
					new TemplateDocument(ownerTemplate, this, text2, ownerTemplate.OwnerDocument.DocumentConfig);
				}
			}
			return !isClosedTag;
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600025F RID: 607 RVA: 0x0000AB04 File Offset: 0x00008D04
		internal override Element Clone(Template ownerTemplate)
		{
			IncludeTag includeTag = new IncludeTag(ownerTemplate);
			this.CopyTo(includeTag);
			includeTag.File = this.File;
			includeTag.Charset = this.Charset;
			return includeTag;
		}
	}
}
