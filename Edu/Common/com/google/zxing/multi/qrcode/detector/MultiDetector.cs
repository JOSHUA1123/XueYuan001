// Decompiled with JetBrains decompiler
// Type: com.google.zxing.multi.qrcode.detector.MultiDetector
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.detector;
using System.Collections;

namespace com.google.zxing.multi.qrcode.detector
{
  /// <summary>
  /// <p>Encapsulates logic that can detect one or more QR Codes in an image, even if the QR Code
  ///             is rotated or skewed, or partially obscured.</p>
  /// </summary>
  /// <author>Sean Owen
  ///             </author><author>Hannes Erven
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class MultiDetector : Detector
  {
    private static readonly DetectorResult[] EMPTY_DETECTOR_RESULTS = new DetectorResult[0];

    public MultiDetector(BitMatrix image)
      : base(image)
    {
    }

    public DetectorResult[] detectMulti(Hashtable hints)
    {
      FinderPatternInfo[] multi = new MultiFinderPatternFinder(this.Image).findMulti(hints);
      if (multi == null || multi.Length == 0)
        throw ReaderException.Instance;
      ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
      for (int index = 0; index < multi.Length; ++index)
      {
        try
        {
          arrayList.Add((object) this.processFinderPatternInfo(multi[index]));
        }
        catch (ReaderException ex)
        {
        }
      }
      if (arrayList.Count == 0)
        return MultiDetector.EMPTY_DETECTOR_RESULTS;
      DetectorResult[] detectorResultArray = new DetectorResult[arrayList.Count];
      for (int index = 0; index < arrayList.Count; ++index)
        detectorResultArray[index] = (DetectorResult) arrayList[index];
      return detectorResultArray;
    }
  }
}
