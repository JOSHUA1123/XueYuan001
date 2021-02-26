// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.BookmarkDoCoMoResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class BookmarkDoCoMoResultParser : AbstractDoCoMoResultParser
  {
    private BookmarkDoCoMoResultParser()
    {
    }

    public static URIParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("MEBKM:"))
        return (URIParsedResult) null;
      string title = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("TITLE:", text, true);
      string[] strArray = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("URL:", text, true);
      if (strArray == null)
        return (URIParsedResult) null;
      string uri = strArray[0];
      if (!URIResultParser.isBasicallyValidURI(uri))
        return (URIParsedResult) null;
      return new URIParsedResult(uri, title);
    }
  }
}
