﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ServiceInterfaces;
using EntitiesInfo;
using Common;

namespace SiteShow.Manage.Utility.UploadPath
{
    /// <summary>
    /// 外部链接
    /// </summary>
    public partial class OtherLink : Extend.CustomPage
    {
        //上传文件的配置项，参照web.config中的Platform/Upload节点
        string pathkey = Common.Request.QueryString["path"].String;
        string uid = Common.Request.QueryString["uid"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                EntitiesInfo.Accessory acc = Business.Do<IAccessory>().GetSingle(uid);
                if (acc == null) return;
                tbName.Text = acc.As_Name;
                tbUrl.Text = acc.As_FileName;
                tbWidth.Text = acc.As_Width.ToString();
                tbHeight.Text = acc.As_Height.ToString();
                tbSpan.Text = (acc.As_Duration/60).ToString();
                //如果是外部链接，且是第三方平台的
                if (!(acc.As_IsOuter && acc.As_IsOther))
                {
                    btnEnter.Visible = false;
                    panleShow.Visible = true;
                }
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string name = Common.Request.Form["name"].UrlDecode;  //视频名称
                string url = Common.Request.Form["url"].UrlDecode;  //视频链接
                int height = Common.Request.Form["height"].Int32 ?? 0;   //视频高
                int width = Common.Request.Form["width"].Int32 ?? 0;     //视频宽
                int span = Common.Request.Form["span"].Int32 ?? 0;   //视频时长
                //添加附件信息
                EntitiesInfo.Accessory acc = new EntitiesInfo.Accessory();
                if (!string.IsNullOrWhiteSpace(uid))
                {
                    //参数                   
                    acc.As_Name = name;
                    acc.As_FileName = url;
                    acc.As_Uid = uid;
                    acc.As_Type = pathkey;
                    acc.As_Duration = span * 60;
                    acc.As_Height = height;
                    acc.As_Width = width;
                    acc.As_IsOuter = true;      //外部链接
                    acc.As_IsOther = true;      //第三方平台url
                    Business.Do<IAccessory>().Delete(uid, false);
                    Business.Do<IAccessory>().Add(acc);
                    string json = "{\"success\":\"1\",\"name\":\"" + name + "\",\"url\":\"" + acc.As_FileName + "\"}";
                    Response.Write(json);
                    Response.End();
                }
            }
        }
    }
}