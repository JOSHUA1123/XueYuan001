// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.EmailDoCoMoResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Implements the "MATMSG" email message entry format.
  /// 
  ///             Supported keys: TO, SUB, BODY
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class EmailDoCoMoResultParser : AbstractDoCoMoResultParser
  {
    private static readonly char[] ATEXT_SYMBOLS = new char[21]
    {
      '@',
      '.',
      '!',
      '#',
      '$',
      '%',
      '&',
      '\'',
      '*',
      '+',
      '-',
      '/',
      '=',
      '?',
      '^',
      '_',
      '`',
      '{',
      '|',
      '}',
      '~'
    };

    public static EmailAddressParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("MATMSG:"))
        return (EmailAddressParsedResult) null;
      string[] strArray = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("TO:", text, true);
      if (strArray == null)
        return (EmailAddressParsedResult) null;
      string str = strArray[0];
      if (!EmailDoCoMoResultParser.isBasicallyValidEmailAddress(str))
        return (EmailAddressParsedResult) null;
      string subject = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("SUB:", text, false);
      string body = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("BODY:", text, false);
      return new EmailAddressParsedResult(str, subject, body, "mailto:" + str);
    }

    /// <summary>
    /// This implements only the most basic checking for an email address's validity -- that it contains
    ///             an '@' contains no characters disallowed by RFC 2822. This is an overly lenient definition of
    ///             validity. We want to generally be lenient here since this class is only intended to encapsulate what's
    ///             in a barcode, not "judge" it.
    /// 
    /// </summary>
    internal static bool isBasicallyValidEmailAddress(string email)
    {
      if (email == null)
        return false;
      bool flag = false;
      for (int index = 0; index < email.Length; ++index)
      {
        char c = email[index];
        if (((int) c < 97 || (int) c > 122) && ((int) c < 65 || (int) c > 90) && (((int) c < 48 || (int) c > 57) && !EmailDoCoMoResultParser.isAtextSymbol(c)))
          return false;
        if ((int) c == 64)
        {
          if (flag)
            return false;
          flag = true;
        }
      }
      return flag;
    }

    private static bool isAtextSymbol(char c)
    {
      for (int index = 0; index < EmailDoCoMoResultParser.ATEXT_SYMBOLS.Length; ++index)
      {
        if ((int) c == (int) EmailDoCoMoResultParser.ATEXT_SYMBOLS[index])
          return true;
      }
      return false;
    }
  }
}
