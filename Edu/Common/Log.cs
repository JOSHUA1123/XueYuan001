using System;
using System.IO;
using System.Text;
using System.Web;

namespace Common
{
	/// <summary>
	/// 用于记录日志
	/// </summary>
	// Token: 0x02000082 RID: 130
	public class Log
	{
		/// <summary>
		/// 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
		/// </summary>
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0001BF5C File Offset: 0x0001A15C
		public static int LOG_LEVEL
		{
			get
			{
				if (Log.log_level >= 0)
				{
					return Log.log_level;
				}
				int num = App.Get["LOG_LEVEL"].Int16 ?? 0;
				Log.log_level = ((num >= 0) ? num : num);
				return Log.log_level;
			}
		}

		/// <summary>
		/// 向日志文件写入调试信息
		/// </summary>
		/// <param name="className">类名</param>
		/// <param name="content">要写入的内容</param>
		// Token: 0x0600035C RID: 860 RVA: 0x0000A729 File Offset: 0x00008929
		public static string Debug(string className, string content)
		{
			if (Log.LOG_LEVEL >= 3)
			{
				return Log.WriteLog("DEBUG", className, content);
			}
			return string.Empty;
		}

		/// <summary>
		/// 向日志文件写入信息
		/// </summary>
		/// <param name="className">类名</param>
		/// <param name="content">要写入的内容</param>
		// Token: 0x0600035D RID: 861 RVA: 0x0000A745 File Offset: 0x00008945
		public static string Info(string className, string content)
		{
			if (Log.LOG_LEVEL >= 2)
			{
				return Log.WriteLog("INFO", className, content);
			}
			return string.Empty;
		}

		/// <summary>
		/// 向日志文件出错信息
		/// </summary>
		/// <param name="className">类名</param>
		/// <param name="content">要写入的内容</param>
		// Token: 0x0600035E RID: 862 RVA: 0x0000A761 File Offset: 0x00008961
		public static string Error(string className, string content)
		{
			if (Log.LOG_LEVEL >= 1)
			{
				return Log.WriteLog("ERROR", className, content);
			}
			return string.Empty;
		}

		/// <summary>
		/// 向日志文件出错信息
		/// </summary>
		/// <param name="className">类名</param>
		/// <param name="ex">错误信息</param>
		// Token: 0x0600035F RID: 863 RVA: 0x0001BFB4 File Offset: 0x0001A1B4
		public static string Error(string className, Exception ex)
		{
			if (Log.LOG_LEVEL >= 1)
			{
				string text = "";
				text = text + "Message:" + ex.Message + "\n\r";
				text = text + "Source:" + ex.Source + "\n\r";
				text = text + "StackTrace:" + ex.StackTrace + "\n\r";
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"TargetSite:",
					ex.TargetSite,
					"\n\r"
				});
				text = text + "ToString:" + ex.ToString() + "\n\r";
				return Log.WriteLog("ERROR", className, text);
			}
			return string.Empty;
		}

		/// <summary>
		/// 实际写日志的方法
		/// </summary>
		/// <param name="logtype">日志类型</param>
		/// <param name="className">类名称</param>
		/// <param name="content">实际内容</param>
		// Token: 0x06000360 RID: 864 RVA: 0x0001C070 File Offset: 0x0001A270
		protected static string WriteLog(string logtype, string className, string content)
		{
			string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
			string str = Log.log_path + "/" + className.Replace(".", "/");
			string text = physicalApplicationPath + str;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string arg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
			string path = text + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
			StreamWriter streamWriter = File.AppendText(path);
			streamWriter.WriteLine(string.Format("{0}: {1}, {2} : ", logtype, arg, className));
			streamWriter.WriteLine(content);
			streamWriter.Close();
			return "/" + str;
		}

		// Token: 0x0400013C RID: 316
		private static string log_path = "logs";

		// Token: 0x0400013D RID: 317
		private static int log_level = -1;

        /// <summary>
        ///打印异常日志
        /// </summary>
        /// <param name="msg"></param>
        public static void TT2(string msg)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var pathLog = path + @"\log\";
            if (!Directory.Exists(pathLog))
            {
                Directory.CreateDirectory(pathLog);
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ", " + msg);
            System.IO.File.AppendAllText(pathLog + DateTime.Now.ToString("yyyyMMddHH") + "_LogERR.txt", sb.ToString());
        }
    }
}
