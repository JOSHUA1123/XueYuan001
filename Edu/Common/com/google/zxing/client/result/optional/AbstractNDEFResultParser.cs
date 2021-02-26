// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.optional.AbstractNDEFResultParser
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.client.result;
using System;
using System.IO;
using System.Text;

namespace com.google.zxing.client.result.optional
{
  /// <summary>
  /// <p>Superclass for classes encapsulating results in the NDEF format.
  ///             See <a href="http://www.nfc-forum.org/specs/">http://www.nfc-forum.org/specs/</a>.</p><p>This code supports a limited subset of NDEF messages, ones that are plausibly
  ///             useful in 2D barcode formats. This generally includes 1-record messages, no chunking,
  ///             "short record" syntax, no ID field.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal abstract class AbstractNDEFResultParser : ResultParser
  {
    internal static string bytesToString(sbyte[] bytes, int offset, int length, string encoding)
    {
      try
      {
        return new string(Encoding.GetEncoding(encoding).GetString(SupportClass.ToByteArray(bytes)).ToCharArray(), offset, length);
      }
      catch (IOException ex)
      {
        throw new SystemException("Platform does not support required encoding: " + (object) ex);
      }
    }
  }
}
