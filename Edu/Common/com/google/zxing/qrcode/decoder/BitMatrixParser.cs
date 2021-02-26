// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.decoder.BitMatrixParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;

namespace com.google.zxing.qrcode.decoder
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class BitMatrixParser
  {
    private BitMatrix bitMatrix;
    private Version parsedVersion;
    private FormatInformation parsedFormatInfo;

    /// <param name="bitMatrix">{@link BitMatrix} to parse
    ///             </param><throws>ReaderException if dimension is not &gt;= 21 and 1 mod 4 </throws>
    internal BitMatrixParser(BitMatrix bitMatrix)
    {
      int dimension = bitMatrix.Dimension;
      if (dimension < 21 || (dimension & 3) != 1)
        throw ReaderException.Instance;
      this.bitMatrix = bitMatrix;
    }

    /// <summary>
    /// <p>Reads format information from one of its two locations within the QR Code.</p>
    /// </summary>
    /// 
    /// <returns>
    /// {@link FormatInformation} encapsulating the QR Code's format info
    /// 
    /// </returns>
    /// <throws>ReaderException if both format information locations cannot be parsed as </throws>
    /// <summary>
    /// the valid encoding of format information
    /// 
    /// </summary>
    internal FormatInformation readFormatInformation()
    {
      if (this.parsedFormatInfo != null)
        return this.parsedFormatInfo;
      int versionBits = 0;
      for (int i = 0; i < 6; ++i)
        versionBits = this.copyBit(i, 8, versionBits);
      int num1 = this.copyBit(8, 7, this.copyBit(8, 8, this.copyBit(7, 8, versionBits)));
      for (int j = 5; j >= 0; --j)
        num1 = this.copyBit(8, j, num1);
      this.parsedFormatInfo = FormatInformation.decodeFormatInformation(num1);
      if (this.parsedFormatInfo != null)
        return this.parsedFormatInfo;
      int dimension = this.bitMatrix.Dimension;
      int num2 = 0;
      int num3 = dimension - 8;
      for (int i = dimension - 1; i >= num3; --i)
        num2 = this.copyBit(i, 8, num2);
      for (int j = dimension - 7; j < dimension; ++j)
        num2 = this.copyBit(8, j, num2);
      this.parsedFormatInfo = FormatInformation.decodeFormatInformation(num2);
      if (this.parsedFormatInfo != null)
        return this.parsedFormatInfo;
      throw ReaderException.Instance;
    }

    /// <summary>
    /// <p>Reads version information from one of its two locations within the QR Code.</p>
    /// </summary>
    /// 
    /// <returns>
    /// {@link Version} encapsulating the QR Code's version
    /// 
    /// </returns>
    /// <throws>ReaderException if both version information locations cannot be parsed as </throws>
    /// <summary>
    /// the valid encoding of version information
    /// 
    /// </summary>
    internal Version readVersion()
    {
      if (this.parsedVersion != null)
        return this.parsedVersion;
      int dimension = this.bitMatrix.Dimension;
      int versionNumber = dimension - 17 >> 2;
      if (versionNumber <= 6)
        return Version.getVersionForNumber(versionNumber);
      int versionBits1 = 0;
      int num = dimension - 11;
      for (int j = 5; j >= 0; --j)
      {
        for (int i = dimension - 9; i >= num; --i)
          versionBits1 = this.copyBit(i, j, versionBits1);
      }
      this.parsedVersion = Version.decodeVersionInformation(versionBits1);
      if (this.parsedVersion != null && this.parsedVersion.DimensionForVersion == dimension)
        return this.parsedVersion;
      int versionBits2 = 0;
      for (int i = 5; i >= 0; --i)
      {
        for (int j = dimension - 9; j >= num; --j)
          versionBits2 = this.copyBit(i, j, versionBits2);
      }
      this.parsedVersion = Version.decodeVersionInformation(versionBits2);
      if (this.parsedVersion != null && this.parsedVersion.DimensionForVersion == dimension)
        return this.parsedVersion;
      throw ReaderException.Instance;
    }

    private int copyBit(int i, int j, int versionBits)
    {
      if (!this.bitMatrix.get_Renamed(i, j))
        return versionBits << 1;
      return versionBits << 1 | 1;
    }

    /// <summary>
    /// <p>Reads the bits in the {@link BitMatrix} representing the finder pattern in the
    ///             correct order in order to reconstitute the codewords bytes contained within the
    ///             QR Code.</p>
    /// </summary>
    /// 
    /// <returns>
    /// bytes encoded within the QR Code
    /// 
    /// </returns>
    /// <throws>ReaderException if the exact number of bytes expected is not read </throws>
    internal sbyte[] readCodewords()
    {
      FormatInformation formatInformation = this.readFormatInformation();
      Version version = this.readVersion();
      DataMask dataMask = DataMask.forReference((int) formatInformation.DataMask);
      int dimension = this.bitMatrix.Dimension;
      dataMask.unmaskBitMatrix(this.bitMatrix, dimension);
      BitMatrix bitMatrix = version.buildFunctionPattern();
      bool flag = true;
      sbyte[] numArray = new sbyte[version.TotalCodewords];
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = dimension - 1;
      while (num4 > 0)
      {
        if (num4 == 6)
          --num4;
        for (int index1 = 0; index1 < dimension; ++index1)
        {
          int y = flag ? dimension - 1 - index1 : index1;
          for (int index2 = 0; index2 < 2; ++index2)
          {
            if (!bitMatrix.get_Renamed(num4 - index2, y))
            {
              ++num3;
              num2 <<= 1;
              if (this.bitMatrix.get_Renamed(num4 - index2, y))
                num2 |= 1;
              if (num3 == 8)
              {
                numArray[num1++] = (sbyte) num2;
                num3 = 0;
                num2 = 0;
              }
            }
          }
        }
        flag = !flag;
        num4 -= 2;
      }
      if (num1 != version.TotalCodewords)
        throw ReaderException.Instance;
      return numArray;
    }
  }
}
