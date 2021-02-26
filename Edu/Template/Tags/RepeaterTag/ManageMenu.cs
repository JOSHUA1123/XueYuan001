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
    /// 管理菜单
    /// </summary>
    public class Managemenu : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.RepeatTag tag = this.Tag as VTemplate.Engine.RepeatTag;
            if (tag == null) return;
            //           
            //菜单是学生的，还是教师的，还是管理员的；
            //例如教师管理teacher,学生管理student,机构管理organAdmin
            string marker = this.Tag.Attributes.GetValue("marker", "");
            if (string.IsNullOrWhiteSpace(marker)) return;
            //数据来源            
            EntitiesInfo.ManageMenu[] mms = Business.Do<IPurview>().GetAll4Org(this.Organ.Org_ID, marker);
            List<Template.Tags.TreeObject> list = Template.Tags.TreeObject.Bulder(mms, "MM_PatId", "0", "MM_Id");
            tag.DataSourse = list;
          
            
        }
       
    }
}
