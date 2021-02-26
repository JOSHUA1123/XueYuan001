// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.ITFReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;
using System.Text;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Implements decoding of the ITF format.</p><p>"ITF" stands for Interleaved Two of Five. This Reader will scan ITF barcode with 6, 10 or 14
  ///             digits. The checksum is optional and is not applied by this Reader. The consumer of the decoded
  ///             value will have to apply a checksum if required.</p><p><a href="http://en.wikipedia.org/wiki/Interleaved_2_of_5">http://en.wikipedia.org/wiki/Interleaved_2_of_5</a>
  ///             is a great reference for Interleaved 2 of 5 information.</p>
  /// </summary>
  /// <author>kevin.osullivan@sita.aero, SITA Lab.
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ITFReader : OneDReader
  {
    private static readonly int MAX_AVG_VARIANCE = (int) ((double) OneDReader.PATTERN_MATCH_RESULT_SCALE_FACTOR * 0.419999986886978);
    private static readonly int MAX_INDIVIDUAL_VARIANCE = (int) ((double) OneDReader.PATTERN_MATCH_RESULT_SCALE_FACTOR * 0.800000011920929);
    private static readonly int[] DEFAULT_ALLOWED_LENGTHS = new int[4]
    {
      6,
      10,
      14,
      44
    };
    /// <summary>
    /// Start/end guard pattern.
    /// 
    ///             Note: The end pattern is reversed because the row is reversed before
    ///             searching for the END_PATTERN
    /// 
    /// </summary>
    private static readonly int[] START_PATTERN = new int[4]
    {
      1,
      1,
      1,
      1
    };
    private static readonly int[] END_PATTERN_REVERSED = new int[3]
    {
      1,
      1,
      3
    };
    /// <summary>
    /// Patterns of Wide / Narrow lines to indicate each digit
    /// </summary>
    private static readonly int[][] PATTERNS = new int[10][]
    {
      new int[5]
      {
        1,
        1,
        3,
        3,
        1
      },
      new int[5]
      {
        3,
        1,
        1,
        1,
        3
      },
      new int[5]
      {
        1,
        3,
        1,
        1,
        3
      },
      new int[5]
      {
        3,
        3,
        1,
        1,
        1
      },
      new int[5]
      {
        1,
        1,
        3,
        1,
        3
      },
      new int[5]
      {
        3,
        1,
        3,
        1,
        1
      },
      new int[5]
      {
        1,
        3,
        3,
        1,
        1
      },
      new int[5]
      {
        1,
        1,
        1,
        3,
        3
      },
      new int[5]
      {
        3,
        1,
        1,
        3,
        1
      },
      new int[5]
      {
        1,
        3,
        1,
        3,
        1
      }
    };
    private int narrowLineWidth = -1;
    private const int W = 3;
    private const int N = 1;

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints)
    {
      int[] numArray1 = this.decodeStart(row);
      int[] numArray2 = this.decodeEnd(row);
      StringBuilder resultString = new StringBuilder(20);
      ITFReader.decodeMiddle(row, numArray1[1], numArray2[0], resultString);
      string text = resultString.ToString();
      int[] numArray3 = (int[]) null;
      if (hints != null)
        numArray3 = (int[]) hints[(object) DecodeHintType.ALLOWED_LENGTHS];
      if (numArray3 == null)
        numArray3 = ITFReader.DEFAULT_ALLOWED_LENGTHS;
      int length = text.Length;
      bool flag = false;
      for (int index = 0; index < numArray3.Length; ++index)
      {
        if (length == numArray3[index])
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        throw ReaderException.Instance;
      return new Result(text, (sbyte[]) null, new ResultPoint[2]
      {
        new ResultPoint((float) numArray1[1], (float) rowNumber),
        new ResultPoint((float) numArray2[0], (float) rowNumber)
      }, BarcodeFormat.ITF);
    }

    /// <param name="row">row of black/white values to search
    ///             </param><param name="payloadStart">offset of start pattern
    ///             </param><param name="resultString">{@link StringBuffer} to append decoded chars to
    ///             </param><throws>ReaderException if decoding could not complete successfully </throws>
    private static void decodeMiddle(com.google.zxing.common.BitArray row, int payloadStart, int payloadEnd, StringBuilder resultString)
    {
      int[] counters1 = new int[10];
      int[] counters2 = new int[5];
      int[] counters3 = new int[5];
      while (payloadStart < payloadEnd)
      {
        OneDReader.recordPattern(row, payloadStart, counters1);
        for (int index1 = 0; index1 < 5; ++index1)
        {
          int index2 = index1 << 1;
          counters2[index1] = counters1[index2];
          counters3[index1] = counters1[index2 + 1];
        }
        int num1 = ITFReader.decodeDigit(counters2);
        resultString.Append((char) (48 + num1));
        int num2 = ITFReader.decodeDigit(counters3);
        resultString.Append((char) (48 + num2));
        for (int index = 0; index < counters1.Length; ++index)
          payloadStart += counters1[index];
      }
    }

    /// <summary>
    /// Identify where the start of the middle / payload section starts.
    /// 
    /// 
    /// </summary>
    /// <param name="row">row of black/white values to search
    ///             </param>
    /// <returns>
    /// Array, containing index of start of 'start block' and end of
    ///             'start block'
    /// 
    /// </returns>
    /// <throws>ReaderException </throws>
    internal int[] decodeStart(com.google.zxing.common.BitArray row)
    {
      int rowOffset = ITFReader.skipWhiteSpace(row);
      int[] guardPattern = ITFReader.findGuardPattern(row, rowOffset, ITFReader.START_PATTERN);
      this.narrowLineWidth = guardPattern[1] - guardPattern[0] >> 2;
      this.validateQuietZone(row, guardPattern[0]);
      return guardPattern;
    }

    private void validateQuietZone(com.google.zxing.common.BitArray row, int startPattern)
    {
      int num = this.narrowLineWidth * 10;
      for (int i = startPattern - 1; num > 0 && i >= 0 && !row.get_Renamed(i); --i)
        --num;
      if (num != 0)
        throw ReaderException.Instance;
    }

    /// <summary>
    /// Skip all whitespace until we get to the first black line.
    /// 
    /// 
    /// </summary>
    /// <param name="row">row of black/white values to search
    ///             </param>
    /// <returns>
    /// index of the first black line.
    /// 
    /// </returns>
    /// <throws>ReaderException Throws exception if no black lines are found in the row </throws>
    private static int skipWhiteSpace(com.google.zxing.common.BitArray row)
    {
      int size = row.Size;
      int i = 0;
      while (i < size && !row.get_Renamed(i))
        ++i;
      if (i == size)
        throw ReaderException.Instance;
      return i;
    }

    /// <summary>
    /// Identify where the end of the middle / payload section ends.
    /// 
    /// 
    /// </summary>
    /// <param name="row">row of black/white values to search
    ///             </param>
    /// <returns>
    /// Array, containing index of start of 'end block' and end of 'end
    ///             block'
    /// 
    /// </returns>
    /// <throws>ReaderException </throws>
    internal int[] decodeEnd(com.google.zxing.common.BitArray row)
    {
      row.reverse();
      try
      {
        int rowOffset = ITFReader.skipWhiteSpace(row);
        int[] guardPattern = ITFReader.findGuardPattern(row, rowOffset, ITFReader.END_PATTERN_REVERSED);
        this.validateQuietZone(row, guardPattern[0]);
        int num = guardPattern[0];
        guardPattern[0] = row.Size - guardPattern[1];
        guardPattern[1] = row.Size - num;
        return guardPattern;
      }
      finally
      {
        row.reverse();
      }
    }

    /// <param name="row">row of black/white values to search
    ///             </param><param name="rowOffset">position to start search
    ///             </param><param name="pattern">pattern of counts of number of black and white pixels that are
    ///             being searched for as a pattern
    ///             </param>
    /// <returns>
    /// start/end horizontal offset of guard pattern, as an array of two
    ///             ints
    /// 
    /// </returns>
    /// <throws>ReaderException if pattern is not found </throws>
    private static int[] findGuardPattern(com.google.zxing.common.BitArray row, int rowOffset, int[] pattern)
    {
      int length = pattern.Length;
      int[] counters = new int[length];
      int size = row.Size;
      bool flag = false;
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
            if (OneDReader.patternMatchVariance(counters, pattern, ITFReader.MAX_INDIVIDUAL_VARIANCE) < ITFReader.MAX_AVG_VARIANCE)
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
    /// Attempts to decode a sequence of ITF black/white lines into single
    ///             digit.
    /// 
    /// 
    /// </summary>
    /// <param name="counters">the counts of runs of observed black/white/black/... values
    ///             </param>
    /// <returns>
    /// The decoded digit
    /// 
    /// </returns>
    /// <throws>ReaderException if digit cannot be decoded </throws>
    private static int decodeDigit(int[] counters)
    {
      int num1 = ITFReader.MAX_AVG_VARIANCE;
      int num2 = -1;
      int length = ITFReader.PATTERNS.Length;
      for (int index = 0; index < length; ++index)
      {
        int[] pattern = ITFReader.PATTERNS[index];
        int num3 = OneDReader.patternMatchVariance(counters, pattern, ITFReader.MAX_INDIVIDUAL_VARIANCE);
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
  }
}
