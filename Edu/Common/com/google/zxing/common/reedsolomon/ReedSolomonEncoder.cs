// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.reedsolomon.ReedSolomonEncoder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Collections;

namespace com.google.zxing.common.reedsolomon
{
  /// <summary>
  /// <p>Implements Reed-Solomon enbcoding, as the name implies.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>William Rucklidge
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ReedSolomonEncoder
  {
    private GF256 field;
    private ArrayList cachedGenerators;

    public ReedSolomonEncoder(GF256 field)
    {
      if (!GF256.QR_CODE_FIELD.Equals((object) field))
        throw new ArgumentException("Only QR Code is supported at this time");
      this.field = field;
      this.cachedGenerators = ArrayList.Synchronized(new ArrayList(10));
      this.cachedGenerators.Add((object) new GF256Poly(field, new int[1]
      {
        1
      }));
    }

    private GF256Poly buildGenerator(int degree)
    {
      if (degree >= this.cachedGenerators.Count)
      {
        GF256Poly gf256Poly1 = (GF256Poly) this.cachedGenerators[this.cachedGenerators.Count - 1];
        for (int count = this.cachedGenerators.Count; count <= degree; ++count)
        {
          GF256Poly gf256Poly2 = gf256Poly1.multiply(new GF256Poly(this.field, new int[2]
          {
            1,
            this.field.exp(count - 1)
          }));
          this.cachedGenerators.Add((object) gf256Poly2);
          gf256Poly1 = gf256Poly2;
        }
      }
      return (GF256Poly) this.cachedGenerators[degree];
    }

    public void encode(int[] toEncode, int ecBytes)
    {
      if (ecBytes == 0)
        throw new ArgumentException("No error correction bytes");
      int length = toEncode.Length - ecBytes;
      if (length <= 0)
        throw new ArgumentException("No data bytes provided");
      GF256Poly other = this.buildGenerator(ecBytes);
      int[] coefficients1 = new int[length];
      Array.Copy((Array) toEncode, 0, (Array) coefficients1, 0, length);
      int[] coefficients2 = new GF256Poly(this.field, coefficients1).multiplyByMonomial(ecBytes, 1).divide(other)[1].Coefficients;
      int num = ecBytes - coefficients2.Length;
      for (int index = 0; index < num; ++index)
        toEncode[length + index] = 0;
      Array.Copy((Array) coefficients2, 0, (Array) toEncode, length + num, coefficients2.Length);
    }
  }
}
