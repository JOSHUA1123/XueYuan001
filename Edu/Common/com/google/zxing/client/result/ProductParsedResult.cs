// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.ProductParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing.client.result
{
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ProductParsedResult : ParsedResult
  {
    private string productID;
    private string normalizedProductID;

    public string ProductID
    {
      get
      {
        return this.productID;
      }
    }

    public string NormalizedProductID
    {
      get
      {
        return this.normalizedProductID;
      }
    }

    public override string DisplayResult
    {
      get
      {
        return this.productID;
      }
    }

    internal ProductParsedResult(string productID)
      : this(productID, productID)
    {
    }

    internal ProductParsedResult(string productID, string normalizedProductID)
      : base(ParsedResultType.PRODUCT)
    {
      this.productID = productID;
      this.normalizedProductID = normalizedProductID;
    }
  }
}
