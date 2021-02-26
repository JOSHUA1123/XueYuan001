// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.decoder.Decoder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.common.reedsolomon;

namespace com.google.zxing.qrcode.decoder
{
  /// <summary>
  /// <p>The main class which implements QR Code decoding -- as opposed to locating and extracting
  ///             the QR Code from an image.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Decoder
  {
    private ReedSolomonDecoder rsDecoder;

    public Decoder()
    {
      this.rsDecoder = new ReedSolomonDecoder(GF256.QR_CODE_FIELD);
    }

    /// <summary>
    /// <p>Convenience method that can decode a QR Code represented as a 2D array of booleans.
    ///             "true" is taken to mean a black module.</p>
    /// </summary>
    /// <param name="image">booleans representing white/black QR Code modules
    ///             </param>
    /// <returns>
    /// text and bytes encoded within the QR Code
    /// 
    /// </returns>
    /// <throws>ReaderException if the QR Code cannot be decoded </throws>
    public DecoderResult decode(bool[][] image)
    {
      int length = image.Length;
      BitMatrix bits = new BitMatrix(length);
      for (int y = 0; y < length; ++y)
      {
        for (int x = 0; x < length; ++x)
        {
          if (image[y][x])
            bits.set_Renamed(x, y);
        }
      }
      return this.decode(bits);
    }

    /// <summary>
    /// <p>Decodes a QR Code represented as a {@link BitMatrix}. A 1 or "true" is taken to mean a black module.</p>
    /// </summary>
    /// <param name="bits">booleans representing white/black QR Code modules
    ///             </param>
    /// <returns>
    /// text and bytes encoded within the QR Code
    /// 
    /// </returns>
    /// <throws>ReaderException if the QR Code cannot be decoded </throws>
    public DecoderResult decode(BitMatrix bits)
    {
      BitMatrixParser bitMatrixParser = new BitMatrixParser(bits);
      Version version = bitMatrixParser.readVersion();
      ErrorCorrectionLevel errorCorrectionLevel = bitMatrixParser.readFormatInformation().ErrorCorrectionLevel;
      DataBlock[] dataBlocks = DataBlock.getDataBlocks(bitMatrixParser.readCodewords(), version, errorCorrectionLevel);
      int length = 0;
      for (int index = 0; index < dataBlocks.Length; ++index)
        length += dataBlocks[index].NumDataCodewords;
      sbyte[] bytes = new sbyte[length];
      int num = 0;
      for (int index1 = 0; index1 < dataBlocks.Length; ++index1)
      {
        DataBlock dataBlock = dataBlocks[index1];
        sbyte[] codewords = dataBlock.Codewords;
        int numDataCodewords = dataBlock.NumDataCodewords;
        this.correctErrors(codewords, numDataCodewords);
        for (int index2 = 0; index2 < numDataCodewords; ++index2)
          bytes[num++] = codewords[index2];
      }
      return DecodedBitStreamParser.decode(bytes, version, errorCorrectionLevel);
    }

    /// <summary>
    /// <p>Given data and error-correction codewords received, possibly corrupted by errors, attempts to
    ///             correct the errors in-place using Reed-Solomon error correction.</p>
    /// </summary>
    /// <param name="codewordBytes">data and error correction codewords
    ///             </param><param name="numDataCodewords">number of codewords that are data bytes
    ///             </param><throws>ReaderException if error correction fails </throws>
    private void correctErrors(sbyte[] codewordBytes, int numDataCodewords)
    {
      int length = codewordBytes.Length;
      int[] received = new int[length];
      for (int index = 0; index < length; ++index)
        received[index] = (int) codewordBytes[index] & (int) byte.MaxValue;
      int twoS = codewordBytes.Length - numDataCodewords;
      try
      {
        this.rsDecoder.decode(received, twoS);
      }
      catch (ReedSolomonException ex)
      {
        throw ReaderException.Instance;
      }
      for (int index = 0; index < numDataCodewords; ++index)
        codewordBytes[index] = (sbyte) received[index];
    }
  }
}
