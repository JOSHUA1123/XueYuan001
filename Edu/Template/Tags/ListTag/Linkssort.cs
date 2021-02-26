using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

using EntitiesInfo;
using ServiceInterfaces;
using VTemplate.Engine;

namespace Template.Tags.ListTag
{
    /// <summary>
    /// 友情链接分类
    /// </summary>
    public class Linkssort : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int count = int.Parse(this.Tag.Attributes.GetValue("count", "-1"));            
            EntitiesInfo.LinksSort[] sorts = Business.Do<ILinks>().GetSortCount(Organ.Org_ID, true, true, count);
            foreach (EntitiesInfo.LinksSort s in sorts)
            {
                s.Ls_Logo = Upload.Get["Links"].Virtual + s.Ls_Logo;
            }
            tag.DataSourse = sorts;    
        }
    }
}
