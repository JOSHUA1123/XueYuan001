// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.ISBNParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing.client.result
{
  /// <author>jbreiden@google.com (Jeff Breidenbach)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ISBNParsedResult : ParsedResult
  {
    private string isbn;

    public string ISBN
    {
      get
      {
        return this.isbn;
      }
    }

    public override string DisplayResult
    {
      get
      {
        return this.isbn;
      }
    }

    internal ISBNParsedResult(string isbn)
      : base(ParsedResultType.ISBN)
    {
      this.isbn = isbn;
    }
  }
}
