using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	// Token: 0x02000022 RID: 34
	[ToolboxData("<{0}:CloseButton runat=server></{0}:CloseButton>")]
	[DefaultProperty("Text")]
	public class CloseRefreshButton : ImageButton
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00006A24 File Offset: 0x00004C24
		protected override void OnInit(EventArgs e)
		{
			if (base.Text.Trim() == "")
			{
				base.Text = "关闭";
			}
			if (base.CssClass == "")
			{
				base.CssClass = "Button";
			}
			base.Attributes.Add("onclick", "new parent.PageBox().CloseAndRefresh();");
			this.CssClass = "btnCloseWin";
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty.gif";
			base.OnInit(e);
			base.OnInit(e);
		}
	}
}
