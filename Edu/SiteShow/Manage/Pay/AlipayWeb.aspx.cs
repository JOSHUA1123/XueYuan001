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
using System.Text.RegularExpressions;



namespace SiteShow.Manage.Pay
{
    public partial class AlipayWeb : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //֧����Ӧ�ó���
        private string scene = Common.Request.QueryString["scene"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
                Pai_Pattern.DdlInterFace.Enabled = false;
            } 
        }
        private void fill()
        {
            EntitiesInfo.PayInterface pi = id <= 0 ? null : Business.Do<IPayInterface>().PaySingle(id);
            if (pi != null)
            {
                this.EntityBind(pi);
                //�Զ���������
                Common.CustomConfig config = CustomConfig.Load(pi.Pai_Config);
                tbPrivatekey.Text = config["Privatekey"].Value.String;
            }
            //�ص������Ϊ��
            if (Pai_Returl.Text.Trim() == "")
                Pai_Returl.Text = "http://" + Common.Server.Domain + ":" + Common.Server.Port + "/";
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.PayInterface pi = id <= 0 ? new EntitiesInfo.PayInterface() : Business.Do<IPayInterface>().PaySingle(id);
            pi = this.EntityFill(pi) as EntitiesInfo.PayInterface;
            //֧����ʽ
            pi.Pai_Pattern = Pai_Pattern.DdlInterFace.SelectedItem.Text;
            //�Զ���������
            Common.CustomConfig config = CustomConfig.Load(pi.Pai_Config);
            string privateKey = Regex.Replace(tbPrivatekey.Text.Trim(), @"\r|\n|\s", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            config["Privatekey"].Text = privateKey;
            pi.Pai_Config = config.XmlString;
            //���õ�ƽ̨
            pi.Pai_Platform = "web";
            //֧����Ӧ�ó���
            pi.Pai_Scene = scene;
            //ֻ�ܸ������ſ�������֧���ӿڣ�Ҳ����˵��Ǯȫ�����������ϣ�
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganRoot();
            if (org != null) pi.Org_ID = org.Org_ID;
            try
            {
                if (id <= 0)
                {
                    Business.Do<IPayInterface>().PayAdd(pi);
                }
                else
                {
                    Business.Do<IPayInterface>().PaySave(pi);
                }

                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}