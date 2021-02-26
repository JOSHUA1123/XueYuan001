// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.UPCEANWriter
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System;
using System.Collections;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Encapsulates functionality and implementation that is common to UPC and EAN families
  ///             of one-dimensional barcodes.</p>
  /// </summary>
  /// <author>aripollak@gmail.com (Ari Pollak)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class UPCEANWriter : Writer
  {
    public virtual ByteMatrix encode(string contents, BarcodeFormat format, int width, int height)
    {
      return this.encode(contents, format, width, height, (Hashtable) null);
    }

    public virtual ByteMatrix encode(string contents, BarcodeFormat format, int width, int height, Hashtable hints)
    {
      if (contents == null || contents.Length == 0)
        throw new ArgumentException("Found empty contents");
      if (width < 0 || height < 0)
        throw new ArgumentException(string.Concat(new object[4]
        {
          (object) "Requested dimensions are too small: ",
          (object) width,
          (object) 'x',
          (object) height
        }));
      return UPCEANWriter.renderResult(this.encode(contents), width, height);
    }

    /// <returns>
    /// a byte array of horizontal pixels (0 = white, 1 = black)
    /// 
    /// </returns>
    private static ByteMatrix renderResult(sbyte[] code, int width, int height)
    {
      int length1 = code.Length;
      int val2 = length1 + (UPCEANReader.START_END_PATTERN.Length << 1);
      int length2 = Math.Max(width, val2);
      int height1 = Math.Max(1, height);
      int num1 = length2 / val2;
      int num2 = (length2 - length1 * num1) / 2;
      ByteMatrix byteMatrix = new ByteMatrix(length2, height1);
      sbyte[][] array = byteMatrix.Array;
      sbyte[] numArray = new sbyte[length2];
      for (int index = 0; index < num2; ++index)
        numArray[index] = (sbyte) SupportClass.Identity((long) byte.MaxValue);
      int num3 = num2;
      for (int index1 = 0; index1 < length1; ++index1)
      {
        sbyte num4 = (int) code[index1] == 1 ? (sbyte) 0 : (sbyte) SupportClass.Identity((long) byte.MaxValue);
        for (int index2 = 0; index2 < num1; ++index2)
          numArray[num3 + index2] = num4;
        num3 += num1;
      }
      for (int index = num2 + length1 * num1; index < length2; ++index)
        numArray[index] = (sbyte) SupportClass.Identity((long) byte.MaxValue);
      for (int index = 0; index < height1; ++index)
        Array.Copy((Array) numArray, 0, (Array) array[index], 0, length2);
      return byteMatrix;
    }

    /// <summary>
    /// Appends the given pattern to the target array starting at pos.
    /// 
    /// 
    /// </summary>
    /// <param name="startColor">starting color - 0 for white, 1 for black
    ///             </param>
    /// <returns>
    /// the number of elements added to target.
    /// 
    /// </returns>
    protected internal static int appendPattern(sbyte[] target, int pos, int[] pattern, int startColor)
    {
      if (startColor != 0 && startColor != 1)
        throw new ArgumentException("startColor must be either 0 or 1, but got: " + (object) startColor);
      sbyte num1 = (sbyte) startColor;
      int num2 = 0;
      for (int index1 = 0; index1 < pattern.Length; ++index1)
      {
        for (int index2 = 0; index2 < pattern[index1]; ++index2)
        {
          target[pos] = num1;
          ++pos;
          ++num2;
        }
        num1 ^= (sbyte) 1;
      }
      return num2;
    }

    /// <returns>
    /// a byte array of horizontal pixels (0 = white, 1 = black)
    /// 
    /// </returns>
    public abstract sbyte[] encode(string contents);
  }
}
