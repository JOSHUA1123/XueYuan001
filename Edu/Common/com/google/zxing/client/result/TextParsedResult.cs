// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.TextParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing.client.result
{
  /// <summary>
  /// A simple result type encapsulating a string that has no further
  ///             interpretation.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class TextParsedResult : ParsedResult
  {
    private string text;
    private string language;

    public string Text
    {
      get
      {
        return this.text;
      }
    }

    public string Language
    {
      get
      {
        return this.language;
      }
    }

    public override string DisplayResult
    {
      get
      {
        return this.text;
      }
    }

    public TextParsedResult(string text, string language)
      : base(ParsedResultType.TEXT)
    {
      this.text = text;
      this.language = language;
    }
  }
}
