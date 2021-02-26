using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Common;
using EntitiesInfo;


using ServiceInterfaces;
using System.Data.Common;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using System.Xml;
using NPOI.SS.UserModel;
using System.IO;
using DataBaseInfo;

namespace ServiceImpls
{
    public class SSOCom : ISSO
    {

        public void Add(SingleSignOn entity)
        {
            entity.SSO_CrtTime = DateTime.Now;
            Gateway.Default.Save<SingleSignOn>(entity); 
        }

        public void Save(SingleSignOn entity)
        {
            Gateway.Default.Save<SingleSignOn>(entity); 
        }

        public void Delete(int identify)
        {
            Gateway.Default.Delete<SingleSignOn>(SingleSignOn._.SSO_ID == identify);
        }

        public SingleSignOn GetSingle(int identify)
        {
            return Gateway.Default.From<SingleSignOn>().Where(SingleSignOn._.SSO_ID == identify).ToFirst<SingleSignOn>();
        }

        public SingleSignOn GetSingle(string appid)
        {
            return Gateway.Default.From<SingleSignOn>().Where(SingleSignOn._.SSO_APPID == appid).ToFirst<SingleSignOn>();
        }
        public SingleSignOn[] GetAll(bool? isuse)
        {
            WhereClip wc = new WhereClip();
            if (isuse != null) wc &= SingleSignOn._.SSO_IsUse == (bool)isuse;
            return Gateway.Default.From<SingleSignOn>().Where(wc).ToArray<SingleSignOn>();
        }

        public SingleSignOn[] GetAll(bool? isuse,string type)
        {
            WhereClip wc = new WhereClip();
            if (isuse != null) wc &= SingleSignOn._.SSO_IsUse == (bool)isuse;
            if (string.IsNullOrWhiteSpace(type)) wc &= SingleSignOn._.SSO_Direction == type;
            return Gateway.Default.From<SingleSignOn>().Where(wc).ToArray<SingleSignOn>();
        }
    }
}
