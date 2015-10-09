using System;
using System.Collections.Generic;
using System.Drawing;

namespace X3Platform.Membership.OrganizationUnitCharts
{
    /// <summary></summary>
    public class OrganizationUnitChartNode : IDisposable
    {
        /// <summary></summary>
        public OrganizationUnitChartNode() { }

        /// <summary></summary>
        public OrganizationUnitChartNode(string id, string parentId, string code, string name, string description)
        {
            this.Id = id;
            this.ParentId = parentId;
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId = string.Empty;

        /// <summary>上级标识</summary>
        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary>编码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>描述</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Position
        /// <summary>Top Left Corner of the specific node</summary>
        private Rectangle m_Position;

        /// <summary>The top left corener of the chart node box</summary>
        public Rectangle Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }
        #endregion

        /// <summary>The data of the employee</summary>
        private IList<OrganizationUnitChartNode> m_ChildNodes;

        /// <summary>The record with all employee data</summary>
        public IList<OrganizationUnitChartNode> ChildNodes
        {
            get { return m_ChildNodes; }
            set { m_ChildNodes = value; }
        }

        private int m_ChildNodeCount = 0;

        /// <summary></summary>
        public int ChildNodeCount
        {
            get { return m_ChildNodeCount; }
            set { m_ChildNodeCount = value; }
        }

        /// <summary></summary>
        public void Dispose()
        {
        }
    }
}
