// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.encoder.BitVector
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Text;

namespace com.google.zxing.qrcode.encoder
{
  /// <summary>
  /// JAVAPORT: This should be combined with BitArray in the future, although that class is not yet
  ///             dynamically resizeable. This implementation is reasonable but there is a lot of function calling
  ///             in loops I'd like to get rid of.
  /// 
  /// 
  /// </summary>
  /// <author>satorux@google.com (Satoru Takabayashi) - creator
  ///             </author><author>dswitkin@google.com (Daniel Switkin) - ported from C++
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class BitVector
  {
    private const int DEFAULT_SIZE_IN_BYTES = 32;
    private int sizeInBits;
    private sbyte[] array;

    public sbyte[] Array
    {
      get
      {
        return this.array;
      }
    }

    public BitVector()
    {
      this.sizeInBits = 0;
      this.array = new sbyte[32];
    }

    public int at(int index)
    {
      if (index < 0 || index >= this.sizeInBits)
        throw new ArgumentException("Bad index: " + (object) index);
      return ((int) this.array[index >> 3] & (int) byte.MaxValue) >> 7 - (index & 7) & 1;
    }

    public int size()
    {
      return this.sizeInBits;
    }

    public int sizeInBytes()
    {
      return this.sizeInBits + 7 >> 3;
    }

    public void appendBit(int bit)
    {
      if (bit != 0 && bit != 1)
        throw new ArgumentException("Bad bit");
      int num = this.sizeInBits & 7;
      if (num == 0)
      {
        this.appendByte(0);
        this.sizeInBits -= 8;
      }
      this.array[this.sizeInBits >> 3] |= (sbyte) (bit << 7 - num);
      ++this.sizeInBits;
    }

    public void appendBits(int value_Renamed, int numBits)
    {
      if (numBits < 0 || numBits > 32)
        throw new ArgumentException("Num bits must be between 0 and 32");
      int num = numBits;
      while (num > 0)
      {
        if ((this.sizeInBits & 7) == 0 && num >= 8)
        {
          this.appendByte(value_Renamed >> num - 8 & (int) byte.MaxValue);
          num -= 8;
        }
        else
        {
          this.appendBit(value_Renamed >> num - 1 & 1);
          --num;
        }
      }
    }

    public void appendBitVector(BitVector bits)
    {
      int num = bits.size();
      for (int index = 0; index < num; ++index)
        this.appendBit(bits.at(index));
    }

    public void xor(BitVector other)
    {
      if (this.sizeInBits != other.size())
        throw new ArgumentException("BitVector sizes don't match");
      int num = this.sizeInBits + 7 >> 3;
      for (int index = 0; index < num; ++index)
        this.array[index] ^= other.array[index];
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(this.sizeInBits);
      for (int index = 0; index < this.sizeInBits; ++index)
      {
        if (this.at(index) == 0)
        {
          stringBuilder.Append('0');
        }
        else
        {
          if (this.at(index) != 1)
            throw new ArgumentException("Byte isn't 0 or 1");
          stringBuilder.Append('1');
        }
      }
      return stringBuilder.ToString();
    }

    private void appendByte(int value_Renamed)
    {
      if (this.sizeInBits >> 3 == this.array.Length)
      {
        sbyte[] numArray = new sbyte[this.array.Length << 1];
        System.Array.Copy((System.Array) this.array, 0, (System.Array) numArray, 0, this.array.Length);
        this.array = numArray;
      }
      this.array[this.sizeInBits >> 3] = (sbyte) value_Renamed;
      this.sizeInBits += 8;
    }
  }
}
