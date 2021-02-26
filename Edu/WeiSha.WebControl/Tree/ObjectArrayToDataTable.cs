using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace WeiSha.WebControl.Tree
{
    /// <summary>
    /// 将对象数组转换为DataTable
    /// 
    /// </summary>
    public class ObjectArrayToDataTable
    {
        public static DataTable To(Array arr)
        {
            if (arr.Length < 1)
                return (DataTable)null;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Tree", typeof(string)));
            dataTable.Columns.Add(new DataColumn("isTop", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("isDown", typeof(bool)));
            IEnumerator enumerator = arr.GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    foreach (PropertyInfo propertyInfo in enumerator.Current.GetType().GetProperties())
                        dataTable.Columns.Add(new DataColumn(propertyInfo.Name, typeof(string)));
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
            foreach (object obj in arr)
            {
                DataRow row = dataTable.NewRow();
                Type type = obj.GetType();
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                    row[propertyInfo.Name] = type.GetProperty(propertyInfo.Name).GetValue(obj, (object[])null);
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}
