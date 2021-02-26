using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	// Token: 0x0200000D RID: 13
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:CloseButton runat=server></{0}:CloseButton>")]
	public class NextButton : ImageButton
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00003140 File Offset: 0x00001340
		protected override void OnInit(EventArgs e)
		{
			this.CssClass = "btnNextWin";
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty.gif";
			base.OnInit(e);
		}
	}
}
