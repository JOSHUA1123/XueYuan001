﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
namespace SiteShow
{
    /// <summary>
    /// 考试列表页
    /// </summary>
    public class Exam : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            #region 我的考试
            if (Extend.LoginState.Accounts.IsLogin)
            {
                EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                ////今天的考试
                //DateTime start = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //DateTime end = start.AddDays(1);
                //List<EntitiesInfo.Examination> today = Business.Do<IExamination>().GetSelfExam(st.Ac_ID, start, end);
                //近期要开展的考试(从当前时间之前的一个小时开始）                
                DateTime start = DateTime.Now.AddHours(1);
                List<EntitiesInfo.Examination> todaylate = Business.Do<IExamination>().GetSelfExam(st.Ac_ID, start, null);
                this.Document.SetValue("todayExam", todaylate);
            }
            #endregion

            #region 所有考试
            Tag examTag = this.Document.GetChildTagById("exams");
            if (examTag != null)
            {
                int size = int.Parse(examTag.Attributes.GetValue("size", "12"));
                int index = Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                EntitiesInfo.Examination[] exams = Business.Do<IExamination>().GetPager(this.Organ.Org_ID, null, null, true, "", size, index, out sum);
                foreach (EntitiesInfo.Examination exam in exams)
                {
                    exam.Exam_Intro = Common.HTML.ClearTag(exam.Exam_Intro);
                }
                this.Document.SetValue("Exams", exams);
                this.Document.RegisterGlobalFunction(this.getGroupType);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)size);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
                this.Document.SetValue("pageSize", size);
                //场次
                this.Document.RegisterGlobalFunction(this.setExamItem);               
            }
            #endregion
            this.Document.RegisterGlobalFunction(this.getTestPaper);    //试卷信息
            this.Document.RegisterGlobalFunction(this.getSubjectPath);    //专业的上级专业等
        }
        /// <summary>
        /// 获取参考人员类型
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected string getGroupType(object[] para)
        {
            int type = 0;
            string uid = "";
            if (para.Length > 0 && para[0] is int)
                type = Convert.ToInt32(para[0]);
            if (para.Length > 0 && para[1] is string)
                uid = para[1].ToString();
            if (type == 1) return "全体学员";
            if (type == 2)
            {
                EntitiesInfo.StudentSort[] sts = Business.Do<IExamination>().GroupForStudentSort(uid);
                string strDep = "";
                for (int i = 0; i < sts.Length; i++)
                {
                    strDep += sts[i].Sts_Name;
                    if (i < sts.Length - 1) strDep += ",";
                }
                return strDep;
            }
            return "";
        }
        /// <summary>
        /// 绑定考试场次的列表
        /// </summary>
        private EntitiesInfo.Examination[] setExamItem(object[] para)
        {
            string uid = "";
            if (para.Length > 0 && para[0] is string)
                uid = para[0].ToString();
            EntitiesInfo.Examination[] ans = Business.Do<IExamination>().ExamItem(uid);            
            for (int i = 0; i < ans.Length; i++)
            {
                DateTime examDate = ans[i].Exam_Date < DateTime.Now.AddYears(-100) ? DateTime.Now : (DateTime)ans[i].Exam_Date;
                ans[i].Exam_Date = examDate.AddYears(100) < DateTime.Now ? DateTime.Now : examDate;
            }
            return ans;
        }
        /// <summary>
        /// 获取考试的试卷信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected EntitiesInfo.TestPaper getTestPaper(object[] para)
        {
            int tpid = 0;
            if (para.Length > 0 && para[0] is int)
            {
                int.TryParse(para[0].ToString(), out tpid);
            }
            EntitiesInfo.TestPaper tp = Business.Do<ITestPaper>().PagerSingle(tpid);
            return tp;
        }
        /// <summary>
        /// 获取当前专业的上级专业
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected string getSubjectPath(object[] para)
        {
            int sbjid = 0;
            if (para.Length > 0 && para[0] is int)
            {
                int.TryParse(para[0].ToString(), out sbjid);
            }
            string sbjstr = "";
            EntitiesInfo.Subject s = Business.Do<ISubject>().SubjectSingle(sbjid);
            if (s != null)
            {
                sbjstr += s.Sbj_Name;
                while (s.Sbj_PID != 0)
                {
                    s = Business.Do<ISubject>().SubjectSingle(s.Sbj_PID);
                    if (s == null) break;
                    sbjstr = s.Sbj_Name + " &gt;&gt; " + sbjstr;
                }
            }
            return sbjstr;
        }
    }
}