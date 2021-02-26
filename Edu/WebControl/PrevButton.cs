using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	// Token: 0x02000013 RID: 19
	[ToolboxData("<{0}:CloseButton runat=server></{0}:CloseButton>")]
	[DefaultProperty("Text")]
	public class PrevButton : ImageButton
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00004634 File Offset: 0x00002834
		protected override void OnInit(EventArgs e)
		{
			this.CssClass = "btnPrevWin";
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty.gif";
			base.OnInit(e);
		}
	}
}
