using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Common.Images;

namespace WebControlShow
{
    [ToolboxData("<{0}:FileUpload runat=server></{0}:FileUpload>")]
    [DefaultProperty("Text")]
    public class FileUpload : System.Web.UI.WebControls.FileUpload
    {
        private bool _isChangeFileName = true;
        private int _smallPicWidth = 100;
        private int _smallPicHeight = 100;
        private string _upPath;
        private bool _isConvertJpg;
        private string _newName;
        private bool _isMakeSmall;
        private int _smallRestrain;
        private FileUpload.FileInfo _file;

        /// <summary>
        /// 上传文件的路径（相对于根路径），在web.config有配置，注意：
        /// 
        /// </summary>
        public string UpPath
        {
            get
            {
                return this._upPath;
            }
            set
            {
                this._upPath = value;
            }
        }

        /// <summary>
        /// 是否需要将上传的文件转成jpg图片。
        /// 
        /// </summary>
        public bool IsConvertJpg
        {
            get
            {
                return this._isConvertJpg;
            }
            set
            {
                this._isConvertJpg = value;
            }
        }

        /// <summary>
        /// 是否在上传文件后，更改文件名为随机（不更改后缀名）
        /// 
        /// </summary>
        public bool IsChangeFileName
        {
            get
            {
                return this._isChangeFileName;
            }
            set
            {
                this._isChangeFileName = value;
            }
        }

        /// <summary>
        /// 上传文件时，手动设定新文件名；否则将返回随机文件名
        /// 
        /// </summary>
        public string NewName
        {
            get
            {
                return this._newName;
            }
            set
            {
                this._newName = value;
            }
        }

        /// <summary>
        /// 是否生成缩略图，如果文件不是图片文件，会出错；
        /// 
        /// </summary>
        public bool IsMakeSmall
        {
            get
            {
                return this._isMakeSmall;
            }
            set
            {
                this._isMakeSmall = value;
            }
        }

        /// <summary>
        /// 缩略图宽度，默认为100
        /// 
        /// </summary>
        public int SmallWidth
        {
            get
            {
                return this._smallPicWidth;
            }
            set
            {
                this._smallPicWidth = value;
            }
        }

        /// <summary>
        /// 缩略图高度，默认为100
        /// 
        /// </summary>
        public int SmallHeight
        {
            get
            {
                return this._smallPicHeight;
            }
            set
            {
                this._smallPicHeight = value;
            }
        }

        /// <summary>
        /// 缩略图缩放类型（非变形缩放），1按宽缩放、2按高缩放、0自适应；
        ///             注意：多余部分被被裁切
        /// 
        /// </summary>
        public int SmallRestrain
        {
            get
            {
                return this._smallRestrain;
            }
            set
            {
                this._smallRestrain = value;
            }
        }

