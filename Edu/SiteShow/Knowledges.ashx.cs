using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
using EntitiesInfo;
namespace SiteShow
{
    /// <summary>
    /// 课程资源库
    /// </summary>
    public class Knowledges : BasePage
    {
        //课程id
        private int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {           
            //所有资源库的分类
            EntitiesInfo.KnowledgeSort[] ncs = Business.Do<IKnowledge>().GetSortAll(Organ.Org_ID, couid, true);
            WeiSha.WebControl.MenuTree mt = new WeiSha.WebControl.MenuTree();
            mt.Title = "全部";
            mt.DataTextField = "Kns_Name";
            mt.IdKeyName = "Kns_Id";
            mt.ParentIdKeyName = "Kns_PID";
            mt.TaxKeyName = "Kns_Tax";
            mt.SourcePath = "/manage/Images/tree";
            //mt.TypeKeyName = "Kns_Type";
            mt.IsUseKeyName = "Kns_IsUse";
            //mt.IsShowKeyName = "Nc_IsShow";
            mt.DataSource = ncs;
            mt.DataBind();            
            this.Document.Variables.SetValue("tree", mt.HTML);
        }
        
       
    }
}