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
    public partial class WeixinH5Pay : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //支付的应用场景
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
                //自定义配置项
                Common.CustomConfig config = CustomConfig.Load(pi.Pai_Config);
                tbMCHID.Text = config["MCHID"].Value.String;    //商户id
                tbPaykey.Text = config["Paykey"].Value.String;  //支付密钥
            }
            //回调域如果为空
            if (Pai_Returl.Text.Trim() == "")
            {
                if (Common.Server.Port == "80")
                {
                    Pai_Returl.Text = "http://" + Common.Server.Domain + "/";
                }
                else
                {
                    Pai_Returl.Text = "http://" + Common.Server.Domain + ":" + Common.Server.Port + "/";
                }
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.PayInterface pi = id <= 0 ? new EntitiesInfo.PayInterface() : Business.Do<IPayInterface>().PaySingle(id);
            pi = this.EntityFill(pi) as EntitiesInfo.PayInterface;
            //支付方式
            pi.Pai_Pattern = Pai_Pattern.DdlInterFace.SelectedItem.Text;
            //自定义配置项
            Common.CustomConfig config = CustomConfig.Load(pi.Pai_Config);
            config["MCHID"].Text = tbMCHID.Text.Trim();     //商户id
            config["Paykey"].Text = tbPaykey.Text.Trim();   //支付密钥
            pi.Pai_Config = config.XmlString;
            //所用的平台
            pi.Pai_Platform = "mobi";
            //支付的应用场景
            pi.Pai_Scene = scene;
            //只能根机构才可以设置支付接口（也就是说，钱全到根机构账上）
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

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
