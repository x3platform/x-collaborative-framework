namespace X3Platform.AttachmentStorage
{
    #region Using Libraries
    using System;
    using System.IO;

    using X3Platform.Util;
    using X3Platform.Web;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.Util;
    #endregion

    /// <summary>附件文件信息</summary>
    public class AttachmentFileInfo : IAttachmentFileInfo
    {
        public AttachmentFileInfo()
        {
            this.Parent = new AttachmentParentObject();
        }

        public AttachmentFileInfo(IAttachmentParentObject parent)
        {
            this.Parent = parent;
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

        #region 属性:Parent
        private IAttachmentParentObject m_Parent;

        /// <summary>父级对象</summary>
        public IAttachmentParentObject Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }
        #endregion

        #region 属性:EntityId
        /// <summary>实体标识</summary>
        public string EntityId
        {
            get { return Parent.EntityId; }
            set { Parent.EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        /// <summary>实体类名称</summary>
        public string EntityClassName
        {
            get { return Parent.EntityClassName; }
            set { Parent.EntityClassName = value; }
        }
        #endregion

        #region 属性:AttachmentName
        private string m_AttachmentName;

        /// <summary>附件名称</summary>
        public string AttachmentName
        {
            get { return m_AttachmentName; }
            set { m_AttachmentName = value; }
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

        #region 属性:FolderRule
        private string m_FolderRule = string.Empty;

        /// <summary>文件夹规则</summary>
        public string FolderRule
        {
            get { return m_FolderRule; }
            set { m_FolderRule = value; }
        }
        #endregion

        #region 属性:FileType
        private string m_FileType;

        /// <summary>文件类型</summary>
        public string FileType
        {
            get { return m_FileType; }
            set { m_FileType = value; }
        }
        #endregion

        #region 属性:FileSize
        private int m_FileSize;

        /// <summary>文件大小</summary>
        public int FileSize
        {
            get { return m_FileSize; }
            set { m_FileSize = value; }
        }
        #endregion

        #region 属性:FileData
        private byte[] m_FileData = null;

        private bool m_FileDataLoaded = false;

        /// <summary>数据</summary>
        public byte[] FileData
        {
            get
            {
                if (!m_FileDataLoaded && m_FileData == null && !string.IsNullOrEmpty(VirtualPath))
                {
                    m_FileDataLoaded = true;

                    // -------------------------------------------------------
                    // 读取 二进制数据
                    // -------------------------------------------------------

                    string path = VirtualPath.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder);

                    if (path.IndexOf("~/") == 0)
                    {
                        path = VirtualPathHelper.GetPhysicalPath(path);
                    }

                    path = DirectoryHelper.FormatLocalPath(path);

                    if (File.Exists(path))
                    {
                        m_FileData = File.ReadAllBytes(path);
                    }
                }

                return this.m_FileData;
            }
            set { this.m_FileData = value; }
        }
        #endregion

        #region 属性:FileStatus
        private int m_FileStatus;

        /// <summary>文件状态:1 默认 2 回收(虚拟删除) 4 普通(确认上传数据)  8 归档 256 原始文件不存在 512 父级对象信息不存在</summary>
        public int FileStatus
        {
            get { return m_FileStatus; }
            set { m_FileStatus = value; }
        }
        #endregion

        #region 属性:OrderId
        private string m_OrderId = string.Empty;

        /// <summary>排序</summary>
        public string OrderId
        {
            get { return m_OrderId; }
            set { m_OrderId = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary>创建日期</summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        /// <summary>还原附件信息</summary>
        public void Restore(string id)
        {
            IAttachmentFileInfo temp = AttachmentStorageContext.Instance.AttachmentFileService.FindOne(id);

            this.Id = temp.Id;
            this.EntityId = temp.EntityId;
            this.EntityClassName = temp.EntityClassName;

            this.Parent.AttachmentFolder = temp.VirtualPath.Substring(0, temp.VirtualPath.IndexOf("/")).Replace("{uploads}", "");
            this.Parent.AttachmentEntityClassName = KernelContext.ParseObjectType(typeof(AttachmentFileInfo));

            this.AttachmentName = temp.AttachmentName;
            this.VirtualPath = temp.VirtualPath;
            this.FolderRule = temp.FolderRule;
            this.FileType = temp.FileType;
            this.FileSize = temp.FileSize;
            this.FileData = null;
            this.FileStatus = temp.FileStatus;

            this.CreatedDate = temp.CreatedDate;
        }

        /// <summary>保存附件信息</summary>
        public void Save()
        {
            AttachmentStorageContext.Instance.AttachmentFileService.Save(this);

            //
            // 保存 二进制数据
            //

            string path = UploadPathHelper.CombinePhysicalPath(Parent.AttachmentFolder, string.Format("{0}{1}", Id, FileType));

            UploadPathHelper.TryCreateDirectory(path);

            ByteHelper.ToFile(FileData, path);

            //
            // 保存 二进制信息
            //

            DistributedFileStorage.Upload(this);
        }
    }
}
