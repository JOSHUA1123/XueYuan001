﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
//using System.Data;
using System.Timers;
using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;
using System.Web.Routing;
using System.Threading;
using DataBaseInfo;

namespace SiteShow
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 在应用程序被实例化或第一次被调用时，该事件被触发。对于所有的HttpApplication 对象实例，它都会被调用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Init(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// 在HttpApplication 类的第一个实例被创建时，该事件被触发。它允许你创建可以由所有HttpApplication 实例访问的对象。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            
            //创建路由
            RegisterRoutes(RouteTable.Routes);

            if (!Gateway.IsCorrect) throw new Exception("数据库链接不正确！");

            if (Gateway.IsCorrect)
            {
                //生成所有机构的二维码
                new Thread(new ThreadStart(() =>
                {
                    Business.Do<IOrganization>().OrganBuildQrCode();
                })).Start();

                //初始化七牛云直播的值
                Business.Do<ILive>().Initialization();
                ////章节缓存创建      
                //new Thread(new ThreadStart(() =>
                //{                    

                //})).Start();
                //Business.Do<IOutline>().OutlineBuildCache();


                #region  试题的事件
                Business.Do<IQuestions>().OnSave(null, EventArgs.Empty);
                #endregion
            }
        }    


        private void RegisterRoutes(RouteCollection routes){
            //自定义页面（.cs为custom缩写，不是csharp哟），系统自动取/tempates/中的模板用于展示
            routes.MapPageRoute(
                "custom",
                "{term}.cshtm",
                "~/templatePage.ashx",
                false
             );
            routes.MapPageRoute(
               "api_url",
               "api/{t1}/{t2}/{t3}",
               "~/api/api.ashx"
           ); 
            ////伪静态页面，自动转到ashx动态页（ashx又自动取了/tempates/中的模板用于展示）
            //routes.MapPageRoute(
            //    "web",
            //    "{term}.htm",
            //    "~/{term}.ashx"
            //);
            //伪静态页面，自动转到ashx动态页（ashx又自动取了/tempates/中的模板用于展示）
            routes.MapPageRoute(
                "wcutom",
                "{term}",
                "~/{term}/default.ashx"
            );
            //帮助文档
            routes.MapPageRoute(
                "helper",
                "help/{term}",
                "~/help/{term}.ashx"
            );          
            //************************************
            ////同上，只是此为手机端的ashx动态页
            //routes.MapPageRoute(
            //    "mobile",
            //    "mobile/{term}.htm",
            //    "~/mobile/{term}.ashx"
            //    );
            //同上，此为手机端自定义页面
            routes.MapPageRoute(
                "mcustom",
                "mobile/{term}.cshtm",
                "~/TemplatePage.ashx"
                );
            //软件下载
            routes.MapPageRoute(
                "download",
                "download/{term}.aspx",
                "~/download/App.aspx"
                );
        }


        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Session_End(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 隐藏 Response Header 中的Server节点（IIS版本信息）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            if (application != null && application.Context != null)
            {
                try
                {
                    application.Context.Response.Headers.Remove("Server");
                }
                catch
                {
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            Exception objExp = HttpContext.Current.Server.GetLastError();
            //LogHelper.ErrorLog("<br/><strong>客户机IP</strong>：" + Request.UserHostAddress + "<br /><strong>错误地址</strong>：" + Request.Url, objExp);

            var str = "客户机IP:" + Request.UserHostAddress + " \r\n" + "错误地址:  " + Request.Url + "  \r\n" + objExp.ToString();
            Log.TT2(str);
        }
    }
}