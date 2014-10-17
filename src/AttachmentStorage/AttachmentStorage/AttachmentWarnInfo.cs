#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :AttachmentWarnInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>����������Ϣ</summary>
    public class AttachmentWarnInfo
    {
        #region ���캯��:AttachmentWarnInfo()
        /// <summary>Ĭ�Ϲ��캯��</summary>
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

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary></summary>
        public DateTime CreateDate
        {
            get { return this.m_CreateDate; }
            set { this.m_CreateDate = value; }
        }
        #endregion
    }
}
