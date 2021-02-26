// Decompiled with JetBrains decompiler
// Type: com.google.zxing.ResultPointCallback
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing
{
  /// <summary>
  /// Callback which is invoked when a possible result point (significant
  ///             point in the barcode image such as a corner) is found.
  /// 
  /// 
  /// </summary>
  /// <seealso cref="F:com.google.zxing.DecodeHintType.NEED_RESULT_POINT_CALLBACK"/>
  public interface ResultPointCallback
  {
    void foundPossibleResultPoint(ResultPoint point);
  }
}
