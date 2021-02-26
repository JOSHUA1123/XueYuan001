// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.QRCodeWriter
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;
using System;
using System.Collections;

namespace com.google.zxing.qrcode
{
  /// <summary>
  /// This object renders a QR Code as a ByteMatrix 2D array of greyscale values.
  /// 
  /// 
  /// </summary>
  /// <author>dswitkin@google.com (Daniel Switkin)
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class QRCodeWriter : Writer
  {
    private const int QUIET_ZONE_SIZE = 0;

    public ByteMatrix encode(string contents, BarcodeFormat format, int width, int height)
    {
      return this.encode(contents, format, width, height, (Hashtable) null);
    }

    /// <summary>
    /// 生成二维码图像矩阵，默认是高容错
    /// 
    /// </summary>
    /// <param name="contents"/><param name="format"/><param name="width"/><param name="height"/><param name="hints"/>
    /// <returns/>
    public ByteMatrix encode(string contents, BarcodeFormat format, int width, int height, Hashtable hints)
    {
      if (contents == null || contents.Length == 0)
        throw new ArgumentException("Found empty contents");
      if (format != BarcodeFormat.QR_CODE)
        throw new ArgumentException("Can only encode QR_CODE, but got " + (object) format);
      if (width < 0 || height < 0)
        throw new ArgumentException(string.Concat(new object[4]
        {
          (object) "Requested dimensions are too small: ",
          (object) width,
          (object) 'x',
          (object) height
        }));
      ErrorCorrectionLevel ecLevel = ErrorCorrectionLevel.H;
      if (hints != null)
      {
        ErrorCorrectionLevel errorCorrectionLevel = (ErrorCorrectionLevel) hints[(object) EncodeHintType.ERROR_CORRECTION];
        if (errorCorrectionLevel != null)
          ecLevel = errorCorrectionLevel;
      }
      QRCode qrCode = new QRCode();
      Encoder.encode(contents, ecLevel, hints, qrCode);
      return QRCodeWriter.renderResult(qrCode, width, height);
    }

    /// <summary>
    /// 生成二维码图像矩阵，可以选择容错级别
    /// 
    /// </summary>
    /// <param name="contents"/><param name="format"/><param name="corrLevel">容错级别</param><param name="width"/><param name="height"/><param name="hints"/>
    /// <returns/>
    public ByteMatrix encode(string contents, BarcodeFormat format, ErrorCorrectionLevel corrLevel, int width, int height, Hashtable hints)
    {
      if (contents == null || contents.Length == 0)
        throw new ArgumentException("Found empty contents");
      if (format != BarcodeFormat.QR_CODE)
        throw new ArgumentException("Can only encode QR_CODE, but got " + (object) format);
      if (width < 0 || height < 0)
        throw new ArgumentException(string.Concat(new object[4]
        {
          (object) "Requested dimensions are too small: ",
          (object) width,
          (object) 'x',
          (object) height
        }));
      ErrorCorrectionLevel ecLevel = corrLevel;
      if (hints != null)
      {
        ErrorCorrectionLevel errorCorrectionLevel = (ErrorCorrectionLevel) hints[(object) EncodeHintType.ERROR_CORRECTION];
        if (errorCorrectionLevel != null)
          ecLevel = errorCorrectionLevel;
      }
      QRCode qrCode = new QRCode();
      Encoder.encode(contents, ecLevel, hints, qrCode);
      return QRCodeWriter.renderResult(qrCode, width, height);
    }

    private static ByteMatrix renderResult(QRCode code, int width, int height)
    {
      ByteMatrix matrix = code.Matrix;
      int width1 = matrix.Width;
      int height1 = matrix.Height;
      int val2_1 = width1;
      int val2_2 = height1;
      int length = Math.Max(width, val2_1);
      int height2 = Math.Max(height, val2_2);
      int num1 = Math.Min(length / val2_1, height2 / val2_2);
      int num2 = (length - width1 * num1) / 2;
      int num3 = (height2 - height1 * num1) / 2;
      ByteMatrix byteMatrix = new ByteMatrix(length, height2);
      sbyte[][] array1 = byteMatrix.Array;
      sbyte[] numArray = new sbyte[length];
      for (int index = 0; index < num3; ++index)
        QRCodeWriter.setRowColor(array1[index], (sbyte) SupportClass.Identity((long) byte.MaxValue));
      sbyte[][] array2 = matrix.Array;
      for (int index1 = 0; index1 < height1; ++index1)
      {
        for (int index2 = 0; index2 < num2; ++index2)
          numArray[index2] = (sbyte) SupportClass.Identity((long) byte.MaxValue);
        int num4 = num2;
        for (int index2 = 0; index2 < width1; ++index2)
        {
          sbyte num5 = (int) array2[index1][index2] == 1 ? (sbyte) 0 : (sbyte) SupportClass.Identity((long) byte.MaxValue);
          for (int index3 = 0; index3 < num1; ++index3)
            numArray[num4 + index3] = num5;
          num4 += num1;
        }
        for (int index2 = num2 + width1 * num1; index2 < length; ++index2)
          numArray[index2] = (sbyte) SupportClass.Identity((long) byte.MaxValue);
        int num6 = num3 + index1 * num1;
        for (int index2 = 0; index2 < num1; ++index2)
          Array.Copy((Array) numArray, 0, (Array) array1[num6 + index2], 0, length);
      }
      for (int index = num3 + height1 * num1; index < height2; ++index)
        QRCodeWriter.setRowColor(array1[index], (sbyte) SupportClass.Identity((long) byte.MaxValue));
      return byteMatrix;
    }

    private static void setRowColor(sbyte[] row, sbyte value_Renamed)
    {
      for (int index = 0; index < row.Length; ++index)
        row[index] = value_Renamed;
    }
  }
}
