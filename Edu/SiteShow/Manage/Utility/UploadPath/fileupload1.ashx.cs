using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace SiteShow.Manage.Utility.UploadPath
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class fileupload : IHttpHandler
    {

        #region 文件分片

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string serverFileName = "", path1 = "";
            //上传路径
            string uploadPath = Common.Request.QueryString["path"].String;
            path1 = Common.Upload.Get[uploadPath].Virtual;
            //如果进行了分片
            if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                //取得chunk和chunks
                int chunk = Convert.ToInt32(context.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数
                //根据GUID创建用该GUID命名的临时文件夹
                string folder = context.Server.MapPath(path1 + context.Request["guid"] + "/");
                string path = folder + chunk;

                //建立临时传输文件夹
                if (!Directory.Exists(Path.GetDirectoryName(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                BinaryWriter AddWriter = new BinaryWriter(addFile);
                //获得上传的分片数据流
                var file = context.Request.Files[0];
                Stream stream = file.InputStream;

                BinaryReader TempReader = new BinaryReader(stream);
                //将上传的分片追加到临时文件末尾
                AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                //关闭BinaryReader文件阅读器
                TempReader.Close();
                stream.Close();
                AddWriter.Close();
                addFile.Close();

                TempReader.Dispose();
                stream.Dispose();
                AddWriter.Dispose();
                addFile.Dispose();
                //return Json(new { chunked = true, hasError = false, f_ext = Path.GetExtension(file.FileName) });
                context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"f_ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
                context.Response.End();
            }
            else//没有分片直接保存
            {
                context.Request.Files[0].SaveAs(context.Server.MapPath(path1 + DateTime.Now.ToFileTime() + Path.GetExtension(context.Request.Files[0].FileName)));
                //return Json(new { chunked = true, hasError = false });
                context.Response.Write("{\"chunked\" : true, \"hasError\" : false}");
                context.Response.End();
            }
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}