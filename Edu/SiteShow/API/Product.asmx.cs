using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Common;

using ServiceInterfaces;
using EntitiesInfo;

namespace SiteShow.SOAP
{
    /// <summary>
    /// Product 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Product : System.Web.Services.WebService
    {

        [WebMethod]
        //检索产品
        public EntitiesInfo.Product[] GetProduct(int ps, string searTxt)
        {
            //产品所有资源的路径
            string resPath = Upload.Get["Product"].Virtual;
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            EntitiesInfo.Product[] pds = Business.Do<IContents>().ProductCount(org.Org_ID, 0, 0, false, true, "");
            foreach (EntitiesInfo.Product entity in pds)
            {
                //将文章内容与简介去除，以方便数据传输
                entity.Pd_Details = "";
                entity.Pd_Logo = resPath + entity.Pd_Logo;
                entity.Pd_LogoSmall = resPath + entity.Pd_LogoSmall;
                entity.Pd_QrCode = resPath + entity.Pd_QrCode;
            }
            return pds;
        }
    }
}
