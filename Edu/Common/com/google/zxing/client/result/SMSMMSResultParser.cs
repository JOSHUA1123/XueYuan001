// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.SMSMMSResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// <p>Parses an "sms:" URI result, which specifies a number to SMS and optional
  ///             "via" number. See <a href="http://gbiv.com/protocols/uri/drafts/draft-antti-gsm-sms-url-04.txt">the IETF draft</a> on this.</p><p>This actually also parses URIs starting with "mms:", "smsto:", "mmsto:", "SMSTO:", and
  ///             "MMSTO:", and treats them all the same way, and effectively converts them to an "sms:" URI
  ///             for purposes of forwarding to the platform.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class SMSMMSResultParser : ResultParser
  {
    private SMSMMSResultParser()
    {
    }

    public static SMSParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null)
        return (SMSParsedResult) null;
      int startIndex;
      if (text.StartsWith("sms:") || text.StartsWith("SMS:") || (text.StartsWith("mms:") || text.StartsWith("MMS:")))
      {
        startIndex = 4;
      }
      else
      {
        if (!text.StartsWith("smsto:") && !text.StartsWith("SMSTO:") && (!text.StartsWith("mmsto:") && !text.StartsWith("MMSTO:")))
          return (SMSParsedResult) null;
        startIndex = 6;
      }
      Hashtable hashtable = ResultParser.parseNameValuePairs(text);
      string subject = (string) null;
      string body = (string) null;
      bool flag = false;
      if (hashtable != null && hashtable.Count != 0)
      {
        subject = (string) hashtable[(object) "subject"];
        body = (string) hashtable[(object) "body"];
        flag = true;
      }
      int num = text.IndexOf('?', startIndex);
      string str1 = num < 0 || !flag ? text.Substring(startIndex) : text.Substring(startIndex, num - startIndex);
      int length1 = str1.IndexOf(';');
      string number;
      string via;
      if (length1 < 0)
      {
        number = str1;
        via = (string) null;
      }
      else
      {
        number = str1.Substring(0, length1);
        string str2 = str1.Substring(length1 + 1);
        via = !str2.StartsWith("via=") ? (string) null : str2.Substring(4);
      }
      if (body == null)
      {
        int length2 = number.IndexOf(':');
        if (length2 >= 0)
        {
          body = number.Substring(length2 + 1);
          number = number.Substring(0, length2);
        }
      }
      return new SMSParsedResult("sms:" + number, number, via, subject, body, (string) null);
    }
  }
}
