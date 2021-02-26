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

namespace SiteShow.Manage.Course
{
    public partial class List : Extend.CustomPage
    {
        private string _uppath = "Course";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {  
                BindData(null, null);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //������ʦ
            EntitiesInfo.Teacher th = Extend.LoginState.Accounts.Teacher;
            //��������
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //��ǰ��ʦ����Ŀγ�
            List<EntitiesInfo.Course> cous = Business.Do<ICourse>().CourseAll(org.Org_ID, -1,th.Th_ID, null);
            foreach (EntitiesInfo.Course c in cous)
            {
                //�γ�ͼƬ
                if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
                {
                    c.Cou_LogoSmall = Upload.Get[_uppath].Virtual + c.Cou_LogoSmall;
                    c.Cou_Logo = Upload.Get[_uppath].Virtual + c.Cou_Logo;
                }
            }
            rptCourse.DataSource = cous;
            rptCourse.DataBind();
        }
        /// <summary>
        /// ɾ���γ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<ICourse>().CourseDelete(id);
            this.Reload();
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            LinkButton ub = (LinkButton)sender;
            int id = int.Parse(ub.CommandArgument);
            //
            EntitiesInfo.Course entity = Business.Do<ICourse>().CourseSingle(id);
            entity.Cou_IsUse = !entity.Cou_IsUse;
            Business.Do<ICourse>().CourseSave(entity);
            BindData(null, null);
           
        }
    }
}
