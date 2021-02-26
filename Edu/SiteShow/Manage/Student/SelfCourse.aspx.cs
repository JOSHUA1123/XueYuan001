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

namespace SiteShow.Manage.Student
{
    public partial class SelfCourse : Extend.CustomPage
    {
        private string _uppath = "Course";
        EntitiesInfo.Organization org;
        //ѧϰ��¼��datatable
        DataTable dtLog = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts st = this.Master.Account;
            if (st == null) return;
            org = Business.Do<IOrganization>().OrganCurrent();
            dtLog = Business.Do<IStudent>().StudentStudyCourseLog(this.Master.Account.Ac_ID);
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
            //��ǰѧ���Ŀγ�
            EntitiesInfo.Accounts st = this.Master.Account;
            if (st == null) return;
            //����Ŀγ�(�������õģ�
            List<EntitiesInfo.Course> cous = Business.Do<ICourse>().CourseForStudent(st.Ac_ID, null, 1,null,-1);
            foreach (EntitiesInfo.Course c in cous)
            {
                //�γ�ͼƬ
                if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
                    c.Cou_LogoSmall = Upload.Get[_uppath].Virtual + c.Cou_LogoSmall;                    
                c.Cou_IsStudy = true;
            }
            rptCourse.DataSource = cous;
            rptCourse.DataBind();
            plNoCourse.Visible = cous.Count < 1;
            //���ڿγ�
            List<EntitiesInfo.Course> cousexp = Business.Do<ICourse>().CourseForStudent(st.Ac_ID, null, 2, false, -1);
            foreach (EntitiesInfo.Course c in cousexp)
            {
                //�γ�ͼƬ
                if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
                    c.Cou_LogoSmall = Upload.Get[_uppath].Virtual + c.Cou_LogoSmall;
                c.Cou_IsStudy = true;
            }
            rptCourseExpire.DataSource = cousexp;
            rptCourseExpire.DataBind();
            divCourseExpire.Visible = cousexp.Count > 0;
        }
        /// <summary>
        /// ��ȡ�γ̵Ĺ�����Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string getBuyInfo(object id)
        {
            int couid = 0;
            int.TryParse(id.ToString(), out couid);
            Student_Course sc= Business.Do<ICourse>().StudentCourse(Extend.LoginState.Accounts.CurrentUser.Ac_ID, couid);
            if (sc == null) return "";
            if (sc.Stc_IsFree) return "��ѣ������ڣ�";
            if (sc.Stc_IsTry) return "����";
            return sc.Stc_StartTime.ToString("yyyy��MM��dd��") + " - " + sc.Stc_EndTime.ToString("yyyy��MM��dd��");
        }
        /// <summary>
        /// ȡ���γ�ѧϰ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSelected_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;            
            int couid = Convert.ToInt32(lb.CommandArgument);    //�γ�id            
            EntitiesInfo.Accounts st = this.Master.Account;     //��ǰѧ��       
            //ȡ��
            Business.Do<ICourse>().DelteCourseBuy(st.Ac_ID, couid);
            //���ص�ǰ��
            this.Reload();
        }
        /// <summary>
        /// �����ۼ�ѧϰʱ��
        /// </summary>
        /// <param name="studyTime"></param>
        /// <returns></returns>
        protected string CaleStudyTime(string studyTime)
        {
            int num = 0;
            int.TryParse(studyTime, out num);
            if (num < 60) return num + "����";
            //�������
            num = num / 60;
            int ss = num % 60;
            if (num < 60) return num + "����";
            //����Сʱ
            int hh = num / 60;
            int mm = num % 60;
            return string.Format("{0}Сʱ{1}����", hh, mm);
        }
        /// <summary>
        /// ��ȡ�ۼ�ѧϰʱ��
        /// </summary>
        /// <param name="studyTime"></param>
        /// <returns></returns>
        protected string GetstudyTime(string couid)
        {
            string studyTime = "0";
            if (dtLog != null)
            {
                foreach (DataRow dr in dtLog.Rows)
                {
                    if (dr["Cou_ID"].ToString() == couid)
                    {
                        studyTime = dr["studyTime"].ToString();
                    }
                }
                return CaleStudyTime(studyTime);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// ��ȡ���ѧϰʱ��
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        protected string GetLastTime(string couid)
        {
            DateTime? lastTime = null;
            if (dtLog != null)
            {
                foreach (DataRow dr in dtLog.Rows)
                {
                    if (dr["Cou_ID"].ToString() == couid)
                    {
                        lastTime = Convert.ToDateTime(dr["LastTime"]);
                    }
                }
            }
            if (lastTime == null) return "����û��ѧϰ��";
            return ((DateTime)lastTime).ToString();
        }
        /// <summary>
        /// ��ȡѧԱѧϰ�Ŀγ̼�¼
        /// </summary>
        /// <param name="couidstr"></param>
        /// <returns></returns>
        protected EntitiesInfo.Student_Course GetStc(string couidstr)
        {
            int couid = 0;
            int.TryParse(couidstr, out couid);
            return Business.Do<ICourse>().StudentCourse(this.Master.Account.Ac_ID, couid);
        }
    }
}