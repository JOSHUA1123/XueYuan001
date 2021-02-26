// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.detector.Detector
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System;
using System.Collections;

namespace com.google.zxing.qrcode.detector
{
  /// <summary>
  /// <p>Encapsulates logic that can detect a QR Code in an image, even if the QR Code
  ///             is rotated or skewed, or partially obscured.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public class Detector
  {
    private BitMatrix image;
    private ResultPointCallback resultPointCallback;

    protected internal virtual BitMatrix Image
    {
      get
      {
        return this.image;
      }
    }

    protected internal virtual ResultPointCallback ResultPointCallback
    {
      get
      {
        return this.resultPointCallback;
      }
    }

    public Detector(BitMatrix image)
    {
      this.image = image;
    }

    /// <summary>
    /// <p>Detects a QR Code in an image, simply.</p>
    /// </summary>
    /// 
    /// <returns>
    /// {@link DetectorResult} encapsulating results of detecting a QR Code
    /// 
    /// </returns>
    /// <throws>ReaderException if no QR Code can be found </throws>
    public virtual DetectorResult detect()
    {
      return this.detect((Hashtable) null);
    }

    /// <summary>
    /// <p>Detects a QR Code in an image, simply.</p>
    /// </summary>
    /// <param name="hints">optional hints to detector
    ///             </param>
    /// <returns>
    /// {@link DetectorResult} encapsulating results of detecting a QR Code
    /// 
    /// </returns>
    /// <throws>ReaderException if no QR Code can be found </throws>
    public virtual DetectorResult detect(Hashtable hints)
    {
      this.resultPointCallback = hints == null ? (ResultPointCallback) null : (ResultPointCallback) hints[(object) DecodeHintType.NEED_RESULT_POINT_CALLBACK];
      return this.processFinderPatternInfo(new FinderPatternFinder(this.image, this.resultPointCallback).find(hints));
    }

    protected internal virtual DetectorResult processFinderPatternInfo(FinderPatternInfo info)
    {
      FinderPattern topLeft = info.TopLeft;
      FinderPattern topRight = info.TopRight;
      FinderPattern bottomLeft = info.BottomLeft;
      float num1 = this.calculateModuleSize((ResultPoint) topLeft, (ResultPoint) topRight, (ResultPoint) bottomLeft);
      if ((double) num1 < 1.0)
        throw ReaderException.Instance;
      int dimension = Detector.computeDimension((ResultPoint) topLeft, (ResultPoint) topRight, (ResultPoint) bottomLeft, num1);
      com.google.zxing.qrcode.decoder.Version versionForDimension = com.google.zxing.qrcode.decoder.Version.getProvisionalVersionForDimension(dimension);
      int num2 = versionForDimension.DimensionForVersion - 7;
      AlignmentPattern alignmentPattern = (AlignmentPattern) null;
      if (versionForDimension.AlignmentPatternCenters.Length > 0)
      {
        float num3 = topRight.X - topLeft.X + bottomLeft.X;
        float num4 = topRight.Y - topLeft.Y + bottomLeft.Y;
        float num5 = (float) (1.0 - 3.0 / (double) num2);
        int estAlignmentX = (int) ((double) topLeft.X + (double) num5 * ((double) num3 - (double) topLeft.X));
        int estAlignmentY = (int) ((double) topLeft.Y + (double) num5 * ((double) num4 - (double) topLeft.Y));
        int num6 = 4;
        while (num6 <= 16)
        {
          try
          {
            alignmentPattern = this.findAlignmentInRegion(num1, estAlignmentX, estAlignmentY, (float) num6);
            break;
          }
          catch (ReaderException ex)
          {
          }
          num6 <<= 1;
        }
      }
      BitMatrix bits = Detector.sampleGrid(this.image, this.createTransform((ResultPoint) topLeft, (ResultPoint) topRight, (ResultPoint) bottomLeft, (ResultPoint) alignmentPattern, dimension), dimension);
      ResultPoint[] points;
      if (alignmentPattern == null)
        points = new ResultPoint[3]
        {
          (ResultPoint) bottomLeft,
          (ResultPoint) topLeft,
          (ResultPoint) topRight
        };
      else
        points = new ResultPoint[4]
        {
          (ResultPoint) bottomLeft,
          (ResultPoint) topLeft,
          (ResultPoint) topRight,
          (ResultPoint) alignmentPattern
        };
      return new DetectorResult(bits, points);
    }

    public virtual PerspectiveTransform createTransform(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft, ResultPoint alignmentPattern, int dimension)
    {
      float num = (float) dimension - 3.5f;
      float x2p;
      float y2p;
      float y2;
      float x2;
      if (alignmentPattern != null)
      {
        x2p = alignmentPattern.X;
        y2p = alignmentPattern.Y;
        x2 = y2 = num - 3f;
      }
      else
      {
        x2p = topRight.X - topLeft.X + bottomLeft.X;
        y2p = topRight.Y - topLeft.Y + bottomLeft.Y;
        x2 = y2 = num;
      }
      return PerspectiveTransform.quadrilateralToQuadrilateral(3.5f, 3.5f, num, 3.5f, x2, y2, 3.5f, num, topLeft.X, topLeft.Y, topRight.X, topRight.Y, x2p, y2p, bottomLeft.X, bottomLeft.Y);
    }

    private static BitMatrix sampleGrid(BitMatrix image, PerspectiveTransform transform, int dimension)
    {
      return GridSampler.Instance.sampleGrid(image, dimension, transform);
    }

    /// <summary>
    /// <p>Computes the dimension (number of modules on a size) of the QR Code based on the position
    ///             of the finder patterns and estimated module size.</p>
    /// </summary>
    protected internal static int computeDimension(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft, float moduleSize)
    {
      int num = (Detector.round(ResultPoint.distance(topLeft, topRight) / moduleSize) + Detector.round(ResultPoint.distance(topLeft, bottomLeft) / moduleSize) >> 1) + 7;
      switch (num & 3)
      {
        case 0:
          ++num;
          break;
        case 2:
          --num;
          break;
        case 3:
          throw ReaderException.Instance;
      }
      return num;
    }

    /// <summary>
    /// <p>Computes an average estimated module size based on estimated derived from the positions
    ///             of the three finder patterns.</p>
    /// </summary>
    protected internal virtual float calculateModuleSize(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft)
    {
      return (float) (((double) this.calculateModuleSizeOneWay(topLeft, topRight) + (double) this.calculateModuleSizeOneWay(topLeft, bottomLeft)) / 2.0);
    }

    /// <summary>
    /// <p>Estimates module size based on two finder patterns -- it uses
    ///             {@link #sizeOfBlackWhiteBlackRunBothWays(int, int, int, int)} to figure the
    ///             width of each, measuring along the axis between their centers.</p>
    /// </summary>
    private float calculateModuleSizeOneWay(ResultPoint pattern, ResultPoint otherPattern)
    {
      float f1 = this.sizeOfBlackWhiteBlackRunBothWays((int) pattern.X, (int) pattern.Y, (int) otherPattern.X, (int) otherPattern.Y);
      float f2 = this.sizeOfBlackWhiteBlackRunBothWays((int) otherPattern.X, (int) otherPattern.Y, (int) pattern.X, (int) pattern.Y);
      if (float.IsNaN(f1))
        return f2 / 7f;
      if (float.IsNaN(f2))
        return f1 / 7f;
      return (float) (((double) f1 + (double) f2) / 14.0);
    }

    private float sizeOfBlackWhiteBlackRunBothWays(int fromX, int fromY, int toX, int toY)
    {
      float num1 = this.sizeOfBlackWhiteBlackRun(fromX, fromY, toX, toY);
      float num2 = 1f;
      int num3 = fromX - (toX - fromX);
      if (num3 < 0)
      {
        num2 = (float) fromX / (float) (fromX - num3);
        num3 = 0;
      }
      else if (num3 >= this.image.Width)
      {
        num2 = (float) (this.image.Width - 1 - fromX) / (float) (num3 - fromX);
        num3 = this.image.Width - 1;
      }
      int toY1 = (int) ((double) fromY - (double) (toY - fromY) * (double) num2);
      float num4 = 1f;
      if (toY1 < 0)
      {
        num4 = (float) fromY / (float) (fromY - toY1);
        toY1 = 0;
      }
      else if (toY1 >= this.image.Height)
      {
        num4 = (float) (this.image.Height - 1 - fromY) / (float) (toY1 - fromY);
        toY1 = this.image.Height - 1;
      }
      int toX1 = (int) ((double) fromX + (double) (num3 - fromX) * (double) num4);
      return num1 + this.sizeOfBlackWhiteBlackRun(fromX, fromY, toX1, toY1) - 1f;
    }

    /// <summary>
    /// <p>This method traces a line from a point in the image, in the direction towards another point.
    ///             It begins in a black region, and keeps going until it finds white, then black, then white again.
    ///             It reports the distance from the start to this point.</p><p>This is used when figuring out how wide a finder pattern is, when the finder pattern
    ///             may be skewed or rotated.</p>
    /// </summary>
    private float sizeOfBlackWhiteBlackRun(int fromX, int fromY, int toX, int toY)
    {
      bool flag = Math.Abs(toY - fromY) > Math.Abs(toX - fromX);
      if (flag)
      {
        int num1 = fromX;
        fromX = fromY;
        fromY = num1;
        int num2 = toX;
        toX = toY;
        toY = num2;
      }
      int num3 = Math.Abs(toX - fromX);
      int num4 = Math.Abs(toY - fromY);
      int num5 = -num3 >> 1;
      int num6 = fromY < toY ? 1 : -1;
      int num7 = fromX < toX ? 1 : -1;
      int num8 = 0;
      int num9 = fromX;
      int num10 = fromY;
      while (num9 != toX)
      {
        int x = flag ? num10 : num9;
        int y = flag ? num9 : num10;
        if (num8 == 1)
        {
          if (this.image.get_Renamed(x, y))
            ++num8;
        }
        else if (!this.image.get_Renamed(x, y))
          ++num8;
        if (num8 == 3)
        {
          int num1 = num9 - fromX;
          int num2 = num10 - fromY;
          return (float) Math.Sqrt((double) (num1 * num1 + num2 * num2));
        }
        num5 += num4;
        if (num5 > 0)
        {
          if (num10 != toY)
          {
            num10 += num6;
            num5 -= num3;
          }
          else
            break;
        }
        num9 += num7;
      }
      int num11 = toX - fromX;
      int num12 = toY - fromY;
      return (float) Math.Sqrt((double) (num11 * num11 + num12 * num12));
    }

    /// <summary>
    /// <p>Attempts to locate an alignment pattern in a limited region of the image, which is
    ///             guessed to contain it. This method uses {@link AlignmentPattern}.</p>
    /// </summary>
    /// <param name="overallEstModuleSize">estimated module size so far
    ///             </param><param name="estAlignmentX">x coordinate of center of area probably containing alignment pattern
    ///             </param><param name="estAlignmentY">y coordinate of above
    ///             </param><param name="allowanceFactor">number of pixels in all directions to search from the center
    ///             </param>
    /// <returns>
    /// {@link AlignmentPattern} if found, or null otherwise
    /// 
    /// </returns>
    /// <throws>ReaderException if an unexpected error occurs during detection </throws>
    protected internal virtual AlignmentPattern findAlignmentInRegion(float overallEstModuleSize, int estAlignmentX, int estAlignmentY, float allowanceFactor)
    {
      int num1 = (int) ((double) allowanceFactor * (double) overallEstModuleSize);
      int startX = Math.Max(0, estAlignmentX - num1);
      int num2 = Math.Min(this.image.Width - 1, estAlignmentX + num1);
      if ((double) (num2 - startX) < (double) overallEstModuleSize * 3.0)
        throw ReaderException.Instance;
      int startY = Math.Max(0, estAlignmentY - num1);
      int num3 = Math.Min(this.image.Height - 1, estAlignmentY + num1);
      return new AlignmentPatternFinder(this.image, startX, startY, num2 - startX, num3 - startY, overallEstModuleSize, this.resultPointCallback).find();
    }

    /// <summary>
    /// Ends up being a bit faster than Math.round(). This merely rounds its argument to the nearest int,
    ///             where x.5 rounds up.
    /// 
    /// </summary>
    private static int round(float d)
    {
      return (int) ((double) d + 0.5);
    }
  }
}
