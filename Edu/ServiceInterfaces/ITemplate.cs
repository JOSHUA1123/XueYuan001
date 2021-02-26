using System;
using System.Collections.Generic;
using System.Text;
using EntitiesInfo;
using System.Data;
using Common;

namespace ServiceInterfaces
{
    /// <summary>
    /// 模板管理
    /// </summary>
    public interface ITemplate : Common.IBusinessInterface
    {
        /// <summary>
        /// 所有Web模板
        /// </summary>
        /// <returns></returns>
        Common.Templates.TemplateBank[] WebTemplates();
        /// <summary>
        /// 所有手机模板
        /// </summary>
        /// <returns></returns>
        Common.Templates.TemplateBank[] MobiTemplates();
        /// <summary>
        /// 机构的当前web模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Common.Templates.TemplateBank WebCurrTemplate(int orgid);
        Common.Templates.TemplateBank WebCurrTemplate();
        /// <summary>
        /// 机构的当前手机模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Common.Templates.TemplateBank MobiCurrTemplate(int orgid);
        Common.Templates.TemplateBank MobiCurrTemplate();
        /// <summary>
        /// 设置当前web模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        Common.Templates.TemplateBank SetWebCurr(int orgid, string tag);
        /// <summary>
        /// 设置当前手机模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        Common.Templates.TemplateBank SetMobiCurr(int orgid, string tag);
        /// <summary>
        /// 通过模板标识获取web模板
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Common.Templates.TemplateBank GetWebTemplate(string tag);
        /// <summary>
        /// 通过模板标识获取手机模板
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Common.Templates.TemplateBank GetMobiTemplate(string tag);
        /// <summary>
        /// 更改模板信息
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        Common.Templates.TemplateBank Save(Common.Templates.TemplateBank tmp);
        
    }
}
