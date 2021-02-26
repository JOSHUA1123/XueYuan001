// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.optional.NDEFRecord
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.client.result.optional
{
  /// <summary>
  /// <p>Represents a record in an NDEF message. This class only supports certain types
  ///             of records -- namely, non-chunked records, where ID length is omitted, and only
  ///             "short records".</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class NDEFRecord
  {
    private const int SUPPORTED_HEADER_MASK = 63;
    private const int SUPPORTED_HEADER = 17;
    public const string TEXT_WELL_KNOWN_TYPE = "T";
    public const string URI_WELL_KNOWN_TYPE = "U";
    public const string SMART_POSTER_WELL_KNOWN_TYPE = "Sp";
    public const string ACTION_WELL_KNOWN_TYPE = "act";
    private int header;
    private string type;
    private sbyte[] payload;
    private int totalRecordLength;

    internal bool MessageBegin
    {
      get
      {
        return (this.header & 128) != 0;
      }
    }

    internal bool MessageEnd
    {
      get
      {
        return (this.header & 64) != 0;
      }
    }

    internal string Type
    {
      get
      {
        return this.type;
      }
    }

    internal sbyte[] Payload
    {
      get
      {
        return this.payload;
      }
    }

    internal int TotalRecordLength
    {
      get
      {
        return this.totalRecordLength;
      }
    }

    private NDEFRecord(int header, string type, sbyte[] payload, int totalRecordLength)
    {
      this.header = header;
      this.type = type;
      this.payload = payload;
      this.totalRecordLength = totalRecordLength;
    }

    internal static NDEFRecord readRecord(sbyte[] bytes, int offset)
    {
      int header = (int) bytes[offset] & (int) byte.MaxValue;
      if (((header ^ 17) & 63) != 0)
        return (NDEFRecord) null;
      int length1 = (int) bytes[offset + 1] & (int) byte.MaxValue;
      int length2 = (int) bytes[offset + 2] & (int) byte.MaxValue;
      string type = AbstractNDEFResultParser.bytesToString(bytes, offset + 3, length1, "US-ASCII");
      sbyte[] payload = new sbyte[length2];
      Array.Copy((Array) bytes, offset + 3 + length1, (Array) payload, 0, length2);
      return new NDEFRecord(header, type, payload, 3 + length1 + length2);
    }
  }
}
