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
using System.IO;

using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;

namespace SiteShow.Manage.Sys
{
    public partial class MenuRoot : Extend.CustomPage
    {      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                EntitiesInfo.ManageMenu[] mm;
                mm = Business.Do<IManageMenu>().GetRoot("func");
                //mm.Length
                GridView1.DataSource = mm;
                GridView1.DataKeyNames = new string[] { "MM_Id" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ɾ��
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
                    Business.Do<IManageMenu>().RootDelete(Convert.ToInt32(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
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
                EntitiesInfo.ManageMenu entity = Business.Do<IManageMenu>().GetSingle(id);
                entity.MM_IsShow = !entity.MM_IsShow;
                Business.Do<IManageMenu>().Save(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// �޸��Ƿ�ʹ�õ�״̬
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
                EntitiesInfo.ManageMenu entity = Business.Do<IManageMenu>().GetSingle(id);
                entity.MM_IsUse = !entity.MM_IsUse;
                Business.Do<IManageMenu>().Save(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        //����
        protected void lbUp_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                //Company.WeiSha.WebControl.GridView gv = (Company.WeiSha.WebControl.GridView)gr.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);

                if (Business.Do<IManageMenu>().RemoveUp(id))
                {
                    BindData(null, null);
                }
                else
                {
                    //Alert("�����Ѿ������������������ˣ��޷����ƣ�");
                    //GridView1.Rows[0]
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        //����
        protected void lbDown_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
                if (Business.Do<IManageMenu>().RemoveDown(id))
                {
                    BindData(null, null);
                }
                else
                {
                    //Alert("�����Ѿ������������������׶ˣ��޷����ƣ�");
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
