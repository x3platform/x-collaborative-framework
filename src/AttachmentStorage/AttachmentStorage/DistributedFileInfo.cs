namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;

    using X3Platform.AttachmentStorage.Configuration;
    #endregion

    /// <summary>文件信息</summary>
    public class DistributedFileInfo
    {
        #region 属性:Id
        private string m_Id;

        /// <summary>标识</summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:VirtualPath
        private string m_VirtualPath;

        /// <summary>虚拟路径</summary>
        public string VirtualPath
        {
            get { return m_VirtualPath; }
            set { m_VirtualPath = value; }
        }
        #endregion

        #region 属性:VirtualPathView
        /// <summary>虚拟路径</summary>
        public string VirtualPathView
        {
            get { return VirtualPath.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.VirtualUploadFolder); }
        }
        #endregion

        #region 属性:FileData
        private byte[] m_FileData = null;

        /// <summary>数据</summary>
        public byte[] FileData
        {
            get { return m_FileData; }
            set { m_FileData = value; }
        }
        #endregion

        #region 属性:CreateDate
        private DateTime m_CreateDate;

        /// <summary>创建日期</summary>
        public DateTime CreateDate
        {
            get { return m_CreateDate; }
            set { m_CreateDate = value; }
        }
        #endregion
    }
}
