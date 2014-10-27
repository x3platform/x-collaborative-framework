namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>应用数据包信息</summary>
    public class ApplicationPackageInfo
    {
        #region 构造函数:ApplicationPackageInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationPackageInfo() { }
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

        #region 属性:Application
        private ApplicationInfo m_Application;

        /// <summary>应用</summary>
        public ApplicationInfo Application
        {
            get
            {
                if (m_Application == null && !string.IsNullOrEmpty(this.ApplicationId))
                {
                    m_Application = AppsContext.Instance.ApplicationService.FindOne(this.ApplicationId);
                }

                return m_Application;
            }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return m_ApplicationId; }
            set { m_ApplicationId = value; }
        }
        #endregion

        #region 属性:ApplicationName
        /// <summary></summary>
        public string ApplicationName
        {
            get { return this.Application == null ? string.Empty : this.Application.ApplicationName; }
        }
        #endregion

        #region 属性:ApplicationDisplayName
        /// <summary></summary>
        public string ApplicationDisplayName
        {
            get { return this.Application == null ? string.Empty : this.Application.ApplicationDisplayName; }
        }
        #endregion

        #region 属性:Direction
        private string m_Direction;

        /// <summary>数据包的流向</summary>
        public string Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }
        #endregion
        
        #region 属性:Code
        private int m_Code;

        /// <summary></summary>
        public int Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }
        #endregion

        #region 属性:ParentCode
        private int m_ParentCode;

        /// <summary></summary>
        public int ParentCode
        {
            get { return m_ParentCode; }
            set { m_ParentCode = value; }
        }
        #endregion

        #region 属性:Type
        private string m_Type;

        /// <summary>类型</summary>
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        #endregion

        #region 属性:Path
        private string m_Path;

        /// <summary>数据包的文件路径</summary>
        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }
        #endregion

        #region 属性:BeginDate
        private DateTime m_BeginDate;

        /// <summary>开始时间</summary>
        public DateTime BeginDate
        {
            get { return m_BeginDate; }
            set { m_BeginDate = value; }
        }
        #endregion

        #region 属性:EndDate
        private DateTime m_EndDate;

        /// <summary>结束时间</summary>
        public DateTime EndDate
        {
            get { return m_EndDate; }
            set { m_EndDate = value; }
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
