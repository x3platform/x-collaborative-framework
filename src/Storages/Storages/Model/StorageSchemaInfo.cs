namespace X3Platform.Storages.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>应用存储架构信息</summary>
    public class StorageSchemaInfo : IStorageSchema
    {
        #region 构造函数:StorageSchemaInfo()
        /// <summary>默认构造函数</summary>
        public StorageSchemaInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return this.m_ApplicationId; }
            set { this.m_ApplicationId = value; }
        }
        #endregion

        #region 属性:AdapterClassName
        private string m_AdapterClassName;

        /// <summary></summary>
        public string AdapterClassName
        {
            get { return this.m_AdapterClassName; }
            set { this.m_AdapterClassName = value; }
        }
        #endregion

        #region 属性:StrategyClassName
        private string m_StrategyClassName;

        /// <summary></summary>
        public string StrategyClassName
        {
            get { return this.m_StrategyClassName; }
            set { this.m_StrategyClassName = value; }
        }
        #endregion

        #region 属性:Options
        private string m_Options;

        /// <summary></summary>
        public string Options
        {
            get { return this.m_Options; }
            set { this.m_Options = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId;

        /// <summary></summary>
        public string OrderId
        {
            get { return this.m_OrderId; }
            set { this.m_OrderId = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark;

        /// <summary></summary>
        public string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private DateTime m_UpdateDate;

        /// <summary></summary>
        public DateTime UpdateDate
        {
            get { return this.m_UpdateDate; }
            set { this.m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion

        /// <summary>获取存储适配对象</summary>
        public IStorageAdapter GetAdapterClass()
        {
            return (IStorageAdapter)KernelContext.CreateObject(this.AdapterClassName);
        }

        /// <summary>获取存储策略对象</summary>
        public IStorageStrategy GetStrategyClass()
        {
            return (IStorageStrategy)KernelContext.CreateObject(this.StrategyClassName, new object[] { this });
        }
    }
}
