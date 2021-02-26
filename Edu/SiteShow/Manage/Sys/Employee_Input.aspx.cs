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

namespace SiteShow.Manage.Sys
{
    public partial class Employee_Input : Extend.CustomPage
    {
        
        //����Ժϵ
        EntitiesInfo.Depart[] departs = null;
        //���а���
        EntitiesInfo.Team[] teams = null;
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
            EntitiesInfo.Organization org;
            if (Extend.LoginState.Admin.IsSuperAdmin)
                org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
            else
                org = Business.Do<IOrganization>().OrganCurrent();
            if (this.departs == null)
                this.departs = Business.Do<IDepart>().GetAll(org.Org_ID,null,null);
            //���а���
            if (this.teams == null) teams = Business.Do<ITeam>().GetTeam(null, 0);
            EntitiesInfo.EmpAccount obj = new EmpAccount();
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {

                //Excel���е�ֵ
                string column = dr[rel.Key].ToString();
                //���ݿ��ֶε�����
                string field = rel.Value;
                if (field == "Acc_Sex")
                {
                    obj.Acc_Sex = (short)(column == "��" ? 1 : 2);
                    continue;
                }
                Type info = obj.GetType();
                PropertyInfo[] properties = info.GetProperties();
                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo pi = properties[j];
                    if (field == pi.Name && !string.IsNullOrEmpty(column))
                    {
                        if (pi.PropertyType.Name == "DateTime") pi.SetValue(obj, Convert.ToDateTime(column), null);
                        if (pi.PropertyType.Name == "Int16") pi.SetValue(obj, Convert.ToInt16(column), null);
                        if (pi.PropertyType.Name == "Int32") pi.SetValue(obj, Convert.ToInt32(column), null);
                        if (pi.PropertyType.Name == "Int64") pi.SetValue(obj, Convert.ToInt64(column), null);
                        if (pi.PropertyType.Name == "Boolean") pi.SetValue(obj, Convert.ToBoolean(column), null);
                        if (pi.PropertyType.Name == "String") pi.SetValue(obj, column, null);
                    }
                }

                obj.Acc_AccName = obj.Acc_EmpCode;
                obj.Acc_Pw = App.Get["DefaultPw"].String;
                //����Ժϵ
                obj.Dep_Id = _getDepartId(departs, obj.Dep_CnName);
                obj.Team_ID = _getTeamId(teams, obj.Team_Name);
                obj.Org_ID = org.Org_ID;
                obj.Org_Name = org.Org_Name;
                Business.Do<IEmployee>().Add(obj);
            }
        }
        /// <summary>
        /// ��ȡԺϵid
        /// </summary>
        /// <param name="depart"></param>
        /// <param name="departName"></param>
        /// <returns></returns>
        private int _getDepartId(EntitiesInfo.Depart[] depart, string sortName)
        {
            try
            {
                int sortId = 0;
                foreach (EntitiesInfo.Depart s in depart)
                {
                    if (sortName.Trim() == s.Dep_CnName)
                    {
                        sortId = s.Dep_Id;
                        break;
                    }
                }
                if (sortId == 0 && sortName.Trim() != "")
                {
                    EntitiesInfo.Depart nwsort = new EntitiesInfo.Depart();
                    nwsort.Dep_CnName = sortName;
                    nwsort.Dep_IsUse = true;
                    nwsort.Dep_IsShow = true;
                    sortId = Business.Do<IDepart>().Add(nwsort);
                    int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                    this.departs = Business.Do<IDepart>().GetAll(orgid,null,null);
                }
                return sortId;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return 0;
            }
        }
        /// <summary>
        /// ��ȡ����Id
        /// </summary>
        /// <param name="depart"></param>
        /// <param name="sortName"></param>
        /// <returns></returns>
        private int _getTeamId(EntitiesInfo.Team[] team, string sortName)
        {
            try
            {
                int sortId = 0;
                foreach (EntitiesInfo.Team s in team)
                {
                    if (sortName.Trim() == s.Team_Name)
                    {
                        sortId = s.Team_ID;
                        break;
                    }
                }
                if (sortId == 0 && sortName.Trim() != "")
                {
                    EntitiesInfo.Team nwsort = new EntitiesInfo.Team();
                    nwsort.Team_Name = sortName;
                    nwsort.Team_IsUse = true;
                    sortId = Business.Do<ITeam>().TeamAdd(nwsort);
                    this.teams = Business.Do<ITeam>().GetTeam(null, 0);
                }
                return sortId;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return 0;
            }
        }
        #endregion
      
    }
}
