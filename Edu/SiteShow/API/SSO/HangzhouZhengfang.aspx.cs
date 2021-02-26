using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using ServiceInterfaces;
using EntitiesInfo;
namespace SiteShow.API
{
    public partial class HangzhouZhengfang : System.Web.UI.Page
    {
        
        string verify = Common.Request.QueryString["verify"].String;                 //md5加密的校验码   
        string userName = Common.Request.QueryString["userName"].String;             //用户名        
        string strSysDatetime = Common.Request.QueryString["strSysDatetime"].String; //时间戳        
        string jsName = Common.Request.QueryString["jsName"].String;                 //角色名        
        string url = Common.Request.QueryString["url"].UrlDecode;                    //转换的Url
        //握手密钥
        string zfkey = Common.App.Get["SSOzfkey"].String;
        //
        //EntitiesInfo.Organization Organ = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ////如果传递参数都有，进行验证
            //if (!string.IsNullOrWhiteSpace(this.Page.ClientQueryString))
            //{
            //    string verStr = new Common.Param.Method.ConvertToAnyValue(userName + zfkey + strSysDatetime + jsName).MD5;//加密参数
            //    //参数验证成功
            //    if (verify.Equals(verStr.ToUpper()))
            //    {
            //        this.Organ = Business.Do<IOrganization>().OrganCurrent();
            //        bool isHave = Business.Do<ITeacher>().IsTeacherExist(Organ.Org_ID, userName);
            //        if (!isHave)
            //        {
            //            Response.Write("教师 " + userName + " 不存在！");
            //            Response.End();
            //            return;
            //        }
            //        else
            //        {
            //            //教师存在
            //            EntitiesInfo.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(userName, Organ.Org_ID);
            //            Extend.LoginState.Teacher.Write(teacher);
            //            this.Response.Redirect(string.IsNullOrWhiteSpace(url) ? "/" : url);
            //        }
            //    }
            //}
            //else
            //{
            //    //如果没有任何参数
            //    //当前登录的教师
            //    EntitiesInfo.Teacher teacher = Extend.LoginState.Accounts.Teacher;
            //    if (teacher == null)
            //    {
            //        Response.Write("#");
            //        Response.End();
            //        return;
            //    }
            //    else
            //    {
            //        strSysDatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //        string verstr = teacher.Th_AccName + zfkey + strSysDatetime + "teacher";
            //        string verstrMd5 = new Common.Param.Method.ConvertToAnyValue(verstr).MD5.ToUpper();
            //        string ssopath = "{0}?verify={1}&userName={2}&strSysDatetime={3}&jsName={4}&url={5}";
            //        ssopath = string.Format(ssopath,
            //            Common.App.Get["SSOpath"].String,
            //            verstrMd5,
            //            teacher.Th_AccName,
            //            strSysDatetime,
            //            "teacher",
            //             Common.App.Get["SSOurl"].UrlEncode);
            //        Response.Write(ssopath);
            //        Response.End();
            //        return;

            //    }
            //}
        }
    }
}