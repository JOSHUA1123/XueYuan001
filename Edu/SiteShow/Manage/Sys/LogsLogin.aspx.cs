﻿using System;
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
using WeiSha.WebControl;
namespace SiteShow.Manage.Sys
{
    public partial class LogsLogin : Extend.CustomPage
    {
        EntitiesInfo.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganRoot();
            if (!this.IsPostBack)
            {
                //EntitiesInfo.Position super = Business.Do<IPosition>().GetSuper();
                //superid = super.Posi_Id.ToString();
                init();
                BindData(null, null);
            }
        }

        protected void init()
        {
            EmpAccount[] eas = Business.Do<IEmployee>().GetAll(org.Org_ID);
            this.ddlEmp.DataSource = eas;
            ddlEmp.DataTextField = "Acc_Name";
            ddlEmp.DataValueField = "acc_id";
            ddlEmp.DataBind();
            this.ddlEmp.Items.Insert(0, new ListItem(" -- 所有员工 -- ", "-1"));
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //总记录数
            int count = 0;
            //当前选择的用户id
            int accid = Convert.ToInt16(this.ddlEmp.SelectedItem.Value);
            DateTime start = DateTime.Now.AddYears(-100);
            DateTime end = DateTime.Now.AddYears(100);
            if (tbStart.Text.Trim() != "")
            {
                start = Convert.ToDateTime(tbStart.Text.Trim());
            }
            if (this.tbEnd.Text.Trim() != "")
            {
                end = Convert.ToDateTime(tbEnd.Text.Trim());
            }
            Logs[] eas = null;
            if (accid == -1)
            {
                eas = Business.Do<ILogs>().GetPager("login", start, end, Pager1.Size, Pager1.Index, out count);
            }
            else
            {
                eas = Business.Do<ILogs>().GetPager(accid, "login", start, end, Pager1.Size, Pager1.Index, out count);
            }
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Log_Id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        } 
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {            
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<ILogs>().Delete(Convert.ToInt16(id));
            }
            BindData(null, null);            
        }
    }
}
