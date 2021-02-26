using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
namespace SiteShow.Ajax
{
    /// <summary>
    /// 记录学员学习视频的时间
    /// </summary>
    [Obsolete]
    public class StudentStudy : IHttpHandler
    {
        //课程id，章节id
        private int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        private int olid = Common.Request.QueryString["olid"].Int32 ?? 0;
        //播放进度，单位毫秒
        private int playTime = Common.Request.QueryString["playTime"].Int32 ?? 0;
        //学习时间，单位秒
        private int studyTime = Common.Request.QueryString["studyTime"].Int32 ?? 0;
        //视频总时长，单位毫秒
        private int totalTime = Common.Request.QueryString["totalTime"].Int32 ?? 0;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //当前学员
           EntitiesInfo.Accounts student = Extend.LoginState.Accounts.CurrentUser;
            if (student == null)
            {
                context.Response.Write("-1");
                return;
            }
            else
            {
                if (totalTime <= 0) throw new Exception("视频总时长为零");
                //记录学习进度，返回完成度的百分比
                double per = Business.Do<IStudent>().LogForStudyUpdate(couid, olid, student, playTime, studyTime, totalTime);
                context.Response.Write(per);
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