// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.EAN13Writer
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
  /// This object renders an EAN13 code as a ByteMatrix 2D array of greyscale
  ///             values.
  /// 
  /// 
  /// </summary>
  /// <author>aripollak@gmail.com (Ari Pollak)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class EAN13Writer : UPCEANWriter
  {
    private const int codeWidth = 95;

    public override ByteMatrix encode(string contents, BarcodeFormat format, int width, int height, Hashtable hints)
    {
      if (format != BarcodeFormat.EAN_13)
        throw new ArgumentException("Can only encode EAN_13, but got " + (object) format);
      return base.encode(contents, format, width, height, hints);
    }

    public override sbyte[] encode(string contents)
    {
      if (contents.Length != 13)
        throw new ArgumentException("Requested contents should be 13 digits long, but got " + (object) contents.Length);
      int index1 = int.Parse(contents.Substring(0, 1));
      int num1 = EAN13Reader.FIRST_DIGIT_ENCODINGS[index1];
      sbyte[] target = new sbyte[95];
      int pos1 = 0;
      int pos2 = pos1 + UPCEANWriter.appendPattern(target, pos1, UPCEANReader.START_END_PATTERN, 1);
      for (int startIndex = 1; startIndex <= 6; ++startIndex)
      {
        int index2 = int.Parse(contents.Substring(startIndex, startIndex + 1 - startIndex));
        if ((num1 >> 6 - startIndex & 1) == 1)
          index2 += 10;
        pos2 += UPCEANWriter.appendPattern(target, pos2, UPCEANReader.L_AND_G_PATTERNS[index2], 0);
      }
      int pos3 = pos2 + UPCEANWriter.appendPattern(target, pos2, UPCEANReader.MIDDLE_PATTERN, 0);
      for (int startIndex = 7; startIndex <= 12; ++startIndex)
      {
        int index2 = int.Parse(contents.Substring(startIndex, startIndex + 1 - startIndex));
        pos3 += UPCEANWriter.appendPattern(target, pos3, UPCEANReader.L_PATTERNS[index2], 1);
      }
      int num2 = pos3 + UPCEANWriter.appendPattern(target, pos3, UPCEANReader.START_END_PATTERN, 1);
      return target;
    }
  }
}
