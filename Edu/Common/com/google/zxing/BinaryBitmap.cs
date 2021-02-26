// Decompiled with JetBrains decompiler
// Type: com.google.zxing.BinaryBitmap
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.common;
using System;

namespace com.google.zxing
{
  /// <summary>
  /// This class is the core bitmap class used by ZXing to represent 1 bit data. Reader objects
  ///             accept a BinaryBitmap and attempt to decode it.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class BinaryBitmap
  {
    private Binarizer binarizer;
    private BitMatrix matrix;

    /// <returns>
    /// The width of the bitmap.
    /// 
    /// </returns>
    public int Width
    {
      get
      {
        return this.binarizer.LuminanceSource.Width;
      }
    }

    /// <returns>
    /// The height of the bitmap.
    /// 
    /// </returns>
    public int Height
    {
      get
      {
        return this.binarizer.LuminanceSource.Height;
      }
    }

    /// <summary>
    /// Converts a 2D array of luminance data to 1 bit. As above, assume this method is expensive
    ///             and do not call it repeatedly. This method is intended for decoding 2D barcodes and may or
    ///             may not apply sharpening. Therefore, a row from this matrix may not be identical to one
    ///             fetched using getBlackRow(), so don't mix and match between them.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The 2D array of bits for the image (true means black).
    /// 
    /// </returns>
    public BitMatrix BlackMatrix
    {
      get
      {
        if (this.matrix == null)
          this.matrix = this.binarizer.BlackMatrix;
        return this.matrix;
      }
    }

    /// <returns>
    /// Whether this bitmap can be cropped.
    /// 
    /// </returns>
    public bool CropSupported
    {
      get
      {
        return this.binarizer.LuminanceSource.CropSupported;
      }
    }

    /// <returns>
    /// Whether this bitmap supports counter-clockwise rotation.
    /// 
    /// </returns>
    public bool RotateSupported
    {
      get
      {
        return this.binarizer.LuminanceSource.RotateSupported;
      }
    }

    public BinaryBitmap(Binarizer binarizer)
    {
      if (binarizer == null)
        throw new ArgumentException("Binarizer must be non-null.");
      this.binarizer = binarizer;
      this.matrix = (BitMatrix) null;
    }

    public BitArray getBlackRow(int y, BitArray row)
    {
      return this.binarizer.getBlackRow(y, row);
    }

    public BinaryBitmap crop(int left, int top, int width, int height)
    {
      return new BinaryBitmap(this.binarizer.createBinarizer(this.binarizer.LuminanceSource.crop(left, top, width, height)));
    }

    /// <summary>
    /// Returns a new object with rotated image data. Only callable if isRotateSupported() is true.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// A rotated version of this object.
    /// 
    /// </returns>
    public BinaryBitmap rotateCounterClockwise()
    {
      return new BinaryBitmap(this.binarizer.createBinarizer(this.binarizer.LuminanceSource.rotateCounterClockwise()));
    }
  }
}
