// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.UPCEANReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;
using System.Text;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Encapsulates functionality and implementation that is common to UPC and EAN families
  ///             of one-dimensional barcodes.</p>
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>Sean Owen
  ///             </author><author>alasdair@google.com (Alasdair Mackintosh)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class UPCEANReader : OneDReader
  {
    private static readonly int MAX_AVG_VARIANCE = (int) ((double) OneDReader.PATTERN_MATCH_RESULT_SCALE_FACTOR * 0.419999986886978);
    private static readonly int MAX_INDIVIDUAL_VARIANCE = (int) ((double) OneDReader.PATTERN_MATCH_RESULT_SCALE_FACTOR * 0.699999988079071);
    /// <summary>
    /// Start/end guard pattern.
    /// </summary>
    internal static readonly int[] START_END_PATTERN = new int[3]
    {
      1,
      1,
      1
    };
    /// <summary>
    /// Pattern marking the middle of a UPC/EAN pattern, separating the two halves.
    /// </summary>
    internal static readonly int[] MIDDLE_PATTERN = new int[5]
    {
      1,
      1,
      1,
      1,
      1
    };
    /// <summary>
    /// "Odd", or "L" patterns used to encode UPC/EAN digits.
    /// </summary>
    internal static readonly int[][] L_PATTERNS = new int[10][]
    {
      new int[4]
      {
        3,
        2,
        1,
        1
      },
      new int[4]
      {
        2,
        2,
        2,
        1
      },
      new int[4]
      {
        2,
        1,
        2,
        2
      },
      new int[4]
      {
        1,
        4,
        1,
        1
      },
      new int[4]
      {
        1,
        1,
        3,
        2
      },
      new int[4]
      {
        1,
        2,
        3,
        1
      },
      new int[4]
      {
        1,
        1,
        1,
        4
      },
      new int[4]
      {
        1,
        3,
        1,
        2
      },
      new int[4]
      {
        1,
        2,
        1,
        3
      },
      new int[4]
      {
        3,
        1,
        1,
        2
      }
    };
    /// <summary>
    /// As above but also including the "even", or "G" patterns used to encode UPC/EAN digits.
    /// </summary>
    internal static int[][] L_AND_G_PATTERNS = new int[20][];
    private StringBuilder decodeRowStringBuffer;

    /// <summary>
    /// Get the format of this decoder.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The 1D format.
    /// 
    /// </returns>
    internal abstract BarcodeFormat BarcodeFormat { get; }

    static UPCEANReader()
    {
      for (int index = 0; index < 10; ++index)
        UPCEANReader.L_AND_G_PATTERNS[index] = UPCEANReader.L_PATTERNS[index];
      for (int index1 = 10; index1 < 20; ++index1)
      {
        int[] numArray1 = UPCEANReader.L_PATTERNS[index1 - 10];
        int[] numArray2 = new int[numArray1.Length];
        for (int index2 = 0; index2 < numArray1.Length; ++index2)
          numArray2[index2] = numArray1[numArray1.Length - index2 - 1];
        UPCEANReader.L_AND_G_PATTERNS[index1] = numArray2;
      }
    }

    protected internal UPCEANReader()
    {
      this.decodeRowStringBuffer = new StringBuilder(20);
    }

    internal static int[] findStartGuardPattern(com.google.zxing.common.BitArray row)
    {
      bool flag = false;
      int[] numArray = (int[]) null;
      int rowOffset = 0;
      while (!flag)
      {
        numArray = UPCEANReader.findGuardPattern(row, rowOffset, false, UPCEANReader.START_END_PATTERN);
        int end = numArray[0];
        rowOffset = numArray[1];
        int start = end - (rowOffset - end);
        if (start >= 0)
          flag = row.isRange(start, end, false);
      }
      return numArray;
    }

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints)
    {
      return this.decodeRow(rowNumber, row, UPCEANReader.findStartGuardPattern(row), hints);
    }

    /// <summary>
    /// <p>Like {@link #decodeRow(int, BitArray, java.util.Hashtable)}, but
    ///             allows caller to inform method about where the UPC/EAN start pattern is
    ///             found. This allows this to be computed once and reused across many implementations.</p>
    /// </summary>
    public virtual Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, int[] startGuardRange, Hashtable hints)
    {
      ResultPointCallback resultPointCallback = hints == null ? (ResultPointCallback) null : (ResultPointCallback) hints[(object) DecodeHintType.NEED_RESULT_POINT_CALLBACK];
      if (resultPointCallback != null)
        resultPointCallback.foundPossibleResultPoint(new ResultPoint((float) (startGuardRange[0] + startGuardRange[1]) / 2f, (float) rowNumber));
      StringBuilder resultString = this.decodeRowStringBuffer;
      resultString.Length = 0;
      int endStart = this.decodeMiddle(row, startGuardRange, resultString);
      if (resultPointCallback != null)
        resultPointCallback.foundPossibleResultPoint(new ResultPoint((float) endStart, (float) rowNumber));
      int[] numArray = this.decodeEnd(row, endStart);
      if (resultPointCallback != null)
        resultPointCallback.foundPossibleResultPoint(new ResultPoint((float) (numArray[0] + numArray[1]) / 2f, (float) rowNumber));
      int start = numArray[1];
      int end = start + (start - numArray[0]);
      if (end >= row.Size || !row.isRange(start, end, false))
        throw ReaderException.Instance;
      string str = resultString.ToString();
      if (!this.checkChecksum(str))
        throw ReaderException.Instance;
      float x1 = (float) (startGuardRange[1] + startGuardRange[0]) / 2f;
      float x2 = (float) (numArray[1] + numArray[0]) / 2f;
      return new Result(str, (sbyte[]) null, new ResultPoint[2]
      {
        new ResultPoint(x1, (float) rowNumber),
        new ResultPoint(x2, (float) rowNumber)
      }, this.BarcodeFormat);
    }

    /// <returns>
    /// {@link #checkStandardUPCEANChecksum(String)}
    /// 
    /// </returns>
    protected internal virtual bool checkChecksum(string s)
    {
      return UPCEANReader.checkStandardUPCEANChecksum(s);
    }

    /// <summary>
    /// Computes the UPC/EAN checksum on a string of digits, and reports
    ///             whether the checksum is correct or not.
    /// 
    /// 
    /// </summary>
    /// <param name="s">string of digits to check
    ///             </param>
    /// <returns>
    /// true iff string of digits passes the UPC/EAN checksum algorithm
    /// 
    /// </returns>
    /// <throws>ReaderException if the string does not contain only digits </throws>
    private static bool checkStandardUPCEANChecksum(string s)
    {
      int length = s.Length;
      if (length == 0)
        return false;
      int num1 = 0;
      int index1 = length - 2;
      while (index1 >= 0)
      {
        int num2 = (int) s[index1] - 48;
        if (num2 < 0 || num2 > 9)
          throw ReaderException.Instance;
        num1 += num2;
        index1 -= 2;
      }
      int num3 = num1 * 3;
      int index2 = length - 1;
      while (index2 >= 0)
      {
        int num2 = (int) s[index2] - 48;
        if (num2 < 0 || num2 > 9)
          throw ReaderException.Instance;
        num3 += num2;
        index2 -= 2;
      }
      return num3 % 10 == 0;
    }

    protected internal virtual int[] decodeEnd(com.google.zxing.common.BitArray row, int endStart)
    {
      return UPCEANReader.findGuardPattern(row, endStart, false, UPCEANReader.START_END_PATTERN);
    }

    /// <param name="row">row of black/white values to search
    ///             </param><param name="rowOffset">position to start search
    ///             </param><param name="whiteFirst">if true, indicates that the pattern specifies white/black/white/...
    ///             pixel counts, otherwise, it is interpreted as black/white/black/...
    ///             </param><param name="pattern">pattern of counts of number of black and white pixels that are being
    ///             searched for as a pattern
    ///             </param>
    /// <returns>
    /// start/end horizontal offset of guard pattern, as an array of two ints
    /// 
    /// </returns>
    /// <throws>ReaderException if pattern is not found </throws>
    internal static int[] findGuardPattern(com.google.zxing.common.BitArray row, int rowOffset, bool whiteFirst, int[] pattern)
    {
      int length = pattern.Length;
      int[] counters = new int[length];
      int size = row.Size;
      bool flag = false;
      for (; rowOffset < size; ++rowOffset)
      {
        flag = !row.get_Renamed(rowOffset);
        if (whiteFirst == flag)
          break;
      }
      int index1 = 0;
      int num = rowOffset;
      for (int i = rowOffset; i < size; ++i)
      {
        if (row.get_Renamed(i) ^ flag)
        {
          ++counters[index1];
        }
        else
        {
          if (index1 == length - 1)
          {
            if (OneDReader.patternMatchVariance(counters, pattern, UPCEANReader.MAX_INDIVIDUAL_VARIANCE) < UPCEANReader.MAX_AVG_VARIANCE)
              return new int[2]
              {
                num,
                i
              };
            num += counters[0] + counters[1];
            for (int index2 = 2; index2 < length; ++index2)
              counters[index2 - 2] = counters[index2];
            counters[length - 2] = 0;
            counters[length - 1] = 0;
            --index1;
          }
          else
            ++index1;
          counters[index1] = 1;
          flag = !flag;
        }
      }
      throw ReaderException.Instance;
    }

    /// <summary>
    /// Attempts to decode a single UPC/EAN-encoded digit.
    /// 
    /// 
    /// </summary>
    /// <param name="row">row of black/white values to decode
    ///             </param><param name="counters">the counts of runs of observed black/white/black/... values
    ///             </param><param name="rowOffset">horizontal offset to start decoding from
    ///             </param><param name="patterns">the set of patterns to use to decode -- sometimes different encodings
    ///             for the digits 0-9 are used, and this indicates the encodings for 0 to 9 that should
    ///             be used
    ///             </param>
    /// <returns>
    /// horizontal offset of first pixel beyond the decoded digit
    /// 
    /// </returns>
    /// <throws>ReaderException if digit cannot be decoded </throws>
    internal static int decodeDigit(com.google.zxing.common.BitArray row, int[] counters, int rowOffset, int[][] patterns)
    {
      OneDReader.recordPattern(row, rowOffset, counters);
      int num1 = UPCEANReader.MAX_AVG_VARIANCE;
      int num2 = -1;
      int length = patterns.Length;
      for (int index = 0; index < length; ++index)
      {
        int[] pattern = patterns[index];
        int num3 = OneDReader.patternMatchVariance(counters, pattern, UPCEANReader.MAX_INDIVIDUAL_VARIANCE);
        if (num3 < num1)
        {
          num1 = num3;
          num2 = index;
        }
      }
      if (num2 >= 0)
        return num2;
      throw ReaderException.Instance;
    }

    /// <summary>
    /// Subclasses override this to decode the portion of a barcode between the start
    ///             and end guard patterns.
    /// 
    /// 
    /// </summary>
    /// <param name="row">row of black/white values to search
    ///             </param><param name="startRange">start/end offset of start guard pattern
    ///             </param><param name="resultString">{@link StringBuffer} to append decoded chars to
    ///             </param>
    /// <returns>
    /// horizontal offset of first pixel after the "middle" that was decoded
    /// 
    /// </returns>
    /// <throws>ReaderException if decoding could not complete successfully </throws>
    protected internal abstract int decodeMiddle(com.google.zxing.common.BitArray row, int[] startRange, StringBuilder resultString);
  }
}
