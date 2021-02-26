// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.DecoderResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.qrcode.decoder;
using System;
using System.Collections;

namespace com.google.zxing.common
{
  /// <summary>
  /// <p>Encapsulates the result of decoding a matrix of bits. This typically
  ///             applies to 2D barcode formats. For now it contains the raw bytes obtained,
  ///             as well as a String interpretation of those bytes, if applicable.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class DecoderResult
  {
    private sbyte[] rawBytes;
    private string text;
    private ArrayList byteSegments;
    private ErrorCorrectionLevel ecLevel;

    public sbyte[] RawBytes
    {
      get
      {
        return this.rawBytes;
      }
    }

    public string Text
    {
      get
      {
        return this.text;
      }
    }

    public ArrayList ByteSegments
    {
      get
      {
        return this.byteSegments;
      }
    }

    public ErrorCorrectionLevel ECLevel
    {
      get
      {
        return this.ecLevel;
      }
    }

    public DecoderResult(sbyte[] rawBytes, string text, ArrayList byteSegments, ErrorCorrectionLevel ecLevel)
    {
      if (rawBytes == null && text == null)
        throw new ArgumentException();
      this.rawBytes = rawBytes;
      this.text = text;
      this.byteSegments = byteSegments;
      this.ecLevel = ecLevel;
    }
  }
}
