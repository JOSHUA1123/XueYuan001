// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.decoder.ErrorCorrectionLevel
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.qrcode.decoder
{
  /// <summary>
  /// <p>See ISO 18004:2006, 6.5.1. This enum encapsulates the four error correction levels
  ///             defined by the QR code standard.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ErrorCorrectionLevel
  {
    /// <summary>
    /// L = ~7% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel L = new ErrorCorrectionLevel(0, 1, "L");
    /// <summary>
    /// M = ~15% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel M = new ErrorCorrectionLevel(1, 0, "M");
    /// <summary>
    /// Q = ~25% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel Q = new ErrorCorrectionLevel(2, 3, "Q");
    /// <summary>
    /// H = ~30% correction
    /// </summary>
    public static readonly ErrorCorrectionLevel H = new ErrorCorrectionLevel(3, 2, "H");
    private static readonly ErrorCorrectionLevel[] FOR_BITS = new ErrorCorrectionLevel[4]
    {
      ErrorCorrectionLevel.M,
      ErrorCorrectionLevel.L,
      ErrorCorrectionLevel.H,
      ErrorCorrectionLevel.Q
    };
    private int ordinal_Renamed_Field;
    private int bits;
    private string name;

    public int Bits
    {
      get
      {
        return this.bits;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    private ErrorCorrectionLevel(int ordinal, int bits, string name)
    {
      this.ordinal_Renamed_Field = ordinal;
      this.bits = bits;
      this.name = name;
    }

    public int ordinal()
    {
      return this.ordinal_Renamed_Field;
    }

    public override string ToString()
    {
      return this.name;
    }

    /// <param name="bits">int containing the two bits encoding a QR Code's error correction level
    ///             </param>
    /// <returns>
    /// {@link ErrorCorrectionLevel} representing the encoded error correction level
    /// 
    /// </returns>
    public static ErrorCorrectionLevel forBits(int bits)
    {
      if (bits < 0 || bits >= ErrorCorrectionLevel.FOR_BITS.Length)
        throw new ArgumentException();
      return ErrorCorrectionLevel.FOR_BITS[bits];
    }
  }
}
