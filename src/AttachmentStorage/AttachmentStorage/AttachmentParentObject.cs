namespace X3Platform.AttachmentStorage
{
    /// <summary>附件父级对象</summary>
    public class AttachmentParentObject : IAttachmentParentObject
    {
        public AttachmentParentObject()
        {
        }

        public AttachmentParentObject(string entityId, string entityClassName, string attachmentEntityClassName, string attachmentFolder)
        {
            this.EntityId = entityId;
            this.EntityClassName = entityClassName;
            this.AttachmentEntityClassName = attachmentEntityClassName;
            this.AttachmentFolder = attachmentFolder;
        }

        #region 属性:EntityId
        private string m_EntityId;

        /// <summary>实体标识</summary>
        public string EntityId
        {
            get { return m_EntityId; }
            set { m_EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        private string m_EntityClassName;

        /// <summary>实体类名称</summary>
        public string EntityClassName
        {
            get { return m_EntityClassName; }
            set { m_EntityClassName = value; }
        }
        #endregion

        #region 属性:AttachmentEntityClassName
        private string m_AttachmentEntityClassName;

        /// <summary>附件实体类名称</summary>
        public string AttachmentEntityClassName
        {
            get { return m_AttachmentEntityClassName; }
            set
            {
                if (value.IndexOf(",") == -1)
                {
                    m_AttachmentEntityClassName = value;
                }
                else
                {
                    m_AttachmentEntityClassName = value.Substring(0, value.IndexOf(","));
                }
            }
        }
        #endregion

        #region 属性:AttachmentFolder
        private string m_AttachmentFolder;

        /// <summary>附件的文件夹名称</summary>
        public string AttachmentFolder
        {
            get { return m_AttachmentFolder; }
            set { m_AttachmentFolder = value; }
        }
        #endregion

        /// <summary>查找</summary>
        /// <param name="id"></param>
        public void Find(string id)
        {

        }
    }
}
