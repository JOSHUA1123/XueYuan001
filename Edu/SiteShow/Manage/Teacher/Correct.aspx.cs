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
using System.Xml;
using System.IO;

namespace SiteShow.Manage.Teacher
{
    public partial class Correct : Extend.CustomPage
    {
        //���Գɼ���¼id
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        //ѧ��Id
        private int stid = Common.Request.QueryString["stid"].Int32 ?? 0;
        //��ǰ����
        EntitiesInfo.Examination exam;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bindStudent();
                fill();
            }
        }
        /// <summary>
        /// ��ǰ�������п���
        /// </summary>
        private void bindStudent()
        {
            EntitiesInfo.ExamResults[] res = Business.Do<IExamination>().ResultCount(id, -1);
            ddlStudent.DataSource = res;
            ddlStudent.DataTextField = "Ac_Name";
            ddlStudent.DataValueField = "Ac_ID";
            ddlStudent.DataBind();
            //
            foreach (ListItem li in ddlStudent.Items)
            {
                int stid = Convert.ToInt32(li.Value);
                foreach (EntitiesInfo.ExamResults ex in res)
                {
                    if (stid == ex.Ac_ID)
                    {
                        if (ex.Exr_Score < ex.Exr_ScoreFinal)
                        {
                            li.Attributes.Add("class","correct");
                        }
                        break;
                    }
                }
            }
        }
        private void fill()
        {
            //��ǰ����
            exam = Business.Do<IExamination>().ExamSingle(id);
            EntitiesInfo.ExamResults er = Business.Do<IExamination>().ResultSingle(id, 0, stid);
            if (er == null) return;
            stid = er.Ac_ID;
            //��ǰѧ��
            ListItem li = ddlStudent.Items.FindByValue(stid.ToString());
            if (li != null) li.Selected = true;
            //
            //ѧ������
            lbStudent.Text = er.Ac_Name;
            //��������
            lbExamTheme.Text = exam.Exam_Title;
            //����
            lbExamName.Text = exam.Exam_Name;
            //Ӧ��ʱ��
            lbExamTime.Text = ((DateTime)er.Exr_CrtTime).ToString("yyyy��MM��dd�� hh:mm");
            //���յ÷�
            lbScoreFinal.Text = er.Exr_ScoreFinal.ToString();
            //չʾ�����
            plJianda.Visible = bindShortQues(er);
        }
        /// <summary>
        /// �󶨼����������
        /// </summary>
        private bool bindShortQues(EntitiesInfo.ExamResults exr)
        {
            if (string.IsNullOrEmpty(exr.Exr_Results)) return false;
            DataTable dt = new DataTable("DataBase");
            //�����id����ɣ��𰸣�����
            dt.Columns.Add(new DataColumn("qid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("qtitle", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("answer", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("type", Type.GetType("System.String")));
            //����������÷֣������ش�����
            dt.Columns.Add(new DataColumn("num", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("score", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("reply", Type.GetType("System.String")));
            //
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(exr.Exr_Results, false);
            XmlNodeList nodeList = resXml.SelectSingleNode("results").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                //���������
                int type = Convert.ToInt32(nodeList[i].Attributes["type"].Value);
                //����ǲ��Ǽ���⣬����
                //if (type != 4) continue;
                if (type==4 || type==5)
                {
                XmlNodeList nodes = nodeList[i].ChildNodes;
                for (int j = 0; j < nodes.Count; j++)
                {
                    //�����Id
                    int id = Convert.ToInt32(nodes[j].Attributes["id"].Value);
                    //����ķ���
                    double num = Convert.ToDouble(nodes[j].Attributes["num"].Value);
                    //����÷�
                    double score = 0;
                    if (nodes[j].Attributes["score"] != null)
                        score = Convert.ToDouble(nodes[j].Attributes["score"].Value);
                    //�ش�
                    string reply = nodes[j].InnerText;
                    //����Datatable
                    DataRow dr = dt.NewRow();
                    dr["qid"] = id.ToString();
                    EntitiesInfo.Questions qus = Business.Do<IQuestions>().QuesSingle(id);
                    if (qus != null)
                    {
                        dr["qtitle"] = qus.Qus_Title;
                        dr["answer"] = qus.Qus_Answer;
                    }
                    dr["type"] = type.ToString();
                    dr["num"] = num.ToString();
                    dr["score"] = score.ToString();
                    dr["reply"] = reply;
                    dt.Rows.Add(dr);
                }
                }
            }
            rptQues.DataSource = dt;
            rptQues.DataBind();
            return dt.Rows.Count > 0;
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //�жϷ�ֵ�Ƿ񳬳�����ֵ
            for (int i = 0; i < rptQues.Items.Count; i++)
            {
                //��ǰ����ķ���
                HiddenField hfnumField = (HiddenField)rptQues.Items[i].FindControl("hfnum");
                double hfnum = 0;
                double.TryParse(hfnumField.Value, out hfnum);
                //ʵ�ʵ÷�
                TextBox tbNumber = (TextBox)rptQues.Items[i].FindControl("tbNumber");
                double number = 0;
                double.TryParse(tbNumber.Text, out number);
                if (string.IsNullOrEmpty(tbNumber.Text))
                {
                    Master.Alert("��" + (i + 1) + "��÷ֲ���Ϊ�գ�");
                    return;
                }
                if (number > hfnum)
                {
                    Master.Alert("��" + (i + 1) + "��÷ֳ���������߷�ֵ��");
                    return;
                }
                if (number < 0)
                {
                    Master.Alert("��" + (i + 1) + "��÷ֲ���Ϊ��ֵ��");
                    return;
                }
            }
            EntitiesInfo.ExamResults mm = Business.Do<IExamination>().ResultSingle(id, 0, stid);
            //�����÷�
            double sQusNum = calcShort(mm);
            double sQusNumT = calcShortT(mm);
            mm.Exr_ScoreFinal = (float)mm.Exr_Score + (float)sQusNum+(float)sQusNumT;
            //���յ÷�
            try
            {
                Business.Do<IExamination>().ResultSave(mm);
                EntitiesInfo.ExamResults next = Business.Do<IExamination>().ResultSingleNext(id, stid, false);
                if (next != null)
                {
                    this.Response.Redirect(string.Format("Correct.aspx?id={0}&stid={1}", next.Exam_ID.ToString(), next.Ac_ID.ToString()));
                }
                else
                {
                    fill();
                    Master.Alert("�����ɹ���");
                }
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        /// <summary>
        /// ��������÷�
        /// </summary>
        /// <returns></returns>
        private double calcShort(EntitiesInfo.ExamResults exr)
        {
            if (string.IsNullOrEmpty(exr.Exr_Results)) return 0;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(exr.Exr_Results, false);
            XmlNodeList nodeList = resXml.SelectSingleNode("results").ChildNodes;
            XmlNodeList nodes = null;
            for (int i = 0; i < nodeList.Count; i++)
            {
                //���������
                int type = Convert.ToInt32(nodeList[i].Attributes["type"].Value);
                //����ǲ��Ǽ���⣬����
                if (type != 4) continue;
                nodes = nodeList[i].ChildNodes;
                //if (type == 4 || type==5)
                //{
                //    nodes = nodeList[i].ChildNodes;
                //}
            }
            if (nodes == null) return 0;
            //�÷ּ�¼
            double scoreSum = 0;
            foreach (RepeaterItem pi in rptQues.Items)
            {
                //id
                Label lbId = (Label)pi.FindControl("lbID");
                //�÷�
                TextBox tb = (TextBox)pi.FindControl("tbNumber");
                HiddenField types = (HiddenField)pi.FindControl("types");
                double score = tb.Text.Trim() == "" ? 0 : Convert.ToDouble(tb.Text);
                int typeNum = types.Value.Trim()==""?0:Convert.ToInt32(types.Value);
                if (typeNum != 4) continue;
                scoreSum += score;
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];
                    if (node.Attributes["id"].Value == lbId.Text)
                    {
                        XmlElement el = (XmlElement)node;
                        el.SetAttribute("score", score.ToString());
                        //����ķ���
                        double num = Convert.ToDouble(node.Attributes["num"].Value);
                        if (score >= num) el.SetAttribute("sucess", "true");
                    }
                }
            }
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            resXml.WriteTo(xw);
            exr.Exr_Results = sw.ToString();
            return scoreSum;
        }
        /// <summary>
        /// ���������÷�
        /// </summary>
        /// <returns></returns>
        private double calcShortT(EntitiesInfo.ExamResults exr)
        {
            if (string.IsNullOrEmpty(exr.Exr_Results)) return 0;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(exr.Exr_Results, false);
            XmlNodeList nodeList = resXml.SelectSingleNode("results").ChildNodes;
            XmlNodeList nodes = null;
            for (int i = 0; i < nodeList.Count; i++)
            {
                //���������
                int type = Convert.ToInt32(nodeList[i].Attributes["type"].Value);
                //����ǲ�������⣬����
                if (type != 5) continue;
                nodes = nodeList[i].ChildNodes;
                //if (type == 4 || type==5)
                //{
                //    nodes = nodeList[i].ChildNodes;
                //}
            }
            if (nodes == null) return 0;
            //�÷ּ�¼
            double scoreSum = 0;
            foreach (RepeaterItem pi in rptQues.Items)
            {
                //id
                Label lbId = (Label)pi.FindControl("lbID");

                //�÷�
                TextBox tb = (TextBox)pi.FindControl("tbNumber");
                HiddenField types = (HiddenField)pi.FindControl("types");

                double score = tb.Text.Trim() == "" ? 0 : Convert.ToDouble(tb.Text);
                int typeNum = types.Value.Trim() == "" ? 0 : Convert.ToInt32(types.Value);
                if (typeNum != 5) continue;
                scoreSum += score;
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];
                    if (node.Attributes["id"].Value == lbId.Text)
                    {
                        XmlElement el = (XmlElement)node;
                        el.SetAttribute("score", score.ToString());
                        //����ķ���
                        double num = Convert.ToDouble(node.Attributes["num"].Value);
                        if (score >= num) el.SetAttribute("sucess", "true");
                    }
                }
            }
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            resXml.WriteTo(xw);
            exr.Exr_Results = sw.ToString();
            return scoreSum;
        }
        protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
        {            
            this.Response.Redirect(string.Format("Correct.aspx?id={0}&stid={1}", id.ToString(), ddlStudent.SelectedValue));
        }
        /// <summary>
        /// �������ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptQues_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //��ȡ��ǰ����id�븽���ؼ�
            Label lbID = (Label)e.Item.FindControl("lbID");
            HyperLink link=(HyperLink)e.Item.FindControl("linkAcc");
            //��ȡ����
            string path = Upload.Get["Exam"].Physics + exam.Exam_ID + "/" + stid + "/";
            string file = "";
            if (System.IO.Directory.Exists(path))
            {                
                foreach (FileInfo f in new DirectoryInfo(path).GetFiles())
                {
                    if (f.Name.IndexOf("_") > 0)
                    {
                        string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                        if (idtm == lbID.Text.ToString())
                        {
                            file = f.Name;
                            break;
                        }
                    }
                }
            }
            if (file.Trim() != "")
            {
                link.Text = file;
                link.NavigateUrl = Upload.Get["Exam"].Virtual + exam.Exam_ID + "/" + stid + "/" + file;
            }

        }
    }
}
