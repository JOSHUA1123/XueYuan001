using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	// Token: 0x0200000B RID: 11
	[ToolboxData("<{0}:StateButton runat=server></{0}:StateButton>")]
	[DefaultProperty("Text")]
	public class StateButton : LinkButton
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002EFF File Offset: 0x000010FF
		// (set) Token: 0x06000044 RID: 68 RVA: 0x00002F2A File Offset: 0x0000112A
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002F44 File Offset: 0x00001144
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00002F71 File Offset: 0x00001171
		[Description("按钮状态为False，显示的文本")]
		[Bindable(true)]
		public string FalseText
		{
			get
			{
				string text = (string)this.ViewState["FalseText"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["FalseText"] = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002F84 File Offset: 0x00001184
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002FB1 File Offset: 0x000011B1
		[Description("按钮状态为True，显示的文本")]
		[Bindable(true)]
		public string TrueText
		{
			get
			{
				string text = (string)this.ViewState["TrueText"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["TrueText"] = value;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002FC4 File Offset: 0x000011C4
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002FCD File Offset: 0x000011CD
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.Text = (this.State ? this.TrueText : this.FalseText);
			this.CssClass = (this.State ? "linkButtonTrue" : "linkButtonFalse");
		}
	}
}
