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


namespace SiteShow.Manage.Teacher
{
    public partial class Archives : Extend.CustomPage
    {
        EntitiesInfo.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!IsPostBack)
            {
                InitBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind()
        {
            
            //    //ѧ��/רҵ
            //EntitiesInfo.Subject[] subs = Business.Do<ISubject>().SubjectCount(org.Org_ID, true, 0);
            //this.ddlSubject.DataSource = subs;
            //this.ddlSubject.DataTextField = "Sbj_Name";
            //this.ddlSubject.DataValueField = "Sbj_ID";
            //this.ddlSubject.DataBind();
            //ddlSubject.Items.Insert(0, new ListItem(" -- ȫ�� -- ", "-1"));
            ////Ĭ��ѡ���������ڵ�רҵ
            //EntitiesInfo.Team team = Business.Do<ITeam>().TeamSingle(Extend.LoginState.Admin.CurrentUser.Team_ID);
            //if (team != null)
            //{
            //    EntitiesInfo.Subject sbj = Business.Do<ISubject>().SubjectSingle(team.Sbj_ID);
            //    if (sbj != null)
            //    {
            //        ListItem liDdl = ddlSubject.Items.FindByValue(sbj.Sbj_ID.ToString());
            //        if (liDdl != null) liDdl.Selected = true;
            //    }
            //}
            
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //�ܼ�¼��
            int count = 0;
            EntitiesInfo.Examination[] eas = null;
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //ʱ������
            int spanType = Convert.ToInt32(ddlTime.SelectedValue);
            DateTime? start = null;
            DateTime? end = null;
            //����ʱ��
            if (spanType == -1)
            {
                eas = Business.Do<IExamination>().GetPager(org.Org_ID, null, null, null, tbTheme.Text, Pager1.Size, Pager1.Index, out count);
            }
            else
            {
                //����
                if (spanType == 1)
                {
                    start = (DateTime?)DateTime.Now.Date;
                    end = (DateTime?)DateTime.Now.AddDays(1).Date;
                }
                //����
                if (spanType == 2)
                {
                    DateTime week = (DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1)).Date;
                    start = (DateTime?)week;
                    end = (DateTime?)week.AddDays(7);
                }
                //����
                if (spanType == 3)
                {
                    DateTime month = (DateTime.Now.AddDays(-DateTime.Now.Day + 1)).Date;
                    start = (DateTime?)month;
                    end = (DateTime?)month.AddMonths(1);
                }
                eas = Business.Do<IExamination>().GetPager(org.Org_ID, start, end, null, tbTheme.Text, Pager1.Size, Pager1.Index, out count);
            }
            rtpExamThem.DataSource = eas;
            rtpExamThem.DataBind();

            Pager1.RecordAmount = count;
        }
        protected void rtpExamThem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lbTitle = (Label)e.Item.FindControl("lbTitle");
            string name = lbTitle.Text;
            //���Գ���
            Repeater rpt = (Repeater)e.Item.FindControl("rtpExam");
            EntitiesInfo.Examination[] exams = Business.Do<IExamination>().ExamItem(lbTitle.CssClass);
            for (int i = 0; i < exams.Length; i++)
            {
                DateTime examDate = exams[i].Exam_Date < DateTime.Now.AddYears(-100) ? DateTime.Now : (DateTime)exams[i].Exam_Date;
                exams[i].Exam_Date = examDate.AddYears(100) < DateTime.Now ? DateTime.Now : examDate;
            }
            rpt.DataSource = exams;
            rpt.DataBind();
        }
        /// <summary>
        /// ��ȡ�ο���Ա����
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected string getGroupType(string type,string uid)
        {
            if (type == "1") return "ȫ��ѧ��";
            if (type == "2")
            {
                EntitiesInfo.StudentSort[] sts = Business.Do<IExamination>().GroupForStudentSort(uid);
                string strDep = "";
                for (int i = 0; i < sts.Length; i++)
                {
                    strDep += sts[i].Sts_Name;
                    if (i < sts.Length - 1) strDep += ",";
                }
                return strDep;
            }
            return "";
        }
        /// <summary>
        /// ��ȡ��ǰ���Ե�ƽ����
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        protected double getAvg4Exam(object examid)
        {
            int id = Convert.ToInt32(examid);
            double tm = Business.Do<IExamination>().Avg4Exam(id);
            tm = Math.Round(tm * 100) / 100;
            return tm;
        }
        protected int getNumber4Exam(object examid)
        {
            int id = Convert.ToInt32(examid);
            return  Business.Do<IExamination>().Number4Exam(id);
        }
        /// <summary>
        /// �жϵ�ǰ�����Ƿ�Ҫ��ʾ�������Ծ���ť
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        protected bool getIsCorrect(object examid)
        {
            int id = Convert.ToInt32(examid);
            //�����������û���˿��ԣ�����Ҫ����
            int students= Business.Do<IExamination>().Number4Exam(id);
            if (students < 1) return false;
            EntitiesInfo.Examination exas = Business.Do<IExamination>().ExamSingle(id);
            EntitiesInfo.TestPaper pager = Business.Do<ITestPaper>().PagerSingle(exas.Tp_Id);
            if (pager == null) return false;
            EntitiesInfo.TestPaperItem[] items = Business.Do<ITestPaper>().GetItemForAny(pager);
            foreach (EntitiesInfo.TestPaperItem ti in items)
            {
                if (ti.TPI_Type == 4) return true;
            }
            return false;
        }
    }
}
