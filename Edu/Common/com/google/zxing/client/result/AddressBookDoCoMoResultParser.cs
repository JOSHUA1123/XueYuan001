// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.AddressBookDoCoMoResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Implements the "MECARD" address book entry format.
  /// 
  ///             Supported keys: N, SOUND, TEL, EMAIL, NOTE, ADR, BDAY, URL, plus ORG
  ///             Unsupported keys: TEL-AV, NICKNAME
  /// 
  ///             Except for TEL, multiple values for keys are also not supported;
  ///             the first one found takes precedence.
  /// 
  ///             Our understanding of the MECARD format is based on this document:
  /// 
  ///             http://www.mobicode.org.tw/files/OMIA%20Mobile%20Bar%20Code%20Standard%20v3.2.1.doc
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class AddressBookDoCoMoResultParser : AbstractDoCoMoResultParser
  {
    public static AddressBookParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("MECARD:"))
        return (AddressBookParsedResult) null;
      string[] strArray = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("N:", text, true);
      if (strArray == null)
        return (AddressBookParsedResult) null;
      string value_Renamed = AddressBookDoCoMoResultParser.parseName(strArray[0]);
      string pronunciation = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("SOUND:", text, true);
      string[] phoneNumbers = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("TEL:", text, true);
      string[] emails = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("EMAIL:", text, true);
      string note = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("NOTE:", text, false);
      string[] addresses = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("ADR:", text, true);
      string str = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("BDAY:", text, true);
      if (str != null && !ResultParser.isStringOfDigits(str, 8))
        str = (string) null;
      string url = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("URL:", text, true);
      string org = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("ORG:", text, true);
      return new AddressBookParsedResult(ResultParser.maybeWrap(value_Renamed), pronunciation, phoneNumbers, emails, note, addresses, org, str, (string) null, url);
    }

    private static string parseName(string name)
    {
      int length = name.IndexOf(',');
      if (length >= 0)
        return name.Substring(length + 1) + (object) ' ' + name.Substring(0, length);
      return name;
    }
  }
}
