// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================

namespace Elane.X.Web.Customize.Model
{
    using System;

    /// <summary>页面信息</summary>
    [Serializable]
    public class WidgetZoneInfo : EntityClass
    {
        public WidgetZoneInfo()
        {
        }

        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:Name
        private string m_Name;

        /// <summary>名称</summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
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

        #region 属性:Html
        private string m_Html;

        /// <summary>Html代码</summary>
        public string Html
        {
            get { return m_Html; }
            set { m_Html = value; }
        }
        #endregion

        #region 属性:UpdateDate
        private string m_UpdateDate;

        /// <summary>修改时间</summary>
        public string UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }
        #endregion

        #region 属性:CreateDate
        private string m_CreateDate;

        /// <summary>创建时间</summary>
        public string CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
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
    }
}
