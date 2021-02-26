// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.reedsolomon.ReedSolomonDecoder
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

namespace com.google.zxing.common.reedsolomon
{
  /// <summary>
  /// <p>Implements Reed-Solomon decoding, as the name implies.</p><p>The algorithm will not be explained here, but the following references were helpful
  ///             in creating this implementation:</p><ul><li>Bruce Maggs.
  ///             <a href="http://www.cs.cmu.edu/afs/cs.cmu.edu/project/pscico-guyb/realworld/www/rs_decode.ps">"Decoding Reed-Solomon Codes"</a> (see discussion of Forney's Formula)</li><li>J.I. Hall. <a href="www.mth.msu.edu/~jhall/classes/codenotes/GRS.pdf">"Chapter 5. Generalized Reed-Solomon Codes"</a>
  ///             (see discussion of Euclidean algorithm)</li></ul><p>Much credit is due to William Rucklidge since portions of this code are an indirect
  ///             port of his C++ Reed-Solomon implementation.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>William Rucklidge
  ///             </author><author>sanfordsquires
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class ReedSolomonDecoder
  {
    private GF256 field;

    public ReedSolomonDecoder(GF256 field)
    {
      this.field = field;
    }

    /// <summary>
    /// <p>Decodes given set of received codewords, which include both data and error-correction
    ///             codewords. Really, this means it uses Reed-Solomon to detect and correct errors, in-place,
    ///             in the input.</p>
    /// </summary>
    /// <param name="received">data and error-correction codewords
    ///             </param><param name="twoS">number of error-correction codewords available
    ///             </param><throws>ReedSolomonException if decoding fails for any reason </throws>
    public void decode(int[] received, int twoS)
    {
      GF256Poly gf256Poly = new GF256Poly(this.field, received);
      int[] coefficients = new int[twoS];
      bool dataMatrix = this.field.Equals((object) GF256.DATA_MATRIX_FIELD);
      bool flag = true;
      for (int index = 0; index < twoS; ++index)
      {
        int num = gf256Poly.evaluateAt(this.field.exp(dataMatrix ? index + 1 : index));
        coefficients[coefficients.Length - 1 - index] = num;
        if (num != 0)
          flag = false;
      }
      if (flag)
        return;
      GF256Poly b = new GF256Poly(this.field, coefficients);
      GF256Poly[] gf256PolyArray = this.runEuclideanAlgorithm(this.field.buildMonomial(twoS, 1), b, twoS);
      GF256Poly errorLocator = gf256PolyArray[0];
      GF256Poly errorEvaluator = gf256PolyArray[1];
      int[] errorLocations = this.findErrorLocations(errorLocator);
      int[] errorMagnitudes = this.findErrorMagnitudes(errorEvaluator, errorLocations, dataMatrix);
      for (int index1 = 0; index1 < errorLocations.Length; ++index1)
      {
        int index2 = received.Length - 1 - this.field.log(errorLocations[index1]);
        if (index2 < 0)
          throw new ReedSolomonException("Bad error location");
        received[index2] = GF256.addOrSubtract(received[index2], errorMagnitudes[index1]);
      }
    }

    private GF256Poly[] runEuclideanAlgorithm(GF256Poly a, GF256Poly b, int R)
    {
      if (a.Degree < b.Degree)
      {
        GF256Poly gf256Poly = a;
        a = b;
        b = gf256Poly;
      }
      GF256Poly gf256Poly1 = a;
      GF256Poly gf256Poly2 = b;
      GF256Poly other1 = this.field.One;
      GF256Poly gf256Poly3 = this.field.Zero;
      GF256Poly other2 = this.field.Zero;
      GF256Poly gf256Poly4 = this.field.One;
      while (gf256Poly2.Degree >= R / 2)
      {
        GF256Poly gf256Poly5 = gf256Poly1;
        GF256Poly other3 = other1;
        GF256Poly other4 = other2;
        gf256Poly1 = gf256Poly2;
        other1 = gf256Poly3;
        other2 = gf256Poly4;
        if (gf256Poly1.Zero)
          throw new ReedSolomonException("r_{i-1} was zero");
        gf256Poly2 = gf256Poly5;
        GF256Poly gf256Poly6 = this.field.Zero;
        int b1 = this.field.inverse(gf256Poly1.getCoefficient(gf256Poly1.Degree));
        int degree;
        int coefficient;
        for (; gf256Poly2.Degree >= gf256Poly1.Degree && !gf256Poly2.Zero; gf256Poly2 = gf256Poly2.addOrSubtract(gf256Poly1.multiplyByMonomial(degree, coefficient)))
        {
          degree = gf256Poly2.Degree - gf256Poly1.Degree;
          coefficient = this.field.multiply(gf256Poly2.getCoefficient(gf256Poly2.Degree), b1);
          gf256Poly6 = gf256Poly6.addOrSubtract(this.field.buildMonomial(degree, coefficient));
        }
        gf256Poly3 = gf256Poly6.multiply(other1).addOrSubtract(other3);
        gf256Poly4 = gf256Poly6.multiply(other2).addOrSubtract(other4);
      }
      int coefficient1 = gf256Poly4.getCoefficient(0);
      if (coefficient1 == 0)
        throw new ReedSolomonException("sigmaTilde(0) was zero");
      int scalar = this.field.inverse(coefficient1);
      return new GF256Poly[2]
      {
        gf256Poly4.multiply(scalar),
        gf256Poly2.multiply(scalar)
      };
    }

    private int[] findErrorLocations(GF256Poly errorLocator)
    {
      int degree = errorLocator.Degree;
      if (degree == 1)
        return new int[1]
        {
          errorLocator.getCoefficient(1)
        };
      int[] numArray = new int[degree];
      int index = 0;
      for (int a = 1; a < 256 && index < degree; ++a)
      {
        if (errorLocator.evaluateAt(a) == 0)
        {
          numArray[index] = this.field.inverse(a);
          ++index;
        }
      }
      if (index != degree)
        throw new ReedSolomonException("Error locator degree does not match number of roots");
      return numArray;
    }

    private int[] findErrorMagnitudes(GF256Poly errorEvaluator, int[] errorLocations, bool dataMatrix)
    {
      int length = errorLocations.Length;
      int[] numArray = new int[length];
      for (int index1 = 0; index1 < length; ++index1)
      {
        int num = this.field.inverse(errorLocations[index1]);
        int a = 1;
        for (int index2 = 0; index2 < length; ++index2)
        {
          if (index1 != index2)
            a = this.field.multiply(a, GF256.addOrSubtract(1, this.field.multiply(errorLocations[index2], num)));
        }
        numArray[index1] = this.field.multiply(errorEvaluator.evaluateAt(num), this.field.inverse(a));
        if (dataMatrix)
          numArray[index1] = this.field.multiply(numArray[index1], num);
      }
      return numArray;
    }
  }
}
