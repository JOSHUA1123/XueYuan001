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

using ServiceInterfaces;
using EntitiesInfo;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Common;
namespace SiteShow.Json
{
    public partial class ExamResults : System.Web.UI.Page
    {       
        //试卷id，考试id,员工id
        private int tpid = Common.Request.Form["tpid"].Int32 ?? 0;
        private int examid = Common.Request.Form["examid"].Int32 ?? 0;
        private int accid = Common.Request.Form["accid"].Int32 ?? 0;
        //获取当前学科下的所有试卷
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取答题信息
          EntitiesInfo.ExamResults exr = Business.Do<IExamination>().ResultSingle(examid, tpid, accid);           
            if (exr == null)
            {
                Response.Write("0");

            }
            else
            {
                Response.Write(exr.Exr_Results);
            }  
            Response.End();
        }       
    }
}
