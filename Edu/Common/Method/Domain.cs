using System;
using System.Configuration;

namespace Common.Method
{
	// Token: 0x02000041 RID: 65
	public class Domain
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00009B9F File Offset: 0x00007D9F
		public Domain()
		{
			this._currtentDomain = Server.Domain;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009BB2 File Offset: 0x00007DB2
		public Domain(string current)
		{
			this._currtentDomain = current;
		}

		/// <summary>
		/// 当前站点的域名
		/// </summary>
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00009BC1 File Offset: 0x00007DC1
		public string CurrtentDomain
		{
			get
			{
				return this._currtentDomain;
			}
		}

		/// <summary>
		/// 当前站点的二级域名
		/// </summary>
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00013980 File Offset: 0x00011B80
		public string TwoDomain
		{
			get
			{
				if (this._currtentDomain.IndexOf(".") < 0)
				{
					return this._currtentDomain;
				}
				if (this._currtentDomain.Length <= this.MainName.Length)
				{
					return this._currtentDomain;
				}
				string text = this._currtentDomain.Substring(this._currtentDomain.Length - this.MainName.Length);
				if (text != this.MainName)
				{
					return this._currtentDomain;
				}
				text = this._currtentDomain.Substring(0, this._currtentDomain.Length - this.MainName.Length - 1);
				if (text.IndexOf(".") < 0)
				{
					return text;
				}
				return text.Substring(text.LastIndexOf(".") + 1);
			}
		}

		/// <summary>
		/// 当前站点的主域（来自web.config设置）
		/// </summary>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00013A4C File Offset: 0x00011C4C
		public string MainName
		{
			get
			{
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
				return connectionStringSettings.Name;
			}
		}

		// Token: 0x0400009A RID: 154
		private string _currtentDomain;
	}
}
