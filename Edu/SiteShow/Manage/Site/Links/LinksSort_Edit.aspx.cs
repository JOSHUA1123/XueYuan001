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

namespace SiteShow.Manage.Site.Links
{
    public partial class LinksSort_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            EntitiesInfo.LinksSort mm = id == 0 ? new EntitiesInfo.LinksSort() : Business.Do<ILinks>().SortSingle(id);   
            if (mm == null) return;
            this.EntityBind(mm);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {

            EntitiesInfo.LinksSort mm = id == 0 ? new EntitiesInfo.LinksSort() : Business.Do<ILinks>().SortSingle(id);
            mm = this.EntityFill(mm) as EntitiesInfo.LinksSort;            
            try
            {
                //确定操作
                if (id == 0)
                {
                    Business.Do<ILinks>().SortAdd(mm);
                }
                else
                {
                    Business.Do<ILinks>().SortSave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
