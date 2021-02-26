// Decompiled with JetBrains decompiler
// Type: com.google.zxing.EncodeHintType
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing
{
  /// <summary>
  /// These are a set of hints that you may pass to Writers to specify their behavior.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class EncodeHintType
  {
    /// <summary>
    /// Specifies what degree of error correction to use, for example in QR Codes (type Integer).
    /// </summary>
    public static readonly EncodeHintType ERROR_CORRECTION = new EncodeHintType();
    /// <summary>
    /// Specifies what character encoding to use where applicable (type String)
    /// </summary>
    public static readonly EncodeHintType CHARACTER_SET = new EncodeHintType();

    private EncodeHintType()
    {
    }
  }
}
