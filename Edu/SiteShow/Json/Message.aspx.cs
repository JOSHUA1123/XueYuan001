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
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace SiteShow.Json
{
    public partial class Message : System.Web.UI.Page
    {
        //返回多少条
        private int count = Common.Request.QueryString["count"].Int32 ?? 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.MessageBoard[] mbs = null;
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            mbs = Business.Do<IMessageBoard>().ThemeCount(org.Org_ID, -1, "", count);
            string tm = "[";
            for (int i = 0; i < mbs.Length; i++)
            {
                mbs[i].Mb_Content = "";
                tm += "" + mbs[i].ToJson();
                if (i < mbs.Length - 1) tm += ",";
            }
            tm += "]";
            Response.Write(tm);
            Response.End();
        }
    }
}
