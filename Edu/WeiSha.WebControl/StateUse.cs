using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	// Token: 0x02000014 RID: 20
	[ToolboxData("<{0}:StateUse runat=server></{0}:StateUse>")]
	[DefaultProperty("Text")]
	public class StateUse : ImageButton
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004687 File Offset: 0x00002887
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000046B2 File Offset: 0x000028B2
		[Description("按钮状态")]
		[Bindable(true)]
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

		// Token: 0x0600009A RID: 154 RVA: 0x000046CC File Offset: 0x000028CC
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + (this.State ? "StateUseTrue.png" : "StateUseFalse.png");
		}
	}
}
