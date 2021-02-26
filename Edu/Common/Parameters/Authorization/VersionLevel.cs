using System;
using System.Data;
using System.Reflection;

namespace Common.Parameters.Authorization
{
	/// <summary>
	/// 当前版本的等级
	/// </summary>
	// Token: 0x020000B5 RID: 181
	public class VersionLevel
	{
		/// <summary>
		/// 版本等级信息
		/// </summary>
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000B2EC File Offset: 0x000094EC
		public static string[] Level
		{
			get
			{
				return VersionLevel._level;
			}
		}

		/// <summary>
		/// 各版本的限制项
		/// </summary>
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x0000B2F3 File Offset: 0x000094F3
		public static VersionLimit[] Limit
		{
			get
			{
				return VersionLevel._limit;
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000925C File Offset: 0x0000745C
		private VersionLevel()
		{
		}

		/// <summary>
		/// 获取版本名称
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		// Token: 0x060004DA RID: 1242 RVA: 0x0000B2FA File Offset: 0x000094FA
		public static string GetLevelName(int level)
		{
			if (level < 0 || level > VersionLevel._level.Length - 1)
			{
				level = 0;
			}
			return VersionLevel._level[level];
		}

		/// <summary>
		/// 获取当前版本等级的各项限制
		/// </summary>
		/// <param name="level">版本等级</param>
		/// <returns></returns>
		// Token: 0x060004DB RID: 1243 RVA: 0x0000B316 File Offset: 0x00009516
		public static VersionLimit GetLevelLimit(int level)
		{
			if (level < 0 || level > VersionLevel._limit.Length - 1)
			{
				level = 0;
			}
			return VersionLevel._limit[level];
		}

		/// <summary>
		/// 所有版本等级的各项数据组成的表
		/// </summary>
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00023A90 File Offset: 0x00021C90
		public static DataTable LevelTable
		{
			get
			{
				DataTable dataTable = new DataTable("DataBase");
				dataTable.Columns.Add(new DataColumn("版本名称", Type.GetType("System.String")));
				VersionLimit levelLimit = VersionLevel.GetLevelLimit(0);
				Type type = levelLimit.GetType();
				foreach (PropertyInfo propertyInfo in type.GetProperties())
				{
					bool visible = Entity.Get.GetVisible(propertyInfo.Name);
					if (visible)
					{
						string columnName = Entity.Get[propertyInfo.Name];
						dataTable.Columns.Add(new DataColumn(columnName, Type.GetType("System.String")));
					}
				}
				for (int j = 0; j < VersionLevel.Level.Length; j++)
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["版本名称"] = VersionLevel.Level[j];
					VersionLimit levelLimit2 = VersionLevel.GetLevelLimit(j);
					foreach (PropertyInfo propertyInfo2 in levelLimit2.GetType().GetProperties())
					{
						bool visible2 = Entity.Get.GetVisible(propertyInfo2.Name);
						if (visible2)
						{
							string columnName2 = Entity.Get[propertyInfo2.Name];
							int num = (int)propertyInfo2.GetValue(levelLimit2, null);
							dataRow[columnName2] = num;
						}
					}
					dataTable.Rows.Add(dataRow);
				}
				return dataTable;
			}
		}

		// Token: 0x040001F6 RID: 502
		private static string[] _level = new string[]
		{
			"免费版",
			"普及版",
			"基础版",
			"标准版",
			"专业版",
			"集团版",
			"至尊版"
		};

		// Token: 0x040001F7 RID: 503
		private static VersionLimit[] _limit = new VersionLimit[]
		{
			new VersionLimit
			{
				Accounts = 60,
				Teacher = 20,
				Subject = 100,
				Course = 200,
				Questions = 2000,
				TestPaper = 20,
				Article = 1000,
				Knowledge = 1000,
				Notice = 500,
				Organization = 2
			},
			new VersionLimit
			{
				Accounts = 300,
				Teacher = 20,
				Subject = 100,
				Course = 200,
				Questions = 5000,
				TestPaper = 100,
				Article = 2000,
				Knowledge = 2000,
				Notice = 1000,
				Organization = 2
			},
			new VersionLimit
			{
				Accounts = 1000,
				Teacher = 50,
				Subject = 180,
				Course = 400,
				Questions = 20000,
				TestPaper = 400,
				Article = 8000,
				Knowledge = 6000,
				Notice = 3000,
				Organization = 2
			},
			new VersionLimit
			{
				Accounts = 2000,
				Teacher = 100,
				Subject = 200,
				Course = 800,
				Questions = 60000,
				TestPaper = 800,
				Article = 0,
				Knowledge = 0,
				Notice = 0,
				Organization = 3
			},
			new VersionLimit
			{
				Accounts = 5000,
				Teacher = 300,
				Subject = 500,
				Course = 2000,
				Questions = 300000,
				TestPaper = 2000,
				Article = 0,
				Knowledge = 0,
				Notice = 0,
				Organization = 4
			},
			new VersionLimit
			{
				Accounts = 10000,
				Teacher = 800,
				Subject = 1000,
				Course = 6000,
				Questions = 1000000,
				TestPaper = 6000,
				Article = 0,
				Knowledge = 0,
				Notice = 0,
				Organization = 4
			},
			new VersionLimit
			{
				Organization = 6
			}
		};
	}
}
