using System;
using System.IO;

namespace VTemplate.Engine
{
	/// <summary>
	/// 元素
	/// </summary>
	// Token: 0x02000002 RID: 2
	public abstract class Element
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		protected Element(Template ownerTemplate)
		{
			this.OwnerTemplate = ownerTemplate;
		}

		/// <summary>
		/// 此元素的宿主模板
		/// </summary>
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020DF File Offset: 0x000002DF
		// (set) Token: 0x06000003 RID: 3 RVA: 0x000020E7 File Offset: 0x000002E7
		public Template OwnerTemplate { get; protected set; }

		/// <summary>
		/// 此元素的宿主模板文档
		/// </summary>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020F0 File Offset: 0x000002F0
		public virtual TemplateDocument OwnerDocument
		{
			get
			{
				if (this.OwnerTemplate != null)
				{
					return this.OwnerTemplate.OwnerDocument;
				}
				return null;
			}
		}

		/// <summary>
		/// 此元素的父级标签
		/// </summary>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002107 File Offset: 0x00000307
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000210F File Offset: 0x0000030F
		public Tag Parent { get; internal set; }

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x06000007 RID: 7
		public abstract void Render(TextWriter writer);

		/// <summary>
		/// 克隆元素
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x06000008 RID: 8
		internal abstract Element Clone(Template ownerTemplate);
	}
}
