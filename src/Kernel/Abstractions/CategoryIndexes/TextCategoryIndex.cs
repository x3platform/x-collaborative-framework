// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;

namespace X3Platform.CategoryIndexes
{
    /// <summary>文本分类索引</summary>
    public class TextCategoryIndex : ICategoryIndex
    {
        #region 属性:Text
        private string m_Text = null;

        /// <summary>文本</summary>
        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }
        #endregion

        #region 属性:Value
        private string m_Value = null;

        /// <summary>值</summary>
        public string Value
        {
            get { return this.Parent == null ? m_Value : string.Format("{0}_{1}", this.Parent.Value, m_Value); }
            set { m_Value = value; }
        }
        #endregion

        #region 属性:Parent
        private ICategoryIndex m_Parnet = null;

        /// <summary>父级对象</summary>
        public ICategoryIndex Parent
        {
            get { return m_Parnet; }
        }
        #endregion

        #region 属性:ChildNodes
        private IList<ICategoryIndex> m_ChildNodes = null;

        public IList<ICategoryIndex> ChildNodes
        {
            get { return m_ChildNodes; }
        }
        #endregion

        #region 属性:HasChildren
        public bool HasChildren
        {
            get { return ChildNodes.Count == 0 ? false : true; }
        }

        #endregion

        #region 函数:LoadChildNode(ICategoryIndex  node)
        public void LoadChildNode(ICategoryIndex node)
        {
            bool isExist = false;

            foreach (ICategoryIndex index in this.ChildNodes)
            {
                if (node.Value == index.Value)
                {
                    isExist = true;
                    index.LoadChildNodes(node.ChildNodes);

                    break;
                }
            }

            if (!isExist)
            {
                this.ChildNodes.Add(node);
            }
        }
        #endregion

        #region 函数:LoadChildNodes(IList<ICategoryIndex> nodes)
        public void LoadChildNodes(IList<ICategoryIndex> nodes)
        {
            foreach (ICategoryIndex node in nodes)
            {
                this.LoadChildNode(node);
            }
        }
        #endregion

        public TextCategoryIndex()
        {
            m_ChildNodes = new List<ICategoryIndex>();
        }

        public TextCategoryIndex(ICategoryIndex parent)
            : this(parent, null)
        {
        }

        public TextCategoryIndex(string index)
            : this(null, index)
        {
        }

        public TextCategoryIndex(ICategoryIndex parent, string index)
            : this()
        {
            if (parent != null)
            {
                this.m_Parnet = parent;
            }

            this.Load(index);
        }

        private void Load(string index)
        {
            // 去除空白信息
            if (index != null)
                index = index.Trim();
            
            if (string.IsNullOrEmpty(index))
                return;

            string[] keys = index.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (keys.Length > 0)
            {
                this.Text = keys[0];

                this.Value = keys[0];
            }

            //
            // 子节点处理
            //

            if (keys.Length > 1)
            {
                ICategoryIndex target = this;

                for (int x = 1; x < keys.Length; x++)
                {
                    bool isExit = false;

                    IList<ICategoryIndex> nodes = target.ChildNodes;

                    for (int y = 0; y < nodes.Count; y++)
                    {
                        if (keys[x] == nodes[y].Value)
                        {
                            isExit = true;
                            target = nodes[y];
                            break;
                        }
                    }

                    if (!isExit)
                    {
                        target.LoadChildNode(new TextCategoryIndex(target, keys[x]));

                        target = target.ChildNodes[target.ChildNodes.Count - 1];
                    }
                }
            }
        }
    }
}