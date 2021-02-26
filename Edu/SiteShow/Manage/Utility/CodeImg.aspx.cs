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
using System.Drawing;
using System.Drawing.Imaging;

namespace SiteShow.Manage.Utility
{
    public partial class CodeImg : System.Web.UI.Page
    {
        //����ַ����ĳ���
        int? _len = Common.Request.QueryString["len"].Int16 ?? 4;
        //�ַ����ͣ�0Ϊ�������Сд��ĸ��1Ϊ�����֣�2Ϊ��С��ĸ��3Ϊ����д��ĸ��4Ϊ��Сд��ĸ��5���ּ�Сд��6���ּӴ�д
        int? _type = Common.Request.QueryString["type"].Int16 ?? 1;
        //cookies������
        string _name = Common.Request.QueryString["name"].String ?? "default";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Src"></param>
        /// <param name="E"></param>
        protected void Page_Load(Object Src, EventArgs E)
        {         
            //�趨���ɼ�λ�����
            string tmp = RndNum((int)_len, (int)_type);
            //�洢�����
            HttpCookie cookie = new HttpCookie(_name);
            //������ַ�ת�ãͣģ�����
            cookie.Value = Md5(tmp);
            Response.Cookies.Add(cookie);
            //����У�����ͼƬ
            ValidateCode(this, tmp);           
        }
        /// <summary>
        /// ����Md5�����룬Md5Ϊ���������
        /// </summary>
        /// <param name="str">��Ҫ���ܵ��ַ���</param>
        /// <param name="code">Md5���ܷ�16λ��32λ���˴�Ӧ��16��32</param>
        /// <returns>����ֵΪ���ܺ��16λ32λ�ַ�</returns>
        public static string Md5(string str, int code)
        {
            if (code == 16) //16λMD5���ܣ�ȡ32λ���ܵ�9~25�ַ��� 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            else//32λ���� 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
        }

        /// <summary>
        /// ����Md5�����룬Md5Ϊ���������
        /// </summary>
        /// <param name="str">��Ҫ���ܵ��ַ���</param>
        /// <returns>����ֵΪ���ܺ��32λ�ַ�</returns>
        
        public static string Md5(string str)
        {
            //16λMD5���ܣ�ȡ32λ���ܵ�9~25�ַ��� 
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }
        /// <summary>
        /// ��������ַ���������ѡ�񳤶�������
        /// </summary>
        /// <param name="VcodeNum">����ַ����ĳ���</param>
        /// <param name="type">���ɵ���������ͣ�0Ϊ�������Сд��ĸ��1Ϊ�����֣�2Ϊ��С��ĸ��3Ϊ����д��ĸ��4Ϊ��Сд��ĸ��5���ּ�Сд��6���ּӴ�д��</param>
        /// <returns></returns>
        public static string RndNum(int VcodeNum, int type)
        {
            string Vchar;
            //���ִ�
            string num = "0,1,2,3,4,5,6,7,8,9,";
            //Сд��ĸ
            string lower = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            //��д��ĸ
            string upper = lower.ToUpper();
            switch (type)
            {
                case 0:
                    Vchar = num + lower + upper;
                    break;
                case 1:
                    Vchar = num;
                    break;
                case 2:
                    Vchar = lower;
                    break;
                case 3:
                    Vchar = upper;
                    break;
                case 4:
                    Vchar = upper + lower;
                    break;
                case 5:
                    Vchar = num + lower;
                    break;
                case 6:
                    Vchar = num + upper;
                    break;
                default:
                    Vchar = num + lower + upper;
                    break;
            }
            Vchar = Vchar.Substring(0, Vchar.Length - 1);
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                VNum += VcArray[rand.Next(VcArray.Length)];
            }
            return VNum;
        }
        /// <summary>
        /// ������֤���ͼƬ�����ַ�����䵽ͼƬ���������������
        /// </summary>
        /// <param name="pg">ҳ�棬����this</param>
        /// <param name="checkCode">��Ҫ����ͼƬ���ַ���</param>
        public static void ValidateCode(Page pg, string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 13);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 18);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //������ɫ
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "����" };
            Random rand = new Random();
            //���������

            for (int i = 0; i < 20; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }

            //�����ͬ�������ɫ����֤���ַ�
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(c.Length);
                int findex = rand.Next(font.Length);

                Font f = new System.Drawing.Font(font[findex], 12, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                //�ַ�����λ�ò�ͬ
                int ii = 0;
                if ((i + 1) % 2 == 0)
                {
                    ii = 1;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 1 + (i * 13), ii);
            }
            //��һ���߿�
            //g.DrawRectangle(new Pen(Color.Black,0),0,0,image.Width-1,image.Height-1);

            //����������
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            pg.Response.ClearContent();
            pg.Response.ContentType = "image/Gif";
            pg.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }
    }
}
