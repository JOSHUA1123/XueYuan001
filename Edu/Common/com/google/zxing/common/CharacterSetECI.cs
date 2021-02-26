// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.CharacterSetECI
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Collections;

namespace com.google.zxing.common
{
  /// <summary>
  /// Encapsulates a Character Set ECI, according to "Extended Channel Interpretations" 5.3.1.1
  ///             of ISO 18004.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class CharacterSetECI : ECI
  {
    private static Hashtable VALUE_TO_ECI;
    private static Hashtable NAME_TO_ECI;
    private string encodingName;

    public string EncodingName
    {
      get
      {
        return this.encodingName;
      }
    }

    private CharacterSetECI(int value_Renamed, string encodingName)
      : base(value_Renamed)
    {
      this.encodingName = encodingName;
    }

    private static void initialize()
    {
      CharacterSetECI.VALUE_TO_ECI = Hashtable.Synchronized(new Hashtable(29));
      CharacterSetECI.NAME_TO_ECI = Hashtable.Synchronized(new Hashtable(29));
      CharacterSetECI.addCharacterSet(0, "Cp437");
      CharacterSetECI.addCharacterSet(1, new string[2]
      {
        "ISO8859_1",
        "ISO-8859-1"
      });
      CharacterSetECI.addCharacterSet(2, "Cp437");
      CharacterSetECI.addCharacterSet(3, new string[2]
      {
        "ISO8859_1",
        "ISO-8859-1"
      });
      CharacterSetECI.addCharacterSet(4, "ISO8859_2");
      CharacterSetECI.addCharacterSet(5, "ISO8859_3");
      CharacterSetECI.addCharacterSet(6, "ISO8859_4");
      CharacterSetECI.addCharacterSet(7, "ISO8859_5");
      CharacterSetECI.addCharacterSet(8, "ISO8859_6");
      CharacterSetECI.addCharacterSet(9, "ISO8859_7");
      CharacterSetECI.addCharacterSet(10, "ISO8859_8");
      CharacterSetECI.addCharacterSet(11, "ISO8859_9");
      CharacterSetECI.addCharacterSet(12, "ISO8859_10");
      CharacterSetECI.addCharacterSet(13, "ISO8859_11");
      CharacterSetECI.addCharacterSet(15, "ISO8859_13");
      CharacterSetECI.addCharacterSet(16, "ISO8859_14");
      CharacterSetECI.addCharacterSet(17, "ISO8859_15");
      CharacterSetECI.addCharacterSet(18, "ISO8859_16");
      CharacterSetECI.addCharacterSet(20, new string[2]
      {
        "SJIS",
        "Shift_JIS"
      });
    }

    private static void addCharacterSet(int value_Renamed, string encodingName)
    {
      CharacterSetECI characterSetEci = new CharacterSetECI(value_Renamed, encodingName);
      CharacterSetECI.VALUE_TO_ECI[(object) value_Renamed] = (object) characterSetEci;
      CharacterSetECI.NAME_TO_ECI[(object) encodingName] = (object) characterSetEci;
    }

    private static void addCharacterSet(int value_Renamed, string[] encodingNames)
    {
      CharacterSetECI characterSetEci = new CharacterSetECI(value_Renamed, encodingNames[0]);
      CharacterSetECI.VALUE_TO_ECI[(object) value_Renamed] = (object) characterSetEci;
      for (int index = 0; index < encodingNames.Length; ++index)
        CharacterSetECI.NAME_TO_ECI[(object) encodingNames[index]] = (object) characterSetEci;
    }

    /// <param name="value">character set ECI value
    ///             </param>
    /// <returns>
    /// {@link CharacterSetECI} representing ECI of given value, or null if it is legal but
    ///             unsupported
    /// 
    /// </returns>
    /// <throws>IllegalArgumentException if ECI value is invalid </throws>
    public static CharacterSetECI getCharacterSetECIByValue(int value_Renamed)
    {
      if (CharacterSetECI.VALUE_TO_ECI == null)
        CharacterSetECI.initialize();
      if (value_Renamed < 0 || value_Renamed >= 900)
        throw new ArgumentException("Bad ECI value: " + (object) value_Renamed);
      return (CharacterSetECI) CharacterSetECI.VALUE_TO_ECI[(object) value_Renamed];
    }

    /// <param name="name">character set ECI encoding name
    ///             </param>
    /// <returns>
    /// {@link CharacterSetECI} representing ECI for character encoding, or null if it is legal
    ///             but unsupported
    /// 
    /// </returns>
    public static CharacterSetECI getCharacterSetECIByName(string name)
    {
      if (CharacterSetECI.NAME_TO_ECI == null)
        CharacterSetECI.initialize();
      return (CharacterSetECI) CharacterSetECI.NAME_TO_ECI[(object) name];
    }
  }
}
