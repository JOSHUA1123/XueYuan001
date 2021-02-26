// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.detector.AlignmentPatternFinder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System;
using System.Collections;

namespace com.google.zxing.qrcode.detector
{
  internal sealed class AlignmentPatternFinder
  {
    private BitMatrix image;
    private ArrayList possibleCenters;
    private int startX;
    private int startY;
    private int width;
    private int height;
    private float moduleSize;
    private int[] crossCheckStateCount;
    private ResultPointCallback resultPointCallback;

    /// <summary>
    /// <p>Creates a finder that will look in a portion of the whole image.</p>
    /// </summary>
    /// <param name="image">image to search
    ///             </param><param name="startX">left column from which to start searching
    ///             </param><param name="startY">top row from which to start searching
    ///             </param><param name="width">width of region to search
    ///             </param><param name="height">height of region to search
    ///             </param><param name="moduleSize">estimated module size so far
    ///             </param>
    internal AlignmentPatternFinder(BitMatrix image, int startX, int startY, int width, int height, float moduleSize, ResultPointCallback resultPointCallback)
    {
      this.image = image;
      this.possibleCenters = ArrayList.Synchronized(new ArrayList(5));
      this.startX = startX;
      this.startY = startY;
      this.width = width;
      this.height = height;
      this.moduleSize = moduleSize;
      this.crossCheckStateCount = new int[3];
      this.resultPointCallback = resultPointCallback;
    }

    /// <summary>
    /// <p>This method attempts to find the bottom-right alignment pattern in the image. It is a bit messy since
    ///             it's pretty performance-critical and so is written to be fast foremost.</p>
    /// </summary>
    /// 
    /// <returns>
    /// {@link AlignmentPattern} if found
    /// 
    /// </returns>
    /// <throws>ReaderException if not found </throws>
    internal AlignmentPattern find()
    {
      int num1 = this.startX;
      int num2 = this.height;
      int j = num1 + this.width;
      int num3 = this.startY + (num2 >> 1);
      int[] stateCount = new int[3];
      for (int index1 = 0; index1 < num2; ++index1)
      {
        int num4 = num3 + ((index1 & 1) == 0 ? index1 + 1 >> 1 : -(index1 + 1 >> 1));
        stateCount[0] = 0;
        stateCount[1] = 0;
        stateCount[2] = 0;
        int num5 = num1;
        while (num5 < j && !this.image.get_Renamed(num5, num4))
          ++num5;
        int index2 = 0;
        for (; num5 < j; ++num5)
        {
          if (this.image.get_Renamed(num5, num4))
          {
            if (index2 == 1)
              ++stateCount[index2];
            else if (index2 == 2)
            {
              if (this.foundPatternCross(stateCount))
              {
                AlignmentPattern alignmentPattern = this.handlePossibleCenter(stateCount, num4, num5);
                if (alignmentPattern != null)
                  return alignmentPattern;
              }
              stateCount[0] = stateCount[2];
              stateCount[1] = 1;
              stateCount[2] = 0;
              index2 = 1;
            }
            else
              ++stateCount[++index2];
          }
          else
          {
            if (index2 == 1)
              ++index2;
            ++stateCount[index2];
          }
        }
        if (this.foundPatternCross(stateCount))
        {
          AlignmentPattern alignmentPattern = this.handlePossibleCenter(stateCount, num4, j);
          if (alignmentPattern != null)
            return alignmentPattern;
        }
      }
      if (this.possibleCenters.Count != 0)
        return (AlignmentPattern) this.possibleCenters[0];
      throw ReaderException.Instance;
    }

    /// <summary>
    /// Given a count of black/white/black pixels just seen and an end position,
    ///             figures the location of the center of this black/white/black run.
    /// 
    /// </summary>
    private static float centerFromEnd(int[] stateCount, int end)
    {
      return (float) (end - stateCount[2]) - (float) stateCount[1] / 2f;
    }

    /// <param name="stateCount">count of black/white/black pixels just read
    ///             </param>
    /// <returns>
    /// true iff the proportions of the counts is close enough to the 1/1/1 ratios
    ///             used by alignment patterns to be considered a match
    /// 
    /// </returns>
    private bool foundPatternCross(int[] stateCount)
    {
      float num1 = this.moduleSize;
      float num2 = num1 / 2f;
      for (int index = 0; index < 3; ++index)
      {
        if ((double) Math.Abs(num1 - (float) stateCount[index]) >= (double) num2)
          return false;
      }
      return true;
    }

