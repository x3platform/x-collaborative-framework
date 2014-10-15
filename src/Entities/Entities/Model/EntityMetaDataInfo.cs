#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityMetaDataInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

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
        #region ���캯��:EntityMetaDataInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
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

        /// <summary>����ʵ���ܹ�</summary>
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
        /// <summary>����ʵ���ܹ�����</summary>
        public string EntitySchemaName
        {
            get { return (this.EntitySchema == null) ? string.Empty : this.EntitySchema.Name; }
        }
        #endregion

        #region 属性:FieldName
        private string m_FieldName = string.Empty;

        /// <summary>�ֶ�����</summary>
        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }
        #endregion

        #region 属性:FieldType
        private string m_FieldType = string.Empty;

        /// <summary>�ֶ�����</summary>
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

        /// <summary>�����ֶ�����</summary>
        public string DataColumnName
        {
            get { return m_DataColumnName; }
            set { m_DataColumnName = value; }
        }
        #endregion

        #region 属性:EffectScope
        private int m_EffectScope;

        /// <summary>���÷�Χ 1 ��ͨ�ֶ� 2 �������ֶ�</summary>
        public int EffectScope
        {
            get { return m_EffectScope; }
            set { m_EffectScope = value; }
        }
        #endregion

        #region 属性:EffectScopeView
        private string m_EffectScopeView;

        /// <summary>�ֶ����÷�Χ��ͼ 1:��ͨ�ʺ� 8:����Ա�ʺ�</summary>
        public string EffectScopeView
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_EffectScopeView))
                {
                    this.m_EffectScopeView = AppsContext.Instance.ApplicationSettingService.GetText(
                        AppsContext.Instance.ApplicationService[EntitiesConfiguration.ApplicationName].Id,
                        "Ӧ�ù���_Эͬƽ̨_ʵ�����ݹ���_�ֶ����÷�Χ",
                        this.EffectScope.ToString()
                        );
                }

                return this.m_EffectScopeView;
            }
        }
        #endregion

        #region 属性:Lock
        private int m_Lock = 1;

        /// <summary>��ֹ����ɾ��</summary>
        public int Lock
        {
            get { return m_Lock; }
            set { m_Lock = value; }
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
