// Decompiled with JetBrains decompiler
// Type: com.google.zxing.LuminanceSource
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing
{
  /// <summary>
  /// The purpose of this class hierarchy is to abstract different bitmap implementations across
  ///             platforms into a standard interface for requesting greyscale luminance values. The interface
  ///             only provides immutable methods; therefore crop and rotation create copies. This is to ensure
  ///             that one Reader does not modify the original luminance source and leave it in an unknown state
  ///             for other Readers in the chain.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class LuminanceSource
  {
    private int width;
    private int height;

    public abstract sbyte[] Matrix { get; }

    /// <returns>
    /// The width of the bitmap.
    /// 
    /// </returns>
    public virtual int Width
    {
      get
      {
        return this.width;
      }
    }

    /// <returns>
    /// The height of the bitmap.
    /// 
    /// </returns>
    public virtual int Height
    {
      get
      {
        return this.height;
      }
    }

    /// <returns>
    /// Whether this subclass supports cropping.
    /// 
    /// </returns>
    public virtual bool CropSupported
    {
      get
      {
        return false;
      }
    }

    /// <returns>
    /// Whether this subclass supports counter-clockwise rotation.
    /// 
    /// </returns>
    public virtual bool RotateSupported
    {
      get
      {
        return false;
      }
    }

    protected internal LuminanceSource(int width, int height)
    {
      this.width = width;
      this.height = height;
    }

    public abstract sbyte[] getRow(int y, sbyte[] row);

    public virtual LuminanceSource crop(int left, int top, int width, int height)
    {
      throw new SystemException("This luminance source does not support cropping.");
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
    public virtual LuminanceSource rotateCounterClockwise()
    {
      throw new SystemException("This luminance source does not support rotation.");
    }
  }
}
