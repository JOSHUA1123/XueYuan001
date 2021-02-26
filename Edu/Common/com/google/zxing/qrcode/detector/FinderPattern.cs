// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.detector.FinderPattern
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;

namespace com.google.zxing.qrcode.detector
{
  /// <summary>
  /// <p>Encapsulates a finder pattern, which are the three square patterns found in
  ///             the corners of QR Codes. It also encapsulates a count of similar finder patterns,
  ///             as a convenience to the finder's bookkeeping.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class FinderPattern : ResultPoint
  {
    private float estimatedModuleSize;
    private int count;

    public float EstimatedModuleSize
    {
      get
      {
        return this.estimatedModuleSize;
      }
    }

    internal int Count
    {
      get
      {
        return this.count;
      }
    }

    internal FinderPattern(float posX, float posY, float estimatedModuleSize)
      : base(posX, posY)
    {
      this.estimatedModuleSize = estimatedModuleSize;
      this.count = 1;
    }

    internal void incrementCount()
    {
      ++this.count;
    }

    /// <summary>
    /// <p>Determines if this finder pattern "about equals" a finder pattern at the stated
    ///             position and size -- meaning, it is at nearly the same center with nearly the same size.</p>
    /// </summary>
    internal bool aboutEquals(float moduleSize, float i, float j)
    {
      if ((double) Math.Abs(i - this.Y) > (double) moduleSize || (double) Math.Abs(j - this.X) > (double) moduleSize)
        return false;
      float num = Math.Abs(moduleSize - this.estimatedModuleSize);
      if ((double) num > 1.0)
        return (double) num / (double) this.estimatedModuleSize <= 1.0;
      return true;
    }
  }
}
