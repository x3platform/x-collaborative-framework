namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;

    using X3Platform.Entities;
    #endregion

    /// <summary>实体类文档对象信息</summary>
    public class EntityDocObjectInfo : IEntityDocObjectInfo
    {
        #region 构造函数:EntityDocObjectInfo()
        /// <summary>默认构造函数</summary>
        public EntityDocObjectInfo()
        {
        }
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

        #region 属性:AccountId
        private string m_AccountId;

        /// <summary></summary>
        public string AccountId
        {
            get { return m_AccountId; }
            set { m_AccountId = value; }
        }
        #endregion

        #region 属性:AccountName
        private string m_AccountName;

        /// <summary></summary>
        public string AccountName
        {
            get { return m_AccountName; }
            set { m_AccountName = value; }
        }
        #endregion

        #region 属性:DocToken
        private string m_DocToken;

        /// <summary></summary>
        public string DocToken
        {
            get { return m_DocToken; }
            set { m_DocToken = value; }
        }
        #endregion

        #region 属性:DocTitle
        private string m_DocTitle;

        public string DocTitle
        {
            get { return m_DocTitle; }
            set { m_DocTitle = value; }
        }
        #endregion

        #region 属性:DocVersion
        private string m_DocVersion;

        public string DocVersion
        {
            get { return m_DocVersion; }
            set { m_DocVersion = value; }
        }
        #endregion

        #region 属性:DocStatus
        private string m_DocStatus;

        public string DocStatus
        {
            get { return m_DocStatus; }
            set { m_DocStatus = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion

    }
}
