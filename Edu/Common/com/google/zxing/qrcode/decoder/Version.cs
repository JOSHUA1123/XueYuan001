// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.decoder.Version
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System;

namespace com.google.zxing.qrcode.decoder
{
  /// <summary>
  /// See ISO 18004:2006 Annex D
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Version
  {
    /// <summary>
    /// See ISO 18004:2006 Annex D.
    ///             Element i represents the raw version bits that specify version i + 7
    /// 
    /// </summary>
    private static readonly int[] VERSION_DECODE_INFO = new int[34]
    {
      31892,
      34236,
      39577,
      42195,
      48118,
      51042,
      55367,
      58893,
      63784,
      68472,
      70749,
      76311,
      79154,
      84390,
      87683,
      92361,
      96236,
      102084,
      102881,
      110507,
      110734,
      117786,
      119615,
      126325,
      127568,
      133589,
      136944,
      141498,
      145311,
      150283,
      152622,
      158308,
      161089,
      167017
    };
    private static readonly Version[] VERSIONS = Version.buildVersions();
    private int versionNumber;
    private int[] alignmentPatternCenters;
    private Version.ECBlocks[] ecBlocks;
    private int totalCodewords;

    public int VersionNumber
    {
      get
      {
        return this.versionNumber;
      }
    }

    public int[] AlignmentPatternCenters
    {
      get
      {
        return this.alignmentPatternCenters;
      }
    }

    public int TotalCodewords
    {
      get
      {
        return this.totalCodewords;
      }
    }

    public int DimensionForVersion
    {
      get
      {
        return 17 + 4 * this.versionNumber;
      }
    }

    private Version(int versionNumber, int[] alignmentPatternCenters, Version.ECBlocks ecBlocks1, Version.ECBlocks ecBlocks2, Version.ECBlocks ecBlocks3, Version.ECBlocks ecBlocks4)
    {
      this.versionNumber = versionNumber;
      this.alignmentPatternCenters = alignmentPatternCenters;
      this.ecBlocks = new Version.ECBlocks[4]
      {
        ecBlocks1,
        ecBlocks2,
        ecBlocks3,
        ecBlocks4
      };
      int num = 0;
      int codewordsPerBlock = ecBlocks1.ECCodewordsPerBlock;
      foreach (Version.ECB ecb in ecBlocks1.getECBlocks())
        num += ecb.Count * (ecb.DataCodewords + codewordsPerBlock);
      this.totalCodewords = num;
    }

    public Version.ECBlocks getECBlocksForLevel(ErrorCorrectionLevel ecLevel)
    {
      return this.ecBlocks[ecLevel.ordinal()];
    }

    /// <summary>
    /// <p>Deduces version information purely from QR Code dimensions.</p>
    /// </summary>
    /// <param name="dimension">dimension in modules
    ///             </param>
    /// <returns>
    /// {@link Version} for a QR Code of that dimension
    /// 
    /// </returns>
    /// <throws>ReaderException if dimension is not 1 mod 4 </throws>
    public static Version getProvisionalVersionForDimension(int dimension)
    {
      if (dimension % 4 != 1)
        throw ReaderException.Instance;
      try
      {
        return Version.getVersionForNumber(dimension - 17 >> 2);
      }
      catch (ArgumentException ex)
      {
        throw ReaderException.Instance;
      }
    }

    public static Version getVersionForNumber(int versionNumber)
    {
      if (versionNumber < 1 || versionNumber > 40)
        throw new ArgumentException();
      return Version.VERSIONS[versionNumber - 1];
    }

    internal static Version decodeVersionInformation(int versionBits)
    {
      int num1 = int.MaxValue;
      int versionNumber = 0;
      for (int index = 0; index < Version.VERSION_DECODE_INFO.Length; ++index)
      {
        int b = Version.VERSION_DECODE_INFO[index];
        if (b == versionBits)
          return Version.getVersionForNumber(index + 7);
        int num2 = FormatInformation.numBitsDiffering(versionBits, b);
        if (num2 < num1)
        {
          versionNumber = index + 7;
          num1 = num2;
        }
      }
      if (num1 <= 3)
        return Version.getVersionForNumber(versionNumber);
      return (Version) null;
    }

    /// <summary>
    /// See ISO 18004:2006 Annex E
    /// </summary>
    internal BitMatrix buildFunctionPattern()
    {
      int dimensionForVersion = this.DimensionForVersion;
      BitMatrix bitMatrix = new BitMatrix(dimensionForVersion);
      bitMatrix.setRegion(0, 0, 9, 9);
      bitMatrix.setRegion(dimensionForVersion - 8, 0, 8, 9);
      bitMatrix.setRegion(0, dimensionForVersion - 8, 9, 8);
      int length = this.alignmentPatternCenters.Length;
      for (int index1 = 0; index1 < length; ++index1)
      {
        int top = this.alignmentPatternCenters[index1] - 2;
        for (int index2 = 0; index2 < length; ++index2)
        {
          if ((index1 != 0 || index2 != 0 && index2 != length - 1) && (index1 != length - 1 || index2 != 0))
            bitMatrix.setRegion(this.alignmentPatternCenters[index2] - 2, top, 5, 5);
        }
      }
      bitMatrix.setRegion(6, 9, 1, dimensionForVersion - 17);
      bitMatrix.setRegion(9, 6, dimensionForVersion - 17, 1);
      if (this.versionNumber > 6)
      {
        bitMatrix.setRegion(dimensionForVersion - 11, 0, 3, 6);
        bitMatrix.setRegion(0, dimensionForVersion - 11, 6, 3);
      }
      return bitMatrix;
    }

    public override string ToString()
    {
      return Convert.ToString(this.versionNumber);
    }

    /// <summary>
    /// See ISO 18004:2006 6.5.1 Table 9
    /// </summary>
    private static Version[] buildVersions()
    {
      return new Version[40]
      {
        new Version(1, new int[0], new Version.ECBlocks(7, new Version.ECB(1, 19)), new Version.ECBlocks(10, new Version.ECB(1, 16)), new Version.ECBlocks(13, new Version.ECB(1, 13)), new Version.ECBlocks(17, new Version.ECB(1, 9))),
        new Version(2, new int[2]
        {
          6,
          18
        }, new Version.ECBlocks(10, new Version.ECB(1, 34)), new Version.ECBlocks(16, new Version.ECB(1, 28)), new Version.ECBlocks(22, new Version.ECB(1, 22)), new Version.ECBlocks(28, new Version.ECB(1, 16))),
        new Version(3, new int[2]
        {
          6,
          22
        }, new Version.ECBlocks(15, new Version.ECB(1, 55)), new Version.ECBlocks(26, new Version.ECB(1, 44)), new Version.ECBlocks(18, new Version.ECB(2, 17)), new Version.ECBlocks(22, new Version.ECB(2, 13))),
        new Version(4, new int[2]
        {
          6,
          26
        }, new Version.ECBlocks(20, new Version.ECB(1, 80)), new Version.ECBlocks(18, new Version.ECB(2, 32)), new Version.ECBlocks(26, new Version.ECB(2, 24)), new Version.ECBlocks(16, new Version.ECB(4, 9))),
        new Version(5, new int[2]
        {
          6,
          30
        }, new Version.ECBlocks(26, new Version.ECB(1, 108)), new Version.ECBlocks(24, new Version.ECB(2, 43)), new Version.ECBlocks(18, new Version.ECB(2, 15), new Version.ECB(2, 16)), new Version.ECBlocks(22, new Version.ECB(2, 11), new Version.ECB(2, 12))),
        new Version(6, new int[2]
        {
          6,
          34
        }, new Version.ECBlocks(18, new Version.ECB(2, 68)), new Version.ECBlocks(16, new Version.ECB(4, 27)), new Version.ECBlocks(24, new Version.ECB(4, 19)), new Version.ECBlocks(28, new Version.ECB(4, 15))),
        new Version(7, new int[3]
        {
          6,
          22,
          38
        }, new Version.ECBlocks(20, new Version.ECB(2, 78)), new Version.ECBlocks(18, new Version.ECB(4, 31)), new Version.ECBlocks(18, new Version.ECB(2, 14), new Version.ECB(4, 15)), new Version.ECBlocks(26, new Version.ECB(4, 13), new Version.ECB(1, 14))),
        new Version(8, new int[3]
        {
          6,
          24,
          42
        }, new Version.ECBlocks(24, new Version.ECB(2, 97)), new Version.ECBlocks(22, new Version.ECB(2, 38), new Version.ECB(2, 39)), new Version.ECBlocks(22, new Version.ECB(4, 18), new Version.ECB(2, 19)), new Version.ECBlocks(26, new Version.ECB(4, 14), new Version.ECB(2, 15))),
        new Version(9, new int[3]
        {
          6,
          26,
          46
        }, new Version.ECBlocks(30, new Version.ECB(2, 116)), new Version.ECBlocks(22, new Version.ECB(3, 36), new Version.ECB(2, 37)), new Version.ECBlocks(20, new Version.ECB(4, 16), new Version.ECB(4, 17)), new Version.ECBlocks(24, new Version.ECB(4, 12), new Version.ECB(4, 13))),
        new Version(10, new int[3]
        {
          6,
          28,
          50
        }, new Version.ECBlocks(18, new Version.ECB(2, 68), new Version.ECB(2, 69)), new Version.ECBlocks(26, new Version.ECB(4, 43), new Version.ECB(1, 44)), new Version.ECBlocks(24, new Version.ECB(6, 19), new Version.ECB(2, 20)), new Version.ECBlocks(28, new Version.ECB(6, 15), new Version.ECB(2, 16))),
        new Version(11, new int[3]
        {
          6,
          30,
          54
        }, new Version.ECBlocks(20, new Version.ECB(4, 81)), new Version.ECBlocks(30, new Version.ECB(1, 50), new Version.ECB(4, 51)), new Version.ECBlocks(28, new Version.ECB(4, 22), new Version.ECB(4, 23)), new Version.ECBlocks(24, new Version.ECB(3, 12), new Version.ECB(8, 13))),
        new Version(12, new int[3]
        {
          6,
          32,
          58
        }, new Version.ECBlocks(24, new Version.ECB(2, 92), new Version.ECB(2, 93)), new Version.ECBlocks(22, new Version.ECB(6, 36), new Version.ECB(2, 37)), new Version.ECBlocks(26, new Version.ECB(4, 20), new Version.ECB(6, 21)), new Version.ECBlocks(28, new Version.ECB(7, 14), new Version.ECB(4, 15))),
        new Version(13, new int[3]
        {
          6,
          34,
          62
        }, new Version.ECBlocks(26, new Version.ECB(4, 107)), new Version.ECBlocks(22, new Version.ECB(8, 37), new Version.ECB(1, 38)), new Version.ECBlocks(24, new Version.ECB(8, 20), new Version.ECB(4, 21)), new Version.ECBlocks(22, new Version.ECB(12, 11), new Version.ECB(4, 12))),
        new Version(14, new int[4]
        {
          6,
          26,
          46,
          66
        }, new Version.ECBlocks(30, new Version.ECB(3, 115), new Version.ECB(1, 116)), new Version.ECBlocks(24, new Version.ECB(4, 40), new Version.ECB(5, 41)), new Version.ECBlocks(20, new Version.ECB(11, 16), new Version.ECB(5, 17)), new Version.ECBlocks(24, new Version.ECB(11, 12), new Version.ECB(5, 13))),
        new Version(15, new int[4]
        {
          6,
          26,
          48,
          70
        }, new Version.ECBlocks(22, new Version.ECB(5, 87), new Version.ECB(1, 88)), new Version.ECBlocks(24, new Version.ECB(5, 41), new Version.ECB(5, 42)), new Version.ECBlocks(30, new Version.ECB(5, 24), new Version.ECB(7, 25)), new Version.ECBlocks(24, new Version.ECB(11, 12), new Version.ECB(7, 13))),
        new Version(16, new int[4]
        {
          6,
          26,
          50,
          74
        }, new Version.ECBlocks(24, new Version.ECB(5, 98), new Version.ECB(1, 99)), new Version.ECBlocks(28, new Version.ECB(7, 45), new Version.ECB(3, 46)), new Version.ECBlocks(24, new Version.ECB(15, 19), new Version.ECB(2, 20)), new Version.ECBlocks(30, new Version.ECB(3, 15), new Version.ECB(13, 16))),
        new Version(17, new int[4]
        {
          6,
          30,
          54,
          78
        }, new Version.ECBlocks(28, new Version.ECB(1, 107), new Version.ECB(5, 108)), new Version.ECBlocks(28, new Version.ECB(10, 46), new Version.ECB(1, 47)), new Version.ECBlocks(28, new Version.ECB(1, 22), new Version.ECB(15, 23)), new Version.ECBlocks(28, new Version.ECB(2, 14), new Version.ECB(17, 15))),
        new Version(18, new int[4]
        {
          6,
          30,
          56,
          82
        }, new Version.ECBlocks(30, new Version.ECB(5, 120), new Version.ECB(1, 121)), new Version.ECBlocks(26, new Version.ECB(9, 43), new Version.ECB(4, 44)), new Version.ECBlocks(28, new Version.ECB(17, 22), new Version.ECB(1, 23)), new Version.ECBlocks(28, new Version.ECB(2, 14), new Version.ECB(19, 15))),
        new Version(19, new int[4]
        {
          6,
          30,
          58,
          86
        }, new Version.ECBlocks(28, new Version.ECB(3, 113), new Version.ECB(4, 114)), new Version.ECBlocks(26, new Version.ECB(3, 44), new Version.ECB(11, 45)), new Version.ECBlocks(26, new Version.ECB(17, 21), new Version.ECB(4, 22)), new Version.ECBlocks(26, new Version.ECB(9, 13), new Version.ECB(16, 14))),
        new Version(20, new int[4]
        {
          6,
          34,
          62,
          90
        }, new Version.ECBlocks(28, new Version.ECB(3, 107), new Version.ECB(5, 108)), new Version.ECBlocks(26, new Version.ECB(3, 41), new Version.ECB(13, 42)), new Version.ECBlocks(30, new Version.ECB(15, 24), new Version.ECB(5, 25)), new Version.ECBlocks(28, new Version.ECB(15, 15), new Version.ECB(10, 16))),
        new Version(21, new int[5]
        {
          6,
          28,
          50,
          72,
          94
        }, new Version.ECBlocks(28, new Version.ECB(4, 116), new Version.ECB(4, 117)), new Version.ECBlocks(26, new Version.ECB(17, 42)), new Version.ECBlocks(28, new Version.ECB(17, 22), new Version.ECB(6, 23)), new Version.ECBlocks(30, new Version.ECB(19, 16), new Version.ECB(6, 17))),
        new Version(22, new int[5]
        {
          6,
          26,
          50,
          74,
          98
        }, new Version.ECBlocks(28, new Version.ECB(2, 111), new Version.ECB(7, 112)), new Version.ECBlocks(28, new Version.ECB(17, 46)), new Version.ECBlocks(30, new Version.ECB(7, 24), new Version.ECB(16, 25)), new Version.ECBlocks(24, new Version.ECB(34, 13))),
        new Version(23, new int[5]
        {
          6,
          30,
          54,
          74,
          102
        }, new Version.ECBlocks(30, new Version.ECB(4, 121), new Version.ECB(5, 122)), new Version.ECBlocks(28, new Version.ECB(4, 47), new Version.ECB(14, 48)), new Version.ECBlocks(30, new Version.ECB(11, 24), new Version.ECB(14, 25)), new Version.ECBlocks(30, new Version.ECB(16, 15), new Version.ECB(14, 16))),
        new Version(24, new int[5]
        {
          6,
          28,
          54,
          80,
          106
        }, new Version.ECBlocks(30, new Version.ECB(6, 117), new Version.ECB(4, 118)), new Version.ECBlocks(28, new Version.ECB(6, 45), new Version.ECB(14, 46)), new Version.ECBlocks(30, new Version.ECB(11, 24), new Version.ECB(16, 25)), new Version.ECBlocks(30, new Version.ECB(30, 16), new Version.ECB(2, 17))),
        new Version(25, new int[5]
        {
          6,
          32,
          58,
          84,
          110
        }, new Version.ECBlocks(26, new Version.ECB(8, 106), new Version.ECB(4, 107)), new Version.ECBlocks(28, new Version.ECB(8, 47), new Version.ECB(13, 48)), new Version.ECBlocks(30, new Version.ECB(7, 24), new Version.ECB(22, 25)), new Version.ECBlocks(30, new Version.ECB(22, 15), new Version.ECB(13, 16))),
        new Version(26, new int[5]
        {
          6,
          30,
          58,
          86,
          114
        }, new Version.ECBlocks(28, new Version.ECB(10, 114), new Version.ECB(2, 115)), new Version.ECBlocks(28, new Version.ECB(19, 46), new Version.ECB(4, 47)), new Version.ECBlocks(28, new Version.ECB(28, 22), new Version.ECB(6, 23)), new Version.ECBlocks(30, new Version.ECB(33, 16), new Version.ECB(4, 17))),
        new Version(27, new int[5]
        {
          6,
          34,
          62,
          90,
          118
        }, new Version.ECBlocks(30, new Version.ECB(8, 122), new Version.ECB(4, 123)), new Version.ECBlocks(28, new Version.ECB(22, 45), new Version.ECB(3, 46)), new Version.ECBlocks(30, new Version.ECB(8, 23), new Version.ECB(26, 24)), new Version.ECBlocks(30, new Version.ECB(12, 15), new Version.ECB(28, 16))),
        new Version(28, new int[6]
        {
          6,
          26,
          50,
          74,
          98,
          122
        }, new Version.ECBlocks(30, new Version.ECB(3, 117), new Version.ECB(10, 118)), new Version.ECBlocks(28, new Version.ECB(3, 45), new Version.ECB(23, 46)), new Version.ECBlocks(30, new Version.ECB(4, 24), new Version.ECB(31, 25)), new Version.ECBlocks(30, new Version.ECB(11, 15), new Version.ECB(31, 16))),
        new Version(29, new int[6]
        {
          6,
          30,
          54,
          78,
          102,
          126
        }, new Version.ECBlocks(30, new Version.ECB(7, 116), new Version.ECB(7, 117)), new Version.ECBlocks(28, new Version.ECB(21, 45), new Version.ECB(7, 46)), new Version.ECBlocks(30, new Version.ECB(1, 23), new Version.ECB(37, 24)), new Version.ECBlocks(30, new Version.ECB(19, 15), new Version.ECB(26, 16))),
        new Version(30, new int[6]
        {
          6,
          26,
          52,
          78,
          104,
          130
        }, new Version.ECBlocks(30, new Version.ECB(5, 115), new Version.ECB(10, 116)), new Version.ECBlocks(28, new Version.ECB(19, 47), new Version.ECB(10, 48)), new Version.ECBlocks(30, new Version.ECB(15, 24), new Version.ECB(25, 25)), new Version.ECBlocks(30, new Version.ECB(23, 15), new Version.ECB(25, 16))),
        new Version(31, new int[6]
        {
          6,
          30,
          56,
          82,
          108,
          134
        }, new Version.ECBlocks(30, new Version.ECB(13, 115), new Version.ECB(3, 116)), new Version.ECBlocks(28, new Version.ECB(2, 46), new Version.ECB(29, 47)), new Version.ECBlocks(30, new Version.ECB(42, 24), new Version.ECB(1, 25)), new Version.ECBlocks(30, new Version.ECB(23, 15), new Version.ECB(28, 16))),
        new Version(32, new int[6]
        {
          6,
          34,
          60,
          86,
          112,
          138
        }, new Version.ECBlocks(30, new Version.ECB(17, 115)), new Version.ECBlocks(28, new Version.ECB(10, 46), new Version.ECB(23, 47)), new Version.ECBlocks(30, new Version.ECB(10, 24), new Version.ECB(35, 25)), new Version.ECBlocks(30, new Version.ECB(19, 15), new Version.ECB(35, 16))),
        new Version(33, new int[6]
        {
          6,
          30,
          58,
          86,
          114,
          142
        }, new Version.ECBlocks(30, new Version.ECB(17, 115), new Version.ECB(1, 116)), new Version.ECBlocks(28, new Version.ECB(14, 46), new Version.ECB(21, 47)), new Version.ECBlocks(30, new Version.ECB(29, 24), new Version.ECB(19, 25)), new Version.ECBlocks(30, new Version.ECB(11, 15), new Version.ECB(46, 16))),
        new Version(34, new int[6]
        {
          6,
          34,
          62,
          90,
          118,
          146
        }, new Version.ECBlocks(30, new Version.ECB(13, 115), new Version.ECB(6, 116)), new Version.ECBlocks(28, new Version.ECB(14, 46), new Version.ECB(23, 47)), new Version.ECBlocks(30, new Version.ECB(44, 24), new Version.ECB(7, 25)), new Version.ECBlocks(30, new Version.ECB(59, 16), new Version.ECB(1, 17))),
        new Version(35, new int[7]
        {
          6,
          30,
          54,
          78,
          102,
          126,
          150
        }, new Version.ECBlocks(30, new Version.ECB(12, 121), new Version.ECB(7, 122)), new Version.ECBlocks(28, new Version.ECB(12, 47), new Version.ECB(26, 48)), new Version.ECBlocks(30, new Version.ECB(39, 24), new Version.ECB(14, 25)), new Version.ECBlocks(30, new Version.ECB(22, 15), new Version.ECB(41, 16))),
        new Version(36, new int[7]
        {
          6,
          24,
          50,
          76,
          102,
          128,
          154
        }, new Version.ECBlocks(30, new Version.ECB(6, 121), new Version.ECB(14, 122)), new Version.ECBlocks(28, new Version.ECB(6, 47), new Version.ECB(34, 48)), new Version.ECBlocks(30, new Version.ECB(46, 24), new Version.ECB(10, 25)), new Version.ECBlocks(30, new Version.ECB(2, 15), new Version.ECB(64, 16))),
        new Version(37, new int[7]
        {
          6,
          28,
          54,
          80,
          106,
          132,
          158
        }, new Version.ECBlocks(30, new Version.ECB(17, 122), new Version.ECB(4, 123)), new Version.ECBlocks(28, new Version.ECB(29, 46), new Version.ECB(14, 47)), new Version.ECBlocks(30, new Version.ECB(49, 24), new Version.ECB(10, 25)), new Version.ECBlocks(30, new Version.ECB(24, 15), new Version.ECB(46, 16))),
        new Version(38, new int[7]
        {
          6,
          32,
          58,
          84,
          110,
          136,
          162
        }, new Version.ECBlocks(30, new Version.ECB(4, 122), new Version.ECB(18, 123)), new Version.ECBlocks(28, new Version.ECB(13, 46), new Version.ECB(32, 47)), new Version.ECBlocks(30, new Version.ECB(48, 24), new Version.ECB(14, 25)), new Version.ECBlocks(30, new Version.ECB(42, 15), new Version.ECB(32, 16))),
        new Version(39, new int[7]
        {
          6,
          26,
          54,
          82,
          110,
          138,
          166
        }, new Version.ECBlocks(30, new Version.ECB(20, 117), new Version.ECB(4, 118)), new Version.ECBlocks(28, new Version.ECB(40, 47), new Version.ECB(7, 48)), new Version.ECBlocks(30, new Version.ECB(43, 24), new Version.ECB(22, 25)), new Version.ECBlocks(30, new Version.ECB(10, 15), new Version.ECB(67, 16))),
        new Version(40, new int[7]
        {
          6,
          30,
          58,
          86,
          114,
          142,
          170
        }, new Version.ECBlocks(30, new Version.ECB(19, 118), new Version.ECB(6, 119)), new Version.ECBlocks(28, new Version.ECB(18, 47), new Version.ECB(31, 48)), new Version.ECBlocks(30, new Version.ECB(34, 24), new Version.ECB(34, 25)), new Version.ECBlocks(30, new Version.ECB(20, 15), new Version.ECB(61, 16)))
      };
    }

    /// <summary>
    /// <p>Encapsulates a set of error-correction blocks in one symbol version. Most versions will
    ///             use blocks of differing sizes within one version, so, this encapsulates the parameters for
    ///             each set of blocks. It also holds the number of error-correction codewords per block since it
    ///             will be the same across all blocks within one version.</p>
    /// </summary>
    public sealed class ECBlocks
    {
      private int ecCodewordsPerBlock;
      private Version.ECB[] ecBlocks;

      public int ECCodewordsPerBlock
      {
        get
        {
          return this.ecCodewordsPerBlock;
        }
      }

      public int NumBlocks
      {
        get
        {
          int num = 0;
          for (int index = 0; index < this.ecBlocks.Length; ++index)
            num += this.ecBlocks[index].Count;
          return num;
        }
      }

      public int TotalECCodewords
      {
        get
        {
          return this.ecCodewordsPerBlock * this.NumBlocks;
        }
      }

      internal ECBlocks(int ecCodewordsPerBlock, Version.ECB ecBlocks)
      {
        this.ecCodewordsPerBlock = ecCodewordsPerBlock;
        this.ecBlocks = new Version.ECB[1]
        {
          ecBlocks
        };
      }

      internal ECBlocks(int ecCodewordsPerBlock, Version.ECB ecBlocks1, Version.ECB ecBlocks2)
      {
        this.ecCodewordsPerBlock = ecCodewordsPerBlock;
        this.ecBlocks = new Version.ECB[2]
        {
          ecBlocks1,
          ecBlocks2
        };
      }

      public Version.ECB[] getECBlocks()
      {
        return this.ecBlocks;
      }
    }

    /// <summary>
    /// <p>Encapsualtes the parameters for one error-correction block in one symbol version.
    ///             This includes the number of data codewords, and the number of times a block with these
    ///             parameters is used consecutively in the QR code version's format.</p>
    /// </summary>
    public sealed class ECB
    {
      private int count;
      private int dataCodewords;

      public int Count
      {
        get
        {
          return this.count;
        }
      }

      public int DataCodewords
      {
        get
        {
          return this.dataCodewords;
        }
      }

      internal ECB(int count, int dataCodewords)
      {
        this.count = count;
        this.dataCodewords = dataCodewords;
      }
    }
  }
}
