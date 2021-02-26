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



namespace SiteShow.Manage.Teacher
{
    public partial class Sort_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //Ա���ϴ����ϵ�����·��
        //private string _uppath = "Teacher";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        private void fill()
        {
            EntitiesInfo.TeacherSort th = id == 0 ? new EntitiesInfo.TeacherSort() : Business.Do<ITeacher>().SortSingle(id);
            if (th == null) return;
            if (id != 0) this.EntityBind(th);          
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.TeacherSort th = id == 0 ? new EntitiesInfo.TeacherSort() : Business.Do<ITeacher>().SortSingle(id);
            th = this.EntityFill(th) as EntitiesInfo.TeacherSort;
            //�ж��Ƿ�����
            if (Business.Do<ITeacher>().SortIsExist(th))
            {
                Master.Alert("��ǰѧ�������Ѿ����ڣ�");
            }
            else
            {
                try
                {
                    if (id == 0)
                    {
                        Business.Do<ITeacher>().SortAdd(th);
                    }
                    else
                    {
                        Business.Do<ITeacher>().SortSave(th);
                    }

                    Master.AlertCloseAndRefresh("�����ɹ���");
                }
                catch (Exception ex)
                {
                    Master.Alert(ex.Message);
                }
            }
        }
       
       
       
    }
}