        public FileUpload.FileInfo File
        {
            get
            {
                return this._file;
            }
            set
            {
                this._file = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("允许上传的文件类型")]
        [Localizable(true)]
        public string FileAllow
        {
            get
            {
                return (string)this.ViewState["FileAllow"] ?? "";
            }
            set
            {
                this.ViewState["FileAllow"] = (object)value.ToString();
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute("style", "width:" + (object)this.Width);
            writer.AddAttribute("fileallow", this.FileAllow);
        }

        /// <summary>
        /// 使用 System.Web.UI.WebControls.FileUpload 控件将上载的文件的内容保存到 Web 服务器上的指定路径。
        /// 
        /// </summary>
        /// <param name="filename">一个字符串，指定服务器上保存上载文件的位置的完整路径。</param>
        public void SaveAs()
        {
            if (!string.IsNullOrWhiteSpace(this.FileAllow))
            {
                bool flag = false;
                string str1 = string.Empty;
                if (this.PostedFile.FileName.IndexOf('.') > -1)
                    str1 = this.PostedFile.FileName.Substring(this.PostedFile.FileName.LastIndexOf('.') + 1).ToLower();
                string fileAllow = this.FileAllow;
                char[] chArray = new char[1]
        {
          '|'
        };
                foreach (string str2 in fileAllow.Split(chArray))
                {
                    if (!string.IsNullOrWhiteSpace(str2) && !(str2.Trim() == "") && str2.ToLower() == str1)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    throw new Exception("上传文件仅限" + this.FileAllow.Replace("|", "、") + "格式");
            }
            this._file = new FileUpload.FileInfo(this);
            this.SaveAs(this._file.Server.FileFullName);
            this._file.Server.ChangeImgFormate();
            this._file.Server.CreateSmallImage();
            this._file.Server.SetFileInfo();
        }

        /// <summary>
        /// 保存并删除原有旧文件
        /// 
        /// </summary>
        /// <param name="oldFilename">要删除的旧文件</param>
        public void SaveAndDeleteOld(string oldFilename)
        {
            this.SaveAs();
            if (string.IsNullOrWhiteSpace(oldFilename))
                return;
            this.File.Server.Delete(oldFilename);
        }

        /// <summary>
        /// 删除指定的文件
        /// 
        /// </summary>
        /// <param name="pathName">文件所在路径名，是web.config中Upload节点的key值</param><param name="fileName">文件名称</param>
        public static void Delete(string pathName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;
            Upload.Get[pathName].DeleteFile(fileName);
        }

        /// <summary>
        /// 文件的信息，包括文本在客户端的信息，以及到了服务端后的信息
        /// 
        /// </summary>
        public class FileInfo
        {
            private FileUpload.UploadControlClientFile _client;
            private FileUpload.UploadControlServerFile _server;

            /// <summary>
            /// 文件来自客户端的信息
            /// 
            /// </summary>
            public FileUpload.UploadControlClientFile Client
            {
                get
                {
                    return this._client;
                }
                set
                {
                    this._client = value;
                }
            }

            /// <summary>
            /// 文件上传到服务器后的文件信息
            /// 
            /// </summary>
            public FileUpload.UploadControlServerFile Server
            {
                get
                {
                    return this._server;
                }
                set
                {
                    this._server = value;
                }
            }

            /// <summary>
            /// 构造方法，用于上传之前
            /// 
            /// </summary>
            /// <param name="upload"/>
            public FileInfo(System.Web.UI.WebControls.FileUpload upload)
            {
                this._client = new FileUpload.UploadControlClientFile(upload);
            }

            /// <summary>
            /// 构造方法
            /// 
            /// </summary>
            public FileInfo(FileUpload upload)
            {
                this._client = new FileUpload.UploadControlClientFile((System.Web.UI.WebControls.FileUpload)upload);
                this._server = new FileUpload.UploadControlServerFile(upload);
            }
        }

        /// <summary>
        /// 服务器端文件与属性
        /// 
        /// </summary>
        public class UploadControlServerFile
        {
            private FileUpload _upload;
            private long _size;
            private string _name;
            private string _extension;
            private int _height;
            private int _width;
            private DateTime _lastWriteTime;
            private DateTime _createTime;

            /// <summary>
            /// 文件名(包括名称与扩展名，如name.ext)，为上传后的文件名；
            /// 
            /// </summary>
            public string FileName
            {
                get
                {
                    return this.Name + "." + this.Extension;
                }
            }

            /// <summary>
            /// 存放在服务器端的完整路径；
            /// 
            /// </summary>
            public string FileFullName
            {
                get
                {
                    return Upload.Get[this._upload.UpPath].Physics + this.Name + "." + this.Extension;
                }
            }

            /// <summary>
            /// 文件的大小，以字节为单位
            /// 
            /// </summary>
            public long Size
            {
                get
                {
                    return this._size;
                }
            }

            /// <summary>
            /// 文件名，为上传后的文件名；此处仅为文件名，不包括扩展名
            /// 
            /// </summary>
            public string Name
            {
                get
                {
                    if (this._upload == null)
                        return (string)null;
                    if (this._upload.NewName != null && this._upload.NewName.Trim() != "")
                    {
                        this._name = this._upload.NewName;
                        return this._name;
                    }
                    if (!this._upload.IsChangeFileName)
                        this._name = this._upload.File.Client.Name;
                    if (this._upload.IsChangeFileName && (this._name == null || this._name == string.Empty))
                        this._name = DateTime.Now.ToString("yyyyMMddhhmmssffff");
                    return this._name;
                }
            }

            /// <summary>
            /// 文件的扩展名
            /// 
            /// </summary>
            public string Extension
            {
                get
                {
                    if (this._upload == null)
                        return (string)null;
                    this._extension = !this._upload.IsConvertJpg ? this._upload.File.Client.Extension : "jpg";
                    return this._extension;
                }
            }

            /// <summary>
            /// 文件的全路径，为Url路径，
            /// 
            /// </summary>
            public string VirtualPath
            {
                get
                {
                    return Upload.Get[this._upload.UpPath].Virtual + this.Name + "." + this.Extension;
                }
            }

            /// <summary>
            /// 缩图图的文件名(name.ext)，
            /// 
            /// </summary>
            public string SmallFileName
            {
                get
                {
                    return this.Name + "_small." + this.Extension;
                }
            }

            /// <summary>
            /// 文件缩略图的全路径，为物理路径，
            /// 
            /// </summary>
            public string SmallPhysicalName
            {
                get
                {
                    return Upload.Get[this._upload.UpPath].Physics + this.SmallFileName;
                }
            }

            /// <summary>
            /// 文件缩略图的全路径，为Url路径，
            /// 
            /// </summary>
            public string SmallVirtualPath
            {
                get
                {
                    return Upload.Get[this._upload.UpPath].Virtual + this.SmallFileName;
                }
            }

            /// <summary>
            /// 图片高度
            /// 
            /// </summary>
            public int Height
            {
                get
                {
                    return this._height;
                }
            }

            /// <summary>
            /// 图片宽度
            /// 
            /// </summary>
            public int Width
            {
                get
                {
                    return this._width;
                }
            }

            /// <summary>
            /// 文件最后修改时间
            /// 
            /// </summary>
            public DateTime LastWriteTime
            {
                get
                {
                    return this._lastWriteTime;
                }
            }

            /// <summary>
            /// 文件创建时间
            /// 
            /// </summary>
            public DateTime CreateTime
            {
                get
                {
                    return this._createTime;
                }
            }

            /// <summary>
            /// 上传文件的根路径（物理路径）
            /// 
            /// </summary>
            public string Path
            {
                get
                {
                    return Upload.Get[this._upload.UpPath].Physics;
                }
            }

            /// <summary>
            /// 构造函数
            /// 
            /// </summary>
            /// <param name="upload">上传控件</param>
            public UploadControlServerFile(FileUpload upload)
            {
                this._upload = upload;
            }

            /// <summary>
            /// 转换图片格式
            /// 
            /// </summary>
            public void ChangeImgFormate()
            {
                if (!this._upload.IsConvertJpg)
                    return;
                try
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.FileFullName);
                    if (fileInfo.Exists)
                    {
                        this._lastWriteTime = fileInfo.LastWriteTime;
                        this._createTime = fileInfo.CreationTime;
                    }
                    Bitmap bitmap = new Bitmap(this.FileFullName);
                    string fileFullName = this.FileFullName;
                    string str = this.FileFullName + ".jpg";
                    bitmap.Save(str, ImageFormat.Jpeg);
                    bitmap.Dispose();
                    if (System.IO.File.Exists(fileFullName))
                        System.IO.File.Delete(fileFullName);
                    new System.IO.FileInfo(str).MoveTo(fileFullName);
                }
                catch
                {
                }
            }

            /// <summary>
            /// 创建缩略图
            /// 
            /// </summary>
            public void CreateSmallImage()
            {
                if (!this._upload.IsMakeSmall)
                    return;
                FileTo.Thumbnail(this.FileFullName, this.Path + this.SmallFileName, this._upload.SmallWidth, this._upload.SmallHeight, this._upload.SmallRestrain);
            }

            /// <summary>
            /// 设置图片宽高
            /// 
            /// </summary>
            /// <param name="width"/><param name="height"/><param name="isDeformation">是否允许变形，如果不允许，则会剪切图形；</param>
            public void ChangeSize(int width, int height, bool isDeformation)
            {
                string fileFullName = this.FileFullName;
                if (!System.IO.File.Exists(fileFullName))
                    return;
                FileTo.Zoom(fileFullName, width, height, isDeformation);
            }

            /// <summary>
            /// 设置图片或文件的信息，如果是图片需要设置宽高
            /// 
            /// </summary>
            public void SetFileInfo()
            {
                try
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(this.FileFullName);
                    this._width = image.Size.Width;
                    this._height = image.Size.Height;
                    image.Dispose();
                }
                catch
                {
                }
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.FileFullName);
                if (!fileInfo.Exists)
                    return;
                this._size = fileInfo.Length;
                this._lastWriteTime = fileInfo.LastWriteTime;
                this._createTime = fileInfo.CreationTime;
            }

