namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;

    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>应用连接信息</summary>
    public class ConnectInfo : EntityClass, ICacheable
    {
        public ConnectInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
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

        #region 属性:AccountName
        private string m_AccountName = string.Empty;

        /// <summary>提交人姓名</summary>
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
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

        #region 属性:AppSecret
        private string m_AppSecret = string.Empty;

        /// <summary>应用私钥</summary>
        public string AppSecret
        {
            get { return this.m_AppSecret; }
            set { this.m_AppSecret = value; }
        }
        #endregion

        #region 属性:AppType
        private string m_AppType = string.Empty;

        /// <summary>应用类型</summary>
        public string AppType
        {
            get { return this.m_AppType; }
            set { this.m_AppType = value; }
        }
        #endregion

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary>编号</summary>
        public string Code
        {
            get { return this.m_Code; }
            set { this.m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary>名称</summary>
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>描述</summary>
        public string Description
        {
            get { return this.m_Description; }
            set { this.m_Description = value; }
        }
        #endregion

        #region 属性:Domain
        private string m_Domain = string.Empty;

        /// <summary>域</summary>
        public string Domain
        {
            get { return this.m_Domain; }
            set { this.m_Domain = value; }
        }
        #endregion

        #region 属性:RedirectUri
        private string m_RedirectUri = string.Empty;

        /// <summary>登录成功后重定向的地址</summary>
        public string RedirectUri
        {
            get { return this.m_RedirectUri; }
            set { this.m_RedirectUri = value; }
        }
        #endregion

        #region 属性:IconPath
        private string m_IconPath = string.Empty;

        /// <summary>图标文件</summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }
        #endregion

        #region 属性:Options
        private string m_Options = string.Empty;

        /// <summary>自定义属性</summary>
        public string Options
        {
            get { return this.m_Options; }
            set { this.m_Options = value; }
        }
        #endregion

        #region 属性:IsInternal
        private bool m_IsInternal = false;

        /// <summary>内置对象</summary>
        public bool IsInternal
        {
            get { return this.m_IsInternal; }
            set { this.m_IsInternal = value; }
        }
        #endregion

        #region 属性:AuthorizationScope
        private string m_AuthorizationScope = string.Empty;

        /// <summary>连接的授权范围(第三方应用默认的权限)</summary>
        public string AuthorizationScope
        {
            get { return this.m_AuthorizationScope; }
            set { this.m_AuthorizationScope = value; }
        }
        #endregion

        #region 属性:CertifiedCode
        private string m_CertifiedCode;

        /// <summary>验证代码</summary>
        public string CertifiedCode
        {
            get { return m_CertifiedCode; }
            set { m_CertifiedCode = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary>状态</summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary>修改日期</summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>创建日期</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 设置 EntityClass 标识
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion

        // -------------------------------------------------------
        // 显式实现 ICacheable
        // -------------------------------------------------------

        #region 属性:Expires
        private DateTime m_Expires = DateTime.MaxValue;

        /// <summary>过期时间</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}
