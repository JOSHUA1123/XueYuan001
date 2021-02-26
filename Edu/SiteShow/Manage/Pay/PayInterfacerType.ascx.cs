﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using ServiceInterfaces;
using EntitiesInfo;
namespace SiteShow.Manage.Pay
{
    public partial class PayInterfacerType : System.Web.UI.UserControl
    {
        //支付接口的id
        protected int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        public DropDownList DdlInterFace
        {
            get { return ddlInterFace; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                EntitiesInfo.PayInterface pi = Business.Do<IPayInterface>().PaySingle(id);
                if (pi != null)
                {
                    ListItem li = ddlInterFace.Items.FindByText(pi.Pai_Pattern);
                    if (li != null)
                    {
                        ddlInterFace.SelectedIndex = -1;
                        li.Selected = true;
                    }
                }
                else
                {
                    string pagename = Common.Request.Page.FileName;
                    ListItem li = null;
                    foreach (ListItem item in ddlInterFace.Items)
                    {
                        string page = item.Value;
                        if (item.Value.Equals(pagename, StringComparison.CurrentCultureIgnoreCase))
                        {
                            li = item;
                            break;
                        }
                    }
                    if (li != null)
                    {
                        ddlInterFace.SelectedIndex = -1;
                        li.Selected = true;
                    }
                }
            }
        }
    }
}