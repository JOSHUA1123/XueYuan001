using Spring.Context;
using Spring.Context.Support;
using System;
using System.Reflection;


namespace Common
{
	// Token: 0x02000022 RID: 34
	public class Business
	{
        /// <summary>
        /// IOC的入口
        /// </summary>
        /// <typeparam name="IBusinessDo"></typeparam>
        /// <returns></returns>
        // Token: 0x060000C8 RID: 200 RVA: 0x0000FBF4 File Offset: 0x0000DDF4
        public static IBusinessDo Do<IBusinessDo>()
        {
            if (!Business.Version.StartsWith(Business.AppVersion))
            {
                throw new ArgumentException("Common.dll的程序集无法与当前应用适配。");
            }
            if (!typeof(IBusinessDo).IsInterface)
            {
                throw new ArgumentException("泛型调用的类不是接口类型。");
            }
            if (!typeof(IBusinessInterface).IsAssignableFrom(typeof(IBusinessDo)))
            {
                throw new ArgumentException("泛型调用的不是系统约定的接口类型。");
            }
            string name = typeof(IBusinessDo).Name;
            IBusinessDo result;
            try
            {
                IApplicationContext context = ContextRegistry.GetContext();
                IBusinessInterface businessInterface = context.GetObject(name) as IBusinessInterface;
                result = (IBusinessDo)((object)businessInterface);
                
            }
            catch (Exception ex)
            {
                string message = "无法构建" + name + "接口的实现，请确定web.config中configuration/spring/objects节的配置正确。更多详情：" + ex.Message;
                throw new Exception(message);
            }
            return result;
        }

        /// <summary>
        /// 当前程序集版本号
        /// </summary>
        // Token: 0x04000043 RID: 67
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

		/// <summary>
		/// 应用层的程序集版本号
		/// </summary>
		// Token: 0x04000044 RID: 68
		private static string AppVersion = App.Version;
	}
}
