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
    public partial class Download : Extend.CustomPage
    {
        private string _uppath = "Download";
        //栏目分类
        int colid = Common.Request.QueryString["colid"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
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
                EntitiesInfo.Download[] eas = null;
                bool? istop = !cbIsTop.Checked ? null : (bool?)true;
                bool? ishot = !cbIsHot.Checked ? null : (bool?)true;
                bool? isrec = !cbIsRec.Checked ? null : (bool?)true;
                EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
                eas = Business.Do<IContents>().DownloadPager(org.Org_ID, colid, tbSear.Text, "", false, null, ishot, isrec, istop, Pager1.Size, Pager1.Index, out count);
                foreach (EntitiesInfo.Download di in eas)
                {
                    di.Dl_FilePath = Upload.Get[_uppath].Virtual + di.Dl_FilePath;
                }
                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "Dl_Id" };
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
                    Business.Do<IContents>().DownloadIsDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IContents>().DownloadIsDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                EntitiesInfo.Download entity = Business.Do<IContents>().DownloadSingle(id);
                entity.Dl_IsShow = !entity.Dl_IsShow;
                Business.Do<IContents>().DownloadSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

      
    }
}
