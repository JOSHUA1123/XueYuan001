// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.Code128Reader
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
  /// <p>Decodes Code 128 barcodes.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Code128Reader : OneDReader
  {
    private static readonly int[][] CODE_PATTERNS = new int[107][]
    {
      new int[6]
      {
        2,
        1,
        2,
        2,
        2,
        2
      },
      new int[6]
      {
        2,
        2,
        2,
        1,
        2,
        2
      },
      new int[6]
      {
        2,
        2,
        2,
        2,
        2,
        1
      },
      new int[6]
      {
        1,
        2,
        1,
        2,
        2,
        3
      },
      new int[6]
      {
        1,
        2,
        1,
        3,
        2,
        2
      },
      new int[6]
      {
        1,
        3,
        1,
        2,
        2,
        2
      },
      new int[6]
      {
        1,
        2,
        2,
        2,
        1,
        3
      },
      new int[6]
      {
        1,
        2,
        2,
        3,
        1,
        2
      },
      new int[6]
      {
        1,
        3,
        2,
        2,
        1,
        2
      },
      new int[6]
      {
        2,
        2,
        1,
        2,
        1,
        3
      },
      new int[6]
      {
        2,
        2,
        1,
        3,
        1,
        2
      },
      new int[6]
      {
        2,
        3,
        1,
        2,
        1,
        2
      },
      new int[6]
      {
        1,
        1,
        2,
        2,
        3,
        2
      },
      new int[6]
      {
        1,
        2,
        2,
        1,
        3,
        2
      },
      new int[6]
      {
        1,
        2,
        2,
        2,
        3,
        1
      },
      new int[6]
      {
        1,
        1,
        3,
        2,
        2,
        2
      },
      new int[6]
      {
        1,
        2,
        3,
        1,
        2,
        2
      },
      new int[6]
      {
        1,
        2,
        3,
        2,
        2,
        1
      },
      new int[6]
      {
        2,
        2,
        3,
        2,
        1,
        1
      },
      new int[6]
      {
        2,
        2,
        1,
        1,
        3,
        2
      },
      new int[6]
      {
        2,
        2,
        1,
        2,
        3,
        1
      },
      new int[6]
      {
        2,
        1,
        3,
        2,
        1,
        2
      },
      new int[6]
      {
        2,
        2,
        3,
        1,
        1,
        2
      },
      new int[6]
      {
        3,
        1,
        2,
        1,
        3,
        1
      },
      new int[6]
      {
        3,
        1,
        1,
        2,
        2,
        2
      },
      new int[6]
      {
        3,
        2,
        1,
        1,
        2,
        2
      },
      new int[6]
      {
        3,
        2,
        1,
        2,
        2,
        1
      },
      new int[6]
      {
        3,
        1,
        2,
        2,
        1,
        2
      },
      new int[6]
      {
        3,
        2,
        2,
        1,
        1,
        2
      },
      new int[6]
      {
        3,
        2,
        2,
        2,
        1,
        1
      },
      new int[6]
      {
        2,
        1,
        2,
        1,
        2,
        3
      },
      new int[6]
      {
        2,
        1,
        2,
        3,
        2,
        1
      },
      new int[6]
      {
        2,
        3,
        2,
        1,
        2,
        1
      },
      new int[6]
      {
        1,
        1,
        1,
        3,
        2,
        3
      },
      new int[6]
      {
        1,
        3,
        1,
        1,
        2,
        3
      },
      new int[6]
      {
        1,
        3,
        1,
        3,
        2,
        1
      },
      new int[6]
      {
        1,
        1,
        2,
        3,
        1,
        3
      },
      new int[6]
      {
        1,
        3,
        2,
        1,
        1,
        3
      },
      new int[6]
      {
        1,
        3,
        2,
        3,
        1,
        1
      },
      new int[6]
      {
        2,
        1,
        1,
        3,
        1,
        3
      },
      new int[6]
      {
        2,
        3,
        1,
        1,
        1,
        3
      },
      new int[6]
      {
        2,
        3,
        1,
        3,
        1,
        1
      },
      new int[6]
      {
        1,
        1,
        2,
        1,
        3,
        3
      },
      new int[6]
      {
        1,
        1,
        2,
        3,
        3,
        1
      },
      new int[6]
      {
        1,
        3,
        2,
        1,
        3,
        1
      },
      new int[6]
      {
        1,
        1,
        3,
        1,
        2,
        3
      },
      new int[6]
      {
        1,
        1,
        3,
        3,
        2,
        1
      },
      new int[6]
      {
        1,
        3,
        3,
        1,
        2,
        1
      },
      new int[6]
      {
        3,
        1,
        3,
        1,
        2,
        1
      },
      new int[6]
      {
        2,
        1,
        1,
        3,
        3,
        1
      },
      new int[6]
      {
        2,
        3,
        1,
        1,
        3,
        1
      },
      new int[6]
      {
        2,
        1,
        3,
        1,
        1,
        3
      },
      new int[6]
      {
        2,
        1,
        3,
        3,
        1,
        1
      },
      new int[6]
      {
        2,
        1,
        3,
        1,
        3,
        1
      },
      new int[6]
      {
        3,
        1,
        1,
        1,
        2,
        3
      },
      new int[6]
      {
        3,
        1,
        1,
        3,
        2,
        1
      },
      new int[6]
      {
        3,
        3,
        1,
        1,
        2,
        1
      },
      new int[6]
      {
        3,
        1,
        2,
        1,
        1,
        3
      },
      new int[6]
      {
        3,
        1,
        2,
        3,
        1,
        1
      },
      new int[6]
      {
        3,
        3,
        2,
        1,
        1,
        1
      },
      new int[6]
      {
        3,
        1,
        4,
        1,
        1,
        1
      },
      new int[6]
      {
        2,
        2,
        1,
        4,
        1,
        1
      },
      new int[6]
      {
        4,
        3,
        1,
        1,
        1,
        1
      },
      new int[6]
      {
        1,
        1,
        1,
        2,
        2,
        4
      },
      new int[6]
      {
        1,
        1,
        1,
        4,
        2,
        2
      },
      new int[6]
      {
        1,
        2,
        1,
        1,
        2,
        4
      },
      new int[6]
      {
        1,
        2,
        1,
        4,
        2,
        1
      },
      new int[6]
      {
        1,
        4,
        1,
        1,
        2,
        2
      },
      new int[6]
      {
        1,
        4,
        1,
        2,
        2,
        1
      },
      new int[6]
      {
        1,
        1,
        2,
        2,
        1,
        4
      },
      new int[6]
      {
        1,
        1,
        2,
        4,
        1,
        2
      },
      new int[6]
      {
        1,
        2,
        2,
        1,
        1,
        4
      },
      new int[6]
      {
        1,
        2,
        2,
        4,
        1,
        1
      },
      new int[6]
      {
        1,
        4,
        2,
        1,
        1,
        2
      },
      new int[6]
      {
        1,
        4,
        2,
        2,
        1,
        1
      },
      new int[6]
      {
        2,
        4,
        1,
        2,
        1,
        1
      },
      new int[6]
      {
        2,
        2,
        1,
        1,
        1,
        4
      },
      new int[6]
      {
        4,
        1,
        3,
        1,
        1,
        1
      },
      new int[6]
      {
        2,
        4,
        1,
        1,
        1,
        2
      },
      new int[6]
      {
        1,
        3,
        4,
        1,
        1,
        1
      },
      new int[6]
      {
        1,
        1,
        1,
        2,
        4,
        2
      },
      new int[6]
      {
        1,
        2,
        1,
        1,
        4,
        2
      },
      new int[6]
      {
        1,
        2,
        1,
        2,
        4,
        1
      },
      new int[6]
      {
        1,
        1,
        4,
        2,
        1,
        2
      },
      new int[6]
      {
        1,
        2,
        4,
        1,
        1,
        2
      },
      new int[6]
      {
        1,
        2,
        4,
        2,
        1,
        1
      },
      new int[6]
      {
        4,
        1,
        1,
        2,
        1,
        2
      },
      new int[6]
      {
        4,
        2,
        1,
        1,
        1,
        2
      },
      new int[6]
      {
        4,
        2,
        1,
        2,
        1,
        1
      },
      new int[6]
      {
        2,
        1,
        2,
        1,
        4,
        1
      },
      new int[6]
      {
        2,
        1,
        4,
        1,
        2,
        1
      },
      new int[6]
      {
        4,
        1,
        2,
        1,
        2,
        1
      },
      new int[6]
      {
        1,
        1,
        1,
        1,
        4,
        3
      },
      new int[6]
      {
        1,
        1,
        1,
        3,
        4,
        1
      },
      new int[6]
      {
        1,
        3,
        1,
        1,
        4,
        1
      },
      new int[6]
      {
        1,
        1,
        4,
        1,
        1,
        3
      },
      new int[6]
      {
        1,
        1,
        4,
        3,
        1,
        1
      },
      new int[6]
      {
        4,
        1,
        1,
        1,
        1,
        3
      },
      new int[6]
      {
        4,
        1,
        1,
        3,
        1,
        1
      },
      new int[6]
      {
        1,
        1,
        3,
        1,
        4,
        1
      },
      new int[6]
      {
        1,
        1,
        4,
        1,
        3,
        1
      },
      new int[6]
      {
        3,
        1,
        1,
        1,
        4,
        1
      },
      new int[6]
      {
        4,
        1,
        1,
        1,
        3,
        1
      },
      new int[6]
      {
        2,
        1,
        1,
        4,
        1,
        2
      },
      new int[6]
      {
        2,
        1,
        1,
        2,
        1,
        4
      },
      new int[6]
      {
        2,
        1,
        1,
        2,
        3,
        2
      },
      new int[7]
      {
        2,
        3,
        3,
        1,
        1,
        1,
        2
      }
    };
    private static readonly int MAX_AVG_VARIANCE = (int) ((double) OneDReader.PATTERN_MATCH_RESULT_SCALE_FACTOR * 0.25);
    private static readonly int MAX_INDIVIDUAL_VARIANCE = (int) ((double) OneDReader.PATTERN_MATCH_RESULT_SCALE_FACTOR * 0.699999988079071);
    private const int CODE_SHIFT = 98;
    private const int CODE_CODE_C = 99;
    private const int CODE_CODE_B = 100;
    private const int CODE_CODE_A = 101;
    private const int CODE_FNC_1 = 102;
    private const int CODE_FNC_2 = 97;
    private const int CODE_FNC_3 = 96;
    private const int CODE_FNC_4_A = 101;
    private const int CODE_FNC_4_B = 100;
    private const int CODE_START_A = 103;
    private const int CODE_START_B = 104;
    private const int CODE_START_C = 105;
    private const int CODE_STOP = 106;

    private static int[] findStartPattern(com.google.zxing.common.BitArray row)
    {
      int size = row.Size;
      int i1 = 0;
      while (i1 < size && !row.get_Renamed(i1))
        ++i1;
      int index1 = 0;
      int[] counters = new int[6];
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
            int num1 = Code128Reader.MAX_AVG_VARIANCE;
            int num2 = -1;
            for (int index2 = 103; index2 <= 105; ++index2)
            {
              int num3 = OneDReader.patternMatchVariance(counters, Code128Reader.CODE_PATTERNS[index2], Code128Reader.MAX_INDIVIDUAL_VARIANCE);
              if (num3 < num1)
              {
                num1 = num3;
                num2 = index2;
              }
            }
            if (num2 >= 0 && row.isRange(Math.Max(0, end - (i2 - end) / 2), end, false))
              return new int[3]
              {
                end,
                i2,
                num2
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

    private static int decodeCode(com.google.zxing.common.BitArray row, int[] counters, int rowOffset)
    {
      OneDReader.recordPattern(row, rowOffset, counters);
      int num1 = Code128Reader.MAX_AVG_VARIANCE;
      int num2 = -1;
      for (int index = 0; index < Code128Reader.CODE_PATTERNS.Length; ++index)
      {
        int[] pattern = Code128Reader.CODE_PATTERNS[index];
        int num3 = OneDReader.patternMatchVariance(counters, pattern, Code128Reader.MAX_INDIVIDUAL_VARIANCE);
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

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints)
    {
      int[] startPattern = Code128Reader.findStartPattern(row);
      int num1 = startPattern[2];
      int num2;
      switch (num1)
      {
        case 103:
          num2 = 101;
          break;
        case 104:
          num2 = 100;
          break;
        case 105:
          num2 = 99;
          break;
        default:
          throw ReaderException.Instance;
      }
      bool flag1 = false;
      bool flag2 = false;
      StringBuilder stringBuilder = new StringBuilder(20);
      int num3 = startPattern[0];
      int num4 = startPattern[1];
      int[] counters = new int[6];
      int num5 = 0;
      int num6 = 0;
      int num7 = num1;
      int num8 = 0;
      bool flag3 = true;
      while (!flag1)
      {
        bool flag4 = flag2;
        flag2 = false;
        num5 = num6;
        num6 = Code128Reader.decodeCode(row, counters, num4);
        if (num6 != 106)
          flag3 = true;
        if (num6 != 106)
        {
          ++num8;
          num7 += num8 * num6;
        }
        num3 = num4;
        for (int index = 0; index < counters.Length; ++index)
          num4 += counters[index];
        switch (num6)
        {
          case 103:
          case 104:
          case 105:
            throw ReaderException.Instance;
          default:
            switch (num2)
            {
              case 99:
                if (num6 < 100)
                {
                  if (num6 < 10)
                    stringBuilder.Append('0');
                  stringBuilder.Append(num6);
                  break;
                }
                if (num6 != 106)
                  flag3 = false;
                switch (num6 - 100)
                {
                  case 0:
                    num2 = 100;
                    break;
                  case 1:
                    num2 = 101;
                    break;
                  case 6:
                    flag1 = true;
                    break;
                }
                break;
              case 100:
                if (num6 < 96)
                {
                  stringBuilder.Append((char) (32 + num6));
                  break;
                }
                if (num6 != 106)
                  flag3 = false;
                switch (num6 - 96)
                {
                  case 2:
                    flag2 = true;
                    num2 = 99;
                    break;
                  case 3:
                    num2 = 99;
                    break;
                  case 5:
                    num2 = 101;
                    break;
                  case 10:
                    flag1 = true;
                    break;
                }
                break;
              case 101:
                if (num6 < 64)
                {
                  stringBuilder.Append((char) (32 + num6));
                  break;
                }
                if (num6 < 96)
                {
                  stringBuilder.Append((char) (num6 - 64));
                  break;
                }
                if (num6 != 106)
                  flag3 = false;
                switch (num6 - 96)
                {
                  case 2:
                    flag2 = true;
                    num2 = 100;
                    break;
                  case 3:
                    num2 = 99;
                    break;
                  case 4:
                    num2 = 100;
                    break;
                  case 10:
                    flag1 = true;
                    break;
                }
                break;
            }
            if (flag4)
            {
              switch (num2)
              {
                case 99:
                  num2 = 100;
                  continue;
                case 100:
                  num2 = 101;
                  continue;
                case 101:
                  num2 = 99;
                  continue;
                default:
                  continue;
              }
            }
            else
              continue;
        }
      }
      int size = row.Size;
      while (num4 < size && row.get_Renamed(num4))
        ++num4;
      if (!row.isRange(num4, Math.Min(size, num4 + (num4 - num3) / 2), false))
        throw ReaderException.Instance;
      if ((num7 - num8 * num5) % 103 != num5)
        throw ReaderException.Instance;
      int length = stringBuilder.Length;
      if (length > 0 && flag3)
      {
        if (num2 == 99)
          stringBuilder.Remove(length - 2, length - (length - 2));
        else
          stringBuilder.Remove(length - 1, length - (length - 1));
      }
      string text = stringBuilder.ToString();
      if (text.Length == 0)
        throw ReaderException.Instance;
      float x1 = (float) (startPattern[1] + startPattern[0]) / 2f;
      float x2 = (float) (num4 + num3) / 2f;
      return new Result(text, (sbyte[]) null, new ResultPoint[2]
      {
        new ResultPoint(x1, (float) rowNumber),
        new ResultPoint(x2, (float) rowNumber)
      }, BarcodeFormat.CODE_128);
    }
  }
}
