using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using EntitiesInfo;
using VTemplate.Engine;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 手机端的首页
    /// </summary>
    public class Default : BasePage
    {
       
        protected override void InitPageTemplate(HttpContext context)
        {
            
            //当前选中的课程
            EntitiesInfo.Course currCourse = Extend.LoginState.Accounts.Course();
            this.Document.SetValue("currCourse", currCourse);
            //微信登录
            this.Document.SetValue("WeixinLoginIsUse", Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? false);

            //已经购买的课程
            int stid = this.Account != null ? this.Account.Ac_ID : 0;
            if (stid > 0)
            {
                //资讯列表
                Tag tag = this.Document.GetChildTagById("buyCou");
                if (tag != null)
                {
                    int count = int.Parse(tag.Attributes.GetValue("count", "3"));
                    List<EntitiesInfo.Course> buyCou = Business.Do<ICourse>().CourseForStudent(stid, null, 1, false, count);
                    for (int i = 0; i < buyCou.Count; i++) buyCou[i].Cou_Intro = string.Empty;
                    this.Document.SetValue("buyCou", buyCou);
                }
            }
        }
    }
}