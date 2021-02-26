// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.encoder.BlockPair
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.common;

namespace com.google.zxing.qrcode.encoder
{
  internal sealed class BlockPair
  {
    private ByteArray dataBytes;
    private ByteArray errorCorrectionBytes;

    public ByteArray DataBytes
    {
      get
      {
        return this.dataBytes;
      }
    }

    public ByteArray ErrorCorrectionBytes
    {
      get
      {
        return this.errorCorrectionBytes;
      }
    }

    internal BlockPair(ByteArray data, ByteArray errorCorrection)
    {
      this.dataBytes = data;
      this.errorCorrectionBytes = errorCorrection;
    }
  }
}
