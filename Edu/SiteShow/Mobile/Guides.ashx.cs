using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
namespace SiteShow.Mobile
{
    /// <summary>
    /// 通知公告列表
    /// </summary>
    public class Guides : BasePage
    {
        //当前公告所属课程的id
        public int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = Common.Request.Form["index"].Int32 ?? 1;  //第几页    
                string sorts = Common.Request.Form["sorts"].String;  //栏目分类id
                string search = Common.Request.Form["sear"].String;  //要检索的字符
                int sumcount = 0;
                //信息列表
                EntitiesInfo.Guide[] guides = null;
                guides = Business.Do<IGuide>().GetGuidePager(0, couid, sorts, search, true, size, index, out sumcount);    
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int n = 0; n < guides.Length; n++)
                {
                    json += guides[n].ToJson(null, "Gu_Details") + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }            
        }
    }
}