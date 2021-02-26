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
using System.Collections.Generic;

namespace SiteShow.Manage.Teacher
{
    public partial class Input : Extend.CustomPage
    {
        
        //������
        EntitiesInfo.TeacherSort[] sorts = null;
        EntitiesInfo.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        

        protected void ExcelInput1_OnInput(object sender, EventArgs e)
        {
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
        }

        #region ��������
       
        /// <summary>
        /// ��ĳһ�����ݼ��뵽���ݿ�
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        private void _inputData(DataRow dr)
        {
            //ȡ���з���
            if (org == null) org = Business.Do<IOrganization>().OrganCurrent();           
            if (this.sorts == null)
                this.sorts = Business.Do<ITeacher>().SortCount(org.Org_ID, null, 0);
            EntitiesInfo.Accounts acc = null;
            EntitiesInfo.Teacher teacher = null;
            bool isExistAcc= false;   //�Ƿ���ڸ��˺�
            bool isExistTh = false;   //�Ƿ���ڸý�ʦ
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {
                //Excel���е�ֵ
                string column = dr[rel.Key].ToString();
                //���ݿ��ֶε�����
                string field = rel.Value;
                if (field == "Th_PhoneMobi")
                {
                    acc = Business.Do<IAccounts>().AccountsSingle(column, -1);                    
                    if (acc != null)                    
                        teacher = Business.Do<IAccounts>().GetTeacher(acc.Ac_ID, null);
                    isExistAcc = acc != null;
                    isExistTh = teacher != null;
                    continue;
                }
            }
            if (acc == null) acc = new EntitiesInfo.Accounts();
            if (teacher == null) teacher = new EntitiesInfo.Teacher();
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {
                //Excel���е�ֵ
                string column = dr[rel.Key].ToString();
                //���ݿ��ֶε�����
                string field = rel.Value;
                if (field == "Th_Sex")
                {
                    teacher.Th_Sex = (short)(column == "��" ? 1 : 2);
                    continue;
                }
                PropertyInfo[] properties = teacher.GetType().GetProperties();
                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo pi = properties[j];
                    if (field == pi.Name && !string.IsNullOrEmpty(column))
                    {
                        pi.SetValue(teacher, Convert.ChangeType(column,pi.PropertyType), null);                        
                    }
                }               
            }
            //���÷���
            if (!string.IsNullOrWhiteSpace(teacher.Ths_Name)) teacher.Ths_ID = _getDepartId(sorts, teacher.Ths_Name);
            if (!string.IsNullOrWhiteSpace(teacher.Th_Pw)) teacher.Th_Pw = teacher.Th_Pw.Trim();
            acc.Org_ID = teacher.Org_ID = org.Org_ID;
            acc.Ac_Name = teacher.Th_Name;
            teacher.Org_Name = org.Org_Name;
            teacher.Th_AccName = teacher.Th_PhoneMobi;
            acc.Ac_IsPass = teacher.Th_IsPass = true;
            teacher.Th_IsShow = true;
            acc.Ac_IsUse = teacher.Th_IsUse = true;           
            //����˺Ų�����
            if (!isExistAcc)
            {
                acc.Ac_AccName = acc.Ac_MobiTel1 = acc.Ac_MobiTel2 = teacher.Th_PhoneMobi;  //�˺��ֻ���
                acc.Ac_Pw = new Common.Param.Method.ConvertToAnyValue(teacher.Th_Pw).MD5;    //����                
                acc.Ac_Sex = teacher.Th_Sex;        //�Ա�
                acc.Ac_Birthday = teacher.Th_Birthday;
                acc.Ac_Qq = teacher.Th_Qq;
                acc.Ac_Email = teacher.Th_Email;
                acc.Ac_IDCardNumber = teacher.Th_IDCardNumber;  //���֤    
                acc.Ac_IsTeacher = true;        //���˺��н�ʦ���
                //����
                teacher.Ac_ID = Business.Do<IAccounts>().AccountsAdd(acc);
                Business.Do<ITeacher>().TeacherSave(teacher);
            }
            else
            {
                acc.Ac_IsTeacher = true;
                teacher.Ac_ID = acc.Ac_ID;                
                //����˺Ŵ���,����ʦ������
                if (!isExistTh)
                {
                    Business.Do<ITeacher>().TeacherAdd(teacher);
                }
                else
                {
                    teacher.Th_Pw = new Common.Param.Method.ConvertToAnyValue(teacher.Th_Pw).MD5;    //����
                    Business.Do<ITeacher>().TeacherSave(teacher);
                }
            }
        }
        /// <summary>
        /// ��ȡ����id
        /// </summary>
        /// <param name="sorts"></param>
        /// <param name="departName"></param>
        /// <returns></returns>
        private int _getDepartId(EntitiesInfo.TeacherSort[] sorts, string sortName)
        {
            try
            {
                int sortId = 0;
                foreach (EntitiesInfo.TeacherSort s in sorts)
                {
                    if (sortName.Trim() == s.Ths_Name)
                    {
                        sortId = s.Ths_ID;
                        break;
                    }
                }
                if (sortId == 0 && sortName.Trim() != "")
                {
                    int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                    EntitiesInfo.TeacherSort nwsort = new EntitiesInfo.TeacherSort();
                    nwsort.Ths_Name = sortName;
                    nwsort.Ths_IsUse = true;
                    nwsort.Org_ID = orgid;
                    Business.Do<ITeacher>().SortAdd(nwsort);
                    sortId = nwsort.Ths_ID;
                    this.sorts = this.sorts = Business.Do<ITeacher>().SortCount(orgid, null, 0);
                }
                return sortId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        #endregion
      
    }
}
