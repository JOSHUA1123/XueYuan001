// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.optional.NDEFTextResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.client.result;

namespace com.google.zxing.client.result.optional
{
  /// <summary>
  /// Recognizes an NDEF message that encodes text according to the
  ///             "Text Record Type Definition" specification.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class NDEFTextResultParser : AbstractNDEFResultParser
  {
    public static TextParsedResult parse(Result result)
    {
      sbyte[] rawBytes = result.RawBytes;
      if (rawBytes == null)
        return (TextParsedResult) null;
      NDEFRecord ndefRecord = NDEFRecord.readRecord(rawBytes, 0);
      if (ndefRecord == null || !ndefRecord.MessageBegin || !ndefRecord.MessageEnd)
        return (TextParsedResult) null;
      if (!ndefRecord.Type.Equals("T"))
        return (TextParsedResult) null;
      string[] strArray = NDEFTextResultParser.decodeTextPayload(ndefRecord.Payload);
      return new TextParsedResult(strArray[0], strArray[1]);
    }

    internal static string[] decodeTextPayload(sbyte[] payload)
    {
      sbyte num = payload[0];
      bool flag = ((int) num & 128) != 0;
      int length = (int) num & 31;
      string str1 = AbstractNDEFResultParser.bytesToString(payload, 1, length, "US-ASCII");
      string encoding = flag ? "UTF-16" : "UTF-8";
      string str2 = AbstractNDEFResultParser.bytesToString(payload, 1 + length, payload.Length - length - 1, encoding);
      return new string[2]
      {
        str1,
        str2
      };
    }
  }
}
