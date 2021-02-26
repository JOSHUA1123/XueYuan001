// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.detector.FinderPatternInfo
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing.qrcode.detector
{
  /// <summary>
  /// <p>Encapsulates information about finder patterns in an image, including the location of
  ///             the three finder patterns, and their estimated module size.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class FinderPatternInfo
  {
    private FinderPattern bottomLeft;
    private FinderPattern topLeft;
    private FinderPattern topRight;

    public FinderPattern BottomLeft
    {
      get
      {
        return this.bottomLeft;
      }
    }

    public FinderPattern TopLeft
    {
      get
      {
        return this.topLeft;
      }
    }

    public FinderPattern TopRight
    {
      get
      {
        return this.topRight;
      }
    }

    public FinderPatternInfo(FinderPattern[] patternCenters)
    {
      this.bottomLeft = patternCenters[0];
      this.topLeft = patternCenters[1];
      this.topRight = patternCenters[2];
    }
  }
}
