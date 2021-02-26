﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using EntitiesInfo;
namespace SiteShow.Ajax
{
    /// <summary>
    /// Teachers 的摘要说明
    /// </summary>
    public class Teachers : IHttpHandler
    {
        int index = Common.Request.QueryString["index"].Int32 ?? 1;
        int size = Common.Request.QueryString["size"].Int32 ?? 1;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
           Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sum = 0;
            EntitiesInfo.Teacher[] teachers = Business.Do<ITeacher>().TeacherPager(org.Org_ID, -1, true,  true, "", size, index, out sum);
            string tm = "{\"sum\":" + sum + ",\"index\":" + index + ",\"object\":[";
            for (int i = 0; i < teachers.Length; i++)
            {
                EntitiesInfo.Teacher t = teachers[i];
                t.Th_Photo = Upload.Get["Teacher"].Virtual + t.Th_Photo;
                t.Th_Intro = "";
                tm += "" + t.ToJson(null, "Th_Pw,Th_IDCardNumber,Th_Qus,Th_Anwser");
                if (i < teachers.Length - 1) tm += ",";
            }
            tm += "]}";
            context.Response.Write(tm);
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