    /// <summary>
    /// <p>After a horizontal scan finds a potential alignment pattern, this method
    ///             "cross-checks" by scanning down vertically through the center of the possible
    ///             alignment pattern to see if the same proportion is detected.</p>
    /// </summary>
    /// <param name="startI">row where an alignment pattern was detected
    ///             </param><param name="centerJ">center of the section that appears to cross an alignment pattern
    ///             </param><param name="maxCount">maximum reasonable number of modules that should be
    ///             observed in any reading state, based on the results of the horizontal scan
    ///             </param>
    /// <returns>
    /// vertical center of alignment pattern, or {@link Float#NaN} if not found
    /// 
    /// </returns>
    private float crossCheckVertical(int startI, int centerJ, int maxCount, int originalStateCountTotal)
    {
      BitMatrix bitMatrix = this.image;
      int height = bitMatrix.Height;
      int[] stateCount = this.crossCheckStateCount;
      stateCount[0] = 0;
      stateCount[1] = 0;
      stateCount[2] = 0;
      int y;
      for (y = startI; y >= 0 && bitMatrix.get_Renamed(centerJ, y) && stateCount[1] <= maxCount; --y)
        ++stateCount[1];
      if (y < 0 || stateCount[1] > maxCount)
        return float.NaN;
      for (; y >= 0 && !bitMatrix.get_Renamed(centerJ, y) && stateCount[0] <= maxCount; --y)
        ++stateCount[0];
      if (stateCount[0] > maxCount)
        return float.NaN;
      int num;
      for (num = startI + 1; num < height && bitMatrix.get_Renamed(centerJ, num) && stateCount[1] <= maxCount; ++num)
        ++stateCount[1];
      if (num == height || stateCount[1] > maxCount)
        return float.NaN;
      for (; num < height && !bitMatrix.get_Renamed(centerJ, num) && stateCount[2] <= maxCount; ++num)
        ++stateCount[2];
      if (stateCount[2] > maxCount || 5 * Math.Abs(stateCount[0] + stateCount[1] + stateCount[2] - originalStateCountTotal) >= 2 * originalStateCountTotal || !this.foundPatternCross(stateCount))
        return float.NaN;
      return AlignmentPatternFinder.centerFromEnd(stateCount, num);
    }

    /// <summary>
    /// <p>This is called when a horizontal scan finds a possible alignment pattern. It will
    ///             cross check with a vertical scan, and if successful, will see if this pattern had been
    ///             found on a previous horizontal scan. If so, we consider it confirmed and conclude we have
    ///             found the alignment pattern.</p>
    /// </summary>
    /// <param name="stateCount">reading state module counts from horizontal scan
    ///             </param><param name="i">row where alignment pattern may be found
    ///             </param><param name="j">end of possible alignment pattern in row
    ///             </param>
    /// <returns>
    /// {@link AlignmentPattern} if we have found the same pattern twice, or null if not
    /// 
    /// </returns>
    private AlignmentPattern handlePossibleCenter(int[] stateCount, int i, int j)
    {
      int originalStateCountTotal = stateCount[0] + stateCount[1] + stateCount[2];
      float num1 = AlignmentPatternFinder.centerFromEnd(stateCount, j);
      float num2 = this.crossCheckVertical(i, (int) num1, 2 * stateCount[1], originalStateCountTotal);
      if (!float.IsNaN(num2))
      {
        float num3 = (float) (stateCount[0] + stateCount[1] + stateCount[2]) / 3f;
        int count = this.possibleCenters.Count;
        for (int index = 0; index < count; ++index)
        {
          if (((AlignmentPattern) this.possibleCenters[index]).aboutEquals(num3, num2, num1))
            return new AlignmentPattern(num1, num2, num3);
        }
        ResultPoint point = (ResultPoint) new AlignmentPattern(num1, num2, num3);
        this.possibleCenters.Add((object) point);
        if (this.resultPointCallback != null)
          this.resultPointCallback.foundPossibleResultPoint(point);
      }
      return (AlignmentPattern) null;
    }
  }
}
