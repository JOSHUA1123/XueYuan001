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
using System.Data.OleDb;

using Common;

using ServiceInterfaces;
using EntitiesInfo;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SiteShow.Manage.Questions
{
    public partial class Questions_Input5 : Extend.CustomPage
    {
        //����Ϊ�����
        int type=Common.Request.QueryString["type"].Int32 ?? 0;
        EntitiesInfo.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ExcelInput1_OnInput(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            //�������е�����
            DataTable dt = ExcelInput1.SheetDataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //throw new Exception();
                    //���������е������ݿ�
                    _inputData(dt.Rows[i]);                    
                }
                catch
                {
                    //��������������з��ظ��ؼ�
                    ExcelInput1.AddError(dt.Rows[i]);
                }
            }
            Business.Do<IQuestions>().OnSave(null, EventArgs.Empty);
            Business.Do<IOutline>().OnSave(null, EventArgs.Empty);
        }
       

        #region ��������
        
        /// <summary>
        /// ��ĳһ�����ݼ��뵽���ݿ�
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        private void _inputData(DataRow dr)
        {
            EntitiesInfo.Questions obj = new EntitiesInfo.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = this.type;
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {
                //Excel���е�ֵ
                string column = dr[rel.Key].ToString();
                //���ݿ��ֶε�����
                string field = rel.Value;
                if (field == "Qus_ID")
                {
                    if (column == string.Empty || column.Trim() == "") continue;
                    int ques = Convert.ToInt32(column);
                    EntitiesInfo.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //����Ѷȡ�רҵ�����⽲��
                if (field == "Qus_Title")
                {
                    if (string.IsNullOrEmpty(column) || column.Trim() == "") return;
                    obj.Qus_Title = column;
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    EntitiesInfo.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(org.Org_ID, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name")
                {
                    EntitiesInfo.Course course = Business.Do<ICourse>().CourseBatchAdd(org.Org_ID, obj.Sbj_ID, column);
                    if (course != null) obj.Cou_ID = course.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    EntitiesInfo.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
                //Ψһֵ ��ȷ�𰸣�����
                obj.Qus_UID = Common.Request.UniqueID();
                if (field == "Qus_Answer")
                {
                    if (column == string.Empty || column.Trim() == "") obj.Qus_IsError = true;
                    obj.Qus_Answer = column;
                }
            }
            //�ٱ���һ�飬ȡ��
            //int ansNum = 0;
            //List<EntitiesInfo.QuesAnswer> ansItem = new List<QuesAnswer>();
            //foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            //{
            //    //���ݿ��ֶε�����
            //    string field = rel.Value;
            //    Match match = new Regex(@"(Ans_Context)(\d+)").Match(field);
            //    if (match.Success)
            //    {
            //        //Excel���е�ֵ
            //        string column = dr[rel.Key].ToString();
            //        if (column == string.Empty || column.Trim() == "") continue;                       
            //        EntitiesInfo.QuesAnswer ans = new EntitiesInfo.QuesAnswer();
            //        ans.Ans_Context = column;                      
            //        ans.Qus_UID = obj.Qus_UID;
            //        ansNum++;
            //        ansItem.Add(ans);
            //    }
            //}
            //obj.Qus_Title = tranTxt(obj.Qus_Title);
            //int bracketsCount = new Regex(@"��[^��]+��").Matches(obj.Qus_Title).Count;
            ////�ж��Ƿ��д�
            //string error = "";
            //if (bracketsCount <= 0) error = "������ȱ������������������ű�ʶ��";
            //if (ansNum <= 0) error = "ȱ�ٴ���";
            //if (ansNum < bracketsCount) error = string.Format("�����������������{0}��,��{1}��", bracketsCount, ansNum);
            ////
            //obj.Qus_IsError = error != "";
            //obj.Qus_ErrorInfo = error;
            obj.Qus_ErrorInfo = "";
            if (obj.Sbj_ID == 0) throw new Exception("��ǰ��������רҵ��������");
            if (obj.Cou_ID == 0) throw new Exception("��ǰ�������ڿγ̲�������");
            //if (obj.Ol_ID == 0) throw new Exception("��ǰ���������½ڲ�������");
            if (org != null) obj.Org_ID = org.Org_ID;
            Business.Do<IQuestions>().QuesInput(obj, null);   
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string tranTxt(string txt)
        {
            txt = txt.Replace("(","��");
            txt = txt.Replace(")", "��");
            txt = txt.Replace("��", "�� ");
            txt = Regex.Replace(txt, @"��[^��]_+��", "��______��");            
            return txt;
        }        
        #endregion
      
    }
}
