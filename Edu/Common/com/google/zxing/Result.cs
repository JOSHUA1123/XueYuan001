// Decompiled with JetBrains decompiler
// Type: com.google.zxing.Result
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Collections;

namespace com.google.zxing
{
  /// <summary>
  /// <p>Encapsulates the result of decoding a barcode within an image.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Result
  {
    private string text;
    private sbyte[] rawBytes;
    private ResultPoint[] resultPoints;
    private BarcodeFormat format;
    private Hashtable resultMetadata;

    /// <returns>
    /// raw text encoded by the barcode, if applicable, otherwise
    /// <code>
    /// null
    /// </code>
    /// 
    /// </returns>
    public string Text
    {
      get
      {
        return this.text;
      }
    }

    /// <returns>
    /// raw bytes encoded by the barcode, if applicable, otherwise
    /// <code>
    /// null
    /// </code>
    /// 
    /// </returns>
    public sbyte[] RawBytes
    {
      get
      {
        return this.rawBytes;
      }
    }

    /// <returns>
    /// points related to the barcode in the image. These are typically points
    ///             identifying finder patterns or the corners of the barcode. The exact meaning is
    ///             specific to the type of barcode that was decoded.
    /// 
    /// </returns>
    public ResultPoint[] ResultPoints
    {
      get
      {
        return this.resultPoints;
      }
    }

    /// <returns>
    /// {@link BarcodeFormat} representing the format of the barcode that was decoded
    /// 
    /// </returns>
    public BarcodeFormat BarcodeFormat
    {
      get
      {
        return this.format;
      }
    }

    /// <returns>
    /// {@link Hashtable} mapping {@link ResultMetadataType} keys to values. May be
    /// 
    /// <code>
    /// null
    /// </code>
    /// . This contains optional metadata about what was detected about the barcode,
    ///             like orientation.
    /// 
    /// </returns>
    public Hashtable ResultMetadata
    {
      get
      {
        return this.resultMetadata;
      }
    }

    public Result(string text, sbyte[] rawBytes, ResultPoint[] resultPoints, BarcodeFormat format)
    {
      if (text == null && rawBytes == null)
        throw new ArgumentException("Text and bytes are null");
      this.text = text;
      this.rawBytes = rawBytes;
      this.resultPoints = resultPoints;
      this.format = format;
      this.resultMetadata = (Hashtable) null;
    }

    public void putMetadata(ResultMetadataType type, object value_Renamed)
    {
      if (this.resultMetadata == null)
        this.resultMetadata = Hashtable.Synchronized(new Hashtable(3));
      this.resultMetadata[(object) type] = value_Renamed;
    }

    public override string ToString()
    {
      if (this.text == null)
        return "[" + (object) this.rawBytes.Length + " bytes]";
      return this.text;
    }
  }
}
