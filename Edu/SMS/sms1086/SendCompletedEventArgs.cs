using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace SMS.sms1086
{
	/// <remarks />
	// Token: 0x02000008 RID: 8
	[DebuggerStepThrough]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	[DesignerCategory("code")]
	public class SendCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000036 RID: 54 RVA: 0x000028CD File Offset: 0x00000ACD
		internal SendCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}

		/// <remarks />
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000028E0 File Offset: 0x00000AE0
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x0400000E RID: 14
		private object[] results;
	}
}
