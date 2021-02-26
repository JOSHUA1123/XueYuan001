using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
using System.Text.RegularExpressions;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 通知公告
    /// </summary>
    public class Notices : BasePage
    {       
        protected override void InitPageTemplate(HttpContext context)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string sear = Common.Request.QueryString["sear"].String;  //搜索
                int size = Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = Common.Request.Form["index"].Int32 ?? 1;  //第几页                
                int sumcount = 0;
                EntitiesInfo.Notice[] nots = Business.Do<INotice>().GetPager(Organ.Org_ID, true, sear, size, index, out sumcount);
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int i = 0; i < nots.Length; i++)
                {
                    EntitiesInfo.Notice not = nots[i];
                    //处理详情
                    not.No_Context = "";
                    not.No_Ttl = not.No_Ttl.Replace("\"", "&quot;");
                    not.No_Ttl = not.No_Ttl;                  
                    json += not.ToJson() + ",";
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }
        }
    }
}