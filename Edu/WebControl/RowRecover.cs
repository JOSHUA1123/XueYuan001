using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	/// <summary>
	/// 数据行内的还原按钮
	/// </summary>
	// Token: 0x02000005 RID: 5
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:RowRecover runat=server></{0}:RowRecover>")]
	public class RowRecover : ImageButton
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021C4 File Offset: 0x000003C4
		protected override void OnInit(EventArgs e)
		{
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty22.gif";
			this.CssClass = base.GetType().Name;
			this.ToolTip = "还原";
			base.OnInit(e);
		}
	}
}
