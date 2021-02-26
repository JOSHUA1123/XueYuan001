using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebControlShow.Tree;

namespace WebControlShow
{
	// Token: 0x02000011 RID: 17
	[ToolboxData("<{0}:DropDownTree runat=server></{0}:DropDownTree>")]
	[DefaultProperty("Text")]
	public class DropDownTree : DropDownList
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000040B8 File Offset: 0x000022B8
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000040E5 File Offset: 0x000022E5
		[Category("属性字段")]
		[Localizable(true)]
		[Bindable(true)]
		[DefaultValue("")]
		public string IdKeyName
		{
			get
			{
				string text = (string)this.ViewState["IdKeyName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["IdKeyName"] = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000040F8 File Offset: 0x000022F8
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00004125 File Offset: 0x00002325
		[Localizable(true)]
		[Bindable(true)]
		[Category("属性字段")]
		[DefaultValue("")]
		public string ParentIdKeyName
		{
			get
			{
				string text = (string)this.ViewState["ParentIdKeyName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ParentIdKeyName"] = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004138 File Offset: 0x00002338
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00004166 File Offset: 0x00002366
		[Bindable(true)]
		[Localizable(true)]
		[Category("属性字段")]
		[DefaultValue("")]
		public int Root
		{
			get
			{
				string text = (string)this.ViewState["Root"];
				if (text != null)
				{
					return Convert.ToInt32(text);
				}
				return 0;
			}
			set
			{
				this.ViewState["Root"] = value.ToString();
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004180 File Offset: 0x00002380
		// (set) Token: 0x0600008B RID: 139 RVA: 0x000041AD File Offset: 0x000023AD
		[Bindable(true)]
		[DefaultValue("")]
		[Localizable(true)]
		[Category("属性字段")]
		public string TaxKeyName
		{
			get
			{
				string text = (string)this.ViewState["TaxKeyName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["TaxKeyName"] = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000041C0 File Offset: 0x000023C0
		// (set) Token: 0x0600008D RID: 141 RVA: 0x000041ED File Offset: 0x000023ED
		[Bindable(true)]
		[Localizable(true)]
		[Category("属性字段")]
		[DefaultValue("")]
		public string TypeKeyName
		{
			get
			{
				string text = (string)this.ViewState["TypeKeyName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["TypeKeyName"] = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004200 File Offset: 0x00002400
		[DefaultValue("")]
		[Localizable(true)]
		[Category("选择项的文本")]
		[Bindable(true)]
		public string SelectedText
		{
			get
			{
				string text = this.SelectedItem.Text;
				if (!string.IsNullOrWhiteSpace(text) && text.IndexOf(" ") > -1)
				{
					text = text.Substring(text.IndexOf(" "));
				}
				return text;
			}
		}

		/// <summary>
		/// 重写绑定，处理数据源
		/// </summary>
		// Token: 0x0600008F RID: 143 RVA: 0x00004244 File Offset: 0x00002444
		public override void DataBind()
		{
			this._TransctionDataSource();
			base.DataBind();
			if (this.DataSource is DataTable)
			{
				DataTable dt = (DataTable)this.DataSource;
				foreach (object obj in this.Items)
				{
					ListItem listItem = (ListItem)obj;
					listItem.Attributes.Add("pid", this._GetPID(listItem.Value, dt));
					if (!string.IsNullOrWhiteSpace(this.TypeKeyName) && this.TypeKeyName.Trim() != "")
					{
						listItem.Attributes.Add("type", this._GetType(listItem.Value, dt));
					}
				}
			}
		}

		/// <summary>
		/// 处理数据源，将它转换成树形
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000090 RID: 144 RVA: 0x0000431C File Offset: 0x0000251C
		protected void _TransctionDataSource()
		{
			if (this.DataSource == null)
			{
				return;
			}
			DataTable dataTable = null;
			if (this.DataSource is DataTable)
			{
				dataTable = (this.DataSource as DataTable);
			}
			if (this.DataSource is IList && !(this.DataSource is Array))
			{
				IList list = this.DataSource as IList;
				if (list == null || list.Count < 1)
				{
					return;
				}
				object[] array = new object[list.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = list[i];
				}
				this.DataSource = array;
			}
			if (this.DataSource is Array)
			{
				Array array2 = this.DataSource as Array;
				if (array2.Length < 1)
				{
					return;
				}
				dataTable = ObjectArrayToDataTable.To(array2);
			}
			dataTable = new DataTableTree
			{
				IdKeyName = this.IdKeyName,
				ParentIdKeyName = this.ParentIdKeyName,
				TaxKeyName = this.TaxKeyName,
				Root = this.Root
			}.BuilderTree(dataTable);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (object obj in dataTable.Rows)
				{
					DataRow dataRow = (DataRow)obj;
					dataRow[this.DataTextField] = dataRow["Tree"].ToString() + " " + dataRow[this.DataTextField].ToString();
				}
			}
			this.DataSource = dataTable;
		}

		/// <summary>
		/// 根据当前项的id，取其父id
		/// </summary>
		/// <param name="id"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		// Token: 0x06000091 RID: 145 RVA: 0x000044C0 File Offset: 0x000026C0
		private string _GetPID(string id, DataTable dt)
		{
			foreach (object obj in dt.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if (dataRow[this.IdKeyName].ToString() == id.ToString())
				{
					return dataRow[this.ParentIdKeyName].ToString();
				}
			}
			return "0";
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000454C File Offset: 0x0000274C
		private string _GetType(string id, DataTable dt)
		{
			foreach (object obj in dt.Rows)
			{
				DataRow dataRow = (DataRow)obj;
				if (dataRow[this.IdKeyName].ToString() == id.ToString())
				{
					return dataRow[this.TypeKeyName].ToString();
				}
			}
			return "";
		}
	}
}
