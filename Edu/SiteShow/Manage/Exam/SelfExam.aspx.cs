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

namespace SiteShow.Manage.Exam
{
    public partial class SelfExam : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(null, null);
            }
        }

        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            //׼����ʼ�Ŀ���
            DateTime start = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime end = start.AddDays(1);
            List<EntitiesInfo.Examination> eas = Business.Do<IExamination>().GetSelfExam(st.Ac_ID, start, end);
            rptExamStart.DataSource = eas;
            rptExamStart.DataBind();
            //����Ҫ��չ�Ŀ���
            start = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(1);
            rtpExamList.DataSource = Business.Do<IExamination>().GetSelfExam(st.Ac_ID, start, null);
            rtpExamList.DataBind();
           
        }    
      
     
        /// <summary>
        /// ��ȡ�ο���Ա����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string getGroupType(string gtype,string uid)
        {
            //try
            //{
            //    int type = Convert.ToInt32(gtype);
            //    if (type == 1) return "ȫ��Ա��";
            //    if (type == 2)
            //    {
            //        EntitiesInfo.Depart[] deps = Business.Do<IExamination>().GroupForDepart(uid);
            //        string strDep = "��Ժϵ��";
            //        for (int i = 0; i < deps.Length; i++)
            //        {
            //            strDep += deps[i].Dep_CnName;
            //            if (i < deps.Length - 1) strDep += ",";
            //        }
            //        return strDep;
            //    }
            //    if (type == 3)
            //    {
            //        EntitiesInfo.Team[] teams = Business.Do<IExamination>().GroupForTeam(uid);
            //        string strTeam = "�����飩";
            //        for (int i = 0; i < teams.Length; i++)
            //        {
            //            strTeam += teams[i].Team_Name;
            //            if (i < teams.Length - 1) strTeam += ",";
            //        }
            //        return strTeam;
            //    }
            //    return "";
            //}
            //catch (Exception ex)
            //{
            //    Message.ExceptionShow(ex);
            //    return "";
            //}
            return "";
        }
    }
}
