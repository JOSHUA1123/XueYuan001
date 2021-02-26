using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	/// <summary>
	/// 用于载入复选框的类
	/// </summary>
	// Token: 0x02000019 RID: 25
	public class CheckTemplate : ITemplate
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x000057CC File Offset: 0x000039CC
		public CheckTemplate(string colname)
		{
			this.colname = colname;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000057DC File Offset: 0x000039DC
		public void InstantiateIn(Control container)
		{
			CheckBox checkBox = new CheckBox();
			checkBox.ID = this.colname;
			checkBox.CssClass = this.colname;
			checkBox.Attributes.Add("ControlType", "SelectBox");
			checkBox.DataBinding += this.OnDataBinding;
			container.Controls.Add(checkBox);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000583C File Offset: 0x00003A3C
		public void OnDataBinding(object sender, EventArgs e)
		{
			try
			{
				CheckBox checkBox = (CheckBox)sender;
				GridViewRow gridViewRow = (GridViewRow)checkBox.NamingContainer;
				checkBox.Checked = Convert.ToBoolean(((DataRowView)gridViewRow.DataItem)[this.colname].ToString());
			}
			catch
			{
			}
		}

		// Token: 0x0400001A RID: 26
		private string colname;
	}
}
