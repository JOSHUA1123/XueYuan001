using System;

namespace pili_sdk.pili
{
	// Token: 0x02000015 RID: 21
	public class FramesPerSecond
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000048D0 File Offset: 0x00002AD0
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000048D8 File Offset: 0x00002AD8
		public virtual float Audio { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000048E1 File Offset: 0x00002AE1
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x000048E9 File Offset: 0x00002AE9
		public virtual float Video { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000048F2 File Offset: 0x00002AF2
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x000048FA File Offset: 0x00002AFA
		public virtual float Data { get; private set; }

		// Token: 0x060000B3 RID: 179 RVA: 0x00004903 File Offset: 0x00002B03
		public FramesPerSecond(float audio, float video, float data)
		{
			this.Audio = audio;
			this.Video = video;
			this.Data = data;
		}
	}
}
