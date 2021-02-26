﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 资讯文章页
    /// </summary>
    public class Article : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {           
            //资讯文章
            int artid = Common.Request.QueryString["id"].Int32 ?? 0;
            EntitiesInfo.Article art = Business.Do<IContents>().ArticleSingle(artid);
            if (art == null) return;
            if ((Common.Request.Cookies["article_" + art.Art_Id].Int32 ?? 0) == 0)
            {
                art.Art_Number++;
                Business.Do<IContents>().ArticleSave(art);
                context.Response.Cookies["article_" + art.Art_Id].Value = art.Art_Id.ToString();
            }
            art.Art_Logo = Upload.Get["News"].Virtual + art.Art_Logo;
            this.Document.Variables.SetValue("art", art);
            //附件
            List<EntitiesInfo.Accessory> acs = Business.Do<IAccessory>().GetAll(art.Art_Uid);
            foreach (EntitiesInfo.Accessory ac in acs)
                ac.As_FileName = Upload.Get["News"].Virtual + ac.As_FileName;
            this.Document.Variables.SetValue("artAcc", acs);
            ////当前资讯的上一条
            //Entities.Article artPrev = Business.Do<IContents>().ArticlePrev(artid);
            //this.Document.Variables.SetValue("artPrev", artPrev);
            ////当前资讯的下一条
            //Entities.Article artNext = Business.Do<IContents>().ArticleNext(artid);
            //this.Document.Variables.SetValue("artNext", artNext);
        }
    }
}