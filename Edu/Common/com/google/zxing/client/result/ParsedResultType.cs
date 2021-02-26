// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.ParsedResultType
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Represents the type of data encoded by a barcode -- from plain text, to a
  ///             URI, to an e-mail address, etc.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ParsedResultType
  {
    public static readonly ParsedResultType ADDRESSBOOK = new ParsedResultType("ADDRESSBOOK");
    public static readonly ParsedResultType EMAIL_ADDRESS = new ParsedResultType("EMAIL_ADDRESS");
    public static readonly ParsedResultType PRODUCT = new ParsedResultType("PRODUCT");
    public static readonly ParsedResultType URI = new ParsedResultType("URI");
    public static readonly ParsedResultType TEXT = new ParsedResultType("TEXT");
    public static readonly ParsedResultType ANDROID_INTENT = new ParsedResultType("ANDROID_INTENT");
    public static readonly ParsedResultType GEO = new ParsedResultType("GEO");
    public static readonly ParsedResultType TEL = new ParsedResultType("TEL");
    public static readonly ParsedResultType SMS = new ParsedResultType("SMS");
    public static readonly ParsedResultType CALENDAR = new ParsedResultType("CALENDAR");
    public static readonly ParsedResultType NDEF_SMART_POSTER = new ParsedResultType("NDEF_SMART_POSTER");
    public static readonly ParsedResultType MOBILETAG_RICH_WEB = new ParsedResultType("MOBILETAG_RICH_WEB");
    public static readonly ParsedResultType ISBN = new ParsedResultType("ISBN");
    private string name;

    private ParsedResultType(string name)
    {
      this.name = name;
    }

    public override string ToString()
    {
      return this.name;
    }
  }
}
