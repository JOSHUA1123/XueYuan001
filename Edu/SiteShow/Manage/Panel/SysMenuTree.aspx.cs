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
using WeiSha.WebControl;

namespace SiteShow.Manage.Panel
{
    /// <summary>
    /// ϵͳ�������������˵�
    /// </summary>
    public partial class SysMenuTree : System.Web.UI.Page
    {
        //�˵�������Դ
        EntitiesInfo.ManageMenu[] _allMM;
        //ϵͳ�˵����Z��
        private int _sysZIndex = 4000;
        protected void Page_Load(object sender, EventArgs e)
        {
            //��ȡ������
            _allMM = Business.Do<IManageMenu>().GetTree("sys", true, true);
            if (_allMM != null)
            {
                Response.Write(_BuildMenu(_allMM));
            }
            Response.End();
        }
        /// <summary>
        /// ����ϵͳ�˵�
        /// </summary>
        /// <returns></returns>
        private string _BuildMenu(EntitiesInfo.ManageMenu[] mm)
        {           
            string tmp = "";
            //��ǰ���ڵ�
            Extend.MenuNode root = new Extend.MenuNode(null, mm);
            if (root.IsChilds)
            {
                //�ݹ������Ӳ˵�
                tmp += this._BuildMenuItem(root.Childs[0], 0, root.Childs[0].MM_Name);
            }
            return tmp;
        }
        //���������˵����Ӳ˵����
        private string _BuildMenuItem(EntitiesInfo.ManageMenu m, int level, string path)
        {
            Extend.MenuNode node = new Extend.MenuNode(m, _allMM);
            //���û���ӽڵ㣬��ֱ�ӷ���
            if (!node.IsChilds) return "";
            //
            string tmp = "";
            if (level == 0)
                tmp = "<div style=\"z-index: " + (_sysZIndex++) + ";\"";
            else
                tmp = "<div style=\"position: absolute; z-index: " + (_sysZIndex++) + ";\"";
            tmp += " patId='" + node.Item.MM_Id + "' ";
            tmp += " class='menuPanel' type='menuPanel' level=\"" + (level++) + "\">";
            for (int i = 0; i < node.Childs.Length; i++)
            {
                //���ɲ˵���
                EntitiesInfo.ManageMenu n = node.Childs[i];
                tmp += this._SysBuildNode(n, "MenuNode", path + "," + n.MM_Name);
                //tmp+="<div>"+n.Name+"</div>";
            }
            tmp += "</div>";
            //�ݹ������Ӳ˵�
            for (int i = 0; i < node.Childs.Length; i++)
            {
                EntitiesInfo.ManageMenu n = node.Childs[i];
                tmp += this._BuildMenuItem(n, level, path + "," + n.MM_Name);
            }
            return tmp;
        }
        //���ɽڵ����ı�
        //node:��ǰ�ڵ�
        //data:��������Դ
        //clas:��ǰ�ڵ��style
        private string _SysBuildNode(EntitiesInfo.ManageMenu m, string clas, string path)
        {
            Extend.MenuNode node = new Extend.MenuNode(m, _allMM);
            string temp = "<div nodeId=\"" + m.MM_Id + "\"";
            temp += " nodetype=\"" + m.MM_Type + "\" ";
            temp += " title='" + (m.MM_Intro == "" ? m.MM_Name : m.MM_Intro) + "'";
            temp += " isChild=\"" + node.IsChilds + "\"  type=\"" + clas + "\" >";
            //�˵��ڵ���Զ�����ʽ
            string style = " ";
            if (m.MM_Color != String.Empty && m.MM_Color != null) style += "color: " + m.MM_Color + ";";
            if (m.MM_IsBold) style += "font-weight: bold;";
            if (m.MM_IsItalic) style += "font-style: italic;";
            if (m.MM_Font != String.Empty && m.MM_Font != null) style += "font-family: '" + m.MM_Font + "';";
            string name = "<span style=\"" + style + "\">" + m.MM_Name + "</span>";
            if (m.MM_Link != "")
            {
                string link = "";
                link += "<{0} href=\"";                
                link += m.MM_Link + "\" isChild=\"" + node.IsChilds + "\" IsUse=\"" + m.MM_IsUse
                     + "\" width=\"" + m.MM_WinWidth + "\" height=\""+m.MM_WinHeight
                     + "\" path=\"" + path + "\" target=\"_blank\" class=\"a\">";
                link += name + "</{0}>";
                switch(m.MM_Type.ToLower())
                {
                    case "link":
                        link = string.Format(link, "a");
                        break;
                    default:
                        link = link.Replace("{0}", "span");
                        break;
                }

                temp += link;
            }
            else
            {
                temp += name;
            }
            temp += "</div>";
            return temp;
        }
    }
}
