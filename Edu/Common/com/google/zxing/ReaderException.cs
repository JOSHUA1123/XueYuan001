// Decompiled with JetBrains decompiler
// Type: com.google.zxing.ReaderException
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing
{
  /// <summary>
  /// The general exception class throw when something goes wrong during decoding of a barcode.
  ///             This includes, but is not limited to, failing checksums / error correction algorithms, being
  ///             unable to locate finder timing patterns, and so on.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  [Serializable]
  public sealed class ReaderException : Exception
  {
    private static readonly ReaderException instance = new ReaderException();

    public static ReaderException Instance
    {
      get
      {
        return ReaderException.instance;
      }
    }

    private ReaderException()
    {
    }

    public Exception fillInStackTrace()
    {
      return (Exception) null;
    }
  }
}
