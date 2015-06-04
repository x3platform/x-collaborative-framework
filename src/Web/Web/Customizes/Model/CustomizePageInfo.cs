namespace X3Platform.Web.Customizes.Model
{
    using System;

    using X3Platform.Util;

    /// <summary>页面信息</summary>
    [Serializable]
    public class CustomizePageInfo
    {
        public CustomizePageInfo()
        {
            this.Id = StringHelper.ToGuid();
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

        #region 属性:AuthorizationObjectType
        private string m_AuthorizationObjectType;

        /// <summary>权限对象类型</summary>
        public string AuthorizationObjectType
        {
            get { return m_AuthorizationObjectType; }
            set { m_AuthorizationObjectType = value; }
        }
        #endregion

        #region 属性:AuthorizationObjectId
        private string m_AuthorizationObjectId;

        /// <summary>权限对象标识</summary>
        public string AuthorizationObjectId
        {
            get { return m_AuthorizationObjectId; }
            set { m_AuthorizationObjectId = value; }
        }
        #endregion

        #region 属性:AuthorizationObjectName
        private string m_AuthorizationObjectName;

        /// <summary>权限对象名称</summary>
        public string AuthorizationObjectName
        {
            get { return m_AuthorizationObjectName; }
            set { m_AuthorizationObjectName = value; }
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
    }
}
