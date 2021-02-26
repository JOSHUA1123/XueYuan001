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
    /// 轮换图片
    /// </summary>
    public class Showpic : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            string site = this.Tag.Attributes.GetValue("site", "web");
            object from = this.Tag.Attributes.GetValue("from", null);
            if (from == null)
            {
                EntitiesInfo.ShowPicture[] shp = Business.Do<IStyle>().ShowPicAll(true, site, Organ.Org_ID);
                foreach (EntitiesInfo.ShowPicture s in shp)
                    s.Shp_File = Upload.Get["ShowPic"].Virtual + s.Shp_File;
                tag.DataSourse = shp;
            }
        }
    }
}
