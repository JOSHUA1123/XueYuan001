// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.Code39Reader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;
using System.Collections;
using System.Text;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Decodes Code 39 barcodes. This does not support "Full ASCII Code 39" yet.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Code39Reader : OneDReader
  {
    private static readonly char[] ALPHABET = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. *$/+%".ToCharArray();
    /// <summary>
    /// These represent the encodings of characters, as patterns of wide and narrow bars.
    ///             The 9 least-significant bits of each int correspond to the pattern of wide and narrow,
    ///             with 1s representing "wide" and 0s representing narrow.
    /// 
    /// </summary>
    internal static readonly int[] CHARACTER_ENCODINGS = new int[44]
    {
      52,
      289,
      97,
      352,
      49,
      304,
      112,
      37,
      292,
      100,
      265,
      73,
      328,
      25,
      280,
      88,
      13,
      268,
      76,
      28,
      259,
      67,
      322,
      19,
      274,
      82,
      7,
      262,
      70,
      22,
      385,
      193,
      448,
      145,
      400,
      208,
      133,
      388,
      196,
      148,
      168,
      162,
      138,
      42
    };
    private static readonly int ASTERISK_ENCODING = Code39Reader.CHARACTER_ENCODINGS[39];
    internal const string ALPHABET_STRING = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. *$/+%";
    private bool usingCheckDigit;
    private bool extendedMode;

    /// <summary>
    /// Creates a reader that assumes all encoded data is data, and does not treat the final
    ///             character as a check digit. It will not decoded "extended Code 39" sequences.
    /// 
    /// </summary>
    public Code39Reader()
    {
      this.usingCheckDigit = false;
      this.extendedMode = false;
    }

    /// <summary>
    /// Creates a reader that can be configured to check the last character as a check digit.
    ///             It will not decoded "extended Code 39" sequences.
    /// 
    /// 
    /// </summary>
    /// <param name="usingCheckDigit">if true, treat the last data character as a check digit, not
    ///             data, and verify that the checksum passes.
    ///             </param>
    public Code39Reader(bool usingCheckDigit)
    {
      this.usingCheckDigit = usingCheckDigit;
      this.extendedMode = false;
    }

    /// <summary>
    /// Creates a reader that can be configured to check the last character as a check digit,
    ///             or optionally attempt to decode "extended Code 39" sequences that are used to encode
    ///             the full ASCII character set.
    /// 
    /// 
    /// </summary>
    /// <param name="usingCheckDigit">if true, treat the last data character as a check digit, not
    ///             data, and verify that the checksum passes.
    ///             </param><param name="extendedMode">if true, will attempt to decode extended Code 39 sequences in the
    ///             text.
    ///             </param>
    public Code39Reader(bool usingCheckDigit, bool extendedMode)
    {
      this.usingCheckDigit = usingCheckDigit;
      this.extendedMode = extendedMode;
    }

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints)
    {
      int[] asteriskPattern = Code39Reader.findAsteriskPattern(row);
      int num1 = asteriskPattern[1];
      int size = row.Size;
      while (num1 < size && !row.get_Renamed(num1))
        ++num1;
      StringBuilder stringBuilder = new StringBuilder(20);
      int[] counters = new int[9];
      char ch;
      int num2;
      do
      {
        OneDReader.recordPattern(row, num1, counters);
        int pattern = Code39Reader.toNarrowWidePattern(counters);
        if (pattern < 0)
          throw ReaderException.Instance;
        ch = Code39Reader.patternToChar(pattern);
        stringBuilder.Append(ch);
        num2 = num1;
        for (int index = 0; index < counters.Length; ++index)
          num1 += counters[index];
        while (num1 < size && !row.get_Renamed(num1))
          ++num1;
      }
      while ((int) ch != 42);
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      int num3 = 0;
      for (int index = 0; index < counters.Length; ++index)
        num3 += counters[index];
      int num4 = num1 - num2 - num3;
      if (num1 != size && num4 / 2 < num3)
        throw ReaderException.Instance;
      if (this.usingCheckDigit)
      {
        int startIndex = stringBuilder.Length - 1;
        int num5 = 0;
        for (int index = 0; index < startIndex; ++index)
          num5 += "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. *$/+%".IndexOf(stringBuilder[index]);
        if (num5 % 43 != "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. *$/+%".IndexOf(stringBuilder[startIndex]))
          throw ReaderException.Instance;
        stringBuilder.Remove(startIndex, 1);
      }
      string str = stringBuilder.ToString();
      if (this.extendedMode)
        str = Code39Reader.decodeExtended(str);
      if (str.Length == 0)
        throw ReaderException.Instance;
      float x1 = (float) (asteriskPattern[1] + asteriskPattern[0]) / 2f;
      float x2 = (float) (num1 + num2) / 2f;
      return new Result(str, (sbyte[]) null, new ResultPoint[2]
      {
        new ResultPoint(x1, (float) rowNumber),
        new ResultPoint(x2, (float) rowNumber)
      }, BarcodeFormat.CODE_39);
    }

    private static int[] findAsteriskPattern(com.google.zxing.common.BitArray row)
    {
      int size = row.Size;
      int i1 = 0;
      while (i1 < size && !row.get_Renamed(i1))
        ++i1;
      int index1 = 0;
      int[] counters = new int[9];
      int end = i1;
      bool flag = false;
      int length = counters.Length;
      for (int i2 = i1; i2 < size; ++i2)
      {
        if (row.get_Renamed(i2) ^ flag)
        {
          ++counters[index1];
        }
        else
        {
          if (index1 == length - 1)
          {
            if (Code39Reader.toNarrowWidePattern(counters) == Code39Reader.ASTERISK_ENCODING && row.isRange(Math.Max(0, end - (i2 - end) / 2), end, false))
              return new int[2]
              {
                end,
                i2
              };
            end += counters[0] + counters[1];
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

    private static int toNarrowWidePattern(int[] counters)
    {
      int length = counters.Length;
      int num1 = 0;
      int num2;
      do
      {
        int num3 = int.MaxValue;
        for (int index = 0; index < length; ++index)
        {
          int num4 = counters[index];
          if (num4 < num3 && num4 > num1)
            num3 = num4;
        }
        num1 = num3;
        num2 = 0;
        int num5 = 0;
        int num6 = 0;
        for (int index = 0; index < length; ++index)
        {
          int num4 = counters[index];
          if (counters[index] > num1)
          {
            num6 |= 1 << length - 1 - index;
            ++num2;
            num5 += num4;
          }
        }
        if (num2 == 3)
        {
          for (int index = 0; index < length && num2 > 0; ++index)
          {
            int num4 = counters[index];
            if (counters[index] > num1)
            {
              --num2;
              if (num4 << 1 >= num5)
                return -1;
            }
          }
          return num6;
        }
      }
      while (num2 > 3);
      return -1;
    }

    private static char patternToChar(int pattern)
    {
      for (int index = 0; index < Code39Reader.CHARACTER_ENCODINGS.Length; ++index)
      {
        if (Code39Reader.CHARACTER_ENCODINGS[index] == pattern)
          return Code39Reader.ALPHABET[index];
      }
      throw ReaderException.Instance;
    }

    private static string decodeExtended(string encoded)
    {
      int length = encoded.Length;
      StringBuilder stringBuilder = new StringBuilder(length);
      for (int index = 0; index < length; ++index)
      {
        char ch1 = encoded[index];
        switch (ch1)
        {
          case '+':
          case '$':
          case '%':
          case '/':
            char ch2 = encoded[index + 1];
            char ch3 = char.MinValue;
            switch (ch1)
            {
              case '$':
                if ((int) ch2 < 65 || (int) ch2 > 90)
                  throw ReaderException.Instance;
                ch3 = (char) ((uint) ch2 - 64U);
                break;
              case '%':
                if ((int) ch2 >= 65 && (int) ch2 <= 69)
                {
                  ch3 = (char) ((uint) ch2 - 38U);
                  break;
                }
                if ((int) ch2 < 70 || (int) ch2 > 87)
                  throw ReaderException.Instance;
                ch3 = (char) ((uint) ch2 - 11U);
                break;
              case '+':
                if ((int) ch2 < 65 || (int) ch2 > 90)
                  throw ReaderException.Instance;
                ch3 = (char) ((uint) ch2 + 32U);
                break;
              case '/':
                if ((int) ch2 >= 65 && (int) ch2 <= 79)
                {
                  ch3 = (char) ((uint) ch2 - 32U);
                  break;
                }
                if ((int) ch2 != 90)
                  throw ReaderException.Instance;
                ch3 = ':';
                break;
            }
            stringBuilder.Append(ch3);
            ++index;
            break;
          default:
            stringBuilder.Append(ch1);
            break;
        }
      }
      return stringBuilder.ToString();
    }
  }
}
