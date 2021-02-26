// Decompiled with JetBrains decompiler
// Type: com.google.zxing.datamatrix.decoder.DecodedBitStreamParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace com.google.zxing.datamatrix.decoder
{
  /// <summary>
  /// <p>Data Matrix Codes can encode text as bits in one of several modes, and can use multiple modes
  ///             in one Data Matrix Code. This class decodes the bits back into text.</p><p>See ISO 16022:2006, 5.2.1 - 5.2.9.2</p>
  /// </summary>
  /// <author>bbrown@google.com (Brian Brown)
  ///             </author><author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class DecodedBitStreamParser
  {
    /// <summary>
    /// See ISO 16022:2006, Annex C Table C.1
    ///             The C40 Basic Character Set (*'s used for placeholders for the shift values)
    /// 
    /// </summary>
    private static readonly char[] C40_BASIC_SET_CHARS = new char[40]
    {
      '*',
      '*',
      '*',
      ' ',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z'
    };
    private static readonly char[] C40_SHIFT2_SET_CHARS = new char[27]
    {
      '!',
      '"',
      '#',
      '$',
      '%',
      '&',
      '\'',
      '(',
      ')',
      '*',
      '+',
      ',',
      '-',
      '.',
      '/',
      ':',
      ';',
      '<',
      '=',
      '>',
      '?',
      '@',
      '[',
      '\\',
      ']',
      '^',
      '_'
    };
    /// <summary>
    /// See ISO 16022:2006, Annex C Table C.2
    ///             The Text Basic Character Set (*'s used for placeholders for the shift values)
    /// 
    /// </summary>
    private static readonly char[] TEXT_BASIC_SET_CHARS = new char[40]
    {
      '*',
      '*',
      '*',
      ' ',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'g',
      'h',
      'i',
      'j',
      'k',
      'l',
      'm',
      'n',
      'o',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z'
    };
    private static char[] TEXT_SHIFT3_SET_CHARS = new char[32]
    {
      '\'',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z',
      '{',
      '|',
      '}',
      '~',
      '\x007F'
    };
    private const int PAD_ENCODE = 0;
    private const int ASCII_ENCODE = 1;
    private const int C40_ENCODE = 2;
    private const int TEXT_ENCODE = 3;
    private const int ANSIX12_ENCODE = 4;
    private const int EDIFACT_ENCODE = 5;
    private const int BASE256_ENCODE = 6;

    private DecodedBitStreamParser()
    {
    }

    internal static DecoderResult decode(sbyte[] bytes)
    {
      BitSource bits = new BitSource(bytes);
      StringBuilder result = new StringBuilder(100);
      StringBuilder resultTrailer = new StringBuilder(0);
      ArrayList byteSegments = ArrayList.Synchronized(new ArrayList(1));
      int num = 1;
      do
      {
        switch (num)
        {
          case 1:
            num = DecodedBitStreamParser.decodeAsciiSegment(bits, result, resultTrailer);
            goto label_10;
          case 2:
            DecodedBitStreamParser.decodeC40Segment(bits, result);
            break;
          case 3:
            DecodedBitStreamParser.decodeTextSegment(bits, result);
            break;
          case 4:
            DecodedBitStreamParser.decodeAnsiX12Segment(bits, result);
            break;
          case 5:
            DecodedBitStreamParser.decodeEdifactSegment(bits, result);
            break;
          case 6:
            DecodedBitStreamParser.decodeBase256Segment(bits, result, byteSegments);
            break;
          default:
            throw ReaderException.Instance;
        }
        num = 1;
label_10:;
      }
      while (num != 0 && bits.available() > 0);
      if (resultTrailer.Length > 0)
        result.Append(resultTrailer.ToString());
      return new DecoderResult(bytes, result.ToString(), byteSegments.Count == 0 ? (ArrayList) null : byteSegments, (ErrorCorrectionLevel) null);
    }

    /// <summary>
    /// See ISO 16022:2006, 5.2.3 and Annex C, Table C.2
    /// </summary>
    private static int decodeAsciiSegment(BitSource bits, StringBuilder result, StringBuilder resultTrailer)
    {
      bool flag = false;
      do
      {
        int num1 = bits.readBits(8);
        if (num1 == 0)
          throw ReaderException.Instance;
        if (num1 <= 128)
        {
          int num2 = flag ? num1 + 128 : num1;
          result.Append((char) (num2 - 1));
          return 1;
        }
        if (num1 == 129)
          return 0;
        if (num1 <= 229)
        {
          int num2 = num1 - 130;
          if (num2 < 10)
            result.Append('0');
          result.Append(num2);
        }
        else
        {
          if (num1 == 230)
            return 2;
          if (num1 == 231)
            return 6;
          if (num1 != 232 && num1 != 233 && num1 != 234)
          {
            if (num1 == 235)
              flag = true;
            else if (num1 == 236)
            {
              result.Append("[)>\x001E05\x001D");
              resultTrailer.Insert(0, "\x001E\x0004");
            }
            else if (num1 == 237)
            {
              result.Append("[)>\x001E06\x001D");
              resultTrailer.Insert(0, "\x001E\x0004");
            }
            else
            {
              if (num1 == 238)
                return 4;
              if (num1 == 239)
                return 3;
              if (num1 == 240)
                return 5;
              if (num1 != 241 && num1 >= 242)
                throw ReaderException.Instance;
            }
          }
        }
      }
      while (bits.available() > 0);
      return 1;
    }

    /// <summary>
    /// See ISO 16022:2006, 5.2.5 and Annex C, Table C.1
    /// </summary>
    private static void decodeC40Segment(BitSource bits, StringBuilder result)
    {
      bool flag = false;
      int[] result1 = new int[3];
      while (bits.available() != 8)
      {
        int firstByte = bits.readBits(8);
        if (firstByte == 254)
          break;
        DecodedBitStreamParser.parseTwoBytes(firstByte, bits.readBits(8), result1);
        int num = 0;
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = result1[index1];
          switch (num)
          {
            case 0:
              if (index2 < 3)
              {
                num = index2 + 1;
                break;
              }
              if (flag)
              {
                result.Append((char) ((uint) DecodedBitStreamParser.C40_BASIC_SET_CHARS[index2] + 128U));
                flag = false;
                break;
              }
              result.Append(DecodedBitStreamParser.C40_BASIC_SET_CHARS[index2]);
              break;
            case 1:
              if (flag)
              {
                result.Append((char) (index2 + 128));
                flag = false;
              }
              else
                result.Append(index2);
              num = 0;
              break;
            case 2:
              if (index2 < 27)
              {
                if (flag)
                {
                  result.Append((char) ((uint) DecodedBitStreamParser.C40_SHIFT2_SET_CHARS[index2] + 128U));
                  flag = false;
                }
                else
                  result.Append(DecodedBitStreamParser.C40_SHIFT2_SET_CHARS[index2]);
              }
              else
              {
                if (index2 == 27)
                  throw ReaderException.Instance;
                if (index2 != 30)
                  throw ReaderException.Instance;
                flag = true;
              }
              num = 0;
              break;
            case 3:
              if (flag)
              {
                result.Append((char) (index2 + 224));
                flag = false;
              }
              else
                result.Append((char) (index2 + 96));
              num = 0;
              break;
            default:
              throw ReaderException.Instance;
          }
        }
        if (bits.available() <= 0)
          break;
      }
    }

    /// <summary>
    /// See ISO 16022:2006, 5.2.6 and Annex C, Table C.2
    /// </summary>
    private static void decodeTextSegment(BitSource bits, StringBuilder result)
    {
      bool flag = false;
      int[] result1 = new int[3];
      while (bits.available() != 8)
      {
        int firstByte = bits.readBits(8);
        if (firstByte == 254)
          break;
        DecodedBitStreamParser.parseTwoBytes(firstByte, bits.readBits(8), result1);
        int num = 0;
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = result1[index1];
          switch (num)
          {
            case 0:
              if (index2 < 3)
              {
                num = index2 + 1;
                break;
              }
              if (flag)
              {
                result.Append((char) ((uint) DecodedBitStreamParser.TEXT_BASIC_SET_CHARS[index2] + 128U));
                flag = false;
                break;
              }
              result.Append(DecodedBitStreamParser.TEXT_BASIC_SET_CHARS[index2]);
              break;
            case 1:
              if (flag)
              {
                result.Append((char) (index2 + 128));
                flag = false;
              }
              else
                result.Append(index2);
              num = 0;
              break;
            case 2:
              if (index2 < 27)
              {
                if (flag)
                {
                  result.Append((char) ((uint) DecodedBitStreamParser.C40_SHIFT2_SET_CHARS[index2] + 128U));
                  flag = false;
                }
                else
                  result.Append(DecodedBitStreamParser.C40_SHIFT2_SET_CHARS[index2]);
              }
              else
              {
                if (index2 == 27)
                  throw ReaderException.Instance;
                if (index2 != 30)
                  throw ReaderException.Instance;
                flag = true;
              }
              num = 0;
              break;
            case 3:
              if (flag)
              {
                result.Append((char) ((uint) DecodedBitStreamParser.TEXT_SHIFT3_SET_CHARS[index2] + 128U));
                flag = false;
              }
              else
                result.Append(DecodedBitStreamParser.TEXT_SHIFT3_SET_CHARS[index2]);
              num = 0;
              break;
            default:
              throw ReaderException.Instance;
          }
        }
        if (bits.available() <= 0)
          break;
      }
    }

    /// <summary>
    /// See ISO 16022:2006, 5.2.7
    /// </summary>
    private static void decodeAnsiX12Segment(BitSource bits, StringBuilder result)
    {
      int[] result1 = new int[3];
      while (bits.available() != 8)
      {
        int firstByte = bits.readBits(8);
        if (firstByte == 254)
          break;
        DecodedBitStreamParser.parseTwoBytes(firstByte, bits.readBits(8), result1);
        for (int index = 0; index < 3; ++index)
        {
          int num = result1[index];
          switch (num)
          {
            case 0:
              result.Append('\r');
              break;
            case 1:
              result.Append('*');
              break;
            case 2:
              result.Append('>');
              break;
            case 3:
              result.Append(' ');
              break;
            default:
              if (num < 14)
              {
                result.Append((char) (num + 44));
                break;
              }
              if (num >= 40)
                throw ReaderException.Instance;
              result.Append((char) (num + 51));
              break;
          }
        }
        if (bits.available() <= 0)
          break;
      }
    }

    private static void parseTwoBytes(int firstByte, int secondByte, int[] result)
    {
      int num1 = (firstByte << 8) + secondByte - 1;
      int num2 = num1 / 1600;
      result[0] = num2;
      int num3 = num1 - num2 * 1600;
      int num4 = num3 / 40;
      result[1] = num4;
      result[2] = num3 - num4 * 40;
    }

    /// <summary>
    /// See ISO 16022:2006, 5.2.8 and Annex C Table C.3
    /// </summary>
    private static void decodeEdifactSegment(BitSource bits, StringBuilder result)
    {
      bool flag = false;
      while (bits.available() > 16)
      {
        for (int index = 0; index < 4; ++index)
        {
          int num = bits.readBits(6);
          if (num == 11111)
            flag = true;
          if (!flag)
          {
            if ((num & 32) == 0)
              num |= 64;
            result.Append(num);
          }
        }
        if (flag || bits.available() <= 0)
          break;
      }
    }

    /// <summary>
    /// See ISO 16022:2006, 5.2.9 and Annex B, B.2
    /// </summary>
    private static void decodeBase256Segment(BitSource bits, StringBuilder result, ArrayList byteSegments)
    {
      int num = bits.readBits(8);
      int length = num != 0 ? (num >= 250 ? 250 * (num - 249) + bits.readBits(8) : num) : bits.available() / 8;
      sbyte[] sbyteArray = new sbyte[length];
      for (int base256CodewordPosition = 0; base256CodewordPosition < length; ++base256CodewordPosition)
        sbyteArray[base256CodewordPosition] = DecodedBitStreamParser.unrandomize255State(bits.readBits(8), base256CodewordPosition);
      byteSegments.Add((object) SupportClass.ToByteArray(sbyteArray));
      try
      {
        result.Append(Encoding.GetEncoding("ISO8859_1").GetString(SupportClass.ToByteArray(sbyteArray)));
      }
      catch (IOException ex)
      {
        throw new SystemException("Platform does not support required encoding: " + (object) ex);
      }
    }

    /// <summary>
    /// See ISO 16022:2006, Annex B, B.2
    /// </summary>
    private static sbyte unrandomize255State(int randomizedBase256Codeword, int base256CodewordPosition)
    {
      int num1 = 149 * base256CodewordPosition % (int) byte.MaxValue + 1;
      int num2 = randomizedBase256Codeword - num1;
      return num2 >= 0 ? (sbyte) num2 : (sbyte) (num2 + 256);
    }
  }
}
