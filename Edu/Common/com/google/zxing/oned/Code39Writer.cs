// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.Code39Writer
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
  /// <p>Implements decoding of the EAN-13 format.</p>
  /// </summary>
  /// <author>erik.barbara@gmail.com (Erik Barbara)
  ///              </author><author>em@nerd.ocracy.org (Emanuele Aina) - Ported from ZXING Java Source
  ///              </author>
  public sealed class Code39Writer : UPCEANWriter
  {
    public override ByteMatrix encode(string contents, BarcodeFormat format, int width, int height, Hashtable hints)
    {
      if (format != BarcodeFormat.CODE_39)
        throw new ArgumentException("Can only encode CODE_39, but got " + (object) format);
      return base.encode(contents, format, width, height, hints);
    }

    public override sbyte[] encode(string contents)
    {
      int length1 = contents.Length;
      if (length1 > 80)
        throw new ArgumentException("Requested contents should be less than 80 digits long, but got " + (object) length1);
      int[] numArray = new int[9];
      int length2 = 25 + length1;
      for (int index1 = 0; index1 < length1; ++index1)
      {
        int index2 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. *$/+%".IndexOf(contents[index1]);
        Code39Writer.toIntArray(Code39Reader.CHARACTER_ENCODINGS[index2], numArray);
        for (int index3 = 0; index3 < numArray.Length; ++index3)
          length2 += numArray[index3];
      }
      sbyte[] target = new sbyte[length2];
      Code39Writer.toIntArray(Code39Reader.CHARACTER_ENCODINGS[39], numArray);
      int pos1 = UPCEANWriter.appendPattern(target, 0, numArray, 1);
      int[] pattern = new int[1]
      {
        1
      };
      int pos2 = pos1 + UPCEANWriter.appendPattern(target, pos1, pattern, 0);
      for (int index1 = length1 - 1; index1 >= 0; --index1)
      {
        int index2 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. *$/+%".IndexOf(contents[index1]);
        Code39Writer.toIntArray(Code39Reader.CHARACTER_ENCODINGS[index2], numArray);
        int pos3 = pos2 + UPCEANWriter.appendPattern(target, pos2, numArray, 1);
        pos2 = pos3 + UPCEANWriter.appendPattern(target, pos3, pattern, 0);
      }
      Code39Writer.toIntArray(Code39Reader.CHARACTER_ENCODINGS[39], numArray);
      int num = pos2 + UPCEANWriter.appendPattern(target, pos2, numArray, 1);
      return target;
    }

    private static void toIntArray(int a, int[] toReturn)
    {
      for (int index = 0; index < 9; ++index)
      {
        int num = a & 1 << index;
        toReturn[index] = num == 0 ? 1 : 2;
      }
    }
  }
}
