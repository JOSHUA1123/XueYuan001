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

namespace SiteShow.Manage.Site
{
    public partial class Statistics_Analysis : Extend.CustomPage
    {
        //考试主题的id
        private int id = Common.Request.QueryString["id"].Int32 ?? 0; 
        //各场次
        //EntitiesInfo.Examination[] exams = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }

        void fill()
        {
            try
            {
                //考试主题的平均成绩
                EntitiesInfo.Examination theme = Business.Do<IExamination>().ExamSingle(id);
                if (theme == null) return;
                //当前考试的所有成绩
                DataTable dt = null;
                if (theme.Exam_GroupType == 3)
                {
                    //dt = Business.Do<IExamination>().Analysis4Team(id);
                }
                else
                {
                    //dt = Business.Do<IExamination>().Analysis4Depart(id);
                }
                gvList.DataSource = dt;
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            
           
        }
        
    }
}
