// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.AddressBookAUResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Implements KDDI AU's address book format. See
  ///             <a href="http://www.au.kddi.com/ezfactory/tec/two_dimensions/index.html">http://www.au.kddi.com/ezfactory/tec/two_dimensions/index.html</a>.
  ///             (Thanks to Yuzo for translating!)
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class AddressBookAUResultParser : ResultParser
  {
    public static AddressBookParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || text.IndexOf("MEMORY") < 0 || text.IndexOf("\r\n") < 0)
        return (AddressBookParsedResult) null;
      string value_Renamed = ResultParser.matchSinglePrefixedField("NAME1:", text, '\r', true);
      string pronunciation = ResultParser.matchSinglePrefixedField("NAME2:", text, '\r', true);
      string[] phoneNumbers = AddressBookAUResultParser.matchMultipleValuePrefix("TEL", 3, text, true);
      string[] emails = AddressBookAUResultParser.matchMultipleValuePrefix("MAIL", 3, text, true);
      string note = ResultParser.matchSinglePrefixedField("MEMORY:", text, '\r', false);
      string str = ResultParser.matchSinglePrefixedField("ADD:", text, '\r', true);
      string[] strArray;
      if (str != null)
        strArray = new string[1]
        {
          str
        };
      else
        strArray = (string[]) null;
      string[] addresses = strArray;
      return new AddressBookParsedResult(ResultParser.maybeWrap(value_Renamed), pronunciation, phoneNumbers, emails, note, addresses, (string) null, (string) null, (string) null, (string) null);
    }

    private static string[] matchMultipleValuePrefix(string prefix, int max, string rawText, bool trim)
    {
      ArrayList strings = (ArrayList) null;
      for (int index = 1; index <= max; ++index)
      {
        string str = ResultParser.matchSinglePrefixedField(prefix + (object) index + (string) (object) ':', rawText, '\r', trim);
        if (str != null)
        {
          if (strings == null)
            strings = ArrayList.Synchronized(new ArrayList(max));
          strings.Add((object) str);
        }
        else
          break;
      }
      if (strings == null)
        return (string[]) null;
      return ResultParser.toStringArray(strings);
    }
  }
}
