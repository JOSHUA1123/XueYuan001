﻿using System;
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
    /// News1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class News1 : System.Web.Services.WebService {

        [WebMethod]
        //检索资讯文章
        public EntitiesInfo.Article[] GetNewsArt(int nc,string searTxt)
        {
            //所属机构的所有课程
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
           EntitiesInfo.Article[] nas = Business.Do<IContents>().ArticleCount(org.Org_ID, 0, 0, null);
            foreach (EntitiesInfo.Article na in nas)
            {
                //将文章内容与简介去除，以方便数据传输
                na.Art_Details = "";
                na.Art_Intro = "";
            }
            return nas;
        }
    }
}
