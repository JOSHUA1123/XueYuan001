using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using Extend;
using VTemplate.Engine;
using System.Web.SessionState;
namespace SiteShow.Help
{
    /// <summary>
    /// ViewData方法的调用说明
    /// </summary>
    public class API : BasePage, IRequiresSessionState
    {
           
        protected override void InitPageTemplate(HttpContext context)
        {
           
        }
      
    }
}