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
using System.Net;
using System.Drawing;
namespace SiteShow.Manage
{
    public partial class Welcome : Extend.CustomPage
    {
        //
        protected Hashtable numitem = new Hashtable(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            numItem();  //各项数值
            fillServerInfo();
            datainit();
            //当前系统版本号
            //lbVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();           
        }
        private void numItem()
        {
            //课程数，试题数
            ViewState["counum"] = Business.Do<ICourse>().CourseOfCount(-1, -1, -1);
            ViewState["quesnum"] = Business.Do<IQuestions>().QuesOfCount(-1, -1, -1, -1, -1, null);
            //学员数,在线数
            ViewState["accnum"] = Business.Do<IAccounts>().AccountsOfCount(-1, null);
            ViewState["onlinenum"] = Extend.LoginState.Accounts.OnlineCount;
        }
        /// <summary>
        /// 填充服务器信息，探针
        /// </summary>
        private void fillServerInfo()
        {
            try
            {
                //IP、端口、域名
                ltIp.Text = Common.Server.IP + ":" + Common.Server.Port;
                //ltDomain.Text = Common.Server.Domain;
                //操作系统，IIS版本
                ltOs.Text = Common.Server.OS;
                ltIISver.Text = Common.Server.IISVersion;
                ltDotNetver.Text = Common.Server.DotNetVersion;
                //硬件,cpu个数，主频，内存大小
                ltCpucount.Text = Common.Server.CPUCount.ToString();
                ltHz.Text = Common.Server.CPUHz;
                ltRamSize.Text = Common.Server.RamSize.ToString();
                //程序所在路径
                ltPath.Text = Common.Server.ProgramPath;
                ////数据库类型
                //ltDatabaseType.Text = Common.Server.DatabaseType.ToUpper();
            }
            catch (Exception ex)
            {
                Common.Log.Error(this.GetType().ToString(), ex);
            }
        }
        /// <summary>
        /// 相关数据的初始化
        /// </summary>
        private void datainit()
        {
            try
            {
                string sql = @"select top {1} o.Org_ID,o.Org_Name,o.Org_PlatformName,o.Org_AbbrName,c.count 
                        from(select Org_ID,COUNT(*) as count from {0}   
                        group by Org_ID ) as c
                        right join Organization as o on o.org_id=c.org_id order by count desc";
                //各机构的课程数
                rptCourse.DataSource = Business.Do<ISystemPara>().ForSql(string.Format(sql, "Course", 10));
                rptCourse.DataBind();
                //学员数量
                rptAccouts.DataSource = Business.Do<ISystemPara>().ForSql(string.Format(sql, "Accounts", 10));
                rptAccouts.DataBind();
                //试题数量
                rptQues.DataSource = Business.Do<ISystemPara>().ForSql(string.Format(sql, "Questions", 10));
                rptQues.DataBind();
            }
            catch (Exception ex)
            {
                Common.Log.Error(this.GetType().ToString(), ex);
            }
        }
    }
}
