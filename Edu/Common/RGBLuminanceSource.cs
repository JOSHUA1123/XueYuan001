using System;
using System.Drawing;
using com.google.zxing;

// Token: 0x020000A0 RID: 160
public class RGBLuminanceSource : LuminanceSource
{
	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000AD8E File Offset: 0x00008F8E
	public override int Height
	{
		get
		{
			if (!this.isRotated)
			{
				return this.__height;
			}
			return this.__width;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x0000ADA5 File Offset: 0x00008FA5
	public override int Width
	{
		get
		{
			if (!this.isRotated)
			{
				return this.__width;
			}
			return this.__height;
		}
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00021658 File Offset: 0x0001F858
	public RGBLuminanceSource(byte[] d, int W, int H) : base(W, H)
	{
		this.__width = W;
		this.__height = H;
		this.luminances = new sbyte[W * H];
		for (int i = 0; i < H; i++)
		{
			int num = i * W;
			for (int j = 0; j < W; j++)
			{
				int num2 = (int)d[num * 3 + j * 3];
				int num3 = (int)d[num * 3 + j * 3 + 1];
				int num4 = (int)d[num * 3 + j * 3 + 2];
				if (num2 == num3 && num3 == num4)
				{
					this.luminances[num + j] = (sbyte)num2;
				}
				else
				{
					this.luminances[num + j] = (sbyte)(num2 + num3 + num3 + num4 >> 2);
				}
			}
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0000ADBC File Offset: 0x00008FBC
	public RGBLuminanceSource(byte[] d, int W, int H, bool Is8Bit) : base(W, H)
	{
		this.__width = W;
		this.__height = H;
		this.luminances = new sbyte[W * H];
		Buffer.BlockCopy(d, 0, this.luminances, 0, W * H);
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0000ADF3 File Offset: 0x00008FF3
	public RGBLuminanceSource(byte[] d, int W, int H, bool Is8Bit, Rectangle Region) : base(W, H)
	{
		this.__width = Region.Width;
		this.__height = Region.Height;
		this.__Region = Region;
		this.__isRegionSelect = true;
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0002170C File Offset: 0x0001F90C
	public RGBLuminanceSource(Bitmap d, int W, int H) : base(W, H)
	{
		this.__width = W;
		this.__height = H;
		this.luminances = new sbyte[W * H];
		for (int i = 0; i < H; i++)
		{
			int num = i * W;
			for (int j = 0; j < W; j++)
			{
				Color pixel = d.GetPixel(j, i);
				this.luminances[num + j] = (sbyte)((int)pixel.R << 16 | (int)pixel.G << 8 | (int)pixel.B);
			}
		}
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x000217A0 File Offset: 0x0001F9A0
	public override sbyte[] getRow(int y, sbyte[] row)
	{
		if (!this.isRotated)
		{
			int width = this.Width;
			if (row == null || row.Length < width)
			{
				row = new sbyte[width];
			}
			for (int i = 0; i < width; i++)
			{
				row[i] = this.luminances[y * width + i];
			}
			return row;
		}
		int _width = this.__width;
		int _height = this.__height;
		if (row == null || row.Length < _height)
		{
			row = new sbyte[_height];
		}
		for (int j = 0; j < _height; j++)
		{
			row[j] = this.luminances[j * _width + y];
		}
		return row;
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000AE26 File Offset: 0x00009026
	public override sbyte[] Matrix
	{
		get
		{
			return this.luminances;
		}
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0000AE2E File Offset: 0x0000902E
	public override LuminanceSource crop(int left, int top, int width, int height)
	{
		return base.crop(left, top, width, height);
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0000AE3B File Offset: 0x0000903B
	public override LuminanceSource rotateCounterClockwise()
	{
		this.isRotated = true;
		return this;
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x0600044E RID: 1102 RVA: 0x0000AE45 File Offset: 0x00009045
	public override bool RotateSupported
	{
		get
		{
			return true;
		}
	}

	// Token: 0x040001A9 RID: 425
	private sbyte[] luminances;

	// Token: 0x040001AA RID: 426
	private bool isRotated;

	// Token: 0x040001AB RID: 427
	private bool __isRegionSelect;

	// Token: 0x040001AC RID: 428
	private Rectangle __Region;

	// Token: 0x040001AD RID: 429
	private int __height;

	// Token: 0x040001AE RID: 430
	private int __width;
}
