// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.ParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Text;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// <p>Abstract class representing the result of decoding a barcode, as more than
  ///             a String -- as some type of structured data. This might be a subclass which represents
  ///             a URL, or an e-mail address. {@link ResultParser#parseResult(Result)} will turn a raw
  ///             decoded string into the most appropriate type of structured representation.</p><p>Thanks to Jeff Griffin for proposing rewrite of these classes that relies less
  ///             on exception-based mechanisms during parsing.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class ParsedResult
  {
    private ParsedResultType type;

    public virtual ParsedResultType Type
    {
      get
      {
        return this.type;
      }
    }

    public abstract string DisplayResult { get; }

    protected internal ParsedResult(ParsedResultType type)
    {
      this.type = type;
    }

    public override string ToString()
    {
      return this.DisplayResult;
    }

    public static void maybeAppend(string value_Renamed, StringBuilder result)
    {
      if (value_Renamed == null || value_Renamed.Length <= 0)
        return;
      if (result.Length > 0)
        result.Append('\n');
      result.Append(value_Renamed);
    }

    public static void maybeAppend(string[] value_Renamed, StringBuilder result)
    {
      if (value_Renamed == null)
        return;
      for (int index = 0; index < value_Renamed.Length; ++index)
      {
        if (value_Renamed[index] != null && value_Renamed[index].Length > 0)
        {
          if (result.Length > 0)
            result.Append('\n');
          result.Append(value_Renamed[index]);
        }
      }
    }
  }
}
