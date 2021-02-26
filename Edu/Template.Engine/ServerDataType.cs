using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 服务器数据类型
	/// </summary>
	// Token: 0x02000030 RID: 48
	public enum ServerDataType
	{
		/// <summary>
		/// 未知
		/// </summary>
		// Token: 0x0400007B RID: 123
		Unknown,
		/// <summary>
		/// 服务器当前时间
		/// </summary>
		// Token: 0x0400007C RID: 124
		Time,
		/// <summary>
		/// 0~1之间的随机数
		/// </summary>
		// Token: 0x0400007D RID: 125
		Random,
		/// <summary>
		/// 服务器当前上下文的HttpApplicationState对象.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x0400007E RID: 126
		Application,
		/// <summary>
		/// 服务器当前上下文的HttpSessionState对象.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x0400007F RID: 127
		Session,
		/// <summary>
		/// 服务器当前上下文的缓存对象
		/// </summary>
		// Token: 0x04000080 RID: 128
		Cache,
		/// <summary>
		/// 服务器当前上下文的Request.QueryString数据集合.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x04000081 RID: 129
		QueryString,
		/// <summary>
		/// 服务器当前上下文的Request.Form数据集合.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x04000082 RID: 130
		Form,
		/// <summary>
		/// 服务器当前上下文的Request.Cookie数据集合.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x04000083 RID: 131
		Cookie,
		/// <summary>
		/// 服务器当前上下文的Request.ServerVariables数据集合.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x04000084 RID: 132
		ServerVariables,
		/// <summary>
		/// 服务器当前上下文的Request.Params数据集合.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x04000085 RID: 133
		RequestParams,
		/// <summary>
		/// 服务器当前上下文的HttpRequest对象.如果模板引擎不在Web程序上使用则无效
		/// </summary>
		// Token: 0x04000086 RID: 134
		Request,
		/// <summary>
		/// 服务器系统平台
		/// </summary>
		// Token: 0x04000087 RID: 135
		Environment,
		/// <summary>
		/// 获取当前应用程序的AppSettings配置节点参数
		/// </summary>
		// Token: 0x04000088 RID: 136
		AppSetting
	}
}
