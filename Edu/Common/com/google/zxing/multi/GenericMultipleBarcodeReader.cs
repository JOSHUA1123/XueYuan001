// Decompiled with JetBrains decompiler
// Type: com.google.zxing.multi.GenericMultipleBarcodeReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.multi
{
  /// <summary>
  /// <p>Attempts to locate multiple barcodes in an image by repeatedly decoding portion of the image.
  ///             After one barcode is found, the areas left, above, right and below the barcode's
  ///             {@link com.google.zxing.ResultPoint}s are scanned, recursively.</p><p>A caller may want to also employ {@link ByQuadrantReader} when attempting to find multiple
  ///             2D barcodes, like QR Codes, in an image, where the presence of multiple barcodes might prevent
  ///             detecting any one of them.</p><p>That is, instead of passing a {@link Reader} a caller might pass
  /// 
  /// <code>
  /// new ByQuadrantReader(reader)
  /// </code>
  /// .</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class GenericMultipleBarcodeReader : MultipleBarcodeReader
  {
    private const int MIN_DIMENSION_TO_RECUR = 30;
    private Reader delegate_Renamed;

    public GenericMultipleBarcodeReader(Reader delegate_Renamed)
    {
      this.delegate_Renamed = delegate_Renamed;
    }

    public Result[] decodeMultiple(BinaryBitmap image)
    {
      return this.decodeMultiple(image, (Hashtable) null);
    }

    public Result[] decodeMultiple(BinaryBitmap image, Hashtable hints)
    {
      ArrayList results = ArrayList.Synchronized(new ArrayList(10));
      this.doDecodeMultiple(image, hints, results, 0, 0);
      if (results.Count == 0)
        throw ReaderException.Instance;
      int count = results.Count;
      Result[] resultArray = new Result[count];
      for (int index = 0; index < count; ++index)
        resultArray[index] = (Result) results[index];
      return resultArray;
    }

    private void doDecodeMultiple(BinaryBitmap image, Hashtable hints, ArrayList results, int xOffset, int yOffset)
    {
      Result result;
      try
      {
        result = this.delegate_Renamed.decode(image, hints);
      }
      catch (ReaderException ex)
      {
        return;
      }
      bool flag = false;
      for (int index = 0; index < results.Count; ++index)
      {
        if (((Result) results[index]).Text.Equals(result.Text))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      results.Add((object) GenericMultipleBarcodeReader.translateResultPoints(result, xOffset, yOffset));
      ResultPoint[] resultPoints = result.ResultPoints;
      if (resultPoints == null || resultPoints.Length == 0)
        return;
      int width = image.Width;
      int height = image.Height;
      float num1 = (float) width;
      float num2 = (float) height;
      float num3 = 0.0f;
      float num4 = 0.0f;
      for (int index = 0; index < resultPoints.Length; ++index)
      {
        ResultPoint resultPoint = resultPoints[index];
        float x = resultPoint.X;
        float y = resultPoint.Y;
        if ((double) x < (double) num1)
          num1 = x;
        if ((double) y < (double) num2)
          num2 = y;
        if ((double) x > (double) num3)
          num3 = x;
        if ((double) y > (double) num4)
          num4 = y;
      }
      if ((double) num1 > 30.0)
        this.doDecodeMultiple(image.crop(0, 0, (int) num1, height), hints, results, xOffset, yOffset);
      if ((double) num2 > 30.0)
        this.doDecodeMultiple(image.crop(0, 0, width, (int) num2), hints, results, xOffset, yOffset);
      if ((double) num3 < (double) (width - 30))
        this.doDecodeMultiple(image.crop((int) num3, 0, width - (int) num3, height), hints, results, xOffset + (int) num3, yOffset);
      if ((double) num4 >= (double) (height - 30))
        return;
      this.doDecodeMultiple(image.crop(0, (int) num4, width, height - (int) num4), hints, results, xOffset, yOffset + (int) num4);
    }

    private static Result translateResultPoints(Result result, int xOffset, int yOffset)
    {
      ResultPoint[] resultPoints1 = result.ResultPoints;
      ResultPoint[] resultPoints2 = new ResultPoint[resultPoints1.Length];
      for (int index = 0; index < resultPoints1.Length; ++index)
      {
        ResultPoint resultPoint = resultPoints1[index];
        resultPoints2[index] = new ResultPoint(resultPoint.X + (float) xOffset, resultPoint.Y + (float) yOffset);
      }
      return new Result(result.Text, result.RawBytes, resultPoints2, result.BarcodeFormat);
    }
  }
}
