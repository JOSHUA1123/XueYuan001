// Decompiled with JetBrains decompiler
// Type: com.google.zxing.qrcode.encoder.QRCode
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using System;
using System.Text;

namespace com.google.zxing.qrcode.encoder
{
  /// <author>satorux@google.com (Satoru Takabayashi) - creator
  ///             </author><author>dswitkin@google.com (Daniel Switkin) - ported from C++
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class QRCode
  {
    public const int NUM_MASK_PATTERNS = 8;
    private Mode mode;
    private ErrorCorrectionLevel ecLevel;
    private int version;
    private int matrixWidth;
    private int maskPattern;
    private int numTotalBytes;
    private int numDataBytes;
    private int numECBytes;
    private int numRSBlocks;
    private ByteMatrix matrix;

    public Mode Mode
    {
      get
      {
        return this.mode;
      }
      set
      {
        this.mode = value;
      }
    }

    public ErrorCorrectionLevel ECLevel
    {
      get
      {
        return this.ecLevel;
      }
      set
      {
        this.ecLevel = value;
      }
    }

    public int Version
    {
      get
      {
        return this.version;
      }
      set
      {
        this.version = value;
      }
    }

    public int MatrixWidth
    {
      get
      {
        return this.matrixWidth;
      }
      set
      {
        this.matrixWidth = value;
      }
    }

    public int MaskPattern
    {
      get
      {
        return this.maskPattern;
      }
      set
      {
        this.maskPattern = value;
      }
    }

    public int NumTotalBytes
    {
      get
      {
        return this.numTotalBytes;
      }
      set
      {
        this.numTotalBytes = value;
      }
    }

    public int NumDataBytes
    {
      get
      {
        return this.numDataBytes;
      }
      set
      {
        this.numDataBytes = value;
      }
    }

    public int NumECBytes
    {
      get
      {
        return this.numECBytes;
      }
      set
      {
        this.numECBytes = value;
      }
    }

    public int NumRSBlocks
    {
      get
      {
        return this.numRSBlocks;
      }
      set
      {
        this.numRSBlocks = value;
      }
    }

    public ByteMatrix Matrix
    {
      get
      {
        return this.matrix;
      }
      set
      {
        this.matrix = value;
      }
    }

    public bool Valid
    {
      get
      {
        if (this.mode != null && this.ecLevel != null && (this.version != -1 && this.matrixWidth != -1) && (this.maskPattern != -1 && this.numTotalBytes != -1 && (this.numDataBytes != -1 && this.numECBytes != -1)) && (this.numRSBlocks != -1 && QRCode.isValidMaskPattern(this.maskPattern) && (this.numTotalBytes == this.numDataBytes + this.numECBytes && this.matrix != null) && this.matrixWidth == this.matrix.Width))
          return this.matrix.Width == this.matrix.Height;
        return false;
      }
    }

    public QRCode()
    {
      this.mode = (Mode) null;
      this.ecLevel = (ErrorCorrectionLevel) null;
      this.version = -1;
      this.matrixWidth = -1;
      this.maskPattern = -1;
      this.numTotalBytes = -1;
      this.numDataBytes = -1;
      this.numECBytes = -1;
      this.numRSBlocks = -1;
      this.matrix = (ByteMatrix) null;
    }

    public int at(int x, int y)
    {
      int num = (int) this.matrix.get_Renamed(x, y);
      switch (num)
      {
        case 0:
        case 1:
          return num;
        default:
          throw new SystemException("Bad value");
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(200);
      stringBuilder.Append("<<\n");
      stringBuilder.Append(" mode: ");
      stringBuilder.Append((object) this.mode);
      stringBuilder.Append("\n ecLevel: ");
      stringBuilder.Append((object) this.ecLevel);
      stringBuilder.Append("\n version: ");
      stringBuilder.Append(this.version);
      stringBuilder.Append("\n matrixWidth: ");
      stringBuilder.Append(this.matrixWidth);
      stringBuilder.Append("\n maskPattern: ");
      stringBuilder.Append(this.maskPattern);
      stringBuilder.Append("\n numTotalBytes: ");
      stringBuilder.Append(this.numTotalBytes);
      stringBuilder.Append("\n numDataBytes: ");
      stringBuilder.Append(this.numDataBytes);
      stringBuilder.Append("\n numECBytes: ");
      stringBuilder.Append(this.numECBytes);
      stringBuilder.Append("\n numRSBlocks: ");
      stringBuilder.Append(this.numRSBlocks);
      if (this.matrix == null)
      {
        stringBuilder.Append("\n matrix: null\n");
      }
      else
      {
        stringBuilder.Append("\n matrix:\n");
        stringBuilder.Append(this.matrix.ToString());
      }
      stringBuilder.Append(">>\n");
      return stringBuilder.ToString();
    }

    public static bool isValidMaskPattern(int maskPattern)
    {
      if (maskPattern >= 0)
        return maskPattern < 8;
      return false;
    }
  }
}
