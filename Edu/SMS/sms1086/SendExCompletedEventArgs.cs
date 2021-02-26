using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace SMS.sms1086
{
	/// <remarks />
	// Token: 0x0200000A RID: 10
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	[DebuggerStepThrough]
	public class SendExCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600003C RID: 60 RVA: 0x000028F5 File Offset: 0x00000AF5
		internal SendExCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}

		/// <remarks />
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002908 File Offset: 0x00000B08
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x0400000F RID: 15
		private object[] results;
	}
}
