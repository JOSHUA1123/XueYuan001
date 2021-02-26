using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections;
using Common;


using EntitiesInfo;
using ServiceInterfaces;

namespace Extend.Login
{
    public class Teacher
    {
        private static readonly Teacher _singleton = new Teacher();
        /// <summary>
        /// 获取参数
        /// </summary>
        public static Teacher Singleton
        {
            get { return _singleton; }
        }
        #region 属性

        /// <summary>
        /// 后台管理登录的状态管理方式，值为Cookies或Session
        /// </summary>
        public static LoginPatternEnum LoginPattern
        {
            get
            {
                string tm = Common.Login.Get["Teacher"].Pattern.String;
                if (tm.Equals("Session", StringComparison.CurrentCultureIgnoreCase)) return LoginPatternEnum.Session;
                if (tm.Equals("Cookies", StringComparison.CurrentCultureIgnoreCase)) return LoginPatternEnum.Cookies;
                return LoginPatternEnum.Cookies;
            }
        }
        private List<EntitiesInfo.Teacher> _onlineUser = new List<EntitiesInfo.Teacher>();
        /// <summary>
        /// 在线用户
        /// </summary>
        public List<EntitiesInfo.Teacher> OnlineUser
        {
            get { return _onlineUser; }
        }
        /// <summary>
        /// 返回当前登录用户的实体
        /// </summary>
        /// <returns></returns>
        public EntitiesInfo.Teacher CurrentUser
        {
            get
            {
                EntitiesInfo.Teacher curr = null;
                //首先从在线列表中取当前登录的用户
                if (this._onlineUser.Count > 0)
                {
                    foreach (EntitiesInfo.Teacher em in this.OnlineUser)
                    {
                        if (em == null) continue;
                        if (em.Th_ID == this.CurrentUserId)
                        {
                            curr = em;
                            break;
                        }
                    }
                }
                //内存中没有的话就从数据库取
                if (curr == null)
                {
                    if (this.CurrentUserId > 0)
                        curr = Business.Do<ITeacher>().TeacherSingle(this.CurrentUserId);
                    if (curr != null) this._onlineUser.Add(curr);
                    if (curr == null && this.CurrentUserId > 0) this.Logout();
                }
                //当前教师是否处在当前机构，主要是在切换机构后，仍然在线的问题
                if (curr != null)
                {
                    EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
                    curr = curr.Org_ID == org.Org_ID ? curr : null;
                }
                return curr;
            }
        }
        /// <summary>
        /// 当前登录用户的id
        /// </summary>
        public int CurrentUserId
        {
            get
            {
                int accid = 0;
                //登录标识名
                string key = Common.Login.Get["Teacher"].KeyName.String;
                string domain = Common.Request.Domain.TwoDomain;
                key = domain + "_" + key;
                if (Teacher.LoginPattern == LoginPatternEnum.Cookies)
                    accid = Common.Request.Cookies[key].Int32 ?? 0;
                if (Teacher.LoginPattern == LoginPatternEnum.Session)
                    accid = Common.Request.Session[key].Int32 ?? 0;
                return accid;
            }
        }
        /// <summary>
        /// 用户是否登录
        /// </summary>
        public bool IsLogin
        {
            get { return this.CurrentUserId != 0; }
        }
        #endregion

