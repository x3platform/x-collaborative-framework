namespace X3Platform.Entities.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary></summary>
    public class EntitySchemaInfo
    {
        #region 构造函数:EntitySchemaInfo()
        /// <summary>默认构造函数</summary>
        public EntitySchemaInfo() { }
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

        #region 属性:Code
        private string m_Code = string.Empty;

        /// <summary></summary>
        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name = string.Empty;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
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

        #region 属性:EntityClassName
        private string m_EntityClassName = string.Empty;

        /// <summary></summary>
        public string EntityClassName
        {
            get { return m_EntityClassName; }
            set { m_EntityClassName = value; }
        }
        #endregion

        #region 属性:DataTableName
        private string m_DataTableName = string.Empty;

        /// <summary>数据表名称</summary>
        public string DataTableName
        {
            get { return m_DataTableName; }
            set { m_DataTableName = value; }
        }
        #endregion

        #region 属性:DataTablePrimaryKey
        private string m_DataTablePrimaryKey = string.Empty;

        /// <summary>数据表主键</summary>
        public string DataTablePrimaryKey
        {
            get { return m_DataTablePrimaryKey; }
            set { m_DataTablePrimaryKey = value; }
        }
        #endregion
        
        #region 属性:Tags
        private string m_Tags = string.Empty;

        /// <summary></summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:Lock
        private int m_Lock = 1;

        /// <summary>防止意外删除</summary>
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
