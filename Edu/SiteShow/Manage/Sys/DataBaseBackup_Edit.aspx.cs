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

namespace SiteShow.Manage.Sys
{
    public partial class DataBaseBackup_Edit : Extend.CustomPage
    {
        //�����ļ���
        private string backfile = Common.Request.QueryString["id"].Decrypt().String ?? "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                full();
            }
           
        }
        private void full()
        {
            try
            {
                if (backfile != String.Empty)
                {
                    FileInfo file = new FileInfo(backfile);
                    //�ļ�����
                    this.tbName.Text = file.Name.Substring(0, file.Name.LastIndexOf('.'));
                    //�ļ���С
                    lbSize.Text = (file.Length / 1024).ToString() + " Kb";
                    //����ʱ��
                    lbTime.Text = file.CreationTime.ToString();

                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo file = new FileInfo(backfile);
                //����·��
                string path = file.FullName;
                path = path.Substring(0, path.LastIndexOf('\\') + 1);
                //��׺��
                string ext = file.Extension.ToLower();
                if (ext == "backup" || ext == "bak")
                {
                    //Ŀ���ļ�
                    string newBack = path + tbName.Text + ext;
                    file.MoveTo(newBack);
                }
                //��������
                //this.Response.Redirect("DataBaseBackup_Edit.aspx?id=" + Server.UrlEncode(newBack));
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ɾ���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //Business.Do<IPosition>().Delete(id);
            //Master.AlertAndClose("�ɹ�ɾ����");
        }

        

    }
}
