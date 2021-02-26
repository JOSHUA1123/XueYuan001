using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 元素属性
	/// </summary>
	// Token: 0x02000018 RID: 24
	public class Attribute : ICloneableElement<Attribute>
	{
		/// <summary>
		/// 元素属性
		/// </summary>
		/// <param name="name"></param>
		/// <param name="text"></param>
		// Token: 0x06000110 RID: 272 RVA: 0x00006219 File Offset: 0x00004419
		private Attribute(string name, string text)
		{
			this.Name = name;
			this.Text = text;
		}

		/// <summary>
		/// 元素属性
		/// </summary>
		/// <param name="ownerElement"></param>
		/// <param name="name"></param>
		/// <param name="text"></param>
		// Token: 0x06000111 RID: 273 RVA: 0x0000622F File Offset: 0x0000442F
		internal Attribute(Element ownerElement, string name, string text)
		{
			this.OwnerElement = ownerElement;
			this.Name = name;
			this.Text = text;
			this.Value = ParserHelper.CreateExpression(this.OwnerElement.OwnerTemplate, this.Text);
		}

		/// <summary>
		/// 宿主标签
		/// </summary>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00006268 File Offset: 0x00004468
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00006270 File Offset: 0x00004470
		public Element OwnerElement { get; internal set; }

		/// <summary>
		/// 属性名称
		/// </summary>
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00006279 File Offset: 0x00004479
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00006281 File Offset: 0x00004481
		public string Name { get; private set; }

		/// <summary>
		/// 属性的值文本
		/// </summary>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000628A File Offset: 0x0000448A
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00006292 File Offset: 0x00004492
		public string Text { get; private set; }

		/// <summary>
		/// 属性的值
		/// </summary>
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000629B File Offset: 0x0000449B
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000062A3 File Offset: 0x000044A3
		public IExpression Value { get; private set; }

		/// <summary>
		/// 获取文本值
		/// </summary>
		// Token: 0x0600011A RID: 282 RVA: 0x000062AC File Offset: 0x000044AC
		internal string GetTextValue()
		{
			if (this.Value == null)
			{
				return this.Text;
			}
			object value = this.Value.GetValue();
			if (Utility.IsNothing(value))
			{
				return string.Empty;
			}
			return value.ToString();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600011B RID: 283 RVA: 0x000062E8 File Offset: 0x000044E8
		public Attribute Clone(Template ownerTemplate)
		{
			return new Attribute(this.Name, this.Text)
			{
				OwnerElement = null,
				Value = ((this.Value == null) ? null : this.Value.Clone(ownerTemplate))
			};
		}
	}
}
