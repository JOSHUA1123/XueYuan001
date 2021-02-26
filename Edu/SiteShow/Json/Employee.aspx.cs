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
using System.Text;
using System.Reflection;

namespace SiteShow.Json
{
    public partial class Employee : System.Web.UI.Page
    {       
        //员工id
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.EmpAccount emp = null;
            if (id < 1) 
                emp = Extend.LoginState.Admin.CurrentUser;
            else
                emp = Business.Do<IEmployee>().GetSingle(id);
            string str = "";
            if (emp != null)
            {
                //资源的路径
                string resPath = Upload.Get["Employee"].Virtual;
                emp.Acc_Photo = resPath + emp.Acc_Photo;
                emp.Acc_Pw = "";
                str = emp.ToJson(null, "Acc_Pw,Acc_Qus,Acc_Ans,Dep_CnName,Acc_IDCardNumber");
            }
            Response.Write(str);
            Response.End();
        }
    }
}
