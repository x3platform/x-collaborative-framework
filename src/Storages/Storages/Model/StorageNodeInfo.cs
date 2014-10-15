#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :StorageNodeInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Storages.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>应用存储节点</summary>
    public class StorageNodeInfo : IStorageNode
    {
        #region 构造函数:StorageNodeInfo()
        /// <summary>默认构造函数</summary>
        public StorageNodeInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:StorageSchemaId
        private string m_StorageSchemaId;

        /// <summary></summary>
        public string StorageSchemaId
        {
            get { return this.m_StorageSchemaId; }
            set { this.m_StorageSchemaId = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        #endregion

        #region 属性:Type
        private string m_Type;

        /// <summary></summary>
        public string Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }
        #endregion

        #region 属性:ProviderName
        private string m_ProviderName;

        /// <summary></summary>
        public string ProviderName
        {
            get { return this.m_ProviderName; }
            set { this.m_ProviderName = value; }
        }
        #endregion

        #region 属性:ConnectionString
        private string m_ConnectionString;

        /// <summary></summary>
        public string ConnectionString
        {
            get { return this.m_ConnectionString; }
            set { this.m_ConnectionString = value; }
        }
        #endregion

        #region 属性:ConnectionState
        private int m_ConnectionState;

        /// <summary>链接状态</summary>
        public int ConnectionState
        {
            get { return this.m_ConnectionState; }
            set { this.m_ConnectionState = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId;

        /// <summary></summary>
        public string OrderId
        {
            get { return this.m_OrderId; }
            set { this.m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return this.m_Status; }
            set { this.m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion
    }
}
