using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace SMS.sms1086
{
	/// <remarks />
	// Token: 0x0200000C RID: 12
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	[DesignerCategory("code")]
	[DebuggerStepThrough]
	public class RecvSMSCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000042 RID: 66 RVA: 0x0000291D File Offset: 0x00000B1D
		internal RecvSMSCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}

		/// <remarks />
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002930 File Offset: 0x00000B30
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (string)this.results[0];
			}
		}

		// Token: 0x04000010 RID: 16
		private object[] results;
	}
}
