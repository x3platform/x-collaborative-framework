namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;
    #endregion

    /// <summary>应用连接访问令牌信息</summary>
    public class ConnectAccessTokenInfo
    {
        public ConnectAccessTokenInfo() { }

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

        #region 属性:ExpireDate
        private DateTime m_ExpireDate;

        /// <summary>过期时间</summary>
        public DateTime ExpireDate
        {
            get { return this.m_ExpireDate; }
            set { this.m_ExpireDate = value; }
        }
        #endregion

        #region 属性:ExpiresIn
        /// <summary>过期时间(单位:秒)</summary>
        public double ExpiresIn
        {
            get { return new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(this.ExpireDate.Ticks)).Duration().TotalSeconds; }
        }
        #endregion

        #region 属性:RefreshToken
        private string m_RefreshToken;

        /// <summary>刷新令牌</summary>
        public string RefreshToken
        {
            get { return this.m_RefreshToken; }
            set { this.m_RefreshToken = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>修改日期</summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>创建日期</summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion
    }
}
