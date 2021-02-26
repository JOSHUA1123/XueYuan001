// Decompiled with JetBrains decompiler
// Type: com.google.zxing.Binarizer
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.common;
using System;

namespace com.google.zxing
{
  /// <summary>
  /// This class hierarchy provides a set of methods to convert luminance data to 1 bit data.
  ///             It allows the algorithm to vary polymorphically, for example allowing a very expensive
  ///             thresholding technique for servers and a fast one for mobile. It also permits the implementation
  ///             to vary, e.g. a JNI version for Android and a Java fallback version for other platforms.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class Binarizer
  {
    private LuminanceSource source;

    public virtual LuminanceSource LuminanceSource
    {
      get
      {
        return this.source;
      }
    }

    /// <summary>
    /// Converts a 2D array of luminance data to 1 bit data. As above, assume this method is expensive
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
    public abstract BitMatrix BlackMatrix { get; }

    protected internal Binarizer(LuminanceSource source)
    {
      if (source == null)
        throw new ArgumentException("Source must be non-null.");
      this.source = source;
    }

    public abstract BitArray getBlackRow(int y, BitArray row);

    /// <summary>
    /// Creates a new object with the same type as this Binarizer implementation, but with pristine
    ///             state. This is needed because Binarizer implementations may be stateful, e.g. keeping a cache
    ///             of 1 bit data. See Effective Java for why we can't use Java's clone() method.
    /// 
    /// 
    /// </summary>
    /// <param name="source">The LuminanceSource this Binarizer will operate on.
    ///             </param>
    /// <returns>
    /// A new concrete Binarizer implementation object.
    /// 
    /// </returns>
    public abstract Binarizer createBinarizer(LuminanceSource source);
  }
}
