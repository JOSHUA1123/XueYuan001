// Decompiled with JetBrains decompiler
// Type: com.google.zxing.MultiFormatWriter
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.common;
using com.google.zxing.oned;
using com.google.zxing.qrcode;
using System;
using System.Collections;

namespace com.google.zxing
{
  /// <summary>
  /// This is a factory class which finds the appropriate Writer subclass for the BarcodeFormat
  ///             requested and encodes the barcode with the supplied contents.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class MultiFormatWriter : Writer
  {
    public ByteMatrix encode(string contents, BarcodeFormat format, int width, int height)
    {
      return this.encode(contents, format, width, height, (Hashtable) null);
    }

    public ByteMatrix encode(string contents, BarcodeFormat format, int width, int height, Hashtable hints)
    {
      if (format == BarcodeFormat.EAN_8)
        return new EAN8Writer().encode(contents, format, width, height, hints);
      if (format == BarcodeFormat.EAN_13)
        return new EAN13Writer().encode(contents, format, width, height, hints);
      if (format == BarcodeFormat.QR_CODE)
        return new QRCodeWriter().encode(contents, format, width, height, hints);
      throw new ArgumentException("No encoder available for format " + (object) format);
    }
  }
}
