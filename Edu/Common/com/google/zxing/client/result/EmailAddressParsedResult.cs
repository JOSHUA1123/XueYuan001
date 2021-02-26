// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.EmailAddressParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Text;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class EmailAddressParsedResult : ParsedResult
  {
    private string emailAddress;
    private string subject;
    private string body;
    private string mailtoURI;

    public string EmailAddress
    {
      get
      {
        return this.emailAddress;
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

    public string MailtoURI
    {
      get
      {
        return this.mailtoURI;
      }
    }

    public override string DisplayResult
    {
      get
      {
        StringBuilder result = new StringBuilder(30);
        ParsedResult.maybeAppend(this.emailAddress, result);
        ParsedResult.maybeAppend(this.subject, result);
        ParsedResult.maybeAppend(this.body, result);
        return result.ToString();
      }
    }

    internal EmailAddressParsedResult(string emailAddress, string subject, string body, string mailtoURI)
      : base(ParsedResultType.EMAIL_ADDRESS)
    {
      this.emailAddress = emailAddress;
      this.subject = subject;
      this.body = body;
      this.mailtoURI = mailtoURI;
    }
  }
}
