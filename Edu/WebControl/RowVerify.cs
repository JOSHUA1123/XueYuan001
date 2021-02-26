using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	/// <summary>
	/// 数据行内的审核按钮
	/// </summary>
	// Token: 0x0200000F RID: 15
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:RowView runat=server></{0}:RowView>")]
	public class RowVerify : ImageButton
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00003EEC File Offset: 0x000020EC
		protected override void OnInit(EventArgs e)
		{
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty22.gif";
			this.CssClass = base.GetType().Name;
			this.ToolTip = "审核";
			base.OnInit(e);
		}
	}
}
