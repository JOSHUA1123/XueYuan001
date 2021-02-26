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
using System.Text.RegularExpressions;
using System.IO;

namespace SiteShow.Manage.Panel
{
    public partial class HelpEditer : Extend.CustomPage
    {
        //�����ļ�
        private string helpfile = Common.Request.QueryString["helpfile"].String;
        //����ģ�������
        private string name = Common.Request.QueryString["name"].String;
        //����ģ��
        private string template = "/manage/help/HeplTemplate.html";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbHelpContext.Text = getBodyContext();
                lbName.Text = name;
                linkHelp.NavigateUrl = helpfile;
            }
        }
        /// <summary>
        /// ��ȡ�����ļ�������
        /// </summary>
        /// <returns></returns>
        private string getBodyContext()
        {
            if (helpfile == "") return "";
            string context = this.getOldHtml();
            string regTxt = @"(?<=<body>).*(?=</body>)";
            Regex re = new Regex(regTxt, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            MatchCollection mc = re.Matches(context);
            if (mc.Count > 0)
                 return mc[0].Value;
             return "";
        }
        /// <summary>
        /// ��ȡԭ�а����ļ�������HTML���ݣ���������ڣ�����ģ�崴��
        /// </summary>
        /// <returns></returns>
        private string getOldHtml()
        {
            if (helpfile == "") return "";
            string context = "";
            string helpfileHy = this.Server.MapPath(helpfile);
            if (!helpfileHy.EndsWith(".html"))
            {
                return "";
            }
            if (File.Exists(helpfileHy))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(helpfileHy))
                {
                    context = sr.ReadToEnd();
                    sr.Close();
                }
            }
            else
            {
                //��������ļ������ڣ���ȡ����ģ��ҳ
                using (System.IO.StreamReader sr = new System.IO.StreamReader(Server.MapPath(template)))
                {
                    context = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return context;
        }
        /// <summary>
        /// ���ɰ����ĵ�������
        /// </summary>
        /// <returns></returns>
        private string setHelpContext()
        {
            //�滻body������
            string regTxt = @"(?<=<body>).*(?=</body>)";
            Regex re = new Regex(regTxt, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            string context = re.Replace(this.getOldHtml(), this.tbHelpContext.Text);
            //�滻����
            context = new Regex("(?<=<title>).*(?=</title>)").Replace(context, name);
            return context;
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                string tm = this.setHelpContext();
                //д��
                string helpfileHy = this.Server.MapPath(helpfile);
                //if (!File.Exists(helpfileHy))
                //{
                //    File.Create(helpfileHy);
                //}
                using (System.IO.StreamWriter sr = new System.IO.StreamWriter(helpfileHy, false))
                {
                    sr.Write(tm);
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 

        }
    }
}
