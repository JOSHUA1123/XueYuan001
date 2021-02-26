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


using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;

namespace SiteShow.Manage.Sys
{
    public partial class LimitDomain : Extend.CustomPage
    {
        //��������Ա��ɫ��id
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
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
            //�ܼ�¼��
            int count = 0;
            EntitiesInfo.LimitDomain[] eas = null;
            eas = Business.Do<ILimitDomain>().DomainPager(null, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "ld_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbIsUse_Click(object sender, EventArgs e)
        {           
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.LimitDomain entity = Business.Do<ILimitDomain>().DomainSingle(id);
            entity.LD_IsUse = !entity.LD_IsUse;
            Business.Do<ILimitDomain>().DomainSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
                Business.Do<ILimitDomain>().DomainDelete(Convert.ToInt32(id));
            BindData(null, null);            
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {            
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<ILimitDomain>().DomainDelete(id);
            BindData(null, null);           
        }
    }
}
