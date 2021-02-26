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
using System.Collections.Generic;


namespace SiteShow.Manage.Common1
{
    public partial class AddressSort :Extend.CustomPage
    {
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

                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                List<EntitiesInfo.AddressSort> eas = null;
                eas = Business.Do<IAddressList>().SortPager(acc.Acc_Id, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "Ads_Id" };
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
                    Business.Do<IAddressList>().SortDelete(Convert.ToInt16(id));
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
                Business.Do<IAddressList>().SortDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                EntitiesInfo.AddressSort entity = Business.Do<IAddressList>().SortSingle(id);
                entity.Ads_IsUse = !entity.Ads_IsUse;
                Business.Do<IAddressList>().SortSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                //Company.WeiSha.WebControl.GridView gv = (Company.WeiSha.WebControl.GridView)gr.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
                Business.Do<IAddressList>().SortRemoveUp(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        //下移
        protected void lbDown_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
                Business.Do<IAddressList>().SortRemoveDown(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
