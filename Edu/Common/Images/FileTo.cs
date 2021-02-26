using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Common.Images
{
	// Token: 0x020000AD RID: 173
	public class FileTo
	{
		/// <summary>
		/// 生成缩略图，非变形剪切；
		/// </summary>
		/// <param name="sourceFile">原文件地址</param>
		/// <param name="thFile">缩略图文件地址</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="restrainObj">约束对象，1为按度约束，高度按宽的比较计算；2为按高约束；0为自适应</param>
		/// <returns></returns>
		// Token: 0x06000496 RID: 1174 RVA: 0x00022C6C File Offset: 0x00020E6C
		public static string Thumbnail(string sourceFile, string thFile, int width, int height, int restrainObj)
		{
			if (!File.Exists(sourceFile))
			{
				return sourceFile;
			}
			Image image = Image.FromFile(sourceFile);
			Image image2 = ImageTo.Thumbnail(image, width, height, restrainObj);
			image2.Save(thFile, ImageFormat.Jpeg);
			image.Dispose();
			image2.Dispose();
			return thFile;
		}

		/// <summary>
		/// 图片叠加，例如生成水印图片
		/// </summary>
		/// <param name="sourceFile">原文件地址</param>
		/// <param name="overFile">要叠加的图片文件地址</param>
		/// <param name="overType"></param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		// Token: 0x06000497 RID: 1175 RVA: 0x00022CB0 File Offset: 0x00020EB0
		public static string OverlayImage(string sourceFile, string overFile, string overType, int opacity)
		{
			if (!File.Exists(sourceFile))
			{
				return sourceFile;
			}
			if (!File.Exists(overFile))
			{
				return sourceFile;
			}
			Image image = Image.FromFile(sourceFile);
			Image image2 = Image.FromFile(overFile);
			image = ImageTo.Overlay(image, image2, overType, opacity);
			string text = sourceFile.Substring(0, sourceFile.LastIndexOf(".")) + "_new" + sourceFile.Substring(sourceFile.LastIndexOf("."));
			image.Save(text, ImageFormat.Jpeg);
			image.Dispose();
			image2.Dispose();
			File.Delete(sourceFile);
			File.Move(text, sourceFile);
			return sourceFile;
		}

		/// <summary>
		/// 图片文件叠加
		/// </summary>
		/// <param name="sourceFile">原文件</param>
		/// <param name="text">要叠加的文字</param>
		/// <param name="overType">叠加的位置</param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		// Token: 0x06000498 RID: 1176 RVA: 0x00022D40 File Offset: 0x00020F40
		public static string OverlayText(string sourceFile, string text, string overType, int opacity)
		{
			if (!File.Exists(sourceFile))
			{
				return sourceFile;
			}
			using (Image image = Image.FromFile(sourceFile))
			{
				Image image2 = ImageTo.Overlay(image, text, overType, opacity);
				string text2 = sourceFile.Substring(0, sourceFile.LastIndexOf(".")) + "_new" + sourceFile.Substring(sourceFile.LastIndexOf("."));
				image2.Save(text2, ImageFormat.Jpeg);
				image.Dispose();
				image2.Dispose();
				File.Delete(sourceFile);
				File.Move(text2, sourceFile);
			}
			return sourceFile;
		}

		/// <summary>
		/// 图片缩放，强制大小尺寸
		/// </summary>
		/// <param name="sourceFile"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		// Token: 0x06000499 RID: 1177 RVA: 0x00022DD8 File Offset: 0x00020FD8
		public static string Zoom(string sourceFile, int width, int height)
		{
			if (!File.Exists(sourceFile))
			{
				return sourceFile;
			}
			if (width < 1 || height < 1)
			{
				return sourceFile;
			}
			using (Image image = Image.FromFile(sourceFile))
			{
				Image image2 = ImageTo.Zoom(image, width, height);
				string text = sourceFile.Substring(0, sourceFile.LastIndexOf(".")) + "_new" + sourceFile.Substring(sourceFile.LastIndexOf("."));
				image2.Save(text, ImageFormat.Jpeg);
				image.Dispose();
				image2.Dispose();
				File.Delete(sourceFile);
				File.Move(text, sourceFile);
			}
			return sourceFile;
		}

		/// <summary>
		/// 图片缩放，强制大小尺寸
		/// </summary>
		/// <param name="sourceFile"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="isDeformation">是否允许变形，如果不允许，则会剪切图形；</param>
		/// <returns></returns>
		// Token: 0x0600049A RID: 1178 RVA: 0x00022E7C File Offset: 0x0002107C
		public static string Zoom(string sourceFile, int width, int height, bool isDeformation)
		{
			if (isDeformation)
			{
				return FileTo.Zoom(sourceFile, width, height);
			}
			if (!File.Exists(sourceFile))
			{
				return sourceFile;
			}
			if (width < 1 || height < 1)
			{
				return sourceFile;
			}
			using (Image image = Image.FromFile(sourceFile))
			{
				Image image2 = ImageTo.Zoom(image, width, height, isDeformation);
				string text = sourceFile.Substring(0, sourceFile.LastIndexOf(".")) + "_new" + sourceFile.Substring(sourceFile.LastIndexOf("."));
				image2.Save(text, ImageFormat.Jpeg);
				image.Dispose();
				image2.Dispose();
				File.Delete(sourceFile);
				File.Move(text, sourceFile);
			}
			return sourceFile;
		}

		/// <summary>
		/// 将图片文件转为Iamge对象
		/// </summary>
		/// <param name="sourceFile"></param>
		/// <returns></returns>
		// Token: 0x0600049B RID: 1179 RVA: 0x00022F2C File Offset: 0x0002112C
		public static Image ToImage(string sourceFile)
		{
			if (!File.Exists(sourceFile))
			{
				return null;
			}
			return Image.FromFile(sourceFile);
		}

		/// <summary>
		/// 图片文件转Base64，如果在网页中引用，请加上data:image/jpeg;base64,
		/// </summary>
		/// <param name="imgFile"></param>
		/// <returns></returns>
		// Token: 0x0600049C RID: 1180 RVA: 0x00022F4C File Offset: 0x0002114C
		public static string ToBase64(string imgFile)
		{
			if (!File.Exists(imgFile))
			{
				return null;
			}
			string result = string.Empty;
			using (Bitmap bitmap = new Bitmap(imgFile))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					bitmap.Save(memoryStream, ImageFormat.Jpeg);
					byte[] array = new byte[memoryStream.Length];
					memoryStream.Position = 0L;
					memoryStream.Read(array, 0, (int)memoryStream.Length);
					memoryStream.Close();
					result = Convert.ToBase64String(array);
				}
				bitmap.Dispose();
			}
			return result;
		}
	}
}
