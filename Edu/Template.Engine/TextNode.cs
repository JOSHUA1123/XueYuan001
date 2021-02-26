using System;
using System.IO;

namespace VTemplate.Engine
{
	/// <summary>
	/// 文本节点
	/// </summary>
	// Token: 0x0200003B RID: 59
	public class TextNode : Element
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="text"></param>
		// Token: 0x060002B9 RID: 697 RVA: 0x0000C08E File Offset: 0x0000A28E
		internal TextNode(Template ownerTemplate, string text) : base(ownerTemplate)
		{
			this.Text = text;
		}

		/// <summary>
		/// 此节点的文本数据
		/// </summary>
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000C09E File Offset: 0x0000A29E
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000C0A6 File Offset: 0x0000A2A6
		public string Text { get; private set; }

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x060002BC RID: 700 RVA: 0x0000C0AF File Offset: 0x0000A2AF
		public override void Render(TextWriter writer)
		{
			writer.Write(this.Text);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060002BD RID: 701 RVA: 0x0000C0BD File Offset: 0x0000A2BD
		internal override Element Clone(Template ownerTemplate)
		{
			return new TextNode(ownerTemplate, this.Text);
		}

		/// <summary>
		/// 返回文本节点的文本字符
		/// </summary>
		/// <returns></returns>
		// Token: 0x060002BE RID: 702 RVA: 0x0000C0CB File Offset: 0x0000A2CB
		public override string ToString()
		{
			return this.Text;
		}
	}
}
