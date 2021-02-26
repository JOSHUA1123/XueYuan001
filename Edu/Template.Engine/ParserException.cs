using System;
using System.Drawing;

namespace VTemplate.Engine
{
	/// <summary>
	/// 解析模板时的错误
	/// </summary>
	// Token: 0x02000017 RID: 23
	public class ParserException : Exception
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="message">描述信息</param>
		// Token: 0x06000109 RID: 265 RVA: 0x00006130 File Offset: 0x00004330
		public ParserException(string message) : base(message)
		{
			this.HaveLineAndColumnNumber = false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="p">行号列号(x = 列号, y = 行号)</param>
		/// <param name="text">模板文本数据</param>
		/// <param name="message">描述信息</param>
		// Token: 0x0600010A RID: 266 RVA: 0x00006140 File Offset: 0x00004340
		public ParserException(Point p, string text, string message) : this(p.Y, p.X, text, message)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="line">所在行号</param>
		/// <param name="column">所在列</param>
		/// <param name="text">模板文本数据</param>
		/// <param name="message">描述信息</param>
		// Token: 0x0600010B RID: 267 RVA: 0x00006158 File Offset: 0x00004358
		public ParserException(int line, int column, string text, string message) : base(string.Format("在解析(行{0}:列{1})的模板文本字符\"{2}\"时,发生错误:{3}", new object[]
		{
			line,
			column,
			text,
			message
		}))
		{
			this.HaveLineAndColumnNumber = true;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName">模板文件</param>
		/// <param name="p">行号列号(x = 列号, y = 行号)</param>
		/// <param name="text">模板文本数据</param>
		/// <param name="message">描述信息</param>
		// Token: 0x0600010C RID: 268 RVA: 0x0000619F File Offset: 0x0000439F
		public ParserException(string fileName, Point p, string text, string message) : this(fileName, p.Y, p.X, text, message)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName">模板文件</param>
		/// <param name="line">所在行号</param>
		/// <param name="column">所在列</param>
		/// <param name="text">模板文本数据</param>
		/// <param name="message">描述信息</param>
		// Token: 0x0600010D RID: 269 RVA: 0x000061BC File Offset: 0x000043BC
		public ParserException(string fileName, int line, int column, string text, string message) : base(string.Format("在解析文件\"{0}\"(行{1}:列{2})的模板文本字符\"{3}\"时,发生错误:{4}", new object[]
		{
			fileName,
			line,
			column,
			text,
			message
		}))
		{
			this.HaveLineAndColumnNumber = true;
		}

		/// <summary>
		/// 是否包含行号与列号
		/// </summary>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006208 File Offset: 0x00004408
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00006210 File Offset: 0x00004410
		public bool HaveLineAndColumnNumber { get; private set; }
	}
}
