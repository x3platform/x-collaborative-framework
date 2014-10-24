namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Entities;
    #endregion

    /// <summary></summary>
    public class EntityLifeHistoryInfo : IEntityLifeHistoryInfo
    {
        #region 构造函数:EntityLifeHistoryInfo()
        /// <summary>默认构造函数</summary>
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
