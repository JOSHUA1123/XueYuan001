// Decompiled with JetBrains decompiler
// Type: com.google.zxing.datamatrix.DataMatrixReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.datamatrix.decoder;
using com.google.zxing.datamatrix.detector;
using System;
using System.Collections;

namespace com.google.zxing.datamatrix
{
  /// <summary>
  /// This implementation can detect and decode Data Matrix codes in an image.
  /// 
  /// 
  /// </summary>
  /// <author>bbrown@google.com (Brian Brown)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class DataMatrixReader : Reader
  {
    private static readonly ResultPoint[] NO_POINTS = new ResultPoint[0];
    private Decoder decoder = new Decoder();

    /// <summary>
    /// Locates and decodes a Data Matrix code in an image.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// a String representing the content encoded by the Data Matrix code
    /// 
    /// </returns>
    /// <throws>ReaderException if a Data Matrix code cannot be found, or cannot be decoded </throws>
    public Result decode(BinaryBitmap image)
    {
      return this.decode(image, (Hashtable) null);
    }

    public Result decode(BinaryBitmap image, Hashtable hints)
    {
      DecoderResult decoderResult;
      ResultPoint[] resultPoints;
      if (hints != null && hints.ContainsKey((object) DecodeHintType.PURE_BARCODE))
      {
        decoderResult = this.decoder.decode(DataMatrixReader.extractPureBits(image.BlackMatrix));
        resultPoints = DataMatrixReader.NO_POINTS;
      }
      else
      {
        DetectorResult detectorResult = new Detector(image.BlackMatrix).detect();
        decoderResult = this.decoder.decode(detectorResult.Bits);
        resultPoints = detectorResult.Points;
      }
      Result result = new Result(decoderResult.Text, decoderResult.RawBytes, resultPoints, BarcodeFormat.DATAMATRIX);
      if (decoderResult.ByteSegments != null)
        result.putMetadata(ResultMetadataType.BYTE_SEGMENTS, (object) decoderResult.ByteSegments);
      if (decoderResult.ECLevel != null)
        result.putMetadata(ResultMetadataType.ERROR_CORRECTION_LEVEL, (object) decoderResult.ECLevel.ToString());
      return result;
    }

    /// <summary>
    /// This method detects a Data Matrix code in a "pure" image -- that is, pure monochrome image
    ///             which contains only an unrotated, unskewed, image of a Data Matrix code, with some white border
    ///             around it. This is a specialized method that works exceptionally fast in this special
    ///             case.
    /// 
    /// </summary>
    private static BitMatrix extractPureBits(BitMatrix image)
    {
      int height = image.Height;
      int width = image.Width;
      int num1 = Math.Min(height, width);
      int num2 = 0;
      while (num2 < num1 && !image.get_Renamed(num2, num2))
        ++num2;
      if (num2 == num1)
        throw ReaderException.Instance;
      int x1 = num2 + 1;
      while (x1 < width && image.get_Renamed(x1, num2))
        ++x1;
      if (x1 == width)
        throw ReaderException.Instance;
      int num3 = x1 - num2;
      int y1 = height - 1;
      while (y1 >= 0 && !image.get_Renamed(num2, y1))
        --y1;
      if (y1 < 0)
        throw ReaderException.Instance;
      int num4 = y1 + 1;
      if ((num4 - num2) % num3 != 0)
        throw ReaderException.Instance;
      int dimension = (num4 - num2) / num3;
      int num5 = num2 + (num3 >> 1);
      int num6 = num5 + (dimension - 1) * num3;
      if (num6 >= width || num6 >= height)
        throw ReaderException.Instance;
      BitMatrix bitMatrix = new BitMatrix(dimension);
      for (int y2 = 0; y2 < dimension; ++y2)
      {
        int y3 = num5 + y2 * num3;
        for (int x2 = 0; x2 < dimension; ++x2)
        {
          if (image.get_Renamed(num5 + x2 * num3, y3))
            bitMatrix.set_Renamed(x2, y2);
        }
      }
      return bitMatrix;
    }
  }
}
