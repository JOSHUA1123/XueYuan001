// Decompiled with JetBrains decompiler
// Type: com.google.zxing.oned.MultiFormatUPCEANReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.oned
{
  /// <summary>
  /// <p>A reader that can read all available UPC/EAN formats. If a caller wants to try to
  ///             read all such formats, it is most efficient to use this implementation rather than invoke
  ///             individual readers.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class MultiFormatUPCEANReader : OneDReader
  {
    private ArrayList readers;

    public MultiFormatUPCEANReader(Hashtable hints)
    {
      ArrayList arrayList = hints == null ? (ArrayList) null : (ArrayList) hints[(object) DecodeHintType.POSSIBLE_FORMATS];
      this.readers = ArrayList.Synchronized(new ArrayList(10));
      if (arrayList != null)
      {
        if (arrayList.Contains((object) BarcodeFormat.EAN_13))
          this.readers.Add((object) new EAN13Reader());
        else if (arrayList.Contains((object) BarcodeFormat.UPC_A))
          this.readers.Add((object) new UPCAReader());
        if (arrayList.Contains((object) BarcodeFormat.EAN_8))
          this.readers.Add((object) new EAN8Reader());
        if (arrayList.Contains((object) BarcodeFormat.UPC_E))
          this.readers.Add((object) new UPCEReader());
      }
      if (this.readers.Count != 0)
        return;
      this.readers.Add((object) new EAN13Reader());
      this.readers.Add((object) new EAN8Reader());
      this.readers.Add((object) new UPCEReader());
    }

    public override Result decodeRow(int rowNumber, com.google.zxing.common.BitArray row, Hashtable hints)
    {
      int[] startGuardPattern = UPCEANReader.findStartGuardPattern(row);
      int count = this.readers.Count;
      for (int index = 0; index < count; ++index)
      {
        UPCEANReader upceanReader = (UPCEANReader) this.readers[index];
        Result result;
        try
        {
          result = upceanReader.decodeRow(rowNumber, row, startGuardPattern, hints);
        }
        catch (ReaderException ex)
        {
          continue;
        }
        if (result.BarcodeFormat.Equals((object) BarcodeFormat.EAN_13) && (int) result.Text[0] == 48)
          return new Result(result.Text.Substring(1), (sbyte[]) null, result.ResultPoints, BarcodeFormat.UPC_A);
        return result;
      }
      throw ReaderException.Instance;
    }
  }
}
