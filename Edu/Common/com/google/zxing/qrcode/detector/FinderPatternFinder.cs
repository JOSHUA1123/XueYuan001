// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.detector.FinderPatternFinder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System;
using System.Collections;

namespace com.google.zxing.qrcode.detector
{
  public class FinderPatternFinder
  {
    private const int CENTER_QUORUM = 2;
    protected internal const int MIN_SKIP = 3;
    protected internal const int MAX_MODULES = 57;
    private const int INTEGER_MATH_SHIFT = 8;
    private BitMatrix image;
    private ArrayList possibleCenters;
    private bool hasSkipped;
    private int[] crossCheckStateCount;
    private ResultPointCallback resultPointCallback;

    protected internal virtual BitMatrix Image
    {
      get
      {
        return this.image;
      }
    }

    protected internal virtual ArrayList PossibleCenters
    {
      get
      {
        return this.possibleCenters;
      }
    }

    private int[] CrossCheckStateCount
    {
      get
      {
        this.crossCheckStateCount[0] = 0;
        this.crossCheckStateCount[1] = 0;
        this.crossCheckStateCount[2] = 0;
        this.crossCheckStateCount[3] = 0;
        this.crossCheckStateCount[4] = 0;
        return this.crossCheckStateCount;
      }
    }

    /// <summary>
    /// <p>Creates a finder that will search the image for three finder patterns.</p>
    /// </summary>
    /// <param name="image">image to search
    ///             </param>
    public FinderPatternFinder(BitMatrix image)
      : this(image, (ResultPointCallback) null)
    {
    }

    public FinderPatternFinder(BitMatrix image, ResultPointCallback resultPointCallback)
    {
      this.image = image;
      this.possibleCenters = ArrayList.Synchronized(new ArrayList(10));
      this.crossCheckStateCount = new int[5];
      this.resultPointCallback = resultPointCallback;
    }

    internal virtual FinderPatternInfo find(Hashtable hints)
    {
      bool flag1 = hints != null && hints.ContainsKey((object) DecodeHintType.TRY_HARDER);
      int height = this.image.Height;
      int width = this.image.Width;
      int num1 = 3 * height / 228;
      if (num1 < 3 || flag1)
        num1 = 3;
      bool flag2 = false;
      int[] stateCount = new int[5];
      int num2 = num1 - 1;
      while (num2 < height && !flag2)
      {
        stateCount[0] = 0;
        stateCount[1] = 0;
        stateCount[2] = 0;
        stateCount[3] = 0;
        stateCount[4] = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < width; ++index2)
        {
          if (this.image.get_Renamed(index2, num2))
          {
            if ((index1 & 1) == 1)
              ++index1;
            ++stateCount[index1];
          }
          else if ((index1 & 1) == 0)
          {
            if (index1 == 4)
            {
              if (FinderPatternFinder.foundPatternCross(stateCount))
              {
                if (this.handlePossibleCenter(stateCount, num2, index2))
                {
                  num1 = 2;
                  if (this.hasSkipped)
                  {
                    flag2 = this.haveMultiplyConfirmedCenters();
                  }
                  else
                  {
                    int rowSkip = this.findRowSkip();
                    if (rowSkip > stateCount[2])
                    {
                      num2 += rowSkip - stateCount[2] - num1;
                      index2 = width - 1;
                    }
                  }
                }
                else
                {
                  do
                  {
                    ++index2;
                  }
                  while (index2 < width && !this.image.get_Renamed(index2, num2));
                  --index2;
                }
                index1 = 0;
                stateCount[0] = 0;
                stateCount[1] = 0;
                stateCount[2] = 0;
                stateCount[3] = 0;
                stateCount[4] = 0;
              }
              else
              {
                stateCount[0] = stateCount[2];
                stateCount[1] = stateCount[3];
                stateCount[2] = stateCount[4];
                stateCount[3] = 1;
                stateCount[4] = 0;
                index1 = 3;
              }
            }
            else
              ++stateCount[++index1];
          }
          else
            ++stateCount[index1];
        }
        if (FinderPatternFinder.foundPatternCross(stateCount) && this.handlePossibleCenter(stateCount, num2, width))
        {
          num1 = stateCount[0];
          if (this.hasSkipped)
            flag2 = this.haveMultiplyConfirmedCenters();
        }
        num2 += num1;
      }
      FinderPattern[] patternCenters = this.selectBestPatterns();
      ResultPoint.orderBestPatterns((ResultPoint[]) patternCenters);
      return new FinderPatternInfo(patternCenters);
    }

