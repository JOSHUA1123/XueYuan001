using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Common.VideoConverter;

namespace Common
{
	/// <summary>
	/// 视频操作方法，此为入口类
	/// </summary>
	// Token: 0x0200006A RID: 106
	public class VideoHandler
	{
		/// <summary>
		/// 通过视频文件名创建视频操作对象
		/// </summary>
		/// <param name="videoFile"></param>
		/// <returns></returns>
		// Token: 0x060002A8 RID: 680 RVA: 0x0000A1A4 File Offset: 0x000083A4
		public static VideoHandler Hanlder(string videoFile)
		{
			return new VideoHandler(videoFile);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00018394 File Offset: 0x00016594
		private VideoHandler(string videoFile)
		{
			this._videoFile = videoFile;
			this._init();
		}

		/// <summary>
		/// 初始化各种值
		/// </summary>
		// Token: 0x060002AA RID: 682 RVA: 0x00018450 File Offset: 0x00016650
		private void _init()
		{
			if (!File.Exists(this._videoFile))
			{
				return;
			}
			FileInfo fileInfo = new FileInfo(this._videoFile);
			if (fileInfo.Exists)
			{
				this._filename = fileInfo.Name;
				this._fileExtension = fileInfo.Extension.Replace(".", "");
				this._filefullname = fileInfo.FullName;
				this._name = ((this._filename.IndexOf(".") > -1) ? this._filename.Substring(0, this._filename.LastIndexOf(".")) : this._filename);
				this._filePath = fileInfo.DirectoryName;
			}
			this._xmlInfo = VideoHandler.getVideoInfo(this._videoFile);
			if (this._xmlInfo == null)
			{
				return;
			}
			XmlNode xmlNode = null;
			XmlNode xmlNode2 = null;
			XmlNodeList elementsByTagName = this._xmlInfo.GetElementsByTagName("stream");
			foreach (object obj in elementsByTagName)
			{
				XmlNode xmlNode3 = (XmlNode)obj;
				if (xmlNode3.Attributes["codec_type"].Value == "video")
				{
					xmlNode = xmlNode3;
				}
				if (xmlNode3.Attributes["codec_type"].Value == "audio")
				{
					xmlNode2 = xmlNode3;
				}
			}
			if (xmlNode != null)
			{
				this._width = int.Parse(xmlNode.Attributes["width"].Value);
				this._height = int.Parse(xmlNode.Attributes["height"].Value);
				this._vcodec = xmlNode.Attributes["codec_name"].Value;
				this._vcodecfull = xmlNode.Attributes["codec_long_name"].Value;
			}
			if (xmlNode2 != null)
			{
				this._audioCode = xmlNode2.Attributes["codec_name"].Value;
				this._audioCodefull = xmlNode2.Attributes["codec_long_name"].Value;
			}
			XmlNode xmlNode4 = this._xmlInfo.SelectSingleNode("/ffprobe/format");
			if (xmlNode4 != null)
			{
				this._formate = xmlNode4.Attributes["format_name"].Value;
				this._formatefull = xmlNode4.Attributes["format_long_name"].Value;
				string value = (xmlNode4.Attributes["duration"] != null) ? xmlNode4.Attributes["duration"].Value : "";
				this._duration = VideoHandler.durationForTimeSpan(value);
				this._size = long.Parse(xmlNode4.Attributes["size"].Value);
				this._bitRate = ((xmlNode4.Attributes["bit_rate"] != null) ? xmlNode4.Attributes["bit_rate"].Value : "");
			}
		}

		/// <summary>
		/// 获取视频文件的各项信息，返回xml对象
		/// </summary>
		/// <param name="videoFile"></param>
		/// <returns></returns>
		// Token: 0x060002AB RID: 683 RVA: 0x00018758 File Offset: 0x00016958
		private static XmlDocument getVideoInfo(string videoFile)
		{
			if (!File.Exists(videoFile))
			{
				return null;
			}
			XmlDocument result;
			try
			{
				string arguments = " -v quiet -print_format xml -show_format -show_streams " + videoFile;
				string text = string.Empty;
				using (Process process = new Process())
				{
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.FileName = VideoHandler.FfprobePath;
					process.StartInfo.Arguments = arguments;
					process.Start();
					process.WaitForExit();
					text = process.StandardOutput.ReadToEnd();
				}
				if (string.IsNullOrWhiteSpace(text) || text.Trim() == "")
				{
					result = null;
				}
				else
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(text);
					result = xmlDocument;
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 将字符型的 h:m:s.f 数据转换为 TimeSpan
		/// </summary>
		// Token: 0x060002AC RID: 684 RVA: 0x00018844 File Offset: 0x00016A44
		private static TimeSpan durationForTimeSpan(string value)
		{
			Match match = VideoHandler._rexTimeSpan.Match(value);
			double num = 0.0;
			if (match == null || !match.Success)
			{
				return default(TimeSpan);
			}
			if (!string.IsNullOrEmpty(match.Groups["h"].Value))
			{
				num += (double)int.Parse(match.Groups["h"].Value);
			}
			num *= 60.0;
			if (!string.IsNullOrEmpty(match.Groups["m"].Value))
			{
				num += (double)int.Parse(match.Groups["m"].Value);
			}
			num *= 60.0;
			if (!string.IsNullOrEmpty(match.Groups["s"].Value))
			{
				num += (double)int.Parse(match.Groups["s"].Value);
			}
			if (!string.IsNullOrEmpty(match.Groups["f"].Value))
			{
				num += double.Parse(string.Format("0{1}{0}", match.Groups["f"].Value, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
			}
			return TimeSpan.FromSeconds(num);
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000A1AC File Offset: 0x000083AC
		public static string FfmpegPath
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(VideoHandler._ffmpegPath))
				{
					return VideoHandler._ffmpegPath;
				}
				return VideoHandler.getPath("ffmpeg.exe", out VideoHandler._ffmpegPath);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000A1CF File Offset: 0x000083CF
		public static string MencoderPath
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(VideoHandler._mencoderPath))
				{
					return VideoHandler._mencoderPath;
				}
				return VideoHandler.getPath("mencoder.exe", out VideoHandler._mencoderPath);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000A1F2 File Offset: 0x000083F2
		public static string FfprobePath
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(VideoHandler._ffprobePath))
				{
					return VideoHandler._ffprobePath;
				}
				return VideoHandler.getPath("ffprobe.exe", out VideoHandler._ffprobePath);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000A215 File Offset: 0x00008415
		public static string FaststartPath
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(VideoHandler._faststartPath))
				{
					return VideoHandler._faststartPath;
				}
				return VideoHandler.getPath("qt-faststart.exe", out VideoHandler._faststartPath);
			}
		}

