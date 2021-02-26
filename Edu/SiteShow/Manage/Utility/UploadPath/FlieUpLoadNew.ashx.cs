using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ServiceInterfaces;
using Common;
using Aspose.Slides.Pptx;
using Aspose.Words;
using Word = Microsoft.Office.Interop.Word;
//using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;

namespace SiteShow.Manage.Utility.UploadPath
{
    /// <summary>
    /// FlieUpLoadNew 的摘要说明
    /// </summary>
    public class FlieUpLoadNew : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string serverFileName = "", path1 = "";
            //上传路径
            string uploadPath = Common.Request.QueryString["path"].String;
            var path = Common.Upload.Get[uploadPath].Virtual;
            //全局唯一值
            string uid = Common.Request.QueryString["uid"].String;
            try
            {

                HttpPostedFile file;
                for (int i = 0; i < context.Request.Files.Count; ++i)
                {
                    file = context.Request.Files[i];
                    if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName)) continue;
                    string ext = Path.GetExtension(file.FileName);


                    serverFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ext;
                    //上传后的文件
                    string videoFile = HttpContext.Current.Server.MapPath(path + serverFileName);
                    file.SaveAs(videoFile);

                   


                    if (!string.IsNullOrWhiteSpace(uid))
                    {

                        //添加附件
                        EntitiesInfo.Accessory acc = new EntitiesInfo.Accessory();
                        acc.As_Name = Path.GetFileName(file.FileName);
                        //acc.As_FileName = System.IO.Path.ChangeExtension(serverFileName, ".flv");   
                        acc.As_Size = file.ContentLength;
                        acc.As_FileName = serverFileName;
                        acc.As_Uid = uid;
                        acc.As_Extension = ext.Replace(".", "");
                        acc.As_Type = uploadPath;
                        Business.Do<IAccessory>().Add(acc);
                    }

                    #region PDF转换操作

                    //if (ext == ".pptx" || ext == ".ppt")
                    //{
                    //    var serverFileName01 = DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";


                    //    string videoFile01 = HttpContext.Current.Server.MapPath(path + serverFileName01);
                    //    var flag=PPTToPDF(videoFile, videoFile01);
                    //    if (System.IO.File.Exists(videoFile))
                    //    {
                    //        System.IO.File.Delete(videoFile);
                    //    }

                    //    if (!string.IsNullOrWhiteSpace(uid)&& flag)
                    //    {
                    //        //添加附件
                    //        EntitiesInfo.Accessory acc = new EntitiesInfo.Accessory();
                    //        acc.As_Name = Path.GetFileName(file.FileName).Split('.')[0] + ".pdf";
                    //        //acc.As_FileName = System.IO.Path.ChangeExtension(serverFileName, ".flv");   
                    //        acc.As_Size = file.ContentLength;
                    //        acc.As_FileName = serverFileName01;
                    //        acc.As_Uid = uid;
                    //        acc.As_Extension = ".pdf";//ext.Replace(".", "");
                    //        acc.As_Type = uploadPath;
                    //        //

                    //        //handler.Delete("flv,"+ acc.As_Extension);
                    //        Business.Do<IAccessory>().Add(acc);
                    //    }
                    //}
                    //else if (ext == ".doc" || ext == ".docx")
                    //{
                    //    var serverFileName01 = DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                    //    string videoFile01 = HttpContext.Current.Server.MapPath(path + serverFileName01);
                    //    var flag = WordToPDF(videoFile, videoFile01);
                    //    if (System.IO.File.Exists(videoFile))
                    //    {
                    //        System.IO.File.Delete(videoFile);
                    //    }

