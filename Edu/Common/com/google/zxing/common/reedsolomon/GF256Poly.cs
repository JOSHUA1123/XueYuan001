// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.reedsolomon.GF256Poly
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System;
using System.Text;

namespace com.google.zxing.common.reedsolomon
{
  /// <summary>
  /// <p>Represents a polynomial whose coefficients are elements of GF(256).
  ///             Instances of this class are immutable.</p><p>Much credit is due to William Rucklidge since portions of this code are an indirect
  ///             port of his C++ Reed-Solomon implementation.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  internal sealed class GF256Poly
  {
    private GF256 field;
    private int[] coefficients;

    internal int[] Coefficients
    {
      get
      {
        return this.coefficients;
      }
    }

    /// <returns>
    /// degree of this polynomial
    /// 
    /// </returns>
    internal int Degree
    {
      get
      {
        return this.coefficients.Length - 1;
      }
    }

    /// <returns>
    /// true iff this polynomial is the monomial "0"
    /// 
    /// </returns>
    internal bool Zero
    {
      get
      {
        return this.coefficients[0] == 0;
      }
    }

    /// <param name="field">the {@link GF256} instance representing the field to use
    ///             to perform computations
    ///             </param><param name="coefficients">coefficients as ints representing elements of GF(256), arranged
    ///             from most significant (highest-power term) coefficient to least significant
    ///             </param><throws>IllegalArgumentException if argument is null or empty, </throws>
    /// <summary>
    /// or if leading coefficient is 0 and this is not a
    ///             constant polynomial (that is, it is not the monomial "0")
    /// 
    /// </summary>
    internal GF256Poly(GF256 field, int[] coefficients)
    {
      if (coefficients == null || coefficients.Length == 0)
        throw new ArgumentException();
      this.field = field;
      int length = coefficients.Length;
      if (length > 1 && coefficients[0] == 0)
      {
        int sourceIndex = 1;
        while (sourceIndex < length && coefficients[sourceIndex] == 0)
          ++sourceIndex;
        if (sourceIndex == length)
        {
          this.coefficients = field.Zero.coefficients;
        }
        else
        {
          this.coefficients = new int[length - sourceIndex];
          Array.Copy((Array) coefficients, sourceIndex, (Array) this.coefficients, 0, this.coefficients.Length);
        }
      }
      else
        this.coefficients = coefficients;
    }

    /// <returns>
    /// coefficient of x^degree term in this polynomial
    /// 
    /// </returns>
    internal int getCoefficient(int degree)
    {
      return this.coefficients[this.coefficients.Length - 1 - degree];
    }

    /// <returns>
    /// evaluation of this polynomial at a given point
    /// 
    /// </returns>
    internal int evaluateAt(int a)
    {
      if (a == 0)
        return this.getCoefficient(0);
      int length = this.coefficients.Length;
      if (a == 1)
      {
        int a1 = 0;
        for (int index = 0; index < length; ++index)
          a1 = GF256.addOrSubtract(a1, this.coefficients[index]);
        return a1;
      }
      int b = this.coefficients[0];
      for (int index = 1; index < length; ++index)
        b = GF256.addOrSubtract(this.field.multiply(a, b), this.coefficients[index]);
      return b;
    }

    internal GF256Poly addOrSubtract(GF256Poly other)
    {
      if (!this.field.Equals((object) other.field))
        throw new ArgumentException("GF256Polys do not have same GF256 field");
      if (this.Zero)
        return other;
      if (other.Zero)
        return this;
      int[] numArray1 = this.coefficients;
      int[] numArray2 = other.coefficients;
      if (numArray1.Length > numArray2.Length)
      {
        int[] numArray3 = numArray1;
        numArray1 = numArray2;
        numArray2 = numArray3;
      }
      int[] coefficients = new int[numArray2.Length];
      int length = numArray2.Length - numArray1.Length;
      Array.Copy((Array) numArray2, 0, (Array) coefficients, 0, length);
      for (int index = length; index < numArray2.Length; ++index)
        coefficients[index] = GF256.addOrSubtract(numArray1[index - length], numArray2[index]);
      return new GF256Poly(this.field, coefficients);
    }

    internal GF256Poly multiply(GF256Poly other)
    {
      if (!this.field.Equals((object) other.field))
        throw new ArgumentException("GF256Polys do not have same GF256 field");
      if (this.Zero || other.Zero)
        return this.field.Zero;
      int[] numArray1 = this.coefficients;
      int length1 = numArray1.Length;
      int[] numArray2 = other.coefficients;
      int length2 = numArray2.Length;
      int[] coefficients = new int[length1 + length2 - 1];
      for (int index1 = 0; index1 < length1; ++index1)
      {
        int a = numArray1[index1];
        for (int index2 = 0; index2 < length2; ++index2)
          coefficients[index1 + index2] = GF256.addOrSubtract(coefficients[index1 + index2], this.field.multiply(a, numArray2[index2]));
      }
      return new GF256Poly(this.field, coefficients);
    }

    internal GF256Poly multiply(int scalar)
    {
      if (scalar == 0)
        return this.field.Zero;
      if (scalar == 1)
        return this;
      int length = this.coefficients.Length;
      int[] coefficients = new int[length];
      for (int index = 0; index < length; ++index)
        coefficients[index] = this.field.multiply(this.coefficients[index], scalar);
      return new GF256Poly(this.field, coefficients);
    }

    internal GF256Poly multiplyByMonomial(int degree, int coefficient)
    {
      if (degree < 0)
        throw new ArgumentException();
      if (coefficient == 0)
        return this.field.Zero;
      int length = this.coefficients.Length;
      int[] coefficients = new int[length + degree];
      for (int index = 0; index < length; ++index)
        coefficients[index] = this.field.multiply(this.coefficients[index], coefficient);
      return new GF256Poly(this.field, coefficients);
    }

    internal GF256Poly[] divide(GF256Poly other)
    {
      if (!this.field.Equals((object) other.field))
        throw new ArgumentException("GF256Polys do not have same GF256 field");
      if (other.Zero)
        throw new ArgumentException("Divide by 0");
      GF256Poly gf256Poly1 = this.field.Zero;
      GF256Poly gf256Poly2 = this;
      int b = this.field.inverse(other.getCoefficient(other.Degree));
      GF256Poly other1;
      for (; gf256Poly2.Degree >= other.Degree && !gf256Poly2.Zero; gf256Poly2 = gf256Poly2.addOrSubtract(other1))
      {
        int degree = gf256Poly2.Degree - other.Degree;
        int coefficient = this.field.multiply(gf256Poly2.getCoefficient(gf256Poly2.Degree), b);
        other1 = other.multiplyByMonomial(degree, coefficient);
        GF256Poly other2 = this.field.buildMonomial(degree, coefficient);
        gf256Poly1 = gf256Poly1.addOrSubtract(other2);
      }
      return new GF256Poly[2]
      {
        gf256Poly1,
        gf256Poly2
      };
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(8 * this.Degree);
      for (int degree = this.Degree; degree >= 0; --degree)
      {
        int a = this.getCoefficient(degree);
        if (a != 0)
        {
          if (a < 0)
          {
            stringBuilder.Append(" - ");
            a = -a;
          }
          else if (stringBuilder.Length > 0)
            stringBuilder.Append(" + ");
          if (degree == 0 || a != 1)
          {
            int num = this.field.log(a);
            switch (num)
            {
              case 0:
                stringBuilder.Append('1');
                break;
              case 1:
                stringBuilder.Append('a');
                break;
              default:
                stringBuilder.Append("a^");
                stringBuilder.Append(num);
                break;
            }
          }
          if (degree != 0)
          {
            if (degree == 1)
            {
              stringBuilder.Append('x');
            }
            else
            {
              stringBuilder.Append("x^");
              stringBuilder.Append(degree);
            }
          }
        }
      }
      return stringBuilder.ToString();
    }
  }
}
