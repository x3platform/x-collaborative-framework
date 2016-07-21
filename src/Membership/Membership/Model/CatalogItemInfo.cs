namespace X3Platform.Membership.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    /// <summary></summary>
    public class CatalogItemInfo
    {
        #region 构造函数:CatalogItemInfo()
        /// <summary>默认构造函数</summary>
        public CatalogItemInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:ParentId
        private string m_ParentId;

        /// <summary></summary>
        public string ParentId
        {
            get { return m_ParentId; }
            set { m_ParentId = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region 属性:IsKey
        private bool m_IsKey;

        /// <summary></summary>
        public bool IsKey
        {
            get { return m_IsKey; }
            set { m_IsKey = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId;

        /// <summary></summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status = 1;

        /// <summary></summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:DistinguishedName
        private string m_DistinguishedName = string.Empty;

        /// <summary></summary>
        public string DistinguishedName
        {
            get { return m_DistinguishedName; }
            set { m_DistinguishedName = value; }
        }
        #endregion


        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        //
        // Xml 元素的导入和导出 
        //

        #region 函数:Deserialize(XmlElement element)
        /// <summary></summary>
        /// <param name="element"></param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.GetElementsByTagName("id")[0].InnerText;
            this.Name = element.GetElementsByTagName("name")[0].InnerText;
            // this.NickName = element.GetElementsByTagName("name")[0].InnerText;
            // <fullName>组织架构</fullName>
            // this.Type = Convert.ToInt32(element.GetElementsByTagName("type")[0].InnerText);
            this.ParentId = element.GetElementsByTagName("parentId")[0].InnerText;
            this.OrderId = element.GetElementsByTagName("orderId")[0].InnerText;
            this.Status = Convert.ToInt32(element.GetElementsByTagName("status")[0].InnerText);
            this.ModifiedDate = Convert.ToDateTime(element.GetElementsByTagName("updateDate")[0].InnerText);
        }
        #endregion

        #region 函数:Serializable()
        /// <summary></summary>
        /// <returns></returns>
        public string Serializable()
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("<!-- 树形分组节点对象 -->");
            outString.Append("<CatalogItem>");
            outString.Append("<!-- 组织标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            outString.Append("<!-- 父级对象标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<parentId><![CDATA[{0}]]></parentId>", this.ParentId);
            outString.Append("<!-- 排序编号(字符串) nvarchar(20) -->");
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            outString.Append("<!-- 备注信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</CatalogItem>");

            return outString.ToString();
        }
        #endregion
    }
}
