using System;
using pili_sdk.pili;

namespace pili_sdk
{
	// Token: 0x02000006 RID: 6
	public interface IStream
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000017 RID: 23
		ICredentials Credentials { get; }

		// Token: 0x06000018 RID: 24
		bool Test(string accesskey, string secretkey, string hubname, string version);

		// Token: 0x06000019 RID: 25
		Stream Create();

		// Token: 0x0600001A RID: 26
		Stream Create(string title);

		// Token: 0x0600001B RID: 27
		Stream Create(string title, string publishKey, string publishSecurity);

		// Token: 0x0600001C RID: 28
		StreamList List();

		// Token: 0x0600001D RID: 29
		StreamList List(bool liveonly);

		// Token: 0x0600001E RID: 30
		StreamList List(string marker, long limit);

		// Token: 0x0600001F RID: 31
		StreamList List(string marker, long limit, string titlePrefix);

		// Token: 0x06000020 RID: 32
		StreamList List(string marker, long limit, string titlePrefix, bool? liveonly);

		// Token: 0x06000021 RID: 33
		Stream GetForID(string id);

		// Token: 0x06000022 RID: 34
		Stream GetForTitle(string title);

		// Token: 0x06000023 RID: 35
		string Delete(Stream stream);

		// Token: 0x06000024 RID: 36
		string Delete(string title);

		// Token: 0x06000025 RID: 37
		string DeleteForID(string id);

		// Token: 0x06000026 RID: 38
		Stream Update(Stream stream, string publishKey, string publishSecrity);

		// Token: 0x06000027 RID: 39
		Stream Update(Stream stream, string publishKey, string publishSecrity, bool? disabled);

		// Token: 0x06000028 RID: 40
		StreamStatus Status(Stream stream);

		// Token: 0x06000029 RID: 41
		StreamStatus Status(string streamid);

		// Token: 0x0600002A RID: 42
		Stream Enable(Stream stream);

		// Token: 0x0600002B RID: 43
		Stream Disable(Stream stream);

		// Token: 0x0600002C RID: 44
		SnapshotResponse Snapshot(Stream stream, string name, string format);

		// Token: 0x0600002D RID: 45
		SnapshotResponse Snapshot(Stream stream, string name, string format, string notifyUrl);

		// Token: 0x0600002E RID: 46
		SnapshotResponse Snapshot(Stream stream, string name, string format, long time, string notifyUrl);
	}
}
