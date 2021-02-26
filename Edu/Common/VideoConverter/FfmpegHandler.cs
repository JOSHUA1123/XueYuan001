using System;
using System.IO;

namespace Common.VideoConverter
{
	// Token: 0x02000027 RID: 39
	public class FfmpegHandler : IVideoHanlder
	{
		/// <summary>
		/// 被操作的视频文件
		/// </summary>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00009620 File Offset: 0x00007820
		public VideoHandler VideoObj
		{
			get
			{
				return this._vobj;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00009628 File Offset: 0x00007828
		public FfmpegHandler(VideoHandler videoObj)
		{
			this._vobj = videoObj;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00009637 File Offset: 0x00007837
		public string ToFlv()
		{
			return this.ToFlv(-1);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00010330 File Offset: 0x0000E530
		public string ToFlv(int qscale)
		{
			string filePath = this._vobj.FilePath;
			string name = this._vobj.Name;
			return this.ToFlv(filePath, name, qscale);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00010360 File Offset: 0x0000E560
		public string ToFlv(string newName, int qscale)
		{
			string filePath = this._vobj.FilePath;
			return this.ToFlv(filePath, newName, qscale);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00010384 File Offset: 0x0000E584
		public string ToFlv(string newPath, string newName, int qscale)
		{
			qscale = ((qscale < 1) ? 5 : qscale);
			string str = newName;
			if (this._vobj.FileExtension.ToLower() == "flv")
			{
				newName += "_tmp";
			}
			string text = newPath + "\\" + newName + ".flv";
			string text2 = newPath + "\\" + str + ".flv";
			string text3 = (this._vobj.AudioCode.ToLower() == "aac") ? "copy" : "aac";
			string text4 = (this._vobj.VideoCode.ToLower() == "h264") ? "copy" : "libx264";
			string text5;
			if (this._vobj.Width > 0 && this._vobj.Height > 0)
			{
				text5 = " -i {0} -f flv -acodec {5}  -vcodec {6} -qscale {4} -r 15 -s {2}x{3} -y {1}";
				text5 = string.Format(text5, new object[]
				{
					this._vobj.FileFullName,
					text,
					this._vobj.Width,
					this._vobj.Height,
					qscale,
					text3,
					text4
				});
			}
			else
			{
				text5 = " -i {0} -f flv -acodec {3}  -vcodec {4} -qscale {2} -r 15 -y {1}";
				text5 = string.Format(text5, new object[]
				{
					this._vobj.FileFullName,
					text,
					qscale,
					text3,
					text4
				});
			}
			VideoHandler.ExecuteCmd(VideoHandler.FfmpegPath, text5);
			if (this._vobj.FileExtension.ToLower() == "flv" && File.Exists(text))
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Move(text, text2);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			return text2;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0001055C File Offset: 0x0000E75C
		public string ToMP4(int qscale, bool isCover)
		{
			string filePath = this._vobj.FilePath;
			string name = this._vobj.Name;
			return this.ToMP4(filePath, name, qscale, isCover);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0001058C File Offset: 0x0000E78C
		public string ToMP4(string newName, int qscale, bool isCover)
		{
			string filePath = this._vobj.FilePath;
			return this.ToMP4(filePath, newName, qscale, isCover);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000105B0 File Offset: 0x0000E7B0
		public string ToMP4(string newPath, string newName, int qscale, bool isCover)
		{
			qscale = ((qscale < 1) ? 5 : qscale);
			string text = newPath + "\\" + newName + ".mp4";
			FileInfo fileInfo = new FileInfo(text);
			if (fileInfo.Exists && !isCover)
			{
				return text;
			}
			string str = newName;
			if (this._vobj.FileExtension.ToLower() == "mp4")
			{
				newName += "_tmp";
			}
			text = newPath + "\\" + newName + ".mp4";
			string text2 = newPath + "\\" + str + ".mp4";
			string text3 = (this._vobj.AudioCode.ToLower() == "aac") ? "copy" : "aac";
			string text4 = (this._vobj.VideoCode.ToLower() == "h264") ? "copy" : "libx264";
			string text5;
			if (this._vobj.Width > 0 && this._vobj.Height > 0)
			{
				text5 = " -y -i {0} -f mp4 -async 1 -s {2}x{3} -acodec {5}  -vcodec {6} -qscale {4} -dts_delta_threshold 1 {1}";
				text5 = string.Format(text5, new object[]
				{
					this._vobj.FileFullName,
					text,
					this._vobj.Width,
					this._vobj.Height,
					qscale,
					text3,
					text4
				});
			}
			else
			{
				text5 = " -y -i {0} -f mp4 -async 1 -acodec {3}  -vcodec {4} -qscale {2} -dts_delta_threshold 1 {1}";
				text5 = string.Format(text5, new object[]
				{
					this._vobj.FileFullName,
					text,
					qscale,
					text3,
					text4
				});
			}
			VideoHandler.ExecuteCmd(VideoHandler.FfmpegPath, text5);
			if (this._vobj.FileExtension.ToLower() == "mp4" && File.Exists(text))
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Move(text, text2);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			if (File.Exists(text))
			{
				string text6 = text + "_fastMp4";
				VideoHandler.ExecuteCmd(VideoHandler.FaststartPath, text + " " + text6);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				File.Move(text6, text);
				if (File.Exists(text6))
				{
					File.Delete(text6);
				}
			}
			return text2;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00009619 File Offset: 0x00007819
		public string GetImage()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00009619 File Offset: 0x00007819
		public string GetImage(string newName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000046 RID: 70
		private VideoHandler _vobj;
	}
}
