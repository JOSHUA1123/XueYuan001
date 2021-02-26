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
using Common;
namespace SiteShow.Json
{
    public partial class TestPaper : System.Web.UI.Page
    {       
        //学科的id
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        //获取当前学科下的所有试卷
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            EntitiesInfo.TestPaper[] kn = null;
            kn = Business.Do<ITestPaper>().PagerCount(org.Org_ID,id, -1, -1, true, 0);
            string tm = "[";
            for (int i = 0; i < kn.Length; i++)
            {
                kn[i].Tp_Intro = "";
                tm += "" + kn[i].ToJson();
                if (i < kn.Length - 1) tm += ",";
            }
            tm += "]";
            Response.Write(tm);
            Response.End();
        }
    }
}
