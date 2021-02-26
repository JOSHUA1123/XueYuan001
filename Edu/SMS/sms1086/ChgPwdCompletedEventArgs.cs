using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace SMS.sms1086
{
	/// <remarks />
	// Token: 0x02000006 RID: 6
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	public class ChgPwdCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000030 RID: 48 RVA: 0x000028A5 File Offset: 0x00000AA5
		internal ChgPwdCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}

		/// <remarks />
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000028B8 File Offset: 0x00000AB8
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x0400000D RID: 13
		private object[] results;
	}
}
