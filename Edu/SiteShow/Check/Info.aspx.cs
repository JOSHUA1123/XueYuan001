using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using ServiceInterfaces;
namespace SiteShow.Check
{
    public partial class Info : System.Web.UI.Page
    {
        //当前所在机构
        protected EntitiesInfo.Organization Organ { get; private set; }
        //各项值
        protected string sbjcount, couCount, testCount, quesCount, stCount, newsCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Organ = Business.Do<IOrganization>().OrganCurrent();
            lbOrgName.Text = Organ.Org_Name;
            //专业数
            this.zy.Text=sbjcount = Business.Do<ISubject>().SubjectOfCount(this.Organ.Org_ID, true, -1).ToString();
            //课程数
            this.kc.Text=couCount = Business.Do<ICourse>().CourseOfCount(this.Organ.Org_ID, -1, -1).ToString();
            //考试数
            this.ks.Text=testCount = Business.Do<ITestPaper>().PagerOfCount(this.Organ.Org_ID, -1, -1, -1, true).ToString();
            //试题数
            this.st.Text=quesCount = Business.Do<IQuestions>().QuesOfCount(this.Organ.Org_ID, -1, -1, -1, -1, true).ToString();
            //学员数
            this.sy.Text=stCount = Business.Do<IAccounts>().AccountsOfCount(this.Organ.Org_ID, null).ToString();
            //资讯数
            this.zx.Text=newsCount = Business.Do<IContents>().ArticleOfCount(this.Organ.Org_ID, -1).ToString();
            
        }
    }
}