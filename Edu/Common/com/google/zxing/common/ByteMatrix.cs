// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.ByteMatrix
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace com.google.zxing.common
{
  public sealed class ByteMatrix
  {
    private sbyte[][] bytes;
    private int width;
    private int height;

    public int Height
    {
      get
      {
        return this.height;
      }
    }

    public int Width
    {
      get
      {
        return this.width;
      }
    }

    public sbyte[][] Array
    {
      get
      {
        return this.bytes;
      }
    }

    public ByteMatrix(int width, int height)
    {
      this.bytes = new sbyte[height][];
      for (int index = 0; index < height; ++index)
        this.bytes[index] = new sbyte[width];
      this.width = width;
      this.height = height;
    }

    public sbyte get_Renamed(int x, int y)
    {
      return this.bytes[y][x];
    }

    public void set_Renamed(int x, int y, sbyte value_Renamed)
    {
      this.bytes[y][x] = value_Renamed;
    }

    public void set_Renamed(int x, int y, int value_Renamed)
    {
      this.bytes[y][x] = (sbyte) value_Renamed;
    }

    public void clear(sbyte value_Renamed)
    {
      for (int index1 = 0; index1 < this.height; ++index1)
      {
        for (int index2 = 0; index2 < this.width; ++index2)
          this.bytes[index1][index2] = value_Renamed;
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(2 * this.width * this.height + 2);
      for (int index1 = 0; index1 < this.height; ++index1)
      {
        for (int index2 = 0; index2 < this.width; ++index2)
        {
          switch (this.bytes[index1][index2])
          {
            case (sbyte) 0:
              stringBuilder.Append(" 0");
              break;
            case (sbyte) 1:
              stringBuilder.Append(" 1");
              break;
            default:
              stringBuilder.Append("  ");
              break;
          }
        }
        stringBuilder.Append('\n');
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Converts this ByteMatrix to a black and white bitmap.
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// A black and white bitmap converted from this ByteMatrix.
    /// </returns>
    public Bitmap ToBitmap()
    {
      sbyte[][] array = this.Array;
      int width = this.Width;
      int height = this.Height;
      Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
      byte[] source = new byte[bitmapdata.Stride * height];
      int num = 0;
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
          source[num++] = (int) array[index1][index2] == 0 ? (byte) 0 : byte.MaxValue;
        num += bitmapdata.Stride - width;
      }
      Marshal.Copy(source, 0, bitmapdata.Scan0, source.Length);
      bitmap.UnlockBits(bitmapdata);
      return bitmap;
    }
  }
}
