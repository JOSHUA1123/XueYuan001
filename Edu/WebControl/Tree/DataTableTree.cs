using System;
using System.Data;

namespace WebControlShow.Tree
{
	// Token: 0x02000017 RID: 23
	public class DataTableTree
	{
		/// <summary>
		/// 用于生成树的数据
		/// </summary>
		// Token: 0x1700003A RID: 58
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00004BD4 File Offset: 0x00002DD4
		public DataTable DataSource
		{
			set
			{
				this._dataSource = value;
			}
		}

		/// <summary>
		/// 数据列的主键id，字段名称
		/// </summary>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004BDD File Offset: 0x00002DDD
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00004BE5 File Offset: 0x00002DE5
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
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004BEE File Offset: 0x00002DEE
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00004BF6 File Offset: 0x00002DF6
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
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004BFF File Offset: 0x00002DFF
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00004C07 File Offset: 0x00002E07
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

		/// <summary>
		/// 树形根节点的id值
		/// </summary>
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004C10 File Offset: 0x00002E10
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00004C18 File Offset: 0x00002E18
		public int Root
		{
			get
			{
				return this._root;
			}
			set
			{
				this._root = value;
			}
		}

		/// <summary>
		/// 将数据源将换成树形数据
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		// Token: 0x060000BC RID: 188 RVA: 0x00004C24 File Offset: 0x00002E24
		public DataTable BuilderTree(DataTable dt)
		{
			if (dt == null)
			{
				return null;
			}
			if (dt.Columns["Tree"] == null)
			{
				dt.Columns.Add(new DataColumn("Tree", typeof(string)));
			}
			if (dt.Columns["isTop"] == null)
			{
				dt.Columns.Add(new DataColumn("isTop", typeof(string)));
			}
			if (dt.Columns["isDown"] == null)
			{
				dt.Columns.Add(new DataColumn("isDown", typeof(string)));
			}
			DataTable dataTable = dt.Clone();
			this.sortFunc(dt, dataTable, this.Root);
			if (dataTable.Rows.Count < dt.Rows.Count)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < dataTable.Rows.Count; j++)
					{
						if (dataTable.Rows[j][this.IdKeyName].ToString() == dt.Rows[i][this.IdKeyName].ToString())
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						dataTable.ImportRow(dt.Rows[i]);
					}
				}
			}
			return dataTable;
		}

		/// <summary>
		/// 处理数据源，生成树形数据
		/// </summary>
		/// <param name="dt">数据源</param>
		/// <param name="sortDt"></param>
		/// <param name="parentId"></param>
		// Token: 0x060000BD RID: 189 RVA: 0x00004D84 File Offset: 0x00002F84
		private void sortFunc(DataTable dt, DataTable sortDt, int parentId)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (dt.Rows[i][this.ParentIdKeyName] != null)
				{
					dt.Rows[i][this.ParentIdKeyName].ToString();
				}
				int num;
				int.TryParse(dt.Rows[i][this.ParentIdKeyName].ToString(), out num);
				if (num == parentId)
				{
					string text = this.line(dt, dt.Rows[i]);
					if (this.isBottom(dt, dt.Rows[i], i))
					{
						text += "┗";
						dt.Rows[i]["isDown"] = true;
					}
					else
					{
						text += "┣";
						dt.Rows[i]["isDown"] = false;
					}
					if (this.isTop(dt, dt.Rows[i], i))
					{
						dt.Rows[i]["isTop"] = true;
					}
					else
					{
						dt.Rows[i]["isTop"] = false;
					}
					dt.Rows[i]["Tree"] = text;
					sortDt.ImportRow(dt.Rows[i]);
					if (this.isChildren(dt, dt.Rows[i]))
					{
						int parentId2;
						int.TryParse(dt.Rows[i][this.IdKeyName].ToString(), out parentId2);
						this.sortFunc(dt, sortDt, parentId2);
					}
				}
			}
		}

		/// <summary>
		/// 当前对象是否有子级
		/// </summary>
		/// <param name="dt">当前层（深度）集合</param>
		/// <param name="dr">当前对象</param>
		/// <returns>是否有子级，有返回true，否则返回false</returns>
		// Token: 0x060000BE RID: 190 RVA: 0x00004F48 File Offset: 0x00003148
		private bool isChildren(DataTable dt, DataRow dr)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (dt.Rows[i][this.ParentIdKeyName].ToString() == dr[this.IdKeyName].ToString())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 是否为当前层深的最后一个；正序排
		/// </summary>
		/// <param name="dt">当前层（深度）集合</param>
		/// <param name="dr">当前对象</param>
		/// <returns>是最后一个，返回true，否则返回false</returns>
		// Token: 0x060000BF RID: 191 RVA: 0x00004FA4 File Offset: 0x000031A4
		private bool isBottom(DataTable dt, DataRow dr, int index)
		{
			int num = (this.TaxKeyName != "") ? Convert.ToInt32((dr[this.TaxKeyName] == null || dr[this.TaxKeyName].ToString() == "") ? 0 : dr[this.TaxKeyName]) : index;
			int num2 = num;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (dt.Rows[i][this.ParentIdKeyName].ToString() == dr[this.ParentIdKeyName].ToString())
				{
					int num3 = (this.TaxKeyName != "") ? Convert.ToInt32((dt.Rows[i][this.TaxKeyName] == null || dt.Rows[i][this.TaxKeyName].ToString() == "") ? 0 : dt.Rows[i][this.TaxKeyName]) : i;
					if (num3 > num)
					{
						num2 = num3;
					}
				}
			}
			return num == num2;
		}

		/// <summary>
		/// 是否为当前层深的第一个；正序排
		/// </summary>
		/// <param name="dt">当前层（深度）集合</param>
		/// <param name="dr">当前对象</param>
		/// <returns>是第一个，返回true，否则返回false</returns>
		// Token: 0x060000C0 RID: 192 RVA: 0x000050E8 File Offset: 0x000032E8
		private bool isTop(DataTable dt, DataRow dr, int index)
		{
			int num = (this.TaxKeyName != "") ? Convert.ToInt32((dr[this.TaxKeyName] == null || dr[this.TaxKeyName].ToString() == "") ? 0 : dr[this.TaxKeyName]) : index;
			int num2 = num;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (dt.Rows[i][this.ParentIdKeyName].ToString() == dr[this.ParentIdKeyName].ToString())
				{
					int num3 = (this.TaxKeyName != "") ? Convert.ToInt32((dt.Rows[i][this.TaxKeyName] == null || dt.Rows[i][this.TaxKeyName].ToString() == "") ? 0 : dt.Rows[i][this.TaxKeyName]) : i;
					if (num3 < num)
					{
						num2 = num3;
					}
				}
			}
			return num == num2;
		}

		/// <summary>
		/// 节点前的空格或竖线
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="dr"></param>
		/// <returns></returns>
		// Token: 0x060000C1 RID: 193 RVA: 0x0000522C File Offset: 0x0000342C
		private string line(DataTable dt, DataRow dr)
		{
			string text = "";
			while (Convert.ToInt32((dr[this.ParentIdKeyName] == null || dr[this.ParentIdKeyName].ToString() == "") ? 0 : dr[this.ParentIdKeyName]) > this.Root)
			{
				int i = 0;
				while (i < dt.Rows.Count)
				{
					if (dt.Rows[i][this.IdKeyName].ToString() == dr[this.ParentIdKeyName].ToString())
					{
						dr = dt.Rows[i];
						if (this.isBottom(dt, dr, i))
						{
							text = "\u3000" + text;
							break;
						}
						text = "┃" + text;
						break;
					}
					else
					{
						i++;
					}
				}
			}
			return text;
		}

		// Token: 0x04000015 RID: 21
		private DataTable _dataSource;

		// Token: 0x04000016 RID: 22
		private string _idKeyName;

		// Token: 0x04000017 RID: 23
		private string _PatIdName;

		// Token: 0x04000018 RID: 24
		private string _taxKeyName;

		// Token: 0x04000019 RID: 25
		private int _root;
	}
}
