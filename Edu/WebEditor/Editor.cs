using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebEditor
{
	// Token: 0x02000005 RID: 5
	[ToolboxData("<{0}:Editor runat=server></{0}:Editor>")]
	[DefaultProperty("Text")]
	public class Editor : TextBox
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000024B0 File Offset: 0x000006B0
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000024DD File Offset: 0x000006DD
		[Category("Kind属性")]
		[Bindable(true)]
		[Localizable(true)]
		[DefaultValue("default")]
		public string ThemeType
		{
			get
			{
				string text = (string)this.ViewState["ThemeType"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ThemeType"] = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000024F0 File Offset: 0x000006F0
		// (set) Token: 0x06000018 RID: 24 RVA: 0x0000251D File Offset: 0x0000071D
		[DefaultValue("")]
		[Bindable(true)]
		[Localizable(true)]
		[Category("Appearance")]
		public override string Text
		{
			get
			{
				string text = (string)this.ViewState["Text"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002530 File Offset: 0x00000730
		// (set) Token: 0x0600001A RID: 26 RVA: 0x0000255D File Offset: 0x0000075D
		[Localizable(true)]
		[Bindable(true)]
		[Category("过滤模式，为true时过滤HTML代码，false时允许输入任何代码")]
		[DefaultValue("true")]
		public string FilterMode
		{
			get
			{
				string text = (string)this.ViewState["FilterMode"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["FilterMode"] = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002570 File Offset: 0x00000770
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000259D File Offset: 0x0000079D
		[Localizable(true)]
		[Category("Kind事件")]
		[DefaultValue("")]
		[Bindable(true)]
		public string afterCreate
		{
			get
			{
				string text = (string)this.ViewState["afterCreate"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["afterCreate"] = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000025B0 File Offset: 0x000007B0
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000025DD File Offset: 0x000007DD
		[Bindable(true)]
		[Category("Kind事件")]
		[DefaultValue("")]
		[Localizable(true)]
		public string afterChange
		{
			get
			{
				string text = (string)this.ViewState["afterChange"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["afterChange"] = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000025F0 File Offset: 0x000007F0
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000261D File Offset: 0x0000081D
		[Category("Kind事件")]
		[DefaultValue("")]
		[Localizable(true)]
		[Bindable(true)]
		public string afterTab
		{
			get
			{
				string text = (string)this.ViewState["afterTab"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["afterTab"] = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002630 File Offset: 0x00000830
		// (set) Token: 0x06000022 RID: 34 RVA: 0x0000265D File Offset: 0x0000085D
		[Localizable(true)]
		[Bindable(true)]
		[DefaultValue("")]
		[Category("Kind事件")]
		public string afterFocus
		{
			get
			{
				string text = (string)this.ViewState["afterFocus"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["afterFocus"] = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002670 File Offset: 0x00000870
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000269D File Offset: 0x0000089D
		[Category("Kind事件")]
		[Bindable(true)]
		[DefaultValue("")]
		[Localizable(true)]
		public string afterBlur
		{
			get
			{
				string text = (string)this.ViewState["afterBlur"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["afterBlur"] = value;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000026B0 File Offset: 0x000008B0
		protected override void OnInit(EventArgs e)
		{
			if (base.DesignMode)
			{
				return;
			}
			base.OnInit(e);
			this.TextMode = TextBoxMode.MultiLine;
			base.Attributes.Add("KindId", this.ClientID);
			string[] array = new string[]
			{
				"kindeditor.js",
				"lang/zh_CN.js",
				"plugins/code/prettify.js"
			};
			string text = "<script charset=\"utf-8\" src=\"{path}{script}\"></script>\r";
			string virtualPath = ConfigHandler.Items["BasePath"].VirtualPath;
			text = text.Replace("{path}", virtualPath);
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered(base.GetType(), "KindEditor"))
			{
				string text2 = "";
				foreach (string newValue in array)
				{
					text2 += text.Replace("{script}", newValue);
				}
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "KindEditor", text2);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027A8 File Offset: 0x000009A8
		protected override void OnLoad(EventArgs e)
		{
			if (base.DesignMode)
			{
				return;
			}
			base.OnLoad(e);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000027BC File Offset: 0x000009BC
		protected override void OnPreRender(EventArgs e)
		{
			if (base.DesignMode)
			{
				return;
			}
			base.OnPreRender(e);
			string text = "<script>\r\n            var {controlId};\r\n\t\t    KindEditor.ready(function(K) {\r\n\t\t\t        {controlId}=K.create('textarea[KindId={controlId}]', {\r\n\t\t\t\t    cssPath : '{path}plugins/code/prettify.css',\r\n\t\t\t\t    uploadJson : '{path}asp.net/upload_json.ashx',\r\n\t\t\t\t    fileManagerJson : '{path}asp.net/file_manager_json.ashx',\r\n                    editorid : '{controlId}',\r\n\t\t\t\t    allowFileManager : true,\r\n                    autoHeightMode : true,\r\n                    readonlyMode : {readonlyMode},                   \r\n                    width:'{width}',height:'{height}',\r\n                    {themeTyle}\r\n\t\t\t\t    afterCreate :{afterCreate},\r\n                    afterChange : {afterChange},\r\n                    afterTab : {afterTab},\r\n                    filterMode : {filterMode},\r\n                    afterFocus : {afterFocus},\r\n                    resizeMode : 0,\r\n                    afterBlur : {afterBlur}                    \r\n\t\t\t    });\r\n\t\t\t    prettyPrint();\r\n\t\t    });\r\n\t    </script>";
			string virtualPath = ConfigHandler.Items["BasePath"].VirtualPath;
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered(base.GetType(), this.ClientID + "_KindEditor"))
			{
				text = text.Replace("{path}", virtualPath);
				text = text.Replace("{controlId}", this.ClientID);
				text = text.Replace("{width}", (this.Width.ToString() == "") ? "100%" : this.Width.ToString());
				text = text.Replace("{height}", (this.Height.ToString() == "") ? "300" : this.Height.ToString());
				text = text.Replace("{readonlyMode}", this.Enabled ? "false" : "true");
				text = text.Replace("{afterCreate}", string.IsNullOrEmpty(this.afterCreate) ? "null" : this.afterCreate);
				text = text.Replace("{afterChange}", string.IsNullOrEmpty(this.afterChange) ? "null" : this.afterChange);
				text = text.Replace("{afterTab}", string.IsNullOrEmpty(this.afterTab) ? "null" : this.afterTab);
				text = text.Replace("{afterFocus}", string.IsNullOrEmpty(this.afterFocus) ? "null" : this.afterFocus);
				text = text.Replace("{afterBlur}", string.IsNullOrEmpty(this.afterBlur) ? "null" : this.afterBlur);
				text = text.Replace("{filterMode}", string.IsNullOrEmpty(this.FilterMode) ? "true" : this.FilterMode);
				if (string.IsNullOrEmpty(this.ThemeType) || ConfigHandler.ThemeType(this.ThemeType) == null)
				{
					text = text.Replace("{themeTyle}", "");
				}
				else
				{
					string newValue = string.Format("items : [{0}],", ConfigHandler.ThemeType(this.ThemeType).Items);
					text = text.Replace("{themeTyle}", newValue);
				}
				text = Regex.Replace(text, "[\\s\\r\\n\\t]+", " ");
				text += "\r";
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), this.ClientID + "_KindEditor", text);
			}
		}
	}
}
