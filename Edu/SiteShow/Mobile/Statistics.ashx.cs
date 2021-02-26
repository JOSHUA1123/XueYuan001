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
    /// 统计分析
    /// </summary>
    public class Statistics : BasePage
    {
        int id = Common.Request.QueryString["id"].Int32 ?? 0;    //当前模拟考试的id
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //当前试卷
                EntitiesInfo.TestPaper paper = null;
                paper = Business.Do<ITestPaper>().PagerSingle(id);
                if (paper != null)
                {
                    this.Document.SetValue("pager", paper);
                }
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = Common.Request.Form["action"].String;
                switch (action)
                {
                    case "delete":
                        delete();
                        break;
                    case "list":
                        list();
                        break;
                }
            }
        }
        /// <summary>
        /// 删除考试成绩
        /// </summary>
        private void delete()
        {
            //成绩id
            int trid = Common.Request.Form["trid"].Int32 ?? 0;
            try
            {
                Business.Do<ITestPaper>().ResultsDelete(trid);
                Response.Write("1");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            Response.End();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        private void list()
        {
            //int id = Common.Request.QueryString["id"].Int32 ?? 0;    //当前模拟考试的id
            int size = Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
            int index = Common.Request.Form["index"].Int32 ?? 1;  //第几页            
            //仅限的输出字段
            string onlyFeild = Common.Request.Form["only"].String;   //输出哪些字段
            string wipeFeild = Common.Request.Form["wipe"].String;   //哪些字段不输出
            //
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            int stid = st.Ac_ID;   //当前学员

            int sumcount = 0;
            EntitiesInfo.TestResults[] tps = Business.Do<ITestPaper>().ResultsPager(stid, id, size, index, out sumcount);
            string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
            json += "\"items\":[";
            for (int n = 0; n < tps.Length; n++)
            {
                tps[n].Tp_Name = tps[n].Tp_Name.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                //增加输出项
                Dictionary<string, object> addParas = new Dictionary<string, object>();
                EntitiesInfo.TestPaper tp = Business.Do<ITestPaper>().PagerSingle((int)tps[n].Tp_Id);
                if (tp != null)
                {
                    tp.Tp_Logo = string.IsNullOrWhiteSpace(tp.Tp_Logo) ? tp.Tp_Logo : Upload.Get["TestPaper"].Virtual + tp.Tp_Logo;
                    addParas.Add("Tp_Logo", tp.Tp_Logo);
                    addParas.Add("Tp_PassScore", tp.Tp_PassScore);
                }
                json += tps[n].ToJson(onlyFeild, wipeFeild, addParas) + ",";
            }
            if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
            json += "]}";
            Response.Write(json);
            Response.End();
        }
    }
}