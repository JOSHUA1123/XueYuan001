// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.ECI
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.common
{
  /// <summary>
  /// Superclass of classes encapsulating types ECIs, according to "Extended Channel Interpretations"
  ///             5.3 of ISO 18004.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public abstract class ECI
  {
    private int value_Renamed;

    public virtual int Value
    {
      get
      {
        return this.value_Renamed;
      }
    }

    internal ECI(int value_Renamed)
    {
      this.value_Renamed = value_Renamed;
    }

    /// <param name="value">ECI value
    ///             </param>
    /// <returns>
    /// {@link ECI} representing ECI of given value, or null if it is legal but unsupported
    /// 
    /// </returns>
    /// <throws>IllegalArgumentException if ECI value is invalid </throws>
    public static ECI getECIByValue(int value_Renamed)
    {
      if (value_Renamed < 0 || value_Renamed > 999999)
        throw new ArgumentException("Bad ECI value: " + (object) value_Renamed);
      if (value_Renamed < 900)
        return (ECI) CharacterSetECI.getCharacterSetECIByValue(value_Renamed);
      return (ECI) null;
    }
  }
}
