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
using System.Text.RegularExpressions;

namespace SiteShow.Manage.Student
{
    public partial class QuesError : Extend.CustomPage
    {
        //���ͷ��຺������
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts st = this.Master.Account;
            if (st == null) return;
            this.Form.DefaultButton = this.btnSear.UniqueID;
            if (!this.IsPostBack)
            {
                init();
                BindData(null, null);
            }
        }
        private void init()
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //ѧ��/רҵ
            EntitiesInfo.Subject[] subs = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, 0, 0);
            this.ddlSubject.DataSource = subs;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.DataBind();
            this.ddlSubject.Items.Insert(0, new ListItem(" -- ȫ�� -- ", "-1"));
            //����
            for (int i = 0; i < typeStr.Length; i++)
            {
                ddlType.Items.Add(new ListItem(typeStr[i], (i+1).ToString()));
            }
            this.ddlType.Items.Insert(0, new ListItem(" -- ȫ�� -- ", "-1"));
            
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //�ܼ�¼��
            int count = 0;
            EntitiesInfo.Accounts st = this.Master.Account;
            int sortid = Convert.ToInt32(ddlSubject.SelectedValue);
            int type = Convert.ToInt32(ddlType.SelectedValue);
            int diff = Convert.ToInt32(this.ddlDiff.SelectedValue);
            EntitiesInfo.Questions[] eas = null;
            eas = Business.Do<IStudent>().QuesPager(st.Ac_ID, sortid, -1,type,diff, Pager1.Size, Pager1.Index, out count);
            //ȥ������е�html��ǩ
            string regexstr = @"(<[^>]*>)|\r|\n|\s";
            foreach (EntitiesInfo.Questions q in eas)
            {
                if (!string.IsNullOrWhiteSpace(q.Qus_Title))
                    q.Qus_Title = Regex.Replace(q.Qus_Title, regexstr, string.Empty, RegexOptions.IgnoreCase);               
            }
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "qus_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbOpenMobile_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            int stid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
            foreach (string id in keys.Split(','))
            {
                Business.Do<IStudent>().QuesDelete(Convert.ToInt32(id), stid);
            }
            BindData(null, null);            
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
            int stid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
            Business.Do<IStudent>().QuesDelete(id, stid);
            BindData(null, null);           
        }
    }
}