            /// <summary>
            /// 删除指定文件，包括其缩略图
            /// 
            /// </summary>
            /// <param name="fileName"/>
            /// <returns>
            /// 删除成功，返回0；否则返回-1
            /// </returns>
            public int Delete(string fileName)
            {
                string path1 = this.Path + fileName;
                string path2 = path1;
                if (path1.IndexOf(".") > -1)
                {
                    string str = path1.Substring(path1.LastIndexOf("."));
                    path2 = path1.Substring(0, path1.LastIndexOf(".")) + "_small" + str;
                }
                try
                {
                    if (System.IO.File.Exists(path1))
                        System.IO.File.Delete(path1);
                    if (System.IO.File.Exists(path2))
                        System.IO.File.Delete(path2);
                    return 0;
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 客户端文件属性
        /// 
        /// </summary>
        public class UploadControlClientFile
        {
            private string _fileFullName;
            private string _fileName;
            private int _size;

            /// <summary>
            /// 文件的全路径，为客户端路径，
            /// 
            /// </summary>
            public string FileFullName
            {
                get
                {
                    return this._fileFullName;
                }
                set
                {
                    this._fileFullName = value;
                    if (this._fileFullName.IndexOf('\\') > -1)
                    {
                        int startIndex = this._fileFullName.LastIndexOf('\\') + 1;
                        this._fileName = this._fileFullName.Substring(startIndex, this._fileFullName.Length - startIndex);
                    }
                    else
                        this._fileName = this._fileFullName;
                }
            }

            /// <summary>
            /// 文件名(包括名称与扩展名，如name.ext)，为原文件（即待上传的文件）名；
            /// 
            /// </summary>
            public string FileName
            {
                get
                {
                    return this._fileName;
                }
            }

            /// <summary>
            /// 文件的大小，以字节为单位
            /// 
            /// </summary>
            public int Size
            {
                get
                {
                    return this._size;
                }
                set
                {
                    this._size = value;
                }
            }

            /// <summary>
            /// 文件名，为原文件（即待上传的文件）名；此处仅为文件名，不包括扩展名
            /// 
            /// </summary>
            public string Name
            {
                get
                {
                    if (this._fileName == null)
                        return (string)null;
                    if (this._fileName.IndexOf('.') > -1)
                        return this._fileName.Substring(0, this._fileName.LastIndexOf('.')).ToLower();
                    return this._fileName;
                }
            }

            /// <summary>
            /// 文件的扩展名
            /// 
            /// </summary>
            public string Extension
            {
                get
                {
                    if (this._fileName == null)
                        return (string)null;
                    if (this._fileName.IndexOf('.') <= -1)
                        return (string)null;
                    int startIndex = this._fileName.LastIndexOf('.') + 1;
                    return this._fileName.Substring(startIndex, this._fileName.Length - startIndex).ToLower();
                }
            }

            /// <summary>
            /// 构造函数
            /// 
            /// </summary>
            /// <param name="upload">上传控件</param>
            public UploadControlClientFile(System.Web.UI.WebControls.FileUpload upload)
            {
                this._size = upload.PostedFile.ContentLength;
                this.FileFullName = upload.PostedFile.FileName;
            }
        }
    }
}
