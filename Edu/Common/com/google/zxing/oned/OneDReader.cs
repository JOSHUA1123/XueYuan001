// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.OneDReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;
using System.Collections;

namespace com.google.zxing.oned
{
  /// <summary>
  /// Encapsulates functionality and implementation that is common to all families
  ///             of one-dimensional barcodes.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class OneDReader : Reader
  {
    internal static readonly int PATTERN_MATCH_RESULT_SCALE_FACTOR = 256;
    private const int INTEGER_MATH_SHIFT = 8;

    public virtual Result decode(BinaryBitmap image)
    {
      return this.decode(image, (Hashtable) null);
    }

    public virtual Result decode(BinaryBitmap image, Hashtable hints)
    {
      try
      {
        return this.doDecode(image, hints);
      }
      catch (ReaderException ex)
      {
        if (hints == null || !hints.ContainsKey((object) DecodeHintType.TRY_HARDER) || !image.RotateSupported)
          throw ex;
        BinaryBitmap image1 = image.rotateCounterClockwise();
        Result result = this.doDecode(image1, hints);
        Hashtable resultMetadata = result.ResultMetadata;
        int num = 270;
        if (resultMetadata != null && resultMetadata.ContainsKey((object) ResultMetadataType.ORIENTATION))
          num = (num + (int) resultMetadata[(object) ResultMetadataType.ORIENTATION]) % 360;
        result.putMetadata(ResultMetadataType.ORIENTATION, (object) num);
        ResultPoint[] resultPoints = result.ResultPoints;
        int height = image1.Height;
        for (int index = 0; index < resultPoints.Length; ++index)
          resultPoints[index] = new ResultPoint((float) ((double) height - (double) resultPoints[index].Y - 1.0), resultPoints[index].X);
        return result;
      }
    }

    /// <summary>
    /// We're going to examine rows from the middle outward, searching alternately above and below the
    ///             middle, and farther out each time. rowStep is the number of rows between each successive
    ///             attempt above and below the middle. So we'd scan row middle, then middle - rowStep, then
    ///             middle + rowStep, then middle - (2 * rowStep), etc.
    ///             rowStep is bigger as the image is taller, but is always at least 1. We've somewhat arbitrarily
    ///             decided that moving up and down by about 1/16 of the image is pretty good; we try more of the
    ///             image if "trying harder".
    /// 
    /// 
    /// </summary>
    /// <param name="image">The image to decode
    ///             </param><param name="hints">Any hints that were requested
    ///             </param>
    /// <returns>
    /// The contents of the decoded barcode
    /// 
    /// </returns>
    /// <throws>ReaderException Any spontaneous errors which occur </throws>
    private Result doDecode(BinaryBitmap image, Hashtable hints)
    {
      int width = image.Width;
      int height = image.Height;
      com.google.zxing.common.BitArray row = new com.google.zxing.common.BitArray(width);
      int num1 = height >> 1;
      bool flag1 = hints != null && hints.ContainsKey((object) DecodeHintType.TRY_HARDER);
      int num2 = Math.Max(1, height >> (flag1 ? 7 : 4));
      int num3 = !flag1 ? 9 : height;
      for (int index1 = 0; index1 < num3; ++index1)
      {
        int num4 = index1 + 1 >> 1;
        bool flag2 = (index1 & 1) == 0;
        int num5 = num1 + num2 * (flag2 ? num4 : -num4);
        if (num5 >= 0)
        {
          if (num5 < height)
          {
            try
            {
              row = image.getBlackRow(num5, row);
            }
            catch (ReaderException ex)
            {
              continue;
            }
            for (int index2 = 0; index2 < 2; ++index2)
            {
              if (index2 == 1)
              {
                row.reverse();
                if (hints != null)
                {
                  if (hints.ContainsKey((object) DecodeHintType.NEED_RESULT_POINT_CALLBACK))
                  {
                    Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
                    foreach (object index3 in (IEnumerable) hints.Keys)
                    {
                      if (!index3.Equals((object) DecodeHintType.NEED_RESULT_POINT_CALLBACK))
                        hashtable[index3] = hints[index3];
                    }
                    hints = hashtable;
                  }
                }
              }
              try
              {
                Result result = this.decodeRow(num5, row, hints);
                if (index2 == 1)
                {
                  result.putMetadata(ResultMetadataType.ORIENTATION, (object) 180);
                  ResultPoint[] resultPoints = result.ResultPoints;
                  resultPoints[0] = new ResultPoint((float) ((double) width - (double) resultPoints[0].X - 1.0), resultPoints[0].Y);
                  resultPoints[1] = new ResultPoint((float) ((double) width - (double) resultPoints[1].X - 1.0), resultPoints[1].Y);
                }
                return result;
              }
              catch (ReaderException ex)
              {
              }
            }
          }
          else
            break;
        }
        else
          break;
      }
      throw ReaderException.Instance;
    }

    /// <summary>
    /// Records the size of successive runs of white and black pixels in a row, starting at a given point.
    ///             The values are recorded in the given array, and the number of runs recorded is equal to the size
    ///             of the array. If the row starts on a white pixel at the given start point, then the first count
    ///             recorded is the run of white pixels starting from that point; likewise it is the count of a run
    ///             of black pixels if the row begin on a black pixels at that point.
    /// 
    /// 
    /// </summary>
    /// <param name="row">row to count from
    ///             </param><param name="start">offset into row to start at
    ///             </param><param name="counters">array into which to record counts
    ///             </param><throws>ReaderException if counters cannot be filled entirely from row before running out </throws>
    /// <summary>
    /// of pixels
    /// 
    /// </summary>
    internal static void recordPattern(com.google.zxing.common.BitArray row, int start, int[] counters)
    {
      int length = counters.Length;
      for (int index = 0; index < length; ++index)
        counters[index] = 0;
      int size = row.Size;
      if (start >= size)
        throw ReaderException.Instance;
      bool flag = !row.get_Renamed(start);
      int index1 = 0;
      int i;
      for (i = start; i < size; ++i)
      {
        if (row.get_Renamed(i) ^ flag)
        {
          ++counters[index1];
        }
        else
        {
          ++index1;
          if (index1 != length)
          {
            counters[index1] = 1;
            flag = !flag;
          }
          else
            break;
        }
      }
      if (index1 != length && (index1 != length - 1 || i != size))
        throw ReaderException.Instance;
    }

    /// <summary>
    /// Determines how closely a set of observed counts of runs of black/white values matches a given
    ///             target pattern. This is reported as the ratio of the total variance from the expected pattern
    ///             proportions across all pattern elements, to the length of the pattern.
    /// 
    /// 
    /// </summary>
    /// <param name="counters">observed counters
    ///             </param><param name="pattern">expected pattern
    ///             </param><param name="maxIndividualVariance">The most any counter can differ before we give up
    ///             </param>
    /// <returns>
    /// ratio of total variance between counters and pattern compared to total pattern size,
    ///             where the ratio has been multiplied by 256. So, 0 means no variance (perfect match); 256 means
    ///             the total variance between counters and patterns equals the pattern length, higher values mean
    ///             even more variance
    /// 
    /// </returns>
    internal static int patternMatchVariance(int[] counters, int[] pattern, int maxIndividualVariance)
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

    /// <summary>
    /// <p>Attempts to decode a one-dimensional barcode format given a single row of
    ///             an image.</p>
    /// </summary>
    /// <param name="rowNumber">row number from top of the row
    ///             </param><param name="row">the black/white pixel data of the row
    ///             </param><param name="hints">decode hints
    ///             </param>
    /// <returns>
    /// {@link Result} containing encoded string and start/end of barcode
    /// 
    /// </returns>
    /// <throws>ReaderException if an error occurs or barcode cannot be found </throws>
    public abstract Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints);
  }
}