		/// <summary>
		/// 返回转换视频的应用路径
		/// </summary>
		/// <param name="filename">工具的文件名</param>
		/// <param name="fullpath">完整路径名</param>
		/// <returns></returns>
		// Token: 0x060002B1 RID: 689 RVA: 0x0001899C File Offset: 0x00016B9C
		private static string getPath(string filename, out string fullpath)
		{
			string mapPath = App.Get["VideoHandler"].MapPath;
			fullpath = mapPath + filename;
			return fullpath;
		}

		/// <summary>
		/// 被操作的视频文件
		/// </summary>
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000A238 File Offset: 0x00008438
		public string VideoFile
		{
			get
			{
				return this._videoFile;
			}
		}

		/// <summary>
		/// 视频宽度
		/// </summary>
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000A240 File Offset: 0x00008440
		public int Width
		{
			get
			{
				return this._width;
			}
		}

		/// <summary>
		/// 视频高度
		/// </summary>
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000A248 File Offset: 0x00008448
		public int Height
		{
			get
			{
				return this._height;
			}
		}

		/// <summary>
		/// 视频时长
		/// </summary>
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000A250 File Offset: 0x00008450
		public TimeSpan Duration
		{
			get
			{
				return this._duration;
			}
		}

		/// <summary>
		/// 视频文件的大小，单位：字节
		/// </summary>
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000A258 File Offset: 0x00008458
		public long Size
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>
		/// 视频文件的大小，以字符形式
		/// </summary>
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x000189CC File Offset: 0x00016BCC
		public string SizeFormate
		{
			get
			{
				if (this._size >= 1073741824L)
				{
					return string.Format("{0:########0.00} GB", (double)this._size / 1073741824.0);
				}
				if (this._size >= 1048576L)
				{
					return string.Format("{0:####0.00} MB", (double)this._size / 1048576.0);
				}
				if (this._size >= 1024L)
				{
					return string.Format("{0:####0.00} KB", (double)this._size / 1024.0);
				}
				return string.Format("{0} bytes", this._size);
			}
		}

		/// <summary>
		/// 视频格式（简短名称）
		/// </summary>
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000A260 File Offset: 0x00008460
		public string Format
		{
			get
			{
				return this._formate;
			}
		}

		/// <summary>
		/// 视频格式（完整名称）
		/// </summary>
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000A268 File Offset: 0x00008468
		public string FormatFull
		{
			get
			{
				return this._formatefull;
			}
		}

		/// <summary>
		/// 视频编码（简短名称）
		/// </summary>
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000A270 File Offset: 0x00008470
		public string VideoCode
		{
			get
			{
				return this._vcodec;
			}
		}

