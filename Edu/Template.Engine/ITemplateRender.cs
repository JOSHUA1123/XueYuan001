using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 模板块数据的解析处理接口
	/// </summary>
	// Token: 0x02000029 RID: 41
	public interface ITemplateRender
	{
		/// <summary>
		/// 预处理解析模板数据
		/// </summary>
		/// <param name="template"></param>
		// Token: 0x06000216 RID: 534
		void PreRender(Template template);
	}
}
