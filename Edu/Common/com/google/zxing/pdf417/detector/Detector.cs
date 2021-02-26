// Decompiled with JetBrains decompiler
// Type: com.google.zxing.pdf417.detector.Detector
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System.Collections;

namespace com.google.zxing.pdf417.detector
{
  /// <summary>
  /// <p>Encapsulates logic that can detect a PDF417 Code in an image, even if the
  ///             PDF417 Code is rotated or skewed, or partially obscured.</p>
  /// </summary>
  /// <author>SITA Lab (kevin.osullivan@sita.aero)
  ///             </author><author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Detector
  {
    private static int MAX_AVG_VARIANCE = (int) SupportClass.Identity(107.52f);
    private static int MAX_INDIVIDUAL_VARIANCE = (int) SupportClass.Identity(204.8f);
    private static readonly int[] START_PATTERN = new int[8]
    {
      8,
      1,
      1,
      1,
      1,
      1,
      1,
      3
    };
    private static readonly int[] START_PATTERN_REVERSE = new int[8]
    {
      3,
      1,
      1,
      1,
      1,
      1,
      1,
      8
    };
    private static readonly int[] STOP_PATTERN = new int[9]
    {
      7,
      1,
      1,
      3,
      1,
      1,
      1,
      2,
      1
    };
    private static readonly int[] STOP_PATTERN_REVERSE = new int[9]
    {
      1,
      2,
      1,
      1,
      1,
      3,
      1,
      1,
      7
    };
    private const int SKEW_THRESHOLD = 2;
    private BinaryBitmap image;

    public Detector(BinaryBitmap image)
    {
      this.image = image;
    }

    /// <summary>
    /// <p>Detects a PDF417 Code in an image, simply.</p>
    /// </summary>
    /// 
    /// <returns>
    /// {@link DetectorResult} encapsulating results of detecting a PDF417 Code
    /// 
    /// </returns>
    /// <throws>ReaderException if no QR Code can be found </throws>
    public DetectorResult detect()
    {
      return this.detect((Hashtable) null);
    }

    /// <summary>
    /// <p>Detects a PDF417 Code in an image. Only checks 0 and 180 degree rotations.</p>
    /// </summary>
    /// <param name="hints">optional hints to detector
    ///             </param>
    /// <returns>
    /// {@link DetectorResult} encapsulating results of detecting a PDF417 Code
    /// 
    /// </returns>
    /// <throws>ReaderException if no PDF417 Code can be found </throws>
    public DetectorResult detect(Hashtable hints)
    {
      BitMatrix blackMatrix = this.image.BlackMatrix;
      ResultPoint[] vertices = Detector.findVertices(blackMatrix);
      if (vertices == null)
      {
        vertices = Detector.findVertices180(blackMatrix);
        if (vertices != null)
          Detector.correctCodeWordVertices(vertices, true);
      }
      else
        Detector.correctCodeWordVertices(vertices, false);
      if (vertices == null)
        throw ReaderException.Instance;
      float moduleWidth = Detector.computeModuleWidth(vertices);
      if ((double) moduleWidth < 1.0)
        throw ReaderException.Instance;
      int dimension = Detector.computeDimension(vertices[4], vertices[6], vertices[5], vertices[7], moduleWidth);
      if (dimension < 1)
        throw ReaderException.Instance;
      return new DetectorResult(Detector.sampleGrid(blackMatrix, vertices[4], vertices[5], vertices[6], vertices[7], dimension), new ResultPoint[4]
      {
        vertices[4],
        vertices[5],
        vertices[6],
        vertices[7]
      });
    }

    /// <summary>
    /// Locate the vertices and the codewords area of a black blob using the Start
    ///             and Stop patterns as locators. Assumes that the barcode begins in the left half
    ///             of the image, and ends in the right half.
    ///             TODO: Fix this assumption, allowing the barcode to be anywhere in the image.
    ///             TODO: Scanning every row is very expensive. We should only do this for TRY_HARDER.
    /// 
    /// 
    /// </summary>
    /// <param name="matrix">the scanned barcode image.
    ///             </param>
    /// <returns>
    /// an array containing the vertices:
    ///             vertices[0] x, y top left barcode
    ///             vertices[1] x, y bottom left barcode
    ///             vertices[2] x, y top right barcode
    ///             vertices[3] x, y bottom right barcode
    ///             vertices[4] x, y top left codeword area
    ///             vertices[5] x, y bottom left codeword area
    ///             vertices[6] x, y top right codeword area
    ///             vertices[7] x, y bottom right codeword area
    /// 
    /// </returns>
    private static ResultPoint[] findVertices(BitMatrix matrix)
    {
      int height = matrix.Height;
      int num = matrix.Width >> 1;
      ResultPoint[] resultPointArray = new ResultPoint[8];
      bool flag = false;
      for (int row = 0; row < height; ++row)
      {
        int[] guardPattern = Detector.findGuardPattern(matrix, 0, row, num, false, Detector.START_PATTERN);
        if (guardPattern != null)
        {
          resultPointArray[0] = new ResultPoint((float) guardPattern[0], (float) row);
          resultPointArray[4] = new ResultPoint((float) guardPattern[1], (float) row);
          flag = true;
          break;
        }
      }
      if (flag)
      {
        flag = false;
        for (int row = height - 1; row > 0; --row)
        {
          int[] guardPattern = Detector.findGuardPattern(matrix, 0, row, num, false, Detector.START_PATTERN);
          if (guardPattern != null)
          {
            resultPointArray[1] = new ResultPoint((float) guardPattern[0], (float) row);
            resultPointArray[5] = new ResultPoint((float) guardPattern[1], (float) row);
            flag = true;
            break;
          }
        }
      }
      if (flag)
      {
        flag = false;
        for (int row = 0; row < height; ++row)
        {
          int[] guardPattern = Detector.findGuardPattern(matrix, num, row, num, false, Detector.STOP_PATTERN);
          if (guardPattern != null)
          {
            resultPointArray[2] = new ResultPoint((float) guardPattern[1], (float) row);
            resultPointArray[6] = new ResultPoint((float) guardPattern[0], (float) row);
            flag = true;
            break;
          }
        }
      }
      if (flag)
      {
        flag = false;
        for (int row = height - 1; row > 0; --row)
        {
          int[] guardPattern = Detector.findGuardPattern(matrix, num, row, num, false, Detector.STOP_PATTERN);
          if (guardPattern != null)
          {
            resultPointArray[3] = new ResultPoint((float) guardPattern[1], (float) row);
            resultPointArray[7] = new ResultPoint((float) guardPattern[0], (float) row);
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return (ResultPoint[]) null;
      return resultPointArray;
    }

    /// <summary>
    /// Locate the vertices and the codewords area of a black blob using the Start
    ///             and Stop patterns as locators. This assumes that the image is rotated 180
    ///             degrees and if it locates the start and stop patterns at it will re-map
    ///             the vertices for a 0 degree rotation.
    ///             TODO: Change assumption about barcode location.
    ///             TODO: Scanning every row is very expensive. We should only do this for TRY_HARDER.
    /// 
    /// 
    /// </summary>
    /// <param name="matrix">the scanned barcode image.
    ///             </param>
    /// <returns>
    /// an array containing the vertices:
    ///             vertices[0] x, y top left barcode
    ///             vertices[1] x, y bottom left barcode
    ///             vertices[2] x, y top right barcode
    ///             vertices[3] x, y bottom right barcode
    ///             vertices[4] x, y top left codeword area
    ///             vertices[5] x, y bottom left codeword area
    ///             vertices[6] x, y top right codeword area
    ///             vertices[7] x, y bottom right codeword area
    /// 
    /// </returns>
    private static ResultPoint[] findVertices180(BitMatrix matrix)
    {
      int height = matrix.Height;
      int num = matrix.Width >> 1;
      ResultPoint[] resultPointArray = new ResultPoint[8];
      bool flag = false;
      for (int row = height - 1; row > 0; --row)
      {
        int[] guardPattern = Detector.findGuardPattern(matrix, num, row, num, true, Detector.START_PATTERN_REVERSE);
        if (guardPattern != null)
        {
          resultPointArray[0] = new ResultPoint((float) guardPattern[1], (float) row);
          resultPointArray[4] = new ResultPoint((float) guardPattern[0], (float) row);
          flag = true;
          break;
        }
      }
      if (flag)
      {
        flag = false;
        for (int row = 0; row < height; ++row)
        {
          int[] guardPattern = Detector.findGuardPattern(matrix, num, row, num, true, Detector.START_PATTERN_REVERSE);
          if (guardPattern != null)
          {
            resultPointArray[1] = new ResultPoint((float) guardPattern[1], (float) row);
            resultPointArray[5] = new ResultPoint((float) guardPattern[0], (float) row);
            flag = true;
            break;
          }
        }
      }
      if (flag)
      {
        flag = false;
        for (int row = height - 1; row > 0; --row)
        {
          int[] guardPattern = Detector.findGuardPattern(matrix, 0, row, num, false, Detector.STOP_PATTERN_REVERSE);
          if (guardPattern != null)
          {
            resultPointArray[2] = new ResultPoint((float) guardPattern[0], (float) row);
            resultPointArray[6] = new ResultPoint((float) guardPattern[1], (float) row);
            flag = true;
            break;
          }
        }
      }
      if (flag)
      {
        flag = false;
        for (int row = 0; row < height; ++row)
        {
          int[] guardPattern = Detector.findGuardPattern(matrix, 0, row, num, false, Detector.STOP_PATTERN_REVERSE);
          if (guardPattern != null)
          {
            resultPointArray[3] = new ResultPoint((float) guardPattern[0], (float) row);
            resultPointArray[7] = new ResultPoint((float) guardPattern[1], (float) row);
            flag = true;
            break;
          }
        }
      }
      if (!flag)
        return (ResultPoint[]) null;
      return resultPointArray;
    }

    /// <summary>
    /// Because we scan horizontally to detect the start and stop patterns, the vertical component of
    ///             the codeword coordinates will be slightly wrong if there is any skew or rotation in the image.
    ///             This method moves those points back onto the edges of the theoretically perfect bounding
    ///             quadrilateral if needed.
    /// 
    /// 
    /// </summary>
    /// <param name="vertices">The eight vertices located by findVertices().
    ///             </param>
    private static void correctCodeWordVertices(ResultPoint[] vertices, bool upsideDown)
    {
      float num1 = vertices[4].Y - vertices[6].Y;
      if (upsideDown)
        num1 = -num1;
      if ((double) num1 > 2.0)
      {
        float num2 = vertices[4].X - vertices[0].X;
        float num3 = vertices[6].X - vertices[0].X;
        float num4 = vertices[6].Y - vertices[0].Y;
        float num5 = num2 * num4 / num3;
        vertices[4] = new ResultPoint(vertices[4].X, vertices[4].Y + num5);
      }
      else if (-(double) num1 > 2.0)
      {
        float num2 = vertices[2].X - vertices[6].X;
        float num3 = vertices[2].X - vertices[4].X;
        float num4 = vertices[2].Y - vertices[4].Y;
        float num5 = num2 * num4 / num3;
        vertices[6] = new ResultPoint(vertices[6].X, vertices[6].Y - num5);
      }
      float num6 = vertices[7].Y - vertices[5].Y;
      if (upsideDown)
        num6 = -num6;
      if ((double) num6 > 2.0)
      {
        float num2 = vertices[5].X - vertices[1].X;
        float num3 = vertices[7].X - vertices[1].X;
        float num4 = vertices[7].Y - vertices[1].Y;
        float num5 = num2 * num4 / num3;
        vertices[5] = new ResultPoint(vertices[5].X, vertices[5].Y + num5);
      }
      else
      {
        if (-(double) num6 <= 2.0)
          return;
        float num2 = vertices[3].X - vertices[7].X;
        float num3 = vertices[3].X - vertices[5].X;
        float num4 = vertices[3].Y - vertices[5].Y;
        float num5 = num2 * num4 / num3;
        vertices[7] = new ResultPoint(vertices[7].X, vertices[7].Y - num5);
      }
    }

    /// <summary>
    /// <p>Estimates module size (pixels in a module) based on the Start and End
    ///             finder patterns.</p>
    /// </summary>
    /// <param name="vertices">an array of vertices:
    ///             vertices[0] x, y top left barcode
    ///             vertices[1] x, y bottom left barcode
    ///             vertices[2] x, y top right barcode
    ///             vertices[3] x, y bottom right barcode
    ///             vertices[4] x, y top left codeword area
    ///             vertices[5] x, y bottom left codeword area
    ///             vertices[6] x, y top right codeword area
    ///             vertices[7] x, y bottom right codeword area
    ///             </param>
    /// <returns>
    /// the module size.
    /// 
    /// </returns>
    private static float computeModuleWidth(ResultPoint[] vertices)
    {
      return (float) ((((double) ResultPoint.distance(vertices[0], vertices[4]) + (double) ResultPoint.distance(vertices[1], vertices[5])) / 34.0 + ((double) ResultPoint.distance(vertices[6], vertices[2]) + (double) ResultPoint.distance(vertices[7], vertices[3])) / 36.0) / 2.0);
    }

    /// <summary>
    /// Computes the dimension (number of modules in a row) of the PDF417 Code
    ///             based on vertices of the codeword area and estimated module size.
    /// 
    /// 
    /// </summary>
    /// <param name="topLeft">of codeword area
    ///             </param><param name="topRight">of codeword area
    ///             </param><param name="bottomLeft">of codeword area
    ///             </param><param name="bottomRight">of codeword are
    ///             </param><param name="moduleWidth">estimated module size
    ///             </param>
    /// <returns>
    /// the number of modules in a row.
    /// 
    /// </returns>
    private static int computeDimension(ResultPoint topLeft, ResultPoint topRight, ResultPoint bottomLeft, ResultPoint bottomRight, float moduleWidth)
    {
      return ((Detector.round(ResultPoint.distance(topLeft, topRight) / moduleWidth) + Detector.round(ResultPoint.distance(bottomLeft, bottomRight) / moduleWidth) >> 1) + 8) / 17 * 17;
    }

    private static BitMatrix sampleGrid(BitMatrix matrix, ResultPoint topLeft, ResultPoint bottomLeft, ResultPoint topRight, ResultPoint bottomRight, int dimension)
    {
      return GridSampler.Instance.sampleGrid(matrix, dimension, 0.0f, 0.0f, (float) dimension, 0.0f, (float) dimension, (float) dimension, 0.0f, (float) dimension, topLeft.X, topLeft.Y, topRight.X, topRight.Y, bottomRight.X, bottomRight.Y, bottomLeft.X, bottomLeft.Y);
    }

    /// <summary>
    /// Ends up being a bit faster than Math.round(). This merely rounds its
    ///             argument to the nearest int, where x.5 rounds up.
    /// 
    /// </summary>
    private static int round(float d)
    {
      return (int) ((double) d + 0.5);
    }

    /// <param name="matrix">row of black/white values to search
    ///             </param><param name="column">x position to start search
    ///             </param><param name="row">y position to start search
    ///             </param><param name="width">the number of pixels to search on this row
    ///             </param><param name="pattern">pattern of counts of number of black and white pixels that are
    ///             being searched for as a pattern
    ///             </param>
    /// <returns>
    /// start/end horizontal offset of guard pattern, as an array of two ints.
    /// 
    /// </returns>
    private static int[] findGuardPattern(BitMatrix matrix, int column, int row, int width, bool whiteFirst, int[] pattern)
    {
      int length = pattern.Length;
      int[] counters = new int[length];
      bool flag = whiteFirst;
      int index1 = 0;
      int num = column;
      for (int x = column; x < column + width; ++x)
      {
        if (matrix.get_Renamed(x, row) ^ flag)
        {
          ++counters[index1];
        }
        else
        {
          if (index1 == length - 1)
          {
            if (Detector.patternMatchVariance(counters, pattern, Detector.MAX_INDIVIDUAL_VARIANCE) < Detector.MAX_AVG_VARIANCE)
              return new int[2]
              {
                num,
                x
              };
            num += counters[0] + counters[1];
            for (int index2 = 2; index2 < length; ++index2)
              counters[index2 - 2] = counters[index2];
            counters[length - 2] = 0;
            counters[length - 1] = 0;
            --index1;
          }
          else
            ++index1;
          counters[index1] = 1;
          flag = !flag;
        }
      }
      return (int[]) null;
    }

    /// <summary>
    /// Determines how closely a set of observed counts of runs of black/white
    ///             values matches a given target pattern. This is reported as the ratio of
    ///             the total variance from the expected pattern proportions across all
    ///             pattern elements, to the length of the pattern.
    /// 
    /// 
    /// </summary>
    /// <param name="counters">observed counters
    ///             </param><param name="pattern">expected pattern
    ///             </param><param name="maxIndividualVariance">The most any counter can differ before we give up
    ///             </param>
    /// <returns>
    /// ratio of total variance between counters and pattern compared to
    ///             total pattern size, where the ratio has been multiplied by 256.
    ///             So, 0 means no variance (perfect match); 256 means the total
    ///             variance between counters and patterns equals the pattern length,
    ///             higher values mean even more variance
    /// 
    /// </returns>
    private static int patternMatchVariance(int[] counters, int[] pattern, int maxIndividualVariance)
    {
      int length = counters.Length;
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < length; ++index)
      {
        num1 += counters[index];
        num2 += pattern[index];
      }
      if (num1 < num2)
        return int.MaxValue;
      int num3 = (num1 << 8) / num2;
      maxIndividualVariance = maxIndividualVariance * num3 >> 8;
      int num4 = 0;
      for (int index = 0; index < length; ++index)
      {
        int num5 = counters[index] << 8;
        int num6 = pattern[index] * num3;
        int num7 = num5 > num6 ? num5 - num6 : num6 - num5;
        if (num7 > maxIndividualVariance)
          return int.MaxValue;
        num4 += num7;
      }
      return num4 / num1;
    }
  }
}
