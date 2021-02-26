using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace SMS.sms1086
{
	/// <remarks />
	// Token: 0x02000004 RID: 4
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	[DesignerCategory("code")]
	public class QueryCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600002A RID: 42 RVA: 0x0000287D File Offset: 0x00000A7D
		internal QueryCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}

		/// <remarks />
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002890 File Offset: 0x00000A90
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x0400000C RID: 12
		private object[] results;
	}
}
