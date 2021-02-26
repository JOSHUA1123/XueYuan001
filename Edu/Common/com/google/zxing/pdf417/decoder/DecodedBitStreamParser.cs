// Decompiled with JetBrains decompiler
// Type: com.google.zxing.pdf417.decoder.DecodedBitStreamParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using System.Collections;
using System.Text;

namespace com.google.zxing.pdf417.decoder
{
  /// <summary>
  /// <p>This class contains the methods for decoding the PDF417 codewords.</p>
  /// </summary>
  /// <author>SITA Lab (kevin.osullivan@sita.aero)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class DecodedBitStreamParser
  {
    private static readonly char[] PUNCT_CHARS = new char[29]
    {
      ';',
      '<',
      '>',
      '@',
      '[',
      '\\',
      '}',
      '_',
      '`',
      '~',
      '!',
      '\r',
      '\t',
      ',',
      ':',
      '\n',
      '-',
      '.',
      '$',
      '/',
      '"',
      '|',
      '*',
      '(',
      ')',
      '?',
      '{',
      '}',
      '\''
    };
    private static readonly char[] MIXED_CHARS = new char[25]
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
      '&',
      '\r',
      '\t',
      ',',
      ':',
      '#',
      '-',
      '.',
      '$',
      '/',
      '+',
      '%',
      '*',
      '=',
      '^'
    };
    private static readonly string[] EXP900 = new string[16]
    {
      "000000000000000000000000000000000000000000001",
      "000000000000000000000000000000000000000000900",
      "000000000000000000000000000000000000000810000",
      "000000000000000000000000000000000000729000000",
      "000000000000000000000000000000000656100000000",
      "000000000000000000000000000000590490000000000",
      "000000000000000000000000000531441000000000000",
      "000000000000000000000000478296900000000000000",
      "000000000000000000000430467210000000000000000",
      "000000000000000000387420489000000000000000000",
      "000000000000000348678440100000000000000000000",
      "000000000000313810596090000000000000000000000",
      "000000000282429536481000000000000000000000000",
      "000000254186582832900000000000000000000000000",
      "000228767924549610000000000000000000000000000",
      "205891132094649000000000000000000000000000000"
    };
    private const int TEXT_COMPACTION_MODE_LATCH = 900;
    private const int BYTE_COMPACTION_MODE_LATCH = 901;
    private const int NUMERIC_COMPACTION_MODE_LATCH = 902;
    private const int BYTE_COMPACTION_MODE_LATCH_6 = 924;
    private const int BEGIN_MACRO_PDF417_CONTROL_BLOCK = 928;
    private const int BEGIN_MACRO_PDF417_OPTIONAL_FIELD = 923;
    private const int MACRO_PDF417_TERMINATOR = 922;
    private const int MODE_SHIFT_TO_BYTE_COMPACTION_MODE = 913;
    private const int MAX_NUMERIC_CODEWORDS = 15;
    private const int ALPHA = 0;
    private const int LOWER = 1;
    private const int MIXED = 2;
    private const int PUNCT = 3;
    private const int PUNCT_SHIFT = 4;
    private const int PL = 25;
    private const int LL = 27;
    private const int AS = 27;
    private const int ML = 28;
    private const int AL = 28;
    private const int PS = 29;
    private const int PAL = 29;

    private DecodedBitStreamParser()
    {
    }

    internal static DecoderResult decode(int[] codewords)
    {
      StringBuilder result = new StringBuilder(100);
      int num1 = 1;
      int[] numArray1 = codewords;
      int index1 = num1;
      int num2 = 1;
      int codeIndex1 = index1 + num2;
      int mode = numArray1[index1];
      while (codeIndex1 < codewords[0])
      {
        int num3;
        switch (mode)
        {
          case 900:
            num3 = DecodedBitStreamParser.textCompaction(codewords, codeIndex1, result);
            break;
          case 901:
            num3 = DecodedBitStreamParser.byteCompaction(mode, codewords, codeIndex1, result);
            break;
          case 902:
            num3 = DecodedBitStreamParser.numericCompaction(codewords, codeIndex1, result);
            break;
          case 913:
            num3 = DecodedBitStreamParser.byteCompaction(mode, codewords, codeIndex1, result);
            break;
          case 924:
            num3 = DecodedBitStreamParser.byteCompaction(mode, codewords, codeIndex1, result);
            break;
          default:
            int codeIndex2 = codeIndex1 - 1;
            num3 = DecodedBitStreamParser.textCompaction(codewords, codeIndex2, result);
            break;
        }
        if (num3 >= codewords.Length)
          throw ReaderException.Instance;
        int[] numArray2 = codewords;
        int index2 = num3;
        int num4 = 1;
        codeIndex1 = index2 + num4;
        mode = numArray2[index2];
      }
      return new DecoderResult((sbyte[]) null, result.ToString(), (ArrayList) null, (ErrorCorrectionLevel) null);
    }

