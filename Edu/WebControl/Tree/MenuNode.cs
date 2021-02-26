using System;
using System.Data;

namespace WebControlShow.Tree
{
	/// <summary>
	/// 为了生成树形菜单，对菜单中的节点进行处理的类
	/// </summary>
	// Token: 0x02000016 RID: 22
	public class MenuNode
	{
		/// <summary>
		/// 所部节点
		/// </summary>
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000047BF File Offset: 0x000029BF
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x000047C7 File Offset: 0x000029C7
		public DataTable FullData
		{
			get
			{
				return this._fullData;
			}
			set
			{
				this._fullData = value;
			}
		}

		/// <summary>
		/// 自身节点
		/// </summary>
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000047D0 File Offset: 0x000029D0
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x000047D8 File Offset: 0x000029D8
		public DataRow Item
		{
			get
			{
				return this._item;
			}
			set
			{
				this._item = value;
			}
		}

		/// <summary>
		/// 所有子级节点
		/// </summary>
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000047E1 File Offset: 0x000029E1
		public DataRow[] Childs
		{
			get
			{
				return this._Childs;
			}
		}

		/// <summary>
		/// 是否有子级节点
		/// </summary>
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000047E9 File Offset: 0x000029E9
		public bool IsChilds
		{
			get
			{
				return this._IsChilds;
			}
		}

		/// <summary>
		/// 是否为最后一个
		/// </summary>
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000047F1 File Offset: 0x000029F1
		public bool IsLast
		{
			get
			{
				return this._IsLast;
			}
		}

		/// <summary>
		/// 当前节点的上级节点
		/// </summary>
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000047F9 File Offset: 0x000029F9
		public DataRow Parent
		{
			get
			{
				return this._parent;
			}
		}

		/// <summary>
		/// 数据列的主键id，字段名称
		/// </summary>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004801 File Offset: 0x00002A01
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00004809 File Offset: 0x00002A09
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
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004812 File Offset: 0x00002A12
		// (set) Token: 0x060000AB RID: 171 RVA: 0x0000481A File Offset: 0x00002A1A
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
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004823 File Offset: 0x00002A23
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000482B File Offset: 0x00002A2B
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
		/// 构造方法
		/// </summary>
		/// <param name="item">自身节点对象</param>
		/// <param name="fulldata">所有节点数组</param>
		/// <param name="general">树形菜单的通用属性</param>
		// Token: 0x060000AE RID: 174 RVA: 0x00004834 File Offset: 0x00002A34
		public MenuNode(DataRow item, DataTable fulldata, GeneralProperty general)
		{
			this._item = item;
			this._fullData = fulldata;
			this._idKeyName = general.IdKeyName;
			this._PatIdName = general.ParentIdKeyName;
			this._taxKeyName = general.TaxKeyName;
			this._Childs = this._getChilds();
			this._parent = this._getParent();
			this._IsLast = this._getIsLast();
		}

		/// <summary>
		/// 求当前节点的下级菜单
		/// </summary>
		// Token: 0x060000AF RID: 175 RVA: 0x000048A0 File Offset: 0x00002AA0
		private DataRow[] _getChilds()
		{
			string b = (this._item == null) ? "0" : this._item[this.IdKeyName].ToString();
			int num = 0;
			foreach (object obj in this._fullData.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if (dataRow[this.ParentIdKeyName].ToString() == b)
				{
					num++;
				}
			}
			this._Childs = new DataRow[num];
			int num2 = 0;
			foreach (object obj2 in this._fullData.Rows)
			{
				DataRow dataRow2 = (DataRow)obj2;
				if (dataRow2[this.ParentIdKeyName].ToString() == b)
				{
					this._Childs[num2++] = dataRow2;
				}
			}
			this._IsChilds = (this._Childs.Length != 0);
			return this.Sort(this._Childs);
		}

		/// <summary>
		/// 排序
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		// Token: 0x060000B0 RID: 176 RVA: 0x000049E8 File Offset: 0x00002BE8
		private DataRow[] Sort(DataRow[] array)
		{
			for (int i = 0; i <= array.Length - 1; i++)
			{
				for (int j = array.Length - 1; j > i; j--)
				{
					int num = (int)Convert.ToInt16(array[j][this.TaxKeyName].ToString());
					int num2 = (int)Convert.ToInt16(array[j - 1][this.TaxKeyName].ToString());
					if (num < num2)
					{
						DataRow dataRow = array[j];
						array[j] = array[j - 1];
						array[j - 1] = dataRow;
					}
				}
			}
			return array;
		}

		/// <summary>
		/// 判断是否最当前层级的最后一个节点
		/// </summary>
		// Token: 0x060000B1 RID: 177 RVA: 0x00004A64 File Offset: 0x00002C64
		private bool _getIsLast()
		{
			if (this._item == null)
			{
				return true;
			}
			bool result = false;
			DataRow dataRow = null;
			foreach (object obj in this._fullData.Rows)
			{
				DataRow dataRow2 = (DataRow)obj;
				if (dataRow2[this.ParentIdKeyName].ToString() == this._item[this.ParentIdKeyName].ToString())
				{
					dataRow = dataRow2;
				}
			}
			if (dataRow != null && dataRow[this.IdKeyName].ToString() == this._item[this.IdKeyName].ToString())
			{
				result = true;
			}
			return result;
		}

		/// <summary>
		/// 获取当前节点的父节点
		/// </summary>
		// Token: 0x060000B2 RID: 178 RVA: 0x00004B30 File Offset: 0x00002D30
		private DataRow _getParent()
		{
			if (this._item == null)
			{
				return null;
			}
			DataRow result = null;
			foreach (object obj in this._fullData.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				string a = dataRow[this.IdKeyName].ToString();
				string b = this._item[this.ParentIdKeyName].ToString();
				if (a == b)
				{
					result = dataRow;
					break;
				}
			}
			return result;
		}

		// Token: 0x0400000C RID: 12
		private DataTable _fullData;

		// Token: 0x0400000D RID: 13
		private DataRow _item;

		// Token: 0x0400000E RID: 14
		private DataRow[] _Childs;

		// Token: 0x0400000F RID: 15
		private bool _IsChilds;

		// Token: 0x04000010 RID: 16
		private bool _IsLast;

		// Token: 0x04000011 RID: 17
		private DataRow _parent;

		// Token: 0x04000012 RID: 18
		private string _idKeyName;

		// Token: 0x04000013 RID: 19
		private string _PatIdName;

		// Token: 0x04000014 RID: 20
		private string _taxKeyName;
	}
}
