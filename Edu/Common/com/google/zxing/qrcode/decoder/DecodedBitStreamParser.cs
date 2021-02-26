// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.decoder.DecodedBitStreamParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace com.google.zxing.qrcode.decoder
{
  /// <summary>
  /// <p>QR Codes can encode text as bits in one of several modes, and can use multiple modes
  ///             in one QR Code. This class decodes the bits back into text.</p><p>See ISO 18004:2006, 6.4.3 - 6.4.7</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class DecodedBitStreamParser
  {
    /// <summary>
    /// See ISO 18004:2006, 6.4.4 Table 5
    /// </summary>
    private static readonly char[] ALPHANUMERIC_CHARS = new char[45]
    {
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
      'Z',
      ' ',
      '$',
      '%',
      '*',
      '+',
      '-',
      '.',
      '/',
      ':'
    };
    private static bool ASSUME_SHIFT_JIS = false;
    private const string SHIFT_JIS = "SJIS";
    private const string EUC_JP = "EUC_JP";
    private const string UTF8 = "UTF-8";
    private const string ISO88591 = "ISO-8859-1";

    private DecodedBitStreamParser()
    {
    }

    internal static DecoderResult decode(sbyte[] bytes, Version version, ErrorCorrectionLevel ecLevel)
    {
      BitSource bits = new BitSource(bytes);
      StringBuilder result = new StringBuilder(50);
      CharacterSetECI currentCharacterSetECI = (CharacterSetECI) null;
      bool fc1InEffect = false;
      ArrayList byteSegments = ArrayList.Synchronized(new ArrayList(1));
      Mode mode;
      do
      {
        if (bits.available() < 4)
        {
          mode = Mode.TERMINATOR;
        }
        else
        {
          try
          {
            mode = Mode.forBits(bits.readBits(4));
          }
          catch (ArgumentException ex)
          {
            throw ReaderException.Instance;
          }
        }
        if (!mode.Equals((object) Mode.TERMINATOR))
        {
          if (mode.Equals((object) Mode.FNC1_FIRST_POSITION) || mode.Equals((object) Mode.FNC1_SECOND_POSITION))
            fc1InEffect = true;
          else if (mode.Equals((object) Mode.STRUCTURED_APPEND))
            bits.readBits(16);
          else if (mode.Equals((object) Mode.ECI))
          {
            currentCharacterSetECI = CharacterSetECI.getCharacterSetECIByValue(DecodedBitStreamParser.parseECIValue(bits));
            if (currentCharacterSetECI == null)
              throw ReaderException.Instance;
          }
          else
          {
            int count = bits.readBits(mode.getCharacterCountBits(version));
            if (mode.Equals((object) Mode.NUMERIC))
              DecodedBitStreamParser.decodeNumericSegment(bits, result, count);
            else if (mode.Equals((object) Mode.ALPHANUMERIC))
              DecodedBitStreamParser.decodeAlphanumericSegment(bits, result, count, fc1InEffect);
            else if (mode.Equals((object) Mode.BYTE))
            {
              DecodedBitStreamParser.decodeByteSegment(bits, result, count, currentCharacterSetECI, byteSegments);
            }
            else
            {
              if (!mode.Equals((object) Mode.KANJI))
                throw ReaderException.Instance;
              DecodedBitStreamParser.decodeKanjiSegment(bits, result, count);
            }
          }
        }
      }
      while (!mode.Equals((object) Mode.TERMINATOR));
      return new DecoderResult(bytes, result.ToString(), byteSegments.Count == 0 ? (ArrayList) null : byteSegments, ecLevel);
    }

    private static void decodeKanjiSegment(BitSource bits, StringBuilder result, int count)
    {
      sbyte[] sbyteArray = new sbyte[2 * count];
      int index = 0;
      for (; count > 0; --count)
      {
        int num1 = bits.readBits(13);
        int num2 = num1 / 192 << 8 | num1 % 192;
        int num3 = num2 >= 7936 ? num2 + 49472 : num2 + 33088;
        sbyteArray[index] = (sbyte) (num3 >> 8);
        sbyteArray[index + 1] = (sbyte) num3;
        index += 2;
      }
      try
      {
        result.Append(Encoding.GetEncoding("SJIS").GetString(SupportClass.ToByteArray(sbyteArray)));
      }
      catch (IOException ex)
      {
        throw ReaderException.Instance;
      }
    }

    private static void decodeByteSegment(BitSource bits, StringBuilder result, int count, CharacterSetECI currentCharacterSetECI, ArrayList byteSegments)
    {
      sbyte[] numArray = new sbyte[count];
      if (count << 3 > bits.available())
        throw ReaderException.Instance;
      for (int index = 0; index < count; ++index)
        numArray[index] = (sbyte) bits.readBits(8);
      string name = currentCharacterSetECI != null ? currentCharacterSetECI.EncodingName : DecodedBitStreamParser.guessEncoding(numArray);
      try
      {
        result.Append(Encoding.GetEncoding(name).GetString(SupportClass.ToByteArray(numArray)));
      }
      catch (IOException ex)
      {
        throw ReaderException.Instance;
      }
      byteSegments.Add((object) SupportClass.ToByteArray(numArray));
    }

    private static void decodeAlphanumericSegment(BitSource bits, StringBuilder result, int count, bool fc1InEffect)
    {
      int length = result.Length;
      while (count > 1)
      {
        int num = bits.readBits(11);
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[num / 45]);
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[num % 45]);
        count -= 2;
      }
      if (count == 1)
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[bits.readBits(6)]);
      if (!fc1InEffect)
        return;
      for (int index = length; index < result.Length; ++index)
      {
        if ((int) result[index] == 37)
        {
          if (index < result.Length - 1 && (int) result[index + 1] == 37)
            result.Remove(index + 1, 1);
          else
            result[index] = '\x001D';
        }
      }
    }

    private static void decodeNumericSegment(BitSource bits, StringBuilder result, int count)
    {
      while (count >= 3)
      {
        int num = bits.readBits(10);
        if (num >= 1000)
          throw ReaderException.Instance;
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[num / 100]);
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[num / 10 % 10]);
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[num % 10]);
        count -= 3;
      }
      if (count == 2)
      {
        int num = bits.readBits(7);
        if (num >= 100)
          throw ReaderException.Instance;
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[num / 10]);
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[num % 10]);
      }
      else
      {
        if (count != 1)
          return;
        int index = bits.readBits(4);
        if (index >= 10)
          throw ReaderException.Instance;
        result.Append(DecodedBitStreamParser.ALPHANUMERIC_CHARS[index]);
      }
    }

    private static string guessEncoding(sbyte[] bytes)
    {
      if (DecodedBitStreamParser.ASSUME_SHIFT_JIS)
        return "SJIS";
      if (bytes.Length > 3 && (int) bytes[0] == (int) (sbyte) SupportClass.Identity(239L) && ((int) bytes[1] == (int) (sbyte) SupportClass.Identity(187L) && (int) bytes[2] == (int) (sbyte) SupportClass.Identity(191L)))
        return "UTF-8";
      int length = bytes.Length;
      bool flag1 = true;
      bool flag2 = true;
      int num1 = 0;
      int num2 = 0;
      bool flag3 = false;
      bool flag4 = false;
      for (int index = 0; index < length && (flag1 || flag2); ++index)
      {
        int num3 = (int) bytes[index] & (int) byte.MaxValue;
        if ((num3 == 194 || num3 == 195) && index < length - 1)
        {
          int num4 = (int) bytes[index + 1] & (int) byte.MaxValue;
          if (num4 <= 191 && (num3 == 194 && num4 >= 160 || num3 == 195 && num4 >= 128))
            flag3 = true;
        }
        if (num3 >= (int) sbyte.MaxValue && num3 <= 159)
          flag1 = false;
        if (num3 >= 161 && num3 <= 223 && !flag4)
          ++num2;
        if (!flag4 && (num3 >= 240 && num3 <= (int) byte.MaxValue || (num3 == 128 || num3 == 160)))
          flag2 = false;
        if (num3 >= 129 && num3 <= 159 || num3 >= 224 && num3 <= 239)
        {
          if (flag4)
          {
            flag4 = false;
          }
          else
          {
            flag4 = true;
            if (index >= bytes.Length - 1)
            {
              flag2 = false;
            }
            else
            {
              int num4 = (int) bytes[index + 1] & (int) byte.MaxValue;
              if (num4 < 64 || num4 > 252)
                flag2 = false;
              else
                ++num1;
            }
          }
        }
        else
          flag4 = false;
      }
      if (flag2 && (num1 >= 3 || 20 * num2 > length))
        return "SJIS";
      return !flag3 && flag1 ? "ISO-8859-1" : "UTF-8";
    }

    private static int parseECIValue(BitSource bits)
    {
      int num1 = bits.readBits(8);
      if ((num1 & 128) == 0)
        return num1 & (int) sbyte.MaxValue;
      if ((num1 & 192) == 128)
      {
        int num2 = bits.readBits(8);
        return (num1 & 63) << 8 | num2;
      }
      if ((num1 & 224) != 192)
        throw new ArgumentException("Bad ECI bits starting with byte " + (object) num1);
      int num3 = bits.readBits(16);
      return (num1 & 31) << 16 | num3;
    }
  }
}
