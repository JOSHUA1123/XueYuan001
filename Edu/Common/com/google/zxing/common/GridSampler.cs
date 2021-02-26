// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.GridSampler
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;

namespace com.google.zxing.common
{
  /// <summary>
  /// Implementations of this class can, given locations of finder patterns for a QR code in an
  ///             image, sample the right points in the image to reconstruct the QR code, accounting for
  ///             perspective distortion. It is abstracted since it is relatively expensive and should be allowed
  ///             to take advantage of platform-specific optimized implementations, like Sun's Java Advanced
  ///             Imaging library, but which may not be available in other environments such as J2ME, and vice
  ///             versa.
  /// 
  ///             The implementation used can be controlled by calling {@link #setGridSampler(GridSampler)}
  ///             with an instance of a class which implements this interface.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class GridSampler
  {
    private static GridSampler gridSampler = (GridSampler) new DefaultGridSampler();

    /// <returns>
    /// the current implementation of {@link GridSampler}
    /// 
    /// </returns>
    public static GridSampler Instance
    {
      get
      {
        return GridSampler.gridSampler;
      }
    }

    /// <summary>
    /// Sets the implementation of {@link GridSampler} used by the library. One global
    ///             instance is stored, which may sound problematic. But, the implementation provided
    ///             ought to be appropriate for the entire platform, and all uses of this library
    ///             in the whole lifetime of the JVM. For instance, an Android activity can swap in
    ///             an implementation that takes advantage of native platform libraries.
    /// 
    /// 
    /// </summary>
    /// <param name="newGridSampler">The platform-specific object to install.
    ///             </param>
    public static void setGridSampler(GridSampler newGridSampler)
    {
      if (newGridSampler == null)
        throw new ArgumentException();
      GridSampler.gridSampler = newGridSampler;
    }

    /// <summary>
    /// <p>Samples an image for a square matrix of bits of the given dimension. This is used to extract
    ///             the black/white modules of a 2D barcode like a QR Code found in an image. Because this barcode
    ///             may be rotated or perspective-distorted, the caller supplies four points in the source image
    ///             that define known points in the barcode, so that the image may be sampled appropriately.</p><p>The last eight "from" parameters are four X/Y coordinate pairs of locations of points in
    ///             the image that define some significant points in the image to be sample. For example,
    ///             these may be the location of finder pattern in a QR Code.</p><p>The first eight "to" parameters are four X/Y coordinate pairs measured in the destination
    ///             {@link BitMatrix}, from the top left, where the known points in the image given by the "from"
    ///             parameters map to.</p><p>These 16 parameters define the transformation needed to sample the image.</p>
    /// </summary>
    /// <param name="image">image to sample
    ///             </param><param name="dimension">width/height of {@link BitMatrix} to sample from image
    ///             </param>
    /// <returns>
    /// {@link BitMatrix} representing a grid of points sampled from the image within a region
    ///             defined by the "from" parameters
    /// 
    /// </returns>
    /// <throws>ReaderException if image can't be sampled, for example, if the transformation defined </throws>
    /// <summary>
    /// by the given points is invalid or results in sampling outside the image boundaries
    /// 
    /// </summary>
    public abstract BitMatrix sampleGrid(BitMatrix image, int dimension, float p1ToX, float p1ToY, float p2ToX, float p2ToY, float p3ToX, float p3ToY, float p4ToX, float p4ToY, float p1FromX, float p1FromY, float p2FromX, float p2FromY, float p3FromX, float p3FromY, float p4FromX, float p4FromY);

    public virtual BitMatrix sampleGrid(BitMatrix image, int dimension, PerspectiveTransform transform)
    {
      throw new NotSupportedException();
    }

    /// <summary>
    /// <p>Checks a set of points that have been transformed to sample points on an image against
    ///             the image's dimensions to see if the point are even within the image.</p><p>This method will actually "nudge" the endpoints back onto the image if they are found to be
    ///             barely (less than 1 pixel) off the image. This accounts for imperfect detection of finder
    ///             patterns in an image where the QR Code runs all the way to the image border.</p><p>For efficiency, the method will check points from either end of the line until one is found
    ///             to be within the image. Because the set of points are assumed to be linear, this is valid.</p>
    /// </summary>
    /// <param name="image">image into which the points should map
    ///             </param><param name="points">actual points in x1,y1,...,xn,yn form
    ///             </param><throws>ReaderException if an endpoint is lies outside the image boundaries </throws>
    protected internal static void checkAndNudgePoints(BitMatrix image, float[] points)
    {
      int width = image.Width;
      int height = image.Height;
      bool flag1 = true;
      int index1 = 0;
      while (index1 < points.Length && flag1)
      {
        int num1 = (int) points[index1];
        int num2 = (int) points[index1 + 1];
        if (num1 < -1 || num1 > width || (num2 < -1 || num2 > height))
          throw ReaderException.Instance;
        flag1 = false;
        if (num1 == -1)
        {
          points[index1] = 0.0f;
          flag1 = true;
        }
        else if (num1 == width)
        {
          points[index1] = (float) (width - 1);
          flag1 = true;
        }
        if (num2 == -1)
        {
          points[index1 + 1] = 0.0f;
          flag1 = true;
        }
        else if (num2 == height)
        {
          points[index1 + 1] = (float) (height - 1);
          flag1 = true;
        }
        index1 += 2;
      }
      bool flag2 = true;
      int index2 = points.Length - 2;
      while (index2 >= 0 && flag2)
      {
        int num1 = (int) points[index2];
        int num2 = (int) points[index2 + 1];
        if (num1 < -1 || num1 > width || (num2 < -1 || num2 > height))
          throw ReaderException.Instance;
        flag2 = false;
        if (num1 == -1)
        {
          points[index2] = 0.0f;
          flag2 = true;
        }
        else if (num1 == width)
        {
          points[index2] = (float) (width - 1);
          flag2 = true;
        }
        if (num2 == -1)
        {
          points[index2 + 1] = 0.0f;
          flag2 = true;
        }
        else if (num2 == height)
        {
          points[index2 + 1] = (float) (height - 1);
          flag2 = true;
        }
        index2 -= 2;
      }
    }
  }
}
