using System;
using System.Collections.Generic;
using System.Text;
using ServiceInterfaces;

using EntitiesInfo;
using Common;
using System.Data.Common;
using DataBaseInfo;

namespace ServiceImpls
{
    public class TrPlanCom:ITrPlan
    {
        #region ITrPlan 成员

        private void addData(TrPlan theme, List<ExamGroup> groups)
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                theme.Org_ID = org.Org_ID;
                theme.Org_Name = org.Org_Name;
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<TrPlan>(theme);
                    if (groups != null)
                    {
                        foreach (ExamGroup g in groups)
                        {
                            tran.Save<ExamGroup>(g);
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        public void TrpAdd(TrPlan theme, List<ExamGroup> groups)
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                theme.Org_ID = org.Org_ID;
                theme.Org_Name = org.Org_Name;
            }
            if (groups == null)
                Gateway.Default.Save<TrPlan>(theme);
            else
                addData(theme, groups);
        }
        public void TrpSave(TrPlan theme, int yuanType, int newType, List<ExamGroup> groups)
        {
            //不管怎么变，先删除分组表中的数据。
            if (yuanType != 1)
                Gateway.Default.Delete<ExamGroup>(ExamGroup._.Exam_UID == theme.TrP_UID);
            if (newType == 1)
                Gateway.Default.Save<TrPlan>(theme);
            else
                addData(theme, groups);
        }

        public void TrpDelete(int identify)
        {
            TrPlan tp = this.TrpSingle(identify);
            this.TrpDelete(tp.TrP_UID);
        }

        public void TrpDelete(string uid)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<ExamGroup>(ExamGroup._.Exam_UID == uid);
                    tran.Delete<TrPlan>(TrPlan._.TrP_UID == uid);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }

        public TrPlan TrpSingle(int identify)
        {
            return Gateway.Default.From<TrPlan>().Where(TrPlan._.TrP_Id == identify).ToFirst<TrPlan>();
        }

        public TrPlan TrpSingle(string uid)
        {
            return Gateway.Default.From<TrPlan>().Where(TrPlan._.TrP_UID == uid).ToFirst<TrPlan>();
        }

        public bool TrpJudge(string uid, int depId, int teamId)
        {
            ExamGroup eg = null;
            if (depId != -1 || teamId != -1)
            {
                eg = Gateway.Default.From<ExamGroup>().Where(ExamGroup._.Exam_UID == uid).ToFirst<ExamGroup>();
            }
            return eg == null ? false : true;
        }

        public TrPlan[] TrpItem(int groupType, int depId, int teamId)
        {
            if (groupType == 1)
                return Gateway.Default.From<TrPlan>().Where(TrPlan._.TrP_GroupType == groupType).ToArray<TrPlan>();
            else
            {
                //获取所有符合条件的数据的唯一标识，
                //再根据这些标识获取所有数据，暂时不做。
                return null;
            }
        }

        public TrPlan[] TrpItem(DateTime? timestall, DateTime? timeend, int? depId, int? sbjId, int? groupType, int? result, string teacher, string content, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (timestall != null)
            {
                wc.And(TrPlan._.TrP_Time >= (DateTime)timestall);
            }
            if (timeend != null)
            {
                wc.And(TrPlan._.TrP_Time < (DateTime)timeend);
            }
            //院系
            if (depId != null && depId > 0) wc.And(TrPlan._.Dep_Id == (int)depId);
            //专科
            if (sbjId != null && sbjId > 0) wc.And(TrPlan._.Sbj_ID == (int)sbjId);
            //分类
            if (groupType != null && groupType > 0) wc.And(TrPlan._.TrP_GroupType == (int)groupType);
            //完成程度
            if (result != null && result > 0) wc.And(TrPlan._.TrP_Result == (int)result);
            //教师
            if (!string.IsNullOrEmpty(teacher) && teacher.Trim() != "")
                wc.And(TrPlan._.TrP_Teacher.Like("%" + teacher.Trim() + "%"));
            //内容
            if(!string.IsNullOrEmpty(content) && content.Trim()!="")
                wc.And(TrPlan._.TrP_Content.Like("%" + content.Trim() + "%"));
            countSum = Gateway.Default.Count<TrPlan>(wc);
            return Gateway.Default.From<TrPlan>().Where(wc).OrderBy(TrPlan._.TrP_Id.Desc).ToArray<TrPlan>(size, (index - 1) * size);
        }
        #endregion
    }
}
