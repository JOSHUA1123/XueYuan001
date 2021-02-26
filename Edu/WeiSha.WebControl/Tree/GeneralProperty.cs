using System;

namespace WeiSha.WebControl.Tree
{
	/// <summary>
	/// 树形菜单的通用属性
	/// </summary>
	// Token: 0x02000002 RID: 2
	public class GeneralProperty
	{
		/// <summary>
		/// 数据列的主键id，字段名称
		/// </summary>
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		// (set) Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000002D8
		public string IdKeyName
		{
			get
			{
				return this._idKeyName;
			}
			set
			{
				this._idKeyName = value;
			}
		}

		/// <summary>
		/// 数据列的父id，字段名称
		/// </summary>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020E1 File Offset: 0x000002E1
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020E9 File Offset: 0x000002E9
		public string ParentIdKeyName
		{
			get
			{
				return this._PatIdName;
			}
			set
			{
				this._PatIdName = value;
			}
		}

		/// <summary>
		/// 数据列的排序号，字段名称
		/// </summary>
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020F2 File Offset: 0x000002F2
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020FA File Offset: 0x000002FA
		public string TaxKeyName
		{
			get
			{
				return this._taxKeyName;
			}
			set
			{
				this._taxKeyName = value;
			}
		}

		// Token: 0x04000001 RID: 1
		private string _idKeyName;

		// Token: 0x04000002 RID: 2
		private string _PatIdName;

		// Token: 0x04000003 RID: 3
		private string _taxKeyName;
	}
}
