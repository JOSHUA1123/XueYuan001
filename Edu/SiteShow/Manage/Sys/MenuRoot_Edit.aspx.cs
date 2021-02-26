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
using System.IO;

using Common;

using ServiceInterfaces;
using EntitiesInfo;

namespace SiteShow.Manage.Sys
{
    public partial class MenuRoot_Edit : Extend.CustomPage
    {
        //Ҫ�����Ķ�������
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
           
        }
        /// <summary>
        /// ���ó�ʼ����
        /// </summary>
        private void fill()
        {
            try
            {
                EntitiesInfo.ManageMenu mm;
                if (id != 0)
                {
                    mm = Business.Do<IManageMenu>().GetSingle(id);
                    //�Ƿ���ʾ
                    cbIsShow.Checked = mm.MM_IsShow;
                    cbIsUse.Checked = mm.MM_IsUse;
                    //�Ƿ����
                    cbIsBold.Checked = mm.MM_IsBold;
                    //�Ƿ�б��
                    cbIsItalic.Checked = mm.MM_IsItalic;   
                }
                else
                {
                    //���������
                    mm = new EntitiesInfo.ManageMenu();
                }
                //����
                tbName.Text = mm.MM_Name;      
                //��ʶ
                tbMarker.Text = mm.MM_Marker;
                //˵��
                tbIntro.Text = mm.MM_Intro;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                EntitiesInfo.ManageMenu mm;
                if (id != 0)
                {
                    mm = Business.Do<IManageMenu>().GetSingle(id);
                }
                else
                {
                    //���������
                    mm = new ManageMenu();
                    mm.MM_IsShow = true;
                }
                //���ڹ��ܲ˵�
                mm.MM_Func = "func";
                //���
                mm.MM_Type = "item";
                //����
                mm.MM_Name = tbName.Text.Trim();
                mm.MM_Marker = tbMarker.Text.Trim();
                //�Ƿ����
                mm.MM_IsBold = cbIsBold.Checked;
                //�Ƿ�б��
                mm.MM_IsItalic = cbIsItalic.Checked;
                //�Ƿ���ʾ
                mm.MM_IsShow = cbIsShow.Checked;
                mm.MM_IsUse = cbIsUse.Checked;
                //˵��
                mm.MM_Intro = tbIntro.Text;
                //ȷ������
                if (id == 0)
                {
                    Business.Do<IManageMenu>().RootAdd(mm);
                }
                else
                {
                    Business.Do<IManageMenu>().RootSave(mm);
                }
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ɾ���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<IManageMenu>().RootDelete(id);
                Master.AlertCloseAndRefresh("�ɹ�ɾ��");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

        

    }
}
