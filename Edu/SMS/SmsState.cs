using System;

namespace SMS
{
	/// <summary>
	/// 短信的发送状态
	/// </summary>
	// Token: 0x02000013 RID: 19
	public class SmsState
	{
		/// <summary>
		/// 是否成功
		/// </summary>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000031FF File Offset: 0x000013FF
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00003207 File Offset: 0x00001407
		public bool Success
		{
			get
			{
				return this._success;
			}
			set
			{
				this._success = value;
			}
		}

		/// <summary>
		/// 发送短信的返回结果
		/// </summary>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003210 File Offset: 0x00001410
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003218 File Offset: 0x00001418
		public string Result
		{
			get
			{
				return this._result;
			}
			set
			{
				this._result = value;
			}
		}

		/// <summary>
		/// 发送后的返回代码，一般0为发送成功
		/// </summary>
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003221 File Offset: 0x00001421
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003229 File Offset: 0x00001429
		public int Code
		{
			get
			{
				return this._code;
			}
			set
			{
				this._code = value;
			}
		}

		/// <summary>
		/// 发送短信的详细返回信息
		/// </summary>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003232 File Offset: 0x00001432
		// (set) Token: 0x06000089 RID: 137 RVA: 0x0000323A File Offset: 0x0000143A
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		/// <summary>
		/// 如果发送失败，失败列表
		/// </summary>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003243 File Offset: 0x00001443
		// (set) Token: 0x0600008B RID: 139 RVA: 0x0000324B File Offset: 0x0000144B
		public string FailList
		{
			get
			{
				return this._failList;
			}
			set
			{
				this._failList = value;
			}
		}

		// Token: 0x04000020 RID: 32
		private bool _success;

		// Token: 0x04000021 RID: 33
		private string _result = "";

		// Token: 0x04000022 RID: 34
		private int _code = -1;

		// Token: 0x04000023 RID: 35
		private string _description = "";

		// Token: 0x04000024 RID: 36
		private string _failList = "";
	}
}
