using System;
using System.Xml.Serialization;
namespace DataBaseInfo.Mapping
{
	[XmlRoot("fieldMapping")]
	[Serializable]
	public class FieldMapping
	{
		[XmlAttribute("propertyName")]
		public string PropertyName
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
	}
}
