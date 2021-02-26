// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.optional.NDEFSmartPosterResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result.optional
{
  /// <summary>
  /// <p>Recognizes an NDEF message that encodes information according to the
  ///             "Smart Poster Record Type Definition" specification.</p><p>This actually only supports some parts of the Smart Poster format: title,
  ///             URI, and action records. Icon records are not supported because the size
  ///             of these records are infeasibly large for barcodes. Size and type records
  ///             are not supported. Multiple titles are not supported.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class NDEFSmartPosterResultParser : AbstractNDEFResultParser
  {
    public static NDEFSmartPosterParsedResult parse(Result result)
    {
      sbyte[] rawBytes = result.RawBytes;
      if (rawBytes == null)
        return (NDEFSmartPosterParsedResult) null;
      NDEFRecord ndefRecord1 = NDEFRecord.readRecord(rawBytes, 0);
      if (ndefRecord1 == null || !ndefRecord1.MessageBegin || !ndefRecord1.MessageEnd)
        return (NDEFSmartPosterParsedResult) null;
      if (!ndefRecord1.Type.Equals("Sp"))
        return (NDEFSmartPosterParsedResult) null;
      int offset = 0;
      int num = 0;
      NDEFRecord ndefRecord2 = (NDEFRecord) null;
      sbyte[] payload = ndefRecord1.Payload;
      int action = -1;
      string title = (string) null;
      string uri = (string) null;
      while (offset < payload.Length && (ndefRecord2 = NDEFRecord.readRecord(payload, offset)) != null)
      {
        if (num == 0 && !ndefRecord2.MessageBegin)
          return (NDEFSmartPosterParsedResult) null;
        string type = ndefRecord2.Type;
        if ("T".Equals(type))
          title = NDEFTextResultParser.decodeTextPayload(ndefRecord2.Payload)[1];
        else if ("U".Equals(type))
          uri = NDEFURIResultParser.decodeURIPayload(ndefRecord2.Payload);
        else if ("act".Equals(type))
          action = (int) ndefRecord2.Payload[0];
        ++num;
        offset += ndefRecord2.TotalRecordLength;
      }
      if (num == 0 || ndefRecord2 != null && !ndefRecord2.MessageEnd)
        return (NDEFSmartPosterParsedResult) null;
      return new NDEFSmartPosterParsedResult(action, uri, title);
    }
  }
}