    /// <summary>
    /// Given a count of black/white/black/white/black pixels just seen and an end position,
    ///             figures the location of the center of this run.
    /// 
    /// </summary>
    private static float centerFromEnd(int[] stateCount, int end)
    {
      return (float) (end - stateCount[4] - stateCount[3]) - (float) stateCount[2] / 2f;
    }

    /// <param name="stateCount">count of black/white/black/white/black pixels just read
    ///             </param>
    /// <returns>
    /// true iff the proportions of the counts is close enough to the 1/1/3/1/1 ratios
    ///             used by finder patterns to be considered a match
    /// 
    /// </returns>
    protected internal static bool foundPatternCross(int[] stateCount)
    {
      int num1 = 0;
      for (int index = 0; index < 5; ++index)
      {
        int num2 = stateCount[index];
        if (num2 == 0)
          return false;
        num1 += num2;
      }
      if (num1 < 7)
        return false;
      int num3 = (num1 << 8) / 7;
      int num4 = num3 / 2;
      if (Math.Abs(num3 - (stateCount[0] << 8)) < num4 && Math.Abs(num3 - (stateCount[1] << 8)) < num4 && (Math.Abs(3 * num3 - (stateCount[2] << 8)) < 3 * num4 && Math.Abs(num3 - (stateCount[3] << 8)) < num4))
        return Math.Abs(num3 - (stateCount[4] << 8)) < num4;
      return false;
    }

    /// <summary>
    /// <p>After a horizontal scan finds a potential finder pattern, this method
    ///             "cross-checks" by scanning down vertically through the center of the possible
    ///             finder pattern to see if the same proportion is detected.</p>
    /// </summary>
    /// <param name="startI">row where a finder pattern was detected
    ///             </param><param name="centerJ">center of the section that appears to cross a finder pattern
    ///             </param><param name="maxCount">maximum reasonable number of modules that should be
    ///             observed in any reading state, based on the results of the horizontal scan
    ///             </param>
    /// <returns>
    /// vertical center of finder pattern, or {@link Float#NaN} if not found
    /// 
    /// </returns>
    private float crossCheckVertical(int startI, int centerJ, int maxCount, int originalStateCountTotal)
    {
      BitMatrix bitMatrix = this.image;
      int height = bitMatrix.Height;
      int[] crossCheckStateCount = this.CrossCheckStateCount;
      int y;
      for (y = startI; y >= 0 && bitMatrix.get_Renamed(centerJ, y); --y)
        ++crossCheckStateCount[2];
      if (y < 0)
        return float.NaN;
      for (; y >= 0 && !bitMatrix.get_Renamed(centerJ, y) && crossCheckStateCount[1] <= maxCount; --y)
        ++crossCheckStateCount[1];
      if (y < 0 || crossCheckStateCount[1] > maxCount)
        return float.NaN;
      for (; y >= 0 && bitMatrix.get_Renamed(centerJ, y) && crossCheckStateCount[0] <= maxCount; --y)
        ++crossCheckStateCount[0];
      if (crossCheckStateCount[0] > maxCount)
        return float.NaN;
      int num;
      for (num = startI + 1; num < height && bitMatrix.get_Renamed(centerJ, num); ++num)
        ++crossCheckStateCount[2];
      if (num == height)
        return float.NaN;
      for (; num < height && !bitMatrix.get_Renamed(centerJ, num) && crossCheckStateCount[3] < maxCount; ++num)
        ++crossCheckStateCount[3];
      if (num == height || crossCheckStateCount[3] >= maxCount)
        return float.NaN;
      for (; num < height && bitMatrix.get_Renamed(centerJ, num) && crossCheckStateCount[4] < maxCount; ++num)
        ++crossCheckStateCount[4];
      if (crossCheckStateCount[4] >= maxCount || 5 * Math.Abs(crossCheckStateCount[0] + crossCheckStateCount[1] + crossCheckStateCount[2] + crossCheckStateCount[3] + crossCheckStateCount[4] - originalStateCountTotal) >= 2 * originalStateCountTotal || !FinderPatternFinder.foundPatternCross(crossCheckStateCount))
        return float.NaN;
      return FinderPatternFinder.centerFromEnd(crossCheckStateCount, num);
    }

