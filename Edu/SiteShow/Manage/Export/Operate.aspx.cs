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
using System.IO;

namespace SiteShow.Manage.Export
{
    public partial class Operate : Extend.CustomPage
    {
        private string _uppath = "Temp";
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
            ////������ʦ
            //EntitiesInfo.Teacher th = Extend.LoginState.Accounts.Teacher;
            ////��������
            //EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            ////��ǰ��ʦ����Ŀγ�
            //List<EntitiesInfo.Course> cous = Business.Do<ICourse>().CourseAll(org.Org_ID, -1,th.Th_ID, null);
            //foreach (EntitiesInfo.Course c in cous)
            //{
            //    //�γ�ͼƬ
            //    if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
            //    {
            //        c.Cou_LogoSmall = Upload.Get[_uppath].Virtual + c.Cou_LogoSmall;
            //        c.Cou_Logo = Upload.Get[_uppath].Virtual + c.Cou_Logo;
            //    }
            //}
            //rptCourse.DataSource = cous;
            //rptCourse.DataBind();
        }

        protected void btnExpStudent_Click(object sender, EventArgs e)
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //�����ļ�
            string name = "ѧ����Ϣ����" + "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
            string filePath = Upload.Get[_uppath].Physics + name;
            //filePath = Business.Do<IStudent>().StudentExport4Excel(filePath, org.Org_ID, -1);
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
        
    }
}
