using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// 数据输出标签,可输出某个标签的数据,或直接输出文件数据.如: &lt;vt:output tagid="list" /&gt; 或 &lt;vt:output file="output.html" charset="utf-8" /&gt;
	/// </summary>
	// Token: 0x02000012 RID: 18
	public class OutputTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x060000BC RID: 188 RVA: 0x00004444 File Offset: 0x00002644
		internal OutputTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000444D File Offset: 0x0000264D
		public override string TagName
		{
			get
			{
				return "output";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004454 File Offset: 0x00002654
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 需要输出数据的标签id
		/// </summary>
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004458 File Offset: 0x00002658
		protected Tag OutputTarget
		{
			get
			{
				if (this.outputTarget == null && this.TagId != null && !string.IsNullOrEmpty(this.TagId.GetTextValue()))
				{
					this.outputTarget = this.OwnerDocument.GetChildTagById(this.TagId.GetTextValue());
				}
				return this.outputTarget;
			}
		}

		/// <summary>
		/// 需要输出数据的标签的Id
		/// </summary>
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000044A9 File Offset: 0x000026A9
		public Attribute TagId
		{
			get
			{
				return base.Attributes["TagId"];
			}
		}

		/// <summary>
		/// 需要输出数据的文件
		/// </summary>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000044BB File Offset: 0x000026BB
		public Attribute File
		{
			get
			{
				return base.Attributes["File"];
			}
		}

		/// <summary>
		/// 文件编码
		/// </summary>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000044CD File Offset: 0x000026CD
		public Attribute Charset
		{
			get
			{
				return base.Attributes["Charset"];
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x060000C3 RID: 195 RVA: 0x000044E0 File Offset: 0x000026E0
		protected override void RenderTagData(TextWriter writer)
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			this.OnBeforeRender(cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				foreach (Element element in base.InnerElements)
				{
					element.Render(writer);
				}
				string text = (this.File == null) ? string.Empty : Utility.ResolveFilePath(base.Parent, this.File.GetTextValue());
				Encoding encoding = (this.Charset == null) ? base.OwnerTemplate.Charset : Utility.GetEncodingFromCharset(this.Charset.GetTextValue(), base.OwnerTemplate.Charset);
				if (this.OutputTarget != null)
				{
					this.OutputTarget.Render(writer);
				}
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						if (System.IO.File.Exists(text))
						{
							writer.Write(System.IO.File.ReadAllText(text, encoding));
						}
					}
					catch
					{
					}
				}
			}
			this.OnAfterRender(EventArgs.Empty);
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
		// Token: 0x060000C4 RID: 196 RVA: 0x000045F0 File Offset: 0x000027F0
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.TagId == null && this.File == null)
			{
				throw new ParserException(string.Format("{0}标签中必须定义tagid或file属性", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060000C5 RID: 197 RVA: 0x00004634 File Offset: 0x00002834
		internal override Element Clone(Template ownerTemplate)
		{
			OutputTag outputTag = new OutputTag(ownerTemplate);
			this.CopyTo(outputTag);
			return outputTag;
		}

		/// <summary>
		///
		/// </summary>
		// Token: 0x0400002F RID: 47
		protected Tag outputTarget;
	}
}
