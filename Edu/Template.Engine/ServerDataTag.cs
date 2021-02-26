using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace VTemplate.Engine
{
	/// <summary>
	/// 服务器数据标签,.如: &lt;vt:serverdata var="request" type="request" /&gt;
	/// </summary>
	// Token: 0x02000031 RID: 49
	public class ServerDataTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x06000261 RID: 609 RVA: 0x0000AB44 File Offset: 0x00008D44
		internal ServerDataTag(Template ownerTemplate) : base(ownerTemplate)
		{
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000AB4D File Offset: 0x00008D4D
		public override string TagName
		{
			get
			{
				return "serverdata";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000AB54 File Offset: 0x00008D54
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 数据类型.
		/// </summary>
		/// <remarks></remarks>
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000AB57 File Offset: 0x00008D57
		public new Attribute Type
		{
			get
			{
				return base.Attributes["Type"];
			}
		}

		/// <summary>
		/// 存储表达式结果的变量
		/// </summary>
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000AB69 File Offset: 0x00008D69
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000AB71 File Offset: 0x00008D71
		public VariableIdentity Variable { get; protected set; }

		/// <summary>
		/// 数据值
		/// </summary>
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000AB7A File Offset: 0x00008D7A
		public Attribute Item
		{
			get
			{
				return base.Attributes["Item"];
			}
		}

		/// <summary>
		/// 是否输出此标签的结果值
		/// </summary>
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000AB8C File Offset: 0x00008D8C
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000AB94 File Offset: 0x00008D94
		public bool Output { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x0600026A RID: 618 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "var")
				{
					this.Variable = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
					return;
				}
				if (!(name == "output"))
				{
					return;
				}
				this.Output = Utility.ConverToBoolean(item.Text);
			}
		}

		/// <summary>
		/// 开始解析标签数据
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="container">标签的容器</param>
		/// <param name="tagStack">标签堆栈</param>
		/// <param name="text"></param>
		/// <param name="match"></param>
		/// <param name="isClosedTag">是否闭合标签</param>
		/// <returns>如果需要继续处理EndTag则返回true.否则请返回false</returns>
		// Token: 0x0600026B RID: 619 RVA: 0x0000ABF8 File Offset: 0x00008DF8
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Variable == null && !this.Output)
			{
				throw new ParserException(string.Format("{0}标签中如果未定义Output属性为true则必须定义var属性", this.TagName));
			}
			if (this.Type == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少type属性或type属性值未知", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600026C RID: 620 RVA: 0x0000AC58 File Offset: 0x00008E58
		internal override Element Clone(Template ownerTemplate)
		{
			ServerDataTag serverDataTag = new ServerDataTag(ownerTemplate);
			this.CopyTo(serverDataTag);
			serverDataTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			serverDataTag.Output = this.Output;
			return serverDataTag;
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x0600026D RID: 621 RVA: 0x0000ACA0 File Offset: 0x00008EA0
		protected override void RenderTagData(TextWriter writer)
		{
			object serverData = this.GetServerData();
			if (this.Variable != null)
			{
				this.Variable.Value = serverData;
			}
			if (this.Output && serverData != null)
			{
				writer.Write(serverData);
			}
			base.RenderTagData(writer);
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600026E RID: 622 RVA: 0x0000ACE4 File Offset: 0x00008EE4
		private object GetServerData()
		{
			switch ((ServerDataType)Utility.ConvertTo(this.Type.GetTextValue(), typeof(ServerDataType)))
			{
			case ServerDataType.Time:
				return this.GetServerTime();
			case ServerDataType.Random:
				return ServerDataTag.random.NextDouble();
			case ServerDataType.Application:
				return this.GetApplicationItem();
			case ServerDataType.Session:
				return this.GetSessionItem();
			case ServerDataType.Cache:
				return this.GetCacheItem();
			case ServerDataType.QueryString:
				return this.GetRequestQueryStringItem();
			case ServerDataType.Form:
				return this.GetRequestFormItem();
			case ServerDataType.Cookie:
				return this.GetCookieItem();
			case ServerDataType.ServerVariables:
				return this.GetRequestServerVariablesItem();
			case ServerDataType.RequestParams:
				return this.GetRequestParamsItem();
			case ServerDataType.Request:
				return this.GetRequestItem();
			case ServerDataType.Environment:
				return typeof(Environment);
			case ServerDataType.AppSetting:
				return ConfigurationManager.AppSettings[this.Item.ToString()];
			default:
				return null;
			}
		}

		/// <summary>
		/// 获取服务器时间
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600026F RID: 623 RVA: 0x0000ADD0 File Offset: 0x00008FD0
		private DateTime GetServerTime()
		{
			DateTime result = DateTime.Now;
			string text = (this.Item == null) ? null : this.Item.GetTextValue();
			string a;
			if (!string.IsNullOrEmpty(text) && (a = text.ToLower()) != null)
			{
				if (!(a == "today"))
				{
					if (!(a == "yesterday"))
					{
						if (a == "tomorrow")
						{
							result = DateTime.Today.AddDays(1.0);
						}
					}
					else
					{
						result = DateTime.Today.AddDays(-1.0);
					}
				}
				else
				{
					result = DateTime.Today;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取HttpApplicationState数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000270 RID: 624 RVA: 0x0000AE70 File Offset: 0x00009070
		private object GetApplicationItem()
		{
			object result = null;
			HttpContext httpContext = HttpContext.Current;
			string text = (this.Item == null) ? null : this.Item.GetTextValue();
			if (!string.IsNullOrEmpty(text) && httpContext != null && httpContext.Application != null)
			{
				result = httpContext.Application[text];
			}
			return result;
		}

		/// <summary>
		/// 获取HttpSessionState数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000271 RID: 625 RVA: 0x0000AEC0 File Offset: 0x000090C0
		private object GetSessionItem()
		{
			object result = null;
			HttpContext httpContext = HttpContext.Current;
			string text = (this.Item == null) ? null : this.Item.GetTextValue();
			if (!string.IsNullOrEmpty(text) && httpContext != null && httpContext.Session != null)
			{
				result = httpContext.Session[text];
			}
			return result;
		}

		/// <summary>
		/// 获取Cache数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000272 RID: 626 RVA: 0x0000AF10 File Offset: 0x00009110
		private object GetCacheItem()
		{
			object result = null;
			Cache cache = HttpRuntime.Cache;
			string text = (this.Item == null) ? null : this.Item.GetTextValue();
			if (!string.IsNullOrEmpty(text) && cache != null)
			{
				result = cache[text];
			}
			return result;
		}

		/// <summary>
		/// 获取Request.Form数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000273 RID: 627 RVA: 0x0000AF50 File Offset: 0x00009150
		private string GetNameValueCollectionItem(NameValueCollection items)
		{
			string result = null;
			string text = (this.Item == null) ? null : this.Item.GetTextValue();
			if (!string.IsNullOrEmpty(text))
			{
				result = items[text];
			}
			return result;
		}

		/// <summary>
		/// 获取Request.QueryString数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000274 RID: 628 RVA: 0x0000AF88 File Offset: 0x00009188
		private string GetRequestQueryStringItem()
		{
			string result = null;
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && httpContext.Request != null)
			{
				result = this.GetNameValueCollectionItem(httpContext.Request.QueryString);
			}
			return result;
		}

		/// <summary>
		/// 获取Request.Form数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000275 RID: 629 RVA: 0x0000AFBC File Offset: 0x000091BC
		private string GetRequestFormItem()
		{
			string result = null;
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && httpContext.Request != null)
			{
				result = this.GetNameValueCollectionItem(httpContext.Request.Form);
			}
			return result;
		}

		/// <summary>
		/// 获取Request.ServerVariables数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000276 RID: 630 RVA: 0x0000AFF0 File Offset: 0x000091F0
		private string GetRequestServerVariablesItem()
		{
			string result = null;
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && httpContext.Request != null)
			{
				result = this.GetNameValueCollectionItem(httpContext.Request.ServerVariables);
			}
			return result;
		}

		/// <summary>
		/// 获取Request.Cookie数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000277 RID: 631 RVA: 0x0000B024 File Offset: 0x00009224
		private string GetCookieItem()
		{
			string text = null;
			HttpContext httpContext = HttpContext.Current;
			string text2 = (this.Item == null) ? null : this.Item.GetTextValue();
			if (!string.IsNullOrEmpty(text2) && httpContext != null && httpContext.Request != null)
			{
				string[] array = text2.Split(".".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);
				HttpCookie httpCookie = httpContext.Request.Cookies[array[0]];
				if (httpCookie != null)
				{
					if (array.Length > 1)
					{
						text = httpCookie[array[1]];
					}
					else
					{
						text = httpCookie.Value;
					}
					if (!string.IsNullOrEmpty(text))
					{
						text = HttpUtility.UrlDecode(text, httpContext.Request.ContentEncoding);
					}
				}
			}
			return text;
		}

		/// <summary>
		/// 获取Request.Params数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000278 RID: 632 RVA: 0x0000B0C8 File Offset: 0x000092C8
		private string GetRequestParamsItem()
		{
			string result = null;
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && httpContext.Request != null)
			{
				result = this.GetNameValueCollectionItem(httpContext.Request.Params);
			}
			return result;
		}

		/// <summary>
		/// 获取Request对象
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000279 RID: 633 RVA: 0x0000B0FC File Offset: 0x000092FC
		private object GetRequestItem()
		{
			object result = null;
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && httpContext.Request != null)
			{
				result = httpContext.Request;
			}
			return result;
		}

		/// <summary>
		/// 随机种子数
		/// </summary>
		// Token: 0x04000089 RID: 137
		private static Random random = new Random();
	}
}
