// Decompiled with JetBrains decompiler
// Type: com.google.zxing.pdf417.decoder.Decoder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;

namespace com.google.zxing.pdf417.decoder
{
  /// <summary>
  /// <p>The main class which implements PDF417 Code decoding -- as
  ///             opposed to locating and extracting the PDF417 Code from an image.</p>
  /// </summary>
  /// <author>SITA Lab (kevin.osullivan@sita.aero)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Decoder
  {
    private const int MAX_ERRORS = 3;
    private const int MAX_EC_CODEWORDS = 512;

    /// <summary>
    /// <p>Convenience method that can decode a PDF417 Code represented as a 2D array of booleans.
    ///             "true" is taken to mean a black module.</p>
    /// </summary>
    /// <param name="image">booleans representing white/black PDF417 modules
    ///             </param>
    /// <returns>
    /// text and bytes encoded within the PDF417 Code
    /// 
    /// </returns>
    /// <throws>ReaderException if the PDF417 Code cannot be decoded </throws>
    public DecoderResult decode(bool[][] image)
    {
      int length = image.Length;
      BitMatrix bits = new BitMatrix(length);
      for (int y = 0; y < length; ++y)
      {
        for (int x = 0; x < length; ++x)
        {
          if (image[x][y])
            bits.set_Renamed(x, y);
        }
      }
      return this.decode(bits);
    }

    /// <summary>
    /// <p>Decodes a PDF417 Code represented as a {@link BitMatrix}.
    ///             A 1 or "true" is taken to mean a black module.</p>
    /// </summary>
    /// <param name="bits">booleans representing white/black PDF417 Code modules
    ///             </param>
    /// <returns>
    /// text and bytes encoded within the PDF417 Code
    /// 
    /// </returns>
    /// <throws>ReaderException if the PDF417 Code cannot be decoded </throws>
    public DecoderResult decode(BitMatrix bits)
    {
      BitMatrixParser bitMatrixParser = new BitMatrixParser(bits);
      int[] codewords = bitMatrixParser.readCodewords();
      if (codewords == null || codewords.Length == 0)
        throw ReaderException.Instance;
      int numECCodewords = 1 << bitMatrixParser.ECLevel + 1;
      int[] erasures = bitMatrixParser.Erasures;
      Decoder.correctErrors(codewords, erasures, numECCodewords);
      Decoder.verifyCodewordCount(codewords, numECCodewords);
      return DecodedBitStreamParser.decode(codewords);
    }

    /// <summary>
    /// Verify that all is OK with the codeword array.
    /// 
    /// 
    /// </summary>
    /// <param name="codewords"/>
    /// <returns>
    /// an index to the first data codeword.
    /// 
    /// </returns>
    /// <throws>ReaderException </throws>
    private static void verifyCodewordCount(int[] codewords, int numECCodewords)
    {
      if (codewords.Length < 4)
        throw ReaderException.Instance;
      int num = codewords[0];
      if (num > codewords.Length)
        throw ReaderException.Instance;
      if (num != 0)
        return;
      if (numECCodewords >= codewords.Length)
        throw ReaderException.Instance;
      codewords[0] = codewords.Length - numECCodewords;
    }

    /// <summary>
    /// <p>Given data and error-correction codewords received, possibly corrupted by errors, attempts to
    ///             correct the errors in-place using Reed-Solomon error correction.</p>
    /// </summary>
    /// <param name="codewords">data and error correction codewords
    ///             </param><throws>ReaderException if error correction fails </throws>
    private static int correctErrors(int[] codewords, int[] erasures, int numECCodewords)
    {
      if (erasures != null && erasures.Length > numECCodewords / 2 + 3 || (numECCodewords < 0 || numECCodewords > 512))
        throw ReaderException.Instance;
      int num = 0;
      if (erasures != null)
      {
        int length = erasures.Length;
        if (num > 0)
          length -= num;
        if (length > 3)
          throw ReaderException.Instance;
      }
      return num;
    }
  }
}
