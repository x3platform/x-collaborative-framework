namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>应用连接流量信息</summary>
    public class ConnectTrafficInfo
    {
        public ConnectTrafficInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>提交人标识</summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AppKey
        private string m_AppKey;

        /// <summary>应用标识</summary>
        public string AppKey
        {
            get { return this.m_AppKey; }
            set { this.m_AppKey = value; }
        }
        #endregion

        #region 属性:AuthorizationCode
        private string m_AuthorizationCode;

        /// <summary>授权代码</summary>
        public string AuthorizationCode
        {
            get { return this.m_AuthorizationCode; }
            set { this.m_AuthorizationCode = value; }
        }
        #endregion

        #region 属性:AuthorizationScope
        private string m_AuthorizationScope;

        /// <summary>授权范围</summary>
        public string AuthorizationScope
        {
            get { return this.m_AuthorizationScope; }
            set { this.m_AuthorizationScope = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>修改日期</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>创建日期</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
