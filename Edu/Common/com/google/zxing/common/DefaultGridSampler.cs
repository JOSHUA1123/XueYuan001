// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.DefaultGridSampler
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;

namespace com.google.zxing.common
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class DefaultGridSampler : GridSampler
  {
    public override BitMatrix sampleGrid(BitMatrix image, int dimension, float p1ToX, float p1ToY, float p2ToX, float p2ToY, float p3ToX, float p3ToY, float p4ToX, float p4ToY, float p1FromX, float p1FromY, float p2FromX, float p2FromY, float p3FromX, float p3FromY, float p4FromX, float p4FromY)
    {
      PerspectiveTransform transform = PerspectiveTransform.quadrilateralToQuadrilateral(p1ToX, p1ToY, p2ToX, p2ToY, p3ToX, p3ToY, p4ToX, p4ToY, p1FromX, p1FromY, p2FromX, p2FromY, p3FromX, p3FromY, p4FromX, p4FromY);
      return this.sampleGrid(image, dimension, transform);
    }

    public override BitMatrix sampleGrid(BitMatrix image, int dimension, PerspectiveTransform transform)
    {
      BitMatrix bitMatrix = new BitMatrix(dimension);
      float[] points = new float[dimension << 1];
      for (int y = 0; y < dimension; ++y)
      {
        int length = points.Length;
        float num = (float) y + 0.5f;
        int index1 = 0;
        while (index1 < length)
        {
          points[index1] = (float) (index1 >> 1) + 0.5f;
          points[index1 + 1] = num;
          index1 += 2;
        }
        transform.transformPoints(points);
        GridSampler.checkAndNudgePoints(image, points);
        try
        {
          int index2 = 0;
          while (index2 < length)
          {
            if (image.get_Renamed((int) points[index2], (int) points[index2 + 1]))
              bitMatrix.set_Renamed(index2 >> 1, y);
            index2 += 2;
          }
        }
        catch (IndexOutOfRangeException ex)
        {
          throw ReaderException.Instance;
        }
      }
      return bitMatrix;
    }
  }
}
