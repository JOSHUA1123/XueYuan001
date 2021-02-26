// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.MultiFormatOneDReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.oned
{
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class MultiFormatOneDReader : OneDReader
  {
    private ArrayList readers;

    public MultiFormatOneDReader(Hashtable hints)
    {
      ArrayList arrayList = hints == null ? (ArrayList) null : (ArrayList) hints[(object) DecodeHintType.POSSIBLE_FORMATS];
      bool usingCheckDigit = hints != null && hints[(object) DecodeHintType.ASSUME_CODE_39_CHECK_DIGIT] != null;
      this.readers = ArrayList.Synchronized(new ArrayList(10));
      if (arrayList != null)
      {
        if (arrayList.Contains((object) BarcodeFormat.EAN_13) || arrayList.Contains((object) BarcodeFormat.UPC_A) || (arrayList.Contains((object) BarcodeFormat.EAN_8) || arrayList.Contains((object) BarcodeFormat.UPC_E)))
          this.readers.Add((object) new MultiFormatUPCEANReader(hints));
        if (arrayList.Contains((object) BarcodeFormat.CODE_39))
          this.readers.Add((object) new Code39Reader(usingCheckDigit));
        if (arrayList.Contains((object) BarcodeFormat.CODE_128))
          this.readers.Add((object) new Code128Reader());
        if (arrayList.Contains((object) BarcodeFormat.ITF))
          this.readers.Add((object) new ITFReader());
      }
      if (this.readers.Count != 0)
        return;
      this.readers.Add((object) new MultiFormatUPCEANReader(hints));
      this.readers.Add((object) new Code39Reader());
      this.readers.Add((object) new Code128Reader());
      this.readers.Add((object) new ITFReader());
    }

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints)
    {
      int count = this.readers.Count;
      for (int index = 0; index < count; ++index)
      {
        OneDReader oneDreader = (OneDReader) this.readers[index];
        try
        {
          return oneDreader.decodeRow(rowNumber, row, hints);
        }
        catch (ReaderException ex)
        {
        }
      }
      throw ReaderException.Instance;
    }
  }
}
