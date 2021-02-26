// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.URLTOResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Parses the "URLTO" result format, which is of the form "URLTO:[title]:[url]".
  ///             This seems to be used sometimes, but I am not able to find documentation
  ///             on its origin or official format?
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class URLTOResultParser
  {
    private URLTOResultParser()
    {
    }

    public static URIParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("urlto:") && !text.StartsWith("URLTO:"))
        return (URIParsedResult) null;
      int num = text.IndexOf(':', 6);
      if (num < 0)
        return (URIParsedResult) null;
      string title = num <= 6 ? (string) null : text.Substring(6, num - 6);
      return new URIParsedResult(text.Substring(num + 1), title);
    }
  }
}
