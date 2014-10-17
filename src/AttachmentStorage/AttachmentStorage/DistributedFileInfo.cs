// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AttachmentParentObject.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.AttachmentStorage
{
    using System;

    using X3Platform.AttachmentStorage.Configuration;

    /// <summary>�ļ���Ϣ</summary>
    public class DistributedFileInfo
    {
        #region 属性:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:VirtualPath
        private string m_VirtualPath;

        /// <summary>����·��</summary>
        public string VirtualPath
        {
            get { return m_VirtualPath; }
            set { m_VirtualPath = value; }
        }
        #endregion

        #region 属性:VirtualPathView
        /// <summary>����·��</summary>
        public string VirtualPathView
        {
            get { return VirtualPath.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.VirtualUploadFolder); }
        }
        #endregion

        #region 属性:FileData
        private byte[] m_FileData = null;

        /// <summary>����</summary>
        public byte[] FileData
        {
            get { return m_FileData; }
            set { m_FileData = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>��������</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
