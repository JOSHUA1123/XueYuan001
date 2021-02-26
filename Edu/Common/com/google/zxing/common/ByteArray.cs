// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.ByteArray
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.common
{
  /// <summary>
  /// This class implements an array of unsigned bytes.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ByteArray
  {
    private const int INITIAL_SIZE = 32;
    private sbyte[] bytes;
    private int size_Renamed_Field;

    public bool Empty
    {
      get
      {
        return this.size_Renamed_Field == 0;
      }
    }

    public ByteArray()
    {
      this.bytes = (sbyte[]) null;
      this.size_Renamed_Field = 0;
    }

    public ByteArray(int size)
    {
      this.bytes = new sbyte[size];
      this.size_Renamed_Field = size;
    }

    public ByteArray(sbyte[] byteArray)
    {
      this.bytes = byteArray;
      this.size_Renamed_Field = this.bytes.Length;
    }

    /// <summary>
    /// Access an unsigned byte at location index.
    /// </summary>
    /// <param name="index">The index in the array to access.
    ///             </param>
    /// <returns>
    /// The unsigned value of the byte as an int.
    /// 
    /// </returns>
    public int at(int index)
    {
      return (int) this.bytes[index] & (int) byte.MaxValue;
    }

    public void set_Renamed(int index, int value_Renamed)
    {
      this.bytes[index] = (sbyte) value_Renamed;
    }

    public int size()
    {
      return this.size_Renamed_Field;
    }

    public void appendByte(int value_Renamed)
    {
      if (this.size_Renamed_Field == 0 || this.size_Renamed_Field >= this.bytes.Length)
        this.reserve(Math.Max(32, this.size_Renamed_Field << 1));
      this.bytes[this.size_Renamed_Field] = (sbyte) value_Renamed;
      ++this.size_Renamed_Field;
    }

    public void reserve(int capacity)
    {
      if (this.bytes != null && this.bytes.Length >= capacity)
        return;
      sbyte[] numArray = new sbyte[capacity];
      if (this.bytes != null)
        Array.Copy((Array) this.bytes, 0, (Array) numArray, 0, this.bytes.Length);
      this.bytes = numArray;
    }

    public void set_Renamed(sbyte[] source, int offset, int count)
    {
      this.bytes = new sbyte[count];
      this.size_Renamed_Field = count;
      for (int index = 0; index < count; ++index)
        this.bytes[index] = source[offset + index];
    }
  }
}
