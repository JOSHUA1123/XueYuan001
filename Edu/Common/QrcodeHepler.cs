using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;
using com.google.zxing.qrcode.decoder;
using Common.Images;

namespace Common
{
	/// <summary>
	/// 二维码操作类
	/// </summary>
	// Token: 0x0200009D RID: 157
	public class QrcodeHepler
	{
		/// <summary>
		/// 生成二维码，默认是高容错图像
		/// </summary>
		/// <param name="content">要生成的文本内容</param>
		/// <param name="wh">二维码的宽高</param>
		/// <param name="color">二维码前景色</param>
		/// <param name="bgcolor">二维码背景色</param>
		/// <returns></returns>
		// Token: 0x06000421 RID: 1057 RVA: 0x00021078 File Offset: 0x0001F278
		public static Image Encode(string content, int wh, string color, string bgcolor)
		{
			ByteMatrix matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, wh, wh, null);
			return QrcodeHepler.toBitmap(matrix, color, bgcolor);
		}

		/// <summary>
		/// 批量生成二维码，默认是高容错图像
		/// </summary>
		/// <param name="content"></param>
		/// <param name="wh"></param>
		/// <param name="color"></param>
		/// <param name="bgcolor"></param>
		/// <returns></returns>
		// Token: 0x06000422 RID: 1058 RVA: 0x000210A4 File Offset: 0x0001F2A4
		public static Image[] Encode(string[] content, int wh, string color, string bgcolor)
		{
			List<Image> list = new List<Image>();
			for (int i = 0; i < content.Length; i++)
			{
				if (string.IsNullOrWhiteSpace(content[i]))
				{
					list.Add(null);
				}
				else
				{
					ByteMatrix matrix = new MultiFormatWriter().encode(content[i], BarcodeFormat.QR_CODE, wh, wh, null);
					Image item = QrcodeHepler.toBitmap(matrix, color, bgcolor);
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		/// <summary>
		/// 生成二维码，可以设置容错级别
		/// </summary>
		/// <param name="content">二维码内容</param>
		/// <param name="corrLevel">容错级别，从低到高依次为：L、M、Q、H</param>
		/// <param name="wh"></param>
		/// <param name="color"></param>
		/// <param name="bgcolor"></param>
		/// <returns></returns>
		// Token: 0x06000423 RID: 1059 RVA: 0x00021104 File Offset: 0x0001F304
		public static Image Encode(string content, string corrLevel, int wh, string color, string bgcolor)
		{
			ErrorCorrectionLevel corrLevel2 = ErrorCorrectionLevel.L;
			if (corrLevel != null)
			{
				string a;
				if ((a = corrLevel.ToLower()) != null)
				{
					if (a == "l")
					{
						corrLevel2 = ErrorCorrectionLevel.L;
						goto IL_6F;
					}
					if (a == "m")
					{
						corrLevel2 = ErrorCorrectionLevel.M;
						goto IL_6F;
					}
					if (a == "q")
					{
						corrLevel2 = ErrorCorrectionLevel.Q;
						goto IL_6F;
					}
					if (a == "h")
					{
						corrLevel2 = ErrorCorrectionLevel.H;
						goto IL_6F;
					}
				}
				corrLevel2 = ErrorCorrectionLevel.L;
			}
			IL_6F:
			ByteMatrix matrix = new QRCodeWriter().encode(content, BarcodeFormat.QR_CODE, corrLevel2, wh, wh, null);
			return QrcodeHepler.toBitmap(matrix, color, bgcolor);
		}

		/// <summary>
		/// 生成带中心图片的二维码，默认是高容错图像
		/// </summary>
		/// <param name="content">二维码内容</param>
		/// <param name="wh">宽高</param>
		/// <param name="centerImgPath">二维码中心图片的路径</param>
		/// <param name="color">前景色</param>
		/// <param name="bgcolor">背景色</param>
		/// <returns></returns>
		// Token: 0x06000424 RID: 1060 RVA: 0x000211A0 File Offset: 0x0001F3A0
		public static Image Encode(string content, int wh, string centerImgPath, string color, string bgcolor)
		{
			if (centerImgPath != null && File.Exists(centerImgPath))
			{
				Image centerImg = Image.FromFile(centerImgPath);
				return QrcodeHepler.Encode(content, wh, centerImg, color, bgcolor);
			}
			return QrcodeHepler.Encode(content, wh, color, bgcolor);
		}

		/// <summary>
		/// 批量生成带中心图片的二维码，默认是高容错图像
		/// </summary>
		/// <param name="content"></param>
		/// <param name="wh"></param>
		/// <param name="centerImgPath"></param>
		/// <param name="color"></param>
		/// <param name="bgcolor"></param>
		/// <returns></returns>
		// Token: 0x06000425 RID: 1061 RVA: 0x000211D8 File Offset: 0x0001F3D8
		public static Image[] Encode(string[] content, int wh, string centerImgPath, string color, string bgcolor)
		{
			if (centerImgPath != null && File.Exists(centerImgPath))
			{
				Image centerImg = Image.FromFile(centerImgPath);
				return QrcodeHepler.Encode(content, wh, centerImg, color, bgcolor);
			}
			return QrcodeHepler.Encode(content, wh, color, bgcolor);
		}

		/// <summary>
		/// 生成带中心图片的二维码，默认是高容错图像
		/// </summary>
		/// <param name="content">二维码内容</param>
		/// <param name="wh">宽高</param>
		/// <param name="centerImg">二维码中心图片对象</param>
		/// <param name="color">前景色</param>
		/// <param name="bgcolor">背景色</param>
		/// <returns></returns>
		// Token: 0x06000426 RID: 1062 RVA: 0x00021210 File Offset: 0x0001F410
		public static Image Encode(string content, int wh, Image centerImg, string color, string bgcolor)
		{
			ByteMatrix matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, wh, wh, null);
			Image image = QrcodeHepler.toBitmap(matrix, color, bgcolor);
			int width = (centerImg.Width > image.Width / 3) ? (image.Width / 3) : centerImg.Width;
			int height = (centerImg.Height > image.Height / 3) ? (image.Height / 3) : centerImg.Height;
			centerImg = ImageTo.Zoom(centerImg, width, height, false);
			centerImg = ImageTo.Rounded(centerImg);
			using (Graphics graphics = Graphics.FromImage(image))
			{
				graphics.InterpolationMode = InterpolationMode.High;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(centerImg, new Rectangle((image.Width - centerImg.Width) / 2, (image.Height - centerImg.Height) / 2, centerImg.Width, centerImg.Height), 0, 0, centerImg.Width, centerImg.Height, GraphicsUnit.Pixel);
				graphics.Dispose();
			}
			return image;
		}

		/// <summary>
		/// 批量生成带中心图片的二维码，默认是高容错图像
		/// </summary>
		/// <param name="content"></param>
		/// <param name="wh"></param>
		/// <param name="centerImg"></param>
		/// <param name="color"></param>
		/// <param name="bgcolor"></param>
		/// <returns></returns>
		// Token: 0x06000427 RID: 1063 RVA: 0x00021314 File Offset: 0x0001F514
		public static Image[] Encode(string[] content, int wh, Image centerImg, string color, string bgcolor)
		{
			List<Image> list = new List<Image>();
			int num = 0;
			for (int i = 0; i < content.Length; i++)
			{
				if (string.IsNullOrWhiteSpace(content[i]))
				{
					list.Add(null);
				}
				else
				{
					ByteMatrix matrix = new MultiFormatWriter().encode(content[i], BarcodeFormat.QR_CODE, wh, wh, null);
					Image image = QrcodeHepler.toBitmap(matrix, color, bgcolor);
					if (num <= 0)
					{
						num = ((centerImg.Width > image.Width / 3) ? (image.Width / 3) : centerImg.Width);
						int height = (centerImg.Height > image.Height / 3) ? (image.Height / 3) : centerImg.Height;
						centerImg = ImageTo.Zoom(centerImg, num, height, false);
						centerImg = ImageTo.Rounded(centerImg);
					}
					using (Graphics graphics = Graphics.FromImage(image))
					{
						graphics.InterpolationMode = InterpolationMode.High;
						graphics.SmoothingMode = SmoothingMode.HighQuality;
						graphics.DrawImage(centerImg, new Rectangle((image.Width - centerImg.Width) / 2, (image.Height - centerImg.Height) / 2, centerImg.Width, centerImg.Height), 0, 0, centerImg.Width, centerImg.Height, GraphicsUnit.Pixel);
						graphics.Dispose();
					}
					list.Add(image);
				}
			}
			return list.ToArray();
		}

		/// <summary>
		/// 生成图片对象
		/// </summary>
		/// <param name="matrix"></param>
		/// <param name="color">前景色</param>
		/// <param name="bgcolor">背景色</param>
		/// <returns></returns>
		// Token: 0x06000428 RID: 1064 RVA: 0x0002146C File Offset: 0x0001F66C
		private static Bitmap toBitmap(ByteMatrix matrix, string color, string bgcolor)
		{
			int width = matrix.Width;
			int height = matrix.Height;
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			Color color2 = (color == null || color == "") ? ColorTranslator.FromHtml("#000000") : ColorTranslator.FromHtml(color);
			Color color3 = (color == null || color == "") ? Color.Transparent : ColorTranslator.FromHtml(bgcolor);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					bitmap.SetPixel(i, j, (matrix.get_Renamed(i, j) != -1) ? color2 : color3);
				}
			}
			return bitmap;
		}

		/// <summary>
		/// 读取二维码信息
		/// </summary>
		/// <param name="img">图片对象</param>
		/// <returns></returns>
		// Token: 0x06000429 RID: 1065 RVA: 0x00021514 File Offset: 0x0001F714
		public static string Decode(Image img)
		{
			MultiFormatReader multiFormatReader = new MultiFormatReader();
			if (img == null)
			{
				return null;
			}
			Bitmap bitmap = (Bitmap)img;
			LuminanceSource source = new RGBLuminanceSource(bitmap, img.Width, img.Height);
			BinaryBitmap image = new BinaryBitmap(new HybridBinarizer(source));
			Result result = multiFormatReader.decode(image);
			bitmap.Dispose();
			return result.Text;
		}

		/// <summary>
		/// 读取二维码信息
		/// </summary>
		/// <param name="imgFilePath">图片所在的物理路径</param>
		/// <returns></returns>
		// Token: 0x0600042A RID: 1066 RVA: 0x00021568 File Offset: 0x0001F768
		public static string Decode(string imgFilePath)
		{
			if (!File.Exists(imgFilePath))
			{
				return null;
			}
			Image img = Image.FromFile(imgFilePath);
			return QrcodeHepler.Decode(img);
		}
	}
}
