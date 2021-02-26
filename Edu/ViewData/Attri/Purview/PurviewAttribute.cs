﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewData.Attri
{
    /// <summary>
    /// 权限验证
    /// </summary>
    public class PurviewAttribute : Attribute
    {
        private bool _ignore = false;
        /// <summary>
        /// 忽略此方法，默认为false
        /// 说明：则某个类设置了特性后，下面的所有方法都需要验证，除非设置[Admin(Ignore=true)]
        /// </summary>
        public bool Ignore
        {
            get { return _ignore; }
            set { _ignore = value; }
        }
        public PurviewAttribute()
        {

        }
        public PurviewAttribute(bool ignore)
        {
            _ignore = ignore;
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool Logged()
        {
            //return Helper.Admin.Singleton.CurrentUser != null;
            return Extend.LoginState.Admin.CurrentUser != null;
        }
        /// <summary>
        /// 将执行结果写入日志
        /// </summary>
        /// <param name="execResult"></param>
        public void LogWrite(object execResult)
        {

        }
    }
}
