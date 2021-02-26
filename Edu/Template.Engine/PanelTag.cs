using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
    /// <summary>
    /// 面板数据标签,如: &lt;vt:panel id="header" /&gt;或者 &lt;vt:panel container="header"&gt;&lt;/vt:panel&gt;
    /// 
    /// </summary>
    public class PanelTag : Tag
    {
        /// <summary>
        /// 返回标签的名称
        /// 
        /// </summary>
        public override string TagName
        {
            get
            {
                return "panel";
            }
        }

        /// <summary>
        /// 返回此标签是否是单一标签.即是不需要配对的结束标签
        /// 
        /// </summary>
        internal override bool IsSingleTag
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 面板所在的容器标签
        /// 
        /// </summary>
        public string Container { get; protected set; }

        /// <summary>
        /// 此容器下所拥有的面板标签
        /// 
        /// </summary>
        protected List<PanelTag> Panels { get; private set; }

        /// <summary/>
        /// <param name="ownerTemplate"/>
        internal PanelTag(Template ownerTemplate)
            : base(ownerTemplate)
        {
            this.Panels = new List<PanelTag>();
        }

        /// <summary>
        /// 呈现本元素的数据
        /// 
        /// </summary>
        /// <param name="writer"/>
        public override void Render(TextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.Container))
                return;
            base.Render(writer);
        }

        /// <summary>
        /// 呈现数据到容器里
        /// 
        /// </summary>
        /// <param name="writer"/>
        private void RenderToContainer(TextWriter writer)
        {
            base.Render(writer);
        }

        /// <summary>
        /// 呈现本元素的数据
        /// 
        /// </summary>
        /// <param name="writer"/>
        protected override void RenderTagData(TextWriter writer)
        {
            CancelEventArgs args = new CancelEventArgs();
            this.OnBeforeRender(args);
            if (!args.Cancel)
            {
                foreach (Element element in this.InnerElements)
                    element.Render(writer);
                foreach (PanelTag panelTag in this.Panels)
                    panelTag.RenderToContainer(writer);
            }
            this.OnAfterRender(EventArgs.Empty);
        }

        /// <summary>
        /// 开始解析标签数据
        /// 
        /// </summary>
        /// <param name="ownerTemplate">宿主模板</param><param name="container">标签的容器</param><param name="tagStack">标签堆栈</param><param name="text"/><param name="match"/><param name="isClosedTag">是否闭合标签</param>
        /// <returns>
        /// 如果需要继续处理EndTag则返回true.否则请返回false
        /// </returns>
        internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
        {
            if (string.IsNullOrEmpty(this.Container) && string.IsNullOrEmpty(this.Id))
                throw new ParserException(string.Format("{0}标签中必须定义id或者container属性", (object)this.TagName));
            if (!string.IsNullOrEmpty(this.Container))
            {
                PanelTag panelTag = this.OwnerDocument.GetChildTagById(this.Container) as PanelTag;
                if (panelTag == null)
                    throw new ParserException(string.Format("{0}标签中Container属性定义的“{1}”<vt:panel>标签不存在", (object)this.TagName, (object)this.Container));
                panelTag.Panels.Add(this);
            }
            return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
        }

        /// <summary>
        /// 添加标签属性时的触发函数.用于设置自身的某些属性值
        /// 
        /// </summary>
        /// <param name="name"/><param name="item"/>
        protected override void OnAddingAttribute(string name, Attribute item)
        {
            switch (name)
            {
                case "container":
                    this.Container = item.Text;
                    break;
            }
        }

        /// <summary>
        /// 克隆当前元素到新的宿主模板
        /// 
        /// </summary>
        /// <param name="ownerTemplate"/>
        /// <returns/>
        internal override Element Clone(Template ownerTemplate)
        {
            PanelTag panelTag1 = new PanelTag(ownerTemplate);
            panelTag1.Container = this.Container;
            this.CopyTo((Tag)panelTag1);
            if (!string.IsNullOrEmpty(this.Container))
            {
                PanelTag panelTag2 = ownerTemplate.OwnerDocument.GetChildTagById(this.Container) as PanelTag;
                if (panelTag2 != null)
                    panelTag2.Panels.Add(panelTag1);
            }
            return (Element)panelTag1;
        }
    }
}
