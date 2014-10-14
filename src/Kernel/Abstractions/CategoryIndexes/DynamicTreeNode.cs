namespace X3Platform.CategoryIndexes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Util;

    /// <summary>异步加载的树节点</summary>
    public class DynamicTreeNode
    {
        #region 属性:Id
        private string m_Id;

        /// <summary>节点标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId;

        /// <summary>父级节点标识</summary>
        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Token
        private string m_Token;

        /// <summary>标记</summary>
        public string Token
        {
            get { return m_Token; }
            set { m_Token = value; }
        }
        #endregion

        #region 属性:CategoryIndex
        private string m_CategoryIndex;

        /// <summary>分类索引</summary>
        public string CategoryIndex
        {
            get { return m_CategoryIndex; }
            set { m_CategoryIndex = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url;

        /// <summary>链接地址，也可能是脚本函数</summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        #endregion

        #region 属性:Title
        private string m_Title;

        /// <summary></summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        #endregion

        #region 属性:Target
        private string m_Target = "_self";

        /// <summary></summary>
        public string Target
        {
            get { return this.m_Target; }
            set { this.m_Target = value; }
        }
        #endregion

        #region 属性:HasChildren
        private bool m_HasChildren = true;

        /// <summary>是否有叶子节点</summary>
        public bool HasChildren
        {
            get { return m_HasChildren; }
            set { m_HasChildren = value; }
        }
        #endregion

        #region 属性:Disabled
        private int m_Disabled;

        /// <summary>禁止</summary>
        public int Disabled
        {
            get { return m_Disabled; }
            set { m_Disabled = value; }
        }
        #endregion

        #region 构造函数:DynamicTreeNode()
        public DynamicTreeNode()
        {
        }
        #endregion

        #region 构造函数:DynamicTreeNode(string id, string parentId, string name, string title, string url, bool hasChildren)
        public DynamicTreeNode(string id, string parentId, string name, string title, string url, bool hasChildren)
            : this(id, parentId, name, title, url, hasChildren, -1)
        {
        }
        #endregion

        #region 构造函数:DynamicTreeNode(string id, string parentId, string name, string title, string url, bool hasChildren, int disabled)
        public DynamicTreeNode(string id, string parentId, string name, string title, string url, bool hasChildren, int disabled)
        {
            this.Id = id;
            this.ParentId = parentId;
            this.Name = name;
            this.Title = title;
            this.HasChildren = hasChildren;
            this.Disabled = -1;
        }
        #endregion

        #region 函数:ToString()
        /// <summary>返回表示当前对象的字符串</summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Title))
            {
                this.Title = this.Name;
            }

            if (this.HasChildren && this.Disabled == 1)
            {
                this.Url = string.Format("javascript:void(0)");
            }
            else
            {
                if (this.Url.ToLower().IndexOf("javascript:") == 0)
                {
                    this.Url = this.Url.Replace("{treeNodeId}", this.Id.Replace("$", "\\\\"))
                        .Replace("{treeNodeParentId}", this.ParentId.Replace("$", "\\\\"))
                        .Replace("{treeNodeName}", this.Name.Replace("\\", "\\\\").Replace("$", "\\\\"))
                        .Replace("{treeNodeToken}", this.Token.Replace("$", "\\\\"))
                        .Replace("{treeNodeCategoryIndex}", this.CategoryIndex.Replace("\\", "\\\\").Replace("$", "\\\\"));
                }
                else
                {
                    this.Url = this.Url.Replace("{treeNodeId}", this.Id)
                        .Replace("{treeNodeParentId}", this.ParentId)
                        .Replace("{treeNodeName}", this.Name)
                        .Replace("{treeNodeToken}", this.Token)
                        .Replace("{treeNodeCategoryIndex}", this.CategoryIndex);
                }
            }

            StringBuilder outString = new StringBuilder();

            outString.Append("{");
            outString.Append("\"id\":\"" + StringHelper.ToSafeJson(this.Id) + "\",");
            outString.Append("\"parentId\":\"" + StringHelper.ToSafeJson(this.ParentId) + "\",");
            outString.Append("\"name\":\"" + StringHelper.ToSafeJson(this.Name) + "\",");
            outString.Append("\"title\":\"" + StringHelper.ToSafeJson(this.Title) + "\",");
            outString.Append("\"url\":\"" + StringHelper.ToSafeJson(this.Url) + "\",");
            outString.Append("\"target\":\"" + StringHelper.ToSafeJson(this.Target) + "\",");
            outString.Append("\"hasChildren\":\"" + this.HasChildren.ToString().ToLower() + "\" ");
            outString.Append("}");

            return outString.ToString();
        }
        #endregion
    }
}
