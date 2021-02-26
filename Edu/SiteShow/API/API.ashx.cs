﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace SiteShow.API
{
    /// <summary>
    /// API 的摘要说明
    /// </summary>
    public class API : System.Web.UI.Page, IHttpHandler, IRequiresSessionState
    {

        public new void ProcessRequest(HttpContext context)
        {
            ViewData.Letter p = new ViewData.Letter(context);
            ViewData.DataResult result = ViewData.ExecuteMethod.ExecToResult(p);

            context.Response.ContentType = "text/plain";
            string resultTxt = "xml".Equals(p.ReturnType, StringComparison.CurrentCultureIgnoreCase) ? result.ToXml() : result.ToJson();
            //如果操作失败，则返回自定义状态码
            //if (!result.Success)
            //    context.Response.StatusCode = 405;
            context.Response.Write(resultTxt);
            context.Response.End();
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}