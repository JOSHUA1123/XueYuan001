using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	/// <summary>
	/// 数据行内的编辑按钮
	/// </summary>
	// Token: 0x02000009 RID: 9
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:RowEdit runat=server></{0}:RowEdit>")]
	public class RowEdit : ImageButton
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000023DC File Offset: 0x000005DC
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002407 File Offset: 0x00000607
		[DefaultValue("")]
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsJsEvent
		{
			get
			{
				return this.ViewState["IsJsEvent"] == null || Convert.ToBoolean(this.ViewState["IsJsEvent"]);
			}
			set
			{
				this.ViewState["IsJsEvent"] = value;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002420 File Offset: 0x00000620
		protected override void OnInit(EventArgs e)
		{
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty22.gif";
			this.CssClass = base.GetType().Name;
			base.Attributes.Add("IsJsEvent", this.IsJsEvent.ToString().ToLower());
			this.ToolTip = "修改";
			base.OnInit(e);
		}
	}
}
