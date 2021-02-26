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
using System.Collections.Generic;
using System.IO;
using Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;
using ServiceInterfaces;
using System.Text.RegularExpressions;
using pili_sdk.pili;


namespace SiteShow.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            //ÊÇ²»ÊÇiPad
            isiPad.Text = Common.Browser.IsIPad.ToString();
            isMobile.Text = Common.Browser.IsMobile.ToString();
            isPhone.Text = Common.Browser.IsIPhone.ToString();
            lbOs.Text = Common.Browser.MobileOS;

            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            string u = _context.Request.ServerVariables["HTTP_USER_AGENT"];
            lbUseagent.Text = u;
        }

    }
}
