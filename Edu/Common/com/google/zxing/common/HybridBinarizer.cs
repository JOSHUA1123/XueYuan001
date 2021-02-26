// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.HybridBinarizer
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.common
{
  /// <summary>
  /// This class implements a local thresholding algorithm, which while slower than the
  ///             GlobalHistogramBinarizer, is fairly efficient for what it does. It is designed for
  ///             high frequency images of barcodes with black data on white backgrounds. For this application,
  ///             it does a much better job than a global blackpoint with severe shadows and gradients.
  ///             However it tends to produce artifacts on lower frequency images and is therefore not
  ///             a good general purpose binarizer for uses outside ZXing.
  /// 
  ///             This class extends GlobalHistogramBinarizer, using the older histogram approach for 1D readers,
  ///             and the newer local approach for 2D readers. 1D decoding using a per-row histogram is already
  ///             inherently local, and only fails for horizontal gradients. We can revisit that problem later,
  ///             but for now it was not a win to use local blocks for 1D.
  /// 
  ///             This Binarizer is the default for the unit tests and the recommended class for library users.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class HybridBinarizer : GlobalHistogramBinarizer
  {
    private const int MINIMUM_DIMENSION = 40;
    private BitMatrix matrix;

    public override BitMatrix BlackMatrix
    {
      get
      {
        this.binarizeEntireImage();
        return this.matrix;
      }
    }

    public HybridBinarizer(LuminanceSource source)
      : base(source)
    {
    }

    public override Binarizer createBinarizer(LuminanceSource source)
    {
      return (Binarizer) new HybridBinarizer(source);
    }

    private void binarizeEntireImage()
    {
      if (this.matrix != null)
        return;
      LuminanceSource luminanceSource = this.LuminanceSource;
      if (luminanceSource.Width >= 40 && luminanceSource.Height >= 40)
      {
        sbyte[] matrix = luminanceSource.Matrix;
        int width = luminanceSource.Width;
        int height = luminanceSource.Height;
        int subWidth = width >> 3;
        int subHeight = height >> 3;
        int[][] blackPoints = HybridBinarizer.calculateBlackPoints(matrix, subWidth, subHeight, width);
        this.matrix = new BitMatrix(width, height);
        HybridBinarizer.calculateThresholdForBlock(matrix, subWidth, subHeight, width, blackPoints, this.matrix);
      }
      else
        this.matrix = base.BlackMatrix;
    }

    private static void calculateThresholdForBlock(sbyte[] luminances, int subWidth, int subHeight, int stride, int[][] blackPoints, BitMatrix matrix)
    {
      for (int index1 = 0; index1 < subHeight; ++index1)
      {
        for (int index2 = 0; index2 < subWidth; ++index2)
        {
          int num1 = index2 > 1 ? index2 : 2;
          int index3 = num1 < subWidth - 2 ? num1 : subWidth - 3;
          int num2 = index1 > 1 ? index1 : 2;
          int num3 = num2 < subHeight - 2 ? num2 : subHeight - 3;
          int num4 = 0;
          for (int index4 = -2; index4 <= 2; ++index4)
          {
            int[] numArray = blackPoints[num3 + index4];
            num4 = num4 + numArray[index3 - 2] + numArray[index3 - 1] + numArray[index3] + numArray[index3 + 1] + numArray[index3 + 2];
          }
          int threshold = num4 / 25;
          HybridBinarizer.threshold8x8Block(luminances, index2 << 3, index1 << 3, threshold, stride, matrix);
        }
      }
    }

    private static void threshold8x8Block(sbyte[] luminances, int xoffset, int yoffset, int threshold, int stride, BitMatrix matrix)
    {
      for (int index1 = 0; index1 < 8; ++index1)
      {
        int num = (yoffset + index1) * stride + xoffset;
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (((int) luminances[num + index2] & (int) byte.MaxValue) < threshold)
            matrix.set_Renamed(xoffset + index2, yoffset + index1);
        }
      }
    }

    private static int[][] calculateBlackPoints(sbyte[] luminances, int subWidth, int subHeight, int stride)
    {
      int[][] numArray = new int[subHeight][];
      for (int index = 0; index < subHeight; ++index)
        numArray[index] = new int[subWidth];
      for (int index1 = 0; index1 < subHeight; ++index1)
      {
        for (int index2 = 0; index2 < subWidth; ++index2)
        {
          int num1 = 0;
          int num2 = (int) byte.MaxValue;
          int num3 = 0;
          for (int index3 = 0; index3 < 8; ++index3)
          {
            int num4 = ((index1 << 3) + index3) * stride + (index2 << 3);
            for (int index4 = 0; index4 < 8; ++index4)
            {
              int num5 = (int) luminances[num4 + index4] & (int) byte.MaxValue;
              num1 += num5;
              if (num5 < num2)
                num2 = num5;
              if (num5 > num3)
                num3 = num5;
            }
          }
          int num6 = num3 - num2 > 24 ? num1 >> 6 : num2 >> 1;
          numArray[index1][index2] = num6;
        }
      }
      return numArray;
    }
  }
}
