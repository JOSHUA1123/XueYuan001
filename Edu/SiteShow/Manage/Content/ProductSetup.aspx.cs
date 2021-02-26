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

namespace SiteShow.Manage.Content
{
    public partial class ProductSetup : System.Web.UI.Page
    {
        private string _uppath = "Product";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            try
            {
                //ˮӡ�ߴ�
                //�Ƿ�ǿ��Լ����С���Լ�Լ���Ŀ��ֵ
                cbIsWH.Checked = Business.Do<ISystemPara>()["Product_IsCompelSize"].Boolean ?? false;
                tbCompelWd.Text = Business.Do<ISystemPara>()["Product_CompelWidth"].String;
                tbCompelHg.Text = Business.Do<ISystemPara>()["Product_CompelHeight"].String;
                //ͼƬĬ������ͼ���
                tbWidth.Text = Business.Do<ISystemPara>()["Product_ThumbnailWidth"].String;
                tbHeight.Text = Business.Do<ISystemPara>()["Product_ThumbnailHeight"].String;
                //��ά��Ŀ��
                this.tbQrWH.Text = Business.Do<ISystemPara>()["Product_QrCode_WidthAndHeight"].String;
                //��ά���ģ��
                this.tbQrTextTmp.Text = Business.Do<ISystemPara>()["Product_QrCode_Template"].String;
            }
            catch (Exception)
            {
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
                //ͼƬ�ߴ�
                //�Ƿ�ǿ��Լ����С���Լ�Լ���Ŀ��ֵ
                Business.Do<ISystemPara>().Save("Product_IsCompelSize", cbIsWH.Checked.ToString());
                Business.Do<ISystemPara>().Save("Product_CompelWidth", tbCompelWd.Text.Trim());
                Business.Do<ISystemPara>().Save("Product_CompelHeight", tbCompelHg.Text.Trim());
                //ͼƬĬ������ͼ���
                Business.Do<ISystemPara>().Save("Product_ThumbnailWidth", tbWidth.Text.Trim());
                Business.Do<ISystemPara>().Save("Product_ThumbnailHeight", tbHeight.Text.Trim());
                //�����ά��
                Business.Do<ISystemPara>().Save("Product_QrCode_WidthAndHeight", tbQrWH.Text);
                Business.Do<ISystemPara>().Save("Product_QrCode_Template", tbQrTextTmp.Text);
                //ˢ��ȫ�ֲ���
                Business.Do<ISystemPara>().Refresh();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// ���沢�������ɶ�ά��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                btnEnter_Click(null, null);
                //��ά��Ŀ��
                int wh = Business.Do<ISystemPara>()["Product_QrCode_WidthAndHeight"].Int16 ?? 200;
                //��ά��ģ������
                string template = Business.Do<ISystemPara>()["Product_QrCode_Template"].String;
                //��ʼ��������
                int sum = 0;
                int size = 20;
                int index = 1;
                EntitiesInfo.Product[] entities;
                do
                {
                    EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
                    entities = Business.Do<IContents>().ProductPager(org.Org_ID, null, "", null, null, null, null, "", size, index, out sum);
                    foreach (EntitiesInfo.Product entity in entities)
                    {
                        createQrCode(entity, _uppath, template, wh);
                    }
                } while (size * index++ < sum);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// ���ɲ�Ʒ�Ķ�ά��
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private string createQrCode(EntitiesInfo.Product pd, string pathType,string template,int wh)
        {
            try
            {
                //��ά��ͼƬ����
                string img = "";
                if (pd != null && pd.Pd_QrCode != null && pd.Pd_QrCode != "")
                {
                    img = pd.Pd_QrCode;
                }
                else
                {
                    img = Common.Request.UniqueID() + ".png";
                    pd.Pd_QrCode = img;
                    Business.Do<IContents>().ProductSave(pd);
                }
                //������ά��
                Extend.QrCode.Creat4Entity(pd, template, Upload.Get[pathType].Physics + img, wh);
                return img;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
