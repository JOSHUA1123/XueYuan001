// Decompiled with JetBrains decompiler
// Type: com.google.zxing.BarcodeFormat
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Collections;

namespace com.google.zxing
{
  /// <summary>
  /// Enumerates barcode formats known to this package.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class BarcodeFormat
  {
    private static readonly Hashtable VALUES = Hashtable.Synchronized(new Hashtable());
    /// <summary>
    /// QR Code 2D barcode format.
    /// </summary>
    public static readonly BarcodeFormat QR_CODE = new BarcodeFormat("QR_CODE");
    /// <summary>
    /// DataMatrix 2D barcode format.
    /// </summary>
    public static readonly BarcodeFormat DATAMATRIX = new BarcodeFormat("DATAMATRIX");
    /// <summary>
    /// UPC-E 1D format.
    /// </summary>
    public static readonly BarcodeFormat UPC_E = new BarcodeFormat("UPC_E");
    /// <summary>
    /// UPC-A 1D format.
    /// </summary>
    public static readonly BarcodeFormat UPC_A = new BarcodeFormat("UPC_A");
    /// <summary>
    /// EAN-8 1D format.
    /// </summary>
    public static readonly BarcodeFormat EAN_8 = new BarcodeFormat("EAN_8");
    /// <summary>
    /// EAN-13 1D format.
    /// </summary>
    public static readonly BarcodeFormat EAN_13 = new BarcodeFormat("EAN_13");
    /// <summary>
    /// Code 128 1D format.
    /// </summary>
    public static readonly BarcodeFormat CODE_128 = new BarcodeFormat("CODE_128");
    /// <summary>
    /// Code 39 1D format.
    /// </summary>
    public static readonly BarcodeFormat CODE_39 = new BarcodeFormat("CODE_39");
    /// <summary>
    /// ITF (Interleaved Two of Five) 1D format.
    /// </summary>
    public static readonly BarcodeFormat ITF = new BarcodeFormat("ITF");
    /// <summary>
    /// PDF417 format.
    /// </summary>
    public static readonly BarcodeFormat PDF417 = new BarcodeFormat("PDF417");
    private string name;

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    private BarcodeFormat(string name)
    {
      this.name = name;
      BarcodeFormat.VALUES[(object) name] = (object) this;
    }

    public override string ToString()
    {
      return this.name;
    }

    public static BarcodeFormat valueOf(string name)
    {
      BarcodeFormat barcodeFormat = (BarcodeFormat) BarcodeFormat.VALUES[(object) name];
      if (barcodeFormat == null)
        throw new ArgumentException();
      return barcodeFormat;
    }
  }
}
