using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;

namespace SiteShow.Ajax
{
    /// <summary>
    /// 学员试题练习的记录
    /// </summary>
    public class LogForStudentQuestions : IHttpHandler
    {
        //课程id,章节id，试题id，试题序号
        int couid =Common.Request.QueryString["couid"].Int32 ?? 00;
        int olid = Common.Request.QueryString["olid"].Int32 ?? 00;
        int qid = Common.Request.QueryString["qid"].Int32 ?? 00;
        int index = Common.Request.QueryString["index"].Int32 ?? 00;
        //学员id
        int acid =Common.Request.QueryString["acid"].Int32 ?? 00;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                Business.Do<ILogs>().QuestionUpdate(acid, couid, olid, qid, index);
                context.Response.Write("1");
            }
            catch
            {
                context.Response.Write("1");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}