    /// <summary>
    /// Text Compaction mode (see 5.4.1.5) permits all printable ASCII characters to be
    ///             encoded, i.e. values 32 - 126 inclusive in accordance with ISO/IEC 646 (IRV), as
    ///             well as selected control characters.
    /// 
    /// 
    /// </summary>
    /// <param name="codewords">The array of codewords (data + error)
    ///             </param><param name="codeIndex">The current index into the codeword array.
    ///             </param><param name="result">The decoded data is appended to the result.
    ///             </param>
    /// <returns>
    /// The next index into the codeword array.
    /// 
    /// </returns>
    private static int textCompaction(int[] codewords, int codeIndex, StringBuilder result)
    {
      int[] textCompactionData = new int[codewords[0] << 1];
      int[] byteCompactionData = new int[codewords[0] << 1];
      int length = 0;
      bool flag = false;
      while (codeIndex < codewords[0] && !flag)
      {
        int num = codewords[codeIndex++];
        if (num < 900)
        {
          textCompactionData[length] = num / 30;
          textCompactionData[length + 1] = num % 30;
          length += 2;
        }
        else
        {
          switch (num)
          {
            case 900:
              --codeIndex;
              flag = true;
              continue;
            case 901:
              --codeIndex;
              flag = true;
              continue;
            case 902:
              --codeIndex;
              flag = true;
              continue;
            case 913:
              textCompactionData[length] = 913;
              byteCompactionData[length] = num;
              ++length;
              continue;
            case 924:
              --codeIndex;
              flag = true;
              continue;
            default:
              continue;
          }
        }
      }
      DecodedBitStreamParser.decodeTextCompaction(textCompactionData, byteCompactionData, length, result);
      return codeIndex;
    }

    /// <summary>
    /// The Text Compaction mode includes all the printable ASCII characters
    ///             (i.e. values from 32 to 126) and three ASCII control characters: HT or tab
    ///             (ASCII value 9), LF or line feed (ASCII value 10), and CR or carriage
    ///             return (ASCII value 13). The Text Compaction mode also includes various latch
    ///             and shift characters which are used exclusively within the mode. The Text
    ///             Compaction mode encodes up to 2 characters per codeword. The compaction rules
    ///             for converting data into PDF417 codewords are defined in 5.4.2.2. The sub-mode
    ///             switches are defined in 5.4.2.3.
    /// 
    /// 
    /// </summary>
    /// <param name="textCompactionData">The text compaction data.
    ///             </param><param name="byteCompactionData">The byte compaction data if there
    ///             was a mode shift.
    ///             </param><param name="length">The size of the text compaction and byte compaction data.
    ///             </param><param name="result">The decoded data is appended to the result.
    ///             </param>
    private static void decodeTextCompaction(int[] textCompactionData, int[] byteCompactionData, int length, StringBuilder result)
    {
      int num1 = 0;
      int num2 = 0;
      for (int index1 = 0; index1 < length; ++index1)
      {
        int index2 = textCompactionData[index1];
        char ch = char.MinValue;
        switch (num1)
        {
          case 0:
            if (index2 < 26)
            {
              ch = (char) (65 + index2);
              break;
            }
            if (index2 == 26)
            {
              ch = ' ';
              break;
            }
            if (index2 == 27)
            {
              num1 = 1;
              break;
            }
            if (index2 == 28)
            {
              num1 = 2;
              break;
            }
            if (index2 == 29)
            {
              num2 = num1;
              num1 = 4;
              break;
            }
            if (index2 == 913)
            {
              result.Append((char) byteCompactionData[index1]);
              break;
            }
            break;
          case 1:
            if (index2 < 26)
            {
              ch = (char) (97 + index2);
              break;
            }
            if (index2 == 26)
            {
              ch = ' ';
              break;
            }
            if (index2 == 28)
            {
              num1 = 0;
              break;
            }
            if (index2 == 28)
            {
              num1 = 2;
              break;
            }
            if (index2 == 29)
            {
              num2 = num1;
              num1 = 4;
              break;
            }
            if (index2 == 913)
            {
              result.Append((char) byteCompactionData[index1]);
              break;
            }
            break;
          case 2:
            if (index2 < 25)
            {
              ch = DecodedBitStreamParser.MIXED_CHARS[index2];
              break;
            }
            if (index2 == 25)
            {
              num1 = 3;
              break;
            }
            if (index2 == 26)
            {
              ch = ' ';
              break;
            }
            if (index2 != 27)
            {
              if (index2 == 28)
              {
                num1 = 0;
                break;
              }
              if (index2 == 29)
              {
                num2 = num1;
                num1 = 4;
                break;
              }
              if (index2 == 913)
              {
                result.Append((char) byteCompactionData[index1]);
                break;
              }
              break;
            }
            break;
          case 3:
            if (index2 < 29)
            {
              ch = DecodedBitStreamParser.PUNCT_CHARS[index2];
              break;
            }
            if (index2 == 29)
            {
              num1 = 0;
              break;
            }
            if (index2 == 913)
            {
              result.Append((char) byteCompactionData[index1]);
              break;
            }
            break;
          case 4:
            num1 = num2;
            if (index2 < 29)
            {
              ch = DecodedBitStreamParser.PUNCT_CHARS[index2];
              break;
            }
            if (index2 == 29)
            {
              num1 = 0;
              break;
            }
            break;
        }
        if ((int) ch != 0)
          result.Append(ch);
      }
    }

