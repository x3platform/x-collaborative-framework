// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ApplicationPackageLogInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Apps.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary></summary>
    public class ApplicationPackageLogInfo
    {
        #region 构造函数:ApplicationPackageLogInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationPackageLogInfo() { }
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

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return m_ApplicationId; }
            set { m_ApplicationId = value; }
        }
        #endregion

        #region 属性:PackageCode
        private int m_PackageCode;

        /// <summary></summary>
        public int PackageCode
        {
            get { return m_PackageCode; }
            set { m_PackageCode = value; }
        }
        #endregion

        #region 属性:Success
        private bool m_Success;

        /// <summary>是否发送成功</summary>
        public bool Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }
        #endregion
        #region 属性:Message
        private string m_Message;

        /// <summary></summary>
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
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
