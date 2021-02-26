using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace SiteShow.Manage.SOAP
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Position : IHttpHandler
    {
        //地址
        private string address = Common.Request.QueryString["address"].String ?? "";
        public void ProcessRequest(HttpContext context)
        {
            Common.Param.Method.Position posi = Common.Request.Position(address);
            context.Response.ContentType = "text/plain";
            string json = "{\"lng\":" + posi.Longitude + ",\"lat\":" + posi.Latitude + "}";
            context.Response.Write(json);
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
