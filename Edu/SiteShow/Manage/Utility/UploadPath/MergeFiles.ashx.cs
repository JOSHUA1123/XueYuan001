using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ServiceInterfaces;
using Common;
using System.Threading;

namespace WebUploadTest
{
    /// <summary>
    /// Summary description for MergeFiles
    /// </summary>
    public class MergeFiles : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var str = string.Empty;
            var dir = string.Empty;
            try
            {
                var guid = context.Request["guid"];//GUID
                                                   //全局唯一值
                string uid = context.Request["uid"];
                string uploadPath = context.Request.QueryString["path"];
                var path1 = Common.Upload.Get[uploadPath].Virtual;
                var uploadDir = context.Server.MapPath(path1);//Upload 文件夹
                 dir = Path.Combine(uploadDir, guid);//临时文件夹
                var ext = Path.GetExtension(context.Request["fileName"]);
                var files = Directory.GetFiles(dir);//获得下面的所有文件
                var name = Guid.NewGuid().ToString("N") + ext;
                var finalPath = Path.Combine(uploadDir, name);//最终的文件名
                var fs = new FileStream(finalPath, FileMode.Create);
                //Log.Info("Mergefiles", "开始合并");
                
                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                {
                       var bytes = System.IO.File.ReadAllBytes(part);
                    fs.Write(bytes, 0, bytes.Length);
                    bytes = null;
                    System.IO.File.Delete(part);//删除分块
                    //Log.Info("Mergefiles执行", nums.ToString());
                }
                //Log.Info("Mergefiles", "结束合并");
                fs.Flush();
                fs.Close();
                Directory.Delete(dir);//删除文件夹
                string videoFile = HttpContext.Current.Server.MapPath(path1 + name);
                if (!string.IsNullOrWhiteSpace(uid))
                {

                    //添加附件
                    EntitiesInfo.Accessory acc = new EntitiesInfo.Accessory();
                    //视频操作对象
                    Common.VideoHandler handler = Common.VideoHandler.Hanlder(videoFile);
                    if (handler.Width > 0) acc = setAcc(acc, handler);

                    //参数
                    acc.As_Name = Path.GetFileName(context.Request["fileName"]);
                    //acc.As_FileName = System.IO.Path.ChangeExtension(serverFileName, ".flv");    
                    acc.As_FileName = name;
                    acc.As_Uid = uid;
                    acc.As_Extension = ext.Replace(".", "");
                    acc.As_Type = uploadPath;
                    //
                    handler = Common.VideoHandler.Hanlder(videoFile);
                    handler.Delete("flv,mp4");
                    Business.Do<IAccessory>().Add(acc);
                }
                //return Json(new { r = 1, path = "/upload/" + name });
                str= "{\"r\" : 1, \"path\" :\"" + path1 + name + "\"}";
                //context.Response.Write("{\"r\" : 1, \"path\" :\"" + path1 + name + "\"}");

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(dir))
                {
                    DeleteFolder(dir);//删除文件夹
                }
                Log.Info("Mergefiles异常", ex.Message);
                var str01 = "上传失败！";
                //return Json(new { r = 0, err = ex.Message });
                str = "{\"r\" : 0, \"err\":\"" + str01 + "\"}";
                //context.Response.Write("{\"r\" : 0, \"err\":\"" + ex.Message + "\"}");

            }
            context.Response.Write(str);
        }



        /// <summary>
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        private static void DeleteFolder(string strPath)
        {
            //删除这个目录下的所有子目录
            if (Directory.GetDirectories(strPath).Length > 0)
            {
                foreach (string fl in Directory.GetDirectories(strPath))
                {
                    Directory.Delete(fl, true);
                }
            }
            //删除这个目录下的所有文件
            if (Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string f in Directory.GetFiles(strPath))
                {
                    System.IO.File.Delete(f);
                }
            }
            Directory.Delete(strPath, true);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 设置附件的各项参数
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        EntitiesInfo.Accessory setAcc(EntitiesInfo.Accessory acc, Common.VideoHandler handler)
        {
            //视频时长
            acc.As_Duration = (int)handler.Duration.TotalSeconds;
            acc.As_Duration = acc.As_Duration > 0 ? acc.As_Duration : 0;
            acc.As_Width = handler.Width;
            acc.As_Height = handler.Height;
            acc.As_Size = (int)handler.Size;
            return acc;
        }
    }
}