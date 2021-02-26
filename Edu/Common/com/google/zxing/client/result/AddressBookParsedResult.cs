// Decompiled with JetBrains decompiler
// Type: com.google.zxing.client.result.AddressBookParsedResult
// Assembly: Common, Version=1.1.0.0, Culture=neutral, PublicKeyToken=fd39f026add70812
// MVID: CA845B7A-AC3D-4034-A2C3-E36DE10AFD1D
// Assembly location: G:\LearningSystem-master\Lib\Common.dll

using System.Text;

namespace com.google.zxing.client.result
{
  /// <author>Sean Owen
  ///             </author><author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source
  ///             </author>
  public sealed class AddressBookParsedResult : ParsedResult
  {
    private string[] names;
    private string pronunciation;
    private string[] phoneNumbers;
    private string[] emails;
    private string note;
    private string[] addresses;
    private string org;
    private string birthday;
    private string title;
    private string url;

    public string[] Names
    {
      get
      {
        return this.names;
      }
    }

    /// <summary>
    /// In Japanese, the name is written in kanji, which can have multiple readings. Therefore a hint
    ///             is often provided, called furigana, which spells the name phonetically.
    /// 
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// The pronunciation of the getNames() field, often in hiragana or katakana.
    /// 
    /// </returns>
    public string Pronunciation
    {
      get
      {
        return this.pronunciation;
      }
    }

    public string[] PhoneNumbers
    {
      get
      {
        return this.phoneNumbers;
      }
    }

    public string[] Emails
    {
      get
      {
        return this.emails;
      }
    }

    public string Note
    {
      get
      {
        return this.note;
      }
    }

    public string[] Addresses
    {
      get
      {
        return this.addresses;
      }
    }

    public string Title
    {
      get
      {
        return this.title;
      }
    }

    public string Org
    {
      get
      {
        return this.org;
      }
    }

    public string URL
    {
      get
      {
        return this.url;
      }
    }

    /// <returns>
    /// birthday formatted as yyyyMMdd (e.g. 19780917)
    /// 
    /// </returns>
    public string Birthday
    {
      get
      {
        return this.birthday;
      }
    }

    public override string DisplayResult
    {
      get
      {
        StringBuilder result = new StringBuilder(100);
        ParsedResult.maybeAppend(this.names, result);
        ParsedResult.maybeAppend(this.pronunciation, result);
        ParsedResult.maybeAppend(this.title, result);
        ParsedResult.maybeAppend(this.org, result);
        ParsedResult.maybeAppend(this.addresses, result);
        ParsedResult.maybeAppend(this.phoneNumbers, result);
        ParsedResult.maybeAppend(this.emails, result);
        ParsedResult.maybeAppend(this.url, result);
        ParsedResult.maybeAppend(this.birthday, result);
        ParsedResult.maybeAppend(this.note, result);
        return result.ToString();
      }
    }

    public AddressBookParsedResult(string[] names, string pronunciation, string[] phoneNumbers, string[] emails, string note, string[] addresses, string org, string birthday, string title, string url)
      : base(ParsedResultType.ADDRESSBOOK)
    {
      this.names = names;
      this.pronunciation = pronunciation;
      this.phoneNumbers = phoneNumbers;
      this.emails = emails;
      this.note = note;
      this.addresses = addresses;
      this.org = org;
      this.birthday = birthday;
      this.title = title;
      this.url = url;
    }
  }
}
