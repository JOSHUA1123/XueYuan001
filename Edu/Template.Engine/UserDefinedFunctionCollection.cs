using System;
using System.Collections.Generic;

namespace VTemplate.Engine
{
	/// <summary>
	/// 自定义函数集合
	/// </summary>
	// Token: 0x0200000D RID: 13
	public class UserDefinedFunctionCollection : Dictionary<string, UserDefinedFunction>
	{
		/// <summary>
		///
		/// </summary>
		// Token: 0x06000090 RID: 144 RVA: 0x00003C76 File Offset: 0x00001E76
		public UserDefinedFunctionCollection() : base(StringComparer.InvariantCultureIgnoreCase)
		{
		}

		/// <summary>
		/// 添加与方法名同名的用户自定义函数
		/// </summary>
		/// <param name="function"></param>
		// Token: 0x06000091 RID: 145 RVA: 0x00003C83 File Offset: 0x00001E83
		public void Add(UserDefinedFunction function)
		{
			if (function != null)
			{
				this.Add(function.Method.Name, function);
			}
		}

		/// <summary>
		/// 添加某个用户自定义函数
		/// </summary>
		/// <param name="key"></param>
		/// <param name="function"></param>
		/// <remarks>重写此函数主要是为便于可重复添加多次同名的自定义函数(但只有最后一次有效)</remarks>
		// Token: 0x06000092 RID: 146 RVA: 0x00003C9C File Offset: 0x00001E9C
		public new void Add(string key, UserDefinedFunction function)
		{
			lock (this)
			{
				if (base.ContainsKey(key))
				{
					base[key] = function;
				}
				else
				{
					base.Add(key, function);
				}
			}
		}
	}
}
