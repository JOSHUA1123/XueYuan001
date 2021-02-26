using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 循环索引
	/// </summary>
	// Token: 0x02000020 RID: 32
	public class LoopIndex : IConvertible, IComparable
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		// Token: 0x0600017F RID: 383 RVA: 0x00007A50 File Offset: 0x00005C50
		public LoopIndex(decimal value)
		{
			this.Value = value;
		}

		/// <summary>
		/// 值
		/// </summary>
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00007A5F File Offset: 0x00005C5F
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00007A67 File Offset: 0x00005C67
		public decimal Value { get; internal set; }

		/// <summary>
		/// 是否是第一个索引值
		/// </summary>
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00007A70 File Offset: 0x00005C70
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00007A78 File Offset: 0x00005C78
		public bool IsFirst { get; internal set; }

		/// <summary>
		/// 是否是最后一个索引值
		/// </summary>
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007A81 File Offset: 0x00005C81
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00007A89 File Offset: 0x00005C89
		public bool IsLast { get; internal set; }

		/// <summary>
		/// 是否是偶数索引值
		/// </summary>
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007A92 File Offset: 0x00005C92
		public bool IsEven
		{
			get
			{
				return this.Value % 2m == 0m;
			}
		}

		/// <summary>
		/// 获取此索引值的字符串表现形式
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000187 RID: 391 RVA: 0x00007AB0 File Offset: 0x00005CB0
		public override string ToString()
		{
			return this.Value.ToString();
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000188 RID: 392 RVA: 0x00007ACC File Offset: 0x00005CCC
		public TypeCode GetTypeCode()
		{
			return this.Value.GetTypeCode();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000189 RID: 393 RVA: 0x00007AE7 File Offset: 0x00005CE7
		public bool ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x0600018A RID: 394 RVA: 0x00007AFA File Offset: 0x00005CFA
		public byte ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x0600018B RID: 395 RVA: 0x00007B0D File Offset: 0x00005D0D
		public char ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x0600018C RID: 396 RVA: 0x00007B20 File Offset: 0x00005D20
		public DateTime ToDateTime(IFormatProvider provider)
		{
			return Convert.ToDateTime(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x0600018D RID: 397 RVA: 0x00007B33 File Offset: 0x00005D33
		public decimal ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x0600018E RID: 398 RVA: 0x00007B46 File Offset: 0x00005D46
		public double ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x0600018F RID: 399 RVA: 0x00007B59 File Offset: 0x00005D59
		public short ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000190 RID: 400 RVA: 0x00007B6C File Offset: 0x00005D6C
		public int ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000191 RID: 401 RVA: 0x00007B7F File Offset: 0x00005D7F
		public long ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000192 RID: 402 RVA: 0x00007B92 File Offset: 0x00005D92
		public sbyte ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000193 RID: 403 RVA: 0x00007BA5 File Offset: 0x00005DA5
		public float ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000194 RID: 404 RVA: 0x00007BB8 File Offset: 0x00005DB8
		public string ToString(IFormatProvider provider)
		{
			return Convert.ToString(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="conversionType"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000195 RID: 405 RVA: 0x00007BC6 File Offset: 0x00005DC6
		public object ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)this.Value).ToType(conversionType, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000196 RID: 406 RVA: 0x00007BDA File Offset: 0x00005DDA
		public ushort ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000197 RID: 407 RVA: 0x00007BED File Offset: 0x00005DED
		public uint ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this.Value, provider);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		// Token: 0x06000198 RID: 408 RVA: 0x00007C00 File Offset: 0x00005E00
		public ulong ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this.Value, provider);
		}

		/// <summary>
		/// 比较器
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		// Token: 0x06000199 RID: 409 RVA: 0x00007C14 File Offset: 0x00005E14
		public int CompareTo(object obj)
		{
			if (obj == null || obj == DBNull.Value)
			{
				return 1;
			}
			decimal value;
			if (!(obj is decimal))
			{
				value = Utility.ConverToDecimal(obj);
			}
			else
			{
				value = (decimal)obj;
			}
			return this.Value.CompareTo(value);
		}
	}
}
