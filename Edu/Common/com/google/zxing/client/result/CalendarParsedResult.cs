// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.CalendarParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Text;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class CalendarParsedResult : ParsedResult
  {
    private string summary;
    private string start;
    private string end;
    private string location;
    private string attendee;
    private string title;

    public string Summary
    {
      get
      {
        return this.summary;
      }
    }

    public string Start
    {
      get
      {
        return this.start;
      }
    }

    /// <seealso cref="!:getStart()May return null if the event has no duration."/>
    public string End
    {
      get
      {
        return this.end;
      }
    }

    public string Location
    {
      get
      {
        return this.location;
      }
    }

    public string Attendee
    {
      get
      {
        return this.attendee;
      }
    }

    public string Title
    {
      get
      {
        return this.title;
      }
    }

    public override string DisplayResult
    {
      get
      {
        StringBuilder result = new StringBuilder(100);
        ParsedResult.maybeAppend(this.summary, result);
        ParsedResult.maybeAppend(this.start, result);
        ParsedResult.maybeAppend(this.end, result);
        ParsedResult.maybeAppend(this.location, result);
        ParsedResult.maybeAppend(this.attendee, result);
        ParsedResult.maybeAppend(this.title, result);
        return result.ToString();
      }
    }

    public CalendarParsedResult(string summary, string start, string end, string location, string attendee, string title)
      : base(ParsedResultType.CALENDAR)
    {
      if (start == null)
        throw new ArgumentException();
      CalendarParsedResult.validateDate(start);
      CalendarParsedResult.validateDate(end);
      this.summary = summary;
      this.start = start;
      this.end = end;
      this.location = location;
      this.attendee = attendee;
      this.title = title;
    }

    /// <summary>
    /// RFC 2445 allows the start and end fields to be of type DATE (e.g. 20081021) or DATE-TIME
    ///             (e.g. 20081021T123000 for local time, or 20081021T123000Z for UTC).
    /// 
    /// 
    /// </summary>
    /// <param name="date">The string to validate
    ///             </param>
    private static void validateDate(string date)
    {
      if (date == null)
        return;
      int length = date.Length;
      switch (length)
      {
        case 8:
        case 15:
        case 16:
          for (int index = 0; index < 8; ++index)
          {
            if (!char.IsDigit(date[index]))
              throw new ArgumentException();
          }
          if (length <= 8)
            break;
          if ((int) date[8] != 84)
            throw new ArgumentException();
          for (int index = 9; index < 15; ++index)
          {
            if (!char.IsDigit(date[index]))
              throw new ArgumentException();
          }
          if (length != 16 || (int) date[15] == 90)
            break;
          throw new ArgumentException();
        default:
          throw new ArgumentException();
      }
    }
  }
}
