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
using System.Collections.Generic;

namespace SiteShow.Manage.Teacher
{
    public partial class Courses : Extend.CustomPage
    {
        EntitiesInfo.Organization org = null;
        //�Ƿ�Ϊ����Ա����״̬
        private bool isAdmin = Common.Request.QueryString["admin"].Boolean ?? false;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "�γ̹���";
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                init();
                BindData(null, null);
            }           
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected void init()
        {
            ddlSubject.Items.Clear();               
            EntitiesInfo.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(org.Org_ID, -1, "", null, 0, 0);
            ddlSubject.DataSource = sbjs;
            ddlSubject.DataTextField = "Sbj_Name";
            ddlSubject.DataValueField = "Sbj_ID";
            ddlSubject.DataBind();
            this.ddlSubject.Items.Insert(0, new ListItem(" -- רҵ -- ", "-1"));          
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //�ܼ�¼��
            bool? isUse = null;
            List<EntitiesInfo.Course> eas = null;
            int sbjid = Convert.ToInt32(ddlSubject.SelectedValue);
            //��ǰ��ʦ
            EntitiesInfo.Teacher th = Extend.LoginState.Accounts.Teacher;
            if (th == null) return;
            int count = 0;
            eas = Business.Do<ICourse>().CoursePager(org.Org_ID, sbjid, th.Th_ID, isUse, tbSear.Text.Trim(), "tax", Pager1.Size, Pager1.Index, out count);
            foreach (EntitiesInfo.Course s in eas)
            {
                if (string.IsNullOrEmpty(s.Cou_Intro) || s.Cou_Intro.Trim() == "") continue;
                if (s.Cou_Intro.Length > 20)
                {
                    s.Cou_Intro = s.Cou_Intro.Substring(0, 20) + "...";
                }
            }

            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Cou_ID" };
            GridView1.DataBind();
            Pager1.RecordAmount = count;    
            
        }
        /// <summary>
        /// ��ȡ��ǰרҵ�µĿγ�����
        /// </summary>
        /// <param name="sbjid"></param>
        /// <returns></returns>
        protected string GetCourseCount(object sbjid)
        {
            int sbj;
            int.TryParse(sbjid.ToString(), out sbj);
            int count = Business.Do<ICourse>().CourseOfCount(org.Org_ID, sbj, -1);
            return count.ToString();
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            BindData(null, null);
        }
        /// <summary>
        /// ��յ�ǰרҵ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbClear_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<ICourse>().CourseClear(id);
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ�ʹ�õ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Course entity = Business.Do<ICourse>().CourseSingle(id);
            entity.Cou_IsUse = !entity.Cou_IsUse;
            Business.Do<ICourse>().CourseSave(entity);
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
                Business.Do<ICourse>().CourseDelete(Convert.ToInt16(id));
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
            Business.Do<ICourse>().CourseDelete(id);
            BindData(null, null);

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {            
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ICourse>().CourseUp(id))
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {            
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ICourse>().CourseDown(id))
            {
                BindData(null, null);
            }                
        }
    }
}
