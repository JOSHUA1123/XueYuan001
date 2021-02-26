using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Common.Images
{
	// Token: 0x02000021 RID: 33
	public class ImageTo
	{
		/// <summary>
		/// 剪切图片
		/// </summary>
		/// <param name="source">原始图片对象</param>
		/// <param name="x">起始X坐标</param>
		/// <param name="y">起始Y坐标</param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns>返回处理后的图片对象</returns>
		// Token: 0x060000B1 RID: 177 RVA: 0x0000ED14 File Offset: 0x0000CF14
		public static Image Cute(Image source, int x, int y, int width, int height)
		{
			int width2 = source.Width;
			int height2 = source.Height;
			width = ((width >= width2) ? width2 : width);
			height = ((height >= height2) ? height2 : height);
			Bitmap bitmap = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.DrawImage(source, 0, 0, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
				graphics.Dispose();
			}
			return bitmap;
		}

		/// <summary>
		/// 图片缩放，强制缩放到指定尺寸
		/// </summary>
		/// <param name="source"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="isDeformation">是否允许变形，如果不允许，则会剪切图形；</param>
		/// <returns></returns>
		// Token: 0x060000B2 RID: 178 RVA: 0x0000ED8C File Offset: 0x0000CF8C
		public static Image Zoom(Image source, int width, int height, bool isDeformation)
		{
			if (isDeformation)
			{
				return ImageTo.Zoom(source, width, height);
			}
			double num = (double)width / (double)source.Width;
			double num2 = (double)height / (double)source.Height;
			double num3 = (num > num2) ? num : num2;
			int num4 = Convert.ToInt32((double)source.Width * num3);
			int num5 = Convert.ToInt32((double)source.Height * num3);
			num4 = ((num4 >= source.Width) ? source.Width : num4);
			num5 = ((num5 >= source.Height) ? source.Height : num5);
			Bitmap bitmap = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.White);
				graphics.DrawImage(source, (width - num4) / 2, (height - num5) / 2, num4, num5);
				graphics.Dispose();
			}
			return bitmap;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000EE68 File Offset: 0x0000D068
		public static Image Zoom(Image source, int width, int height)
		{
			Bitmap bitmap = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.High;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(source, 0, 0, width, height);
				graphics.Dispose();
			}
			return bitmap;
		}

		/// <summary>
		/// 图片缩放，按比例
		/// </summary>
		/// <param name="source"></param>
		/// <param name="percent">原图的尺寸百分比</param>
		/// <returns></returns>
		// Token: 0x060000B4 RID: 180 RVA: 0x0000EEC0 File Offset: 0x0000D0C0
		public static Image Zoom(Image source, int percent)
		{
			int width = source.Width * percent / 100;
			int height = source.Height * percent / 100;
			return ImageTo.Zoom(source, width, height);
		}

		/// <summary>
		/// 生成缩略图，非变形生成
		/// </summary>
		/// <param name="source">原始图片对象</param>
		/// <param name="width">缩略图宽</param>
		/// <param name="height">缩略高</param>
		/// <returns></returns>
		// Token: 0x060000B5 RID: 181 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		public static Image Thumbnail(Image source, int width, int height)
		{
			int num = source.Width;
			int num2 = source.Height;
			double num3 = (double)width / (double)source.Width;
			double num4 = (double)height / (double)source.Height;
			double num5 = (num3 > num4) ? num3 : num4;
			num = Convert.ToInt32((double)source.Width * num5);
			num2 = Convert.ToInt32((double)source.Height * num5);
			num = ((num >= source.Width) ? source.Width : num);
			num2 = ((num2 >= source.Height) ? source.Height : num2);
			Bitmap bitmap = new Bitmap(num, num2);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.High;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(source, 0, 0, num, num2);
				graphics.Dispose();
			}
			return ImageTo.Cute(bitmap, (bitmap.Width - width) / 2, (bitmap.Height - height) / 2, width, height);
		}

		/// <summary>
		/// 按百分比生成缩略图
		/// </summary>
		/// <param name="source">原始图片对象</param>
		/// <param name="percent">原图的百分之多少</param>
		/// <returns></returns>
		// Token: 0x060000B6 RID: 182 RVA: 0x0000EFE4 File Offset: 0x0000D1E4
		public static Image Thumbnail(Image source, int percent)
		{
			int width = source.Width * percent / 100;
			int height = source.Height * percent / 100;
			return ImageTo.Thumbnail(source, width, height);
		}

		/// <summary>
		/// 非变形的生成缩略图，根据宽或高缩放，保持图片缩放时不变形
		/// </summary>
		/// <param name="source">原始图片对象</param>
		/// <param name="width">缩略图宽</param>
		/// <param name="height">缩略高</param>
		/// <param name="restrainObj">约束对象，1为度，2为高，0为自适应</param>
		/// <returns></returns>
		// Token: 0x060000B7 RID: 183 RVA: 0x0000F014 File Offset: 0x0000D214
		public static Image Thumbnail(Image source, int width, int height, int restrainObj)
		{
			int num = width;
			int num2 = height;
			if (restrainObj == 0)
			{
				double num3 = (double)width / (double)source.Width;
				double num4 = (double)height / (double)source.Height;
				double num5 = (num3 > num4) ? num3 : num4;
				num = Convert.ToInt32((double)source.Width * num5);
				num2 = Convert.ToInt32((double)source.Height * num5);
			}
			if (restrainObj == 1)
			{
				double num6 = (double)width / (double)source.Width;
				num2 = Convert.ToInt32((double)source.Height * num6);
			}
			if (restrainObj == 2)
			{
				double num7 = (double)height / (double)source.Height;
				num = Convert.ToInt32((double)source.Width * num7);
			}
			num = ((num >= source.Width) ? source.Width : num);
			num2 = ((num2 >= source.Height) ? source.Height : num2);
			Bitmap bitmap = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.White);
				graphics.DrawImage(source, (width - num) / 2, (height - num2) / 2, num, num2);
				graphics.Dispose();
			}
			return bitmap;
		}

		/// <summary>
		/// 图片叠加
		/// </summary>
		/// <param name="source">原图片对象</param>
		/// <param name="img">叠加的图片对象</param>
		/// <param name="x">起始X坐标</param>
		/// <param name="y">起始Y坐标</param>
		/// <returns></returns>
		// Token: 0x060000B8 RID: 184 RVA: 0x0000F128 File Offset: 0x0000D328
		public static Image Overlay(Image source, Image img, int x, int y)
		{
			using (Graphics graphics = Graphics.FromImage(source))
			{
				graphics.DrawImage(img, new Rectangle(x, y, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
				graphics.Dispose();
			}
			return source;
		}

		/// <summary>
		/// 图片叠加，可以设置透明度，即水印图片添加
		/// </summary>
		/// <param name="source"></param>
		/// <param name="img">叠加的图片对象</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="opacity">透明度，0为完全透明，100为不透明</param>
		/// <returns></returns>
		// Token: 0x060000B9 RID: 185 RVA: 0x000095A5 File Offset: 0x000077A5
		public static Image Overlay(Image source, Image img, int x, int y, int opacity)
		{
			img = ImageTo.Transparent(img, opacity);
			return ImageTo.Overlay(source, img, x, y);
		}

		/// <summary>
		/// 图片叠加
		/// </summary>
		/// <param name="source">原图片对象</param>
		/// <param name="img">要叠加的图片对象</param>
		/// <param name="overType">叠加的类型，分为左left、右right、上top、下down、中cengter；另分为左上、左下、右上、右下;</param>
		/// <param name="opacity">透明度，0为完全透明，100为不透明</param>
		/// <returns></returns>
		// Token: 0x060000BA RID: 186 RVA: 0x0000F188 File Offset: 0x0000D388
		public static Image Overlay(Image source, Image img, string overType, int opacity)
		{
			string key;
			int x;
			int y;
			switch (key = overType.ToLower())
			{
			case "left":
				x = 0;
				y = (source.Height - img.Height) / 2;
				goto IL_1B0;
			case "right":
				x = source.Width - img.Width;
				y = (source.Height - img.Height) / 2;
				goto IL_1B0;
			case "top":
				x = (source.Width - img.Width) / 2;
				y = 0;
				goto IL_1B0;
			case "down":
				x = (source.Width - img.Width) / 2;
				y = source.Height - img.Height;
				goto IL_1B0;
			case "lefttop":
				x = 0;
				y = 0;
				goto IL_1B0;
			case "leftdown":
				x = 0;
				y = source.Height - img.Height;
				goto IL_1B0;
			case "righttop":
				x = source.Width - img.Width;
				y = 0;
				goto IL_1B0;
			case "rightdown":
				x = source.Width - img.Width;
				y = source.Height - img.Height;
				goto IL_1B0;
			}
			x = (source.Width - img.Width) / 2;
			y = (source.Height - img.Height) / 2;
			IL_1B0:
			return ImageTo.Overlay(source, img, x, y, opacity);
		}

		/// <summary>
		/// 叠加文字
		/// </summary>
		/// <param name="source">原图片对象</param>
		/// <param name="text">在叠加的字符串</param>
		/// <param name="overType">叠加的类型，分为左left、右right、上top、下down、中cengter；另分为左上、左下、右上、右下;</param>
		/// <param name="opacity">透明度，0为完全透明，100为不透明</param>
		/// <returns></returns>
		// Token: 0x060000BB RID: 187 RVA: 0x0000F350 File Offset: 0x0000D550
		public static Image Overlay(Image source, string text, string overType, int opacity)
		{
			Image img = ImageTo.FromString(text, 12, "宋体", "#000");
			return ImageTo.Overlay(source, img, overType, opacity);
		}

		/// <summary>
		/// 叠加文字
		/// </summary>
		/// <param name="source"></param>
		/// <param name="text"></param>
		/// <param name="overType">叠加的类型，分为左left、右right、上top、下down、中cengter；另分为左上、左下、右上、右下;</param>
		/// <param name="opacity">透明度，0为完全透明，100为不透明</param>
		/// <param name="size">字符大小</param>
		/// <param name="font">字体</param>
		/// <param name="color">文字颜色</param>
		/// <returns></returns>
		// Token: 0x060000BC RID: 188 RVA: 0x0000F37C File Offset: 0x0000D57C
		public static Image Overlay(Image source, string text, string overType, int opacity, int size, string font, string color)
		{
			Image img = ImageTo.FromString(text, size, font, color);
			return ImageTo.Overlay(source, img, overType, opacity);
		}

		/// <summary>
		/// 将文本创建成图片对象
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		// Token: 0x060000BD RID: 189 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		public static Image FromString(string text)
		{
			int width = text.Length * 18;
			Bitmap bitmap = new Bitmap(width, 18);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				Font font = new Font("宋体", 12f, FontStyle.Bold);
				Brush brush = new SolidBrush(Color.Black);
				graphics.DrawString(text, font, brush, 0f, 0f);
				graphics.Dispose();
			}
			return bitmap;
		}

		/// <summary>
		/// 将文本创建成图片对象
		/// </summary>
		/// <param name="text">要创建图像的字符串</param>
		/// <param name="size">字符的大小</param>
		/// <param name="font">字体</param>
		/// <param name="color">字符颜色</param>
		/// <returns></returns>
		// Token: 0x060000BE RID: 190 RVA: 0x0000F41C File Offset: 0x0000D61C
		public static Image FromString(string text, int size, string font, string color)
		{
			font = (string.IsNullOrWhiteSpace(font) ? "宋体" : font);
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			int num = 0;
			foreach (string text2 in array)
			{
				num = ((text2.Length > num) ? text2.Length : num);
			}
			int num2 = (int)((double)num * ((double)((float)size) * 1.5));
			num2 = ((num2 < 1) ? 1 : num2);
			Bitmap bitmap = new Bitmap(num2, (int)Math.Floor((double)(size * array.Length) * 1.5));
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.White);
				Font font2 = new Font(font, (float)size, FontStyle.Bold);
				Color color2 = ColorTranslator.FromHtml(color);
				Brush brush = new SolidBrush(color2);
				graphics.DrawString(text, font2, brush, 0f, 0f);
				graphics.Dispose();
			}
			return bitmap;
		}

		/// <summary>
		/// 设置图片透明度
		/// </summary>
		/// <param name="img">要操作的图片对象</param>
		/// <param name="opacity">透明度,0为完全透明，100为不透明</param>
		/// <returns></returns>
		// Token: 0x060000BF RID: 191 RVA: 0x0000F52C File Offset: 0x0000D72C
		public static Image Transparent(Image img, int opacity)
		{
			opacity = ((opacity < 0) ? 0 : opacity);
			opacity = ((opacity > 100) ? 100 : opacity);
			float[][] array = new float[5][];
			float[][] array2 = array;
			int num = 0;
			float[] array3 = new float[5];
			array3[0] = 1f;
			array2[num] = array3;
			float[][] array4 = array;
			int num2 = 1;
			float[] array5 = new float[5];
			array5[1] = 1f;
			array4[num2] = array5;
			float[][] array6 = array;
			int num3 = 2;
			float[] array7 = new float[5];
			array7[2] = 1f;
			array6[num3] = array7;
			float[][] array8 = array;
			int num4 = 3;
			float[] array9 = new float[5];
			array9[3] = (float)opacity / 100f;
			array8[num4] = array9;
			array[4] = new float[]
			{
				0f,
				0f,
				0f,
				0f,
				1f
			};
			float[][] newColorMatrix = array;
			ColorMatrix newColorMatrix2 = new ColorMatrix(newColorMatrix);
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			Bitmap bitmap = new Bitmap(img.Width, img.Height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttributes);
				graphics.Dispose();
			}
			return bitmap;
		}

		/// <summary>
		/// 给图片生成一个边框
		/// </summary>
		/// <param name="img"></param>
		/// <returns></returns>
		// Token: 0x060000C0 RID: 192 RVA: 0x0000F654 File Offset: 0x0000D854
		public static Image Border(Image img)
		{
			using (Graphics graphics = Graphics.FromImage(img))
			{
				graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, img.Width - 1, img.Height - 1);
				graphics.Dispose();
			}
			return img;
		}

		/// <summary>
		/// 图片文件转Base64
		/// </summary>
		/// <param name="img"></param>
		/// <returns></returns>
		// Token: 0x060000C1 RID: 193 RVA: 0x0000F6B4 File Offset: 0x0000D8B4
		public static string ToBase64(Image img)
		{
			string result = string.Empty;
			Image image = (Image)img.Clone();
			using (Bitmap bitmap = (Bitmap)image)
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

		/// <summary>
		/// Base64字符串转图片对象
		/// </summary>
		/// <param name="base64string"></param>
		/// <returns></returns>
		// Token: 0x060000C2 RID: 194 RVA: 0x0000F75C File Offset: 0x0000D95C
		public static Image FromBase64(string base64string)
		{
			byte[] buffer = Convert.FromBase64String(base64string);
			Bitmap result = null;
			using (MemoryStream memoryStream = new MemoryStream(buffer))
			{
				result = new Bitmap(memoryStream);
				memoryStream.Dispose();
			}
			return result;
		}

		/// <summary>
		/// 图片生成圆角
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		// Token: 0x060000C3 RID: 195 RVA: 0x000095BA File Offset: 0x000077BA
		public static Image Rounded(Image image)
		{
			return ImageTo.Rounded(image, string.Empty);
		}

		/// <summary>
		/// 图片生成圆角
		/// </summary>
		/// <param name="image"></param>
		/// <param name="cornerLocation">圆角位置，参数：TopLeft,TopRight,BottomLeft,BottomRight</param>
		/// <returns></returns>
		// Token: 0x060000C4 RID: 196 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		public static Image Rounded(Image image, string cornerLocation)
		{
			Graphics graphics = Graphics.FromImage(image);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
			GraphicsPath path = ImageTo.CreateRoundRectanglePath(rect, image.Width / 4, cornerLocation);
			return ImageTo.BitmapCrop((Bitmap)image, path);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000F800 File Offset: 0x0000DA00
		private static GraphicsPath CreateRoundRectanglePath(Rectangle rect, int radius, string sPosition)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			if (sPosition != null)
			{
				if (sPosition == "TopLeft")
				{
					graphicsPath.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180f, 90f);
					graphicsPath.AddLine(rect.Left, rect.Top, rect.Left, rect.Top + radius);
					return graphicsPath;
				}
				if (sPosition == "TopRight")
				{
					graphicsPath.AddArc(rect.Right - radius * 2, rect.Top, radius * 2, radius * 2, 270f, 90f);
					graphicsPath.AddLine(rect.Right, rect.Top, rect.Right - radius, rect.Top);
					return graphicsPath;
				}
				if (sPosition == "BottomLeft")
				{
					graphicsPath.AddArc(rect.Left, rect.Bottom - radius * 2, radius * 2, radius * 2, 90f, 90f);
					graphicsPath.AddLine(rect.Left, rect.Bottom - radius, rect.Left, rect.Bottom);
					return graphicsPath;
				}
				if (sPosition == "BottomRight")
				{
					graphicsPath.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0f, 90f);
					graphicsPath.AddLine(rect.Right - radius, rect.Bottom, rect.Right, rect.Bottom);
					return graphicsPath;
				}
			}
			graphicsPath.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180f, 90f);
			graphicsPath.AddLine(rect.X + radius, rect.Y, rect.Right - radius * 2, rect.Y);
			graphicsPath.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270f, 90f);
			graphicsPath.AddLine(rect.Right, rect.Y + radius * 2, rect.Right, rect.Y + rect.Height - radius * 2);
			graphicsPath.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0f, 90f);
			graphicsPath.AddLine(rect.Right - radius * 2, rect.Bottom, rect.X + radius * 2, rect.Bottom);
			graphicsPath.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90f, 90f);
			graphicsPath.AddLine(rect.X, rect.Bottom - radius * 2, rect.X, rect.Y + radius * 2);
			return graphicsPath;
		}

		/// <summary>
		/// 图片不规则截图
		/// </summary>
		/// <param name="bitmap">原图</param>
		/// <param name="path">裁剪路径</param>
		/// <returns></returns>
		// Token: 0x060000C6 RID: 198 RVA: 0x0000FB08 File Offset: 0x0000DD08
		public static Image BitmapCrop(Bitmap bitmap, GraphicsPath path)
		{
			RectangleF bounds = path.GetBounds();
			int num = (int)bounds.Left;
			int num2 = (int)bounds.Top;
			int num3 = (int)bounds.Width;
			int num4 = (int)bounds.Height;
			Bitmap bitmap2 = (Bitmap)bitmap.Clone();
			Bitmap bitmap3 = new Bitmap(num3, num4);
			for (int i = num; i < num + num3; i++)
			{
				for (int j = num2; j < num2 + num4; j++)
				{
					if (path.IsVisible(i, j))
					{
						bitmap3.SetPixel(i - num, j - num2, bitmap2.GetPixel(i, j));
						bitmap2.SetPixel(i, j, Color.FromArgb(0, bitmap2.GetPixel(i, j)));
					}
					else
					{
						bitmap3.SetPixel(i - num, j - num2, Color.FromArgb(0, 255, 255, 255));
					}
				}
			}
			bitmap.Dispose();
			return bitmap3;
		}
	}
}
