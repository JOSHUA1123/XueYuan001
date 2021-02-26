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

namespace SiteShow.Manage.Content
{
    public partial class DownloadRecycle : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlColumnBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 院系下拉绑定
        /// </summary>
        private void ddlColumnBind()
        {
            //EntitiesInfo.DownloadCategory[] pc = Business.Do<IContents>().CategoryAll(null, true);
            //this.ddlColumn.DataSource = pc;
            //this.ddlColumn.DataTextField = "Dc_Name";
            //this.ddlColumn.DataValueField = "Dc_Id";
            //this.ddlColumn.DataBind();
            //
            this.ddlColumn.Items.Insert(0, new ListItem(" -- 所有分类 -- ", "-1"));
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                //当前选择的栏目id
                int col = Convert.ToInt16(ddlColumn.SelectedItem.Value);
                EntitiesInfo.Download[] eas = null;
                EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
                eas = Business.Do<IContents>().DownloadPager(org.Org_ID, col, true, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "Di_id" };
                GridView1.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
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
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<IContents>().DownloadDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RecoverEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<IContents>().DownloadRecover(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
