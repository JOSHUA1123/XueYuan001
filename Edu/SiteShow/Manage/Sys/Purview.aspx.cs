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

namespace SiteShow.Manage.Sys
{
    public partial class Purview : Extend.CustomPage
    {
        string type = Common.Request.QueryString["type"].String;
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            try
            {
                switch (type.ToLower())
                {
                    case "posi":
                        EntitiesInfo.Position p = Business.Do<IPosition>().GetSingle(id);
                        ltName.Text = p.Posi_Name;
                        break;
                    case "group":
                        EntitiesInfo.EmpGroup e = Business.Do<IEmpGroup>().GetSingle(id);
                        ltName.Text = e.EGrp_Name;
                        break;
                    case "depart":
                        EntitiesInfo.Depart d = Business.Do<IDepart>().GetSingle(id);
                        ltName.Text = d.Dep_CnName;
                        break;
                    case "organ":
                        ltName.Text = "所有机构";
                        break;
                    case "orglevel":
                        EntitiesInfo.OrganLevel lv = Business.Do<IOrganization>().LevelSingle(id);
                        ltName.Text = "机构等级："+lv.Olv_Name;
                        break;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
