// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AccountCacheInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Sessions
{
    using System;

    using X3Platform.CacheBuffer;

    /// <summary>�ʺŻ�����Ϣ</summary>
    [Serializable]
    public class AccountCacheInfo : ICacheable
    {
        /// <summary>���캯��</summary>
        public AccountCacheInfo() { }

        #region 属性:AccountIdentity
        private string m_AccountIdentity;

        /// <summary>�ʺŻỰΨһ��ʶ</summary>
        public string AccountIdentity
        {
            get { return m_AccountIdentity; }
            set { m_AccountIdentity = value; }
        }
        #endregion

        #region 属性:AppKey
        private string m_AppKey;

        /// <summary>Ӧ�ñ�ʶ</summary>
        public string AppKey
        {
            get { return m_AppKey; }
            set { m_AppKey = value; }
        }
        #endregion

        #region 属性:AccountCacheValue
        private string m_AccountCacheValue;

        /// <summary>�ʺŻỰ�Ļ�����Ϣ</summary>
        public string AccountCacheValue
        {
            get { return m_AccountCacheValue; }
            set { m_AccountCacheValue = value; }
        }
        #endregion

        #region 属性:AccountObject
        private string m_AccountObject;

        /// <summary>�ʺŻỰ�Ļ�������</summary>
        public string AccountObject
        {
            get { return m_AccountObject; }
            set { m_AccountObject = value; }
        }
        #endregion

        #region 属性:AccountObjectType
        private string m_AccountObjectType;

        /// <summary>AccountObject������</summary>
        public string AccountObjectType
        {
            get { return m_AccountObjectType; }
            set { m_AccountObjectType = value; }
        }
        #endregion

        #region 属性:IP
        private string m_IP;

        /// <summary>�ʺŻỰ��IP��ַ</summary>
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        #endregion

        #region 属性:BeginDate
        private DateTime m_BeginDate;

        /// <summary>��ʼʱ��</summary>
        public DateTime BeginDate
        {
            get { return m_BeginDate; }
            set { m_BeginDate = value; }
        }
        #endregion

        #region 属性:EndDate
        private DateTime m_EndDate;

        /// <summary>����ʱ��</summary>
        public DateTime EndDate
        {
            get { return m_EndDate; }
            set { m_EndDate = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>����ʱ��</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // ��ʽʵ�� ICacheable
        // -------------------------------------------------------

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddDays(1);

        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}
