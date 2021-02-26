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

namespace SiteShow.Manage.Pay
{
    public partial class PayList : Extend.CustomPage
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
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganRoot();
            EntitiesInfo.PayInterface[] eas = Business.Do<IPayInterface>().PayAll(org.Org_ID, null, null);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Pai_ID" };
            GridView1.DataBind();


        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
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
                Business.Do<IPayInterface>().PayDelete(Convert.ToInt32(id));
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
            Business.Do<IPayInterface>().PayDelete(id);
            BindData(null, null);
        }
        #region �б��е��¼�
        /// <summary>
        /// �޸��Ƿ�ʹ�õ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbEnable_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.PayInterface entity = Business.Do<IPayInterface>().PaySingle(id);
            entity.Pai_IsEnable = !entity.Pai_IsEnable;
            Business.Do<IPayInterface>().PaySave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IPayInterface>().PayRemoveUp(id))
                BindData(null, null);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IPayInterface>().PayRemoveDown(id))
                BindData(null, null);
        }
        #endregion
    }
}
