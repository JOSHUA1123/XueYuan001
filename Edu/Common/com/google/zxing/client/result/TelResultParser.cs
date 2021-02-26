// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.TelResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Parses a "tel:" URI result, which specifies a phone number.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class TelResultParser : ResultParser
  {
    private TelResultParser()
    {
    }

    public static TelParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("tel:") && !text.StartsWith("TEL:"))
        return (TelParsedResult) null;
      string telURI = text.StartsWith("TEL:") ? "tel:" + text.Substring(4) : text;
      int num = text.IndexOf('?', 4);
      return new TelParsedResult(num < 0 ? text.Substring(4) : text.Substring(4, num - 4), telURI, (string) null);
    }
  }
}
