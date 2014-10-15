// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityLifeHistoryInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Entities.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Entities;

    /// <summary></summary>
    public class EntityLifeHistoryInfo : IEntityLifeHistoryInfo
    {
        #region ���캯��:EntityLifeHistoryInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
        public EntityLifeHistoryInfo() { }
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

        #region 属性:EntityId
        private string m_EntityId;

        /// <summary></summary>
        public string EntityId
        {
            get { return m_EntityId; }
            set { m_EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        private string m_EntityClassName;

        /// <summary></summary>
        public string EntityClassName
        {
            get { return m_EntityClassName; }
            set { m_EntityClassName = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary></summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:MethodName
        private string m_MethodName;

        /// <summary></summary>
        public string MethodName
        {
            get { return m_MethodName; }
            set { m_MethodName = value; }
        }
        #endregion

        #region 属性:ContextDiffLog
        private string m_ContextDiffLog = string.Empty;

        /// <summary></summary>
        public string ContextDiffLog
        {
            get { return m_ContextDiffLog; }
            set { m_ContextDiffLog = value; }
        }
        #endregion

        #region 属性:Date
        private DateTime m_Date;

        /// <summary></summary>
        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }
        #endregion
    }
}
