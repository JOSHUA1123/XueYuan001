using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using EntitiesInfo;
using ServiceInterfaces;
using ViewData.Attri;

namespace ViewData.Methods
{
    /// <summary>
    /// 机构管理
    /// </summary>
    [HttpGet]
    public class Organ : IViewAPI
    {
        /// <summary>
        /// 通过机构id获取机构信息
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构实体</returns>
        public EntitiesInfo.Organization ForID(int id)
        {
            return Business.Do<IOrganization>().OrganSingle(id);
        }
        /// <summary>
        /// 获取所有可用的机构
        /// </summary>
        /// <returns></returns>
        public EntitiesInfo.Organization[] All()
        {
            return Business.Do<IOrganization>().OrganAll(true, -1);
        }
        /// <summary>
        /// 当前机构
        /// </summary>
        /// <returns></returns>
        public EntitiesInfo.Organization Current()
        {
            return Business.Do<IOrganization>().OrganCurrent();
        }
    }
}
