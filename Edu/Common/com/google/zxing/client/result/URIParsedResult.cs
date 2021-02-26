// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.URIParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Text;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class URIParsedResult : ParsedResult
  {
    private string uri;
    private string title;

    public string URI
    {
      get
      {
        return this.uri;
      }
    }

    public string Title
    {
      get
      {
        return this.title;
      }
    }

    /// <returns>
    /// true if the URI contains suspicious patterns that may suggest it intends to
    ///             mislead the user about its true nature. At the moment this looks for the presence
    ///             of user/password syntax in the host/authority portion of a URI which may be used
    ///             in attempts to make the URI's host appear to be other than it is. Example:
    ///             http://yourbank.com@phisher.com  This URI connects to phisher.com but may appear
    ///             to connect to yourbank.com at first glance.
    /// 
    /// </returns>
    public bool PossiblyMaliciousURI
    {
      get
      {
        return this.containsUser();
      }
    }

    public override string DisplayResult
    {
      get
      {
        StringBuilder result = new StringBuilder(30);
        ParsedResult.maybeAppend(this.title, result);
        ParsedResult.maybeAppend(this.uri, result);
        return result.ToString();
      }
    }

    public URIParsedResult(string uri, string title)
      : base(ParsedResultType.URI)
    {
      this.uri = URIParsedResult.massageURI(uri);
      this.title = title;
    }

    private bool containsUser()
    {
      int startIndex = this.uri.IndexOf(':') + 1;
      int length = this.uri.Length;
      while (startIndex < length && (int) this.uri[startIndex] == 47)
        ++startIndex;
      int num1 = this.uri.IndexOf('/', startIndex);
      if (num1 < 0)
        num1 = length;
      int num2 = this.uri.IndexOf('@', startIndex);
      if (num2 >= startIndex)
        return num2 < num1;
      return false;
    }

    /// <summary>
    /// Transforms a string that represents a URI into something more proper, by adding or canonicalizing
    ///             the protocol.
    /// 
    /// </summary>
    private static string massageURI(string uri)
    {
      int num = uri.IndexOf(':');
      uri = num >= 0 ? (!URIParsedResult.isColonFollowedByPortNumber(uri, num) ? uri.Substring(0, num).ToLower() + uri.Substring(num) : "http://" + uri) : "http://" + uri;
      return uri;
    }

    private static bool isColonFollowedByPortNumber(string uri, int protocolEnd)
    {
      int num = uri.IndexOf('/', protocolEnd + 1);
      if (num < 0)
        num = uri.Length;
      if (num <= protocolEnd + 1)
        return false;
      for (int index = protocolEnd + 1; index < num; ++index)
      {
        if ((int) uri[index] < 48 || (int) uri[index] > 57)
          return false;
      }
      return true;
    }
  }
}
