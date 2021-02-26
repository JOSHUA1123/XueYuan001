// Decompiled with JetBrains decompiler
// Type: com.google.zxing.Reader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Collections;

namespace com.google.zxing
{
  /// <summary>
  /// Implementations of this interface can decode an image of a barcode in some format into
  ///             the String it encodes. For example, {@link com.google.zxing.qrcode.QRCodeReader} can
  ///             decode a QR code. The decoder may optionally receive hints from the caller which may help
  ///             it decode more quickly or accurately.
  /// 
  ///             See {@link com.google.zxing.MultiFormatReader}, which attempts to determine what barcode
  ///             format is present within the image as well, and then decodes it accordingly.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public interface Reader
  {
    /// <summary>
    /// Locates and decodes a barcode in some format within an image.
    /// 
    /// 
    /// </summary>
    /// <param name="image">image of barcode to decode
    ///             </param>
    /// <returns>
    /// String which the barcode encodes
    /// 
    /// </returns>
    /// <throws>ReaderException if the barcode cannot be located or decoded for any reason </throws>
    Result decode(BinaryBitmap image);

    /// <summary>
    /// Locates and decodes a barcode in some format within an image. This method also accepts
    ///             hints, each possibly associated to some data, which may help the implementation decode.
    /// 
    /// 
    /// </summary>
    /// <param name="image">image of barcode to decode
    ///             </param><param name="hints">passed as a {@link java.util.Hashtable} from {@link com.google.zxing.DecodeHintType}
    ///             to arbitrary data. The
    ///             meaning of the data depends upon the hint type. The implementation may or may not do
    ///             anything with these hints.
    ///             </param>
    /// <returns>
    /// String which the barcode encodes
    /// 
    /// </returns>
    /// <throws>ReaderException if the barcode cannot be located or decoded for any reason </throws>
    Result decode(BinaryBitmap image, Hashtable hints);
  }
}
