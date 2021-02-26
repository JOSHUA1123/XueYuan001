// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.URIResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Tries to parse results that are a URI of some kind.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class URIResultParser : ResultParser
  {
    private URIResultParser()
    {
    }

    public static URIParsedResult parse(Result result)
    {
      string uri = result.Text;
      if (uri != null && uri.StartsWith("URL:"))
        uri = uri.Substring(4);
      if (!URIResultParser.isBasicallyValidURI(uri))
        return (URIParsedResult) null;
      return new URIParsedResult(uri, (string) null);
    }

    /// <summary>
    /// Determines whether a string is not obviously not a URI. This implements crude checks; this class does not
    ///             intend to strictly check URIs as its only function is to represent what is in a barcode, but, it does
    ///             need to know when a string is obviously not a URI.
    /// 
    /// </summary>
    internal static bool isBasicallyValidURI(string uri)
    {
      if (uri == null || uri.IndexOf(' ') >= 0 || uri.IndexOf('\n') >= 0)
        return false;
      int num1 = uri.IndexOf('.');
      if (num1 >= uri.Length - 2)
        return false;
      int num2 = uri.IndexOf(':');
      if (num1 < 0 && num2 < 0)
        return false;
      if (num2 >= 0)
      {
        if (num1 < 0 || num1 > num2)
        {
          for (int index = 0; index < num2; ++index)
          {
            char ch = uri[index];
            if (((int) ch < 97 || (int) ch > 122) && ((int) ch < 65 || (int) ch > 90))
              return false;
          }
        }
        else
        {
          if (num2 >= uri.Length - 2)
            return false;
          for (int index = num2 + 1; index < num2 + 3; ++index)
          {
            char ch = uri[index];
            if ((int) ch < 48 || (int) ch > 57)
              return false;
          }
        }
      }
      return true;
    }
  }
}