                    //    if (!string.IsNullOrWhiteSpace(uid) && flag)
                    //    {
                    //        //添加附件
                    //        EntitiesInfo.Accessory acc = new EntitiesInfo.Accessory();
                    //        acc.As_Name = Path.GetFileName(file.FileName).Split('.')[0] + ".pdf";
                    //        //acc.As_FileName = System.IO.Path.ChangeExtension(serverFileName, ".flv");   
                    //        acc.As_Size = file.ContentLength;
                    //        acc.As_FileName = serverFileName01;
                    //        acc.As_Uid = uid;
                    //        acc.As_Extension = ".pdf";//ext.Replace(".", "");
                    //        acc.As_Type = uploadPath;
                    //        Business.Do<IAccessory>().Add(acc);
                    //    }
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrWhiteSpace(uid))
                    //    {

                    //        //添加附件
                    //        EntitiesInfo.Accessory acc = new EntitiesInfo.Accessory();


                    //        acc.As_Name = Path.GetFileName(file.FileName);
                    //        //acc.As_FileName = System.IO.Path.ChangeExtension(serverFileName, ".flv");   
                    //        acc.As_Size = file.ContentLength;
                    //        acc.As_FileName = serverFileName;
                    //        acc.As_Uid = uid;
                    //        acc.As_Extension = ext.Replace(".", "");
                    //        acc.As_Type = uploadPath;
                    //        Business.Do<IAccessory>().Add(acc);
                    //    }
                    //}
                    #endregion
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 700;
                context.Response.Write(ex.Message + " 详情请查看错误日志");
                //写入Log
                string log = context.Server.MapPath(path) + "errorlog.txt";
                using (System.IO.StreamWriter sw = new StreamWriter(log, true))
                {
                    sw.WriteLine("执行时间：" + DateTime.Now.ToString());
                    sw.WriteLine("错误信息：" + ex.Message);
                    sw.WriteLine("堆栈信息：" + ex.StackTrace);
                    sw.WriteLine("");
                    sw.Close();
                }
                context.Response.End();
            }
            finally
            {
                string msg = "{{\"code\":\"{0}\",\"path\":\"{1}\"}}";
                var re = string.Format(msg, 1, path + serverFileName);
                context.Response.Write(re);
                context.Response.End();
            }


            //context.Request.Files[0].SaveAs(context.Server.MapPath(path1 + DateTime.Now.ToFileTime() + Path.GetExtension(context.Request.Files[0].FileName)));
            //    //return Json(new { chunked = true, hasError = false });
            //    context.Response.Write("{\"chunked\" : true, \"hasError\" : false}");
            //    context.Response.End();

        }



        /// <summary>
        /// ppt 转PDF
        /// </summary>
        public bool PPTToPDF(string OldFile, string NewFile)
        {
            var flag = false;
            try
            {

           
            PowerPoint.Application app = null;
            PowerPoint.Presentation doc = null;
           
            object missing = System.Reflection.Missing.Value;
            string templateFile = OldFile;//@"D:\Downloads\企业文化（2020-09）——培训学院.pptx"; //Application.StartupPath + @"\1122.docx";
            try
            {
                app = new PowerPoint.Application();//.ApplicationClass();
                doc = app.Presentations.Open(templateFile, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                doc.SaveAs(NewFile, PowerPoint.PpSaveAsFileType.ppSaveAsPDF, MsoTriState.msoTrue);
                //doc.ExportAsFixedFormat(Application.StartupPath + @"\1122.pdf", Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
                //MessageBox.Show("转换完成！");
                //PresentationEx pres = new PresentationEx(OldFile);
                //pres.Save(NewFile, Aspose.Slides.Export.SaveFormat.Pdf);
                flag = true;
            }
            catch (Exception exp)
            {
                Log.Info("fujian", "PPTToPDFError:" + exp.Message);
            }
            //销毁word进程
            finally
            {
                //object saveChange = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
                if (doc != null)
                    doc.Close();
                if (app != null)
                {
                    app.Quit();
                    app = null;
                }
            }
            }
            catch (Exception es)
            {
                Log.Info("fujian", "PPTToPDFError:" + es.Message);
            }
            return flag;
        }
        /// <summary>
        /// word 转PDF
        /// </summary>
        public bool WordToPDF(string OldFile, string NewFile)
        {
            Word.Application app = null;
            Word.Document doc = null;
            var flag = false;
            object missing = System.Reflection.Missing.Value;
            object templateFile = OldFile;// @"D:\Downloads\员工手册.doc";//Application.StartupPath + @"\1122.docx";
            try
            {
                app = new Word.Application();//.ApplicationClass();
                doc = app.Documents.Add(ref templateFile, ref missing, ref missing, ref missing);
                doc.ExportAsFixedFormat(NewFile, Word.WdExportFormat.wdExportFormatPDF);
                //MessageBox.Show("转换完成！");
                //Document doc = new Document(OldFile);
                //doc.Save(NewFile, Aspose.Words.SaveFormat.Pdf);
                flag = true;
            }
            catch (Exception exp)
            {
                //MessageBox.Show(exp.Message, this.Text);
                Log.Info("fujian", "WordToPDFError:"+ exp.Message);
            }
            //销毁word进程
            finally
            {
                object saveChange = Word.WdSaveOptions.wdDoNotSaveChanges;
                if (doc != null)
                    doc.Close(ref saveChange, ref missing, ref missing);
                if (app != null)
                    app.Quit(ref missing, ref missing, ref missing);
            }
            return flag;
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