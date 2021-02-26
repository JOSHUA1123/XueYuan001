// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.BitArray
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Text;

namespace com.google.zxing.common
{
  /// <summary>
  /// <p>A simple, fast array of bits, represented compactly by an array of ints internally.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class BitArray
  {
    public int[] bits;
    public int size;

    public int Size
    {
      get
      {
        return this.size;
      }
    }

    public BitArray(int size)
    {
      if (size < 1)
        throw new ArgumentException("size must be at least 1");
      this.size = size;
      this.bits = BitArray.makeArray(size);
    }

    /// <param name="i">bit to get
    ///             </param>
    /// <returns>
    /// true iff bit i is set
    /// 
    /// </returns>
    public bool get_Renamed(int i)
    {
      return (this.bits[i >> 5] & 1 << i) != 0;
    }

    /// <summary>
    /// Sets bit i.
    /// 
    /// 
    /// </summary>
    /// <param name="i">bit to set
    ///             </param>
    public void set_Renamed(int i)
    {
      this.bits[i >> 5] |= 1 << i;
    }

    /// <summary>
    /// Flips bit i.
    /// 
    /// 
    /// </summary>
    /// <param name="i">bit to set
    ///             </param>
    public void flip(int i)
    {
      this.bits[i >> 5] ^= 1 << i;
    }

    /// <summary>
    /// Sets a block of 32 bits, starting at bit i.
    /// 
    /// 
    /// </summary>
    /// <param name="i">first bit to set
    ///             </param><param name="newBits">the new value of the next 32 bits. Note again that the least-significant bit
    ///             corresponds to bit i, the next-least-significant to i+1, and so on.
    ///             </param>
    public void setBulk(int i, int newBits)
    {
      this.bits[i >> 5] = newBits;
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
    /// Efficient method to check if a range of bits is set, or not set.
    /// 
    /// 
    /// </summary>
    /// <param name="start">start of range, inclusive.
    ///             </param><param name="end">end of range, exclusive
    ///             </param><param name="value">if true, checks that bits in range are set, otherwise checks that they are not set
    ///             </param>
    /// <returns>
    /// true iff all bits are set or not set in range, according to value argument
    /// 
    /// </returns>
    /// <throws>IllegalArgumentException if end is less than or equal to start </throws>
    public bool isRange(int start, int end, bool value_Renamed)
    {
      if (end < start)
        throw new ArgumentException();
      if (end == start)
        return true;
      --end;
      int num1 = start >> 5;
      int num2 = end >> 5;
      for (int index1 = num1; index1 <= num2; ++index1)
      {
        int num3 = index1 > num1 ? 0 : start & 31;
        int num4 = index1 < num2 ? 31 : end & 31;
        int num5;
        if (num3 == 0 && num4 == 31)
        {
          num5 = -1;
        }
        else
        {
          num5 = 0;
          for (int index2 = num3; index2 <= num4; ++index2)
            num5 |= 1 << index2;
        }
        if ((this.bits[index1] & num5) != (value_Renamed ? num5 : 0))
          return false;
      }
      return true;
    }

    /// <returns>
    /// underlying array of ints. The first element holds the first 32 bits, and the least
    ///             significant bit is bit 0.
    /// 
    /// </returns>
    public int[] getBitArray()
    {
      return this.bits;
    }

    /// <summary>
    /// Reverses all bits in the array.
    /// </summary>
    public void reverse()
    {
      int[] numArray = new int[this.bits.Length];
      int num = this.size;
      for (int index = 0; index < num; ++index)
      {
        if (this.get_Renamed(num - index - 1))
          numArray[index >> 5] |= 1 << index;
      }
      this.bits = numArray;
    }

    private static int[] makeArray(int size)
    {
      int length = size >> 5;
      if ((size & 31) != 0)
        ++length;
      return new int[length];
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(this.size);
      for (int i = 0; i < this.size; ++i)
      {
        if ((i & 7) == 0)
          stringBuilder.Append(' ');
        stringBuilder.Append(this.get_Renamed(i) ? 'X' : '.');
      }
      return stringBuilder.ToString();
    }
  }
}
