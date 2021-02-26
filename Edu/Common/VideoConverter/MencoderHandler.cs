using System;

namespace Common.VideoConverter
{
	// Token: 0x02000026 RID: 38
	public class MencoderHandler : IVideoHanlder
	{
		/// <summary>
		/// 被操作的视频文件
		/// </summary>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000095F9 File Offset: 0x000077F9
		public VideoHandler VideoObj
		{
			get
			{
				return this._vobj;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00009601 File Offset: 0x00007801
		public MencoderHandler(VideoHandler videoObj)
		{
			this._vobj = videoObj;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00009610 File Offset: 0x00007810
		public string ToFlv()
		{
			return this.ToFlv(-1);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00010188 File Offset: 0x0000E388
		public string ToFlv(int qscale)
		{
			string filePath = this._vobj.FilePath;
			string name = this._vobj.Name;
			return this.ToFlv(filePath, name, qscale);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000101B8 File Offset: 0x0000E3B8
		public string ToFlv(string newName, int qscale)
		{
			string filePath = this._vobj.FilePath;
			return this.ToFlv(filePath, newName, qscale);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000101DC File Offset: 0x0000E3DC
		public string ToFlv(string newPath, string newName, int qscale)
		{
			qscale = ((qscale < 1) ? 1 : qscale);
			if (this._vobj.VideoCode == "flv")
			{
				return this._vobj.FileFullName;
			}
			string text = newPath + "\\" + newName + ".flv";
			string parameters = "  -quiet  -oac mp3lame -lameopts abr:br=56 -srate 22050 -af channels=2  -ovc lavc  -vf harddup,hqdn3d,scale=176:-3   -lavcopts vcodec=flv:vbitrate=152:mbd=2:trell:v4mv:turbo:keyint=45 -ofps 15 -of lavf   " + this._vobj.FileFullName + " -o " + text;
			VideoHandler.ExecuteCmd(VideoHandler.FfmpegPath, parameters);
			return text;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00010250 File Offset: 0x0000E450
		public string ToMP4(int qscale, bool isCover)
		{
			string filePath = this._vobj.FilePath;
			string name = this._vobj.Name;
			return this.ToMP4(filePath, name, qscale, isCover);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00010280 File Offset: 0x0000E480
		public string ToMP4(string newName, int qscale, bool isCover)
		{
			string filePath = this._vobj.FilePath;
			return this.ToMP4(filePath, newName, qscale, isCover);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000102A4 File Offset: 0x0000E4A4
		public string ToMP4(string newPath, string newName, int qscale, bool isCover)
		{
			qscale = ((qscale < 1) ? 1 : qscale);
			if (this._vobj.VideoCode == "flv")
			{
				return this._vobj.FileFullName;
			}
			string text = newPath + "\\" + newName + ".mp4";
			string.Format(" {0} -o {1} {2}", this._vobj.FileFullName, text);
			string parameters = "  -quiet  -oac mp3lame -lameopts abr:br=56 -srate 22050 -af channels=2  -ovc lavc  -vf harddup,hqdn3d,scale=176:-3   -lavcopts vcodec=flv:vbitrate=152:mbd=2:trell:v4mv:turbo:keyint=45 -ofps 15 -of lavf   " + this._vobj.FileFullName + " -o " + text;
			VideoHandler.ExecuteCmd(VideoHandler.MencoderPath, parameters);
			return text;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00009619 File Offset: 0x00007819
		public string GetImage()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00009619 File Offset: 0x00007819
		public string GetImage(string newName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000045 RID: 69
		private VideoHandler _vobj;
	}
}
