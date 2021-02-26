using System;

namespace SMS
{
	// Token: 0x0200000D RID: 13
	public interface ISMS
	{
		/// <summary>
		/// 短信平台的管理项
		/// </summary>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000044 RID: 68
		// (set) Token: 0x06000045 RID: 69
		SmsItem Current { get; set; }

		/// <summary>
		/// 用户的账号
		/// </summary>
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000046 RID: 70
		// (set) Token: 0x06000047 RID: 71
		string User { get; set; }

		/// <summary>
		/// 用户的密码
		/// </summary>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000048 RID: 72
		// (set) Token: 0x06000049 RID: 73
		string Password { get; set; }

		/// <summary>
		/// 发送短信
		/// </summary>
		/// <param name="mobiles"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		// Token: 0x0600004A RID: 74
		SmsState Send(string mobiles, string context);

		/// <summary>
		/// 定时发送短信
		/// </summary>
		/// <param name="mobiles"></param>
		/// <param name="context"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		// Token: 0x0600004B RID: 75
		SmsState Send(string mobiles, string context, DateTime time);

		/// <summary>
		/// 查询剩余的短信条数
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600004C RID: 76
		int Query();

		/// <summary>
		/// 查询剩余的短信条数
		/// </summary>
		/// <param name="user">账号</param>
		/// <param name="pw">密码</param>
		/// <returns></returns>
		// Token: 0x0600004D RID: 77
		int Query(string user, string pw);

		/// <summary>
		/// 接收回发的短信
		/// </summary>
		/// <param name="from">开始接收的时间</param>
		/// <param name="readflag">是否已读，0:未读短信，1:所有短信</param>
		/// <returns></returns>
		// Token: 0x0600004E RID: 78
		string ReceiveSMS(DateTime from, string readflag);
	}
}
