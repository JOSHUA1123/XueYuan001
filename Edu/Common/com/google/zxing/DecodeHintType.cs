// Decompiled with JetBrains decompiler
// Type: com.google.zxing.DecodeHintType
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing
{
  /// <summary>
  /// Encapsulates a type of hint that a caller may pass to a barcode reader to help it
  ///             more quickly or accurately decode it. It is up to implementations to decide what,
  ///             if anything, to do with the information that is supplied.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author><seealso cref="!:Reader.decode(BinaryBitmap,java.util.Hashtable)"/>
  public sealed class DecodeHintType
  {
    /// <summary>
    /// Unspecified, application-specific hint. Maps to an unspecified {@link Object}.
    /// </summary>
    public static readonly DecodeHintType OTHER = new DecodeHintType();
    /// <summary>
    /// Image is a pure monochrome image of a barcode. Doesn't matter what it maps to;
    ///             use {@link Boolean#TRUE}.
    /// 
    /// </summary>
    public static readonly DecodeHintType PURE_BARCODE = new DecodeHintType();
    /// <summary>
    /// Image is known to be of one of a few possible formats.
    ///             Maps to a {@link java.util.Vector} of {@link BarcodeFormat}s.
    /// 
    /// </summary>
    public static readonly DecodeHintType POSSIBLE_FORMATS = new DecodeHintType();
    /// <summary>
    /// Spend more time to try to find a barcode; optimize for accuracy, not speed.
    ///             Doesn't matter what it maps to; use {@link Boolean#TRUE}.
    /// 
    /// </summary>
    public static readonly DecodeHintType TRY_HARDER = new DecodeHintType();
    /// <summary>
    /// Allowed lengths of encoded data -- reject anything else. Maps to an int[].
    /// </summary>
    public static readonly DecodeHintType ALLOWED_LENGTHS = new DecodeHintType();
    /// <summary>
    /// Assume Code 39 codes employ a check digit. Maps to {@link Boolean}.
    /// </summary>
    public static readonly DecodeHintType ASSUME_CODE_39_CHECK_DIGIT = new DecodeHintType();
    /// <summary>
    /// The caller needs to be notified via callback when a possible {@link ResultPoint}
    ///             is found. Maps to a {@link ResultPointCallback}.
    /// 
    /// </summary>
    public static readonly DecodeHintType NEED_RESULT_POINT_CALLBACK = new DecodeHintType();

    private DecodeHintType()
    {
    }
  }
}
