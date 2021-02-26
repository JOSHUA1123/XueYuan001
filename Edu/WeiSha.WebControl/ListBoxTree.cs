using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.WebControl.Tree;

namespace WeiSha.WebControl
{
    [ToolboxData("<{0}:ListBoxTree runat=server></{0}:ListBoxTree>")]
    [DefaultProperty("Text")]
    public class ListBoxTree : ListBox
    {
        [DefaultValue("")]
        [Localizable(true)]
        [Category("属性字段")]
        [Bindable(true)]
        public string IdKeyName
        {
            get
            {
                return (string)this.ViewState["IdKeyName"] ?? string.Empty;
            }
            set
            {
                this.ViewState["IdKeyName"] = (object)value;
            }
        }

        [DefaultValue("")]
        [Category("属性字段")]
        [Localizable(true)]
        [Bindable(true)]
        public string ParentIdKeyName
        {
            get
            {
                return (string)this.ViewState["ParentIdKeyName"] ?? string.Empty;
            }
            set
            {
                this.ViewState["ParentIdKeyName"] = (object)value;
            }
        }

        [Category("属性字段")]
        [DefaultValue("")]
        [Localizable(true)]
        [Bindable(true)]
        public int Root
        {
            get
            {
                string str = (string)this.ViewState["Root"];
                if (str != null)
                    return Convert.ToInt32(str);
                return 0;
            }
            set
            {
                this.ViewState["Root"] = (object)value;
            }
        }

        [Localizable(true)]
        [Category("属性字段")]
        [DefaultValue("")]
        [Bindable(true)]
        public string TaxKeyName
        {
            get
            {
                return (string)this.ViewState["TaxKeyName"] ?? string.Empty;
            }
            set
            {
                this.ViewState["TaxKeyName"] = (object)value;
            }
        }

        [Localizable(true)]
        [DefaultValue("")]
        [Category("属性字段")]
        [Bindable(true)]
        public string TypeKeyName
        {
            get
            {
                return (string)this.ViewState["TypeKeyName"] ?? string.Empty;
            }
            set
            {
                this.ViewState["TypeKeyName"] = (object)value;
            }
        }

        /// <summary>
        /// 重写绑定，处理数据源
        /// 
        /// </summary>
        public override void DataBind()
        {
            this._TransctionDataSource();
            base.DataBind();
            if (!(this.DataSource is DataTable))
                return;
            DataTable dt = (DataTable)this.DataSource;
            foreach (ListItem listItem in this.Items)
            {
                listItem.Attributes.Add("pid", this._GetPID(listItem.Value, dt));
                if (!string.IsNullOrWhiteSpace(this.TypeKeyName) && this.TypeKeyName.Trim() != "")
                    listItem.Attributes.Add("type", this._GetType(listItem.Value, dt));
            }
        }

        /// <summary>
        /// 处理数据源，将它转换成树形
        /// 
        /// </summary>
        /// 
        /// <returns/>
        protected void _TransctionDataSource()
        {
            if (this.DataSource == null)
                return;
            DataTable dt = (DataTable)null;
            if (this.DataSource is DataTable)
                dt = this.DataSource as DataTable;
            if (this.DataSource is IList && !(this.DataSource is Array))
            {
                IList list = this.DataSource as IList;
                if (list == null || list.Count < 1)
                    return;
                object[] objArray = new object[list.Count];
                for (int index = 0; index < objArray.Length; ++index)
                    objArray[index] = list[index];
                this.DataSource = (object)objArray;
            }
            if (this.DataSource is Array)
            {
                Array arr = this.DataSource as Array;
                if (arr.Length < 1)
                    return;
                dt = ObjectArrayToDataTable.To(arr);
            }
            if (dt == null)
                return;
            DataTable dataTable = new DataTableTree()
            {
                IdKeyName = this.IdKeyName,
                ParentIdKeyName = this.ParentIdKeyName,
                TaxKeyName = this.TaxKeyName,
                Root = this.Root
            }.BuilderTree(dt);
            foreach (DataRow dataRow in (InternalDataCollectionBase)dataTable.Rows)
                dataRow[this.DataTextField] = (object)(dataRow["Tree"].ToString() + " " + dataRow[this.DataTextField].ToString());
            this.DataSource = (object)dataTable;
        }

        /// <summary>
        /// 根据当前项的id，取其父id
        /// 
        /// </summary>
        /// <param name="id"/><param name="dt"/>
        /// <returns/>
        private string _GetPID(string id, DataTable dt)
        {
            foreach (DataRow dataRow in (InternalDataCollectionBase)dt.Rows)
            {
                if (dataRow[this.IdKeyName].ToString() == id.ToString())
                    return dataRow[this.ParentIdKeyName].ToString();
            }
            return "0";
        }

        private string _GetType(string id, DataTable dt)
        {
            foreach (DataRow dataRow in (InternalDataCollectionBase)dt.Rows)
            {
                if (dataRow[this.IdKeyName].ToString() == id.ToString())
                    return dataRow[this.TypeKeyName].ToString();
            }
            return "";
        }
    }
}
