// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.decoder.DataBlock
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.qrcode.decoder
{
  /// <summary>
  /// <p>Encapsulates a block of data within a QR Code. QR Codes may split their data into
  ///             multiple blocks, each of which is a unit of data and error-correction codewords. Each
  ///             is represented by an instance of this class.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class DataBlock
  {
    private int numDataCodewords;
    private sbyte[] codewords;

    internal int NumDataCodewords
    {
      get
      {
        return this.numDataCodewords;
      }
    }

    internal sbyte[] Codewords
    {
      get
      {
        return this.codewords;
      }
    }

    private DataBlock(int numDataCodewords, sbyte[] codewords)
    {
      this.numDataCodewords = numDataCodewords;
      this.codewords = codewords;
    }

    /// <summary>
    /// <p>When QR Codes use multiple data blocks, they are actually interleaved.
    ///             That is, the first byte of data block 1 to n is written, then the second bytes, and so on. This
    ///             method will separate the data into original blocks.</p>
    /// </summary>
    /// <param name="rawCodewords">bytes as read directly from the QR Code
    ///             </param><param name="version">version of the QR Code
    ///             </param><param name="ecLevel">error-correction level of the QR Code
    ///             </param>
    /// <returns>
    /// {@link DataBlock}s containing original bytes, "de-interleaved" from representation in the
    ///             QR Code
    /// 
    /// </returns>
    internal static DataBlock[] getDataBlocks(sbyte[] rawCodewords, Version version, ErrorCorrectionLevel ecLevel)
    {
      if (rawCodewords.Length != version.TotalCodewords)
        throw new ArgumentException();
      Version.ECBlocks ecBlocksForLevel = version.getECBlocksForLevel(ecLevel);
      int length1 = 0;
      Version.ECB[] ecBlocks = ecBlocksForLevel.getECBlocks();
      for (int index = 0; index < ecBlocks.Length; ++index)
        length1 += ecBlocks[index].Count;
      DataBlock[] dataBlockArray = new DataBlock[length1];
      int num1 = 0;
      for (int index1 = 0; index1 < ecBlocks.Length; ++index1)
      {
        Version.ECB ecb = ecBlocks[index1];
        for (int index2 = 0; index2 < ecb.Count; ++index2)
        {
          int dataCodewords = ecb.DataCodewords;
          int length2 = ecBlocksForLevel.ECCodewordsPerBlock + dataCodewords;
          dataBlockArray[num1++] = new DataBlock(dataCodewords, new sbyte[length2]);
        }
      }
      int length3 = dataBlockArray[0].codewords.Length;
      int index3 = dataBlockArray.Length - 1;
      while (index3 >= 0 && dataBlockArray[index3].codewords.Length != length3)
        --index3;
      int num2 = index3 + 1;
      int index4 = length3 - ecBlocksForLevel.ECCodewordsPerBlock;
      int num3 = 0;
      for (int index1 = 0; index1 < index4; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          dataBlockArray[index2].codewords[index1] = rawCodewords[num3++];
      }
      for (int index1 = num2; index1 < num1; ++index1)
        dataBlockArray[index1].codewords[index4] = rawCodewords[num3++];
      int length4 = dataBlockArray[0].codewords.Length;
      for (int index1 = index4; index1 < length4; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
        {
          int index5 = index2 < num2 ? index1 : index1 + 1;
          dataBlockArray[index2].codewords[index5] = rawCodewords[num3++];
        }
      }
      return dataBlockArray;
    }
  }
}
