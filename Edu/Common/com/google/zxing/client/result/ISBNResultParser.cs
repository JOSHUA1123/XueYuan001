// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.ISBNResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Parses strings of digits that represent a ISBN.
  /// 
  /// 
  /// </summary>
  /// <author>jbreiden@google.com (Jeff Breidenbach)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public class ISBNResultParser : ResultParser
  {
    private ISBNResultParser()
    {
    }

    public static ISBNParsedResult parse(Result result)
    {
      BarcodeFormat barcodeFormat = result.BarcodeFormat;
      if (!BarcodeFormat.EAN_13.Equals((object) barcodeFormat))
        return (ISBNParsedResult) null;
      string text = result.Text;
      if (text == null)
        return (ISBNParsedResult) null;
      if (text.Length != 13)
        return (ISBNParsedResult) null;
      if (!text.StartsWith("978") && !text.StartsWith("979"))
        return (ISBNParsedResult) null;
      return new ISBNParsedResult(text);
    }
  }
}
