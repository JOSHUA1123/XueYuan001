// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.AbstractDoCoMoResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing.client.result
{
  /// <summary>
  /// <p>See
  ///             <a href="http://www.nttdocomo.co.jp/english/service/imode/make/content/barcode/about/s2.html">DoCoMo's documentation</a> about the result types represented by subclasses of this class.</p><p>Thanks to Jeff Griffin for proposing rewrite of these classes that relies less
  ///             on exception-based mechanisms during parsing.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal abstract class AbstractDoCoMoResultParser : ResultParser
  {
    internal static string[] matchDoCoMoPrefixedField(string prefix, string rawText, bool trim)
    {
      return ResultParser.matchPrefixedField(prefix, rawText, ';', trim);
    }

    internal static string matchSingleDoCoMoPrefixedField(string prefix, string rawText, bool trim)
    {
      return ResultParser.matchSinglePrefixedField(prefix, rawText, ';', trim);
    }
  }
}
