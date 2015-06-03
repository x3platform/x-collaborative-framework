// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Web.Customizes.Model
{
    using System;

    /// <summary>页面信息</summary>
    [Serializable]
    public class WidgetInfo : EntityClass
    {
        public WidgetInfo()
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

        #region 属性:Title
        private string m_Title;

        /// <summary>标题</summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        #endregion

        #region 属性:Height
        private int m_Height;

        /// <summary>高度</summary>
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }
        #endregion

        #region 属性:Width
        private int m_Width;

        /// <summary>宽度</summary>
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url;

        /// <summary>Url</summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
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

        #region 属性:Options
        private string m_Options;

        /// <summary>配置</summary>
        public string Options
        {
            get { return m_Options; }
            set { m_Options = value; }
        }
        #endregion

        #region 属性:OptionHtml
        private string m_OptionHtml;

        /// <summary>编辑框</summary>
        public string OptionHtml
        {
            get { return m_OptionHtml; }
            set { m_OptionHtml = value; }
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

        #region 属性:Html
        private string m_Html;

        /// <summary>Html代码</summary>
        public string Html
        {
            get { return m_Html; }
            set { m_Html = value; }
        }
        #endregion

        #region 属性:ClassName
        private string m_ClassName;

        /// <summary>类的名称</summary>
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }
        #endregion

        #region 属性:RedirctUrl
        private string m_RedirctUrl;

        /// <summary>指向的页面地址</summary>
        public string RedirctUrl
        {
            get { return m_RedirctUrl; }
            set { m_RedirctUrl = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId;

        /// <summary>排序</summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
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
