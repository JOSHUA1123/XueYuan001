// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.GeoParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Text;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class GeoParsedResult : ParsedResult
  {
    private string geoURI;
    private double latitude;
    private double longitude;
    private double altitude;

    public string GeoURI
    {
      get
      {
        return this.geoURI;
      }
    }

    /// <returns>
    /// latitude in degrees
    /// 
    /// </returns>
    public double Latitude
    {
      get
      {
        return this.latitude;
      }
    }

    /// <returns>
    /// longitude in degrees
    /// 
    /// </returns>
    public double Longitude
    {
      get
      {
        return this.longitude;
      }
    }

    /// <returns>
    /// altitude in meters. If not specified, in the geo URI, returns 0.0
    /// 
    /// </returns>
    public double Altitude
    {
      get
      {
        return this.altitude;
      }
    }

    public override string DisplayResult
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder(50);
        stringBuilder.Append(this.latitude);
        stringBuilder.Append(", ");
        stringBuilder.Append(this.longitude);
        if (this.altitude > 0.0)
        {
          stringBuilder.Append(", ");
          stringBuilder.Append(this.altitude);
          stringBuilder.Append('m');
        }
        return stringBuilder.ToString();
      }
    }

    internal GeoParsedResult(string geoURI, double latitude, double longitude, double altitude)
      : base(ParsedResultType.GEO)
    {
      this.geoURI = geoURI;
      this.latitude = latitude;
      this.longitude = longitude;
      this.altitude = altitude;
    }
  }
}
