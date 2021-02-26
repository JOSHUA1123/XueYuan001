// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.GlobalHistogramBinarizer
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.common
{
  /// <summary>
  /// This Binarizer implementation uses the old ZXing global histogram approach. It is suitable
  ///             for low-end mobile devices which don't have enough CPU or memory to use a local thresholding
  ///             algorithm. However, because it picks a global black point, it cannot handle difficult shadows
  ///             and gradients.
  /// 
  ///             Faster mobile devices and all desktop applications should probably use HybridBinarizer instead.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public class GlobalHistogramBinarizer : Binarizer
  {
    private static readonly int LUMINANCE_SHIFT = 3;
    private static readonly int LUMINANCE_BUCKETS = 32;
    private const int LUMINANCE_BITS = 5;
    private sbyte[] luminances;
    private int[] buckets;

    public override BitMatrix BlackMatrix
    {
      get
      {
        LuminanceSource luminanceSource = this.LuminanceSource;
        int width = luminanceSource.Width;
        int height = luminanceSource.Height;
        BitMatrix bitMatrix = new BitMatrix(width, height);
        this.initArrays(width);
        int[] buckets = this.buckets;
        for (int index1 = 1; index1 < 5; ++index1)
        {
          int y = height * index1 / 5;
          sbyte[] row = luminanceSource.getRow(y, this.luminances);
          int num1 = (width << 2) / 5;
          for (int index2 = width / 5; index2 < num1; ++index2)
          {
            int num2 = (int) row[index2] & (int) byte.MaxValue;
            ++buckets[num2 >> GlobalHistogramBinarizer.LUMINANCE_SHIFT];
          }
        }
        int num3 = GlobalHistogramBinarizer.estimateBlackPoint(buckets);
        sbyte[] matrix = luminanceSource.Matrix;
        for (int y = 0; y < height; ++y)
        {
          int num1 = y * width;
          for (int x = 0; x < width; ++x)
          {
            if (((int) matrix[num1 + x] & (int) byte.MaxValue) < num3)
              bitMatrix.set_Renamed(x, y);
          }
        }
        return bitMatrix;
      }
    }

    public GlobalHistogramBinarizer(LuminanceSource source)
      : base(source)
    {
    }

    public override BitArray getBlackRow(int y, BitArray row)
    {
      LuminanceSource luminanceSource = this.LuminanceSource;
      int width = luminanceSource.Width;
      if (row == null || row.Size < width)
        row = new BitArray(width);
      else
        row.clear();
      this.initArrays(width);
      sbyte[] row1 = luminanceSource.getRow(y, this.luminances);
      int[] buckets = this.buckets;
      for (int index = 0; index < width; ++index)
      {
        int num = (int) row1[index] & (int) byte.MaxValue;
        ++buckets[num >> GlobalHistogramBinarizer.LUMINANCE_SHIFT];
      }
      int num1 = GlobalHistogramBinarizer.estimateBlackPoint(buckets);
      int num2 = (int) row1[0] & (int) byte.MaxValue;
      int num3 = (int) row1[1] & (int) byte.MaxValue;
      for (int i = 1; i < width - 1; ++i)
      {
        int num4 = (int) row1[i + 1] & (int) byte.MaxValue;
        if ((num3 << 2) - num2 - num4 >> 1 < num1)
          row.set_Renamed(i);
        num2 = num3;
        num3 = num4;
      }
      return row;
    }

    public override Binarizer createBinarizer(LuminanceSource source)
    {
      return (Binarizer) new GlobalHistogramBinarizer(source);
    }

    private void initArrays(int luminanceSize)
    {
      if (this.luminances == null || this.luminances.Length < luminanceSize)
        this.luminances = new sbyte[luminanceSize];
      if (this.buckets == null)
      {
        this.buckets = new int[GlobalHistogramBinarizer.LUMINANCE_BUCKETS];
      }
      else
      {
        for (int index = 0; index < GlobalHistogramBinarizer.LUMINANCE_BUCKETS; ++index)
          this.buckets[index] = 0;
      }
    }

    private static int estimateBlackPoint(int[] buckets)
    {
      int length = buckets.Length;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      for (int index = 0; index < length; ++index)
      {
        if (buckets[index] > num3)
        {
          num2 = index;
          num3 = buckets[index];
        }
        if (buckets[index] > num1)
          num1 = buckets[index];
      }
      int num4 = 0;
      int num5 = 0;
      for (int index = 0; index < length; ++index)
      {
        int num6 = index - num2;
        int num7 = buckets[index] * num6 * num6;
        if (num7 > num5)
        {
          num4 = index;
          num5 = num7;
        }
      }
      if (num2 > num4)
      {
        int num6 = num2;
        num2 = num4;
        num4 = num6;
      }
      if (num4 - num2 <= length >> 4)
        throw ReaderException.Instance;
      int num8 = num4 - 1;
      int num9 = -1;
      for (int index = num4 - 1; index > num2; --index)
      {
        int num6 = index - num2;
        int num7 = num6 * num6 * (num4 - index) * (num1 - buckets[index]);
        if (num7 > num9)
        {
          num8 = index;
          num9 = num7;
        }
      }
      return num8 << GlobalHistogramBinarizer.LUMINANCE_SHIFT;
    }
  }
}
