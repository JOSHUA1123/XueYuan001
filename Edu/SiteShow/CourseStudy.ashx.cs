using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using EntitiesInfo;
using System.Data;
using System.IO;

namespace SiteShow
{
    /// <summary>
    /// 课程学习（web端）
    /// </summary>
    public class CourseStudy : BasePage
    {
        //章节id,课程id
        int id = Common.Request.QueryString["olid"].Int32 ?? 0;
        int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
           
        }
        /// <summary>
        /// 判断是否必须在桌面应用中学习
        /// </summary>
        /// <returns>如果为true，则必须在课面应用中学习</returns>
        private bool getForDeskapp(EntitiesInfo.Course course, EntitiesInfo.Outline ol)
        {
            //自定义配置项
            Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
            //是否限制在桌面应用中学习
            bool studyFordesk = config["StudyForDeskapp"].Value.Boolean ?? false;   //课程学习需要在桌面应用打开
            bool freeFordesk = config["FreeForDeskapp"].Value.Boolean ?? false;     //免费课程和试用章节除外
            if (!Common.Browser.IsDestopApp)
            {
                if (!freeFordesk)
                {
                    return studyFordesk && !Common.Browser.IsDestopApp;
                }
                else
                {
                    if (course.Cou_IsFree || course.Cou_IsLimitFree) return false;
                    if (ol.Ol_IsFree) return false;
                }
            }
            return true && !Common.Browser.IsDestopApp;
        }
    }
}