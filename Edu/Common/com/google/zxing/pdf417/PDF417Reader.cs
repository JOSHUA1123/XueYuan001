// Decompiled with JetBrains decompiler
// Type: com.google.zxing.pdf417.PDF417Reader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.pdf417.decoder;
using com.google.zxing.pdf417.detector;
using System;
using System.Collections;

namespace com.google.zxing.pdf417
{
  /// <summary>
  /// This implementation can detect and decode PDF417 codes in an image.
  /// 
  /// 
  /// </summary>
  /// <author>SITA Lab (kevin.osullivan@sita.aero)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class PDF417Reader : Reader
  {
    private static readonly ResultPoint[] NO_POINTS = new ResultPoint[0];
    private Decoder decoder = new Decoder();

    /// <summary>
    /// Locates and decodes a PDF417 code in an image.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// a String representing the content encoded by the PDF417 code
    /// 
    /// </returns>
    /// <throws>ReaderException if a PDF417 code cannot be found, or cannot be decoded </throws>
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
        decoderResult = this.decoder.decode(PDF417Reader.extractPureBits(image));
        resultPoints = PDF417Reader.NO_POINTS;
      }
      else
      {
        DetectorResult detectorResult = new Detector(image).detect();
        decoderResult = this.decoder.decode(detectorResult.Bits);
        resultPoints = detectorResult.Points;
      }
      return new Result(decoderResult.Text, decoderResult.RawBytes, resultPoints, BarcodeFormat.PDF417);
    }

    /// <summary>
    /// This method detects a barcode in a "pure" image -- that is, pure monochrome image
    ///             which contains only an unrotated, unskewed, image of a barcode, with some white border
    ///             around it. This is a specialized method that works exceptionally fast in this special
    ///             case.
    /// 
    /// </summary>
    private static BitMatrix extractPureBits(BinaryBitmap image)
    {
      BitMatrix blackMatrix = image.BlackMatrix;
      int height = blackMatrix.Height;
      int width = blackMatrix.Width;
      int num1 = Math.Min(height, width);
      int num2 = 0;
      while (num2 < num1 && !blackMatrix.get_Renamed(num2, num2))
        ++num2;
      if (num2 == num1)
        throw ReaderException.Instance;
      int num3 = num2;
      while (num3 < num1 && blackMatrix.get_Renamed(num3, num3))
        ++num3;
      if (num3 == num1)
        throw ReaderException.Instance;
      int num4 = num3 - num2;
      int x1 = width - 1;
      while (x1 >= 0 && !blackMatrix.get_Renamed(x1, num2))
        --x1;
      if (x1 < 0)
        throw ReaderException.Instance;
      int num5 = x1 + 1;
      if ((num5 - num2) % num4 != 0)
        throw ReaderException.Instance;
      int dimension = (num5 - num2) / num4;
      int num6 = num2 + (num4 >> 1);
      int num7 = num6 + (dimension - 1) * num4;
      if (num7 >= width || num7 >= height)
        throw ReaderException.Instance;
      BitMatrix bitMatrix = new BitMatrix(dimension);
      for (int y1 = 0; y1 < dimension; ++y1)
      {
        int y2 = num6 + y1 * num4;
        for (int x2 = 0; x2 < dimension; ++x2)
        {
          if (blackMatrix.get_Renamed(num6 + x2 * num4, y2))
            bitMatrix.set_Renamed(x2, y1);
        }
      }
      return bitMatrix;
    }
  }
}
