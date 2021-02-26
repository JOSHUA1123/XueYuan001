// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.ResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;
using System.Text;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// <p>Abstract class representing the result of decoding a barcode, as more than
  ///             a String -- as some type of structured data. This might be a subclass which represents
  ///             a URL, or an e-mail address. {@link #parseResult(com.google.zxing.Result)} will turn a raw
  ///             decoded string into the most appropriate type of structured representation.</p><p>Thanks to Jeff Griffin for proposing rewrite of these classes that relies less
  ///             on exception-based mechanisms during parsing.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class ResultParser
  {
    public static ParsedResult parseResult(Result theResult)
    {
      return (ParsedResult) BookmarkDoCoMoResultParser.parse(theResult) ?? (ParsedResult) AddressBookDoCoMoResultParser.parse(theResult) ?? (ParsedResult) EmailDoCoMoResultParser.parse(theResult) ?? (ParsedResult) AddressBookAUResultParser.parse(theResult) ?? (ParsedResult) VCardResultParser.parse(theResult) ?? (ParsedResult) BizcardResultParser.parse(theResult) ?? (ParsedResult) VEventResultParser.parse(theResult) ?? (ParsedResult) EmailAddressResultParser.parse(theResult) ?? (ParsedResult) TelResultParser.parse(theResult) ?? (ParsedResult) SMSMMSResultParser.parse(theResult) ?? (ParsedResult) GeoResultParser.parse(theResult) ?? (ParsedResult) URLTOResultParser.parse(theResult) ?? (ParsedResult) URIResultParser.parse(theResult) ?? (ParsedResult) ISBNResultParser.parse(theResult) ?? (ParsedResult) ProductResultParser.parse(theResult) ?? (ParsedResult) new TextParsedResult(theResult.Text, (string) null);
    }

    protected internal static void maybeAppend(string value_Renamed, StringBuilder result)
    {
      if (value_Renamed == null)
        return;
      result.Append('\n');
      result.Append(value_Renamed);
    }

    protected internal static void maybeAppend(string[] value_Renamed, StringBuilder result)
    {
      if (value_Renamed == null)
        return;
      for (int index = 0; index < value_Renamed.Length; ++index)
      {
        result.Append('\n');
        result.Append(value_Renamed[index]);
      }
    }

    protected internal static string[] maybeWrap(string value_Renamed)
    {
      if (value_Renamed == null)
        return (string[]) null;
      return new string[1]
      {
        value_Renamed
      };
    }

    protected internal static string unescapeBackslash(string escaped)
    {
      if (escaped != null)
      {
        int charCount = escaped.IndexOf('\\');
        if (charCount >= 0)
        {
          int length = escaped.Length;
          StringBuilder stringBuilder = new StringBuilder(length - 1);
          stringBuilder.Append(escaped.ToCharArray(), 0, charCount);
          bool flag = false;
          for (int index = charCount; index < length; ++index)
          {
            char ch = escaped[index];
            if (flag || (int) ch != 92)
            {
              stringBuilder.Append(ch);
              flag = false;
            }
            else
              flag = true;
          }
          return stringBuilder.ToString();
        }
      }
      return escaped;
    }

    private static string urlDecode(string escaped)
    {
      if (escaped == null)
        return (string) null;
      char[] escapedArray = escaped.ToCharArray();
      int firstEscape = ResultParser.findFirstEscape(escapedArray);
      if (firstEscape < 0)
        return escaped;
      int length = escapedArray.Length;
      StringBuilder stringBuilder = new StringBuilder(length - 2);
      stringBuilder.Append(escapedArray, 0, firstEscape);
      for (int index = firstEscape; index < length; ++index)
      {
        char ch = escapedArray[index];
        switch (ch)
        {
          case '+':
            stringBuilder.Append(' ');
            break;
          case '%':
            if (index >= length - 2)
            {
              stringBuilder.Append('%');
              break;
            }
            int num1;
            int num2 = ResultParser.parseHexDigit(escapedArray[num1 = index + 1]);
            int num3 = ResultParser.parseHexDigit(escapedArray[index = num1 + 1]);
            if (num2 < 0 || num3 < 0)
            {
              stringBuilder.Append('%');
              stringBuilder.Append(escapedArray[index - 1]);
              stringBuilder.Append(escapedArray[index]);
            }
            stringBuilder.Append((char) ((num2 << 4) + num3));
            break;
          default:
            stringBuilder.Append(ch);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    private static int findFirstEscape(char[] escapedArray)
    {
      int length = escapedArray.Length;
      for (int index = 0; index < length; ++index)
      {
        switch (escapedArray[index])
        {
          case '+':
          case '%':
            return index;
          default:
            goto default;
        }
      }
      return -1;
    }

    private static int parseHexDigit(char c)
    {
      if ((int) c >= 97)
      {
        if ((int) c <= 102)
          return 10 + ((int) c - 97);
      }
      else if ((int) c >= 65)
      {
        if ((int) c <= 70)
          return 10 + ((int) c - 65);
      }
      else if ((int) c >= 48 && (int) c <= 57)
        return (int) c - 48;
      return -1;
    }

    protected internal static bool isStringOfDigits(string value_Renamed, int length)
    {
      if (value_Renamed == null)
        return false;
      int length1 = value_Renamed.Length;
      if (length != length1)
        return false;
      for (int index = 0; index < length; ++index)
      {
        char ch = value_Renamed[index];
        if ((int) ch < 48 || (int) ch > 57)
          return false;
      }
      return true;
    }

    protected internal static bool isSubstringOfDigits(string value_Renamed, int offset, int length)
    {
      if (value_Renamed == null)
        return false;
      int length1 = value_Renamed.Length;
      int num = offset + length;
      if (length1 < num)
        return false;
      for (int index = offset; index < num; ++index)
      {
        char ch = value_Renamed[index];
        if ((int) ch < 48 || (int) ch > 57)
          return false;
      }
      return true;
    }

    internal static Hashtable parseNameValuePairs(string uri)
    {
      int num1 = uri.IndexOf('?');
      if (num1 < 0)
        return (Hashtable) null;
      Hashtable result = Hashtable.Synchronized(new Hashtable(3));
      int num2;
      int paramEnd;
      for (num2 = num1 + 1; (paramEnd = uri.IndexOf('&', num2)) >= 0; num2 = paramEnd + 1)
        ResultParser.appendKeyValue(uri, num2, paramEnd, result);
      ResultParser.appendKeyValue(uri, num2, uri.Length, result);
      return result;
    }

    private static void appendKeyValue(string uri, int paramStart, int paramEnd, Hashtable result)
    {
      int num = uri.IndexOf('=', paramStart);
      if (num < 0)
        return;
      string str1 = uri.Substring(paramStart, num - paramStart);
      string str2 = ResultParser.urlDecode(uri.Substring(num + 1, paramEnd - (num + 1)));
      result[(object) str1] = (object) str2;
    }

    internal static string[] matchPrefixedField(string prefix, string rawText, char endChar, bool trim)
    {
      ArrayList strings = (ArrayList) null;
      int startIndex1 = 0;
      int length = rawText.Length;
      while (startIndex1 < length)
      {
        int num1 = rawText.IndexOf(prefix, startIndex1);
        if (num1 >= 0)
        {
          startIndex1 = num1 + prefix.Length;
          int startIndex2 = startIndex1;
          bool flag = false;
          while (!flag)
          {
            int num2 = rawText.IndexOf(endChar, startIndex1);
            if (num2 < 0)
            {
              startIndex1 = rawText.Length;
              flag = true;
            }
            else if ((int) rawText[num2 - 1] == 92)
            {
              startIndex1 = num2 + 1;
            }
            else
            {
              if (strings == null)
                strings = ArrayList.Synchronized(new ArrayList(3));
              string str = ResultParser.unescapeBackslash(rawText.Substring(startIndex2, num2 - startIndex2));
              if (trim)
                str = str.Trim();
              strings.Add((object) str);
              startIndex1 = num2 + 1;
              flag = true;
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

    internal static string matchSinglePrefixedField(string prefix, string rawText, char endChar, bool trim)
    {
      string[] strArray = ResultParser.matchPrefixedField(prefix, rawText, endChar, trim);
      if (strArray != null)
        return strArray[0];
      return (string) null;
    }

    internal static string[] toStringArray(ArrayList strings)
    {
      int count = strings.Count;
      string[] strArray = new string[count];
      for (int index = 0; index < count; ++index)
        strArray[index] = (string) strings[index];
      return strArray;
    }
  }
}
