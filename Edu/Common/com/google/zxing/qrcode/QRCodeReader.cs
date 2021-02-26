// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.QRCodeReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.detector;
using System;
using System.Collections;

namespace com.google.zxing.qrcode
{
  /// <summary>
  /// This implementation can detect and decode QR Codes in an image.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public class QRCodeReader : Reader
  {
    private static readonly ResultPoint[] NO_POINTS = new ResultPoint[0];
    private Decoder decoder = new Decoder();

    protected internal virtual Decoder Decoder
    {
      get
      {
        return this.decoder;
      }
    }

    /// <summary>
    /// Locates and decodes a QR code in an image.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// a String representing the content encoded by the QR code
    /// 
    /// </returns>
    /// <throws>ReaderException if a QR code cannot be found, or cannot be decoded </throws>
    public virtual Result decode(BinaryBitmap image)
    {
      return this.decode(image, (Hashtable) null);
    }

    public virtual Result decode(BinaryBitmap image, Hashtable hints)
    {
      DecoderResult decoderResult;
      ResultPoint[] resultPoints;
      if (hints != null && hints.ContainsKey((object) DecodeHintType.PURE_BARCODE))
      {
        decoderResult = this.decoder.decode(QRCodeReader.extractPureBits(image.BlackMatrix));
        resultPoints = QRCodeReader.NO_POINTS;
      }
      else
      {
        DetectorResult detectorResult = new Detector(image.BlackMatrix).detect(hints);
        decoderResult = this.decoder.decode(detectorResult.Bits);
        resultPoints = detectorResult.Points;
      }
      Result result = new Result(decoderResult.Text, decoderResult.RawBytes, resultPoints, BarcodeFormat.QR_CODE);
      if (decoderResult.ByteSegments != null)
        result.putMetadata(ResultMetadataType.BYTE_SEGMENTS, (object) decoderResult.ByteSegments);
      if (decoderResult.ECLevel != null)
        result.putMetadata(ResultMetadataType.ERROR_CORRECTION_LEVEL, (object) decoderResult.ECLevel.ToString());
      return result;
    }

    /// <summary>
    /// This method detects a barcode in a "pure" image -- that is, pure monochrome image
    ///             which contains only an unrotated, unskewed, image of a barcode, with some white border
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
      int num3 = num2;
      while (num3 < num1 && image.get_Renamed(num3, num3))
        ++num3;
      if (num3 == num1)
        throw ReaderException.Instance;
      int num4 = num3 - num2;
      int x1 = width - 1;
      while (x1 >= 0 && !image.get_Renamed(x1, num2))
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
          if (image.get_Renamed(num6 + x2 * num4, y2))
            bitMatrix.set_Renamed(x2, y1);
        }
      }
      return bitMatrix;
    }
  }
}
