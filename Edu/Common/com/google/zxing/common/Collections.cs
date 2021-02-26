// Decompiled with JetBrains decompiler
// Type: com.google.zxing.common.Collections
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Collections;

namespace com.google.zxing.common
{
  /// <summary>
  /// <p>This is basically a substitute for
  /// <code>
  /// java.util.Collections
  /// </code>
  /// , which is not
  ///             present in MIDP 2.0 / CLDC 1.1.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class Collections
  {
    private Collections()
    {
    }

    /// <summary>
    /// Sorts its argument (destructively) using insert sort; in the context of this package
    ///             insertion sort is simple and efficient given its relatively small inputs.
    /// 
    /// 
    /// </summary>
    /// <param name="vector">vector to sort
    ///             </param><param name="comparator">comparator to define sort ordering
    ///             </param>
    public static void insertionSort(ArrayList vector, Comparator comparator)
    {
      int count = vector.Count;
      for (int index1 = 1; index1 < count; ++index1)
      {
        object o2 = vector[index1];
        int index2;
        object obj;
        for (index2 = index1 - 1; index2 >= 0 && comparator.compare(obj = vector[index2], o2) > 0; --index2)
          vector[index2 + 1] = obj;
        vector[index2 + 1] = o2;
      }
    }
  }
}
