// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.EAN8Writer
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
  /// This object renders an EAN8 code as a ByteMatrix 2D array of greyscale
  ///             values.
  /// 
  /// 
  /// </summary>
  /// <author>aripollak@gmail.com (Ari Pollak)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class EAN8Writer : UPCEANWriter
  {
    private const int codeWidth = 67;

    public override ByteMatrix encode(string contents, BarcodeFormat format, int width, int height, Hashtable hints)
    {
      if (format != BarcodeFormat.EAN_8)
        throw new ArgumentException("Can only encode EAN_8, but got " + (object) format);
      return base.encode(contents, format, width, height, hints);
    }

    /// <returns>
    /// a byte array of horizontal pixels (0 = white, 1 = black)
    /// 
    /// </returns>
    public override sbyte[] encode(string contents)
    {
      if (contents.Length != 8)
        throw new ArgumentException("Requested contents should be 8 digits long, but got " + (object) contents.Length);
      sbyte[] target = new sbyte[67];
      int pos1 = 0;
      int pos2 = pos1 + UPCEANWriter.appendPattern(target, pos1, UPCEANReader.START_END_PATTERN, 1);
      for (int startIndex = 0; startIndex <= 3; ++startIndex)
      {
        int index = int.Parse(contents.Substring(startIndex, startIndex + 1 - startIndex));
        pos2 += UPCEANWriter.appendPattern(target, pos2, UPCEANReader.L_PATTERNS[index], 0);
      }
      int pos3 = pos2 + UPCEANWriter.appendPattern(target, pos2, UPCEANReader.MIDDLE_PATTERN, 0);
      for (int startIndex = 4; startIndex <= 7; ++startIndex)
      {
        int index = int.Parse(contents.Substring(startIndex, startIndex + 1 - startIndex));
        pos3 += UPCEANWriter.appendPattern(target, pos3, UPCEANReader.L_PATTERNS[index], 1);
      }
      int num = pos3 + UPCEANWriter.appendPattern(target, pos3, UPCEANReader.START_END_PATTERN, 1);
      return target;
    }
  }
}
