// Decompiled with JetBrains decompiler
// Type: com.google.zxing.Writer
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.common;
using System.Collections;

namespace com.google.zxing
{
  /// <summary>
  /// The base class for all objects which encode/generate a barcode image.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public interface Writer
  {
    /// <summary>
    /// Encode a barcode using the default settings.
    /// 
    /// 
    /// </summary>
    /// <param name="contents">The contents to encode in the barcode
    ///             </param><param name="format">The barcode format to generate
    ///             </param><param name="width">The preferred width in pixels
    ///             </param><param name="height">The preferred height in pixels
    ///             </param>
    /// <returns>
    /// The generated barcode as a Matrix of unsigned bytes (0 == black, 255 == white)
    /// 
    /// </returns>
    ByteMatrix encode(string contents, BarcodeFormat format, int width, int height);

    /// <summary/>
    /// <param name="contents">The contents to encode in the barcode
    ///             </param><param name="format">The barcode format to generate
    ///             </param><param name="width">The preferred width in pixels
    ///             </param><param name="height">The preferred height in pixels
    ///             </param><param name="hints">Additional parameters to supply to the encoder
    ///             </param>
    /// <returns>
    /// The generated barcode as a Matrix of unsigned bytes (0 == black, 255 == white)
    /// 
    /// </returns>
    ByteMatrix encode(string contents, BarcodeFormat format, int width, int height, Hashtable hints);
  }
}
