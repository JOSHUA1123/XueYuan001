using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	// Token: 0x02000007 RID: 7
	[ToolboxData("<{0}:EnterButton runat=server></{0}:EnterButton>")]
	[DefaultProperty("Text")]
	public class SubmitButton : ImageButton
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002324 File Offset: 0x00000524
		protected override void OnInit(EventArgs e)
		{
			this.CssClass = "btnEnterWin";
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty.gif";
			base.OnInit(e);
		}
	}
}
