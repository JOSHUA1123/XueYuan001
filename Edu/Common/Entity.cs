using System;

namespace Common
{
	/// <summary>
	/// 系统中的实体说明
	/// </summary>
	// Token: 0x02000096 RID: 150
	public class Entity
	{
		/// <summary>
		/// 获取参数
		/// </summary>
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000AC62 File Offset: 0x00008E62
		public static Entity Get
		{
			get
			{
				return Entity._get;
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000925C File Offset: 0x0000745C
		private Entity()
		{
		}

		/// <summary>
		/// 通过键获取值的名称
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		// Token: 0x17000141 RID: 321
		public string this[string key]
		{
			get
			{
				string text = PlatformInfoHandler.Node("Entity").ItemValue(key);
				if (string.IsNullOrWhiteSpace(text))
				{
					return key;
				}
				return text;
			}
		}

		/// <summary>
		/// 当前实体是否显示
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		// Token: 0x06000409 RID: 1033 RVA: 0x0001FA10 File Offset: 0x0001DC10
		public bool GetVisible(string key)
		{
			bool result;
			try
			{
				if (PlatformInfoHandler.Node("Entity")[key] == null)
				{
					result = true;
				}
				else
				{
					string attr = PlatformInfoHandler.Node("Entity")[key].GetAttr("visible");
					if (string.IsNullOrWhiteSpace(attr))
					{
						result = true;
					}
					else
					{
						result = !(attr.Trim().ToLower() == "false");
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x04000190 RID: 400
		private static readonly Entity _get = new Entity();
	}
}
