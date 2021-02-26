using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;

namespace SiteShow.Ajax
{
    /// <summary>
    /// Notices 的摘要说明
    /// </summary>
    public class Notices : IHttpHandler
    {
        int index = Common.Request.QueryString["index"].Int32 ?? 1;
        int size = Common.Request.QueryString["size"].Int32 ?? 1;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sum = 0;
           EntitiesInfo.Notice[] notice = Business.Do<INotice>().GetPager(org.Org_ID, true, "", size, index, out sum);
            string tm = "{\"sum\":"+sum+",\"object\":[";
            for (int i = 0; i < notice.Length; i++)
            {
                notice[i].No_Context = "";
                tm += "" + notice[i].ToJson();
                if (i < notice.Length - 1) tm += ",";
            }
            tm += "]}";
            context.Response.Write(tm);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}