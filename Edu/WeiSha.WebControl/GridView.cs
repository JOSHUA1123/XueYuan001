using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace WeiSha.WebControl
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:GridView runat=server></{0}:GridView>")]
    public class GridView : System.Web.UI.WebControls.GridView
    {
        [Localizable(true)]
        [Category("Appearance")]
        [Bindable(true)]
        [DefaultValue("")]
        public string SelectBoxKeyName
        {
            get
            {
                return (string)this.ViewState["SelectBoxKeyName"] ?? "SelectBox";
            }
            set
            {
                this.ViewState["SelectBoxKeyName"] = (object)value;
            }
        }

        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public bool ShowSelectBox
        {
            get
            {
                string str = (string)this.ViewState["ShowSelectBox"];
                return Convert.ToBoolean(str == null ? "True" : str);
            }
            set
            {
                this.ViewState["ShowSelectBox"] = (object)value.ToString();
            }
        }

        /// <summary>
        /// 获取选中数据行的主键值
        /// 
        /// </summary>
        [DefaultValue("")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        public string GetKeyValues
        {
            get
            {
                if (this.DataKeyNames.Length < 1)
                    return "";
                string str = "";
                foreach (GridViewRow gridViewRow in this.Rows)
                {
                    if (gridViewRow.RowType == DataControlRowType.DataRow && ((CheckBox)gridViewRow.FindControl(this.SelectBoxKeyName)).Checked)
                        str = str + this.DataKeys[gridViewRow.RowIndex].Value.ToString() + ",";
                }
                if (str.IndexOf(",") > -1)
                    str = str.Substring(0, str.LastIndexOf(","));
                return str;
            }
        }

        /// <summary>
        /// 要隐藏的列
        /// 
        /// </summary>
        [DefaultValue("")]
        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        public string HideColumn
        {
            get
            {
                return (string)this.ViewState["HideColumn"] ?? "";
            }
            set
            {
                this.ViewState["HideColumn"] = (object)value.ToString();
            }
        }

        [Bindable(true)]
        [Localizable(true)]
        [Category("Appearance")]
        [DefaultValue("true")]
        public bool IsEncrypt
        {
            get
            {
                string str = (string)this.ViewState["IsEncrypt"];
                return Convert.ToBoolean(str == null ? "True" : str);
            }
            set
            {
                this.ViewState["IsEncrypt"] = (object)value.ToString();
            }
        }

        [Localizable(true)]
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("id")]
        public string PrimaryKey
        {
            get
            {
                return (string)this.ViewState["PrimaryKey"] ?? "id";
            }
            set
            {
                this.ViewState["PrimaryKey"] = (object)value.ToString();
            }
        }

        protected override void OnDataBound(EventArgs e)
        {
            if (this.CssClass == string.Empty)
                this.CssClass = "GridView";
            this.Attributes.Add("ControlType", "GridView");
            this.Attributes.Add("ControlId", this.ID);
            this.Attributes.Add("HideColumn", this.HideColumn);
            this.Attributes.Add("style", "display:none");
            base.OnDataBound(e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.ShowSelectBox)
            {
                TemplateField templateField = new TemplateField();
                templateField.HeaderText = "<input type=\"checkbox\" name=\"selectBox\" id=\"selectBox\" />";
                CheckTemplate checkTemplate = new CheckTemplate(this.SelectBoxKeyName);
                templateField.ItemTemplate = (ITemplate)checkTemplate;
                this.Columns.Insert(0, (DataControlField)templateField);
            }
            base.OnInit(e);
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.Write("GridView_Init(\"" + this.ID + "\");");
            writer.RenderEndTag();
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("RowType", "DataRow");
                if (this.DataKeyNames != null && this.DataKeyNames.Length > 0)
                {
                    string encryptStr = this.DataKeys[e.Row.RowIndex].Value.ToString();
                    e.Row.Attributes.Add("DataKey", encryptStr);
                    string str = encryptStr;
                    if (this.IsEncrypt)
                        str = HttpUtility.UrlEncode(DataConvert.EncryptForBase64(encryptStr));
                    e.Row.Attributes.Add("EncryptKey", str);
                    e.Row.Attributes.Add("PrimaryKey", this.PrimaryKey);
                }
                if (this.ShowSelectBox)
                {
                    e.Row.Cells[0].Attributes.Add("ItemType", "SelectBox");
                    e.Row.Cells[0].Attributes.Add("class", "center noprint");
                }
            }
            if (e.Row.RowType == DataControlRowType.Header && this.ShowSelectBox)
                e.Row.Cells[0].Attributes.Add("class", "center noprint");
            base.OnRowDataBound(e);
        }
    }
}
