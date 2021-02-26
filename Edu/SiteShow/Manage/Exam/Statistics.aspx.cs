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

namespace SiteShow.Manage.Exam
{
    public partial class Statistics : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
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
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //�ܼ�¼��
            int count = 0;
            EntitiesInfo.Examination[] eas = null;
            eas = Business.Do<IExamination>().GetPager(org.Org_ID, null, null, null, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Exam_ID" };
            GridView1.DataBind();
            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }        
        /// <summary>
        /// ��ȡ�ο���Ա����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string getGroupType(string gtype,string uid)
        {
            int type = Convert.ToInt32(gtype);
            if (type == 1) return "ȫ��Ա��";
            if (type == 2) return "ָ��Ժϵ";
            if (type == 3) return "ָ������";
            return "";
        }
        /// <summary>
        /// ��ȡ��ǰ���Եļ�����
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        protected string getPassRate(string uid)
        {
            double rate = Business.Do<IExamination>().PassRate4Theme(uid);
            rate = Math.Round(rate * 100) / 100;
            return rate.ToString();
        }
    }
}
