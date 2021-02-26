// Decompiled with JetBrains decompiler
// Type: com.google.zxing.WriterException
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing
{
  /// <summary>
  /// A base class which covers the range of exceptions which may occur when encoding a barcode using
  ///             the Writer framework.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  [Serializable]
  public sealed class WriterException : Exception
  {
    public WriterException()
    {
    }

    public WriterException(string message)
      : base(message)
    {
    }
  }
}
