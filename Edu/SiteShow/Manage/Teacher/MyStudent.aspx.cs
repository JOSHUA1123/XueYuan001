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
using System.IO;
using System.Collections.Generic;

namespace SiteShow.Manage.Teacher
{
    public partial class MyStudent : Extend.CustomPage
    {
        EntitiesInfo.Organization org;
        protected int couid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                bindTree();
                BindData(null, null);
            }
        }
        /// <summary>
        /// �󶨿γ�
        /// </summary>
        private void bindTree()
        {
            //��ǰ��ʦ
            EntitiesInfo.Teacher th = Extend.LoginState.Accounts.Teacher;
            List<EntitiesInfo.Course> eas = null;
            eas = Business.Do<ICourse>().CourseCount(org.Org_ID, -1, th.Th_ID, -1, null, null, -1);
            ddlCourse.DataSource = eas;
            ddlCourse.DataTextField = "Cou_Name";
            ddlCourse.DataValueField = "Cou_ID";
            ddlCourse.DataBind();
            //this.ddlCourse.Items.Insert(0, new ListItem(" -- �ҵ����пγ� -- ", "-1")); 
        }

        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //�ܼ�¼��
            int count = 0;
            int.TryParse(ddlCourse.SelectedValue, out couid);
            EntitiesInfo.Accounts[] eas = null;
            eas = Business.Do<ICourse>().Student4Course(couid, tbStName.Text.Trim(), tbStMobi.Text.Trim(), Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Ac_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Teacher entity = Business.Do<ITeacher>().TeacherSingle(id);
            entity.Th_IsUse = !entity.Th_IsUse;
            Business.Do<ITeacher>().TeacherSave(entity);
            BindData(null, null);          
        }
        /// <summary>
        /// ע������Ƿ�ͨ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbPass_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Teacher entity = Business.Do<ITeacher>().TeacherSingle(id);
            entity.Th_IsPass = !entity.Th_IsPass;
            Business.Do<ITeacher>().TeacherSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<ITeacher>().TeacherDelete(Convert.ToInt32(id));
            }
            BindData(null, null);            
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {            
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<ITeacher>().TeacherDelete(id);
            BindData(null, null);           
        }
        /// <summary>
        /// ������ʦ��Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void outputEvent(object sender, EventArgs e)
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //�����ļ�
            string name = "��ʦ��Ϣ����" + "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
            string filePath = Upload.Get["Temp"].Physics + name;
            filePath = Business.Do<ITeacher>().TeacherExport4Excel(filePath, org.Org_ID, -1);
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
    }
}
