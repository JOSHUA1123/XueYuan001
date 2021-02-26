// Decompiled with JetBrains decompiler
// Type: com.google.zxing.multi.qrcode.detector.MultiFinderPatternFinder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.detector;
using System;
using System.Collections;

namespace com.google.zxing.multi.qrcode.detector
{
  internal sealed class MultiFinderPatternFinder : FinderPatternFinder
  {
    private static readonly FinderPatternInfo[] EMPTY_RESULT_ARRAY = new FinderPatternInfo[0];
    private const float MAX_MODULE_COUNT_PER_EDGE = 180f;
    private const float MIN_MODULE_COUNT_PER_EDGE = 9f;
    /// <summary>
    /// More or less arbitrary cutoff point for determining if two finder patterns might belong
    ///             to the same code if they differ less than DIFF_MODSIZE_CUTOFF_PERCENT percent in their
    ///             estimated modules sizes.
    /// 
    /// </summary>
    private const float DIFF_MODSIZE_CUTOFF_PERCENT = 0.05f;
    /// <summary>
    /// More or less arbitrary cutoff point for determining if two finder patterns might belong
    ///             to the same code if they differ less than DIFF_MODSIZE_CUTOFF pixels/module in their
    ///             estimated modules sizes.
    /// 
    /// </summary>
    private const float DIFF_MODSIZE_CUTOFF = 0.5f;

    /// <summary>
    /// <p>Creates a finder that will search the image for three finder patterns.</p>
    /// </summary>
    /// <param name="image">image to search
    ///             </param>
    internal MultiFinderPatternFinder(BitMatrix image)
      : base(image)
    {
    }

    internal MultiFinderPatternFinder(BitMatrix image, ResultPointCallback resultPointCallback)
      : base(image, resultPointCallback)
    {
    }

    /// <returns>
    /// the 3 best {@link FinderPattern}s from our list of candidates. The "best" are
    ///             those that have been detected at least {@link #CENTER_QUORUM} times, and whose module
    ///             size differs from the average among those patterns the least
    /// 
    /// </returns>
    /// <throws>ReaderException if 3 such finder patterns do not exist </throws>
    private FinderPattern[][] selectBestPatterns()
    {
      ArrayList possibleCenters = this.PossibleCenters;
      int count = possibleCenters.Count;
      if (count < 3)
        throw ReaderException.Instance;
      if (count == 3)
        return new FinderPattern[1][]
        {
          new FinderPattern[3]
          {
            (FinderPattern) possibleCenters[0],
            (FinderPattern) possibleCenters[1],
            (FinderPattern) possibleCenters[2]
          }
        };
      Collections.insertionSort(possibleCenters, (Comparator) new MultiFinderPatternFinder.ModuleSizeComparator());
      ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
      for (int index1 = 0; index1 < count - 2; ++index1)
      {
        FinderPattern finderPattern1 = (FinderPattern) possibleCenters[index1];
        if (finderPattern1 != null)
        {
          for (int index2 = index1 + 1; index2 < count - 1; ++index2)
          {
            FinderPattern finderPattern2 = (FinderPattern) possibleCenters[index2];
            if (finderPattern2 != null)
            {
              float num1 = (finderPattern1.EstimatedModuleSize - finderPattern2.EstimatedModuleSize) / Math.Min(finderPattern1.EstimatedModuleSize, finderPattern2.EstimatedModuleSize);
              if ((double) Math.Abs(finderPattern1.EstimatedModuleSize - finderPattern2.EstimatedModuleSize) <= 0.5 || (double) num1 < 0.0500000007450581)
              {
                for (int index3 = index2 + 1; index3 < count; ++index3)
                {
                  FinderPattern finderPattern3 = (FinderPattern) possibleCenters[index3];
                  if (finderPattern3 != null)
                  {
                    float num2 = (finderPattern2.EstimatedModuleSize - finderPattern3.EstimatedModuleSize) / Math.Min(finderPattern2.EstimatedModuleSize, finderPattern3.EstimatedModuleSize);
                    if ((double) Math.Abs(finderPattern2.EstimatedModuleSize - finderPattern3.EstimatedModuleSize) <= 0.5 || (double) num2 < 0.0500000007450581)
                    {
                      FinderPattern[] patternCenters = new FinderPattern[3]
                      {
                        finderPattern1,
                        finderPattern2,
                        finderPattern3
                      };
                      ResultPoint.orderBestPatterns((ResultPoint[]) patternCenters);
                      FinderPatternInfo finderPatternInfo = new FinderPatternInfo(patternCenters);
                      float val1_1 = ResultPoint.distance((ResultPoint) finderPatternInfo.TopLeft, (ResultPoint) finderPatternInfo.BottomLeft);
                      float val1_2 = ResultPoint.distance((ResultPoint) finderPatternInfo.TopRight, (ResultPoint) finderPatternInfo.BottomLeft);
                      float val2_1 = ResultPoint.distance((ResultPoint) finderPatternInfo.TopLeft, (ResultPoint) finderPatternInfo.TopRight);
                      float num3 = (float) (((double) val1_1 + (double) val2_1) / (double) finderPattern1.EstimatedModuleSize / 2.0);
                      if ((double) num3 <= 180.0 && (double) num3 >= 9.0 && (double) Math.Abs((val1_1 - val2_1) / Math.Min(val1_1, val2_1)) < 0.100000001490116)
                      {
                        float val2_2 = (float) Math.Sqrt((double) val1_1 * (double) val1_1 + (double) val2_1 * (double) val2_1);
                        if ((double) Math.Abs((val1_2 - val2_2) / Math.Min(val1_2, val2_2)) < 0.100000001490116)
                          arrayList.Add((object) patternCenters);
                      }
                    }
                    else
                      break;
                  }
                }
              }
              else
                break;
            }
          }
        }
      }
      if (arrayList.Count == 0)
        throw ReaderException.Instance;
      FinderPattern[][] finderPatternArray = new FinderPattern[arrayList.Count][];
      for (int index = 0; index < arrayList.Count; ++index)
        finderPatternArray[index] = (FinderPattern[]) arrayList[index];
      return finderPatternArray;
    }

