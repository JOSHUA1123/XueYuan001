using System;

namespace SMS
{
	// Token: 0x02000011 RID: 17
	public class SmsItem
	{
		/// <summary>
		/// 短信发送帐号
		/// </summary>
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002EC0 File Offset: 0x000010C0
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002EC8 File Offset: 0x000010C8
		public string User
		{
			get
			{
				return this.user;
			}
			set
			{
				this.user = value;
			}
		}

		/// <summary>
		/// 短信发送的密码
		/// </summary>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002ED1 File Offset: 0x000010D1
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002ED9 File Offset: 0x000010D9
		public string Password
		{
			get
			{
				return this.pw;
			}
			set
			{
				this.pw = value;
			}
		}

		/// <summary>
		/// 短信接口的实现类
		/// </summary>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002EE2 File Offset: 0x000010E2
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002EEA File Offset: 0x000010EA
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002EF3 File Offset: 0x000010F3
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002EFB File Offset: 0x000010FB
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>
		/// 备注信息
		/// </summary>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002F04 File Offset: 0x00001104
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00002F0C File Offset: 0x0000110C
		public string Remarks
		{
			get
			{
				return this.remarks;
			}
			set
			{
				this.remarks = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002F15 File Offset: 0x00001115
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002F1D File Offset: 0x0000111D
		public string Domain
		{
			get
			{
				return this._domain;
			}
			set
			{
				this._domain = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002F26 File Offset: 0x00001126
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002F2E File Offset: 0x0000112E
		public string RegisterUrl
		{
			get
			{
				return this._regurl;
			}
			set
			{
				this._regurl = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002F37 File Offset: 0x00001137
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002F3F File Offset: 0x0000113F
		public string PayUrl
		{
			get
			{
				return this._payurl;
			}
			set
			{
				this._payurl = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002F48 File Offset: 0x00001148
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002F50 File Offset: 0x00001150
		public bool IsUse
		{
			get
			{
				return this._isUse;
			}
			set
			{
				this._isUse = value;
			}
		}

		/// <summary>
		/// 是否当前采用的短信平台
		/// </summary>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002F59 File Offset: 0x00001159
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002F61 File Offset: 0x00001161
		public bool IsCurrent
		{
			get
			{
				return this.isCurrent;
			}
			set
			{
				this.isCurrent = value;
			}
		}

		// Token: 0x04000013 RID: 19
		private string user;

		// Token: 0x04000014 RID: 20
		private string pw;

		// Token: 0x04000015 RID: 21
		private string type;

		// Token: 0x04000016 RID: 22
		private string name;

		// Token: 0x04000017 RID: 23
		private string remarks;

		// Token: 0x04000018 RID: 24
		private string _domain;

		// Token: 0x04000019 RID: 25
		private string _regurl;

		// Token: 0x0400001A RID: 26
		private string _payurl;

		// Token: 0x0400001B RID: 27
		private bool _isUse;

		// Token: 0x0400001C RID: 28
		private bool isCurrent;
	}
}
