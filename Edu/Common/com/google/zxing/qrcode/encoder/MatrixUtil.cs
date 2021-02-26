// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.encoder.MatrixUtil
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;

namespace com.google.zxing.qrcode.encoder
{
  /// <author>satorux@google.com (Satoru Takabayashi) - creator
  ///             </author><author>dswitkin@google.com (Daniel Switkin) - ported from C++
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class MatrixUtil
  {
    private const int VERSION_INFO_POLY = 7973;
    private const int TYPE_INFO_POLY = 1335;
    private const int TYPE_INFO_MASK_PATTERN = 21522;
    private static readonly int[][] POSITION_DETECTION_PATTERN;
    private static readonly int[][] HORIZONTAL_SEPARATION_PATTERN;
    private static readonly int[][] VERTICAL_SEPARATION_PATTERN;
    private static readonly int[][] POSITION_ADJUSTMENT_PATTERN;
    private static readonly int[][] POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE;
    private static readonly int[][] TYPE_INFO_COORDINATES;

    static MatrixUtil()
    {
      int[][] numArray1 = new int[7][];
      numArray1[0] = new int[7]
      {
        1,
        1,
        1,
        1,
        1,
        1,
        1
      };
      int[][] numArray2 = numArray1;
      int index1 = 1;
      int[] numArray3 = new int[7];
      numArray3[0] = 1;
      numArray3[6] = 1;
      int[] numArray4 = numArray3;
      numArray2[index1] = numArray4;
      numArray1[2] = new int[7]
      {
        1,
        0,
        1,
        1,
        1,
        0,
        1
      };
      numArray1[3] = new int[7]
      {
        1,
        0,
        1,
        1,
        1,
        0,
        1
      };
      numArray1[4] = new int[7]
      {
        1,
        0,
        1,
        1,
        1,
        0,
        1
      };
      int[][] numArray5 = numArray1;
      int index2 = 5;
      int[] numArray6 = new int[7];
      numArray6[0] = 1;
      numArray6[6] = 1;
      int[] numArray7 = numArray6;
      numArray5[index2] = numArray7;
      numArray1[6] = new int[7]
      {
        1,
        1,
        1,
        1,
        1,
        1,
        1
      };
      MatrixUtil.POSITION_DETECTION_PATTERN = numArray1;
      MatrixUtil.HORIZONTAL_SEPARATION_PATTERN = new int[1][]
      {
        new int[8]
      };
      MatrixUtil.VERTICAL_SEPARATION_PATTERN = new int[7][]
      {
        new int[1],
        new int[1],
        new int[1],
        new int[1],
        new int[1],
        new int[1],
        new int[1]
      };
      int[][] numArray8 = new int[5][];
      numArray8[0] = new int[5]
      {
        1,
        1,
        1,
        1,
        1
      };
      int[][] numArray9 = numArray8;
      int index3 = 1;
      int[] numArray10 = new int[5];
      numArray10[0] = 1;
      numArray10[4] = 1;
      int[] numArray11 = numArray10;
      numArray9[index3] = numArray11;
      numArray8[2] = new int[5]
      {
        1,
        0,
        1,
        0,
        1
      };
      int[][] numArray12 = numArray8;
      int index4 = 3;
      int[] numArray13 = new int[5];
      numArray13[0] = 1;
      numArray13[4] = 1;
      int[] numArray14 = numArray13;
      numArray12[index4] = numArray14;
      numArray8[4] = new int[5]
      {
        1,
        1,
        1,
        1,
        1
      };
      MatrixUtil.POSITION_ADJUSTMENT_PATTERN = numArray8;
      MatrixUtil.POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE = new int[40][]
      {
        new int[7]
        {
          -1,
          -1,
          -1,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          18,
          -1,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          22,
          -1,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          26,
          -1,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          30,
          -1,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          34,
          -1,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          22,
          38,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          24,
          42,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          26,
          46,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          28,
          50,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          30,
          54,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          32,
          58,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          34,
          62,
          -1,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          26,
          46,
          66,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          26,
          48,
          70,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          26,
          50,
          74,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          30,
          54,
          78,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          30,
          56,
          82,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          30,
          58,
          86,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          34,
          62,
          90,
          -1,
          -1,
          -1
        },
        new int[7]
        {
          6,
          28,
          50,
          72,
          94,
          -1,
          -1
        },
        new int[7]
        {
          6,
          26,
          50,
          74,
          98,
          -1,
          -1
        },
        new int[7]
        {
          6,
          30,
          54,
          78,
          102,
          -1,
          -1
        },
        new int[7]
        {
          6,
          28,
          54,
          80,
          106,
          -1,
          -1
        },
        new int[7]
        {
          6,
          32,
          58,
          84,
          110,
          -1,
          -1
        },
        new int[7]
        {
          6,
          30,
          58,
          86,
          114,
          -1,
          -1
        },
        new int[7]
        {
          6,
          34,
          62,
          90,
          118,
          -1,
          -1
        },
        new int[7]
        {
          6,
          26,
          50,
          74,
          98,
          122,
          -1
        },
        new int[7]
        {
          6,
          30,
          54,
          78,
          102,
          126,
          -1
        },
        new int[7]
        {
          6,
          26,
          52,
          78,
          104,
          130,
          -1
        },
        new int[7]
        {
          6,
          30,
          56,
          82,
          108,
          134,
          -1
        },
        new int[7]
        {
          6,
          34,
          60,
          86,
          112,
          138,
          -1
        },
        new int[7]
        {
          6,
          30,
          58,
          86,
          114,
          142,
          -1
        },
        new int[7]
        {
          6,
          34,
          62,
          90,
          118,
          146,
          -1
        },
        new int[7]
        {
          6,
          30,
          54,
          78,
          102,
          126,
          150
        },
        new int[7]
        {
          6,
          24,
          50,
          76,
          102,
          128,
          154
        },
        new int[7]
        {
          6,
          28,
          54,
          80,
          106,
          132,
          158
        },
        new int[7]
        {
          6,
          32,
          58,
          84,
          110,
          136,
          162
        },
        new int[7]
        {
          6,
          26,
          54,
          82,
          110,
          138,
          166
        },
        new int[7]
        {
          6,
          30,
          58,
          86,
          114,
          142,
          170
        }
      };
      MatrixUtil.TYPE_INFO_COORDINATES = new int[15][]
      {
        new int[2]
        {
          8,
          0
        },
        new int[2]
        {
          8,
          1
        },
        new int[2]
        {
          8,
          2
        },
        new int[2]
        {
          8,
          3
        },
        new int[2]
        {
          8,
          4
        },
        new int[2]
        {
          8,
          5
        },
        new int[2]
        {
          8,
          7
        },
        new int[2]
        {
          8,
          8
        },
        new int[2]
        {
          7,
          8
        },
        new int[2]
        {
          5,
          8
        },
        new int[2]
        {
          4,
          8
        },
        new int[2]
        {
          3,
          8
        },
        new int[2]
        {
          2,
          8
        },
        new int[2]
        {
          1,
          8
        },
        new int[2]
        {
          0,
          8
        }
      };
    }

    private MatrixUtil()
    {
    }

    public static void clearMatrix(ByteMatrix matrix)
    {
      matrix.clear((sbyte) -1);
    }

    public static void buildMatrix(BitVector dataBits, ErrorCorrectionLevel ecLevel, int version, int maskPattern, ByteMatrix matrix)
    {
      MatrixUtil.clearMatrix(matrix);
      MatrixUtil.embedBasicPatterns(version, matrix);
      MatrixUtil.embedTypeInfo(ecLevel, maskPattern, matrix);
      MatrixUtil.maybeEmbedVersionInfo(version, matrix);
      MatrixUtil.embedDataBits(dataBits, maskPattern, matrix);
    }

    public static void embedBasicPatterns(int version, ByteMatrix matrix)
    {
      MatrixUtil.embedPositionDetectionPatternsAndSeparators(matrix);
      MatrixUtil.embedDarkDotAtLeftBottomCorner(matrix);
      MatrixUtil.maybeEmbedPositionAdjustmentPatterns(version, matrix);
      MatrixUtil.embedTimingPatterns(matrix);
    }

    public static void embedTypeInfo(ErrorCorrectionLevel ecLevel, int maskPattern, ByteMatrix matrix)
    {
      BitVector bits = new BitVector();
      MatrixUtil.makeTypeInfoBits(ecLevel, maskPattern, bits);
      for (int index = 0; index < bits.size(); ++index)
      {
        int value_Renamed = bits.at(bits.size() - 1 - index);
        int x1 = MatrixUtil.TYPE_INFO_COORDINATES[index][0];
        int y1 = MatrixUtil.TYPE_INFO_COORDINATES[index][1];
        matrix.set_Renamed(x1, y1, value_Renamed);
        if (index < 8)
        {
          int x2 = matrix.Width - index - 1;
          int y2 = 8;
          matrix.set_Renamed(x2, y2, value_Renamed);
        }
        else
        {
          int x2 = 8;
          int y2 = matrix.Height - 7 + (index - 8);
          matrix.set_Renamed(x2, y2, value_Renamed);
        }
      }
    }

    public static void maybeEmbedVersionInfo(int version, ByteMatrix matrix)
    {
      if (version < 7)
        return;
      BitVector bits = new BitVector();
      MatrixUtil.makeVersionInfoBits(version, bits);
      int index1 = 17;
      for (int index2 = 0; index2 < 6; ++index2)
      {
        for (int index3 = 0; index3 < 3; ++index3)
        {
          int value_Renamed = bits.at(index1);
          --index1;
          matrix.set_Renamed(index2, matrix.Height - 11 + index3, value_Renamed);
          matrix.set_Renamed(matrix.Height - 11 + index3, index2, value_Renamed);
        }
      }
    }

    public static void embedDataBits(BitVector dataBits, int maskPattern, ByteMatrix matrix)
    {
      int index1 = 0;
      int num1 = -1;
      int num2 = matrix.Width - 1;
      int y = matrix.Height - 1;
      while (num2 > 0)
      {
        if (num2 == 6)
          --num2;
        while (y >= 0 && y < matrix.Height)
        {
          for (int index2 = 0; index2 < 2; ++index2)
          {
            int x = num2 - index2;
            if (MatrixUtil.isEmpty((int) matrix.get_Renamed(x, y)))
            {
              int value_Renamed;
              if (index1 < dataBits.size())
              {
                value_Renamed = dataBits.at(index1);
                ++index1;
              }
              else
                value_Renamed = 0;
              if (maskPattern != -1 && MaskUtil.getDataMaskBit(maskPattern, x, y))
                value_Renamed ^= 1;
              matrix.set_Renamed(x, y, value_Renamed);
            }
          }
          y += num1;
        }
        num1 = -num1;
        y += num1;
        num2 -= 2;
      }
      if (index1 != dataBits.size())
        throw new WriterException(string.Concat(new object[4]
        {
          (object) "Not all bits consumed: ",
          (object) index1,
          (object) '/',
          (object) dataBits.size()
        }));
    }

    public static int findMSBSet(int value_Renamed)
    {
      int num = 0;
      while (value_Renamed != 0)
      {
        value_Renamed = SupportClass.URShift(value_Renamed, 1);
        ++num;
      }
      return num;
    }

    public static int calculateBCHCode(int value_Renamed, int poly)
    {
      int msbSet = MatrixUtil.findMSBSet(poly);
      value_Renamed <<= msbSet - 1;
      while (MatrixUtil.findMSBSet(value_Renamed) >= msbSet)
        value_Renamed ^= poly << MatrixUtil.findMSBSet(value_Renamed) - msbSet;
      return value_Renamed;
    }

    public static void makeTypeInfoBits(ErrorCorrectionLevel ecLevel, int maskPattern, BitVector bits)
    {
      if (!QRCode.isValidMaskPattern(maskPattern))
        throw new WriterException("Invalid mask pattern");
      int value_Renamed1 = ecLevel.Bits << 3 | maskPattern;
      bits.appendBits(value_Renamed1, 5);
      int value_Renamed2 = MatrixUtil.calculateBCHCode(value_Renamed1, 1335);
      bits.appendBits(value_Renamed2, 10);
      BitVector other = new BitVector();
      other.appendBits(21522, 15);
      bits.xor(other);
      if (bits.size() != 15)
        throw new WriterException("should not happen but we got: " + (object) bits.size());
    }

    public static void makeVersionInfoBits(int version, BitVector bits)
    {
      bits.appendBits(version, 6);
      int value_Renamed = MatrixUtil.calculateBCHCode(version, 7973);
      bits.appendBits(value_Renamed, 12);
      if (bits.size() != 18)
        throw new WriterException("should not happen but we got: " + (object) bits.size());
    }

    private static bool isEmpty(int value_Renamed)
    {
      return value_Renamed == -1;
    }

    private static bool isValidValue(int value_Renamed)
    {
      if (value_Renamed != -1 && value_Renamed != 0)
        return value_Renamed == 1;
      return true;
    }

    private static void embedTimingPatterns(ByteMatrix matrix)
    {
      for (int index = 8; index < matrix.Width - 8; ++index)
      {
        int value_Renamed = (index + 1) % 2;
        if (!MatrixUtil.isValidValue((int) matrix.get_Renamed(index, 6)))
          throw new WriterException();
        if (MatrixUtil.isEmpty((int) matrix.get_Renamed(index, 6)))
          matrix.set_Renamed(index, 6, value_Renamed);
        if (!MatrixUtil.isValidValue((int) matrix.get_Renamed(6, index)))
          throw new WriterException();
        if (MatrixUtil.isEmpty((int) matrix.get_Renamed(6, index)))
          matrix.set_Renamed(6, index, value_Renamed);
      }
    }

    private static void embedDarkDotAtLeftBottomCorner(ByteMatrix matrix)
    {
      if ((int) matrix.get_Renamed(8, matrix.Height - 8) == 0)
        throw new WriterException();
      matrix.set_Renamed(8, matrix.Height - 8, 1);
    }

    private static void embedHorizontalSeparationPattern(int xStart, int yStart, ByteMatrix matrix)
    {
      if (MatrixUtil.HORIZONTAL_SEPARATION_PATTERN[0].Length != 8 || MatrixUtil.HORIZONTAL_SEPARATION_PATTERN.Length != 1)
        throw new WriterException("Bad horizontal separation pattern");
      for (int index = 0; index < 8; ++index)
      {
        if (!MatrixUtil.isEmpty((int) matrix.get_Renamed(xStart + index, yStart)))
          throw new WriterException();
        matrix.set_Renamed(xStart + index, yStart, MatrixUtil.HORIZONTAL_SEPARATION_PATTERN[0][index]);
      }
    }

    private static void embedVerticalSeparationPattern(int xStart, int yStart, ByteMatrix matrix)
    {
      if (MatrixUtil.VERTICAL_SEPARATION_PATTERN[0].Length != 1 || MatrixUtil.VERTICAL_SEPARATION_PATTERN.Length != 7)
        throw new WriterException("Bad vertical separation pattern");
      for (int index = 0; index < 7; ++index)
      {
        if (!MatrixUtil.isEmpty((int) matrix.get_Renamed(xStart, yStart + index)))
          throw new WriterException();
        matrix.set_Renamed(xStart, yStart + index, MatrixUtil.VERTICAL_SEPARATION_PATTERN[index][0]);
      }
    }

    private static void embedPositionAdjustmentPattern(int xStart, int yStart, ByteMatrix matrix)
    {
      if (MatrixUtil.POSITION_ADJUSTMENT_PATTERN[0].Length != 5 || MatrixUtil.POSITION_ADJUSTMENT_PATTERN.Length != 5)
        throw new WriterException("Bad position adjustment");
      for (int index1 = 0; index1 < 5; ++index1)
      {
        for (int index2 = 0; index2 < 5; ++index2)
        {
          if (!MatrixUtil.isEmpty((int) matrix.get_Renamed(xStart + index2, yStart + index1)))
            throw new WriterException();
          matrix.set_Renamed(xStart + index2, yStart + index1, MatrixUtil.POSITION_ADJUSTMENT_PATTERN[index1][index2]);
        }
      }
    }

    private static void embedPositionDetectionPattern(int xStart, int yStart, ByteMatrix matrix)
    {
      if (MatrixUtil.POSITION_DETECTION_PATTERN[0].Length != 7 || MatrixUtil.POSITION_DETECTION_PATTERN.Length != 7)
        throw new WriterException("Bad position detection pattern");
      for (int index1 = 0; index1 < 7; ++index1)
      {
        for (int index2 = 0; index2 < 7; ++index2)
        {
          if (!MatrixUtil.isEmpty((int) matrix.get_Renamed(xStart + index2, yStart + index1)))
            throw new WriterException();
          matrix.set_Renamed(xStart + index2, yStart + index1, MatrixUtil.POSITION_DETECTION_PATTERN[index1][index2]);
        }
      }
    }

    private static void embedPositionDetectionPatternsAndSeparators(ByteMatrix matrix)
    {
      int length1 = MatrixUtil.POSITION_DETECTION_PATTERN[0].Length;
      MatrixUtil.embedPositionDetectionPattern(0, 0, matrix);
      MatrixUtil.embedPositionDetectionPattern(matrix.Width - length1, 0, matrix);
      MatrixUtil.embedPositionDetectionPattern(0, matrix.Width - length1, matrix);
      int length2 = MatrixUtil.HORIZONTAL_SEPARATION_PATTERN[0].Length;
      MatrixUtil.embedHorizontalSeparationPattern(0, length2 - 1, matrix);
      MatrixUtil.embedHorizontalSeparationPattern(matrix.Width - length2, length2 - 1, matrix);
      MatrixUtil.embedHorizontalSeparationPattern(0, matrix.Width - length2, matrix);
      int length3 = MatrixUtil.VERTICAL_SEPARATION_PATTERN.Length;
      MatrixUtil.embedVerticalSeparationPattern(length3, 0, matrix);
      MatrixUtil.embedVerticalSeparationPattern(matrix.Height - length3 - 1, 0, matrix);
      MatrixUtil.embedVerticalSeparationPattern(length3, matrix.Height - length3, matrix);
    }

    private static void maybeEmbedPositionAdjustmentPatterns(int version, ByteMatrix matrix)
    {
      if (version < 2)
        return;
      int index1 = version - 1;
      int[] numArray = MatrixUtil.POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE[index1];
      int length = MatrixUtil.POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE[index1].Length;
      for (int index2 = 0; index2 < length; ++index2)
      {
        for (int index3 = 0; index3 < length; ++index3)
        {
          int y = numArray[index2];
          int x = numArray[index3];
          if (x != -1 && y != -1 && MatrixUtil.isEmpty((int) matrix.get_Renamed(x, y)))
            MatrixUtil.embedPositionAdjustmentPattern(x - 2, y - 2, matrix);
        }
      }
    }
  }
}
