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
    /// <summary>������������</summary>
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

        /// <summary>ʵ����ʶ</summary>
        public string EntityId
        {
            get { return m_EntityId; }
            set { m_EntityId = value; }
        }
        #endregion

        #region 属性:EntityClassName
        private string m_EntityClassName;

        /// <summary>ʵ��������</summary>
        public string EntityClassName
        {
            get { return m_EntityClassName; }
            set { m_EntityClassName = value; }
        }
        #endregion

        #region 属性:AttachmentEntityClassName
        private string m_AttachmentEntityClassName;

        /// <summary>����ʵ��������</summary>
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

        /// <summary>�������ļ�������</summary>
        public string AttachmentFolder
        {
            get { return m_AttachmentFolder; }
            set { m_AttachmentFolder = value; }
        }
        #endregion

        /// <summary>����</summary>
        /// <param name="id"></param>
        public void Find(string id)
        {

        }
    }
}
