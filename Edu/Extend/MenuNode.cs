using System;
using System.Collections.Generic;
using System.Text;

namespace Extend
{
    public class MenuNode
    {
        #region 属性
        private EntitiesInfo.ManageMenu[] _fullData;
        /// <summary>
        /// 所部节点
        /// </summary>
        public EntitiesInfo.ManageMenu[] FullData
        {
            get { return _fullData; }
            set { _fullData = value; }
        }
 
        private EntitiesInfo.ManageMenu _item;
        /// <summary>
        /// 自身节点
        /// </summary>
        public EntitiesInfo.ManageMenu Item
        {
            get { return _item; }
            set { _item = value; }
        }
        private EntitiesInfo.ManageMenu[] _Childs;
        /// <summary>
        /// 所有子级节点
        /// </summary>
        public EntitiesInfo.ManageMenu[] Childs
        {
            get { return _Childs; }
        }
        private bool _IsChilds=false;
        /// <summary>
        /// 是否有子级节点
        /// </summary>
        public bool IsChilds
        {
            get { return _IsChilds; }
        }
        private bool _IsLast=false;
        /// <summary>
        /// 是否为最后一个
        /// </summary>
        public bool IsLast
        {
            get { return _IsLast; }
        }
        private EntitiesInfo.ManageMenu _parent;
        /// <summary>
        /// 当前节点的上级节点
        /// </summary>
        public EntitiesInfo.ManageMenu Parent
        {
            get { return _parent; }
        }
        #endregion
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="item">自身节点对象</param>
        /// <param name="fulldata">所有节点数组</param>
        public MenuNode(EntitiesInfo.ManageMenu item, EntitiesInfo.ManageMenu[] fulldata)
        {
            if (item == null)
            {
                this._item = new EntitiesInfo.ManageMenu();
            }
            else
            {
                this._item = item;
            }
            this._fullData = fulldata;
            this._getChilds();
            //if (this._Childs.Length >0)
            //{
            //    this._IsChilds = true;
            //}
            this._getParent();
            _getIsLast();
        }
        #region 私有方法，求属性值
        /// <summary>
        /// 求当前节点的下级菜单
        /// </summary>
        private void _getChilds()
        {
            int length = 0;
            foreach (EntitiesInfo.ManageMenu m in this._fullData)
            {
                if (m.MM_PatId == Item.MM_Id)
                {
                    length++;
                    _IsChilds = true;
                }
            }
            this._Childs=new EntitiesInfo.ManageMenu[length];
            int i = 0;
            foreach (EntitiesInfo.ManageMenu n in this._fullData)
            {
                if (n.MM_PatId == Item.MM_Id)
                {
                    this._Childs[i++] = n;
                }
            }
            this._Childs = this.Sort(_Childs);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private EntitiesInfo.ManageMenu[] Sort(EntitiesInfo.ManageMenu[] array) 
        {
            for (int i = 0; i <= array.Length - 1; i++)
            {
                for (int j = array.Length - 1; j > i; j--)
                {
                    int jj = (int)array[j].MM_Tax;
                    int jn = (int)array[j - 1].MM_Tax;
                    if (jj < jn)
                    {
                        EntitiesInfo.ManageMenu temp = array[j];
                        array[j] = array[j - 1];
                        array[j - 1] = temp;
                    }
                }
            }
            return array;
        } 

        private void _getIsLast()
        {
            EntitiesInfo.ManageMenu tm = new EntitiesInfo.ManageMenu();
            foreach (EntitiesInfo.ManageMenu m in this._fullData)
            {
                if (m.MM_PatId == Item.MM_PatId)
                {
                    tm = m;
                }
            }
            if (tm.MM_Id == this._item.MM_Id)
            {
                this._IsLast = true;
            }  
            
        }
        private void _getParent()
        {
            foreach (EntitiesInfo.ManageMenu m in this._fullData)
            {
                if (m.MM_Id == Item.MM_PatId)
                {
                    this._parent = m;
                    break;
                }
            }
        }
       

        #endregion
    }
}
