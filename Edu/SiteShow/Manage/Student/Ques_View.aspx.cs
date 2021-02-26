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



namespace SiteShow.Manage.Student
{
    public partial class Ques_View : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //���ͷ��຺������
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        protected EntitiesInfo.Questions mm;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {              
                fill();
            }
            
        }

        private void fill()
        {
            if (id < 1) return;
            mm = Business.Do<IQuestions>().QuesSingle(id);
            ltTitle.Text = Extend.Html.ClearHTML(mm.Qus_Title, "pre", "p");
            //֪ʶ�����
            ltExplan.Text = mm.Qus_Explain == string.Empty ? "��" : mm.Qus_Explain;
            ltExplan.Text = Extend.Html.ClearHTML(ltExplan.Text, "pre", "p");
            if (mm.Qus_Type == 1) getAnswer1(mm);
            if (mm.Qus_Type == 2) getAnswer2(mm);
            if (mm.Qus_Type == 3) getAnswer3(mm);
            if (mm.Qus_Type == 4) getAnswer4(mm);
            if (mm.Qus_Type == 5) getAnswer5(mm);
        }
        #region ��ȡ��
        /// <summary>
        /// ��ȡ��ѡ���
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer1(EntitiesInfo.Questions qus)
        {
            string ansStr = "";
            //��ǰ����Ĵ�
            EntitiesInfo.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                if (ans[i].Ans_IsCorrect)
                    ansStr += (char)(65 + i);
            }
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;
            //
            rptItem.DataSource = ans;
            rptItem.DataBind();
        }
        /// <summary>
        /// ��ȡ��ѡ���
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer2(EntitiesInfo.Questions qus)
        {
            string ansStr = "";
            //��ǰ����Ĵ�
            EntitiesInfo.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                if (ans[i].Ans_IsCorrect)
                    ansStr += (char)(65 + i) + "��";
            }
            ansStr = ansStr.Substring(0, ansStr.LastIndexOf("��"));
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;
            rptItem.DataSource = ans;
            rptItem.DataBind();
        }
        /// <summary>
        /// ��ȡ�ж����
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer3(EntitiesInfo.Questions qus)
        {
            string ansStr = qus.Qus_IsCorrect ? "��ȷ" : "����";
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;           
            
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="qus"></param>
        private void getAnswer4(EntitiesInfo.Questions qus)
        {
            if (qus != null && !string.IsNullOrEmpty(qus.Qus_Answer))
            {
                ltAnswerText.Text = qus.Qus_Answer;
            }
            divAnswerWord.Visible = false;
            divAnswerText.Visible = true;
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="qusUid"></param>
        private void getAnswer5(EntitiesInfo.Questions qus)
        {
            string ansStr = "";
            //��ǰ����Ĵ�
            EntitiesInfo.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                ansStr += ans[i].Ans_Context + "<br/>";
            }
            ltAnswerWord.Text = ansStr;
            divAnswerWord.Visible = true;
            divAnswerText.Visible = false;
        }
        #endregion
    }
}
