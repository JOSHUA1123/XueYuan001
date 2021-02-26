// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.encoder.Encoder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.common.reedsolomon;
using com.google.zxing.qrcode.decoder;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace com.google.zxing.qrcode.encoder
{
  /// <author>satorux@google.com (Satoru Takabayashi) - creator
  ///             </author><author>dswitkin@google.com (Daniel Switkin) - ported from C++
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Encoder
  {
    private static readonly int[] ALPHANUMERIC_TABLE = new int[96]
    {
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      36,
      -1,
      -1,
      -1,
      37,
      38,
      -1,
      -1,
      -1,
      -1,
      39,
      40,
      -1,
      41,
      42,
      43,
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      7,
      8,
      9,
      44,
      -1,
      -1,
      -1,
      -1,
      -1,
      -1,
      10,
      11,
      12,
      13,
      14,
      15,
      16,
      17,
      18,
      19,
      20,
      21,
      22,
      23,
      24,
      25,
      26,
      27,
      28,
      29,
      30,
      31,
      32,
      33,
      34,
      35,
      -1,
      -1,
      -1,
      -1,
      -1
    };
    internal const string DEFAULT_BYTE_MODE_ENCODING = "UTF-8";

    private Encoder()
    {
    }

    private static int calculateMaskPenalty(ByteMatrix matrix)
    {
      return 0 + MaskUtil.applyMaskPenaltyRule1(matrix) + MaskUtil.applyMaskPenaltyRule2(matrix) + MaskUtil.applyMaskPenaltyRule3(matrix) + MaskUtil.applyMaskPenaltyRule4(matrix);
    }

    /// <summary>
    /// Encode "bytes" with the error correction level "ecLevel". The encoding mode will be chosen
    ///             internally by chooseMode(). On success, store the result in "qrCode".
    /// 
    ///             We recommend you to use QRCode.EC_LEVEL_L (the lowest level) for
    ///             "getECLevel" since our primary use is to show QR code on desktop screens. We don't need very
    ///             strong error correction for this purpose.
    /// 
    ///             Note that there is no way to encode bytes in MODE_KANJI. We might want to add EncodeWithMode()
    ///             with which clients can specify the encoding mode. For now, we don't need the functionality.
    /// 
    /// </summary>
    public static void encode(string content, ErrorCorrectionLevel ecLevel, QRCode qrCode)
    {
      Encoder.encode(content, ecLevel, (Hashtable) null, qrCode);
    }

    public static void encode(string content, ErrorCorrectionLevel ecLevel, Hashtable hints, QRCode qrCode)
    {
      string str = (hints == null ? (string) null : (string) hints[(object) EncodeHintType.CHARACTER_SET]) ?? "UTF-8";
      Mode mode = Encoder.chooseMode(content, str);
      BitVector bits1 = new BitVector();
      Encoder.appendBytes(content, mode, bits1, str);
      Encoder.initQRCode(bits1.sizeInBytes(), ecLevel, mode, qrCode);
      BitVector bits2 = new BitVector();
      if (mode == Mode.BYTE && !"UTF-8".Equals(str))
      {
        CharacterSetECI characterSetEciByName = CharacterSetECI.getCharacterSetECIByName(str);
        if (characterSetEciByName != null)
          Encoder.appendECI(characterSetEciByName, bits2);
      }
      Encoder.appendModeInfo(mode, bits2);
      Encoder.appendLengthInfo(mode.Equals((object) Mode.BYTE) ? bits1.sizeInBytes() : content.Length, qrCode.Version, mode, bits2);
      bits2.appendBitVector(bits1);
      Encoder.terminateBits(qrCode.NumDataBytes, bits2);
      BitVector bitVector = new BitVector();
      Encoder.interleaveWithECBytes(bits2, qrCode.NumTotalBytes, qrCode.NumDataBytes, qrCode.NumRSBlocks, bitVector);
      ByteMatrix matrix = new ByteMatrix(qrCode.MatrixWidth, qrCode.MatrixWidth);
      qrCode.MaskPattern = Encoder.chooseMaskPattern(bitVector, qrCode.ECLevel, qrCode.Version, matrix);
      MatrixUtil.buildMatrix(bitVector, qrCode.ECLevel, qrCode.Version, qrCode.MaskPattern, matrix);
      qrCode.Matrix = matrix;
      if (!qrCode.Valid)
        throw new WriterException("Invalid QR code: " + qrCode.ToString());
    }

    /// <returns>
    /// the code point of the table used in alphanumeric mode or
    ///             -1 if there is no corresponding code in the table.
    /// 
    /// </returns>
    internal static int getAlphanumericCode(int code)
    {
      if (code < Encoder.ALPHANUMERIC_TABLE.Length)
        return Encoder.ALPHANUMERIC_TABLE[code];
      return -1;
    }

    public static Mode chooseMode(string content)
    {
      return Encoder.chooseMode(content, (string) null);
    }

    /// <summary>
    /// Choose the best mode by examining the content. Note that 'encoding' is used as a hint;
    ///             if it is Shift_JIS, and the input is only double-byte Kanji, then we return {@link Mode#KANJI}.
    /// 
    /// </summary>
    public static Mode chooseMode(string content, string encoding)
    {
      if ("Shift_JIS".Equals(encoding))
      {
        if (!Encoder.isOnlyDoubleByteKanji(content))
          return Mode.BYTE;
        return Mode.KANJI;
      }
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < content.Length; ++index)
      {
        char ch = content[index];
        if ((int) ch >= 48 && (int) ch <= 57)
        {
          flag1 = true;
        }
        else
        {
          if (Encoder.getAlphanumericCode((int) ch) == -1)
            return Mode.BYTE;
          flag2 = true;
        }
      }
      if (flag2)
        return Mode.ALPHANUMERIC;
      if (flag1)
        return Mode.NUMERIC;
      return Mode.BYTE;
    }

    private static bool isOnlyDoubleByteKanji(string content)
    {
      sbyte[] numArray;
      try
      {
        numArray = SupportClass.ToSByteArray(Encoding.GetEncoding("Shift_JIS").GetBytes(content));
      }
      catch (IOException ex)
      {
        return false;
      }
      int length = numArray.Length;
      if (length % 2 != 0)
        return false;
      int index = 0;
      while (index < length)
      {
        int num = (int) numArray[index] & (int) byte.MaxValue;
        if ((num < 129 || num > 159) && (num < 224 || num > 235))
          return false;
        index += 2;
      }
      return true;
    }

    private static int chooseMaskPattern(BitVector bits, ErrorCorrectionLevel ecLevel, int version, ByteMatrix matrix)
    {
      int num1 = int.MaxValue;
      int num2 = -1;
      for (int maskPattern = 0; maskPattern < 8; ++maskPattern)
      {
        MatrixUtil.buildMatrix(bits, ecLevel, version, maskPattern, matrix);
        int num3 = Encoder.calculateMaskPenalty(matrix);
        if (num3 < num1)
        {
          num1 = num3;
          num2 = maskPattern;
        }
      }
      return num2;
    }

    /// <summary>
    /// Initialize "qrCode" according to "numInputBytes", "ecLevel", and "mode". On success,
    ///             modify "qrCode".
    /// 
    /// </summary>
    private static void initQRCode(int numInputBytes, ErrorCorrectionLevel ecLevel, Mode mode, QRCode qrCode)
    {
      qrCode.ECLevel = ecLevel;
      qrCode.Mode = mode;
      for (int versionNumber = 1; versionNumber <= 40; ++versionNumber)
      {
        com.google.zxing.qrcode.decoder.Version versionForNumber = com.google.zxing.qrcode.decoder.Version.getVersionForNumber(versionNumber);
        int totalCodewords = versionForNumber.TotalCodewords;
        com.google.zxing.qrcode.decoder.Version.ECBlocks ecBlocksForLevel = versionForNumber.getECBlocksForLevel(ecLevel);
        int totalEcCodewords = ecBlocksForLevel.TotalECCodewords;
        int numBlocks = ecBlocksForLevel.NumBlocks;
        int num = totalCodewords - totalEcCodewords;
        if (num >= numInputBytes + 3)
        {
          qrCode.Version = versionNumber;
          qrCode.NumTotalBytes = totalCodewords;
          qrCode.NumDataBytes = num;
          qrCode.NumRSBlocks = numBlocks;
          qrCode.NumECBytes = totalEcCodewords;
          qrCode.MatrixWidth = versionForNumber.DimensionForVersion;
          return;
        }
      }
      throw new WriterException("Cannot find proper rs block info (input data too big?)");
    }

    /// <summary>
    /// Terminate bits as described in 8.4.8 and 8.4.9 of JISX0510:2004 (p.24).
    /// </summary>
    internal static void terminateBits(int numDataBytes, BitVector bits)
    {
      int num1 = numDataBytes << 3;
      if (bits.size() > num1)
        throw new WriterException(string.Concat(new object[4]
        {
          (object) "data bits cannot fit in the QR Code",
          (object) bits.size(),
          (object) " > ",
          (object) num1
        }));
      for (int index = 0; index < 4 && bits.size() < num1; ++index)
        bits.appendBit(0);
      int num2 = bits.size() % 8;
      if (num2 > 0)
      {
        int num3 = 8 - num2;
        for (int index = 0; index < num3; ++index)
          bits.appendBit(0);
      }
      if (bits.size() % 8 != 0)
        throw new WriterException("Number of bits is not a multiple of 8");
      int num4 = numDataBytes - bits.sizeInBytes();
      for (int index = 0; index < num4; ++index)
      {
        if (index % 2 == 0)
          bits.appendBits(236, 8);
        else
          bits.appendBits(17, 8);
      }
      if (bits.size() != num1)
        throw new WriterException("Bits size does not equal capacity");
    }

    /// <summary>
    /// Get number of data bytes and number of error correction bytes for block id "blockID". Store
    ///             the result in "numDataBytesInBlock", and "numECBytesInBlock". See table 12 in 8.5.1 of
    ///             JISX0510:2004 (p.30)
    /// 
    /// </summary>
    internal static void getNumDataBytesAndNumECBytesForBlockID(int numTotalBytes, int numDataBytes, int numRSBlocks, int blockID, int[] numDataBytesInBlock, int[] numECBytesInBlock)
    {
      if (blockID >= numRSBlocks)
        throw new WriterException("Block ID too large");
      int num1 = numTotalBytes % numRSBlocks;
      int num2 = numRSBlocks - num1;
      int num3 = numTotalBytes / numRSBlocks;
      int num4 = num3 + 1;
      int num5 = numDataBytes / numRSBlocks;
      int num6 = num5 + 1;
      int num7 = num3 - num5;
      int num8 = num4 - num6;
      if (num7 != num8)
        throw new WriterException("EC bytes mismatch");
      if (numRSBlocks != num2 + num1)
        throw new WriterException("RS blocks mismatch");
      if (numTotalBytes != (num5 + num7) * num2 + (num6 + num8) * num1)
        throw new WriterException("Total bytes mismatch");
      if (blockID < num2)
      {
        numDataBytesInBlock[0] = num5;
        numECBytesInBlock[0] = num7;
      }
      else
      {
        numDataBytesInBlock[0] = num6;
        numECBytesInBlock[0] = num8;
      }
    }

    /// <summary>
    /// Interleave "bits" with corresponding error correction bytes. On success, store the result in
    ///             "result". The interleave rule is complicated. See 8.6 of JISX0510:2004 (p.37) for details.
    /// 
    /// </summary>
    internal static void interleaveWithECBytes(BitVector bits, int numTotalBytes, int numDataBytes, int numRSBlocks, BitVector result)
    {
      if (bits.sizeInBytes() != numDataBytes)
        throw new WriterException("Number of bits and data bytes does not match");
      int offset = 0;
      int val1_1 = 0;
      int val1_2 = 0;
      ArrayList arrayList = ArrayList.Synchronized(new ArrayList(numRSBlocks));
      for (int blockID = 0; blockID < numRSBlocks; ++blockID)
      {
        int[] numDataBytesInBlock = new int[1];
        int[] numECBytesInBlock = new int[1];
        Encoder.getNumDataBytesAndNumECBytesForBlockID(numTotalBytes, numDataBytes, numRSBlocks, blockID, numDataBytesInBlock, numECBytesInBlock);
        ByteArray byteArray = new ByteArray();
        byteArray.set_Renamed(bits.Array, offset, numDataBytesInBlock[0]);
        ByteArray errorCorrection = Encoder.generateECBytes(byteArray, numECBytesInBlock[0]);
        arrayList.Add((object) new BlockPair(byteArray, errorCorrection));
        val1_1 = Math.Max(val1_1, byteArray.size());
        val1_2 = Math.Max(val1_2, errorCorrection.size());
        offset += numDataBytesInBlock[0];
      }
      if (numDataBytes != offset)
        throw new WriterException("Data bytes does not match offset");
      for (int index1 = 0; index1 < val1_1; ++index1)
      {
        for (int index2 = 0; index2 < arrayList.Count; ++index2)
        {
          ByteArray dataBytes = ((BlockPair) arrayList[index2]).DataBytes;
          if (index1 < dataBytes.size())
            result.appendBits(dataBytes.at(index1), 8);
        }
      }
      for (int index1 = 0; index1 < val1_2; ++index1)
      {
        for (int index2 = 0; index2 < arrayList.Count; ++index2)
        {
          ByteArray errorCorrectionBytes = ((BlockPair) arrayList[index2]).ErrorCorrectionBytes;
          if (index1 < errorCorrectionBytes.size())
            result.appendBits(errorCorrectionBytes.at(index1), 8);
        }
      }
      if (numTotalBytes != result.sizeInBytes())
        throw new WriterException("Interleaving error: " + (object) numTotalBytes + " and " + (string) (object) result.sizeInBytes() + " differ.");
    }

    internal static ByteArray generateECBytes(ByteArray dataBytes, int numEcBytesInBlock)
    {
      int num = dataBytes.size();
      int[] toEncode = new int[num + numEcBytesInBlock];
      for (int index = 0; index < num; ++index)
        toEncode[index] = dataBytes.at(index);
      new ReedSolomonEncoder(GF256.QR_CODE_FIELD).encode(toEncode, numEcBytesInBlock);
      ByteArray byteArray = new ByteArray(numEcBytesInBlock);
      for (int index = 0; index < numEcBytesInBlock; ++index)
        byteArray.set_Renamed(index, toEncode[num + index]);
      return byteArray;
    }

    /// <summary>
    /// Append mode info. On success, store the result in "bits".
    /// </summary>
    internal static void appendModeInfo(Mode mode, BitVector bits)
    {
      bits.appendBits(mode.Bits, 4);
    }

    /// <summary>
    /// Append length info. On success, store the result in "bits".
    /// </summary>
    internal static void appendLengthInfo(int numLetters, int version, Mode mode, BitVector bits)
    {
      int characterCountBits = mode.getCharacterCountBits(com.google.zxing.qrcode.decoder.Version.getVersionForNumber(version));
      if (numLetters > (1 << characterCountBits) - 1)
        throw new WriterException((string) (object) numLetters + (object) "is bigger than" + (string) (object) ((1 << characterCountBits) - 1));
      bits.appendBits(numLetters, characterCountBits);
    }

    /// <summary>
    /// Append "bytes" in "mode" mode (encoding) into "bits". On success, store the result in "bits".
    /// </summary>
    internal static void appendBytes(string content, Mode mode, BitVector bits, string encoding)
    {
      if (mode.Equals((object) Mode.NUMERIC))
        Encoder.appendNumericBytes(content, bits);
      else if (mode.Equals((object) Mode.ALPHANUMERIC))
        Encoder.appendAlphanumericBytes(content, bits);
      else if (mode.Equals((object) Mode.BYTE))
      {
        Encoder.append8BitBytes(content, bits, encoding);
      }
      else
      {
        if (!mode.Equals((object) Mode.KANJI))
          throw new WriterException("Invalid mode: " + (object) mode);
        Encoder.appendKanjiBytes(content, bits);
      }
    }

    internal static void appendNumericBytes(string content, BitVector bits)
    {
      int length = content.Length;
      int index = 0;
      while (index < length)
      {
        int value_Renamed = (int) content[index] - 48;
        if (index + 2 < length)
        {
          int num1 = (int) content[index + 1] - 48;
          int num2 = (int) content[index + 2] - 48;
          bits.appendBits(value_Renamed * 100 + num1 * 10 + num2, 10);
          index += 3;
        }
        else if (index + 1 < length)
        {
          int num = (int) content[index + 1] - 48;
          bits.appendBits(value_Renamed * 10 + num, 7);
          index += 2;
        }
        else
        {
          bits.appendBits(value_Renamed, 4);
          ++index;
        }
      }
    }

    internal static void appendAlphanumericBytes(string content, BitVector bits)
    {
      int length = content.Length;
      int index = 0;
      while (index < length)
      {
        int alphanumericCode1 = Encoder.getAlphanumericCode((int) content[index]);
        if (alphanumericCode1 == -1)
          throw new WriterException();
        if (index + 1 < length)
        {
          int alphanumericCode2 = Encoder.getAlphanumericCode((int) content[index + 1]);
          if (alphanumericCode2 == -1)
            throw new WriterException();
          bits.appendBits(alphanumericCode1 * 45 + alphanumericCode2, 11);
          index += 2;
        }
        else
        {
          bits.appendBits(alphanumericCode1, 6);
          ++index;
        }
      }
    }

    internal static void append8BitBytes(string content, BitVector bits, string encoding)
    {
      sbyte[] numArray;
      try
      {
        numArray = SupportClass.ToSByteArray(Encoding.GetEncoding(encoding).GetBytes(content));
      }
      catch (IOException ex)
      {
        throw new WriterException(ex.ToString());
      }
      for (int index = 0; index < numArray.Length; ++index)
        bits.appendBits((int) numArray[index], 8);
    }

    internal static void appendKanjiBytes(string content, BitVector bits)
    {
      sbyte[] numArray;
      try
      {
        numArray = SupportClass.ToSByteArray(Encoding.GetEncoding("Shift_JIS").GetBytes(content));
      }
      catch (IOException ex)
      {
        throw new WriterException(ex.ToString());
      }
      int length = numArray.Length;
      int index = 0;
      while (index < length)
      {
        int num1 = ((int) numArray[index] & (int) byte.MaxValue) << 8 | (int) numArray[index + 1] & (int) byte.MaxValue;
        int num2 = -1;
        if (num1 >= 33088 && num1 <= 40956)
          num2 = num1 - 33088;
        else if (num1 >= 57408 && num1 <= 60351)
          num2 = num1 - 49472;
        if (num2 == -1)
          throw new WriterException("Invalid byte sequence");
        int value_Renamed = (num2 >> 8) * 192 + (num2 & (int) byte.MaxValue);
        bits.appendBits(value_Renamed, 13);
        index += 2;
      }
    }

    private static void appendECI(CharacterSetECI eci, BitVector bits)
    {
      bits.appendBits(Mode.ECI.Bits, 4);
      bits.appendBits(eci.Value, 8);
    }
  }
}
