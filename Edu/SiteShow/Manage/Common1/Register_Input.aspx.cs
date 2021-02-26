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
using System.Collections.Generic;

namespace SiteShow.Manage
{
    public partial class Register_Input : Extend.CustomPage
    {
  
        //����Ժϵ
        List<EntitiesInfo.AddressSort> sort = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        protected void ExcelInput1_OnInput(object sender, EventArgs e)
        {
            ////�������е�����
            //DataTable dt = ExcelInput1.SheetDataTable;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    try
            //    {
            //        //throw new Exception();
            //        //���������е������ݿ�
            //        _inputData(dt.Rows[i]);
            //    }
            //    catch
            //    {
            //        //��������������з��ظ��ؼ�
            //        ExcelInput1.AddError(dt.Rows[i]);
            //    }
            //}
        }

        #region ��������
       
        /// <summary>
        /// ��ĳһ�����ݼ��뵽���ݿ�
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        private void _inputData(DataRow dr)
        {
            
                ////ȡ���з���
                //this.sort = Business.Do<IAddressList>().SortAll(null);
                //EntitiesInfo.AddressList addr = new AddressList();
                //foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
                //{
                //    //Excel���е�ֵ
                //    string column = dr[rel.Key].ToString();
                //    //���ݿ��ֶε�����
                //    string field = rel.Value;
                //    if (field == "Adl_Name") addr.Adl_Name = column;
                //    if (field == "Adl_Sex") addr.Adl_Sex = (column == "��" ? 1 : 2);
                //    if (field == "Adl_Tel") addr.Adl_Tel = column;
                //    if (field == "Adl_CoTel") addr.Adl_CoTel = column;
                //    if (field == "Adl_MobileTel") addr.Adl_MobileTel = column;
                //    if (field == "Adl_Company") addr.Adl_Company = column;
                //    if (field == "Ads_Name") addr.Ads_Name = column;
                //    if (field == "Adl_Address") addr.Adl_Address = column;
                //    if (field == "Adl_Zip") addr.Adl_Zip = column;
                //    if (field == "Adl_Email") addr.Adl_Email = column;
                //    if (field == "Adl_QQ") addr.Adl_QQ = column;
                //}
                //addr.Ads_Id = _getSortId(sort, addr.Ads_Name);
                //Business.Do<IAddressList>().AddressAdd(addr);
                //EntitiesInfo.AddressList ent = Business.Do<IAddressList>().AddressSingle(addr.Adl_MobileTel);
                //if (ent == null)
                //{
                //    Business.Do<IAddressList>().AddressSave(addr);
                //}
                //else
                //{
                //    Business.Do<IAddressList>().AddressSave(addr);
                //}
            
        }
        /// <summary>
        /// ��ȡ����id
        /// </summary>
        /// <param name="depart"></param>
        /// <param name="departName"></param>
        /// <returns></returns>
        private int _getSortId(List<EntitiesInfo.AddressSort> sortArr, string sortName)
        {
            int sortId = 0;
            try
            {
                foreach (EntitiesInfo.AddressSort s in sortArr)
                {
                    if (sortName.Trim() == s.Ads_Name)
                    {
                        sortId = s.Ads_Id;
                        break;
                    }
                }
                if (sortId == 0)
                {
                    EntitiesInfo.AddressSort nwsort = new EntitiesInfo.AddressSort();
                    nwsort.Ads_Name = sortName;
                    nwsort.Acc_Id = Extend.LoginState.Admin.CurrentUserId;
                    nwsort.Ads_IsUse = true;
                    sortId = Business.Do<IAddressList>().SortAdd(nwsort);
                    this.sort = Business.Do<IAddressList>().SortAll(null);
                }
                return sortId;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return sortId;
            }
        }
        #endregion
       
    }
}