    /// <summary>
    /// <p>Like {@link #crossCheckVertical(int, int, int, int)}, and in fact is basically identical,
    ///             except it reads horizontally instead of vertically. This is used to cross-cross
    ///             check a vertical cross check and locate the real center of the alignment pattern.</p>
    /// </summary>
    private float crossCheckHorizontal(int startJ, int centerI, int maxCount, int originalStateCountTotal)
    {
      BitMatrix bitMatrix = this.image;
      int width = bitMatrix.Width;
      int[] crossCheckStateCount = this.CrossCheckStateCount;
      int x;
      for (x = startJ; x >= 0 && bitMatrix.get_Renamed(x, centerI); --x)
        ++crossCheckStateCount[2];
      if (x < 0)
        return float.NaN;
      for (; x >= 0 && !bitMatrix.get_Renamed(x, centerI) && crossCheckStateCount[1] <= maxCount; --x)
        ++crossCheckStateCount[1];
      if (x < 0 || crossCheckStateCount[1] > maxCount)
        return float.NaN;
      for (; x >= 0 && bitMatrix.get_Renamed(x, centerI) && crossCheckStateCount[0] <= maxCount; --x)
        ++crossCheckStateCount[0];
      if (crossCheckStateCount[0] > maxCount)
        return float.NaN;
      int num;
      for (num = startJ + 1; num < width && bitMatrix.get_Renamed(num, centerI); ++num)
        ++crossCheckStateCount[2];
      if (num == width)
        return float.NaN;
      for (; num < width && !bitMatrix.get_Renamed(num, centerI) && crossCheckStateCount[3] < maxCount; ++num)
        ++crossCheckStateCount[3];
      if (num == width || crossCheckStateCount[3] >= maxCount)
        return float.NaN;
      for (; num < width && bitMatrix.get_Renamed(num, centerI) && crossCheckStateCount[4] < maxCount; ++num)
        ++crossCheckStateCount[4];
      if (crossCheckStateCount[4] >= maxCount || 5 * Math.Abs(crossCheckStateCount[0] + crossCheckStateCount[1] + crossCheckStateCount[2] + crossCheckStateCount[3] + crossCheckStateCount[4] - originalStateCountTotal) >= originalStateCountTotal || !FinderPatternFinder.foundPatternCross(crossCheckStateCount))
        return float.NaN;
      return FinderPatternFinder.centerFromEnd(crossCheckStateCount, num);
    }

