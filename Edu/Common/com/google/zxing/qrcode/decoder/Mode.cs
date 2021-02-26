// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.decoder.Mode
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.qrcode.decoder
{
  /// <summary>
  /// <p>See ISO 18004:2006, 6.4.1, Tables 2 and 3. This enum encapsulates the various modes in which
  ///             data can be encoded to bits in the QR code standard.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Mode
  {
    public static readonly Mode TERMINATOR = new Mode(new int[3], 0, "TERMINATOR");
    public static readonly Mode NUMERIC = new Mode(new int[3]
    {
      10,
      12,
      14
    }, 1, "NUMERIC");
    public static readonly Mode ALPHANUMERIC = new Mode(new int[3]
    {
      9,
      11,
      13
    }, 2, "ALPHANUMERIC");
    public static readonly Mode STRUCTURED_APPEND = new Mode(new int[3], 3, "STRUCTURED_APPEND");
    public static readonly Mode BYTE = new Mode(new int[3]
    {
      8,
      16,
      16
    }, 4, "BYTE");
    public static readonly Mode ECI = new Mode((int[]) null, 7, "ECI");
    public static readonly Mode KANJI = new Mode(new int[3]
    {
      8,
      10,
      12
    }, 8, "KANJI");
    public static readonly Mode FNC1_FIRST_POSITION = new Mode((int[]) null, 5, "FNC1_FIRST_POSITION");
    public static readonly Mode FNC1_SECOND_POSITION = new Mode((int[]) null, 9, "FNC1_SECOND_POSITION");
    private int[] characterCountBitsForVersions;
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

    private Mode(int[] characterCountBitsForVersions, int bits, string name)
    {
      this.characterCountBitsForVersions = characterCountBitsForVersions;
      this.bits = bits;
      this.name = name;
    }

    /// <param name="bits">four bits encoding a QR Code data mode
    ///             </param>
    /// <returns>
    /// {@link Mode} encoded by these bits
    /// 
    /// </returns>
    /// <throws>IllegalArgumentException if bits do not correspond to a known mode </throws>
    public static Mode forBits(int bits)
    {
      switch (bits)
      {
        case 0:
          return Mode.TERMINATOR;
        case 1:
          return Mode.NUMERIC;
        case 2:
          return Mode.ALPHANUMERIC;
        case 3:
          return Mode.STRUCTURED_APPEND;
        case 4:
          return Mode.BYTE;
        case 5:
          return Mode.FNC1_FIRST_POSITION;
        case 7:
          return Mode.ECI;
        case 8:
          return Mode.KANJI;
        case 9:
          return Mode.FNC1_SECOND_POSITION;
        default:
          throw new ArgumentException();
      }
    }

    /// <param name="version">version in question
    ///             </param>
    /// <returns>
    /// number of bits used, in this QR Code symbol {@link Version}, to encode the
    ///             count of characters that will follow encoded in this {@link Mode}
    /// 
    /// </returns>
    public int getCharacterCountBits(Version version)
    {
      if (this.characterCountBitsForVersions == null)
        throw new ArgumentException("Character count doesn't apply to this mode");
      int versionNumber = version.VersionNumber;
      return this.characterCountBitsForVersions[versionNumber > 9 ? (versionNumber > 26 ? 2 : 1) : 0];
    }

    public override string ToString()
    {
      return this.name;
    }
  }
}