		/// <summary>
		/// 视频编码（简短名称）
		/// </summary>
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000A278 File Offset: 0x00008478
		public string BitRate
		{
			get
			{
				return this._bitRate;
			}
		}

		/// <summary>
		/// 视频编码（完整名称）
		/// </summary>
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000A280 File Offset: 0x00008480
		public string VideoCodeFull
		{
			get
			{
				return this._vcodecfull;
			}
		}

		/// <summary>
		/// 音频编码（简短名称）
		/// </summary>
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000A288 File Offset: 0x00008488
		public string AudioCode
		{
			get
			{
				return this._audioCode;
			}
		}

		/// <summary>
		/// 音频编码（完整名称）
		/// </summary>
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000A290 File Offset: 0x00008490
		public string AudioCodeFull
		{
			get
			{
				return this._audioCodefull;
			}
		}

		/// <summary>
		/// 视频文件名称，不带路径与扩展名
		/// </summary>
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000A298 File Offset: 0x00008498
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		/// <summary>
		/// 视频频文件的扩展名，不带点
		/// </summary>
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000A2A0 File Offset: 0x000084A0
		public string FileExtension
		{
			get
			{
				return this._fileExtension;
			}
		}

		/// <summary>
		/// 视频频文件名称，不带路径名
		/// </summary>
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000A2A8 File Offset: 0x000084A8
		public string FileName
		{
			get
			{
				return this._filename;
			}
		}

		/// <summary>
		/// 视频频文件名称，带完整路径名
		/// </summary>
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000A2B0 File Offset: 0x000084B0
		public string FileFullName
		{
			get
			{
				return this._filefullname;
			}
		}

		/// <summary>
		/// 视频频文件的路径名称，不包括文件
		/// </summary>
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000A2B8 File Offset: 0x000084B8
		public string FilePath
		{
			get
			{
				return this._filePath;
			}
		}

		/// <summary>
		/// 视频信息的完整XML对象
		/// </summary>
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000A2C0 File Offset: 0x000084C0
		public XmlDocument XMLInfo
		{
			get
			{
				return this._xmlInfo;
			}
		}

		/// <summary>
		/// 视频转换的操作对象(程序会通过视频文件的后缀名判断返回操作对)
		/// </summary>
		/// <returns></returns>
		// Token: 0x060002C5 RID: 709 RVA: 0x00018A7C File Offset: 0x00016C7C
		public IVideoHanlder Convert()
		{
			string text = string.Empty;
			if (this._videoFile.IndexOf('.') > -1)
			{
				text = this._videoFile.Substring(this._videoFile.LastIndexOf('.'));
			}
			text = text.Replace(".", "");
			IVideoHanlder videoHanlder = null;
			foreach (string a in VideoHandler.strArrMencoder)
			{
				if (a == text)
				{
					videoHanlder = new MencoderHandler(this);
					break;
				}
			}
			if (videoHanlder == null)
			{
				videoHanlder = new FfmpegHandler(this);
			}
			return videoHanlder;
		}

		/// <summary>
		/// 视频转换的操作对象
		/// </summary>
		/// <param name="funcname">直接指定方法名,ffmpeg、mencoder</param>
		// Token: 0x060002C6 RID: 710 RVA: 0x00018B08 File Offset: 0x00016D08
		public IVideoHanlder Convert(string funcname)
		{
			IVideoHanlder videoHanlder = null;
			if (string.IsNullOrWhiteSpace(funcname))
			{
				videoHanlder = new FfmpegHandler(this);
			}
			if (funcname.Trim().ToLower() == "ffmpeg")
			{
				videoHanlder = new FfmpegHandler(this);
			}
			if (funcname.Trim().ToLower() == "mencoder")
			{
				videoHanlder = new MencoderHandler(this);
			}
			if (videoHanlder == null)
			{
				videoHanlder = new FfmpegHandler(this);
			}
			return videoHanlder;
		}

		/// <summary>
		/// 执行命令行指令
		/// </summary>
		/// <param name="exePath">要执行的程序文件（完整路径名）</param>
		/// <param name="parameters">执行命令的参数</param>
		/// <returns></returns>
		// Token: 0x060002C7 RID: 711 RVA: 0x00018B70 File Offset: 0x00016D70
		public static void ExecuteCmd(string exePath, string parameters)
		{
			bool flag = App.Get["VideoHandlerIsLog"].Boolean ?? true;
			string path = App.Get["VideoHandler"].MapPath + "log.txt";
			if (flag)
			{
				using (StreamWriter streamWriter = new StreamWriter(path, true))
				{
					streamWriter.WriteLine(DateTime.Now.ToString());
					streamWriter.WriteLine(exePath + " " + parameters);
					streamWriter.WriteLine("");
				}
			}
			try
			{
				using (Process process = new Process())
				{
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = false;
					process.StartInfo.RedirectStandardOutput = false;
					process.StartInfo.FileName = exePath;
					process.StartInfo.Arguments = parameters;
					process.Start();
					process.WaitForExit();
				}
			}
			catch (Exception ex)
			{
				using (StreamWriter streamWriter2 = new StreamWriter(path, true))
				{
					streamWriter2.WriteLine("错误信息：" + ex.Message);
					streamWriter2.WriteLine(DateTime.Now.ToString());
					streamWriter2.WriteLine(exePath + " " + parameters);
					streamWriter2.WriteLine(ex.StackTrace);
				}
			}
		}

