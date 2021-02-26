using Common;
using ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SiteShow.Manage.Utility.UploadPath
{
    /// <summary>
    /// GetChunkFiles 的摘要说明
    /// </summary>
    public class GetChunkFiles : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                var guid = context.Request["guid"];//GUID
                                                   //全局唯一值
                //string uid = context.Request["uid"];
                string uploadPath = context.Request.QueryString["path"];
                var path1 = Common.Upload.Get[uploadPath].Virtual;
                var uploadDir = context.Server.MapPath(path1);//Upload 文件夹
                var dir = Path.Combine(uploadDir, guid);//临时文件夹
                                                        //var ext = Path.GetExtension(context.Request["fileName"]);

                //var name = Guid.NewGuid().ToString("N") + ext;
                //var finalPath = Path.Combine(uploadDir, name);//最终的文件名
                //var fs = new FileStream(finalPath, FileMode.Create);
                //Log.Info("Mergefiles", "开始合并");
                var counts = 0;
                if (Directory.Exists(dir))
                {
                    var files = Directory.GetFiles(dir);//获得下面的所有文件
                    counts = files.Length;
                }
               
                //foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                //{
                //    var bytes = System.IO.File.ReadAllBytes(part);
                //    fs.Write(bytes, 0, bytes.Length);
                //    bytes = null;
                //    System.IO.File.Delete(part);//删除分块
                //}
             
              
                //return Json(new { r = 1, path = "/upload/" + name });
                context.Response.Write("{\"r\" : 1, \"counts\" :\"" + counts + "\"}");

            }
            catch (Exception ex)
            {
                Log.Info("Mergefiles异常", ex.Message);
                //return Json(new { r = 0, err = ex.Message });
                context.Response.Write("{\"r\" : 0},\"err\":\"" + ex.Message + "\"}");

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