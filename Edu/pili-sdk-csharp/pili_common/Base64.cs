using System;
using System.Diagnostics;
using System.Text;

namespace pili_sdk.pili_common
{
	// Token: 0x02000008 RID: 8
	public sealed class Base64
	{
		// Token: 0x06000035 RID: 53 RVA: 0x0000284D File Offset: 0x00000A4D
		private Base64()
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002858 File Offset: 0x00000A58
		public static byte[] decode(string str, int flags)
		{
			return Base64.decode(Encoding.UTF8.GetBytes(str), flags);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000287C File Offset: 0x00000A7C
		public static byte[] decode(byte[] input, int flags)
		{
			return Base64.decode(input, 0, input.Length, flags);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000289C File Offset: 0x00000A9C
		public static byte[] decode(byte[] input, int offset, int len, int flags)
		{
			Base64.Decoder decoder = new Base64.Decoder(flags, new byte[len * 3 / 4]);
			bool flag = !decoder.process(input, offset, len, true);
			if (flag)
			{
				throw new ArgumentException("bad base-64");
			}
			bool flag2 = decoder.op == decoder.output.Length;
			byte[] result;
			if (flag2)
			{
				result = decoder.output;
			}
			else
			{
				byte[] array = new byte[decoder.op];
				Array.Copy(decoder.output, 0, array, 0, decoder.op);
				result = array;
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002920 File Offset: 0x00000B20
		public static string encodeToString(byte[] input, int flags)
		{
			string result;
			try
			{
				result = StringHelperClass.NewString(Base64.encode(input, flags), "US-ASCII");
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002958 File Offset: 0x00000B58
		public static string encodeToString(byte[] input, int offset, int len, int flags)
		{
			string result;
			try
			{
				result = StringHelperClass.NewString(Base64.encode(input, offset, len, flags), "US-ASCII");
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002994 File Offset: 0x00000B94
		public static byte[] encode(byte[] input, int flags)
		{
			return Base64.encode(input, 0, input.Length, flags);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000029B4 File Offset: 0x00000BB4
		public static byte[] encode(byte[] input, int offset, int len, int flags)
		{
			Base64.Encoder encoder = new Base64.Encoder(flags, null);
			int num = len / 3 * 4;
			bool do_padding = encoder.do_padding;
			if (do_padding)
			{
				bool flag = len % 3 > 0;
				if (flag)
				{
					num += 4;
				}
			}
			else
			{
				switch (len % 3)
				{
				case 1:
					num += 2;
					break;
				case 2:
					num += 3;
					break;
				}
			}
			bool flag2 = encoder.do_newline && len > 0;
			if (flag2)
			{
				num += ((len - 1) / 57 + 1) * (encoder.do_cr ? 2 : 1);
			}
			encoder.output = new byte[num];
			encoder.process(input, offset, len, true);
			Debug.Assert(encoder.op == num);
			return encoder.output;
		}

		// Token: 0x04000007 RID: 7
		public const int DEFAULT = 0;

		// Token: 0x04000008 RID: 8
		public const int NO_PADDING = 1;

		// Token: 0x04000009 RID: 9
		public const int NO_WRAP = 2;

		// Token: 0x0400000A RID: 10
		public const int CRLF = 4;

		// Token: 0x0400000B RID: 11
		public const int URL_SAFE = 8;

		// Token: 0x0400000C RID: 12
		public const int NO_CLOSE = 16;

		// Token: 0x02000018 RID: 24
		internal abstract class Coder
		{
			// Token: 0x060000E3 RID: 227
			public abstract bool process(byte[] input, int offset, int len, bool finish);

			// Token: 0x060000E4 RID: 228
			public abstract int maxOutputSize(int len);

			// Token: 0x0400005C RID: 92
			public byte[] output;

			// Token: 0x0400005D RID: 93
			public int op;
		}

		// Token: 0x02000019 RID: 25
		internal class Decoder : Base64.Coder
		{
			// Token: 0x060000E6 RID: 230 RVA: 0x00004E12 File Offset: 0x00003012
			public Decoder(int flags, byte[] output)
			{
				this.output = output;
				this.alphabet = (((flags & 8) == 0) ? Base64.Decoder.DECODE : Base64.Decoder.DECODE_WEBSAFE);
				this.state = 0;
				this.value = 0;
			}

			// Token: 0x060000E7 RID: 231 RVA: 0x00004E48 File Offset: 0x00003048
			public override int maxOutputSize(int len)
			{
				return len * 3 / 4 + 10;
			}

			// Token: 0x060000E8 RID: 232 RVA: 0x00004E64 File Offset: 0x00003064
			public override bool process(byte[] input, int offset, int len, bool finish)
			{
				bool flag = this.state == 6;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					int i = offset;
					len += offset;
					int num = this.state;
					int num2 = this.value;
					int num3 = 0;
					byte[] output = this.output;
					int[] array = this.alphabet;
					while (i < len)
					{
						bool flag2 = num == 0;
						if (flag2)
						{
							while (i + 4 <= len && (num2 = (array[(int)(input[i] & 255)] << 18 | array[(int)(input[i + 1] & 255)] << 12 | array[(int)(input[i + 2] & 255)] << 6 | array[(int)(input[i + 3] & 255)])) >= 0)
							{
								output[num3 + 2] = (byte)num2;
								output[num3 + 1] = (byte)(num2 >> 8);
								output[num3] = (byte)(num2 >> 16);
								num3 += 3;
								i += 4;
							}
							bool flag3 = i >= len;
							if (flag3)
							{
								break;
							}
						}
						int num4 = array[(int)(input[i++] & byte.MaxValue)];
						switch (num)
						{
						case 0:
						{
							bool flag4 = num4 >= 0;
							if (flag4)
							{
								num2 = num4;
								num++;
							}
							else
							{
								bool flag5 = num4 != -1;
								if (flag5)
								{
									this.state = 6;
									return false;
								}
							}
							break;
						}
						case 1:
						{
							bool flag6 = num4 >= 0;
							if (flag6)
							{
								num2 = (num2 << 6 | num4);
								num++;
							}
							else
							{
								bool flag7 = num4 != -1;
								if (flag7)
								{
									this.state = 6;
									return false;
								}
							}
							break;
						}
						case 2:
						{
							bool flag8 = num4 >= 0;
							if (flag8)
							{
								num2 = (num2 << 6 | num4);
								num++;
							}
							else
							{
								bool flag9 = num4 == -2;
								if (flag9)
								{
									output[num3++] = (byte)(num2 >> 4);
									num = 4;
								}
								else
								{
									bool flag10 = num4 != -1;
									if (flag10)
									{
										this.state = 6;
										return false;
									}
								}
							}
							break;
						}
						case 3:
						{
							bool flag11 = num4 >= 0;
							if (flag11)
							{
								num2 = (num2 << 6 | num4);
								output[num3 + 2] = (byte)num2;
								output[num3 + 1] = (byte)(num2 >> 8);
								output[num3] = (byte)(num2 >> 16);
								num3 += 3;
								num = 0;
							}
							else
							{
								bool flag12 = num4 == -2;
								if (flag12)
								{
									output[num3 + 1] = (byte)(num2 >> 2);
									output[num3] = (byte)(num2 >> 10);
									num3 += 2;
									num = 5;
								}
								else
								{
									bool flag13 = num4 != -1;
									if (flag13)
									{
										this.state = 6;
										return false;
									}
								}
							}
							break;
						}
						case 4:
						{
							bool flag14 = num4 == -2;
							if (flag14)
							{
								num++;
							}
							else
							{
								bool flag15 = num4 != -1;
								if (flag15)
								{
									this.state = 6;
									return false;
								}
							}
							break;
						}
						case 5:
						{
							bool flag16 = num4 != -1;
							if (flag16)
							{
								this.state = 6;
								return false;
							}
							break;
						}
						}
					}
					bool flag17 = !finish;
					if (flag17)
					{
						this.state = num;
						this.value = num2;
						this.op = num3;
						result = true;
					}
					else
					{
						switch (num)
						{
						case 1:
							this.state = 6;
							return false;
						case 2:
							output[num3++] = (byte)(num2 >> 4);
							break;
						case 3:
							output[num3++] = (byte)(num2 >> 10);
							output[num3++] = (byte)(num2 >> 2);
							break;
						case 4:
							this.state = 6;
							return false;
						}
						this.state = num;
						this.op = num3;
						result = true;
					}
				}
				return result;
			}

			// Token: 0x0400005E RID: 94
			internal static readonly int[] DECODE = new int[]
			{
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				62,
				-1,
				-1,
				-1,
				63,
				52,
				53,
				54,
				55,
				56,
				57,
				58,
				59,
				60,
				61,
				-1,
				-1,
				-1,
				-2,
				-1,
				-1,
				-1,
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				20,
				21,
				22,
				23,
				24,
				25,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				26,
				27,
				28,
				29,
				30,
				31,
				32,
				33,
				34,
				35,
				36,
				37,
				38,
				39,
				40,
				41,
				42,
				43,
				44,
				45,
				46,
				47,
				48,
				49,
				50,
				51,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1
			};

			// Token: 0x0400005F RID: 95
			internal static readonly int[] DECODE_WEBSAFE = new int[]
			{
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				62,
				-1,
				-1,
				52,
				53,
				54,
				55,
				56,
				57,
				58,
				59,
				60,
				61,
				-1,
				-1,
				-1,
				-2,
				-1,
				-1,
				-1,
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				20,
				21,
				22,
				23,
				24,
				25,
				-1,
				-1,
				-1,
				-1,
				63,
				-1,
				26,
				27,
				28,
				29,
				30,
				31,
				32,
				33,
				34,
				35,
				36,
				37,
				38,
				39,
				40,
				41,
				42,
				43,
				44,
				45,
				46,
				47,
				48,
				49,
				50,
				51,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1
			};

			// Token: 0x04000060 RID: 96
			internal const int SKIP = -1;

			// Token: 0x04000061 RID: 97
			internal const int EQUALS = -2;

			// Token: 0x04000062 RID: 98
			internal readonly int[] alphabet;

			// Token: 0x04000063 RID: 99
			internal int state;

			// Token: 0x04000064 RID: 100
			internal int value;
		}

		// Token: 0x0200001A RID: 26
		internal class Encoder : Base64.Coder
		{
			// Token: 0x060000EA RID: 234 RVA: 0x0000522C File Offset: 0x0000342C
			public Encoder(int flags, byte[] output)
			{
				this.output = output;
				this.do_padding = ((flags & 1) == 0);
				this.do_newline = ((flags & 2) == 0);
				this.do_cr = ((flags & 4) != 0);
				this.alphabet = (((flags & 8) == 0) ? Base64.Encoder.ENCODE : Base64.Encoder.ENCODE_WEBSAFE);
				this.tail = new byte[2];
				this.tailLen = 0;
				this.count = (this.do_newline ? 19 : -1);
			}

			// Token: 0x060000EB RID: 235 RVA: 0x000052AC File Offset: 0x000034AC
			public override int maxOutputSize(int len)
			{
				return len * 8 / 5 + 10;
			}

			// Token: 0x060000EC RID: 236 RVA: 0x000052C8 File Offset: 0x000034C8
			public override bool process(byte[] input, int offset, int len, bool finish)
			{
				byte[] array = this.alphabet;
				byte[] output = this.output;
				int num = 0;
				int num2 = this.count;
				int num3 = offset;
				len += offset;
				int num4 = -1;
				switch (this.tailLen)
				{
				case 1:
				{
					bool flag = num3 + 2 <= len;
					if (flag)
					{
						num4 = ((int)(this.tail[0] & byte.MaxValue) << 16 | (int)(input[num3++] & byte.MaxValue) << 8 | (int)(input[num3++] & byte.MaxValue));
						this.tailLen = 0;
					}
					break;
				}
				case 2:
				{
					bool flag2 = num3 + 1 <= len;
					if (flag2)
					{
						num4 = ((int)(this.tail[0] & byte.MaxValue) << 16 | (int)(this.tail[1] & byte.MaxValue) << 8 | (int)(input[num3++] & byte.MaxValue));
						this.tailLen = 0;
					}
					break;
				}
				}
				bool flag3 = num4 != -1;
				if (flag3)
				{
					output[num++] = array[num4 >> 18 & 63];
					output[num++] = array[num4 >> 12 & 63];
					output[num++] = array[num4 >> 6 & 63];
					output[num++] = array[num4 & 63];
					bool flag4 = --num2 == 0;
					if (flag4)
					{
						bool flag5 = this.do_cr;
						if (flag5)
						{
							output[num++] = 13;
						}
						output[num++] = 10;
						num2 = 19;
					}
				}
				while (num3 + 3 <= len)
				{
					num4 = ((int)(input[num3] & byte.MaxValue) << 16 | (int)(input[num3 + 1] & byte.MaxValue) << 8 | (int)(input[num3 + 2] & byte.MaxValue));
					output[num] = array[num4 >> 18 & 63];
					output[num + 1] = array[num4 >> 12 & 63];
					output[num + 2] = array[num4 >> 6 & 63];
					output[num + 3] = array[num4 & 63];
					num3 += 3;
					num += 4;
					bool flag6 = --num2 == 0;
					if (flag6)
					{
						bool flag7 = this.do_cr;
						if (flag7)
						{
							output[num++] = 13;
						}
						output[num++] = 10;
						num2 = 19;
					}
				}
				if (finish)
				{
					bool flag8 = num3 - this.tailLen == len - 1;
					if (flag8)
					{
						int num5 = 0;
						num4 = (int)(((this.tailLen > 0) ? this.tail[num5++] : input[num3++]) & byte.MaxValue) << 4;
						this.tailLen -= num5;
						output[num++] = array[num4 >> 6 & 63];
						output[num++] = array[num4 & 63];
						bool flag9 = this.do_padding;
						if (flag9)
						{
							output[num++] = 61;
							output[num++] = 61;
						}
						bool flag10 = this.do_newline;
						if (flag10)
						{
							bool flag11 = this.do_cr;
							if (flag11)
							{
								output[num++] = 13;
							}
							output[num++] = 10;
						}
					}
					else
					{
						bool flag12 = num3 - this.tailLen == len - 2;
						if (flag12)
						{
							int num6 = 0;
							num4 = ((int)(((this.tailLen > 1) ? this.tail[num6++] : input[num3++]) & byte.MaxValue) << 10 | (int)(((this.tailLen > 0) ? this.tail[num6++] : input[num3++]) & byte.MaxValue) << 2);
							this.tailLen -= num6;
							output[num++] = array[num4 >> 12 & 63];
							output[num++] = array[num4 >> 6 & 63];
							output[num++] = array[num4 & 63];
							bool flag13 = this.do_padding;
							if (flag13)
							{
								output[num++] = 61;
							}
							bool flag14 = this.do_newline;
							if (flag14)
							{
								bool flag15 = this.do_cr;
								if (flag15)
								{
									output[num++] = 13;
								}
								output[num++] = 10;
							}
						}
						else
						{
							bool flag16 = this.do_newline && num > 0 && num2 != 19;
							if (flag16)
							{
								bool flag17 = this.do_cr;
								if (flag17)
								{
									output[num++] = 13;
								}
								output[num++] = 10;
							}
						}
					}
					Debug.Assert(this.tailLen == 0);
					Debug.Assert(num3 == len);
				}
				else
				{
					bool flag18 = num3 == len - 1;
					if (flag18)
					{
						byte[] array2 = this.tail;
						int num7 = this.tailLen;
						this.tailLen = num7 + 1;
						array2[num7] = input[num3];
					}
					else
					{
						bool flag19 = num3 == len - 2;
						if (flag19)
						{
							byte[] array3 = this.tail;
							int num7 = this.tailLen;
							this.tailLen = num7 + 1;
							array3[num7] = input[num3];
							byte[] array4 = this.tail;
							num7 = this.tailLen;
							this.tailLen = num7 + 1;
							array4[num7] = input[num3 + 1];
						}
					}
				}
				this.op = num;
				this.count = num2;
				return true;
			}

			// Token: 0x04000065 RID: 101
			public const int LINE_GROUPS = 19;

			// Token: 0x04000066 RID: 102
			internal static readonly byte[] ENCODE = new byte[]
			{
				65,
				66,
				67,
				68,
				69,
				70,
				71,
				72,
				73,
				74,
				75,
				76,
				77,
				78,
				79,
				80,
				81,
				82,
				83,
				84,
				85,
				86,
				87,
				88,
				89,
				90,
				97,
				98,
				99,
				100,
				101,
				102,
				103,
				104,
				105,
				106,
				107,
				108,
				109,
				110,
				111,
				112,
				113,
				114,
				115,
				116,
				117,
				118,
				119,
				120,
				121,
				122,
				48,
				49,
				50,
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				43,
				47
			};

			// Token: 0x04000067 RID: 103
			internal static readonly byte[] ENCODE_WEBSAFE = new byte[]
			{
				65,
				66,
				67,
				68,
				69,
				70,
				71,
				72,
				73,
				74,
				75,
				76,
				77,
				78,
				79,
				80,
				81,
				82,
				83,
				84,
				85,
				86,
				87,
				88,
				89,
				90,
				97,
				98,
				99,
				100,
				101,
				102,
				103,
				104,
				105,
				106,
				107,
				108,
				109,
				110,
				111,
				112,
				113,
				114,
				115,
				116,
				117,
				118,
				119,
				120,
				121,
				122,
				48,
				49,
				50,
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				45,
				95
			};

			// Token: 0x04000068 RID: 104
			public readonly bool do_padding;

			// Token: 0x04000069 RID: 105
			public readonly bool do_newline;

			// Token: 0x0400006A RID: 106
			public readonly bool do_cr;

			// Token: 0x0400006B RID: 107
			internal readonly byte[] tail;

			// Token: 0x0400006C RID: 108
			internal readonly byte[] alphabet;

			// Token: 0x0400006D RID: 109
			internal int tailLen;

			// Token: 0x0400006E RID: 110
			internal int count;
		}
	}
}
