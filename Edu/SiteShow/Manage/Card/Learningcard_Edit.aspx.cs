using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SiteShow.Manage.Card
{
    public partial class Learningcard_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            Lcs_SecretKey.Enabled = id < 1;
        }
        void fill()
        {            
            EntitiesInfo.LearningCardSet set = id == 0 ? new EntitiesInfo.LearningCardSet() : Business.Do<ILearningCard>().SetSingle(id);
            if (id > 0)
            {
                this.EntityBind(set);
                //当前学习卡关联的课程
                EntitiesInfo.Course[] courses = Business.Do<ILearningCard>().CoursesGet(set);
                if (courses != null)
                {
                    rtpCourses.DataSource = courses;
                    rtpCourses.DataBind();
                    foreach (EntitiesInfo.Course c in courses)
                    {
                        tbCourses.Text += c.Cou_ID + ",";
                    }
                }
            }
            if (id <1)
            {
                //如果是新增   
                Lcs_LimitStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Lcs_LimitEnd.Text = DateTime.Now.AddMonths(12).ToString("yyyy-MM-dd");               
                Lcs_SecretKey.Text = getUID();
            }            
            
        }       
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.LearningCardSet set = id != 0 ? Business.Do<ILearningCard>().SetSingle(id) : new EntitiesInfo.LearningCardSet();
            set = this.EntityFill(set) as EntitiesInfo.LearningCardSet;
            //关联的课程
            set = Business.Do<ILearningCard>().CoursesSet(set, tbCourses.Text.Trim());
            try
            {
                if (id == 0)
                {
                    Business.Do<ILearningCard>().SetAdd(set);
                }
                else
                {
                    Business.Do<ILearningCard>().SetSave(set);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            } 
        }
       
    }
}
