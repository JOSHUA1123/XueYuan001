// Decompiled with JetBrains decompiler
// Type: com.google.zxing.ResultPoint
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Text;

namespace com.google.zxing
{
  /// <summary>
  /// <p>Encapsulates a point of interest in an image containing a barcode. Typically, this
  ///             would be the location of a finder pattern or the corner of the barcode, for example.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public class ResultPoint
  {
    private float x;
    private float y;

    public virtual float X
    {
      get
      {
        return this.x;
      }
    }

    public virtual float Y
    {
      get
      {
        return this.y;
      }
    }

    public ResultPoint(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public override bool Equals(object other)
    {
      if (!(other is ResultPoint))
        return false;
      ResultPoint resultPoint = (ResultPoint) other;
      if ((double) this.x == (double) resultPoint.x)
        return (double) this.y == (double) resultPoint.y;
      return false;
    }

    public override int GetHashCode()
    {
      return 0;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(25);
      stringBuilder.Append('(');
      stringBuilder.Append(this.x);
      stringBuilder.Append(',');
      stringBuilder.Append(this.y);
      stringBuilder.Append(')');
      return stringBuilder.ToString();
    }

    public static void orderBestPatterns(ResultPoint[] patterns)
    {
      float num1 = ResultPoint.distance(patterns[0], patterns[1]);
      float num2 = ResultPoint.distance(patterns[1], patterns[2]);
      float num3 = ResultPoint.distance(patterns[0], patterns[2]);
      ResultPoint pointB;
      ResultPoint pointA;
      ResultPoint pointC;
      if ((double) num2 >= (double) num1 && (double) num2 >= (double) num3)
      {
        pointB = patterns[0];
        pointA = patterns[1];
        pointC = patterns[2];
      }
      else if ((double) num3 >= (double) num2 && (double) num3 >= (double) num1)
      {
        pointB = patterns[1];
        pointA = patterns[0];
        pointC = patterns[2];
      }
      else
      {
        pointB = patterns[2];
        pointA = patterns[0];
        pointC = patterns[1];
      }
      if ((double) ResultPoint.crossProductZ(pointA, pointB, pointC) < 0.0)
      {
        ResultPoint resultPoint = pointA;
        pointA = pointC;
        pointC = resultPoint;
      }
      patterns[0] = pointA;
      patterns[1] = pointB;
      patterns[2] = pointC;
    }

    /// <returns>
    /// distance between two points
    /// 
    /// </returns>
    public static float distance(ResultPoint pattern1, ResultPoint pattern2)
    {
      float num1 = pattern1.X - pattern2.X;
      float num2 = pattern1.Y - pattern2.Y;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    /// <summary>
    /// Returns the z component of the cross product between vectors BC and BA.
    /// </summary>
    private static float crossProductZ(ResultPoint pointA, ResultPoint pointB, ResultPoint pointC)
    {
      float num1 = pointB.x;
      float num2 = pointB.y;
      return (float) (((double) pointC.x - (double) num1) * ((double) pointA.y - (double) num2) - ((double) pointC.y - (double) num2) * ((double) pointA.x - (double) num1));
    }
  }
}
