// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.optional.NDEFURIResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.client.result;

namespace com.google.zxing.client.result.optional
{
  /// <summary>
  /// Recognizes an NDEF message that encodes a URI according to the
  ///             "URI Record Type Definition" specification.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class NDEFURIResultParser : AbstractNDEFResultParser
  {
    private static readonly string[] URI_PREFIXES = new string[36]
    {
      null,
      "http://www.",
      "https://www.",
      "http://",
      "https://",
      "tel:",
      "mailto:",
      "ftp://anonymous:anonymous@",
      "ftp://ftp.",
      "ftps://",
      "sftp://",
      "smb://",
      "nfs://",
      "ftp://",
      "dav://",
      "news:",
      "telnet://",
      "imap:",
      "rtsp://",
      "urn:",
      "pop:",
      "sip:",
      "sips:",
      "tftp:",
      "btspp://",
      "btl2cap://",
      "btgoep://",
      "tcpobex://",
      "irdaobex://",
      "file://",
      "urn:epc:id:",
      "urn:epc:tag:",
      "urn:epc:pat:",
      "urn:epc:raw:",
      "urn:epc:",
      "urn:nfc:"
    };

    public static URIParsedResult parse(Result result)
    {
      sbyte[] rawBytes = result.RawBytes;
      if (rawBytes == null)
        return (URIParsedResult) null;
      NDEFRecord ndefRecord = NDEFRecord.readRecord(rawBytes, 0);
      if (ndefRecord == null || !ndefRecord.MessageBegin || !ndefRecord.MessageEnd)
        return (URIParsedResult) null;
      if (!ndefRecord.Type.Equals("U"))
        return (URIParsedResult) null;
      return new URIParsedResult(NDEFURIResultParser.decodeURIPayload(ndefRecord.Payload), (string) null);
    }

    internal static string decodeURIPayload(sbyte[] payload)
    {
      int index = (int) payload[0] & (int) byte.MaxValue;
      string str1 = (string) null;
      if (index < NDEFURIResultParser.URI_PREFIXES.Length)
        str1 = NDEFURIResultParser.URI_PREFIXES[index];
      string str2 = AbstractNDEFResultParser.bytesToString(payload, 1, payload.Length - 1, "UTF-8");
      if (str1 != null)
        return str1 + str2;
      return str2;
    }
  }
}
