using System;
using Common.Param.Method;

namespace Common
{
	// Token: 0x02000074 RID: 116
	public class LoginItem
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000A448 File Offset: 0x00008648
		public LoginItem(PlatformInfoHandler.SiteInfoHandlerParaNode node)
		{
			this._node = node;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000A457 File Offset: 0x00008657
		public ConvertToAnyValue Pattern
		{
			get
			{
				return new ConvertToAnyValue(this._node.ItemValue("Pattern"));
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000A46E File Offset: 0x0000866E
		public ConvertToAnyValue KeyName
		{
			get
			{
				return new ConvertToAnyValue(this._node.ItemValue("KeyName"));
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000A485 File Offset: 0x00008685
		public ConvertToAnyValue Expires
		{
			get
			{
				return new ConvertToAnyValue(this._node.ItemValue("Expires"));
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000A49C File Offset: 0x0000869C
		public ConvertToAnyValue DefaultPw
		{
			get
			{
				return new ConvertToAnyValue(this._node.ItemValue("DefaultPw"));
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000A4B3 File Offset: 0x000086B3
		public ConvertToAnyValue NoLoginPath
		{
			get
			{
				return new ConvertToAnyValue(this._node.ItemValue("NoLoginPath"));
			}
		}

		// Token: 0x0400011F RID: 287
		private PlatformInfoHandler.SiteInfoHandlerParaNode _node;
	}
}
