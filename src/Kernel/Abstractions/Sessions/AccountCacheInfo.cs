namespace X3Platform.Sessions
{
    #region Using Libraries
    using System;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>帐号缓存信息</summary>
    [Serializable]
    public class AccountCacheInfo : ICacheable
    {
        /// <summary>构造函数</summary>
        public AccountCacheInfo() { }

        #region 属性:AccountIdentity
        private string m_AccountIdentity;

        /// <summary>帐号会话唯一标识</summary>
        public string AccountIdentity
        {
            get { return m_AccountIdentity; }
            set { m_AccountIdentity = value; }
        }
        #endregion

        #region 属性:AppKey
        private string m_AppKey;

        /// <summary>应用标识</summary>
        public string AppKey
        {
            get { return m_AppKey; }
            set { m_AppKey = value; }
        }
        #endregion

        #region 属性:AccountCacheValue
        private string m_AccountCacheValue;

        /// <summary>帐号会话的缓存信息</summary>
        public string AccountCacheValue
        {
            get { return m_AccountCacheValue; }
            set { m_AccountCacheValue = value; }
        }
        #endregion

        #region 属性:AccountObject
        private string m_AccountObject;

        /// <summary>帐号会话的缓存对象</summary>
        public string AccountObject
        {
            get { return m_AccountObject; }
            set { m_AccountObject = value; }
        }
        #endregion

        #region 属性:AccountObjectType
        private string m_AccountObjectType;

        /// <summary>AccountObject的类型</summary>
        public string AccountObjectType
        {
            get { return m_AccountObjectType; }
            set { m_AccountObjectType = value; }
        }
        #endregion

        #region 属性:IP
        private string m_IP;

        /// <summary>帐号会话的IP地址</summary>
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        #endregion

        #region 属性:BeginDate
        private DateTime m_BeginDate;

        /// <summary>开始时间</summary>
        public DateTime BeginDate
        {
            get { return m_BeginDate; }
            set { m_BeginDate = value; }
        }
        #endregion

        #region 属性:EndDate
        private DateTime m_EndDate;

        /// <summary>结束时间</summary>
        public DateTime EndDate
        {
            get { return m_EndDate; }
            set { m_EndDate = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>更新时间</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 显式实现 ICacheable
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
