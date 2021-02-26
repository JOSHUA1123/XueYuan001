using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	// Token: 0x02000015 RID: 21
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:StateShow runat=server></{0}:StateShow>")]
	public class StateShow : ImageButton
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004723 File Offset: 0x00002923
		// (set) Token: 0x0600009D RID: 157 RVA: 0x0000474E File Offset: 0x0000294E
		[Bindable(true)]
		[Description("按钮状态")]
		public bool State
		{
			get
			{
				return this.ViewState["State"] != null && Convert.ToBoolean(this.ViewState["State"]);
			}
			set
			{
				this.ViewState["State"] = value;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004768 File Offset: 0x00002968
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + (this.State ? "StateShowTrue.png" : "StateShowFalse.png");
		}
	}
}
