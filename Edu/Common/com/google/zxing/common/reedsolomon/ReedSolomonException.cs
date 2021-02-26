// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.reedsolomon.ReedSolomonException
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.common.reedsolomon
{
  /// <summary>
  /// <p>Thrown when an exception occurs during Reed-Solomon decoding, such as when
  ///             there are too many errors to correct.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  [Serializable]
  public sealed class ReedSolomonException : Exception
  {
    public ReedSolomonException(string message)
      : base(message)
    {
    }
  }
}
