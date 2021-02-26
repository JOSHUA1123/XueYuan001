// Decompiled with JetBrains decompiler
// Type: com.google.zxing.multi.qrcode.QRCodeMultiReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.multi;
using com.google.zxing.multi.qrcode.detector;
using com.google.zxing.qrcode;
using System.Collections;

namespace com.google.zxing.multi.qrcode
{
  /// <summary>
  /// This implementation can detect and decode multiple QR Codes in an image.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>Hannes Erven
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class QRCodeMultiReader : QRCodeReader, MultipleBarcodeReader
  {
    private static readonly Result[] EMPTY_RESULT_ARRAY = new Result[0];

    public Result[] decodeMultiple(BinaryBitmap image)
    {
      return this.decodeMultiple(image, (Hashtable) null);
    }

    public Result[] decodeMultiple(BinaryBitmap image, Hashtable hints)
    {
      ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
      DetectorResult[] detectorResultArray = new MultiDetector(image.BlackMatrix).detectMulti(hints);
      for (int index = 0; index < detectorResultArray.Length; ++index)
      {
        try
        {
          DecoderResult decoderResult = this.Decoder.decode(detectorResultArray[index].Bits);
          ResultPoint[] points = detectorResultArray[index].Points;
          Result result = new Result(decoderResult.Text, decoderResult.RawBytes, points, BarcodeFormat.QR_CODE);
          if (decoderResult.ByteSegments != null)
            result.putMetadata(ResultMetadataType.BYTE_SEGMENTS, (object) decoderResult.ByteSegments);
          if (decoderResult.ECLevel != null)
            result.putMetadata(ResultMetadataType.ERROR_CORRECTION_LEVEL, (object) decoderResult.ECLevel.ToString());
          arrayList.Add((object) result);
        }
        catch (ReaderException ex)
        {
        }
      }
      if (arrayList.Count == 0)
        return QRCodeMultiReader.EMPTY_RESULT_ARRAY;
      Result[] resultArray = new Result[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        resultArray[index] = (Result) arrayList[index];
      return resultArray;
    }
  }
}
