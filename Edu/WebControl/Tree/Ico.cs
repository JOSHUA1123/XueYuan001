using System;
using System.Reflection;

namespace WebControlShow.Tree
{
	// Token: 0x0200001C RID: 28
	public class Ico
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005DD4 File Offset: 0x00003FD4
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00005DDC File Offset: 0x00003FDC
		public string Empty
		{
			get
			{
				return this.empty;
			}
			set
			{
				this.empty = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005DE5 File Offset: 0x00003FE5
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00005DED File Offset: 0x00003FED
		public string Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005DF6 File Offset: 0x00003FF6
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00005DFE File Offset: 0x00003FFE
		public string MinusBottom
		{
			get
			{
				return this.minusBottom;
			}
			set
			{
				this.minusBottom = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00005E07 File Offset: 0x00004007
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00005E0F File Offset: 0x0000400F
		public string FolderOpen
		{
			get
			{
				return this.folderOpen;
			}
			set
			{
				this.folderOpen = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005E18 File Offset: 0x00004018
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00005E20 File Offset: 0x00004020
		public string Minus
		{
			get
			{
				return this.minus;
			}
			set
			{
				this.minus = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00005E29 File Offset: 0x00004029
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00005E31 File Offset: 0x00004031
		public string JoinBottom
		{
			get
			{
				return this.joinBottom;
			}
			set
			{
				this.joinBottom = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005E3A File Offset: 0x0000403A
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00005E42 File Offset: 0x00004042
		public string Page
		{
			get
			{
				return this.page;
			}
			set
			{
				this.page = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00005E4B File Offset: 0x0000404B
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00005E53 File Offset: 0x00004053
		public string Join
		{
			get
			{
				return this.join;
			}
			set
			{
				this.join = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00005E5C File Offset: 0x0000405C
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00005E64 File Offset: 0x00004064
		public string Root
		{
			get
			{
				return this.root;
			}
			set
			{
				this.root = value;
			}
		}

		/// <summary>
		/// 在构造时，将各个属性转换成html图片标签，如:<img src="xx.gif" />
		/// </summary>
		/// <param name="path">图片所处的路径</param>
		/// <param name="nodeHeight">节点的高度</param>
		// Token: 0x060000F9 RID: 249 RVA: 0x00005E70 File Offset: 0x00004070
		public Ico(string path, int nodeHeight)
		{
			Type type = base.GetType();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				string text = propertyInfo.Name + ".gif";
				string text2 = "float:left;";
				text = string.Concat(new object[]
				{
					"<img src=\"",
					path,
					"/",
					text,
					"\" style=\"",
					text2,
					"\" width=\"",
					nodeHeight,
					"px\" height=\"",
					nodeHeight,
					"px\"/>"
				});
				type.GetProperty(propertyInfo.Name).SetValue(this, text, null);
			}
		}

		// Token: 0x0400001B RID: 27
		private string empty;

		// Token: 0x0400001C RID: 28
		private string line;

		// Token: 0x0400001D RID: 29
		private string minusBottom;

		// Token: 0x0400001E RID: 30
		private string folderOpen;

		// Token: 0x0400001F RID: 31
		private string minus;

		// Token: 0x04000020 RID: 32
		private string joinBottom;

		// Token: 0x04000021 RID: 33
		private string page;

		// Token: 0x04000022 RID: 34
		private string join;

		// Token: 0x04000023 RID: 35
		private string root;
	}
}
