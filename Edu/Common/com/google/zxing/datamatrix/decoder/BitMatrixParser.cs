// Decompiled with JetBrains decompiler
// Type: com.google.zxing.datamatrix.decoder.BitMatrixParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System;

namespace com.google.zxing.datamatrix.decoder
{
  /// <author>bbrown@google.com (Brian Brown)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class BitMatrixParser
  {
    private BitMatrix mappingBitMatrix;
    private BitMatrix readMappingMatrix;
    private Version version;

    internal BitMatrixParser(BitMatrix bitMatrix)
    {
      int dimension = bitMatrix.Dimension;
      if (dimension < 10 || dimension > 144 || (dimension & 1) != 0)
        throw ReaderException.Instance;
      this.version = this.readVersion(bitMatrix);
      this.mappingBitMatrix = this.extractDataRegion(bitMatrix);
      this.readMappingMatrix = new BitMatrix(this.mappingBitMatrix.Dimension);
    }

    /// <summary>
    /// <p>Creates the version object based on the dimension of the original bit matrix from
    ///             the datamatrix code.</p><p>See ISO 16022:2006 Table 7 - ECC 200 symbol attributes</p>
    /// </summary>
    /// <param name="bitMatrix">Original {@link BitMatrix} including alignment patterns
    ///             </param>
    /// <returns>
    /// {@link Version} encapsulating the Data Matrix Code's "version"
    /// 
    /// </returns>
    /// <throws>ReaderException if the dimensions of the mapping matrix are not valid </throws>
    /// <summary>
    /// Data Matrix dimensions.
    /// 
    /// </summary>
    internal Version readVersion(BitMatrix bitMatrix)
    {
      if (this.version != null)
        return this.version;
      int dimension = bitMatrix.Dimension;
      int numColumns = dimension;
      return Version.getVersionForDimensions(dimension, numColumns);
    }

    /// <summary>
    /// <p>Reads the bits in the {@link BitMatrix} representing the mapping matrix (No alignment patterns)
    ///             in the correct order in order to reconstitute the codewords bytes contained within the
    ///             Data Matrix Code.</p>
    /// </summary>
    /// 
    /// <returns>
    /// bytes encoded within the Data Matrix Code
    /// 
    /// </returns>
    /// <throws>ReaderException if the exact number of bytes expected is not read </throws>
    internal sbyte[] readCodewords()
    {
      sbyte[] numArray = new sbyte[this.version.TotalCodewords];
      int num1 = 0;
      int num2 = 4;
      int num3 = 0;
      int dimension = this.mappingBitMatrix.Dimension;
      int numColumns = dimension;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      do
      {
        if (num2 == dimension && num3 == 0 && !flag1)
        {
          numArray[num1++] = (sbyte) this.readCorner1(dimension, numColumns);
          num2 -= 2;
          num3 += 2;
          flag1 = true;
        }
        else if (num2 == dimension - 2 && num3 == 0 && ((numColumns & 3) != 0 && !flag2))
        {
          numArray[num1++] = (sbyte) this.readCorner2(dimension, numColumns);
          num2 -= 2;
          num3 += 2;
          flag2 = true;
        }
        else if (num2 == dimension + 4 && num3 == 2 && ((numColumns & 7) == 0 && !flag3))
        {
          numArray[num1++] = (sbyte) this.readCorner3(dimension, numColumns);
          num2 -= 2;
          num3 += 2;
          flag3 = true;
        }
        else if (num2 == dimension - 2 && num3 == 0 && ((numColumns & 7) == 4 && !flag4))
        {
          numArray[num1++] = (sbyte) this.readCorner4(dimension, numColumns);
          num2 -= 2;
          num3 += 2;
          flag4 = true;
        }
        else
        {
          do
          {
            if (num2 < dimension && num3 >= 0 && !this.readMappingMatrix.get_Renamed(num3, num2))
              numArray[num1++] = (sbyte) this.readUtah(num2, num3, dimension, numColumns);
            num2 -= 2;
            num3 += 2;
          }
          while (num2 >= 0 && num3 < numColumns);
          int num4 = num2 + 1;
          int num5 = num3 + 3;
          do
          {
            if (num4 >= 0 && num5 < numColumns && !this.readMappingMatrix.get_Renamed(num5, num4))
              numArray[num1++] = (sbyte) this.readUtah(num4, num5, dimension, numColumns);
            num4 += 2;
            num5 -= 2;
          }
          while (num4 < dimension && num5 >= 0);
          num2 = num4 + 3;
          num3 = num5 + 1;
        }
      }
      while (num2 < dimension || num3 < numColumns);
      if (num1 != this.version.TotalCodewords)
        throw ReaderException.Instance;
      return numArray;
    }

    /// <summary>
    /// <p>Reads a bit of the mapping matrix accounting for boundary wrapping.</p>
    /// </summary>
    /// <param name="row">Row to read in the mapping matrix
    ///             </param><param name="column">Column to read in the mapping matrix
    ///             </param><param name="numRows">Number of rows in the mapping matrix
    ///             </param><param name="numColumns">Number of columns in the mapping matrix
    ///             </param>
    /// <returns>
    /// value of the given bit in the mapping matrix
    /// 
    /// </returns>
    internal bool readModule(int row, int column, int numRows, int numColumns)
    {
      if (row < 0)
      {
        row += numRows;
        column += 4 - (numRows + 4 & 7);
      }
      if (column < 0)
      {
        column += numColumns;
        row += 4 - (numColumns + 4 & 7);
      }
      this.readMappingMatrix.set_Renamed(column, row);
      return this.mappingBitMatrix.get_Renamed(column, row);
    }

    /// <summary>
    /// <p>Reads the 8 bits of the standard Utah-shaped pattern.</p><p>See ISO 16022:2006, 5.8.1 Figure 6</p>
    /// </summary>
    /// <param name="row">Current row in the mapping matrix, anchored at the 8th bit (LSB) of the pattern
    ///             </param><param name="column">Current column in the mapping matrix, anchored at the 8th bit (LSB) of the pattern
    ///             </param><param name="numRows">Number of rows in the mapping matrix
    ///             </param><param name="numColumns">Number of columns in the mapping matrix
    ///             </param>
    /// <returns>
    /// byte from the utah shape
    /// 
    /// </returns>
    internal int readUtah(int row, int column, int numRows, int numColumns)
    {
      int num1 = 0;
      if (this.readModule(row - 2, column - 2, numRows, numColumns))
        num1 |= 1;
      int num2 = num1 << 1;
      if (this.readModule(row - 2, column - 1, numRows, numColumns))
        num2 |= 1;
      int num3 = num2 << 1;
      if (this.readModule(row - 1, column - 2, numRows, numColumns))
        num3 |= 1;
      int num4 = num3 << 1;
      if (this.readModule(row - 1, column - 1, numRows, numColumns))
        num4 |= 1;
      int num5 = num4 << 1;
      if (this.readModule(row - 1, column, numRows, numColumns))
        num5 |= 1;
      int num6 = num5 << 1;
      if (this.readModule(row, column - 2, numRows, numColumns))
        num6 |= 1;
      int num7 = num6 << 1;
      if (this.readModule(row, column - 1, numRows, numColumns))
        num7 |= 1;
      int num8 = num7 << 1;
      if (this.readModule(row, column, numRows, numColumns))
        num8 |= 1;
      return num8;
    }

    /// <summary>
    /// <p>Reads the 8 bits of the special corner condition 1.</p><p>See ISO 16022:2006, Figure F.3</p>
    /// </summary>
    /// <param name="numRows">Number of rows in the mapping matrix
    ///             </param><param name="numColumns">Number of columns in the mapping matrix
    ///             </param>
    /// <returns>
    /// byte from the Corner condition 1
    /// 
    /// </returns>
    internal int readCorner1(int numRows, int numColumns)
    {
      int num1 = 0;
      if (this.readModule(numRows - 1, 0, numRows, numColumns))
        num1 |= 1;
      int num2 = num1 << 1;
      if (this.readModule(numRows - 1, 1, numRows, numColumns))
        num2 |= 1;
      int num3 = num2 << 1;
      if (this.readModule(numRows - 1, 2, numRows, numColumns))
        num3 |= 1;
      int num4 = num3 << 1;
      if (this.readModule(0, numColumns - 2, numRows, numColumns))
        num4 |= 1;
      int num5 = num4 << 1;
      if (this.readModule(0, numColumns - 1, numRows, numColumns))
        num5 |= 1;
      int num6 = num5 << 1;
      if (this.readModule(1, numColumns - 1, numRows, numColumns))
        num6 |= 1;
      int num7 = num6 << 1;
      if (this.readModule(2, numColumns - 1, numRows, numColumns))
        num7 |= 1;
      int num8 = num7 << 1;
      if (this.readModule(3, numColumns - 1, numRows, numColumns))
        num8 |= 1;
      return num8;
    }

    /// <summary>
    /// <p>Reads the 8 bits of the special corner condition 2.</p><p>See ISO 16022:2006, Figure F.4</p>
    /// </summary>
    /// <param name="numRows">Number of rows in the mapping matrix
    ///             </param><param name="numColumns">Number of columns in the mapping matrix
    ///             </param>
    /// <returns>
    /// byte from the Corner condition 2
    /// 
    /// </returns>
    internal int readCorner2(int numRows, int numColumns)
    {
      int num1 = 0;
      if (this.readModule(numRows - 3, 0, numRows, numColumns))
        num1 |= 1;
      int num2 = num1 << 1;
      if (this.readModule(numRows - 2, 0, numRows, numColumns))
        num2 |= 1;
      int num3 = num2 << 1;
      if (this.readModule(numRows - 1, 0, numRows, numColumns))
        num3 |= 1;
      int num4 = num3 << 1;
      if (this.readModule(0, numColumns - 4, numRows, numColumns))
        num4 |= 1;
      int num5 = num4 << 1;
      if (this.readModule(0, numColumns - 3, numRows, numColumns))
        num5 |= 1;
      int num6 = num5 << 1;
      if (this.readModule(0, numColumns - 2, numRows, numColumns))
        num6 |= 1;
      int num7 = num6 << 1;
      if (this.readModule(0, numColumns - 1, numRows, numColumns))
        num7 |= 1;
      int num8 = num7 << 1;
      if (this.readModule(1, numColumns - 1, numRows, numColumns))
        num8 |= 1;
      return num8;
    }

    /// <summary>
    /// <p>Reads the 8 bits of the special corner condition 3.</p><p>See ISO 16022:2006, Figure F.5</p>
    /// </summary>
    /// <param name="numRows">Number of rows in the mapping matrix
    ///             </param><param name="numColumns">Number of columns in the mapping matrix
    ///             </param>
    /// <returns>
    /// byte from the Corner condition 3
    /// 
    /// </returns>
    internal int readCorner3(int numRows, int numColumns)
    {
      int num1 = 0;
      if (this.readModule(numRows - 1, 0, numRows, numColumns))
        num1 |= 1;
      int num2 = num1 << 1;
      if (this.readModule(numRows - 1, numColumns - 1, numRows, numColumns))
        num2 |= 1;
      int num3 = num2 << 1;
      if (this.readModule(0, numColumns - 3, numRows, numColumns))
        num3 |= 1;
      int num4 = num3 << 1;
      if (this.readModule(0, numColumns - 2, numRows, numColumns))
        num4 |= 1;
      int num5 = num4 << 1;
      if (this.readModule(0, numColumns - 1, numRows, numColumns))
        num5 |= 1;
      int num6 = num5 << 1;
      if (this.readModule(1, numColumns - 3, numRows, numColumns))
        num6 |= 1;
      int num7 = num6 << 1;
      if (this.readModule(1, numColumns - 2, numRows, numColumns))
        num7 |= 1;
      int num8 = num7 << 1;
      if (this.readModule(1, numColumns - 1, numRows, numColumns))
        num8 |= 1;
      return num8;
    }

    /// <summary>
    /// <p>Reads the 8 bits of the special corner condition 4.</p><p>See ISO 16022:2006, Figure F.6</p>
    /// </summary>
    /// <param name="numRows">Number of rows in the mapping matrix
    ///             </param><param name="numColumns">Number of columns in the mapping matrix
    ///             </param>
    /// <returns>
    /// byte from the Corner condition 4
    /// 
    /// </returns>
    internal int readCorner4(int numRows, int numColumns)
    {
      int num1 = 0;
      if (this.readModule(numRows - 3, 0, numRows, numColumns))
        num1 |= 1;
      int num2 = num1 << 1;
      if (this.readModule(numRows - 2, 0, numRows, numColumns))
        num2 |= 1;
      int num3 = num2 << 1;
      if (this.readModule(numRows - 1, 0, numRows, numColumns))
        num3 |= 1;
      int num4 = num3 << 1;
      if (this.readModule(0, numColumns - 2, numRows, numColumns))
        num4 |= 1;
      int num5 = num4 << 1;
      if (this.readModule(0, numColumns - 1, numRows, numColumns))
        num5 |= 1;
      int num6 = num5 << 1;
      if (this.readModule(1, numColumns - 1, numRows, numColumns))
        num6 |= 1;
      int num7 = num6 << 1;
      if (this.readModule(2, numColumns - 1, numRows, numColumns))
        num7 |= 1;
      int num8 = num7 << 1;
      if (this.readModule(3, numColumns - 1, numRows, numColumns))
        num8 |= 1;
      return num8;
    }

    /// <summary>
    /// <p>Extracts the data region from a {@link BitMatrix} that contains
    ///             alignment patterns.</p>
    /// </summary>
    /// <param name="bitMatrix">Original {@link BitMatrix} with alignment patterns
    ///             </param>
    /// <returns>
    /// BitMatrix that has the alignment patterns removed
    /// 
    /// </returns>
    internal BitMatrix extractDataRegion(BitMatrix bitMatrix)
    {
      int symbolSizeRows = this.version.SymbolSizeRows;
      int symbolSizeColumns = this.version.SymbolSizeColumns;
      if (bitMatrix.Dimension != symbolSizeRows)
        throw new ArgumentException("Dimension of bitMarix must match the version size");
      int dataRegionSizeRows = this.version.DataRegionSizeRows;
      int regionSizeColumns = this.version.DataRegionSizeColumns;
      int num1 = symbolSizeRows / dataRegionSizeRows;
      int num2 = symbolSizeColumns / regionSizeColumns;
      BitMatrix bitMatrix1 = new BitMatrix(num1 * dataRegionSizeRows);
      for (int index1 = 0; index1 < num1; ++index1)
      {
        int num3 = index1 * dataRegionSizeRows;
        for (int index2 = 0; index2 < num2; ++index2)
        {
          int num4 = index2 * regionSizeColumns;
          for (int index3 = 0; index3 < dataRegionSizeRows; ++index3)
          {
            int y1 = index1 * (dataRegionSizeRows + 2) + 1 + index3;
            int y2 = num3 + index3;
            for (int index4 = 0; index4 < regionSizeColumns; ++index4)
            {
              int x1 = index2 * (regionSizeColumns + 2) + 1 + index4;
              if (bitMatrix.get_Renamed(x1, y1))
              {
                int x2 = num4 + index4;
                bitMatrix1.set_Renamed(x2, y2);
              }
            }
          }
        }
      }
      return bitMatrix1;
    }
  }
}
