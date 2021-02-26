// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.optional.NDEFSmartPosterParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.client.result;

namespace com.google.zxing.client.result.optional
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class NDEFSmartPosterParsedResult : ParsedResult
  {
    public const int ACTION_UNSPECIFIED = -1;
    public const int ACTION_DO = 0;
    public const int ACTION_SAVE = 1;
    public const int ACTION_OPEN = 2;
    private string title;
    private string uri;
    private int action;

    public string Title
    {
      get
      {
        return this.title;
      }
    }

    public string URI
    {
      get
      {
        return this.uri;
      }
    }

    public int Action
    {
      get
      {
        return this.action;
      }
    }

    public override string DisplayResult
    {
      get
      {
        if (this.title == null)
          return this.uri;
        return this.title + (object) '\n' + this.uri;
      }
    }

    internal NDEFSmartPosterParsedResult(int action, string uri, string title)
      : base(ParsedResultType.NDEF_SMART_POSTER)
    {
      this.action = action;
      this.uri = uri;
      this.title = title;
    }
  }
}
