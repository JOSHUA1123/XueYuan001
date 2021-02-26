// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.BizcardResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System.Collections;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Implements the "BIZCARD" address book entry format, though this has been
  ///             largely reverse-engineered from examples observed in the wild -- still
  ///             looking for a definitive reference.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class BizcardResultParser : AbstractDoCoMoResultParser
  {
    public static AddressBookParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null || !text.StartsWith("BIZCARD:"))
        return (AddressBookParsedResult) null;
      string value_Renamed1 = BizcardResultParser.buildName(AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("N:", text, true), AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("X:", text, true));
      string title = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("T:", text, true);
      string org = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("C:", text, true);
      string[] addresses = AbstractDoCoMoResultParser.matchDoCoMoPrefixedField("A:", text, true);
      string number1 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("B:", text, true);
      string number2 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("M:", text, true);
      string number3 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("F:", text, true);
      string value_Renamed2 = AbstractDoCoMoResultParser.matchSingleDoCoMoPrefixedField("E:", text, true);
      return new AddressBookParsedResult(ResultParser.maybeWrap(value_Renamed1), (string) null, BizcardResultParser.buildPhoneNumbers(number1, number2, number3), ResultParser.maybeWrap(value_Renamed2), (string) null, addresses, org, (string) null, title, (string) null);
    }

    private static string[] buildPhoneNumbers(string number1, string number2, string number3)
    {
      ArrayList arrayList = ArrayList.Synchronized(new ArrayList(3));
      if (number1 != null)
        arrayList.Add((object) number1);
      if (number2 != null)
        arrayList.Add((object) number2);
      if (number3 != null)
        arrayList.Add((object) number3);
      int count = arrayList.Count;
      if (count == 0)
        return (string[]) null;
      string[] strArray = new string[count];
      for (int index = 0; index < count; ++index)
        strArray[index] = (string) arrayList[index];
      return strArray;
    }

    private static string buildName(string firstName, string lastName)
    {
      if (firstName == null)
        return lastName;
      if (lastName != null)
        return firstName + (object) ' ' + lastName;
      return firstName;
    }
  }
}
