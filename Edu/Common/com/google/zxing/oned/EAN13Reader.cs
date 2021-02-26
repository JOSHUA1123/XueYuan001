// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.EAN13Reader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System.Text;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Implements decoding of the EAN-13 format.</p>
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>Sean Owen
  ///             </author><author>alasdair@google.com (Alasdair Mackintosh)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class EAN13Reader : UPCEANReader
  {
    internal static readonly int[] FIRST_DIGIT_ENCODINGS = new int[10]
    {
      0,
      11,
      13,
      14,
      19,
      25,
      28,
      21,
      22,
      26
    };
    private int[] decodeMiddleCounters;

    internal override BarcodeFormat BarcodeFormat
    {
      get
      {
        return BarcodeFormat.EAN_13;
      }
    }

    public EAN13Reader()
    {
      this.decodeMiddleCounters = new int[4];
    }

    protected internal override int decodeMiddle(BitArray row, int[] startRange, StringBuilder resultString)
    {
      int[] counters = this.decodeMiddleCounters;
      counters[0] = 0;
      counters[1] = 0;
      counters[2] = 0;
      counters[3] = 0;
      int size = row.Size;
      int rowOffset1 = startRange[1];
      int lgPatternFound = 0;
      for (int index1 = 0; index1 < 6 && rowOffset1 < size; ++index1)
      {
        int num = UPCEANReader.decodeDigit(row, counters, rowOffset1, UPCEANReader.L_AND_G_PATTERNS);
        resultString.Append((char) (48 + num % 10));
        for (int index2 = 0; index2 < counters.Length; ++index2)
          rowOffset1 += counters[index2];
        if (num >= 10)
          lgPatternFound |= 1 << 5 - index1;
      }
      EAN13Reader.determineFirstDigit(resultString, lgPatternFound);
      int rowOffset2 = UPCEANReader.findGuardPattern(row, rowOffset1, true, UPCEANReader.MIDDLE_PATTERN)[1];
      for (int index1 = 0; index1 < 6 && rowOffset2 < size; ++index1)
      {
        int num = UPCEANReader.decodeDigit(row, counters, rowOffset2, UPCEANReader.L_PATTERNS);
        resultString.Append((char) (48 + num));
        for (int index2 = 0; index2 < counters.Length; ++index2)
          rowOffset2 += counters[index2];
      }
      return rowOffset2;
    }

    /// <summary>
    /// Based on pattern of odd-even ('L' and 'G') patterns used to encoded the explicitly-encoded
    ///             digits in a barcode, determines the implicitly encoded first digit and adds it to the
    ///             result string.
    /// 
    /// 
    /// </summary>
    /// <param name="resultString">string to insert decoded first digit into
    ///             </param><param name="lgPatternFound">int whose bits indicates the pattern of odd/even L/G patterns used to
    ///             encode digits
    ///             </param><throws>ReaderException if first digit cannot be determined </throws>
    private static void determineFirstDigit(StringBuilder resultString, int lgPatternFound)
    {
      for (int index = 0; index < 10; ++index)
      {
        if (lgPatternFound == EAN13Reader.FIRST_DIGIT_ENCODINGS[index])
        {
          resultString.Insert(0, (char) (48 + index));
          return;
        }
      }
      throw ReaderException.Instance;
    }
  }
}
