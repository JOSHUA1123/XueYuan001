// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.VEventResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using System;

namespace com.google.zxing.client.result
{
  /// <summary>
  /// Partially implements the iCalendar format's "VEVENT" format for specifying a
  ///             calendar event. See RFC 2445. This supports SUMMARY, DTSTART and DTEND fields.
  /// 
  /// 
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class VEventResultParser : ResultParser
  {
    private VEventResultParser()
    {
    }

    public static CalendarParsedResult parse(Result result)
    {
      string text = result.Text;
      if (text == null)
        return (CalendarParsedResult) null;
      if (text.IndexOf("BEGIN:VEVENT") < 0)
        return (CalendarParsedResult) null;
      if (text.IndexOf("END:VEVENT") < 0)
        return (CalendarParsedResult) null;
      string summary = VCardResultParser.matchSingleVCardPrefixedField("SUMMARY", text, true);
      string start = VCardResultParser.matchSingleVCardPrefixedField("DTSTART", text, true);
      string end = VCardResultParser.matchSingleVCardPrefixedField("DTEND", text, true);
      try
      {
        return new CalendarParsedResult(summary, start, end, (string) null, (string) null, (string) null);
      }
      catch (ArgumentException ex)
      {
        return (CalendarParsedResult) null;
      }
    }
  }
}
