// Decompiled with JetBrains decompiler
// Type: com.google.zxing.MultiFormatReader
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.datamatrix;
using com.google.zxing.oned;
using com.google.zxing.pdf417;
using com.google.zxing.qrcode;
using System.Collections;

namespace com.google.zxing
{
  /// <summary>
  /// MultiFormatReader is a convenience class and the main entry point into the library for most uses.
  ///             By default it attempts to decode all barcode formats that the library supports. Optionally, you
  ///             can provide a hints object to request different behavior, for example only decoding QR codes.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class MultiFormatReader : Reader
  {
    private Hashtable hints;
    private ArrayList readers;

    /// <summary>
    /// This method adds state to the MultiFormatReader. By setting the hints once, subsequent calls
    ///             to decodeWithState(image) can reuse the same set of readers without reallocating memory. This
    ///             is important for performance in continuous scan clients.
    /// 
    /// 
    /// </summary>
    /// <param name="hints">The set of hints to use for subsequent calls to decode(image)
    ///             </param>
    public Hashtable Hints
    {
      set
      {
        this.hints = value;
        bool flag1 = value != null && value.ContainsKey((object) DecodeHintType.TRY_HARDER);
        ArrayList arrayList = value == null ? (ArrayList) null : (ArrayList) value[(object) DecodeHintType.POSSIBLE_FORMATS];
        this.readers = ArrayList.Synchronized(new ArrayList(10));
        if (arrayList != null)
        {
          bool flag2 = arrayList.Contains((object) BarcodeFormat.UPC_A) || arrayList.Contains((object) BarcodeFormat.UPC_E) || (arrayList.Contains((object) BarcodeFormat.EAN_13) || arrayList.Contains((object) BarcodeFormat.EAN_8)) || (arrayList.Contains((object) BarcodeFormat.CODE_39) || arrayList.Contains((object) BarcodeFormat.CODE_128)) || arrayList.Contains((object) BarcodeFormat.ITF);
          if (flag2 && !flag1)
            this.readers.Add((object) new MultiFormatOneDReader(value));
          if (arrayList.Contains((object) BarcodeFormat.QR_CODE))
            this.readers.Add((object) new QRCodeReader());
          if (arrayList.Contains((object) BarcodeFormat.DATAMATRIX))
            this.readers.Add((object) new DataMatrixReader());
          if (arrayList.Contains((object) BarcodeFormat.PDF417))
            this.readers.Add((object) new PDF417Reader());
          if (flag2 && flag1)
            this.readers.Add((object) new MultiFormatOneDReader(value));
        }
        if (this.readers.Count != 0)
          return;
        if (!flag1)
          this.readers.Add((object) new MultiFormatOneDReader(value));
        this.readers.Add((object) new QRCodeReader());
        if (!flag1)
          return;
        this.readers.Add((object) new MultiFormatOneDReader(value));
      }
    }

    /// <summary>
    /// This version of decode honors the intent of Reader.decode(BinaryBitmap) in that it
    ///             passes null as a hint to the decoders. However, that makes it inefficient to call repeatedly.
    ///             Use setHints() followed by decodeWithState() for continuous scan applications.
    /// 
    /// 
    /// </summary>
    /// <param name="image">The pixel data to decode
    ///             </param>
    /// <returns>
    /// The contents of the image
    /// 
    /// </returns>
    /// <throws>ReaderException Any errors which occurred </throws>
    public Result decode(BinaryBitmap image)
    {
      this.Hints = (Hashtable) null;
      return this.decodeInternal(image);
    }

    /// <summary>
    /// Decode an image using the hints provided. Does not honor existing state.
    /// 
    /// 
    /// </summary>
    /// <param name="image">The pixel data to decode
    ///             </param><param name="hints">The hints to use, clearing the previous state.
    ///             </param>
    /// <returns>
    /// The contents of the image
    /// 
    /// </returns>
    /// <throws>ReaderException Any errors which occurred </throws>
    public Result decode(BinaryBitmap image, Hashtable hints)
    {
      this.Hints = hints;
      return this.decodeInternal(image);
    }

    /// <summary>
    /// Decode an image using the state set up by calling setHints() previously. Continuous scan
    ///             clients will get a <b>large</b> speed increase by using this instead of decode().
    /// 
    /// 
    /// </summary>
    /// <param name="image">The pixel data to decode
    ///             </param>
    /// <returns>
    /// The contents of the image
    /// 
    /// </returns>
    /// <throws>ReaderException Any errors which occurred </throws>
    public Result decodeWithState(BinaryBitmap image)
    {
      if (this.readers == null)
        this.Hints = (Hashtable) null;
      return this.decodeInternal(image);
    }

    private Result decodeInternal(BinaryBitmap image)
    {
      int count = this.readers.Count;
      for (int index = 0; index < count; ++index)
      {
        Reader reader = (Reader) this.readers[index];
        try
        {
          return reader.decode(image, this.hints);
        }
        catch (ReaderException ex)
        {
        }
      }
      throw ReaderException.Instance;
    }
  }
}
