using System;
using System.Text;
using System.Text.RegularExpressions;

// Token: 0x02000002 RID: 2
internal static class StringHelperClass
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	internal static string SubstringSpecial(this string self, int start, int end)
	{
		return self.Substring(start, end - start);
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
	internal static bool StartsWith(this string self, string prefix, int toffset)
	{
		return self.IndexOf(prefix, toffset, StringComparison.Ordinal) == toffset;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000208C File Offset: 0x0000028C
	internal static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings)
	{
		string[] array = Regex.Split(self, regexDelimiter);
		if (trimTrailingEmptyStrings)
		{
			bool flag = array.Length > 1;
			if (flag)
			{
				for (int i = array.Length; i > 0; i--)
				{
					bool flag2 = array[i - 1].Length > 0;
					if (flag2)
					{
						bool flag3 = i < array.Length;
						if (flag3)
						{
							Array.Resize<string>(ref array, i);
						}
						break;
					}
				}
			}
		}
		return array;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002100 File Offset: 0x00000300
	internal static string NewString(byte[] bytes)
	{
		return StringHelperClass.NewString(bytes, 0, bytes.Length);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000211C File Offset: 0x0000031C
	internal static string NewString(byte[] bytes, int index, int count)
	{
		return Encoding.UTF8.GetString((byte[])bytes, index, count);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00000340
	internal static string NewString(byte[] bytes, string encoding)
	{
		return StringHelperClass.NewString(bytes, 0, bytes.Length, encoding);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002160 File Offset: 0x00000360
	internal static string NewString(byte[] bytes, int index, int count, string encoding)
	{
		return Encoding.GetEncoding(encoding).GetString((byte[])bytes, index, count);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002188 File Offset: 0x00000388
	internal static byte[] GetBytes(this string self)
	{
		return StringHelperClass.GetbytesForEncoding(Encoding.UTF8, self);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000021A8 File Offset: 0x000003A8
	internal static byte[] GetBytes(this string self, string encoding)
	{
		return StringHelperClass.GetbytesForEncoding(Encoding.GetEncoding(encoding), self);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000021C8 File Offset: 0x000003C8
	private static byte[] GetbytesForEncoding(Encoding encoding, string s)
	{
		byte[] array = new byte[encoding.GetByteCount(s)];
		encoding.GetBytes(s, 0, s.Length, (byte[])array, 0);
		return array;
	}
}
