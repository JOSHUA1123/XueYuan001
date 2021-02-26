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

namespace SiteShow.Manage.Panel
{
    public partial class Authorization : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) init();
            //�����ϴ�����Ȩ�ļ�
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                HttpPostedFile file=null;
                for (int i = 0; i < this.Request.Files.Count; ++i)                
                    file = this.Request.Files[i];
                if (file == null || file.ContentLength==0) return;
                if (!Extend.LoginState.Admin.IsSuperAdmin)
                {
                    lbShow.Text = "*�˲������޳�������Ա";
                    return;
                }
                //
                if (file.FileName.ToLower() != "license.txt")
                {
                    lbShow.Text = "*���ϴ�license.txt�ļ�";
                }
                else
                {
                    string serverfile = HttpContext.Current.Server.MapPath("\\") + file.FileName;
                    try
                    {
                        if (System.IO.File.Exists(serverfile))
                        {
                            string newfile = file.FileName.Substring(0, file.FileName.IndexOf("."));
                            newfile += "(" + DateTime.Now.ToString("yyyy-M-d hhmmss") + ").txt";
                            System.IO.File.Move(serverfile, HttpContext.Current.Server.MapPath("\\") + newfile);
                        }
                        file.SaveAs(serverfile);
                        lbShow.Text = "*��Ȩ�ļ��ϴ��ɹ���";
                        Common.License.Value.Init();
                        init();
                    }
                    catch
                    {
                        lbShow.Text = "*��Ȩ�ļ��޷�д�룡";
                    }
                }
            }
        }
        protected void init()
        {
            Common.License lic = Common.License.Value;
            //�Ƿ�����Ȩ
            if (lic.IsLicense)
            {
                plYesLic.Visible = true;
                plNoLic.Visible = false;
                //��ǰ�汾
                lbVersion.Text = lic.VersionName;
                //��Ȩ���ͣ���Ȩ���壬��ʼʱ��
                lbLicType.Text = lic.Type.ToString();
                if ((int)lic.Type == 1 || (int)lic.Type == 2)
                    lbLicInfo.Text = lic.Serial;
                else
                    lbLicInfo.Text = lic.Serial + ":" + lic.Port;
                lbStartTime.Text = lic.StartTime.ToString("yyyy-MM-dd");
                lbEndTime.Text = lic.EndTime.ToString("yyyy-MM-dd");
            }
            else
            {
                //û�л����Ȩ
                plYesLic.Visible = false;
                plNoLic.Visible = true;
                rblActivType_SelectedIndexChanged(null, null);
            }
            //��ǰ�汾������
            rptLimit.DataSource = lic.LimitItems;
            rptLimit.DataBind();
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblActivType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int type = Convert.ToInt16(rblActivType.SelectedValue);
            if (type == 1) lbActivCode.Text = Common.Activationcode.CodeForCPU;
            if (type == 2) lbActivCode.Text = Common.Activationcode.CodeForHardDisk;
            if (type == 3) lbActivCode.Text = Common.Activationcode.CodeForIP;
            if (type == 4) lbActivCode.Text = Common.Activationcode.CodeForDomain;
            if (type == 5) lbActivCode.Text = Common.Activationcode.CodeForRoot;         //���򼤻���
        }
        
    }
}
