// Decompiled with JetBrains decompiler
// Type: com.google.zxing.datamatrix.decoder.Version
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;

namespace com.google.zxing.datamatrix.decoder
{
  /// <summary>
  /// The Version object encapsulates attributes about a particular
  ///             size Data Matrix Code.
  /// 
  /// 
  /// </summary>
  /// <author>bbrown@google.com (Brian Brown)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Version
  {
    private static readonly Version[] VERSIONS = Version.buildVersions();
    private int versionNumber;
    private int symbolSizeRows;
    private int symbolSizeColumns;
    private int dataRegionSizeRows;
    private int dataRegionSizeColumns;
    private Version.ECBlocks ecBlocks;
    private int totalCodewords;

    public int VersionNumber
    {
      get
      {
        return this.versionNumber;
      }
    }

    public int SymbolSizeRows
    {
      get
      {
        return this.symbolSizeRows;
      }
    }

    public int SymbolSizeColumns
    {
      get
      {
        return this.symbolSizeColumns;
      }
    }

    public int DataRegionSizeRows
    {
      get
      {
        return this.dataRegionSizeRows;
      }
    }

    public int DataRegionSizeColumns
    {
      get
      {
        return this.dataRegionSizeColumns;
      }
    }

    public int TotalCodewords
    {
      get
      {
        return this.totalCodewords;
      }
    }

    private Version(int versionNumber, int symbolSizeRows, int symbolSizeColumns, int dataRegionSizeRows, int dataRegionSizeColumns, Version.ECBlocks ecBlocks)
    {
      this.versionNumber = versionNumber;
      this.symbolSizeRows = symbolSizeRows;
      this.symbolSizeColumns = symbolSizeColumns;
      this.dataRegionSizeRows = dataRegionSizeRows;
      this.dataRegionSizeColumns = dataRegionSizeColumns;
      this.ecBlocks = ecBlocks;
      int num = 0;
      int ecCodewords = ecBlocks.ECCodewords;
      foreach (Version.ECB ecb in ecBlocks.getECBlocks())
        num += ecb.Count * (ecb.DataCodewords + ecCodewords);
      this.totalCodewords = num;
    }

    internal Version.ECBlocks getECBlocks()
    {
      return this.ecBlocks;
    }

    /// <summary>
    /// <p>Deduces version information from Data Matrix dimensions.</p>
    /// </summary>
    /// <param name="numRows">Number of rows in modules
    ///             </param><param name="numColumns">Number of columns in modules
    ///             </param>
    /// <returns>
    /// {@link Version} for a Data Matrix Code of those dimensions
    /// 
    /// </returns>
    /// <throws>ReaderException if dimensions do correspond to a valid Data Matrix size </throws>
    public static Version getVersionForDimensions(int numRows, int numColumns)
    {
      if ((numRows & 1) != 0 || (numColumns & 1) != 0)
        throw ReaderException.Instance;
      int length = Version.VERSIONS.Length;
      for (int index = 0; index < length; ++index)
      {
        Version version = Version.VERSIONS[index];
        if (version.symbolSizeRows == numRows && version.symbolSizeColumns == numColumns)
          return version;
      }
      throw ReaderException.Instance;
    }

    public override string ToString()
    {
      return Convert.ToString(this.versionNumber);
    }

    /// <summary>
    /// See ISO 16022:2006 5.5.1 Table 7
    /// </summary>
    private static Version[] buildVersions()
    {
      return new Version[30]
      {
        new Version(1, 10, 10, 8, 8, new Version.ECBlocks(5, new Version.ECB(1, 3))),
        new Version(2, 12, 12, 10, 10, new Version.ECBlocks(7, new Version.ECB(1, 5))),
        new Version(3, 14, 14, 12, 12, new Version.ECBlocks(10, new Version.ECB(1, 8))),
        new Version(4, 16, 16, 14, 14, new Version.ECBlocks(12, new Version.ECB(1, 12))),
        new Version(5, 18, 18, 16, 16, new Version.ECBlocks(14, new Version.ECB(1, 18))),
        new Version(6, 20, 20, 18, 18, new Version.ECBlocks(18, new Version.ECB(1, 22))),
        new Version(7, 22, 22, 20, 20, new Version.ECBlocks(20, new Version.ECB(1, 30))),
        new Version(8, 24, 24, 22, 22, new Version.ECBlocks(24, new Version.ECB(1, 36))),
        new Version(9, 26, 26, 24, 24, new Version.ECBlocks(28, new Version.ECB(1, 44))),
        new Version(10, 32, 32, 14, 14, new Version.ECBlocks(36, new Version.ECB(1, 62))),
        new Version(11, 36, 36, 16, 16, new Version.ECBlocks(42, new Version.ECB(1, 86))),
        new Version(12, 40, 40, 18, 18, new Version.ECBlocks(48, new Version.ECB(1, 114))),
        new Version(13, 44, 44, 20, 20, new Version.ECBlocks(56, new Version.ECB(1, 144))),
        new Version(14, 48, 48, 22, 22, new Version.ECBlocks(68, new Version.ECB(1, 174))),
        new Version(15, 52, 52, 24, 24, new Version.ECBlocks(42, new Version.ECB(2, 102))),
        new Version(16, 64, 64, 14, 14, new Version.ECBlocks(56, new Version.ECB(2, 140))),
        new Version(17, 72, 72, 16, 16, new Version.ECBlocks(36, new Version.ECB(4, 92))),
        new Version(18, 80, 80, 18, 18, new Version.ECBlocks(48, new Version.ECB(4, 114))),
        new Version(19, 88, 88, 20, 20, new Version.ECBlocks(56, new Version.ECB(4, 144))),
        new Version(20, 96, 96, 22, 22, new Version.ECBlocks(68, new Version.ECB(4, 174))),
        new Version(21, 104, 104, 24, 24, new Version.ECBlocks(56, new Version.ECB(6, 136))),
        new Version(22, 120, 120, 18, 18, new Version.ECBlocks(68, new Version.ECB(6, 175))),
        new Version(23, 132, 132, 20, 20, new Version.ECBlocks(62, new Version.ECB(8, 163))),
        new Version(24, 144, 144, 22, 22, new Version.ECBlocks(62, new Version.ECB(8, 156), new Version.ECB(2, 155))),
        new Version(25, 8, 18, 6, 16, new Version.ECBlocks(7, new Version.ECB(1, 5))),
        new Version(26, 8, 32, 6, 14, new Version.ECBlocks(11, new Version.ECB(1, 10))),
        new Version(27, 12, 26, 10, 24, new Version.ECBlocks(14, new Version.ECB(1, 16))),
        new Version(28, 12, 36, 10, 16, new Version.ECBlocks(18, new Version.ECB(1, 22))),
        new Version(29, 16, 36, 10, 16, new Version.ECBlocks(24, new Version.ECB(1, 32))),
        new Version(30, 16, 48, 14, 22, new Version.ECBlocks(28, new Version.ECB(1, 49)))
      };
    }

    /// <summary>
    /// <p>Encapsulates a set of error-correction blocks in one symbol version. Most versions will
    ///             use blocks of differing sizes within one version, so, this encapsulates the parameters for
    ///             each set of blocks. It also holds the number of error-correction codewords per block since it
    ///             will be the same across all blocks within one version.</p>
    /// </summary>
    internal sealed class ECBlocks
    {
      private int ecCodewords;
      private Version.ECB[] ecBlocks;

      internal int ECCodewords
      {
        get
        {
          return this.ecCodewords;
        }
      }

      internal ECBlocks(int ecCodewords, Version.ECB ecBlocks)
      {
        this.ecCodewords = ecCodewords;
        this.ecBlocks = new Version.ECB[1]
        {
          ecBlocks
        };
      }

      internal ECBlocks(int ecCodewords, Version.ECB ecBlocks1, Version.ECB ecBlocks2)
      {
        this.ecCodewords = ecCodewords;
        this.ecBlocks = new Version.ECB[2]
        {
          ecBlocks1,
          ecBlocks2
        };
      }

      internal Version.ECB[] getECBlocks()
      {
        return this.ecBlocks;
      }
    }

    /// <summary>
    /// <p>Encapsualtes the parameters for one error-correction block in one symbol version.
    ///             This includes the number of data codewords, and the number of times a block with these
    ///             parameters is used consecutively in the Data Matrix code version's format.</p>
    /// </summary>
    internal sealed class ECB
    {
      private int count;
      private int dataCodewords;

      internal int Count
      {
        get
        {
          return this.count;
        }
      }

      internal int DataCodewords
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
