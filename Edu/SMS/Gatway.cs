using System;

namespace SMS
{
	// Token: 0x0200000F RID: 15
	public class Gatway
	{
		/// <summary>
		/// 发送短信的入口，实现IOC依赖反转
		/// </summary>
		/// <returns></returns>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002D50 File Offset: 0x00000F50
		public static ISMS Service
		{
			get
			{
				string type = Config.Current.Type;
				string typeName = type.Substring(0, type.IndexOf(","));
				type.Substring(type.IndexOf(",") + 1);
				Type type2 = Type.GetType(typeName);
				if (type2 == null)
				{
					return null;
				}
				object obj = Activator.CreateInstance(type2);
				ISMS isms = (ISMS)obj;
				isms.Current = Config.Current;
				return isms;
			}
		}

		/// <summary>
		/// 设置当前使用的短信平台
		/// </summary>
		/// <param name="name">短信平台名称，来自于web.config中的配置</param>
		// Token: 0x0600005C RID: 92 RVA: 0x00002DBE File Offset: 0x00000FBE
		public static void GetCurrentPlate(string name)
		{
			Config.Singleton.CurrentName = name;
		}

		/// <summary>
		/// 根据短信平台的名称，获取短信平台接口实例
		/// </summary>
		/// <param name="remark">短信接口备注名，来自于webconfig中的设置项</param>
		/// <returns></returns>
		// Token: 0x0600005D RID: 93 RVA: 0x00002DCC File Offset: 0x00000FCC
		public static ISMS GetService(string remark)
		{
			SmsItem smsItem = Config.Current;
			foreach (SmsItem smsItem2 in Config.SmsItems)
			{
				if (smsItem2.Remarks == remark)
				{
					smsItem = smsItem2;
					break;
				}
			}
			string type = smsItem.Type;
			string typeName = type.Substring(0, type.IndexOf(","));
			type.Substring(type.IndexOf(",") + 1);
			Type type2 = Type.GetType(typeName);
			if (type2 == null)
			{
				return null;
			}
			object obj = Activator.CreateInstance(type2);
			ISMS isms = (ISMS)obj;
			isms.Current = smsItem;
			return isms;
		}
	}
}
