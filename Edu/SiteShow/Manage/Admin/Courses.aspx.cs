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

namespace SiteShow.Manage.Admin
{
    public partial class Courses : Extend.CustomPage
    {
        EntitiesInfo.Organization org = null;
        //�Ƿ�Ϊ����Ա����״̬
        private bool isAdmin = Common.Request.QueryString["admin"].Boolean ?? false;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            this.Title = "�γ̹���";
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                bindTree();
                BindData(null, null);
            }           
        }
        /// <summary>
        /// �󶨵���
        /// </summary>
        private void bindTree()
        {
            ddlSubject.Items.Clear();
            int depid = 0;
            EntitiesInfo.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(org.Org_ID, depid, "", null, 0, 0);
            ddlSubject.DataSource = sbjs;
            ddlSubject.DataTextField = "Sbj_Name";
            ddlSubject.DataValueField = "Sbj_ID";
            ddlSubject.DataBind();
            this.ddlSubject.Items.Insert(0, new ListItem(" -- רҵ -- ", "-1"));
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //�ܼ�¼��
            int count = 0;
            bool? isUse = null;
            List<EntitiesInfo.Course> eas = null;
            int sbjid = Convert.ToInt32(ddlSubject.SelectedValue);
            eas = Business.Do<ICourse>().CoursePager(org.Org_ID, sbjid, -1, isUse, tbSear.Text, "tax", Pager1.Size, Pager1.Index, out count);
            foreach (EntitiesInfo.Course s in eas)
            {
                if (string.IsNullOrEmpty(s.Sbj_Name) || s.Sbj_Name.Trim() == "")
                {
                    EntitiesInfo.Subject subject = Business.Do<ISubject>().SubjectSingle(s.Sbj_ID);
                    if (subject != null) s.Sbj_Name = subject.Sbj_Name;
                    Business.Do<ICourse>().CourseSave(s);
                }
                if (string.IsNullOrEmpty(s.Cou_Intro) || s.Cou_Intro.Trim() == "") continue;
                if (s.Cou_Intro.Length > 20)
                {
                    s.Cou_Intro = s.Cou_Intro.Substring(0, 20) + "...";
                }                
            }
            //DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(eas.ToArray());
            //WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            //tree.IdKeyName = "Cou_ID";
            //tree.ParentIdKeyName = "Cou_PID";
            //tree.TaxKeyName = "Cou_Tax";
            //tree.Root = 0;
            //dt = tree.BuilderTree(dt);


            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Cou_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;            
        }
        /// <summary>
        /// ��ȡ��ǰרҵ�µĿγ�����
        /// </summary>
        /// <param name="sbjid"></param>
        /// <returns></returns>
        protected string GetCourseCount(object sbjid)
        {
            int sbj;
            int.TryParse(sbjid.ToString(), out sbj);
            int count = Business.Do<ICourse>().CourseOfCount(org.Org_ID, sbj, -1);
            return count.ToString();
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// ��յ�ǰרҵ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbClear_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<ICourse>().CourseClear(id);
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ�ʹ�õ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Course entity = Business.Do<ICourse>().CourseSingle(id);
            entity.Cou_IsUse = !entity.Cou_IsUse;
            Business.Do<ICourse>().CourseSave(entity);
            BindData(null, null);           
        }
        /// <summary>
        /// �޸��Ƿ��Ƽ���״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbRec_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Course entity = Business.Do<ICourse>().CourseSingle(id);
            entity.Cou_IsRec = !entity.Cou_IsRec;
            Business.Do<ICourse>().CourseSave(entity);
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
                Business.Do<ICourse>().CourseDelete(Convert.ToInt32(id));
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
            Business.Do<ICourse>().CourseDelete(id);
            BindData(null, null);

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {            
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ICourse>().CourseUp(id))
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {            
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ICourse>().CourseDown(id))
            {
                BindData(null, null);
            }
        }

        #region ָ�ɿγ̽�ʦ        
        
        /// <summary>
        /// GridView�а��¼�����Ҫ�ǲ����γ���ʦ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             //��Ϊ������ʱ
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                EntitiesInfo.Teacher[] eas = Business.Do<ITeacher>().TeacherCount(org.Org_ID, true, -1);
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlTeacher");
                ddl.DataSource = eas;
                ddl.DataTextField = "th_Name";
                ddl.DataValueField = "th_ID";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("��", "-1"));
                //��ʦid
                Label lbThid = (Label)e.Row.FindControl("lbThID");
                ListItem liThid = ddl.Items.FindByValue(lbThid.Text);
                if (liThid != null) liThid.Selected = true;
            }
        }
        protected void ddlTeacher_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //��ʦ�����б�
            DropDownList ddl = (DropDownList)sender;
            //ȡ��ǰ�γ�
            GridViewRow gr = (GridViewRow)(ddl).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            EntitiesInfo.Course cou = Business.Do<ICourse>().CourseSingle(id);
            //���ý�ʦ
            int thid;
            int.TryParse(ddl.SelectedValue.ToString(), out thid);
            cou.Th_ID = thid;
            cou.Th_Name = ddl.SelectedItem.Text;
            Business.Do<ICourse>().CourseSave(cou);
            BindData(null, null);
        }
        #endregion
    }
}