    protected internal virtual bool handlePossibleCenter(int[] stateCount, int i, int j)
    {
      int originalStateCountTotal = stateCount[0] + stateCount[1] + stateCount[2] + stateCount[3] + stateCount[4];
      float num1 = FinderPatternFinder.centerFromEnd(stateCount, j);
      float num2 = this.crossCheckVertical(i, (int) num1, stateCount[2], originalStateCountTotal);
      if (!float.IsNaN(num2))
      {
        float num3 = this.crossCheckHorizontal((int) num1, (int) num2, stateCount[2], originalStateCountTotal);
        if (!float.IsNaN(num3))
        {
          float num4 = (float) originalStateCountTotal / 7f;
          bool flag = false;
          int count = this.possibleCenters.Count;
          for (int index = 0; index < count; ++index)
          {
            FinderPattern finderPattern = (FinderPattern) this.possibleCenters[index];
            if (finderPattern.aboutEquals(num4, num2, num3))
            {
              finderPattern.incrementCount();
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            ResultPoint point = (ResultPoint) new FinderPattern(num3, num2, num4);
            this.possibleCenters.Add((object) point);
            if (this.resultPointCallback != null)
              this.resultPointCallback.foundPossibleResultPoint(point);
          }
          return true;
        }
      }
      return false;
    }

    /// <returns>
    /// number of rows we could safely skip during scanning, based on the first
    ///             two finder patterns that have been located. In some cases their position will
    ///             allow us to infer that the third pattern must lie below a certain point farther
    ///             down in the image.
    /// 
    /// </returns>
    private int findRowSkip()
    {
      int count = this.possibleCenters.Count;
      if (count <= 1)
        return 0;
      FinderPattern finderPattern1 = (FinderPattern) null;
      for (int index = 0; index < count; ++index)
      {
        FinderPattern finderPattern2 = (FinderPattern) this.possibleCenters[index];
        if (finderPattern2.Count >= 2)
        {
          if (finderPattern1 == null)
          {
            finderPattern1 = finderPattern2;
          }
          else
          {
            this.hasSkipped = true;
            return (int) ((double) Math.Abs(finderPattern1.X - finderPattern2.X) - (double) Math.Abs(finderPattern1.Y - finderPattern2.Y)) / 2;
          }
        }
      }
      return 0;
    }

    /// <returns>
    /// true iff we have found at least 3 finder patterns that have been detected
    ///             at least {@link #CENTER_QUORUM} times each, and, the estimated module size of the
    ///             candidates is "pretty similar"
    /// 
    /// </returns>
    private bool haveMultiplyConfirmedCenters()
    {
      int num1 = 0;
      float num2 = 0.0f;
      int count = this.possibleCenters.Count;
      for (int index = 0; index < count; ++index)
      {
        FinderPattern finderPattern = (FinderPattern) this.possibleCenters[index];
        if (finderPattern.Count >= 2)
        {
          ++num1;
          num2 += finderPattern.EstimatedModuleSize;
        }
      }
      if (num1 < 3)
        return false;
      float num3 = num2 / (float) count;
      float num4 = 0.0f;
      for (int index = 0; index < count; ++index)
      {
        FinderPattern finderPattern = (FinderPattern) this.possibleCenters[index];
        num4 += Math.Abs(finderPattern.EstimatedModuleSize - num3);
      }
      return (double) num4 <= 0.0500000007450581 * (double) num2;
    }

    /// <returns>
    /// the 3 best {@link FinderPattern}s from our list of candidates. The "best" are
    ///             those that have been detected at least {@link #CENTER_QUORUM} times, and whose module
    ///             size differs from the average among those patterns the least
    /// 
    /// </returns>
    /// <throws>ReaderException if 3 such finder patterns do not exist </throws>
    private FinderPattern[] selectBestPatterns()
    {
      int count = this.possibleCenters.Count;
      if (count < 3)
        throw ReaderException.Instance;
      if (count > 3)
      {
        float num1 = 0.0f;
        for (int index = 0; index < count; ++index)
          num1 += ((FinderPattern) this.possibleCenters[index]).EstimatedModuleSize;
        float num2 = num1 / (float) count;
        for (int index = 0; index < this.possibleCenters.Count && this.possibleCenters.Count > 3; ++index)
        {
          if ((double) Math.Abs(((FinderPattern) this.possibleCenters[index]).EstimatedModuleSize - num2) > 0.200000002980232 * (double) num2)
          {
            this.possibleCenters.RemoveAt(index);
            --index;
          }
        }
      }
      if (this.possibleCenters.Count > 3)
      {
        Collections.insertionSort(this.possibleCenters, (Comparator) new FinderPatternFinder.CenterComparator());
        SupportClass.SetCapacity(this.possibleCenters, 3);
      }
      return new FinderPattern[3]
      {
        (FinderPattern) this.possibleCenters[0],
        (FinderPattern) this.possibleCenters[1],
        (FinderPattern) this.possibleCenters[2]
      };
    }

    /// <summary>
    /// <p>Orders by {@link FinderPattern#getCount()}, descending.</p>
    /// </summary>
    private class CenterComparator : Comparator
    {
      public virtual int compare(object center1, object center2)
      {
        return ((FinderPattern) center2).Count - ((FinderPattern) center1).Count;
      }
    }
  }
}
