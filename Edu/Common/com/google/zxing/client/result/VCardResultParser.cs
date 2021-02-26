// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.VCardResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;
using System.Text;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Parses contact information formatted according to the VCard (2.1) format. This is not a complete
  ///             implementation but should parse information as commonly encoded in 2D barcodes.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class VCardResultParser : ResultParser
  {
    private VCardResultParser()
    {
    }

    public static AddressBookParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("BEGIN:VCARD"))
        return (AddressBookParsedResult) null;
      string[] names = VCardResultParser.matchVCardPrefixedField("FN", text, true);
      if (names == null)
      {
        names = VCardResultParser.matchVCardPrefixedField("N", text, true);
        VCardResultParser.formatNames(names);
      }
      string[] phoneNumbers = VCardResultParser.matchVCardPrefixedField("TEL", text, true);
      string[] emails = VCardResultParser.matchVCardPrefixedField("EMAIL", text, true);
      string note = VCardResultParser.matchSingleVCardPrefixedField("NOTE", text, false);
      string[] addresses = VCardResultParser.matchVCardPrefixedField("ADR", text, true);
      if (addresses != null)
      {
        for (int index = 0; index < addresses.Length; ++index)
          addresses[index] = VCardResultParser.formatAddress(addresses[index]);
      }
      string org = VCardResultParser.matchSingleVCardPrefixedField("ORG", text, true);
      string str = VCardResultParser.matchSingleVCardPrefixedField("BDAY", text, true);
      if (!VCardResultParser.isLikeVCardDate(str))
        str = (string) null;
      string title = VCardResultParser.matchSingleVCardPrefixedField("TITLE", text, true);
      string url = VCardResultParser.matchSingleVCardPrefixedField("URL", text, true);
      return new AddressBookParsedResult(names, (string) null, phoneNumbers, emails, note, addresses, org, str, title, url);
    }

    private static string[] matchVCardPrefixedField(string prefix, string rawText, bool trim)
    {
      ArrayList strings = (ArrayList) null;
      int startIndex1 = 0;
      int length = rawText.Length;
      while (startIndex1 < length)
      {
        int num1 = rawText.IndexOf(prefix, startIndex1);
        if (num1 >= 0)
        {
          if (num1 > 0 && (int) rawText[num1 - 1] != 10)
          {
            startIndex1 = num1 + 1;
          }
          else
          {
            startIndex1 = num1 + prefix.Length;
            if ((int) rawText[startIndex1] == 58 || (int) rawText[startIndex1] == 59)
            {
              while ((int) rawText[startIndex1] != 58)
                ++startIndex1;
              int startIndex2 = startIndex1 + 1;
              int startIndex3 = startIndex2;
              int num2 = rawText.IndexOf('\n', startIndex2);
              if (num2 < 0)
                startIndex1 = length;
              else if (num2 > startIndex3)
              {
                if (strings == null)
                  strings = ArrayList.Synchronized(new ArrayList(3));
                string str = rawText.Substring(startIndex3, num2 - startIndex3);
                if (trim)
                  str = str.Trim();
                strings.Add((object) str);
                startIndex1 = num2 + 1;
              }
              else
                startIndex1 = num2 + 1;
            }
          }
        }
        else
          break;
      }
      if (strings == null || strings.Count == 0)
        return (string[]) null;
      return ResultParser.toStringArray(strings);
    }

    internal static string matchSingleVCardPrefixedField(string prefix, string rawText, bool trim)
    {
      string[] strArray = VCardResultParser.matchVCardPrefixedField(prefix, rawText, trim);
      if (strArray != null)
        return strArray[0];
      return (string) null;
    }

    private static bool isLikeVCardDate(string value_Renamed)
    {
      if (value_Renamed == null || ResultParser.isStringOfDigits(value_Renamed, 8))
        return true;
      if (value_Renamed.Length == 10 && (int) value_Renamed[4] == 45 && ((int) value_Renamed[7] == 45 && ResultParser.isSubstringOfDigits(value_Renamed, 0, 4)) && ResultParser.isSubstringOfDigits(value_Renamed, 5, 2))
        return ResultParser.isSubstringOfDigits(value_Renamed, 8, 2);
      return false;
    }

    private static string formatAddress(string address)
    {
      if (address == null)
        return (string) null;
      int length = address.Length;
      StringBuilder stringBuilder = new StringBuilder(length);
      for (int index = 0; index < length; ++index)
      {
        char ch = address[index];
        if ((int) ch == 59)
          stringBuilder.Append(' ');
        else
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString().Trim();
    }

    /// <summary>
    /// Formats name fields of the form "Public;John;Q.;Reverend;III" into a form like
    ///             "Reverend John Q. Public III".
    /// 
    /// 
    /// </summary>
    /// <param name="names">name values to format, in place
    ///             </param>
    private static void formatNames(string[] names)
    {
      if (names == null)
        return;
      for (int index1 = 0; index1 < names.Length; ++index1)
      {
        string str = names[index1];
        string[] components = new string[5];
        int startIndex = 0;
        int index2 = 0;
        int num;
        for (; (num = str.IndexOf(';', startIndex)) > 0; startIndex = num + 1)
        {
          components[index2] = str.Substring(startIndex, num - startIndex);
          ++index2;
        }
        components[index2] = str.Substring(startIndex);
        StringBuilder newName = new StringBuilder(100);
        VCardResultParser.maybeAppendComponent(components, 3, newName);
        VCardResultParser.maybeAppendComponent(components, 1, newName);
        VCardResultParser.maybeAppendComponent(components, 2, newName);
        VCardResultParser.maybeAppendComponent(components, 0, newName);
        VCardResultParser.maybeAppendComponent(components, 4, newName);
        names[index1] = newName.ToString().Trim();
      }
    }

    private static void maybeAppendComponent(string[] components, int i, StringBuilder newName)
    {
      if (components[i] == null)
        return;
      newName.Append(' ');
      newName.Append(components[i]);
    }
  }
}
