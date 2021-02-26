// Decompiled with JetBrains decompiler
// Type: com.google.zxing.multi.MultipleBarcodeReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.multi
{
  /// <summary>
  /// Implementation of this interface attempt to read several barcodes from one image.
  /// 
  /// 
  /// </summary>
  /// <seealso cref="T:com.google.zxing.Reader"/><author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public interface MultipleBarcodeReader
  {
    Result[] decodeMultiple(BinaryBitmap image);

    Result[] decodeMultiple(BinaryBitmap image, Hashtable hints);
  }
}
