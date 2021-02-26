using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using Common;
using ServiceInterfaces;

namespace SiteShow.Manage.Site
{
    /// <summary>
    /// ��¼��վ������
    /// </summary>
    public class FlowNumber : IHttpHandler
    {
        #region IHttpHandler ��Ա

        public bool IsReusable
        {
            get { return true; } 
        }

        public void ProcessRequest(HttpContext context)
        {            
            string filePath = context.Request.FilePath;
            int cookie = Common.Request.Cookies["flowNumber"].Int32 ?? 0;
            if (cookie < 1)
            {
                context.Response.Cookies.Add(new HttpCookie("flowNumber", "1"));
                int flow = Business.Do<ISystemPara>()["flowNumber"].Int32 ?? 0;
                flow++;
                Business.Do<ISystemPara>().Save("flowNumber", flow.ToString());

            }
            StreamReader read = new StreamReader(context.Server.MapPath(filePath));
            string html = read.ReadToEnd();
            read.Close();
            context.Response.Write(html);            
        }
        #endregion

    }
}