    public FinderPatternInfo[] findMulti(Hashtable hints)
    {
      bool flag = hints != null && hints.ContainsKey((object) DecodeHintType.TRY_HARDER);
      BitMatrix image = this.Image;
      int height = image.Height;
      int width = image.Width;
      int num1 = (int) ((double) height / 228.0 * 3.0);
      if (num1 < 3 || flag)
        num1 = 3;
      int[] stateCount = new int[5];
      int num2 = num1 - 1;
      while (num2 < height)
      {
        stateCount[0] = 0;
        stateCount[1] = 0;
        stateCount[2] = 0;
        stateCount[3] = 0;
        stateCount[4] = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < width; ++index2)
        {
          if (image.get_Renamed(index2, num2))
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
                if (!this.handlePossibleCenter(stateCount, num2, index2))
                {
                  do
                  {
                    ++index2;
                  }
                  while (index2 < width && !image.get_Renamed(index2, num2));
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
        if (FinderPatternFinder.foundPatternCross(stateCount))
          this.handlePossibleCenter(stateCount, num2, width);
        num2 += num1;
      }
      FinderPattern[][] finderPatternArray = this.selectBestPatterns();
      ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
      for (int index = 0; index < finderPatternArray.Length; ++index)
      {
        FinderPattern[] patternCenters = finderPatternArray[index];
        ResultPoint.orderBestPatterns((ResultPoint[]) patternCenters);
        arrayList.Add((object) new FinderPatternInfo(patternCenters));
      }
      if (arrayList.Count == 0)
        return MultiFinderPatternFinder.EMPTY_RESULT_ARRAY;
      FinderPatternInfo[] finderPatternInfoArray = new FinderPatternInfo[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        finderPatternInfoArray[index] = (FinderPatternInfo) arrayList[index];
      return finderPatternInfoArray;
    }

    /// <summary>
    /// A comparator that orders FinderPatterns by their estimated module size.
    /// </summary>
    private class ModuleSizeComparator : Comparator
    {
      public int compare(object center1, object center2)
      {
        float num = ((FinderPattern) center2).EstimatedModuleSize - ((FinderPattern) center1).EstimatedModuleSize;
        if ((double) num < 0.0)
          return -1;
        return (double) num <= 0.0 ? 0 : 1;
      }
    }
  }
}
