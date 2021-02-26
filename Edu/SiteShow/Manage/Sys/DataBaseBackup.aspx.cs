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

using Common;

using ServiceInterfaces;
using EntitiesInfo;
//using NBear.Data;
using System.Data.SqlClient;

namespace SiteShow.Manage.Sys
{
    public partial class DataBaseBackup : Extend.CustomPage
    {
        //���ݿ�����������·��
        private string dataBaseHy = "";
        //���ݿⱸ��Ŀ¼
        private string backDir = "";
        //���ݿⱸ���ļ���׺��
        private string backExt = ".bak";
        //���ݿ����ͣ�����access��sqlserver��
        private string dbType = "access";
        //Ҫ���������ݿ����ơ�
        private string dbName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //���ݿ�����
            dbType = Common.Server.DatabaseType;
            if (dbType == "access")
            {
                backDir = "accBackup";
                dataBaseHy = Common.Server.DatabaseFilePath;
            }
            else if (dbType == "sqlServer")
            {
                backDir = "sqlSBackup";
                backExt = ".sasp";
                ConnectionStringSettings connsett = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
                string str = connsett.ConnectionString;

                int a = str.IndexOf("Initial Catalog=");
                a = a == -1 ? str.IndexOf("DataBase=") + 9 : a + 16;
                string strjie = str.Substring(a);
                dbName = str.Substring(a, strjie.IndexOf(";"));

            }
            else
            {
                Message.Prompt("�˹��ܽ���Access���SQL Server�档");
                return;
            }
            //��ȡ����
            getDataBase();
            if (!IsPostBack)
            {
                //������
                BindData();
            }
            //this.Response.Write(dataBaseHy);
        }
        /// <summary>
        /// ��ȡ���ݿ�ĵ�ַ��
        /// </summary>
        private void getDataBase()
        {
            try
            {
                string backPath = Server.MapPath("~/App_Data/")+backDir;
                //����Ŀ¼
                DirectoryInfo di = new DirectoryInfo(backPath);
                if (!di.Exists)
                {
                    //�����Ŀ¼�����ڣ��򴴽�
                    di.Create();
                }
                backDir = di.FullName;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        private void BindData()
        {
            try
            {
                //����Ŀ¼
                DirectoryInfo di = new DirectoryInfo(this.backDir);

                DataTable dt = new DataTable("DataBase");
                DataColumn dc;
                //�ļ���
                dc = new DataColumn("file", Type.GetType("System.String"));
                dt.Columns.Add(dc);
                //·����
                dc = new DataColumn("path", Type.GetType("System.String"));
                dt.Columns.Add(dc);
                //����ʱ��
                dc = new DataColumn("time", Type.GetType("System.DateTime"));
                dt.Columns.Add(dc);
                //�ļ���С
                dc = new DataColumn("size", Type.GetType("System.Int32"));
                dt.Columns.Add(dc);
                //·����
                dc = new DataColumn("type", Type.GetType("System.String"));
                dt.Columns.Add(dc);
                //����Ŀ¼�µ������ļ�
                FileInfo[] fi = di.GetFiles();
                foreach (FileInfo file in fi)
                {
                    if (file.Extension != backExt && file.Extension.ToLower() != ".master")
                    {
                        continue;
                    }
                    DataRow dr = dt.NewRow();
                    dr["file"] = file.Name.Substring(0, file.Name.LastIndexOf('.'));
                    dr["path"] = file.FullName;
                    dr["time"] = file.CreationTime;
                    dr["size"] = file.Length / 1024;
                    dr["type"] = file.Extension.ToLower() == ".backup" ? "ϵͳ" : "";
                    dt.Rows.Add(dr);
                }
                DataView dv = dt.DefaultView;
                dv.Sort = "time desc";
                GridView1.DataSource = dv;
                GridView1.DataKeyNames = new string[] { "path" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ���ӱ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddEvent(object sender, EventArgs e)
        {

            string backName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            if (dbType == "access")
            {
                try
                {
                    FileInfo fi = new FileInfo(dataBaseHy);
                    fi.CopyTo(backDir + "\\" + backName + "" + backExt, true);
                    BindData();
                }
                catch (Exception ex)
                {
                    Message.ExceptionShow(ex);
                }
            }
            else if (dbType == "sqlServer")
            {
                sqlServerBackup(dbName, backDir+"\\"+backName+backExt);
                BindData();
            }
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string file in keys.Split(','))
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Exists)
                    {
                        if (fi.Extension.ToLower() != ".backup")
                            fi.Delete();
                    }

                }
                BindData();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                string file = this.GridView1.DataKeys[index].Value.ToString();
                FileInfo fi = new FileInfo(file);
                if (fi.Exists)
                {
                    if (fi.Extension.ToLower() != ".backup")
                        fi.Delete();
                }
                BindData();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ��ԭ���ݿ⣬��ť�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RecoverEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string file in keys.Split(','))
                {
                    FileInfo backDbase = new FileInfo(file);
                    if (dbType == "access")
                        backDbase.CopyTo(dataBaseHy, true);
                    else if (dbType == "sqlServer")
                        sqlServerRestore(dbName, backDbase.FullName);
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }          
        }
        /// <summary>
        /// ��ԭ���ݿ⣬�����е��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecover_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((WeiSha.WebControl.RowRecover)sender).Parent.Parent;
                //�����ļ�
                string backfile = GridView1.DataKeys[gr.RowIndex].Value.ToString();
                FileInfo backDbase = new FileInfo(backfile);
                if(dbType == "access")
                    backDbase.CopyTo(dataBaseHy, true);
                else if (dbType == "sqlServer")
                {
                    sqlServerRestore(dbName, backDbase.FullName);
                }
                new Extend.Scripts(this).Alert("��ԭ�ɹ���");
            }
            catch (Exception ex)
            {
                new Extend.Scripts(this).Alert(ex.Message);
            }           
        }
        /// <summary>
        /// SQL Server���ݿ�ı��ݡ�
        /// </summary>
        /// <param name="dbName">���ݿ�����</param>
        /// <param name="fileName">Ҫ���ݵ�λ��(����·��)</param>
        public void sqlServerBackup(string dbName, string fileName)
        {
            //ƴ��SQL��䡣
            string sql = "backup database " + dbName + " to disk='" + fileName + "' WITH FORMAT";
            //Gateway.Default.fr.FromCustomSql(sql);
            zhixingSql(sql);
        }
        /// <summary>
        /// SQL Server���ݿ�Ļָ�������SQL��䵽������ִ�С�
        /// </summary>
        /// <param name="baseName">���ݿ�����</param>
        /// <param name="fileName">Ҫ��ԭ�ı����ļ���λ��(����·�����ļ����ƺ���չ��)</param>
        public void sqlServerRestore(string baseName, string fileName)
        {
            string sql = "use master " +
                         "declare @sql varchar(100) while 1=1 begin select top 1 @sql='kill '+cast(spid as varchar(3)) from master..sysprocesses where spid>50 and spid<>@@spid and dbid=db_id('" + baseName + "') " +
                         "if @@rowcount=0 break exec(@sql) end RESTORE DATABASE " + baseName + " FROM disk = '" + fileName + "' WITH REPLACE";
            zhixingSql(sql);
        }
        /// <summary>
        /// ִ�����ɵ�SQL��䡣
        /// </summary>
        /// <param name="sql">Ҫִ�е�SQL���</param>
        public void zhixingSql(string sql)
        {
            try
            {
                //ִ��SQL��䡣
                ConnectionStringSettings connsett = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
                string ConnectString = connsett.ConnectionString;
                SqlConnection connection = new SqlConnection(ConnectString);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                new Extend.Scripts(this).Alert(ex.Message);
            }
        }
    }
}
