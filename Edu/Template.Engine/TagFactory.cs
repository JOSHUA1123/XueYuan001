using System;
using System.Collections.Generic;

namespace VTemplate.Engine
{
	/// <summary>
	/// 标签工厂
	/// </summary>
	// Token: 0x02000016 RID: 22
	internal class TagFactory
	{
       
		/// <summary>
    /// 根据标签名建立标签实例
    /// 
    /// </summary>
    /// <param name="ownerTemplate"/><param name="tagName"/>
    /// <returns/>
    internal static Tag FromTagName(Template ownerTemplate, string tagName)
    {
      if (!string.IsNullOrEmpty(tagName))
      {
        switch (tagName.ToLower())
        {
          case "for":
            return (Tag) new ForTag(ownerTemplate);
          case "foreach":
            return (Tag) new ForEachTag(ownerTemplate);
          case "foreachelse":
            return (Tag) new ForEachElseTag(ownerTemplate);
          case "list":
            return (Tag) new ListTag(ownerTemplate);
          case "listelse":
            return (Tag) new ListElseTag(ownerTemplate);
          case "repeat":
            return (Tag) new RepeatTag(ownerTemplate);
          case "repeatelse":
            return (Tag) new RepeatElseTag(ownerTemplate);
          case "detail":
            return (Tag) new DetailTag(ownerTemplate);
          case "detailelse":
            return (Tag) new DetailElseTag(ownerTemplate);
          case "if":
            return (Tag) new IfTag(ownerTemplate);
          case "elseif":
            return (Tag) new IfConditionTag(ownerTemplate);
          case "else":
            return (Tag) new ElseTag(ownerTemplate);
          case "template":
            return (Tag) new Template(ownerTemplate);
          case "include":
            return (Tag) new IncludeTag(ownerTemplate);
          case "expression":
            return (Tag) new ExpressionTag(ownerTemplate);
          case "function":
            return (Tag) new FunctionTag(ownerTemplate);
          case "property":
            return (Tag) new PropertyTag(ownerTemplate);
          case "serverdata":
            return (Tag) new ServerDataTag(ownerTemplate);
          case "set":
            return (Tag) new SetTag(ownerTemplate);
          case "import":
            return (Tag) new ImportTag(ownerTemplate);
          case "output":
            return (Tag) new OutputTag(ownerTemplate);
          case "panel":
            return (Tag) new PanelTag(ownerTemplate);
          case "datareader":
            if (ownerTemplate.OwnerDocument.DocumentConfig != null && ownerTemplate.OwnerDocument.DocumentConfig.TagOpenMode == TagOpenMode.Full)
              return (Tag) new DataReaderTag(ownerTemplate);
            return (Tag) null;
        }
      }
      return (Tag) null;
    }
  
	}
}
