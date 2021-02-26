using System;
using System.Xml.Serialization;
namespace DataBaseInfo.Mapping
{
	[XmlRoot("tableMapping")]
	[Serializable]
	public class TableMapping
	{
		[XmlAttribute("timeout")]
		public int Timeout
		{
			get;
			set;
		}
		[XmlAttribute("className")]
		public string ClassName
		{
			get;
			set;
		}
		[XmlAttribute("usePrefix")]
		public bool UsePrefix
		{
			get;
			set;
		}
		[XmlAttribute("useSuffix")]
		public bool UseSuffix
		{
			get;
			set;
		}
		[XmlAttribute("mappingName")]
		public string MappingName
		{
			get;
			set;
		}
		[XmlElement("fieldMapping")]
		public FieldMapping[] Mappings
		{
			get;
			set;
		}
		public TableMapping()
		{
			this.Timeout = 60;
			this.UsePrefix = true;
			this.UseSuffix = true;
			this.Mappings = new FieldMapping[0];
		}
	}
}
