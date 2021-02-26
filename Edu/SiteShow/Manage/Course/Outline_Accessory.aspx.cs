using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

using ServiceInterfaces;
using EntitiesInfo;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;


namespace SiteShow.Manage.Course
{
    public partial class Outline_Accessory : Extend.CustomPage
    {
        //章节ID
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        //UID，即全局唯一ID
        private string uid = Common.Request.QueryString["uid"].String;
        //上传资料的所有路径
        private string _uppath = "Course";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Uploader1.UID = uid;
                initBind();
            }
           
        }
        /// <summary>
        /// 附件绑定
        /// </summary>
        protected void initBind()
        {
            List<EntitiesInfo.Accessory> acs = Business.Do<IAccessory>().GetAll(uid, _uppath);
            foreach (Accessory ac in acs)
            {
                ac.As_FileName = Upload.Get[_uppath].Virtual + ac.As_FileName;
            }
            dlAcc.DataSource = acs;
            dlAcc.DataKeyField = "As_Id";
            dlAcc.DataBind();
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            EntitiesInfo.Accessory dd = new Accessory();
            //图片
            //if (fuLoad.HasFile)
            //{
            //    try
            //    {
            //        fuLoad.UpPath = _uppath;
            //        fuLoad.IsMakeSmall = false;
            //        fuLoad.SaveAndDeleteOld(dd.As_Name);
            //        dd.As_Name = fuLoad.FileName;
            //        dd.As_FileName = fuLoad.File.Server.FileName;
            //        dd.As_Size = fuLoad.PostedFile.ContentLength;
            //        dd.As_Extension = fuLoad.File.Server.Extension;
            //        dd.As_Uid = uid;
            //        dd.As_Type = _uppath;
            //        Business.Do<IAccessory>().Add(dd);
            //    }
            //    catch (Exception ex)
            //    {
            //        this.Alert(ex.Message);
            //    }
            //}
            initBind();
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int accid = Convert.ToInt32(lb.CommandArgument);
            Business.Do<IAccessory>().Delete(accid);
            initBind();
        }
        
    }
}
