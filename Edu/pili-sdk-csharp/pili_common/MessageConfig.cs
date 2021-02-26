using System;

namespace pili_sdk.pili_common
{
	// Token: 0x0200000A RID: 10
	public class MessageConfig
	{
		// Token: 0x04000018 RID: 24
		public const string NULL_STREAM_ID_EXCEPTION_MSG = "FATAL EXCEPTION: streamId is null!";

		// Token: 0x04000019 RID: 25
		public const string NULL_HUBNAME_EXCEPTION_MSG = "FATAL EXCEPTION: hubName is null!";

		// Token: 0x0400001A RID: 26
		public const string NULL_CREDENTIALS_EXCEPTION_MSG = "FATAL EXCEPTION: credentials is null!";

		// Token: 0x0400001B RID: 27
		public const string ILLEGAL_RTMP_PUBLISH_URL_MSG = "Illegal rtmp publish url!";

		// Token: 0x0400001C RID: 28
		public const string ILLEGAL_TIME_MSG = "Illegal startTime or endTime!";

		// Token: 0x0400001D RID: 29
		public static readonly string ILLEGAL_TITLE_MSG = string.Concat(new object[]
		{
			"The length of title should be at least:",
			5,
			",or at most:",
			200
		});

		// Token: 0x0400001E RID: 30
		public const string ILLEGAL_FILE_NAME_EXCEPTION_MSG = "Illegal file name !";

		// Token: 0x0400001F RID: 31
		public const string ILLEGAL_FORMAT_EXCEPTION_MSG = "Illegal format !";
	}
}
