// Decompiled with JetBrains decompiler
// Type: com.google.zxing.ResultMetadataType
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing
{
  /// <summary>
  /// Represents some type of metadata about the result of the decoding that the decoder
  ///             wishes to communicate back to the caller.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ResultMetadataType
  {
    /// <summary>
    /// Unspecified, application-specific metadata. Maps to an unspecified {@link Object}.
    /// </summary>
    public static readonly ResultMetadataType OTHER = new ResultMetadataType();
    /// <summary>
    /// Denotes the likely approximate orientation of the barcode in the image. This value
    ///             is given as degrees rotated clockwise from the normal, upright orientation.
    ///             For example a 1D barcode which was found by reading top-to-bottom would be
    ///             said to have orientation "90". This key maps to an {@link Integer} whose
    ///             value is in the range [0,360).
    /// 
    /// </summary>
    public static readonly ResultMetadataType ORIENTATION = new ResultMetadataType();
    /// <summary>
    /// <p>2D barcode formats typically encode text, but allow for a sort of 'byte mode'
    ///             which is sometimes used to encode binary data. While {@link Result} makes available
    ///             the complete raw bytes in the barcode for these formats, it does not offer the bytes
    ///             from the byte segments alone.</p><p>This maps to a {@link java.util.Vector} of byte arrays corresponding to the
    ///             raw bytes in the byte segments in the barcode, in order.</p>
    /// </summary>
    public static readonly ResultMetadataType BYTE_SEGMENTS = new ResultMetadataType();
    /// <summary>
    /// Error correction level used, if applicable. The value type depends on the
    ///             format, but is typically a String.
    /// 
    /// </summary>
    public static readonly ResultMetadataType ERROR_CORRECTION_LEVEL = new ResultMetadataType();

    private ResultMetadataType()
    {
    }
  }
}
