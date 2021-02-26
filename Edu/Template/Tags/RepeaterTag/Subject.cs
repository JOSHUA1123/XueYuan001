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
    /// 专业
    /// </summary>
    public class Subject : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.RepeatTag tag = this.Tag as VTemplate.Engine.RepeatTag;
            if (tag == null) return;
            //
            int count = tag.Count;
            int start = int.Parse(this.Tag.Attributes.GetValue("start", "0")); //起始索引号
            string search = this.Tag.Attributes.GetValue("search", "");         //检索字符
            //上级专业的id
            int pid = 0;
            int.TryParse(this.Tag.Attributes.GetValue("pid", "-1"), out pid);
            //排序方式，def默认排序（先推荐，后排序号），tax按排序号,rec按推荐
            string order = this.Tag.Attributes.GetValue("order", "def");


            //数据来源            
            EntitiesInfo.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(this.Organ.Org_ID, search, true, pid, order, start, count);
            foreach (EntitiesInfo.Subject c in sbjs)
            {
                c.Sbj_Logo = Upload.Get["Subject"].Virtual + c.Sbj_Logo;
                c.Sbj_LogoSmall = Upload.Get["Subject"].Virtual + c.Sbj_LogoSmall;
                c.Sbj_Intro = HTML.ClearTag(c.Sbj_Intro);
            }
            List<Template.Tags.TreeObject> list = Template.Tags.TreeObject.Bulder(sbjs, "Sbj_PID", "0","Sbj_ID");
            tag.DataSourse = list;
          
            
        }
       
    }
}
