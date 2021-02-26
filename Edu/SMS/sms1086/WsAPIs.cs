using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using SMS.Properties;

namespace SMS.sms1086
{
	/// <remarks />
	// Token: 0x02000002 RID: 2
	[WebServiceBinding(Name = "WsAPIsSoap", Namespace = "http://tempuri.org/")]
	[GeneratedCode("System.Web.Services", "4.0.30319.1")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class WsAPIs : SoapHttpClientProtocol
	{
		/// <remarks />
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public WsAPIs()
		{
			this.Url = Settings.Default.WeiSha_SMS_sms1086_WsAPIs;
			if (this.IsLocalFileSystemWebService(this.Url))
			{
				this.UseDefaultCredentials = true;
				this.useDefaultCredentialsSetExplicitly = false;
				return;
			}
			this.useDefaultCredentialsSetExplicitly = true;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000210C File Offset: 0x0000030C
		// (set) Token: 0x06000003 RID: 3 RVA: 0x00002114 File Offset: 0x00000314
		public new string Url
		{
			get
			{
				return base.Url;
			}
			set
			{
				if (this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly && !this.IsLocalFileSystemWebService(value))
				{
					base.UseDefaultCredentials = false;
				}
				base.Url = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002143 File Offset: 0x00000343
		// (set) Token: 0x06000005 RID: 5 RVA: 0x0000214B File Offset: 0x0000034B
		public new bool UseDefaultCredentials
		{
			get
			{
				return base.UseDefaultCredentials;
			}
			set
			{
				base.UseDefaultCredentials = value;
				this.useDefaultCredentialsSetExplicitly = true;
			}
		}

		/// <remarks />
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000006 RID: 6 RVA: 0x0000215C File Offset: 0x0000035C
		// (remove) Token: 0x06000007 RID: 7 RVA: 0x00002194 File Offset: 0x00000394
		public event QueryCompletedEventHandler QueryCompleted;

		/// <remarks />
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000008 RID: 8 RVA: 0x000021CC File Offset: 0x000003CC
		// (remove) Token: 0x06000009 RID: 9 RVA: 0x00002204 File Offset: 0x00000404
		public event ChgPwdCompletedEventHandler ChgPwdCompleted;

		/// <remarks />
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600000A RID: 10 RVA: 0x0000223C File Offset: 0x0000043C
		// (remove) Token: 0x0600000B RID: 11 RVA: 0x00002274 File Offset: 0x00000474
		public event SendCompletedEventHandler SendCompleted;

		/// <remarks />
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600000C RID: 12 RVA: 0x000022AC File Offset: 0x000004AC
		// (remove) Token: 0x0600000D RID: 13 RVA: 0x000022E4 File Offset: 0x000004E4
		public event SendExCompletedEventHandler SendExCompleted;

		/// <remarks />
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600000E RID: 14 RVA: 0x0000231C File Offset: 0x0000051C
		// (remove) Token: 0x0600000F RID: 15 RVA: 0x00002354 File Offset: 0x00000554
		public event RecvSMSCompletedEventHandler RecvSMSCompleted;

		/// <remarks />
		// Token: 0x06000010 RID: 16 RVA: 0x0000238C File Offset: 0x0000058C
		[SoapDocumentMethod("http://tempuri.org/Query", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string Query(string Username, string Password)
		{
			object[] array = base.Invoke("Query", new object[]
			{
				Username,
				Password
			});
			return (string)array[0];
		}

		/// <remarks />
		// Token: 0x06000011 RID: 17 RVA: 0x000023BD File Offset: 0x000005BD
		public void QueryAsync(string Username, string Password)
		{
			this.QueryAsync(Username, Password, null);
		}

		/// <remarks />
		// Token: 0x06000012 RID: 18 RVA: 0x000023C8 File Offset: 0x000005C8
		public void QueryAsync(string Username, string Password, object userState)
		{
			if (this.QueryOperationCompleted == null)
			{
				this.QueryOperationCompleted = new SendOrPostCallback(this.OnQueryOperationCompleted);
			}
			base.InvokeAsync("Query", new object[]
			{
				Username,
				Password
			}, this.QueryOperationCompleted, userState);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002414 File Offset: 0x00000614
		private void OnQueryOperationCompleted(object arg)
		{
			if (this.QueryCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.QueryCompleted(this, new QueryCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		/// <remarks />
		// Token: 0x06000014 RID: 20 RVA: 0x0000245C File Offset: 0x0000065C
		[SoapDocumentMethod("http://tempuri.org/ChgPwd", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string ChgPwd(string Username, string Password, string NewPassword)
		{
			object[] array = base.Invoke("ChgPwd", new object[]
			{
				Username,
				Password,
				NewPassword
			});
			return (string)array[0];
		}

		/// <remarks />
		// Token: 0x06000015 RID: 21 RVA: 0x00002491 File Offset: 0x00000691
		public void ChgPwdAsync(string Username, string Password, string NewPassword)
		{
			this.ChgPwdAsync(Username, Password, NewPassword, null);
		}

		/// <remarks />
		// Token: 0x06000016 RID: 22 RVA: 0x000024A0 File Offset: 0x000006A0
		public void ChgPwdAsync(string Username, string Password, string NewPassword, object userState)
		{
			if (this.ChgPwdOperationCompleted == null)
			{
				this.ChgPwdOperationCompleted = new SendOrPostCallback(this.OnChgPwdOperationCompleted);
			}
			base.InvokeAsync("ChgPwd", new object[]
			{
				Username,
				Password,
				NewPassword
			}, this.ChgPwdOperationCompleted, userState);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024F0 File Offset: 0x000006F0
		private void OnChgPwdOperationCompleted(object arg)
		{
			if (this.ChgPwdCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.ChgPwdCompleted(this, new ChgPwdCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		/// <remarks />
		// Token: 0x06000018 RID: 24 RVA: 0x00002538 File Offset: 0x00000738
		[SoapDocumentMethod("http://tempuri.org/Send", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string Send(string Username, string Password, string Mobiles, string Content, string AtTime)
		{
			object[] array = base.Invoke("Send", new object[]
			{
				Username,
				Password,
				Mobiles,
				Content,
				AtTime
			});
			return (string)array[0];
		}

		/// <remarks />
		// Token: 0x06000019 RID: 25 RVA: 0x00002577 File Offset: 0x00000777
		public void SendAsync(string Username, string Password, string Mobiles, string Content, string AtTime)
		{
			this.SendAsync(Username, Password, Mobiles, Content, AtTime, null);
		}

		/// <remarks />
		// Token: 0x0600001A RID: 26 RVA: 0x00002588 File Offset: 0x00000788
		public void SendAsync(string Username, string Password, string Mobiles, string Content, string AtTime, object userState)
		{
			if (this.SendOperationCompleted == null)
			{
				this.SendOperationCompleted = new SendOrPostCallback(this.OnSendOperationCompleted);
			}
			base.InvokeAsync("Send", new object[]
			{
				Username,
				Password,
				Mobiles,
				Content,
				AtTime
			}, this.SendOperationCompleted, userState);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025E0 File Offset: 0x000007E0
		private void OnSendOperationCompleted(object arg)
		{
			if (this.SendCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.SendCompleted(this, new SendCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		/// <remarks />
		// Token: 0x0600001C RID: 28 RVA: 0x00002628 File Offset: 0x00000828
		[SoapDocumentMethod("http://tempuri.org/SendEx", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string SendEx(string Username, string Password, string Mobiles, string Content, string AtTime, string LinkID, int LongSms)
		{
			object[] array = base.Invoke("SendEx", new object[]
			{
				Username,
				Password,
				Mobiles,
				Content,
				AtTime,
				LinkID,
				LongSms
			});
			return (string)array[0];
		}

		/// <remarks />
		// Token: 0x0600001D RID: 29 RVA: 0x00002678 File Offset: 0x00000878
		public void SendExAsync(string Username, string Password, string Mobiles, string Content, string AtTime, string LinkID, int LongSms)
		{
			this.SendExAsync(Username, Password, Mobiles, Content, AtTime, LinkID, LongSms, null);
		}

		/// <remarks />
		// Token: 0x0600001E RID: 30 RVA: 0x00002698 File Offset: 0x00000898
		public void SendExAsync(string Username, string Password, string Mobiles, string Content, string AtTime, string LinkID, int LongSms, object userState)
		{
			if (this.SendExOperationCompleted == null)
			{
				this.SendExOperationCompleted = new SendOrPostCallback(this.OnSendExOperationCompleted);
			}
			base.InvokeAsync("SendEx", new object[]
			{
				Username,
				Password,
				Mobiles,
				Content,
				AtTime,
				LinkID,
				LongSms
			}, this.SendExOperationCompleted, userState);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002700 File Offset: 0x00000900
		private void OnSendExOperationCompleted(object arg)
		{
			if (this.SendExCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.SendExCompleted(this, new SendExCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		/// <remarks />
		// Token: 0x06000020 RID: 32 RVA: 0x00002748 File Offset: 0x00000948
		[SoapDocumentMethod("http://tempuri.org/RecvSMS", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string RecvSMS(string username, string password, string fromtime, string readflag)
		{
			object[] array = base.Invoke("RecvSMS", new object[]
			{
				username,
				password,
				fromtime,
				readflag
			});
			return (string)array[0];
		}

		/// <remarks />
		// Token: 0x06000021 RID: 33 RVA: 0x00002782 File Offset: 0x00000982
		public void RecvSMSAsync(string username, string password, string fromtime, string readflag)
		{
			this.RecvSMSAsync(username, password, fromtime, readflag, null);
		}

		/// <remarks />
		// Token: 0x06000022 RID: 34 RVA: 0x00002790 File Offset: 0x00000990
		public void RecvSMSAsync(string username, string password, string fromtime, string readflag, object userState)
		{
			if (this.RecvSMSOperationCompleted == null)
			{
				this.RecvSMSOperationCompleted = new SendOrPostCallback(this.OnRecvSMSOperationCompleted);
			}
			base.InvokeAsync("RecvSMS", new object[]
			{
				username,
				password,
				fromtime,
				readflag
			}, this.RecvSMSOperationCompleted, userState);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000027E4 File Offset: 0x000009E4
		private void OnRecvSMSOperationCompleted(object arg)
		{
			if (this.RecvSMSCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.RecvSMSCompleted(this, new RecvSMSCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		/// <remarks />
		// Token: 0x06000024 RID: 36 RVA: 0x00002829 File Offset: 0x00000A29
		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002834 File Offset: 0x00000A34
		private bool IsLocalFileSystemWebService(string url)
		{
			if (url == null || url == string.Empty)
			{
				return false;
			}
			Uri uri = new Uri(url);
			return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x04000001 RID: 1
		private SendOrPostCallback QueryOperationCompleted;

		// Token: 0x04000002 RID: 2
		private SendOrPostCallback ChgPwdOperationCompleted;

		// Token: 0x04000003 RID: 3
		private SendOrPostCallback SendOperationCompleted;

		// Token: 0x04000004 RID: 4
		private SendOrPostCallback SendExOperationCompleted;

		// Token: 0x04000005 RID: 5
		private SendOrPostCallback RecvSMSOperationCompleted;

		// Token: 0x04000006 RID: 6
		private bool useDefaultCredentialsSetExplicitly;
	}
}
