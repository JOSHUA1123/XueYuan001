// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.reedsolomon.GF256
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;

namespace com.google.zxing.common.reedsolomon
{
  /// <summary>
  /// <p>This class contains utility methods for performing mathematical operations over
  ///             the Galois Field GF(256). Operations use a given primitive polynomial in calculations.</p><p>Throughout this package, elements of GF(256) are represented as an
  /// <code>
  /// int
  /// </code>
  /// 
  ///             for convenience and speed (but at the cost of memory).
  ///             Only the bottom 8 bits are really used.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class GF256
  {
    public static readonly GF256 QR_CODE_FIELD = new GF256(285);
    public static readonly GF256 DATA_MATRIX_FIELD = new GF256(301);
    private int[] expTable;
    private int[] logTable;
    private GF256Poly zero;
    private GF256Poly one;

    internal GF256Poly Zero
    {
      get
      {
        return this.zero;
      }
    }

    internal GF256Poly One
    {
      get
      {
        return this.one;
      }
    }

    /// <summary>
    /// Create a representation of GF(256) using the given primitive polynomial.
    /// 
    /// 
    /// </summary>
    /// <param name="primitive">irreducible polynomial whose coefficients are represented by
    ///             the bits of an int, where the least-significant bit represents the constant
    ///             coefficient
    ///             </param>
    private GF256(int primitive)
    {
      this.expTable = new int[256];
      this.logTable = new int[256];
      int num = 1;
      for (int index = 0; index < 256; ++index)
      {
        this.expTable[index] = num;
        num <<= 1;
        if (num >= 256)
          num ^= primitive;
      }
      for (int index = 0; index < (int) byte.MaxValue; ++index)
        this.logTable[this.expTable[index]] = index;
      this.zero = new GF256Poly(this, new int[1]);
      this.one = new GF256Poly(this, new int[1]
      {
        1
      });
    }

    /// <returns>
    /// the monomial representing coefficient * x^degree
    /// 
    /// </returns>
    internal GF256Poly buildMonomial(int degree, int coefficient)
    {
      if (degree < 0)
        throw new ArgumentException();
      if (coefficient == 0)
        return this.zero;
      int[] coefficients = new int[degree + 1];
      coefficients[0] = coefficient;
      return new GF256Poly(this, coefficients);
    }

    /// <summary>
    /// Implements both addition and subtraction -- they are the same in GF(256).
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// sum/difference of a and b
    /// 
    /// </returns>
    internal static int addOrSubtract(int a, int b)
    {
      return a ^ b;
    }

    /// <returns>
    /// 2 to the power of a in GF(256)
    /// 
    /// </returns>
    internal int exp(int a)
    {
      return this.expTable[a];
    }

    /// <returns>
    /// base 2 log of a in GF(256)
    /// 
    /// </returns>
    internal int log(int a)
    {
      if (a == 0)
        throw new ArgumentException();
      return this.logTable[a];
    }

    /// <returns>
    /// multiplicative inverse of a
    /// 
    /// </returns>
    internal int inverse(int a)
    {
      if (a == 0)
        throw new ArithmeticException();
      return this.expTable[(int) byte.MaxValue - this.logTable[a]];
    }

    /// <param name="a"/><param name="b"/>
    /// <returns>
    /// product of a and b in GF(256)
    /// 
    /// </returns>
    internal int multiply(int a, int b)
    {
      if (a == 0 || b == 0)
        return 0;
      if (a == 1)
        return b;
      if (b == 1)
        return a;
      return this.expTable[(this.logTable[a] + this.logTable[b]) % (int) byte.MaxValue];
    }
  }
}
