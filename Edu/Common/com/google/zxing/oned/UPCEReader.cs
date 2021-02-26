// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.UPCEReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System.Text;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Implements decoding of the UPC-E format.</p><p/><p><a href="http://www.barcodeisland.com/upce.phtml">This</a> is a great reference for
  ///             UPC-E information.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class UPCEReader : UPCEANReader
  {
    /// <summary>
    /// The pattern that marks the middle, and end, of a UPC-E pattern.
    ///             There is no "second half" to a UPC-E barcode.
    /// 
    /// </summary>
    private static readonly int[] MIDDLE_END_PATTERN = new int[6]
    {
      1,
      1,
      1,
      1,
      1,
      1
    };
    /// <summary>
    /// See {@link #L_AND_G_PATTERNS}; these values similarly represent patterns of
    ///             even-odd parity encodings of digits that imply both the number system (0 or 1)
    ///             used, and the check digit.
    /// 
    /// </summary>
    private static readonly int[][] NUMSYS_AND_CHECK_DIGIT_PATTERNS = new int[2][]
    {
      new int[10]
      {
        56,
        52,
        50,
        49,
        44,
        38,
        35,
        42,
        41,
        37
      },
      new int[10]
      {
        7,
        11,
        13,
        14,
        19,
        25,
        28,
        21,
        22,
        26
      }
    };
    private int[] decodeMiddleCounters;

    internal override BarcodeFormat BarcodeFormat
    {
      get
      {
        return BarcodeFormat.UPC_E;
      }
    }

    public UPCEReader()
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
      int rowOffset = startRange[1];
      int lgPatternFound = 0;
      for (int index1 = 0; index1 < 6 && rowOffset < size; ++index1)
      {
        int num = UPCEANReader.decodeDigit(row, counters, rowOffset, UPCEANReader.L_AND_G_PATTERNS);
        result.Append((char) (48 + num % 10));
        for (int index2 = 0; index2 < counters.Length; ++index2)
          rowOffset += counters[index2];
        if (num >= 10)
          lgPatternFound |= 1 << 5 - index1;
      }
      UPCEReader.determineNumSysAndCheckDigit(result, lgPatternFound);
      return rowOffset;
    }

    protected internal override int[] decodeEnd(BitArray row, int endStart)
    {
      return UPCEANReader.findGuardPattern(row, endStart, true, UPCEReader.MIDDLE_END_PATTERN);
    }

    protected internal override bool checkChecksum(string s)
    {
      return base.checkChecksum(UPCEReader.convertUPCEtoUPCA(s));
    }

    private static void determineNumSysAndCheckDigit(StringBuilder resultString, int lgPatternFound)
    {
      for (int index1 = 0; index1 <= 1; ++index1)
      {
        for (int index2 = 0; index2 < 10; ++index2)
        {
          if (lgPatternFound == UPCEReader.NUMSYS_AND_CHECK_DIGIT_PATTERNS[index1][index2])
          {
            resultString.Insert(0, (char) (48 + index1));
            resultString.Append((char) (48 + index2));
            return;
          }
        }
      }
      throw ReaderException.Instance;
    }

    /// <summary>
    /// Expands a UPC-E value back into its full, equivalent UPC-A code value.
    /// 
    /// 
    /// </summary>
    /// <param name="upce">UPC-E code as string of digits
    ///             </param>
    /// <returns>
    /// equivalent UPC-A code as string of digits
    /// 
    /// </returns>
    public static string convertUPCEtoUPCA(string upce)
    {
      char[] destinationArray = new char[6];
      SupportClass.GetCharsFromString(upce, 1, 7, destinationArray, 0);
      StringBuilder stringBuilder = new StringBuilder(12);
      stringBuilder.Append(upce[0]);
      char ch = destinationArray[5];
      switch (ch)
      {
        case '0':
        case '1':
        case '2':
          stringBuilder.Append(destinationArray, 0, 2);
          stringBuilder.Append(ch);
          stringBuilder.Append("0000");
          stringBuilder.Append(destinationArray, 2, 3);
          break;
        case '3':
          stringBuilder.Append(destinationArray, 0, 3);
          stringBuilder.Append("00000");
          stringBuilder.Append(destinationArray, 3, 2);
          break;
        case '4':
          stringBuilder.Append(destinationArray, 0, 4);
          stringBuilder.Append("00000");
          stringBuilder.Append(destinationArray[4]);
          break;
        default:
          stringBuilder.Append(destinationArray, 0, 5);
          stringBuilder.Append("0000");
          stringBuilder.Append(ch);
          break;
      }
      stringBuilder.Append(upce[7]);
      return stringBuilder.ToString();
    }
  }
}
