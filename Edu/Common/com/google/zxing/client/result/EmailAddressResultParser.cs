// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.EmailAddressResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Represents a result that encodes an e-mail address, either as a plain address
  ///             like "joe@example.org" or a mailto: URL like "mailto:joe@example.org".
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class EmailAddressResultParser : ResultParser
  {
    public static EmailAddressParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null)
        return (EmailAddressParsedResult) null;
      if (text.StartsWith("mailto:") || text.StartsWith("MAILTO:"))
      {
        string emailAddress = text.Substring(7);
        int length = emailAddress.IndexOf('?');
        if (length >= 0)
          emailAddress = emailAddress.Substring(0, length);
        Hashtable hashtable = ResultParser.parseNameValuePairs(text);
        string subject = (string) null;
        string body = (string) null;
        if (hashtable != null)
        {
          if (emailAddress.Length == 0)
            emailAddress = (string) hashtable[(object) "to"];
          subject = (string) hashtable[(object) "subject"];
          body = (string) hashtable[(object) "body"];
        }
        return new EmailAddressParsedResult(emailAddress, subject, body, text);
      }
      if (!EmailDoCoMoResultParser.isBasicallyValidEmailAddress(text))
        return (EmailAddressParsedResult) null;
      string emailAddress1 = text;
      return new EmailAddressParsedResult(emailAddress1, (string) null, (string) null, "mailto:" + emailAddress1);
    }
  }
}
