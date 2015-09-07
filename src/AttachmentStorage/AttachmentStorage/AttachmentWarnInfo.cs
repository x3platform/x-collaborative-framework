namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>附件警告信息</summary>
    public class AttachmentWarnInfo
    {
        #region 构造函数:AttachmentWarnInfo()
        /// <summary>默认构造函数</summary>
        public AttachmentWarnInfo()
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

        #region 属性:WarnType
        private string m_WarnType;

        /// <summary></summary>
        public string WarnType
        {
            get { return this.m_WarnType; }
            set { this.m_WarnType = value; }
        }
        #endregion

        #region 属性:Message
        private string m_Message;

        /// <summary></summary>
        public string Message
        {
            get { return this.m_Message; }
            set { this.m_Message = value; }
        }
        #endregion

        #region 属性:AttachmentStorageId
        private string m_AttachmentStorageId;

        /// <summary></summary>
        public string AttachmentStorageId
        {
            get { return this.m_AttachmentStorageId; }
            set { this.m_AttachmentStorageId = value; }
        }
        #endregion

        #region 属性:VirtualPath
        private string m_VirtualPath;

        /// <summary></summary>
        public string VirtualPath
        {
            get { return this.m_VirtualPath; }
            set { this.m_VirtualPath = value; }
        }
        #endregion

        #region 属性:AttachmentName
        private string m_AttachmentName;

        /// <summary></summary>
        public string AttachmentName
        {
            get { return this.m_AttachmentName; }
            set { this.m_AttachmentName = value; }
        }
        #endregion

        #region 属性:FileType
        private string m_FileType;

        /// <summary></summary>
        public string FileType
        {
            get { return this.m_FileType; }
            set { this.m_FileType = value; }
        }
        #endregion

        #region 属性:FileSize
        private int m_FileSize;

        /// <summary></summary>
        public int FileSize
        {
            get { return this.m_FileSize; }
            set { this.m_FileSize = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return this.m_CreatedDate; }
            set { this.m_CreatedDate = value; }
        }
        #endregion
    }
}
