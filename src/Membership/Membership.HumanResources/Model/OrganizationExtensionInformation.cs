//=============================================================================
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
//=============================================================================

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Membership.HumanResources.Model
{
    /// <summary>
    /// 实体类 GroupInfo 
    /// </summary>
    [Serializable]
    public class OrganizationExtensionInformation : IExtensionInformation
    {
        public OrganizationExtensionInformation() { }

        public OrganizationExtensionInformation(string id)
        {
            this.m_Id = id;
        }

        private Dictionary<string, object> properties = new Dictionary<string, object>();

        #region 索引:this[string name]
        /// <summary>属性索引信息</summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性值</returns>
        public object this[string name]
        {
            get { return this.properties[name]; }
            set
            {
                if (this.properties.ContainsKey(name))
                {
                    this.properties[name] = value;
                }
                else
                {
                    this.properties.Add(name, value);
                }
            }
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:ParentId
        /// <summary>[虚]原来的名称</summary>
        public string ParentId
        {
            get { return this.properties["ParentId"].ToString(); }
            set { this.properties["ParentId"] = value; }
        }
        #endregion

        #region 属性:UpdateDateView
        private string m_UpdateDateView;

        /// <summary>[虚]原来的名称</summary>
        public string UpdateDateView
        {
            get { return m_UpdateDateView; }
            set { m_UpdateDateView = value; }
        }
        #endregion

        #region 属性:OriginalName
        private string m_OriginalName;

        /// <summary>[虚]原来的名称</summary>
        public string OriginalName
        {
            get { return m_OriginalName; }
            set { m_OriginalName = value; }
        }
        #endregion

        #region 属性:OriginalGlobalName
        private string m_OriginalGlobalName;

        /// <summary>[虚]原来的部门全局简称</summary>
        public string OriginalGlobalName
        {
            get { return m_OriginalGlobalName; }
            set { m_OriginalGlobalName = value; }
        }
        #endregion

        #region 属性:OriginalParentId
        private int m_OriginalParentId;

        /// <summary>[虚]原来的父级部门编号</summary>
        public int OriginalParentId
        {
            get { return m_OriginalParentId; }
            set { m_OriginalParentId = value; }
        }
        #endregion

        //
        //
        //

        #region 属性:Leaders
        private IList<IAccountInfo> m_Leaders = new List<IAccountInfo>();

        /// <summary>负责人</summary>
        public IList<IAccountInfo> Leaders
        {
            get { return m_Leaders; }
            set { m_Leaders = value; }
        }
        #endregion

        #region 属性:Members
        private IList<IAccountInfo> m_Members = new List<IAccountInfo>();

        /// <summary>成员</summary>
        public IList<IAccountInfo> Members
        {
            get { return m_Members; }
            set { m_Members = value; }
        }
        #endregion

        #region 属性:ChiefLeaderId
        private int m_ChiefLeaderId;

        /// <summary>[虚]负责人标识</summary>
        public int ChiefLeaderId
        {
            get { return m_ChiefLeaderId; }
            set { m_ChiefLeaderId = value; }
        }
        #endregion

        #region IOrganizationExtensionInformation 成员
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IExtensionInformation 成员


        public void Load(Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IExtensionInformation 成员


        public void Load(System.Xml.XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