    /// <summary>
    /// Byte Compaction mode (see 5.4.3) permits all 256 possible 8-bit byte values to be encoded.
    ///             This includes all ASCII characters value 0 to 127 inclusive and provides for international
    ///             character set support.
    /// 
    /// 
    /// </summary>
    /// <param name="mode">The byte compaction mode i.e. 901 or 924
    ///             </param><param name="codewords">The array of codewords (data + error)
    ///             </param><param name="codeIndex">The current index into the codeword array.
    ///             </param><param name="result">The decoded data is appended to the result.
    ///             </param>
    /// <returns>
    /// The next index into the codeword array.
    /// 
    /// </returns>
    private static int byteCompaction(int mode, int[] codewords, int codeIndex, StringBuilder result)
    {
      if (mode == 901)
      {
        int index1 = 0;
        long num1 = 0L;
        char[] chArray = new char[6];
        int[] numArray = new int[6];
        bool flag = false;
        while (codeIndex < codewords[0] && !flag)
        {
          int num2 = codewords[codeIndex++];
          if (num2 < 900)
          {
            numArray[index1] = num2;
            ++index1;
            num1 = num1 * 900L + (long) num2;
          }
          else
          {
            if (num2 != 900 && num2 != 901 && (num2 != 902 && num2 != 924) && (num2 != 928 && num2 != 923))
              ;
            --codeIndex;
            flag = true;
          }
          if (index1 % 5 == 0 && index1 > 0)
          {
            for (int index2 = 0; index2 < 6; ++index2)
            {
              chArray[5 - index2] = (char) ((ulong) num1 % 256UL);
              num1 >>= 8;
            }
            result.Append(chArray);
            index1 = 0;
          }
        }
        for (int index2 = index1 / 5 * 5; index2 < index1; ++index2)
          result.Append((char) numArray[index2]);
      }
      else if (mode == 924)
      {
        int num1 = 0;
        long num2 = 0L;
        bool flag = false;
        while (codeIndex < codewords[0] && !flag)
        {
          int num3 = codewords[codeIndex++];
          if (num3 < 900)
          {
            ++num1;
            num2 = num2 * 900L + (long) num3;
          }
          else
          {
            if (num3 != 900 && num3 != 901 && (num3 != 902 && num3 != 924) && (num3 != 928 && num3 != 923))
              ;
            --codeIndex;
            flag = true;
          }
          if (num1 % 5 == 0 && num1 > 0)
          {
            char[] chArray = new char[6];
            for (int index = 0; index < 6; ++index)
            {
              chArray[5 - index] = (char) ((ulong) num2 % 256UL);
              num2 >>= 8;
            }
            result.Append(chArray);
          }
        }
      }
      return codeIndex;
    }

