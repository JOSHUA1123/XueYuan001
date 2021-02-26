using System;
using System.Configuration;
using System.Reflection;
using DataBaseInfo.SqlServer;
using DataBaseInfo.SqlServer9;
namespace DataBaseInfo
{
	public static class DbProviderFactory
	{
		public static DbProvider Default
		{
			get
			{
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
				string[] array = connectionStringSettings.ProviderName.Split(new char[]
				{
					','
				});
				DbProvider result;
				if (array.Length > 1)
				{
					result = DbProviderFactory.CreateDbProvider(array[0].Trim(), array[1].Trim(), connectionStringSettings.ConnectionString);
				}
				else
				{
					result = DbProviderFactory.CreateDbProvider(array[0].Trim(), null, connectionStringSettings.ConnectionString);
				}
				return result;
			}
		}
		public static DbProvider CreateDbProvider(DbProviderType providerType, string connectionString)
		{
			string[] array = EnumDescriptionAttribute.GetDescription(providerType).Split(new char[]
			{
				','
			});
			DbProvider result;
			if (array.Length > 1)
			{
				result = DbProviderFactory.CreateDbProvider(array[0].Trim(), array[1].Trim(), connectionString);
			}
			else
			{
				result = DbProviderFactory.CreateDbProvider(array[0].Trim(), null, connectionString);
			}
			return result;
		}
		public static DbProvider CreateDbProvider(string className, string assemblyName, string connectionString)
		{
			if (connectionString.ToLower().Contains("microsoft.jet.oledb") || connectionString.ToLower().Contains(".db3"))
			{
				if (connectionString.ToLower().IndexOf("data source") < 0)
				{
					throw new DataException("ConnectionString的格式有错误，请查证！");
				}
				string text = connectionString.Substring(connectionString.ToLower().IndexOf("data source") + "data source".Length + 1).TrimStart(new char[]
				{
					' ',
					'='
				});
				if (text.ToLower().StartsWith("|datadirectory|"))
				{
					text = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[]
					{
						'\\'
					}) + "\\App_Data" + text.Substring("|datadirectory|".Length);
				}
				else
				{
					if (connectionString.StartsWith("./") || connectionString.EndsWith(".\\"))
					{
						connectionString = connectionString.Replace("/", "\\").Replace(".\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[]
						{
							'\\'
						}) + "\\");
					}
				}
				connectionString = connectionString.Substring(0, connectionString.ToLower().IndexOf("data source")) + "Data Source=" + text;
			}
			if (connectionString.Contains("~/") || connectionString.Contains("~\\"))
			{
				connectionString = connectionString.Replace("/", "\\").Replace("~\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[]
				{
					'\\'
				}) + "\\");
			}
			if (string.IsNullOrEmpty(className))
			{
				className = typeof(SqlServer9Provider).ToString();
			}
			else
			{
				if (className.ToLower().IndexOf("System.Data.SqlClient".ToLower()) >= 0 || className.ToLower().Trim() == "sql" || className.ToLower().Trim() == "sqlserver")
				{
					className = typeof(SqlServerProvider).ToString();
				}
				else
				{
					if (className.ToLower().Trim() == "sql9" || className.ToLower().Trim() == "sqlserver9" || className.ToLower().Trim() == "sqlserver2005" || className.ToLower().Trim() == "sql2005")
					{
						className = typeof(SqlServer9Provider).ToString();
					}
				}
			}
			Assembly assembly;
			if (string.IsNullOrEmpty(assemblyName))
			{
				assembly = typeof(DbProvider).Assembly;
			}
			else
			{
				assembly = Assembly.Load(assemblyName);
			}
			return assembly.CreateInstance(className, false, BindingFlags.Default, null, new object[]
			{
				connectionString
			}, null, null) as DbProvider;
		}
		public static DbProvider CreateDbProvider(string connectName)
		{
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectName];
			string[] array = connectionStringSettings.ProviderName.Split(new char[]
			{
				','
			});
			DbProvider result;
			if (array.Length > 1)
			{
				result = DbProviderFactory.CreateDbProvider(array[0].Trim(), array[1].Trim(), connectionStringSettings.ConnectionString);
			}
			else
			{
				result = DbProviderFactory.CreateDbProvider(array[0].Trim(), null, connectionStringSettings.ConnectionString);
			}
			return result;
		}
		public static DbProvider CreateDbProvider(string connectName, DbProviderType providerType)
		{
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectName];
			return DbProviderFactory.CreateDbProvider(providerType, connectionStringSettings.ConnectionString);
		}
	}
}
