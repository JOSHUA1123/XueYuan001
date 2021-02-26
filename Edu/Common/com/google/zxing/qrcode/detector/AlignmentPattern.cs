// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.detector.AlignmentPattern
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;

namespace com.google.zxing.qrcode.detector
{
  /// <summary>
  /// <p>Encapsulates an alignment pattern, which are the smaller square patterns found in
  ///             all but the simplest QR Codes.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class AlignmentPattern : ResultPoint
  {
    private float estimatedModuleSize;

    internal AlignmentPattern(float posX, float posY, float estimatedModuleSize)
      : base(posX, posY)
    {
      this.estimatedModuleSize = estimatedModuleSize;
    }

    /// <summary>
    /// <p>Determines if this alignment pattern "about equals" an alignment pattern at the stated
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
