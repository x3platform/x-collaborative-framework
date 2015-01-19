namespace X3Platform.Security.Authority
{
    #region Using Libraries
    using System;
    using X3Platform.CacheBuffer;
    #endregion

    /// <summary>权限信息</summary>
    public class AuthorityInfo : ICacheable
    {
        /// <summary>构造函数</summary>
        public AuthorityInfo() { }

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
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

        #region 属性:Description
        private string m_Description;

        /// <summary>描述信息</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Locking
        private int m_Locking;

        /// <summary>锁定 0=未锁定, 1=锁定</summary>
        public int Locking
        {
            get { return m_Locking; }
            set { m_Locking = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary>标签</summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
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

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary>修改时间</summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>创建时间</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
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
