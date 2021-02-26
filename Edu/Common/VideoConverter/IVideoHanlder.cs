using System;

namespace Common.VideoConverter
{
	// Token: 0x02000025 RID: 37
	public interface IVideoHanlder
	{
		/// <summary>
		/// 要操作的视频对象
		/// </summary>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000D3 RID: 211
		VideoHandler VideoObj { get; }

		/// <summary>
		/// 转换为Flv视频格式
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000D4 RID: 212
		string ToFlv();

		/// <summary>
		/// 转换为Flv视频格式
		/// </summary>
		/// <param name="qscale">视频质量(1-255)，值越小质量越高输出文件就越大,50以后质量就非常糟糕了；ffmpeg官方解释：固定的视频量化标度</param>
		/// <returns>视频完整路径名</returns>
		// Token: 0x060000D5 RID: 213
		string ToFlv(int qscale);

		/// <summary>
		/// 转换为Flv视频格式
		/// </summary>
		/// <param name="newName">指定新的文件名，此处不包括扩展名，系统自动加上.flv</param>
		/// <param name="qscale"></param>
		/// <returns>视频完整路径名</returns>
		// Token: 0x060000D6 RID: 214
		string ToFlv(string newName, int qscale);

		/// <summary>
		/// 转换为Flv视频格式
		/// </summary>
		/// <param name="newPath">指定输出文件的所在路径</param>
		/// <param name="newName"></param>
		/// <param name="qscale"></param>
		/// <returns>视频完整路径名</returns>
		// Token: 0x060000D7 RID: 215
		string ToFlv(string newPath, string newName, int qscale);

		/// <summary>
		/// 转换为MP4
		/// </summary>
		/// <param name="qscale">视频质量，后边的值越小质量越高，但是输出文件就越大；ffmpeg官方解释：固定的视频量化标度</param>
		/// <param name="isCover">是否覆盖已经转换的视频，如果为flase将跳过；如果为true将删除已经生成的</param>
		/// <returns>视频完整路径名</returns>
		// Token: 0x060000D8 RID: 216
		string ToMP4(int qscale, bool isCover);

		/// <summary>
		/// 转换为MP4
		/// </summary>
		/// <param name="newName">指定新的文件名，此处不包括扩展名，系统自动加上.mp4</param>
		/// <param name="qscale"></param>
		/// <param name="isCover"></param>
		/// <returns>视频完整路径名</returns>
		// Token: 0x060000D9 RID: 217
		string ToMP4(string newName, int qscale, bool isCover);

		/// <summary>
		/// 转换为MP4
		/// </summary>
		/// <param name="newPath">指定输出文件的所在路径</param>
		/// <param name="newName"></param>
		/// <param name="qscale"></param>
		/// <param name="isCover"></param>
		/// <returns>视频完整路径名</returns>
		// Token: 0x060000DA RID: 218
		string ToMP4(string newPath, string newName, int qscale, bool isCover);

		/// <summary>
		/// 获取视频截图
		/// </summary>
		/// <returns>图片完整路径名</returns>
		// Token: 0x060000DB RID: 219
		string GetImage();

		// Token: 0x060000DC RID: 220
		string GetImage(string newName);
	}
}