        #region 登录与注销的方法
        /// <summary>
        /// 将已经登录入的用户，写入seesion
        /// </summary>
        /// <param name="acc"></param>
        public void Write(EntitiesInfo.Teacher acc)
        {
            this.Write(acc, -1);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">教师</param>
        /// <param name="expiresDay">登录时效，单位：天</param>
        public void Write(EntitiesInfo.Teacher acc, int expiresDay)
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //登录标识名
            string key = Common.Login.Get["Teacher"].KeyName.String;
            string domain = Common.Request.Domain.TwoDomain;
            key = domain + "_" + key;
            if (Teacher.LoginPattern == LoginPatternEnum.Cookies)
            {

                System.Web.HttpCookie cookie = new System.Web.HttpCookie(key);
                cookie.Value = acc.Th_ID.ToString();
                string exp = Common.Login.Get["Teacher"].Expires.String;
                if (expiresDay <= 0)
                {
                    if (!exp.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                        cookie.Expires = DateTime.Now.AddMinutes(Common.Login.Get["Teacher"].Expires.Int32 ?? 10);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(expiresDay);
                }
                _context.Response.Cookies.Add(cookie);
            }
            if (Teacher.LoginPattern == LoginPatternEnum.Session)
            {
                _context.Session[key] = acc.Th_ID;
            }
            this._register(acc);
        }
        /// <summary>
        /// 写入当前用户
        /// </summary>
        public void Write()
        {
            //先读取当前用户，再写入
            EntitiesInfo.Teacher ea = this.Read();
            this.Write(ea);
        }
        /// <summary>
        /// 获取当前登录用户的对象
        /// </summary>
        /// <returns></returns>
        public EntitiesInfo.Teacher Read()
        {
            int accid = this.CurrentUserId;
            if (accid < 1) return null;

            EntitiesInfo.Teacher acc = this.CurrentUser;
            if (acc == null) acc = Business.Do<ITeacher>().TeacherSingle(accid);
            return acc;
        }
        /// <summary>
        /// 注册已经登录的在线用户，如果已经注册，则更新注册时间
        /// </summary>
        public void Register()
        {
            EntitiesInfo.Teacher ea = this.Read();
            if (ea != null) _register(ea);
        }
        /// <summary>
        /// 注册某个用户到在线列表中
        /// </summary>
        /// <param name="acc"></param>
        private void _register(EntitiesInfo.Teacher acc)
        {
            if (acc == null) return;
            //登录时间，该时间不入数据库，仅为临时使用
            acc.Th_LastTime = DateTime.Now;
            //登录用户是否已经存在;
            bool isHav = false;
            for (int i = 0; i < this._onlineUser.Count; i++)
            {
                EntitiesInfo.Teacher e = this._onlineUser[i];
                if (e == null) continue;
                if (e.Th_ID == acc.Th_ID)
                {
                    this._onlineUser[i] = acc;
                    isHav = true;
                    break;
                }
            }
            //如果未登录，则注册进去
            if (!isHav) this._onlineUser.Add(acc);
        }
        /// <summary>
        /// 注销当前用户
        /// </summary>
        public void Logout()
        {
            int accid = this.CurrentUserId;
            if (accid < 1) return;
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //登录标识名
            string key = Common.Login.Get["Teacher"].KeyName.String;
            string domain = Common.Request.Domain.TwoDomain;
            key = domain + "_" + key;
            if (Teacher.LoginPattern == LoginPatternEnum.Cookies)
                _context.Response.Cookies[key].Expires = DateTime.Now.AddYears(-1);
            if (Teacher.LoginPattern == LoginPatternEnum.Session)
                _context.Session.Abandon();
            CleanOut();
        }
        /// <summary>
        /// 清理超时用户
        /// </summary>
        public void CleanOut()
        {
            //设置超时时间，单位分钟
            int outTimeNumer = 3;
            List<EntitiesInfo.Teacher> _tm = new List<EntitiesInfo.Teacher>();
            foreach (EntitiesInfo.Teacher em in this.OnlineUser)
            {
                if (em == null) continue;
                if (DateTime.Now.AddMinutes(-outTimeNumer) > em.Th_LastTime)
                {
                    _tm.Add(em);
                }
            }
            this._onlineUser = _tm;
        }
        /// <summary>
        /// 验证是否登录，没有登录，则跳转
        /// </summary>
        public void VerifyLogin()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //获取当前文件的文件名
            string path = _context.Request.ServerVariables["PATH_INFO"];
            path = path.Substring(path.LastIndexOf("/") + 1);
            //如果不是首页
            if (path != "index.aspx")
            {
                if (!this.IsLogin)
                {
                    //如果未登录
                    string url = Common.Login.Get["Teacher"].NoLoginPath.String ?? "/";
                    _context.Response.Redirect(url);
                }
            }
        }
        #endregion



    }
}
