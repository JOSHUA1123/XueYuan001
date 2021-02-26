using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Parameters.Authorization
{
	/// <summary>
	/// 版本的限制项
	/// </summary>
	// Token: 0x02000052 RID: 82
	public class VersionLimit
	{
		/// <summary>
		/// 学员
		/// </summary>
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00009FC8 File Offset: 0x000081C8
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00009FD0 File Offset: 0x000081D0
		public int Accounts
		{
			get
			{
				return this._accounts;
			}
			internal set
			{
				this._accounts = value;
			}
		}

		/// <summary>
		/// 教师
		/// </summary>
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00009FD9 File Offset: 0x000081D9
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00009FE1 File Offset: 0x000081E1
		public int Teacher
		{
			get
			{
				return this._teacher;
			}
			internal set
			{
				this._teacher = value;
			}
		}

		/// <summary>
		/// 专业
		/// </summary>
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00009FEA File Offset: 0x000081EA
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00009FF2 File Offset: 0x000081F2
		public int Subject
		{
			get
			{
				return this._subject;
			}
			internal set
			{
				this._subject = value;
			}
		}

		/// <summary>
		/// 课程，或科目
		/// </summary>
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00009FFB File Offset: 0x000081FB
		// (set) Token: 0x06000251 RID: 593 RVA: 0x0000A003 File Offset: 0x00008203
		public int Course
		{
			get
			{
				return this._course;
			}
			internal set
			{
				this._course = value;
			}
		}

		/// <summary>
		/// 试题
		/// </summary>
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000A00C File Offset: 0x0000820C
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000A014 File Offset: 0x00008214
		public int Questions
		{
			get
			{
				return this._questions;
			}
			internal set
			{
				this._questions = value;
			}
		}

		/// <summary>
		/// 试卷
		/// </summary>
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000A01D File Offset: 0x0000821D
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000A025 File Offset: 0x00008225
		public int TestPaper
		{
			get
			{
				return this._testPaper;
			}
			internal set
			{
				this._testPaper = value;
			}
		}

		/// <summary>
		/// 文章，或资讯
		/// </summary>
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000A02E File Offset: 0x0000822E
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000A036 File Offset: 0x00008236
		public int Article
		{
			get
			{
				return this._article;
			}
			internal set
			{
				this._article = value;
			}
		}

		/// <summary>
		/// 知识
		/// </summary>
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000A03F File Offset: 0x0000823F
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000A047 File Offset: 0x00008247
		public int Knowledge
		{
			get
			{
				return this._knowledge;
			}
			internal set
			{
				this._knowledge = value;
			}
		}

		/// <summary>
		/// 公告
		/// </summary>
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000A050 File Offset: 0x00008250
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000A058 File Offset: 0x00008258
		public int Notice
		{
			get
			{
				return this._notice;
			}
			internal set
			{
				this._notice = value;
			}
		}

		/// <summary>
		/// 机构
		/// </summary>
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000A061 File Offset: 0x00008261
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000A069 File Offset: 0x00008269
		public int Organization
		{
			get
			{
				return this._organization;
			}
			internal set
			{
				this._organization = value;
			}
		}

		/// <summary>
		/// 通过字段获取限制数量
		/// </summary>
		/// <param name="field"></param>
		/// <returns>返回值为-1，则不限制数量</returns>
		// Token: 0x0600025E RID: 606 RVA: 0x0001700C File Offset: 0x0001520C
		public int GetLimitNumber(string field)
		{
			Type type = base.GetType();
			PropertyInfo property = type.GetProperty(field);
			if (property == null)
			{
				return -1;
			}
			return (int)property.GetValue(this, null);
		}

		/// <summary>
		/// 当前版本各限制项的数据集
		/// </summary>
		// Token: 0x0600025F RID: 607 RVA: 0x00017044 File Offset: 0x00015244
		public static IDictionary<string, int> DataItems(VersionLimit limit)
		{
			IDictionary<string, int> dictionary = new Dictionary<string, int>();
			Type type = limit.GetType();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				bool visible = Entity.Get.GetVisible(propertyInfo.Name);
				if (visible)
				{
					string key = Entity.Get[propertyInfo.Name];
					int value = (int)propertyInfo.GetValue(limit, null);
					dictionary.Add(key, value);
				}
			}
			return dictionary;
		}

		/// <summary>
		/// 限定的根域名
		/// </summary>
		// Token: 0x040000C4 RID: 196
		public static string[] Domain = new string[]
		{
			"net",
			"com",
			"org",
			"cn",
			"me",
			"site",
			"co",
			"cc",
			"info",
			"net.cn",
			"com.cn",
			"org.cn"
		};

		// Token: 0x040000C5 RID: 197
		private int _accounts;

		// Token: 0x040000C6 RID: 198
		private int _teacher;

		// Token: 0x040000C7 RID: 199
		private int _subject;

		// Token: 0x040000C8 RID: 200
		private int _course;

		// Token: 0x040000C9 RID: 201
		private int _questions;

		// Token: 0x040000CA RID: 202
		private int _testPaper;

		// Token: 0x040000CB RID: 203
		private int _article;

		// Token: 0x040000CC RID: 204
		private int _knowledge;

		// Token: 0x040000CD RID: 205
		private int _notice;

		// Token: 0x040000CE RID: 206
		private int _organization = 5;
	}
}
