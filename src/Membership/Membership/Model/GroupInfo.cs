// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :GroupInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    /// <summary>普通群组信息</summary>
    public class GroupInfo : IGroupInfo
    {
        #region 构造函数:GroupInfo()
        /// <summary>默认构造函数</summary>
        public GroupInfo() { }
        #endregion

        /// <summary></summary>
        public GroupInfo(string id)
        {
            this.Id = id;
        }

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code;

        /// <summary>编码</summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
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

        #region 属性:GlobalName
        private string m_GlobalName;

        /// <summary>全局名称</summary>
        public string GlobalName
        {
            get { return string.IsNullOrEmpty(m_GlobalName) ? this.Name : this.m_GlobalName; }
            set { m_GlobalName = value; }
        }
        #endregion

        #region 属性:PinYin
        private string m_PinYin;

        /// <summary></summary>
        public string PinYin
        {
            get { return m_PinYin; }
            set { m_PinYin = value; }
        }
        #endregion

        #region 属性:GroupTreeNodeId
        private string m_GroupTreeNodeId;

        /// <summary></summary>
        public string GroupTreeNodeId
        {
            get { return m_GroupTreeNodeId; }
            set { m_GroupTreeNodeId = value; }
        }
        #endregion

        #region 属性:GroupTreeNodeName
        /// <summary>标准角色名称</summary>
        public string GroupTreeNodeName
        {
            get { return this.GroupTreeNode == null ? string.Empty : this.GroupTreeNode.Name; }
        }
        #endregion

        #region 属性:GroupTreeNode
        private GroupTreeNodeInfo m_GroupTreeNode = null;

        /// <summary>所属的标准组织</summary>
        public GroupTreeNodeInfo GroupTreeNode
        {
            get
            {
                if (m_GroupTreeNode == null && !string.IsNullOrEmpty(this.GroupTreeNodeId))
                {
                    m_GroupTreeNode = MembershipManagement.Instance.GroupTreeNodeService[this.GroupTreeNodeId];
                }

                return m_GroupTreeNode;
            }
        }
        #endregion

        #region 属性:EnableExchangeEmail
        private int m_EnableExchangeEmail;

        /// <summary>启用企业邮箱</summary>
        public int EnableExchangeEmail
        {
            get { return m_EnableExchangeEmail; }
            set { m_EnableExchangeEmail = value; }
        }
        #endregion

        #region 属性:Locking
        private int m_Locking = 1;

        /// <summary>防止意外删除 0 不锁定 | 1 锁定(默认)</summary>
        public int Locking
        {
            get { return m_Locking; }
            set { m_Locking = value; }
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

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary>备注</summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:FullPath
        private string m_FullPath = null;

        /// <summary>所属组织架构全路径</summary>
        public string FullPath
        {
            get { return m_FullPath; }
            set { m_FullPath = value; }
        }
        #endregion

        #region 属性:DistinguishedName
        private string m_DistinguishedName = null;

        /// <summary>唯一名称</summary>
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

        // -------------------------------------------------------
        // 重置成员关系
        // -------------------------------------------------------

        #region 函数:ResetMemberRelations(string relationText)
        /// <summary>重置成员关系</summary>
        /// <param name="relationText"></param>
        public void ResetMemberRelations(string relationText)
        {
            string[] list = relationText.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            // 清空旧的成员关系
            this.Members.Clear();

            string[] keys = null;

            // 设置新的成员关系
            foreach (string item in list)
            {
                keys = item.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                if (keys.Length > 2 && keys[0] == "account")
                {
                    this.Members.Add(MembershipManagement.Instance.AccountService[keys[1]]);
                }
            }
        }
        #endregion

        // -------------------------------------------------------
        // 群组所拥有的成员
        // -------------------------------------------------------

        #region 属性:Members
        private IList<IAccountInfo> m_Members = null;

        /// <summary>权限</summary>
        public IList<IAccountInfo> Members
        {
            get
            {
                if (m_Members == null && !string.IsNullOrEmpty(this.Id))
                {
                    m_Members = MembershipManagement.Instance.AccountService.FindAllByGroupId(this.Id);
                }

                return m_Members;
            }
        }
        #endregion

        #region 属性:MemberView
        private string m_MemberView;

        /// <summary>权限视图</summary>
        public string MemberView
        {
            get
            {
                if (this.Members != null
                    && string.IsNullOrEmpty(m_MemberView) && this.Members.Count > 0)
                {
                    foreach (IAccountInfo item in Members)
                    {
                        m_MemberView += item.GlobalName + ";";
                    }
                }

                return m_MemberView;
            }
        }
        #endregion

        #region 属性:MemberText
        private string m_MemberText;

        /// <summary>成员视图</summary>
        public string MemberText
        {
            get
            {
                if (this.Members != null
                    && string.IsNullOrEmpty(m_MemberText) && this.Members.Count > 0)
                {
                    foreach (IAccountInfo item in Members)
                    {
                        m_MemberText += string.Format("account#{0}#{1},", item.Id, item.GlobalName);
                    }
                }

                return m_MemberText;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 显式实现 IAuthorizationObject Type
        // ------------------------------------------------------- 

        #region 属性:IAuthorizationObject.Type
        /// <summary>类型</summary>
        string IAuthorizationObject.Type
        {
            get { return "Group"; }
        }
        #endregion

        // -------------------------------------------------------
        // Xml 元素的导入和导出 
        // -------------------------------------------------------

        #region 函数:Deserialize(XmlElement element)
        /// <summary></summary>
        /// <param name="element"></param>
        public void Deserialize(XmlElement element)
        {
            this.Id = element.SelectSingleNode("id").InnerText;
            this.Code = element.SelectSingleNode("code").InnerText;
            this.Name = element.SelectSingleNode("name").InnerText;
            this.PinYin = element.SelectSingleNode("pinyin").InnerText;
            // this.ParentId = element.SelectSingleNode("parentId")[0].InnerText;
            this.OrderId = element.SelectSingleNode("orderId").InnerText;
            this.Status = Convert.ToInt32(element.SelectSingleNode("status").InnerText);
            this.Remark = element.SelectSingleNode("remark").InnerText;
            this.ModifiedDate = Convert.ToDateTime(element.SelectSingleNode("updateDate").InnerText);
        }
        #endregion

        #region 函数:Serializable()
        /// <summary></summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary></summary>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            if (displayComment)
                outString.Append("<!-- 角色对象 -->");
            outString.Append("<role>");
            if (displayComment)
                outString.Append("<!-- 角色标识 (字符串) (nvarchar(36)) -->");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            if (displayComment)
                outString.Append("<!-- 编码 (字符串) (nvarchar(30)) -->");
            outString.AppendFormat("<code><![CDATA[{0}]]></code>", this.Code);
            if (displayComment)
                outString.Append("<!-- 名称 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<name><![CDATA[{0}]]></name>", this.Name);
            if (displayComment)
                outString.Append("<!-- 拼音 (字符串) (nvarchar(50)) -->");
            outString.AppendFormat("<pinyin><![CDATA[{0}]]></pinyin>", this.PinYin);
            if (displayComment)
                outString.Append("<!-- 排序编号(字符串) nvarchar(20) -->");
            outString.AppendFormat("<orderId><![CDATA[{0}]]></orderId>", this.OrderId);
            if (displayComment)
                outString.Append("<!-- 状态 (整型) (int) -->");
            outString.AppendFormat("<status><![CDATA[{0}]]></status>", this.Status);
            if (displayComment)
                outString.Append("<!-- 备注信息 (字符串) (nvarchar(200)) -->");
            outString.AppendFormat("<remark><![CDATA[{0}]]></remark>", this.Remark);
            if (displayComment)
                outString.Append("<!-- 最后更新时间 (时间) (datetime) -->");
            outString.AppendFormat("<updateDate><![CDATA[{0}]]></updateDate>", this.ModifiedDate);
            outString.Append("</role>");

            return outString.ToString();
        }
        #endregion
    }
}
