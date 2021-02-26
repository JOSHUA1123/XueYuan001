using System;

namespace VTemplate.Engine
{
	/// <summary>
	/// 模板文档的配置参数
	/// </summary>
	// Token: 0x02000011 RID: 17
	public class TemplateDocumentConfig
	{
		/// <summary>
		/// 实例化默认的配置.标签的开放模式为简单、不压缩文本
		/// </summary>
		// Token: 0x060000B2 RID: 178 RVA: 0x0000439D File Offset: 0x0000259D
		public TemplateDocumentConfig()
		{
			this.TagOpenMode = TagOpenMode.Simple;
			this.CompressText = false;
			this.CompatibleMode = false;
		}

		/// <summary>
		/// 根据参数实例化
		/// </summary>
		/// <param name="tagOpenMode">标签的开放模式</param>
		// Token: 0x060000B3 RID: 179 RVA: 0x000043BA File Offset: 0x000025BA
		public TemplateDocumentConfig(TagOpenMode tagOpenMode)
		{
			this.TagOpenMode = tagOpenMode;
			this.CompressText = false;
			this.CompatibleMode = false;
		}

		/// <summary>
		/// 根据参数实例化
		/// </summary>
		/// <param name="tagOpenMode">标签的开放模式</param>
		/// <param name="compressText">是否压缩文本</param>
		// Token: 0x060000B4 RID: 180 RVA: 0x000043D7 File Offset: 0x000025D7
		public TemplateDocumentConfig(TagOpenMode tagOpenMode, bool compressText)
		{
			this.TagOpenMode = tagOpenMode;
			this.CompressText = compressText;
			this.CompatibleMode = false;
		}

		/// <summary>
		/// 根据参数实例化
		/// </summary>
		/// <param name="tagOpenMode">标签的开放模式</param>
		/// <param name="compressText">是否压缩文本</param>
		/// <param name="compatibleMode">是否采用兼容模式</param>
		// Token: 0x060000B5 RID: 181 RVA: 0x000043F4 File Offset: 0x000025F4
		public TemplateDocumentConfig(TagOpenMode tagOpenMode, bool compressText, bool compatibleMode)
		{
			this.TagOpenMode = tagOpenMode;
			this.CompressText = compressText;
			this.CompatibleMode = compatibleMode;
		}

		/// <summary>
		/// 标签的开放模式
		/// </summary>
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004411 File Offset: 0x00002611
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00004419 File Offset: 0x00002619
		public TagOpenMode TagOpenMode { get; private set; }

		/// <summary>
		/// 是否压缩文本
		/// </summary>
		/// <remarks>
		/// 压缩文本.即是删除换行符和无用的空格(换行符前后的空格)
		/// </remarks>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004422 File Offset: 0x00002622
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x0000442A File Offset: 0x0000262A
		public bool CompressText { get; private set; }

		/// <summary>
		/// 兼容模式
		/// </summary>
		/// <remarks>如果采用兼容模式.则&lt;vt:foreach&gt;标签的from属性与&lt;vt:expression&gt;标签的args属性等可以不以$字符开头定义变量表达式</remarks>
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004433 File Offset: 0x00002633
		// (set) Token: 0x060000BB RID: 187 RVA: 0x0000443B File Offset: 0x0000263B
		public bool CompatibleMode { get; private set; }

		/// <summary>
		/// 标签的开放模式为简单,不压缩文本
		/// </summary>
		// Token: 0x04000028 RID: 40
		public static readonly TemplateDocumentConfig Default = new TemplateDocumentConfig();

		/// <summary>
		/// 标签的开放模式为完全,不压缩文本
		/// </summary>
		// Token: 0x04000029 RID: 41
		public static readonly TemplateDocumentConfig Full = new TemplateDocumentConfig(TagOpenMode.Full, false);

		/// <summary>
		/// 标签的开放模式为简单,压缩文本
		/// </summary>
		// Token: 0x0400002A RID: 42
		public static readonly TemplateDocumentConfig Compress = new TemplateDocumentConfig(TagOpenMode.Simple, true);

		/// <summary>
		/// 标签的开放模式为简单,不压缩文本，且采用兼容模式
		/// </summary>
		// Token: 0x0400002B RID: 43
		public static readonly TemplateDocumentConfig Compatible = new TemplateDocumentConfig(TagOpenMode.Simple, false, true);
	}
}
