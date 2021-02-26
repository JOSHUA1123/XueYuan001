// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.TelParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Text;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class TelParsedResult : ParsedResult
  {
    private string number;
    private string telURI;
    private string title;

    public string Number
    {
      get
      {
        return this.number;
      }
    }

    public string TelURI
    {
      get
      {
        return this.telURI;
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
        StringBuilder result = new StringBuilder(20);
        ParsedResult.maybeAppend(this.number, result);
        ParsedResult.maybeAppend(this.title, result);
        return result.ToString();
      }
    }

    public TelParsedResult(string number, string telURI, string title)
      : base(ParsedResultType.TEL)
    {
      this.number = number;
      this.telURI = telURI;
      this.title = title;
    }
  }
}
