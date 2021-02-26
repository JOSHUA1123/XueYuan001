using System;
using System.Collections.Generic;
using System.Web;

namespace Common.Param.Method
{
	/// <summary>
	/// 获取当前页面的信息
	/// </summary>
	// Token: 0x02000050 RID: 80
	public class PageInfo
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00009E3B File Offset: 0x0000803B
		public PageInfo(HttpContext context)
		{
			this._context = context;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009E4A File Offset: 0x0000804A
		public PageInfo()
		{
			this._context = HttpContext.Current;
		}

		/// <summary>
		/// 当前aspx文档的文件名，包括后缀名，不包括路径
		/// </summary>
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00016250 File Offset: 0x00014450
		public string FileName
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.CurrentExecutionFilePath;
				if (text.IndexOf("/") > -1)
				{
					text = text.Substring(text.LastIndexOf("/") + 1);
				}
				return text.ToLower();
			}
		}

		/// <summary>
		/// 当前Aspx文档的名称，不包括后缀名，例如:test.aspx页面，将返回test
		/// </summary>
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00016298 File Offset: 0x00014498
		public string Name
		{
			get
			{
				string text = this.FileName;
				if (text.IndexOf(".") > -1)
				{
					text = text.Substring(0, text.LastIndexOf("."));
				}
				return text;
			}
		}

		/// <summary>
		/// 全名称物理路径
		/// </summary>
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600021D RID: 541 RVA: 0x000162D0 File Offset: 0x000144D0
		public string FullNamePhysics
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string absolutePath = httpContext.Request.Url.AbsolutePath;
				return Server.MapPath(absolutePath);
			}
		}

		/// <summary>
		/// 当前页面的权限标题，例如：Article_Edit.aspx，将返回Article;
		/// </summary>
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600021E RID: 542 RVA: 0x000162FC File Offset: 0x000144FC
		public string PurviewLabel
		{
			get
			{
				string name = this.Name;
				string text = name.ToLower();
				if (text.IndexOf("_") > -1)
				{
					text = text.Substring(0, text.IndexOf("_"));
				}
				return text;
			}
		}

		/// <summary>
		/// 当前页面相对于于/Manage文件夹的文件名，如sys/emplayee.aspx
		/// </summary>
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0001633C File Offset: 0x0001453C
		public string ManageFileName
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.Url.AbsolutePath.ToLower();
				string text2 = "/manage/";
				if (text.IndexOf(text2) > -1)
				{
					text = text.Substring(text.IndexOf(text2) + text2.Length);
				}
				return text;
			}
		}

		/// <summary>
		/// 当前页面的后缀名，如aspx；
		/// </summary>
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0001638C File Offset: 0x0001458C
		public string Extension
		{
			get
			{
				string text = this.FileName;
				if (text.IndexOf(".") > -1)
				{
					text = text.Substring(text.LastIndexOf(".") + 1);
				}
				return text;
			}
		}

		/// <summary>
		/// 当前页面所在的功能模块名称；即相对于/Manage文件夹的路径，因为每一个模块在Manage目录下是一个单独目录
		/// </summary>
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000221 RID: 545 RVA: 0x000163C4 File Offset: 0x000145C4
		public string Module
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.ServerVariables["PATH_INFO"].ToLower();
				string text2 = "/manage/";
				string text3 = text.Substring(text.IndexOf(text2) + text2.Length);
				if (text3.IndexOf("/") > -1)
				{
					return text3.Substring(0, text3.IndexOf("/"));
				}
				return "";
			}
		}

		/// <summary>
		/// 当前aspx文档的URI完整路径，不包括文件名
		/// </summary>
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00016438 File Offset: 0x00014638
		public string AbsolutePath
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.Url.AbsolutePath;
				if (text.IndexOf("/") > -1)
				{
					text = text.Substring(0, text.LastIndexOf("/") + 1);
				}
				return text;
			}
		}

		/// <summary>
		/// 当前页面的物理路径，不包括文件名
		/// </summary>
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00009E5D File Offset: 0x0000805D
		public string PhysicsPath
		{
			get
			{
				return Server.MapPath(this.AbsolutePath);
			}
		}

		/// <summary>
		/// 根域，例如http://xxx:82/
		/// </summary>
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00016480 File Offset: 0x00014680
		public string Authority
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				string text = httpContext.Request.Url.ToString().ToLower();
				if (text.StartsWith("http://"))
				{
					text = "http://" + httpContext.Request.Url.Authority;
				}
				if (text.StartsWith("https://"))
				{
					text = "https://" + httpContext.Request.Url.Authority;
				}
				return text + "/";
			}
		}

		/// <summary>
		/// 增加Get参数
		/// </summary>
		/// <param name="key">参数名</param>
		/// <param name="value">参数值</param>
		/// <returns></returns>
		// Token: 0x06000225 RID: 549 RVA: 0x00016504 File Offset: 0x00014704
		public string AddPara(string key, string value)
		{
			HttpContext httpContext = HttpContext.Current;
			string text = httpContext.Request.Url.PathAndQuery;
			if (text.IndexOf("?") < 0)
			{
				text = string.Concat(new string[]
				{
					text,
					"?",
					key,
					"=",
					value
				});
			}
			else
			{
				text = string.Concat(new string[]
				{
					text,
					"&",
					key,
					"=",
					value
				});
			}
			return text;
		}

		/// <summary>
		/// 增加某地址的参数
		/// </summary>
		/// <param name="url">地址</param>
		/// <param name="para">参数，采用key=value的形式</param>
		/// <returns></returns>
		// Token: 0x06000226 RID: 550 RVA: 0x00016590 File Offset: 0x00014790
		public string AddPara(string url, params string[] para)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (url.IndexOf("?") > -1)
			{
				string text = url.Substring(url.IndexOf("?") + 1);
				foreach (string text2 in text.Split(new char[]
				{
					'&'
				}))
				{
					if (text2.IndexOf("=") > -1)
					{
						dictionary.Add(text2.Substring(0, text2.IndexOf("=")), text2.Substring(text2.IndexOf("=") + 1));
					}
				}
				url = url.Substring(0, url.IndexOf("?"));
			}
			for (int j = 0; j < para.Length; j++)
			{
				string value = "";
				string key;
				if (para[j].IndexOf("=") > -1)
				{
					key = para[j].Substring(0, para[j].IndexOf("="));
					value = para[j].Substring(para[j].IndexOf("=") + 1);
				}
				else
				{
					key = para[j];
				}
				if (dictionary.ContainsKey(key))
				{
					dictionary[key] = value;
				}
				else
				{
					dictionary.Add(key, value);
				}
			}
			url += "?";
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				url += string.Format("{0}={1}&", keyValuePair.Key, keyValuePair.Value);
			}
			if (url.Substring(url.Length - 1) == "&")
			{
				url = url.Substring(0, url.Length - 1);
			}
			return url;
		}

		/// <summary>
		/// 给当前页面添加参数
		/// </summary>
		/// <param name="para">参数，采用key=value的形式</param>
		/// <returns></returns>
		// Token: 0x06000227 RID: 551 RVA: 0x00016760 File Offset: 0x00014960
		public string AddPara(params string[] para)
		{
			HttpContext httpContext = HttpContext.Current;
			string pathAndQuery = httpContext.Request.Url.PathAndQuery;
			return this.AddPara(pathAndQuery, para);
		}

		/// <summary>
		/// 移除url中的某些参数
		/// </summary>
		/// <param name="url"></param>
		/// <param name="para">参数，采用key=value的形式</param>
		/// <returns></returns>
		// Token: 0x06000228 RID: 552 RVA: 0x0001678C File Offset: 0x0001498C
		public string RemovePara(string url, params string[] para)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (url.IndexOf("?") > -1)
			{
				string text = url.Substring(url.IndexOf("?") + 1);
				foreach (string text2 in text.Split(new char[]
				{
					'&'
				}))
				{
					if (text2.IndexOf("=") > -1)
					{
						dictionary.Add(text2.Substring(0, text2.IndexOf("=")), text2.Substring(text2.IndexOf("=") + 1));
					}
				}
				url = url.Substring(0, url.IndexOf("?"));
			}
			for (int j = 0; j < para.Length; j++)
			{
				if (dictionary.ContainsKey(para[j]))
				{
					dictionary.Remove(para[j]);
				}
			}
			url += "?";
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				url += string.Format("{0}={1}&", keyValuePair.Key, keyValuePair.Value);
			}
			if (url.Substring(url.Length - 1) == "&")
			{
				url = url.Substring(0, url.Length - 1);
			}
			return url;
		}

		/// <summary>
		/// 移除当前页面地址的某些参数
		/// </summary>
		/// <param name="para">参数，采用key=value的形式</param>
		/// <returns></returns>
		// Token: 0x06000229 RID: 553 RVA: 0x000168FC File Offset: 0x00014AFC
		public string RemovePara(params string[] para)
		{
			HttpContext httpContext = HttpContext.Current;
			string pathAndQuery = httpContext.Request.Url.PathAndQuery;
			return this.RemovePara(pathAndQuery, para);
		}

		// Token: 0x040000C0 RID: 192
		private HttpContext _context;
	}
}
