using System;

namespace VTemplate.Engine
{
    /// <summary>
    /// 预处理模板数据的方法属性
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TemplateRenderMethodAttribute : System.Attribute
    {
        /// <summary>
        /// 描述
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
