// Decompiled with JetBrains decompiler
// Type: com.google.zxing.datamatrix.detector.Detector
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.common.detector;
using System;
using System.Collections;

namespace com.google.zxing.datamatrix.detector
{
  /// <summary>
  /// <p>Encapsulates logic that can detect a Data Matrix Code in an image, even if the Data Matrix Code
  ///             is rotated or skewed, or partially obscured.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Detector
  {
    private static readonly int[] INTEGERS = new int[5]
    {
      0,
      1,
      2,
      3,
      4
    };
    private BitMatrix image;
    private MonochromeRectangleDetector rectangleDetector;

    public Detector(BitMatrix image)
    {
      this.image = image;
      this.rectangleDetector = new MonochromeRectangleDetector(image);
    }

    /// <summary>
    /// <p>Detects a Data Matrix Code in an image.</p>
    /// </summary>
    /// 
    /// <returns>
    /// {@link DetectorResult} encapsulating results of detecting a QR Code
    /// 
    /// </returns>
    /// <throws>ReaderException if no Data Matrix Code can be found </throws>
    public DetectorResult detect()
    {
      ResultPoint[] resultPointArray = this.rectangleDetector.detect();
      ResultPoint from = resultPointArray[0];
      ResultPoint resultPoint1 = resultPointArray[1];
      ResultPoint resultPoint2 = resultPointArray[2];
      ResultPoint to1 = resultPointArray[3];
      ArrayList vector = ArrayList.Synchronized(new ArrayList(4));
      vector.Add((object) this.transitionsBetween(from, resultPoint1));
      vector.Add((object) this.transitionsBetween(from, resultPoint2));
      vector.Add((object) this.transitionsBetween(resultPoint1, to1));
      vector.Add((object) this.transitionsBetween(resultPoint2, to1));
      Collections.insertionSort(vector, (Comparator) new Detector.ResultPointsAndTransitionsComparator());
      Detector.ResultPointsAndTransitions pointsAndTransitions1 = (Detector.ResultPointsAndTransitions) vector[0];
      Detector.ResultPointsAndTransitions pointsAndTransitions2 = (Detector.ResultPointsAndTransitions) vector[1];
      Hashtable table = Hashtable.Synchronized(new Hashtable());
      Detector.increment(table, pointsAndTransitions1.From);
      Detector.increment(table, pointsAndTransitions1.To);
      Detector.increment(table, pointsAndTransitions2.From);
      Detector.increment(table, pointsAndTransitions2.To);
      ResultPoint resultPoint3 = (ResultPoint) null;
      ResultPoint resultPoint4 = (ResultPoint) null;
      ResultPoint resultPoint5 = (ResultPoint) null;
      foreach (ResultPoint resultPoint6 in (IEnumerable) table.Keys)
      {
        if ((int) table[(object) resultPoint6] == 2)
          resultPoint4 = resultPoint6;
        else if (resultPoint3 == null)
          resultPoint3 = resultPoint6;
        else
          resultPoint5 = resultPoint6;
      }
      if (resultPoint3 == null || resultPoint4 == null || resultPoint5 == null)
        throw ReaderException.Instance;
      ResultPoint[] patterns = new ResultPoint[3]
      {
        resultPoint3,
        resultPoint4,
        resultPoint5
      };
      ResultPoint.orderBestPatterns(patterns);
      ResultPoint resultPoint7 = patterns[0];
      ResultPoint bottomLeft = patterns[1];
      ResultPoint resultPoint8 = patterns[2];
      ResultPoint to2 = table.ContainsKey((object) from) ? (table.ContainsKey((object) resultPoint1) ? (table.ContainsKey((object) resultPoint2) ? to1 : resultPoint2) : resultPoint1) : from;
      int num = Math.Min(this.transitionsBetween(resultPoint8, to2).Transitions, this.transitionsBetween(resultPoint7, to2).Transitions);
      if ((num & 1) == 1)
        ++num;
      int dimension = num + 2;
      return new DetectorResult(Detector.sampleGrid(this.image, resultPoint8, bottomLeft, resultPoint7, dimension), new ResultPoint[4]
      {
        from,
        resultPoint1,
        resultPoint2,
        to1
      });
    }

    /// <summary>
    /// Increments the Integer associated with a key by one.
    /// </summary>
    private static void increment(Hashtable table, ResultPoint key)
    {
      int num = 0;
      try
      {
        if (table.Count > 0)
          num = (int) table[(object) key];
      }
      catch
      {
        num = 0;
      }
      table[(object) key] = (object) (num == 0 ? Detector.INTEGERS[1] : Detector.INTEGERS[num + 1]);
    }

    private static BitMatrix sampleGrid(BitMatrix image, ResultPoint topLeft, ResultPoint bottomLeft, ResultPoint bottomRight, int dimension)
    {
      float p2FromX = bottomRight.X - bottomLeft.X + topLeft.X;
      float p2FromY = bottomRight.Y - bottomLeft.Y + topLeft.Y;
      return GridSampler.Instance.sampleGrid(image, dimension, 0.0f, 0.0f, (float) dimension, 0.0f, (float) dimension, (float) dimension, 0.0f, (float) dimension, topLeft.X, topLeft.Y, p2FromX, p2FromY, bottomRight.X, bottomRight.Y, bottomLeft.X, bottomLeft.Y);
    }

    /// <summary>
    /// Counts the number of black/white transitions between two points, using something like Bresenham's algorithm.
    /// </summary>
    private Detector.ResultPointsAndTransitions transitionsBetween(ResultPoint from, ResultPoint to)
    {
      int num1 = (int) from.X;
      int num2 = (int) from.Y;
      int num3 = (int) to.X;
      int num4 = (int) to.Y;
      bool flag1 = Math.Abs(num4 - num2) > Math.Abs(num3 - num1);
      if (flag1)
      {
        int num5 = num1;
        num1 = num2;
        num2 = num5;
        int num6 = num3;
        num3 = num4;
        num4 = num6;
      }
      int num7 = Math.Abs(num3 - num1);
      int num8 = Math.Abs(num4 - num2);
      int num9 = -num7 >> 1;
      int num10 = num2 < num4 ? 1 : -1;
      int num11 = num1 < num3 ? 1 : -1;
      int transitions = 0;
      bool flag2 = this.image.get_Renamed(flag1 ? num2 : num1, flag1 ? num1 : num2);
      int num12 = num1;
      int num13 = num2;
      while (num12 != num3)
      {
        bool renamed = this.image.get_Renamed(flag1 ? num13 : num12, flag1 ? num12 : num13);
        if (renamed != flag2)
        {
          ++transitions;
          flag2 = renamed;
        }
        num9 += num8;
        if (num9 > 0)
        {
          if (num13 != num4)
          {
            num13 += num10;
            num9 -= num7;
          }
          else
            break;
        }
        num12 += num11;
      }
      return new Detector.ResultPointsAndTransitions(from, to, transitions);
    }

    /// <summary>
    /// Simply encapsulates two points and a number of transitions between them.
    /// </summary>
    private class ResultPointsAndTransitions
    {
      private ResultPoint from;
      private ResultPoint to;
      private int transitions;

      public ResultPoint From
      {
        get
        {
          return this.from;
        }
      }

      public ResultPoint To
      {
        get
        {
          return this.to;
        }
      }

      public int Transitions
      {
        get
        {
          return this.transitions;
        }
      }

      internal ResultPointsAndTransitions(ResultPoint from, ResultPoint to, int transitions)
      {
        this.from = from;
        this.to = to;
        this.transitions = transitions;
      }

      public override string ToString()
      {
        return (string) (object) this.from + (object) "/" + (string) (object) this.to + (string) (object) '/' + (string) (object) this.transitions;
      }
    }

    /// <summary>
    /// Orders ResultPointsAndTransitions by number of transitions, ascending.
    /// </summary>
    private class ResultPointsAndTransitionsComparator : Comparator
    {
      public int compare(object o1, object o2)
      {
        return ((Detector.ResultPointsAndTransitions) o1).Transitions - ((Detector.ResultPointsAndTransitions) o2).Transitions;
      }
    }
  }
}
