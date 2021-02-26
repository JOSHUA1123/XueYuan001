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


namespace SiteShow.Manage.Student
{
    public partial class TestArchives : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts st = this.Master.Account;
            if (st == null) return;
            //this.Form.DefaultButton = this.btnSear.UniqueID;
            if (!IsPostBack)
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
            EntitiesInfo.Accounts st = this.Master.Account;
            if (st == null) return;
            EntitiesInfo.TestResults[] trs = null;
            trs = Business.Do<ITestPaper>().ResultsPager(st.Ac_ID, -1, -1, "", Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = trs;
            GridView1.DataKeyNames = new string[] { "Tr_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;

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
            {
                Business.Do<ITestPaper>().ResultsDelete(Convert.ToInt16(id));
            }
            BindData(null, null);
            
        }
    }
}
