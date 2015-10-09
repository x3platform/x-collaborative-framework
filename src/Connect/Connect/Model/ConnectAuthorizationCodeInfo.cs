namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>应用连接授权码信息</summary>
    public class ConnectAuthorizationCodeInfo
    {
        public ConnectAuthorizationCodeInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
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

        #region 属性:AccountId
        private string m_AccountId = string.Empty;

        /// <summary>提交人标识</summary>
        public string AccountId
        {
            get { return this.m_AccountId; }
            set { this.m_AccountId = value; }
        }
        #endregion

        #region 属性:AuthorizationScope
        private string m_AuthorizationScope;

        /// <summary>授权范围(除第三方应用默认的权限外，用户授予的权限)</summary>
        public string AuthorizationScope
        {
            get { return this.m_AuthorizationScope; }
            set { this.m_AuthorizationScope = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>修改日期</summary>
        public DateTime ModifiedDate
        {
            get { return this.m_ModifiedDate; }
            set { this.m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>创建日期</summary>
        public DateTime CreatedDate
        {
            get { return this.m_CreatedDate; }
            set { this.m_CreatedDate = value; }
        }
        #endregion
    }
}
