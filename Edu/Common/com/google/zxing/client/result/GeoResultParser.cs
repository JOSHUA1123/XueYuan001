// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.GeoResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Parses a "geo:" URI result, which specifies a location on the surface of
  ///             the Earth as well as an optional altitude above the surface. See
  ///             <a href="http://tools.ietf.org/html/draft-mayrhofer-geo-uri-00">http://tools.ietf.org/html/draft-mayrhofer-geo-uri-00</a>.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class GeoResultParser : ResultParser
  {
    private GeoResultParser()
    {
    }

    public static GeoParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("geo:") && !text.StartsWith("GEO:"))
        return (GeoParsedResult) null;
      int num1 = text.IndexOf('?', 4);
      string str = num1 < 0 ? text.Substring(4) : text.Substring(4, num1 - 4);
      int length = str.IndexOf(',');
      if (length < 0)
        return (GeoParsedResult) null;
      int num2 = str.IndexOf(',', length + 1);
      double latitude;
      double longitude;
      double altitude;
      try
      {
        latitude = double.Parse(str.Substring(0, length));
        if (num2 < 0)
        {
          longitude = double.Parse(str.Substring(length + 1));
          altitude = 0.0;
        }
        else
        {
          longitude = double.Parse(str.Substring(length + 1, num2 - (length + 1)));
          altitude = double.Parse(str.Substring(num2 + 1));
        }
      }
      catch (FormatException ex)
      {
        return (GeoParsedResult) null;
      }
      return new GeoParsedResult(text.StartsWith("GEO:") ? "geo:" + text.Substring(4) : text, latitude, longitude, altitude);
    }
  }
}
