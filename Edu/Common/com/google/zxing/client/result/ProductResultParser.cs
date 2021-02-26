// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.ProductResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.oned;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Parses strings of digits that represent a UPC code.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class ProductResultParser : ResultParser
  {
    private ProductResultParser()
    {
    }

    public static ProductParsedResult parse(Result result)
    {
      BarcodeFormat barcodeFormat = result.BarcodeFormat;
      if (!BarcodeFormat.UPC_A.Equals((object) barcodeFormat) && !BarcodeFormat.UPC_E.Equals((object) barcodeFormat) && (!BarcodeFormat.EAN_8.Equals((object) barcodeFormat) && !BarcodeFormat.EAN_13.Equals((object) barcodeFormat)))
        return (ProductParsedResult) null;
      string text = result.Text;
      if (text == null)
        return (ProductParsedResult) null;
      int length = text.Length;
      for (int index = 0; index < length; ++index)
      {
        char ch = text[index];
        if ((int) ch < 48 || (int) ch > 57)
          return (ProductParsedResult) null;
      }
      string normalizedProductID = !BarcodeFormat.UPC_E.Equals((object) barcodeFormat) ? text : UPCEReader.convertUPCEtoUPCA(text);
      return new ProductParsedResult(text, normalizedProductID);
    }
  }
}
