using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DataBaseInfo.Mapping;
namespace DataBaseInfo
{
	[Serializable]
	public class EntityConfig
	{
		public static EntityConfig Instance = new EntityConfig();
		private TableSetting[] _Settings;
		private EntityConfig()
		{
			this.LoadConfig();
		}
		private void LoadConfig()
		{
			string text = ConfigurationManager.AppSettings["EntityConfigPath"];
			if (string.IsNullOrEmpty(text))
			{
				text = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[]
				{
					'\\'
				}) + "\\EntityConfig.xml";
			}
			else
			{
				if (text.Contains("~/") || text.Contains("~\\"))
				{
					text = text.Replace("/", "\\").Replace("~\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[]
					{
						'\\'
					}) + "\\");
				}
			}
			if (File.Exists(text))
			{
				XmlTextReader xmlTextReader = new XmlTextReader(text);
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableSetting[]));
					this._Settings = (xmlSerializer.Deserialize(xmlTextReader) as TableSetting[]);
				}
				catch
				{
				}
				finally
				{
					xmlTextReader.Close();
				}
			}
		}
		public void Refresh()
		{
			this.LoadConfig();
		}
		public int GetTableTimeout<T>() where T : class
		{
			if (this._Settings == null || this._Settings.Length == 0)
			{
				return 60;
			}
			string Namespace = typeof(T).Namespace;
			string ClassName = typeof(T).Name;
			List<TableSetting> list = new List<TableSetting>(this._Settings);
			TableSetting tableSetting = list.Find((TableSetting p) => p.Namespace == Namespace);
			if (tableSetting == null)
			{
				return 60;
			}
			if (tableSetting.Mappings == null || tableSetting.Mappings.Length <= 0)
			{
				return 60;
			}
			List<TableMapping> list2 = new List<TableMapping>(tableSetting.Mappings);
			TableMapping tableMapping = list2.Find((TableMapping p) => p.ClassName == ClassName);
			if (tableMapping != null)
			{
				return tableMapping.Timeout;
			}
			return 60;
		}
		public Table GetMappingTable<T>(string tableName) where T : class
		{
			if (this._Settings == null || this._Settings.Length == 0)
			{
				return new Table(tableName);
			}
			string Namespace = typeof(T).Namespace;
			string ClassName = typeof(T).Name;
			Table table = new Table(tableName);
			List<TableSetting> list = new List<TableSetting>(this._Settings);
			TableSetting tableSetting = list.Find((TableSetting p) => p.Namespace == Namespace);
			if (tableSetting != null)
			{
				table.Prefix = tableSetting.Prefix;
				table.Suffix = tableSetting.Suffix;
				if (tableSetting.Mappings != null && tableSetting.Mappings.Length > 0)
				{
					List<TableMapping> list2 = new List<TableMapping>(tableSetting.Mappings);
					TableMapping tableMapping = list2.Find((TableMapping p) => p.ClassName == ClassName);
					if (tableMapping != null && !string.IsNullOrEmpty(tableMapping.MappingName))
					{
						table.TableName = tableMapping.MappingName;
						if (!tableMapping.UsePrefix)
						{
							table.Prefix = null;
						}
						if (!tableMapping.UseSuffix)
						{
							table.Suffix = null;
						}
					}
				}
			}
			return table;
		}
		public Field GetMappingField<T>(string propertyName, string fieldName) where T : class
		{
			if (this._Settings == null || this._Settings.Length == 0)
			{
				return new Field(fieldName);
			}
			string Namespace = typeof(T).Namespace;
			string ClassName = typeof(T).Name;
			Field result = new Field(fieldName);
			List<TableSetting> list = new List<TableSetting>(this._Settings);
			TableSetting tableSetting = list.Find((TableSetting p) => p.Namespace == Namespace);
			if (tableSetting != null && tableSetting.Mappings != null && tableSetting.Mappings.Length > 0)
			{
				List<TableMapping> list2 = new List<TableMapping>(tableSetting.Mappings);
				TableMapping tableMapping = list2.Find((TableMapping p) => p.ClassName == ClassName);
				if (tableMapping != null && tableMapping.Mappings != null && tableMapping.Mappings.Length > 0)
				{
					List<FieldMapping> list3 = new List<FieldMapping>(tableMapping.Mappings);
					FieldMapping fieldMapping = list3.Find((FieldMapping p) => p.PropertyName == propertyName);
					if (fieldMapping != null)
					{
						result = new Field(fieldMapping.MappingName);
					}
				}
			}
			return result;
		}
	}
}
