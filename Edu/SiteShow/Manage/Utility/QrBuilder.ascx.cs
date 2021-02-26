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

namespace SiteShow.Manage.Utility
{
    public partial class QrBuilder : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            //��Դ��·��
            string resPath = Upload.Get["Org"].Virtual;
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganDefault();
            org.Org_Logo = resPath + org.Org_Logo;
            org.Org_QrCode = resPath + org.Org_QrCode;
            imgQr.ImageUrl = org.Org_QrCode;
        }
        /// <summary>
        /// ���ɶ�ά��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuilerQr_Click(object sender, EventArgs e)
        {
            //����
            string context = tbContent.Text.Trim();           
            //ǰ��ɫ�뱳��ɫ
            string color = tbColor.Text.Trim();
            string bgcolor = tbBgcolor.Text.Trim();
            //���
            int wh = tbWh.Text.Trim() == "" ? 200 : Convert.ToInt32(tbWh.Text.Trim());
            //�Ƿ���������logo
            bool isCenterImg = cbIsLogo.Checked;
            //��ά�������ͼ��
            string centerImg = Upload.Get["Org"].Physics + "QrCodeLogo.png";
            //��Դ��·��
            string resPath = Upload.Get["Temp"].Physics + "QrCode\\";
            //����ļ��в������򴴽�������ɾ��ԭ��ͼƬ
            if (!System.IO.Directory.Exists(resPath))
            {
                System.IO.Directory.CreateDirectory(resPath);
            }
            else
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(resPath);
                foreach (System.IO.FileInfo fi in di.GetFiles("*.png"))
                {
                    if (fi.Name.IndexOf("_") > -1)
                    {
                        //string tm = fi.Name.Substring(0, fi.Name.IndexOf("_"));
                        if (fi.Name.Substring(0, fi.Name.IndexOf("_")) == "qrcode")
                        {
                            fi.Delete();
                        }
                    }
                }
            }
            string file = "qrcode_" + Common.Request.UniqueID() + ".png";
            //
            System.Drawing.Image image = null;
            try
            {
                if (isCenterImg)
                    image=Common.QrcodeHepler.Encode(context, wh, centerImg, color, bgcolor);
                else
                    image = Common.QrcodeHepler.Encode(context, wh, color, bgcolor);
                string imgpath = resPath + file;
                image.Save(imgpath);
                imgQr.ImageUrl = Upload.Get["Temp"].Virtual + "QrCode/" + file;
            }
            catch (Exception ex)
            {
                new Extend.Scripts(this.Page).Alert(ex.Message);
            }
        }
    }
}