// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.EAN8Reader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System.Text;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Implements decoding of the EAN-8 format.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class EAN8Reader : UPCEANReader
  {
    private int[] decodeMiddleCounters;

    internal override BarcodeFormat BarcodeFormat
    {
      get
      {
        return BarcodeFormat.EAN_8;
      }
    }

    public EAN8Reader()
    {
      this.decodeMiddleCounters = new int[4];
    }

    protected internal override int decodeMiddle(BitArray row, int[] startRange, StringBuilder result)
    {
      int[] counters = this.decodeMiddleCounters;
      counters[0] = 0;
      counters[1] = 0;
      counters[2] = 0;
      counters[3] = 0;
      int size = row.Size;
      int rowOffset1 = startRange[1];
      for (int index1 = 0; index1 < 4 && rowOffset1 < size; ++index1)
      {
        int num = UPCEANReader.decodeDigit(row, counters, rowOffset1, UPCEANReader.L_PATTERNS);
        result.Append((char) (48 + num));
        for (int index2 = 0; index2 < counters.Length; ++index2)
          rowOffset1 += counters[index2];
      }
      int rowOffset2 = UPCEANReader.findGuardPattern(row, rowOffset1, true, UPCEANReader.MIDDLE_PATTERN)[1];
      for (int index1 = 0; index1 < 4 && rowOffset2 < size; ++index1)
      {
        int num = UPCEANReader.decodeDigit(row, counters, rowOffset2, UPCEANReader.L_PATTERNS);
        result.Append((char) (48 + num));
        for (int index2 = 0; index2 < counters.Length; ++index2)
          rowOffset2 += counters[index2];
      }
      return rowOffset2;
    }
  }
}
