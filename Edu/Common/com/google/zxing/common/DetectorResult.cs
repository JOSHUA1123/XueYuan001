// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.DetectorResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.common
{
  /// <summary>
  /// <p>Encapsulates the result of detecting a barcode in an image. This includes the raw
  ///             matrix of black/white pixels corresponding to the barcode, and possibly points of interest
  ///             in the image, like the location of finder patterns or corners of the barcode in the image.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class DetectorResult
  {
    private BitMatrix bits;
    private ResultPoint[] points;

    public BitMatrix Bits
    {
      get
      {
        return this.bits;
      }
    }

    public ResultPoint[] Points
    {
      get
      {
        return this.points;
      }
    }

    public DetectorResult(BitMatrix bits, ResultPoint[] points)
    {
      this.bits = bits;
      this.points = points;
    }
  }
}
