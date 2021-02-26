// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.UPCAReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;
using System.Text;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>Implements decoding of the UPC-A format.</p>
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class UPCAReader : UPCEANReader
  {
    private UPCEANReader ean13Reader = (UPCEANReader) new EAN13Reader();

    internal override BarcodeFormat BarcodeFormat
    {
      get
      {
        return BarcodeFormat.UPC_A;
      }
    }

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, int[] startGuardRange, Hashtable hints)
    {
      return UPCAReader.maybeReturnResult(this.ean13Reader.decodeRow(rowNumber, row, startGuardRange, hints));
    }

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints)
    {
      return UPCAReader.maybeReturnResult(this.ean13Reader.decodeRow(rowNumber, row, hints));
    }

    public override Result decode(BinaryBitmap image)
    {
      return UPCAReader.maybeReturnResult(this.ean13Reader.decode(image));
    }

    public override Result decode(BinaryBitmap image, Hashtable hints)
    {
      return UPCAReader.maybeReturnResult(this.ean13Reader.decode(image, hints));
    }

    protected internal override int decodeMiddle(com.google.zxing.common.BitArray row, int[] startRange, StringBuilder resultString)
    {
      return this.ean13Reader.decodeMiddle(row, startRange, resultString);
    }

    private static Result maybeReturnResult(Result result)
    {
      string text = result.Text;
      if ((int) text[0] == 48)
        return new Result(text.Substring(1), (sbyte[]) null, result.ResultPoints, BarcodeFormat.UPC_A);
      throw ReaderException.Instance;
    }
  }
}
