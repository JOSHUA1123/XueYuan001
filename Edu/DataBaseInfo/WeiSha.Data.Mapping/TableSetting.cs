using System;
using System.Xml.Serialization;
namespace DataBaseInfo.Mapping
{
	[XmlRoot("tableSetting")]
	[Serializable]
	public class TableSetting
	{
		[XmlAttribute("namespace")]
		public string Namespace
		{
			get;
			set;
		}
		[XmlAttribute("prefix")]
		public string Prefix
		{
			get;
			set;
		}
		[XmlAttribute("suffix")]
		public string Suffix
		{
			get;
			set;
		}
		[XmlElement("tableMapping")]
		public TableMapping[] Mappings
		{
			get;
			set;
		}
		public TableSetting()
		{
			this.Mappings = new TableMapping[0];
		}
	}
}
