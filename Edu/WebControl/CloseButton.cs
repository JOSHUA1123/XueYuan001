using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	// Token: 0x0200001A RID: 26
	[ToolboxData("<{0}:CloseButton runat=server></{0}:CloseButton>")]
	[DefaultProperty("Text")]
	public class CloseButton : ImageButton
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x00005898 File Offset: 0x00003A98
		protected override void OnInit(EventArgs e)
		{
			base.Attributes.Add("onclick", "new top.PageBox().Close(window.name);");
			this.CssClass = "btnCloseWin";
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty.gif";
			base.OnInit(e);
		}
	}
}
