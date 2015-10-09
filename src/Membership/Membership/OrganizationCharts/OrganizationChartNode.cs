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

        #region ����:Id
        private string m_Id = string.Empty;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region ����:ParentId
        private string m_ParentId = string.Empty;

        /// <summary>�ϼ���ʶ</summary>
        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }
        #endregion

        #region ����:Code
        private string m_Code = string.Empty;

        /// <summary>����</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region ����:Name
        private string m_Name = string.Empty;

        /// <summary>����</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region ����:Description
        private string m_Description = string.Empty;

        /// <summary>����</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region ����:Position
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
