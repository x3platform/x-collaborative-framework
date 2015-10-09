namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using X3Platform.Entities.Configuration;
    using X3Platform.Apps;
    #endregion

    /// <summary></summary>
    public class EntityMetaDataInfo
    {
        #region 构造函数:EntityMetaDataInfo()
        /// <summary>默认构造函数</summary>
        public EntityMetaDataInfo() { }
        #endregion

        #region 属性:Id
        private string m_Id = string.Empty;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:EntitySchema
        private EntitySchemaInfo m_EntitySchema;

        /// <summary>所属实体架构</summary>
        public EntitySchemaInfo EntitySchema
        {
            get
            {
                if (this.m_EntitySchema == null && !string.IsNullOrEmpty(this.EntitySchemaId))
                {
                    this.m_EntitySchema = EntitiesManagement.Instance.EntitySchemaService.FindOne(this.EntitySchemaId);
                }

                return this.m_EntitySchema;
            }
        }
        #endregion

        #region 属性:EntitySchemaId
        private string m_EntitySchemaId = string.Empty;

        /// <summary></summary>
        public string EntitySchemaId
        {
            get { return m_EntitySchemaId; }
            set { m_EntitySchemaId = value; }
        }
        #endregion

        #region 属性:EntitySchemaName
        /// <summary>所属实体架构名称</summary>
        public string EntitySchemaName
        {
            get { return (this.EntitySchema == null) ? string.Empty : this.EntitySchema.Name; }
        }
        #endregion

        #region 属性:FieldName
        private string m_FieldName = string.Empty;

        /// <summary>字段名称</summary>
        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }
        #endregion

        #region 属性:FieldType
        private string m_FieldType = string.Empty;

        /// <summary>字段类型</summary>
        public string FieldType
        {
            get { return m_FieldType; }
            set { m_FieldType = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary></summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:DataColumnName
        private string m_DataColumnName = string.Empty;

        /// <summary>数据字段名称</summary>
        public string DataColumnName
        {
            get { return m_DataColumnName; }
            set { m_DataColumnName = value; }
        }
        #endregion

        #region 属性:EffectScope
        private int m_EffectScope;

        /// <summary>作用范围 1 普通字段 2 大数据字段</summary>
        public int EffectScope
        {
            get { return m_EffectScope; }
            set { m_EffectScope = value; }
        }
        #endregion

        #region 属性:EffectScopeView
        private string m_EffectScopeView;

        /// <summary>字段作用范围视图 1:普通帐号 8:管理员帐号</summary>
        public string EffectScopeView
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_EffectScopeView))
                {
                    this.m_EffectScopeView = AppsContext.Instance.ApplicationSettingService.GetText(
                        AppsContext.Instance.ApplicationService[EntitiesConfiguration.ApplicationName].Id,
                        "应用管理_协同平台_实体数据管理_字段作用范围",
                        this.EffectScope.ToString()
                        );
                }

                return this.m_EffectScopeView;
            }
        }
        #endregion

        #region 属性:Locking
        private int m_Locking = 1;

        /// <summary>防止意外删除</summary>
        public int Locking
        {
            get { return m_Locking; }
            set { m_Locking = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

        /// <summary></summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:Status
        private int m_Status;

        /// <summary></summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        #endregion

        #region 属性:Remark
        private string m_Remark = string.Empty;

        /// <summary></summary>
        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion

        #region 属性:ModifiedDate
        private DateTime m_ModifiedDate;

        /// <summary></summary>
        public DateTime ModifiedDate
        {
            get { return m_ModifiedDate; }
            set { m_ModifiedDate = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion
    }
}
