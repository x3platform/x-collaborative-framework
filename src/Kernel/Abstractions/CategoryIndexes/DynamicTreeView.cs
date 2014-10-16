namespace X3Platform.CategoryIndexes
{
    #region Using Libraries
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>异步加载的树</summary>
    public class DynamicTreeView
    {
        private string m_TreeName;

        /// <summary></summary>
        public string TreeName
        {
            get { return m_TreeName; }
            set { m_TreeName = value; }
        }

        private string m_ParentId;

        /// <summary></summary>
        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }

        private IList<DynamicTreeNode> nodes;

        /// <summary></summary>
        /// <param name="treeName"></param>
        /// <param name="parentId"></param>
        public DynamicTreeView(string treeName, string parentId)
        {
            this.m_TreeName = treeName;

            this.m_ParentId = parentId;

            this.nodes = new List<DynamicTreeNode>();
        }

        /// <summary>新增树的节点</summary>
        /// <param name="node"></param>
        public void Add(DynamicTreeNode node)
        {
            this.nodes.Add(node);
        }

        #region 函数:ToString()
        /// <summary>返回表示当前对象的字符串</summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("{\"tree\":\"" + this.TreeName + "\",\"parentId\":\"" + this.ParentId + "\",");

            outString.Append("childNodes:[");

            foreach (DynamicTreeNode node in nodes)
            {
                outString.Append(node.ToString() + ",");
            }

            if (outString.ToString().Substring(outString.Length - 1, 1) == ",")
            {
                outString = outString.Remove(outString.Length - 1, 1);
            }

            outString.Append("]}");

            return outString.ToString();
        }
        #endregion
    }
}
