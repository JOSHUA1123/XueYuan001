// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.SMSParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Text;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class SMSParsedResult : ParsedResult
  {
    private string smsURI;
    private string number;
    private string via;
    private string subject;
    private string body;
    private string title;

    public string SMSURI
    {
      get
      {
        return this.smsURI;
      }
    }

    public string Number
    {
      get
      {
        return this.number;
      }
    }

    public string Via
    {
      get
      {
        return this.via;
      }
    }

    public string Subject
    {
      get
      {
        return this.subject;
      }
    }

    public string Body
    {
      get
      {
        return this.body;
      }
    }

    public string Title
    {
      get
      {
        return this.title;
      }
    }

    public override string DisplayResult
    {
      get
      {
        StringBuilder result = new StringBuilder(100);
        ParsedResult.maybeAppend(this.number, result);
        ParsedResult.maybeAppend(this.via, result);
        ParsedResult.maybeAppend(this.subject, result);
        ParsedResult.maybeAppend(this.body, result);
        ParsedResult.maybeAppend(this.title, result);
        return result.ToString();
      }
    }

    public SMSParsedResult(string smsURI, string number, string via, string subject, string body, string title)
      : base(ParsedResultType.SMS)
    {
      this.smsURI = smsURI;
      this.number = number;
      this.via = via;
      this.subject = subject;
      this.body = body;
      this.title = title;
    }
  }
}
