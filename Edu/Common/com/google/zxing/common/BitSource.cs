// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.BitSource
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.common
{
  /// <summary>
  /// <p>This provides an easy abstraction to read bits at a time from a sequence of bytes, where the
  ///             number of bits read is not often a multiple of 8.</p><p>This class is thread-safe but not reentrant. Unless the caller modifies the bytes array
  ///             it passed in, in which case all bets are off.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class BitSource
  {
    private sbyte[] bytes;
    private int byteOffset;
    private int bitOffset;

    /// <param name="bytes">bytes from which this will read bits. Bits will be read from the first byte first.
    ///             Bits are read within a byte from most-significant to least-significant bit.
    ///             </param>
    public BitSource(sbyte[] bytes)
    {
      this.bytes = bytes;
    }

    /// <param name="numBits">number of bits to read
    ///             </param>
    /// <returns>
    /// int representing the bits read. The bits will appear as the least-significant
    ///             bits of the int
    /// 
    /// </returns>
    /// <throws>IllegalArgumentException if numBits isn't in [1,32] </throws>
    public int readBits(int numBits)
    {
      if (numBits < 1 || numBits > 32)
        throw new ArgumentException();
      int num1 = 0;
      if (this.bitOffset > 0)
      {
        int num2 = 8 - this.bitOffset;
        int num3 = numBits < num2 ? numBits : num2;
        int num4 = num2 - num3;
        num1 = ((int) this.bytes[this.byteOffset] & (int) byte.MaxValue >> 8 - num3 << num4) >> num4;
        numBits -= num3;
        this.bitOffset += num3;
        if (this.bitOffset == 8)
        {
          this.bitOffset = 0;
          ++this.byteOffset;
        }
      }
      if (numBits > 0)
      {
        while (numBits >= 8)
        {
          num1 = num1 << 8 | (int) this.bytes[this.byteOffset] & (int) byte.MaxValue;
          ++this.byteOffset;
          numBits -= 8;
        }
        if (numBits > 0)
        {
          int num2 = 8 - numBits;
          int num3 = (int) byte.MaxValue >> num2 << num2;
          num1 = num1 << numBits | ((int) this.bytes[this.byteOffset] & num3) >> num2;
          this.bitOffset += numBits;
        }
      }
      return num1;
    }

    /// <returns>
    /// number of bits that can be read successfully
    /// 
    /// </returns>
    public int available()
    {
      return 8 * (this.bytes.Length - this.byteOffset) - this.bitOffset;
    }
  }
}
