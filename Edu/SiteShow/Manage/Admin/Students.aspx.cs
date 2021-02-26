using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;
using System.IO;

namespace SiteShow.Manage.Admin
{
    public partial class Students : Extend.CustomPage
    {
        EntitiesInfo.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                init();
                BindData(null, null);
            }
        }
        private void init()
        {
            EntitiesInfo.StudentSort[] sort = Business.Do<IStudent>().SortAll(org.Org_ID, true);
            ddlSort.DataSource = sort;
            ddlSort.DataBind();
            ddlSort.Items.Insert(0, new ListItem(" -- ���� -- ", "-1"));
            this.SearchBind(this.searchBox);
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //�ܼ�¼��
            int count = 0;
            int sortid = Convert.ToInt32(ddlSort.SelectedValue);
            EntitiesInfo.Accounts[] eas = null;
            eas = Business.Do<IAccounts>().AccountsPager(org.Org_ID, sortid, null, tbAccName.Text, tbName.Text.Trim(), tbPhone.Text.Trim(), Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "ac_id" };
            GridView1.DataBind();
 
            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Qurey = this.SearchQuery(this.searchBox);
            Pager1.Index = 1;
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Accounts entity = Business.Do<IAccounts>().AccountsSingle(id);
            entity.Ac_IsUse = !entity.Ac_IsUse;
            Business.Do<IAccounts>().AccountsSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// ע������Ƿ�ͨ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbPass_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Accounts entity = Business.Do<IAccounts>().AccountsSingle(id);
            entity.Ac_IsPass = !entity.Ac_IsPass;
            Business.Do<IAccounts>().AccountsSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<IAccounts>().AccountsDelete(Convert.ToInt32(id));
            }
            BindData(null, null);            
        }
        /// <summary>
        /// ����ѧԱ��Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void outputEvent(object sender, EventArgs e)
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //�����ļ�
            string name = "ѧ����Ϣ����" + "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
            string filePath = Upload.Get["Temp"].Physics + name;
            //filePath = Business.Do<IStudent>().StudentExport4Excel(filePath, org.Org_ID, -1);
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {            
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<IAccounts>().AccountsDelete(id);
            BindData(null, null);           
        }
    }
}
