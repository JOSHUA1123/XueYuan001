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
using System.IO;
using System.Text.RegularExpressions;

namespace SiteShow.Manage.Utility
{
    public partial class Pager2 : System.Web.UI.UserControl
    {
        #region ����

        /// <summary>
        /// ÿҳ��ʾ������
        /// </summary>
        public int Size
        {
            get
            {
                object obj = ViewState["Size"];
                return (obj == null) ? 0 : (int)obj;
            }
            set
            {
                ViewState["Size"] = value;
                //���ý���Ч��
                faceSetup();
            }
        }
        /// <summary>
        /// ��ʾ������ҳ��
        /// </summary>
        public int Display 
        {
            get
            {
                object obj = ViewState["Display"];
                return (obj == null) ? 3 : (int)obj;
            }
            set
            {
                ViewState["Display"] = value;
            }
        }
        /// <summary>
        /// ��ǰҳ����
        /// </summary>
        public int Index
        {
            get
            {
                int pindex = Common.Request.QueryString["index"].Int32 ?? 1;
                if (pindex >= PageAmount && PageAmount > 0) return PageAmount;
                if (pindex < 1) return 1;
                return pindex;
            }
            set
            {
                string query = this.Qurey;
                string fileName = Path.GetFileName(this.Request.FilePath);
                if (string.IsNullOrWhiteSpace(query))
                {
                    fileName += "?index=" + 1;
                }
                else
                {
                    fileName += "?" + getQuery("index", 1);
                }
                Response.Redirect(fileName);
            }
        }
        private int _recordAmount;
        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        public int RecordAmount
        {
            get
            {
                //return Common.Request.QueryString["Recordcount"].Int32 ?? 1;
                return _recordAmount;
            }
            set
            {
                _recordAmount = value;
            }
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int PageAmount
        {
            get { return (int)Math.Ceiling((double)RecordAmount / (double)Size); }
        }

        private string _query;
        /// <summary>
        /// ��ѯ��Get��ʽ���ַ���
        /// </summary>
        public string Qurey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_query))
                    return this.Page.ClientQueryString;
                else
                    return _query;
            }
            set
            {
                _query = value;
            }
        }
        #endregion

        #region ����
        public string First
        {
            get
            {
                int index = this.Index;
                int display = this.Display;
                string url = "";
                if (index - 1 <= display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                url = fileName + "?" + getQuery("index", 1);
                url = "<a href=\"" + url + "\">|&lt;</a>";
                return url;
            }
        }
        public string Prev
        {
            get
            {
                int index = this.Index;
                int display = this.Display;
                //
                string url = "";
                if (index - 1 <= display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                if (index - 1 <= 0) url = fileName + "?" + getQuery("index", index); 
                if (index - 1 > 0) url = fileName + "?" + getQuery("index", index - 1 - display * 2);                
                url = "<a href=\"" + url + "\">...</a>";
                return url;
            }
        }
        public string Next
        {
            get
            {
                int index = this.Index;
                int amount = this.PageAmount;
                int display = this.Display;
                //
                string url = "";
                if (index >= amount - display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                if (index + 1 < amount - display * 2)
                    url = fileName + "?" + getQuery("index", index + 1 + this.Display * 2);
                else
                    url = fileName + "?" + getQuery("index", amount);
                return url = "<a href=\"" + url + "\">...</a>";
            }
        }
        public string Last
        {
            get
            {
                int index = this.Index;
                int amount = this.PageAmount;
                int display = this.Display;
                string url = "";
                if (index >= amount || index >= amount - display) return url;
                string fileName = Path.GetFileName(this.Request.FilePath);
                url = fileName + "?" + getQuery("index", amount);
                return url = "<a href=\"" + url + "\">&gt;|</a>";
            }
        }
        /// <summary>
        /// ����ƴ��ҳ��Ĳ�ѯ�ִ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string getQuery(string key, int value)
        {
            string query = this.Qurey;
            if (string.IsNullOrWhiteSpace(query)) return key + "=" + value;
            //
            Regex regex = new Regex(@"(?<key>" + key + @")=(?<value>(|.)[^&]*)", RegexOptions.IgnoreCase);
            if (regex.Match(query).Success)
                query = regex.Replace(query, "$2=" + value);
            else
                query += "&" + key + "=" + value;
            return query;
        }
        /// <summary>
        /// �������ֵ���
        /// </summary>
        /// <param name="index">��ǰ������</param>
        /// <param name="tag">html��ʶ</param>
        /// <returns></returns>
        public string NumberNav(int index, string tag)
        {
            string nav = "";
            string fileName = Path.GetFileName(this.Request.FilePath);
            int amount = this.PageAmount;
            int display = this.Display;
            //�������
            for (int i = index - display; i <= index + display; i++)
            {
                if (i > 0 && i <= amount)
                {

                    if (i == index)
                    {
                        nav += "<" + tag + " class=\"curr\">" + i + "</" + tag + ">";
                    }
                    else
                    {
                        nav += "<a href=\"" + fileName + "?" + getQuery("index", i) + "\">" + i + "</a>";
                    }
                   
                }
            }
            return nav;
        }
        #endregion       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                faceSetup();
                tbGoPagenum.Attributes.Add("numlimit", "1-" + this.PageAmount);
            }
        }
        /// <summary>
        /// ���ý���Ч��
        /// </summary>
        private void faceSetup()
        {
            string name = Common.Request.Page.Name;
        }
        /// <summary>
        /// ��ת
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoPagenum_Click(object sender, EventArgs e)
        {
            int goIndex = 1;
            int.TryParse(tbGoPagenum.Text, out goIndex);
            string fileName = Path.GetFileName(this.Request.FilePath);
            string url = fileName + "?" + getQuery("index", goIndex);
            this.Response.Redirect(url);
        }
  
    }
}