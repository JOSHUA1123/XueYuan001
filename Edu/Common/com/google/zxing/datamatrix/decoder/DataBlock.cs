// Decompiled with JetBrains decompiler
// Type: com.google.zxing.datamatrix.decoder.DataBlock
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.datamatrix.decoder
{
  /// <summary>
  /// <p>Encapsulates a block of data within a Data Matrix Code. Data Matrix Codes may split their data into
  ///             multiple blocks, each of which is a unit of data and error-correction codewords. Each
  ///             is represented by an instance of this class.</p>
  /// </summary>
  /// <author>bbrown@google.com (Brian Brown)
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
    /// <p>When Data Matrix Codes use multiple data blocks, they actually interleave the bytes of each of them.
    ///             That is, the first byte of data block 1 to n is written, then the second bytes, and so on. This
    ///             method will separate the data into original blocks.</p>
    /// </summary>
    /// <param name="rawCodewords">bytes as read directly from the Data Matrix Code
    ///             </param><param name="version">version of the Data Matrix Code
    ///             </param>
    /// <returns>
    /// {@link DataBlock}s containing original bytes, "de-interleaved" from representation in the
    ///             Data Matrix Code
    /// 
    /// </returns>
    internal static DataBlock[] getDataBlocks(sbyte[] rawCodewords, Version version)
    {
      Version.ECBlocks ecBlocks1 = version.getECBlocks();
      int length1 = 0;
      Version.ECB[] ecBlocks2 = ecBlocks1.getECBlocks();
      for (int index = 0; index < ecBlocks2.Length; ++index)
        length1 += ecBlocks2[index].Count;
      DataBlock[] dataBlockArray = new DataBlock[length1];
      int num1 = 0;
      for (int index1 = 0; index1 < ecBlocks2.Length; ++index1)
      {
        Version.ECB ecb = ecBlocks2[index1];
        for (int index2 = 0; index2 < ecb.Count; ++index2)
        {
          int dataCodewords = ecb.DataCodewords;
          int length2 = ecBlocks1.ECCodewords + dataCodewords;
          dataBlockArray[num1++] = new DataBlock(dataCodewords, new sbyte[length2]);
        }
      }
      int num2 = dataBlockArray[0].codewords.Length - ecBlocks1.ECCodewords;
      int num3 = num2 - 1;
      int num4 = 0;
      for (int index1 = 0; index1 < num3; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          dataBlockArray[index2].codewords[index1] = rawCodewords[num4++];
      }
      bool flag = version.VersionNumber == 24;
      int num5 = flag ? 8 : num1;
      for (int index = 0; index < num5; ++index)
        dataBlockArray[index].codewords[num2 - 1] = rawCodewords[num4++];
      int length3 = dataBlockArray[0].codewords.Length;
      for (int index1 = num2; index1 < length3; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
        {
          int index3 = !flag || index2 <= 7 ? index1 : index1 - 1;
          dataBlockArray[index2].codewords[index3] = rawCodewords[num4++];
        }
      }
      if (num4 != rawCodewords.Length)
        throw new ArgumentException();
      return dataBlockArray;
    }
  }
}
