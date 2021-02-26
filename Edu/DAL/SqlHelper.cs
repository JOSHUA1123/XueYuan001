using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Configuration;

namespace DAL
{
    /// <summary>
    /// Sql数据库相关操作
    /// </summary>
    public class SqlHelper
    {
        #region 隐藏字段
        private SqlConnection connection = null;
        private static string connStr = null;
        private bool autoCloseConnection = true;
        #endregion

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string ConnStr
        {
            get
            {
                if (connStr == null) connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                return connStr;
            }
        }
        /// <summary>
        /// 连接Connection对象
        /// </summary>
        private SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(ConnStr);
                }
                if (connection.State == ConnectionState.Closed) connection.Open();
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        /// <summary>
        /// 是否自动关闭连接
        /// </summary>
        public bool AutoCloseConnection
        {
            get
            {
                return autoCloseConnection;
            }
            set
            {
                autoCloseConnection = value;
            }
        }

        /// <summary>
        /// 执行Sql代码
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlparameters">参数组</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string sql, params SqlParameter[] sqlparameters)
        {
            int result = 0;

            using (SqlCommand cmd = Connection.CreateCommand())
            {
                if (sqlparameters != null)
                    cmd.Parameters.AddRange(sqlparameters);
                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            if (AutoCloseConnection == true) Connection.Close();

            return result;
        }
        /// <summary>
        /// 获取第一列第一行数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlparameters">参数组</param>
        /// <returns>返回第一列第一行数据</returns>
        public object ExecuteScalar(string sql, params SqlParameter[] sqlparameters)
        {
            object result = null;
            using (SqlCommand cmd = Connection.CreateCommand())
            {
                if (sqlparameters != null)
                    cmd.Parameters.AddRange(sqlparameters);
                cmd.CommandText = sql;
                result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }

            if (AutoCloseConnection == true) Connection.Close();
            return result;
        }
        /// <summary>
        /// 获取SqlDataReader对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlparameters">参数组</param>
        /// <returns>返回SqlDataReader对象</returns>
        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] sqlparameters)
        {
            SqlDataReader result = null;
            using (SqlCommand cmd = Connection.CreateCommand())
            {
                if (sqlparameters != null)
                    cmd.Parameters.AddRange(sqlparameters);
                cmd.CommandText = sql;
                result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            if (AutoCloseConnection == true) Connection.Close();
            return result;
        }
        /// <summary>
        /// 获取DataSet对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlparameters">参数组</param>
        /// <returns>返回DataSet对象</returns>
        public DataSet GetDataSet(string sql, params SqlParameter[] sqlparameters)
        {
            DataSet result = new DataSet();
            using (SqlDataAdapter adpter = new SqlDataAdapter(sql, Connection))
            {

                if (sqlparameters != null)
                    adpter.SelectCommand.Parameters.AddRange(sqlparameters);
                adpter.Fill(result);
                adpter.SelectCommand.Parameters.Clear();
            }

            if (AutoCloseConnection == true) Connection.Close();
            return result;
        }
        /// <summary>
        /// 获取DataTable对象(第一张表)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlparameters">参数组</param>
        /// <returns>返回DataTable对象</returns>
        public DataTable GetDataTable(string sql, params SqlParameter[] sqlparameters)
        {
            DataSet result = new DataSet();
            using (SqlDataAdapter adpter = new SqlDataAdapter(sql, Connection))
            {
                if (sqlparameters != null)
                    adpter.SelectCommand.Parameters.AddRange(sqlparameters);
                adpter.Fill(result);
                adpter.SelectCommand.Parameters.Clear();
            }

            if (AutoCloseConnection == true) Connection.Close();
            return result.Tables[0];
        }
        /// <summary>
        /// 执行存储过程 返回DataSet对象
        /// </summary>
        /// <param name="storedprocname">存储过程名称</param>
        /// <param name="sqlparameters">参数</param>
        /// <returns>返回DataSet对象</returns>
        public DataSet ExecuteStoredProcedureGetDataSet(string storedprocname, params SqlParameter[] sqlparameters)
        {
            DataSet result = new DataSet();
            using (SqlDataAdapter adpter = new SqlDataAdapter(storedprocname, Connection))
            {

                if (sqlparameters != null)
                    adpter.SelectCommand.Parameters.AddRange(sqlparameters);
                adpter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpter.Fill(result);
                adpter.SelectCommand.Parameters.Clear();
            }

            if (AutoCloseConnection == true) Connection.Close();
            return result;
        }

        /// <summary>
        /// 执行存储过程 返回DataTable对象
        /// </summary>
        /// <param name="storedprocname">存储过程名称</param>
        /// <param name="sqlparameters">参数</param>
        /// <returns>返回DataTable对象</returns>
        public DataTable ExecuteStoredProcedureGetDataTable(string storedprocname, params SqlParameter[] sqlparameters)
        {
            DataSet result = new DataSet();
            using (SqlDataAdapter adpter = new SqlDataAdapter(storedprocname, Connection))
            {

                if (sqlparameters != null)
                    adpter.SelectCommand.Parameters.AddRange(sqlparameters);
                adpter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adpter.Fill(result);
                adpter.SelectCommand.Parameters.Clear();
            }

            if (AutoCloseConnection == true) Connection.Close();

            return result.Tables[0];
        }

        /// <summary>
        /// 执行存储过程 返回受影响行数
        /// </summary>
        /// <param name="storedprocname">存储过程名称</param>
        /// <param name="sqlparameters">参数</param>
        /// <returns>返回受影响行数</returns>
        public int ExecuteStoredProcedure(string storedprocname, params SqlParameter[] sqlparameters)
        {
            int pos = 0;
            using (SqlDataAdapter adpter = new SqlDataAdapter(storedprocname, Connection))
            {

                if (sqlparameters != null)
                    adpter.SelectCommand.Parameters.AddRange(sqlparameters);
                adpter.SelectCommand.CommandType = CommandType.StoredProcedure;
                pos = adpter.SelectCommand.ExecuteNonQuery();
                adpter.SelectCommand.Parameters.Clear();
            }

            if (AutoCloseConnection == true) Connection.Close();
            return pos;
        }
    }
}

