using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

using EntitiesInfo;
using ServiceInterfaces;
using VTemplate.Engine;

namespace Template.Tags.RepeaterTag
{
    /// <summary>
    /// 课程导航（现在叫通知公告）的分类
    /// </summary>
    public class Guidecolumns : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.RepeatTag tag = this.Tag as VTemplate.Engine.RepeatTag;
            if (tag == null) return;

            int couid = int.Parse(this.Tag.Attributes.GetValue("couid", "-1"));  //所属课程的id
            EntitiesInfo.GuideColumns[] sorts = Business.Do<IGuide>().GetColumnsAll(couid, true);
            //
            List<Template.Tags.TreeObject> list = Template.Tags.TreeObject.Bulder(sorts, "Gc_PID", "0", "Gc_ID");
            tag.DataSourse = list;    
        }
    }
}