    /// <summary>
    /// Numeric Compaction mode (see 5.4.4) permits efficient encoding of numeric data strings.
    /// 
    /// 
    /// </summary>
    /// <param name="codewords">The array of codewords (data + error)
    ///             </param><param name="codeIndex">The current index into the codeword array.
    ///             </param><param name="result">The decoded data is appended to the result.
    ///             </param>
    /// <returns>
    /// The next index into the codeword array.
    /// 
    /// </returns>
    private static int numericCompaction(int[] codewords, int codeIndex, StringBuilder result)
    {
      int count = 0;
      bool flag = false;
      int[] codewords1 = new int[15];
      while (codeIndex < codewords.Length && !flag)
      {
        int num = codewords[codeIndex++];
        if (num < 900)
        {
          codewords1[count] = num;
          ++count;
        }
        else
        {
          if (num != 900 && num != 901 && (num != 924 && num != 928) && num != 923)
            ;
          --codeIndex;
          flag = true;
        }
        if (count % 15 == 0 || num == 902)
        {
          string str = DecodedBitStreamParser.decodeBase900toBase10(codewords1, count);
          result.Append(str);
          count = 0;
        }
      }
      return codeIndex;
    }

    /// <summary>
    /// Convert a list of Numeric Compacted codewords from Base 900 to Base 10.
    /// 
    /// 
    /// </summary>
    /// <param name="codewords">The array of codewords
    ///             </param><param name="count">The number of codewords
    ///             </param>
    /// <returns>
    /// The decoded string representing the Numeric data.
    /// 
    /// </returns>
    private static string decodeBase900toBase10(int[] codewords, int count)
    {
      StringBuilder stringBuilder1 = (StringBuilder) null;
      for (int index = 0; index < count; ++index)
      {
        StringBuilder stringBuilder2 = DecodedBitStreamParser.multiply(DecodedBitStreamParser.EXP900[count - index - 1], codewords[index]);
        stringBuilder1 = stringBuilder1 != null ? DecodedBitStreamParser.add(stringBuilder1.ToString(), stringBuilder2.ToString()) : stringBuilder2;
      }
      string str = (string) null;
      for (int index = 0; index < stringBuilder1.Length; ++index)
      {
        if ((int) stringBuilder1[index] == 49)
        {
          str = stringBuilder1.ToString().Substring(index + 1);
          break;
        }
      }
      if (str == null)
        str = stringBuilder1.ToString();
      return str;
    }

    private static StringBuilder multiply(string value1, int value2)
    {
      StringBuilder stringBuilder = new StringBuilder(value1.Length);
      for (int index = 0; index < value1.Length; ++index)
        stringBuilder.Append('0');
      int num1 = value2 / 100;
      int num2 = value2 / 10 % 10;
      int num3 = value2 % 10;
      for (int index = 0; index < num3; ++index)
        stringBuilder = DecodedBitStreamParser.add(stringBuilder.ToString(), value1);
      for (int index = 0; index < num2; ++index)
        stringBuilder = DecodedBitStreamParser.add(stringBuilder.ToString(), (value1 + (object) '0').Substring(1));
      for (int index = 0; index < num1; ++index)
        stringBuilder = DecodedBitStreamParser.add(stringBuilder.ToString(), (value1 + "00").Substring(2));
      return stringBuilder;
    }

    /// <summary>
    /// Add two numbers which are represented as strings.
    /// 
    /// 
    /// </summary>
    /// <param name="value1"/><param name="value2"/>
    /// <returns>
    /// the result of value1 + value2
    /// 
    /// </returns>
    private static StringBuilder add(string value1, string value2)
    {
      StringBuilder stringBuilder1 = new StringBuilder(5);
      StringBuilder stringBuilder2 = new StringBuilder(5);
      StringBuilder stringBuilder3 = new StringBuilder(value1.Length);
      for (int index = 0; index < value1.Length; ++index)
        stringBuilder3.Append('0');
      int num1 = 0;
      int index1 = value1.Length - 3;
      while (index1 > -1)
      {
        stringBuilder1.Length = 0;
        stringBuilder1.Append(value1[index1]);
        stringBuilder1.Append(value1[index1 + 1]);
        stringBuilder1.Append(value1[index1 + 2]);
        stringBuilder2.Length = 0;
        stringBuilder2.Append(value2[index1]);
        stringBuilder2.Append(value2[index1 + 1]);
        stringBuilder2.Append(value2[index1 + 2]);
        int num2 = int.Parse(stringBuilder1.ToString());
        int num3 = int.Parse(stringBuilder2.ToString());
        int num4 = (num2 + num3 + num1) % 1000;
        num1 = (num2 + num3 + num1) / 1000;
        stringBuilder3[index1 + 2] = (char) (num4 % 10 + 48);
        stringBuilder3[index1 + 1] = (char) (num4 / 10 % 10 + 48);
        stringBuilder3[index1] = (char) (num4 / 100 + 48);
        index1 -= 3;
      }
      return stringBuilder3;
    }
  }
}
