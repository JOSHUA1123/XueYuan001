// Decompiled with JetBrains decompiler
// Type: com.google.zxing.multi.ByQuadrantReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.multi
{
  /// <summary>
  /// This class attempts to decode a barcode from an image, not by scanning the whole image,
  ///             but by scanning subsets of the image. This is important when there may be multiple barcodes in
  ///             an image, and detecting a barcode may find parts of multiple barcode and fail to decode
  ///             (e.g. QR Codes). Instead this scans the four quadrants of the image -- and also the center
  ///             'quadrant' to cover the case where a barcode is found in the center.
  /// 
  /// 
  /// </summary>
  /// <seealso cref="T:com.google.zxing.multi.GenericMultipleBarcodeReader"/>
  public sealed class ByQuadrantReader : Reader
  {
    private Reader delegate_Renamed;

    public ByQuadrantReader(Reader delegate_Renamed)
    {
      this.delegate_Renamed = delegate_Renamed;
    }

    public Result decode(BinaryBitmap image)
    {
      return this.decode(image, (Hashtable) null);
    }

    public Result decode(BinaryBitmap image, Hashtable hints)
    {
      int width = image.Width;
      int height = image.Height;
      int num1 = width / 2;
      int num2 = height / 2;
      BinaryBitmap image1 = image.crop(0, 0, num1, num2);
      try
      {
        return this.delegate_Renamed.decode(image1, hints);
      }
      catch (ReaderException ex)
      {
      }
      BinaryBitmap image2 = image.crop(num1, 0, num1, num2);
      try
      {
        return this.delegate_Renamed.decode(image2, hints);
      }
      catch (ReaderException ex)
      {
      }
      BinaryBitmap image3 = image.crop(0, num2, num1, num2);
      try
      {
        return this.delegate_Renamed.decode(image3, hints);
      }
      catch (ReaderException ex)
      {
      }
      BinaryBitmap image4 = image.crop(num1, num2, num1, num2);
      try
      {
        return this.delegate_Renamed.decode(image4, hints);
      }
      catch (ReaderException ex)
      {
      }
      int left = num1 / 2;
      int top = num2 / 2;
      return this.delegate_Renamed.decode(image.crop(left, top, num1, num2), hints);
    }
  }
}
