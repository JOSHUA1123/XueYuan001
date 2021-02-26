// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.encoder.MaskUtil
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.common;
using System;

namespace com.google.zxing.qrcode.encoder
{
  /// <author>satorux@google.com (Satoru Takabayashi) - creator
  ///             </author><author>dswitkin@google.com (Daniel Switkin) - ported from C++
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class MaskUtil
  {
    private MaskUtil()
    {
    }

    public static int applyMaskPenaltyRule1(ByteMatrix matrix)
    {
      return MaskUtil.applyMaskPenaltyRule1Internal(matrix, true) + MaskUtil.applyMaskPenaltyRule1Internal(matrix, false);
    }

    public static int applyMaskPenaltyRule2(ByteMatrix matrix)
    {
      int num1 = 0;
      sbyte[][] array = matrix.Array;
      int width = matrix.Width;
      int height = matrix.Height;
      for (int index1 = 0; index1 < height - 1; ++index1)
      {
        for (int index2 = 0; index2 < width - 1; ++index2)
        {
          int num2 = (int) array[index1][index2];
          if (num2 == (int) array[index1][index2 + 1] && num2 == (int) array[index1 + 1][index2] && num2 == (int) array[index1 + 1][index2 + 1])
            num1 += 3;
        }
      }
      return num1;
    }

    public static int applyMaskPenaltyRule3(ByteMatrix matrix)
    {
      int num = 0;
      sbyte[][] array = matrix.Array;
      int width = matrix.Width;
      int height = matrix.Height;
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          if (index2 + 6 < width && (int) array[index1][index2] == 1 && ((int) array[index1][index2 + 1] == 0 && (int) array[index1][index2 + 2] == 1) && ((int) array[index1][index2 + 3] == 1 && (int) array[index1][index2 + 4] == 1 && ((int) array[index1][index2 + 5] == 0 && (int) array[index1][index2 + 6] == 1)) && (index2 + 10 < width && (int) array[index1][index2 + 7] == 0 && ((int) array[index1][index2 + 8] == 0 && (int) array[index1][index2 + 9] == 0) && (int) array[index1][index2 + 10] == 0 || index2 - 4 >= 0 && (int) array[index1][index2 - 1] == 0 && ((int) array[index1][index2 - 2] == 0 && (int) array[index1][index2 - 3] == 0) && (int) array[index1][index2 - 4] == 0))
            num += 40;
          if (index1 + 6 < height && (int) array[index1][index2] == 1 && ((int) array[index1 + 1][index2] == 0 && (int) array[index1 + 2][index2] == 1) && ((int) array[index1 + 3][index2] == 1 && (int) array[index1 + 4][index2] == 1 && ((int) array[index1 + 5][index2] == 0 && (int) array[index1 + 6][index2] == 1)) && (index1 + 10 < height && (int) array[index1 + 7][index2] == 0 && ((int) array[index1 + 8][index2] == 0 && (int) array[index1 + 9][index2] == 0) && (int) array[index1 + 10][index2] == 0 || index1 - 4 >= 0 && (int) array[index1 - 1][index2] == 0 && ((int) array[index1 - 2][index2] == 0 && (int) array[index1 - 3][index2] == 0) && (int) array[index1 - 4][index2] == 0))
            num += 40;
        }
      }
      return num;
    }

    public static int applyMaskPenaltyRule4(ByteMatrix matrix)
    {
      int num1 = 0;
      sbyte[][] array = matrix.Array;
      int width = matrix.Width;
      int height = matrix.Height;
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          if ((int) array[index1][index2] == 1)
            ++num1;
        }
      }
      int num2 = matrix.Height * matrix.Width;
      return Math.Abs((int) ((double) num1 / (double) num2 * 100.0 - 50.0)) / 5 * 10;
    }

    public static bool getDataMaskBit(int maskPattern, int x, int y)
    {
      if (!QRCode.isValidMaskPattern(maskPattern))
        throw new ArgumentException("Invalid mask pattern");
      int num1;
      switch (maskPattern)
      {
        case 0:
          num1 = y + x & 1;
          break;
        case 1:
          num1 = y & 1;
          break;
        case 2:
          num1 = x % 3;
          break;
        case 3:
          num1 = (y + x) % 3;
          break;
        case 4:
          num1 = SupportClass.URShift(y, 1) + x / 3 & 1;
          break;
        case 5:
          int num2 = y * x;
          num1 = (num2 & 1) + num2 % 3;
          break;
        case 6:
          int num3 = y * x;
          num1 = (num3 & 1) + num3 % 3 & 1;
          break;
        case 7:
          num1 = y * x % 3 + (y + x & 1) & 1;
          break;
        default:
          throw new ArgumentException("Invalid mask pattern: " + (object) maskPattern);
      }
      return num1 == 0;
    }

    private static int applyMaskPenaltyRule1Internal(ByteMatrix matrix, bool isHorizontal)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = -1;
      int num4 = isHorizontal ? matrix.Height : matrix.Width;
      int num5 = isHorizontal ? matrix.Width : matrix.Height;
      sbyte[][] array = matrix.Array;
      for (int index1 = 0; index1 < num4; ++index1)
      {
        for (int index2 = 0; index2 < num5; ++index2)
        {
          int num6 = isHorizontal ? (int) array[index1][index2] : (int) array[index2][index1];
          if (num6 == num3)
          {
            ++num2;
            if (num2 == 5)
              num1 += 3;
            else if (num2 > 5)
              ++num1;
          }
          else
          {
            num2 = 1;
            num3 = num6;
          }
        }
        num2 = 0;
      }
      return num1;
    }
  }
}
