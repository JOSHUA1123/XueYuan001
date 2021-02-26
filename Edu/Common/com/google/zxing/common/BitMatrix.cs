// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.BitMatrix
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Text;

namespace com.google.zxing.common
{
  /// <summary>
  /// <p>Represents a 2D matrix of bits. In function arguments below, and throughout the common
  ///             module, x is the column position, and y is the row position. The ordering is always x, y.
  ///             The origin is at the top-left.</p><p>Internally the bits are represented in a 1-D array of 32-bit ints. However, each row begins
  ///             with a new int. This is done intentionally so that we can copy out a row into a BitArray very
  ///             efficiently.</p><p>The ordering of bits is row-major. Within each int, the least significant bits are used first,
  ///             meaning they represent lower x values. This is compatible with BitArray's implementation.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class BitMatrix
  {
    public int width;
    public int height;
    public int rowSize;
    public int[] bits;

    /// <returns>
    /// The width of the matrix
    /// 
    /// </returns>
    public int Width
    {
      get
      {
        return this.width;
      }
    }

    /// <returns>
    /// The height of the matrix
    /// 
    /// </returns>
    public int Height
    {
      get
      {
        return this.height;
      }
    }

    /// <summary>
    /// This method is for compatibility with older code. It's only logical to call if the matrix
    ///             is square, so I'm throwing if that's not the case.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// row/column dimension of this matrix
    /// 
    /// </returns>
    public int Dimension
    {
      get
      {
        if (this.width != this.height)
          throw new SystemException("Can't call getDimension() on a non-square matrix");
        return this.width;
      }
    }

    public BitMatrix(int dimension)
      : this(dimension, dimension)
    {
    }

    public BitMatrix(int width, int height)
    {
      if (width < 1 || height < 1)
        throw new ArgumentException("Both dimensions must be greater than 0");
      this.width = width;
      this.height = height;
      int num = width >> 5;
      if ((width & 31) != 0)
        ++num;
      this.rowSize = num;
      this.bits = new int[num * height];
    }

    /// <summary>
    /// <p>Gets the requested bit, where true means black.</p>
    /// </summary>
    /// <param name="x">The horizontal component (i.e. which column)
    ///             </param><param name="y">The vertical component (i.e. which row)
    ///             </param>
    /// <returns>
    /// value of given bit in matrix
    /// 
    /// </returns>
    public bool get_Renamed(int x, int y)
    {
      return (SupportClass.URShift(this.bits[y * this.rowSize + (x >> 5)], x & 31) & 1) != 0;
    }

    /// <summary>
    /// <p>Sets the given bit to true.</p>
    /// </summary>
    /// <param name="x">The horizontal component (i.e. which column)
    ///             </param><param name="y">The vertical component (i.e. which row)
    ///             </param>
    public void set_Renamed(int x, int y)
    {
      this.bits[y * this.rowSize + (x >> 5)] |= 1 << x;
    }

    /// <summary>
    /// <p>Flips the given bit.</p>
    /// </summary>
    /// <param name="x">The horizontal component (i.e. which column)
    ///             </param><param name="y">The vertical component (i.e. which row)
    ///             </param>
    public void flip(int x, int y)
    {
      this.bits[y * this.rowSize + (x >> 5)] ^= 1 << x;
    }

    /// <summary>
    /// Clears all bits (sets to false).
    /// </summary>
    public void clear()
    {
      int length = this.bits.Length;
      for (int index = 0; index < length; ++index)
        this.bits[index] = 0;
    }

    /// <summary>
    /// <p>Sets a square region of the bit matrix to true.</p>
    /// </summary>
    /// <param name="left">The horizontal position to begin at (inclusive)
    ///             </param><param name="top">The vertical position to begin at (inclusive)
    ///             </param><param name="width">The width of the region
    ///             </param><param name="height">The height of the region
    ///             </param>
    public void setRegion(int left, int top, int width, int height)
    {
      if (top < 0 || left < 0)
        throw new ArgumentException("Left and top must be nonnegative");
      if (height < 1 || width < 1)
        throw new ArgumentException("Height and width must be at least 1");
      int num1 = left + width;
      int num2 = top + height;
      if (num2 > this.height || num1 > this.width)
        throw new ArgumentException("The region must fit inside the matrix");
      for (int index1 = top; index1 < num2; ++index1)
      {
        int num3 = index1 * this.rowSize;
        for (int index2 = left; index2 < num1; ++index2)
          this.bits[num3 + (index2 >> 5)] |= 1 << index2;
      }
    }

    /// <summary>
    /// A fast method to retrieve one row of data from the matrix as a BitArray.
    /// 
    /// 
    /// </summary>
    /// <param name="y">The row to retrieve
    ///             </param><param name="row">An optional caller-allocated BitArray, will be allocated if null or too small
    ///             </param>
    /// <returns>
    /// The resulting BitArray - this reference should always be used even when passing
    ///             your own row
    /// 
    /// </returns>
    public BitArray getRow(int y, BitArray row)
    {
      if (row == null || row.Size < this.width)
        row = new BitArray(this.width);
      int num = y * this.rowSize;
      for (int index = 0; index < this.rowSize; ++index)
        row.setBulk(index << 5, this.bits[num + index]);
      return row;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(this.height * (this.width + 1));
      for (int y = 0; y < this.height; ++y)
      {
        for (int x = 0; x < this.width; ++x)
          stringBuilder.Append(this.get_Renamed(x, y) ? "X " : "  ");
        stringBuilder.Append('\n');
      }
      return stringBuilder.ToString();
    }
  }
}