		/// <summary>
		/// 删除视频文件
		/// </summary>
		// Token: 0x060002C8 RID: 712 RVA: 0x0000A2C8 File Offset: 0x000084C8
		public void Delete()
		{
			if (File.Exists(this.FileFullName))
			{
				File.Delete(this.FileFullName);
			}
		}

		/// <summary>
		/// 删除视频文件
		/// </summary>
		/// <param name="outexist">要排除的扩展名</param>
		// Token: 0x060002C9 RID: 713 RVA: 0x00018D14 File Offset: 0x00016F14
		public void Delete(string outexist)
		{
			if (!File.Exists(this._filefullname))
			{
				return;
			}
			string text = "";
			try
			{
				FileInfo fileInfo = new FileInfo(this._filefullname);
				text = fileInfo.Extension.ToLower();
				text = text.Replace(".", "");
			}
			catch
			{
				throw new Exception("文件路径：“" + this._filefullname + "”不合法");
			}
			bool flag = false;
			foreach (string text2 in outexist.Split(new char[]
			{
				','
			}))
			{
				if (text2.Trim().ToLower() == text)
				{
					flag = true;
					break;
				}
			}
			if (!flag && File.Exists(this._filefullname))
			{
				File.Delete(this._filefullname);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000A2E2 File Offset: 0x000084E2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00018DF4 File Offset: 0x00016FF4
		~VideoHandler()
		{
			this.Dispose(false);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000A2F1 File Offset: 0x000084F1
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
		}

		// Token: 0x040000E1 RID: 225
		private static readonly Regex _rexTimeSpan = new Regex("^(((?<h>\\d+):)?(?<m>\\d+):)?(?<s>\\d+)([\\.,](?<f>\\d+))?$", RegexOptions.Compiled);

		// Token: 0x040000E2 RID: 226
		private static string _ffmpegPath = string.Empty;

		// Token: 0x040000E3 RID: 227
		private static string _mencoderPath = string.Empty;

		// Token: 0x040000E4 RID: 228
		private static string _ffprobePath = string.Empty;

		// Token: 0x040000E5 RID: 229
		private static string _faststartPath = string.Empty;

		// Token: 0x040000E6 RID: 230
		private string _videoFile = string.Empty;

		// Token: 0x040000E7 RID: 231
		private int _width;

		// Token: 0x040000E8 RID: 232
		private int _height;

		// Token: 0x040000E9 RID: 233
		private TimeSpan _duration = TimeSpan.MinValue;

		// Token: 0x040000EA RID: 234
		private long _size;

		// Token: 0x040000EB RID: 235
		private string _formate = string.Empty;

		// Token: 0x040000EC RID: 236
		private string _formatefull = string.Empty;

		// Token: 0x040000ED RID: 237
		private string _vcodec = string.Empty;

		// Token: 0x040000EE RID: 238
		private string _bitRate = string.Empty;

		// Token: 0x040000EF RID: 239
		private string _vcodecfull = string.Empty;

		// Token: 0x040000F0 RID: 240
		private string _audioCode = string.Empty;

		// Token: 0x040000F1 RID: 241
		private string _audioCodefull = string.Empty;

		// Token: 0x040000F2 RID: 242
		private string _name = string.Empty;

		// Token: 0x040000F3 RID: 243
		private string _fileExtension = string.Empty;

		// Token: 0x040000F4 RID: 244
		private string _filename = string.Empty;

		// Token: 0x040000F5 RID: 245
		private string _filefullname = string.Empty;

		// Token: 0x040000F6 RID: 246
		private string _filePath = string.Empty;

		// Token: 0x040000F7 RID: 247
		private XmlDocument _xmlInfo;

		// Token: 0x040000F8 RID: 248
		private static string[] strArrFfmpeg = new string[]
		{
			"asf",
			"avi",
			"mpg",
			"3gp",
			"mov",
			"mp4",
			"mkv"
		};

		// Token: 0x040000F9 RID: 249
		private static string[] strArrMencoder = new string[]
		{
			"wmv",
			"rmvb",
			"rm"
		};

		// Token: 0x040000FA RID: 250
		private bool _disposed;
	}
}
