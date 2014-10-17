// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.AttachmentStorage.BLL
{
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.IBLL;
    using X3Platform.AttachmentStorage.IDAL;
    using X3Platform.AttachmentStorage.Util;
    using System;

    public sealed class AttachmentStorageService : IAttachmentStorageService
    {
        private IAttachmentStorageProvider provider = null;

        private AttachmentStorageConfiguration configuration = null;

        public AttachmentStorageService()
        {
            // ��ȡ������Ϣ
            this.configuration = AttachmentStorageConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // �������ݷ�������
            this.provider = objectBuilder.GetObject<IAttachmentStorageProvider>(typeof(IAttachmentStorageProvider));
        }

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAttachmentFileInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(AccountInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</param>
        ///<returns>ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</returns>
        public IAttachmentFileInfo Save(IAttachmentFileInfo param)
        {
            if (string.IsNullOrEmpty(param.AttachmentName)) { throw new ArgumentNullException("�������Ʊ��"); }

            param.FileType = param.FileType.ToLower();

            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string id)
        ///<summary>ɾ����¼</summary>
        ///<param name="id">��ʶ</param>
        public void Delete(string id)
        {
            provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(int id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">AccountInfo Id��</param>
        ///<returns>����һ�� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        public IAttachmentFileInfo FindOne(string id)
        {
            AttachmentFileInfo param = (AttachmentFileInfo)provider.FindOne(id);

            IAttachmentParentObject parent = new AttachmentParentObject();


            parent.EntityId = param.EntityId;
            parent.EntityClassName = param.EntityClassName;
            parent.AttachmentEntityClassName = KernelContext.ParseObjectType(typeof(AttachmentFileInfo));
            parent.AttachmentFolder = UploadPathHelper.GetAttachmentFolder(param.VirtualPath);

            param.Parent = parent;

            return param;
        }
        #endregion

        #region 属性:FindAll()
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        public IList<IAttachmentFileInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        public IList<IAttachmentFileInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        public IList<IAttachmentFileInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByEntityId(string entityClassName, string entityId)
        ///<summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="entityId">ʵ������ʶ</param>
        ///<returns>�������� IAttachmentFileInfo ʵ������ϸ��Ϣ</returns>
        public IList<IAttachmentFileInfo> FindAllByEntityId(string entityClassName, string entityId)
        {
            return provider.FindAllByEntityId(entityClassName, entityId);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        public IList<IAttachmentFileInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
        ///<param name="id">��Ա��ʶ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 属性:Rename(string id, string name)
        ///<summary>������</summary>
        ///<param name="id">������ʶ</param>
        ///<param name="name">�µĸ�������</param>
        public void Rename(string id, string name)
        {
            provider.Rename(id, name);
        }
        #endregion

        #region 属性:Copy(IAttachmentFileInfo param, string entityId, string entityClassName)
        ///<summary>������ȫ��������Ϣ��ʵ����</summary>
        ///<param name="param">IAttachmentFileInfo ʵ����ϸ��Ϣ</param>
        ///<param name="entityId">ʵ����ʶ</param>
        ///<param name="entityClassName">ʵ��������</param>
        ///<returns>�µ� IAttachmentFileInfo ʵ����ϸ��Ϣ</returns>
        public IAttachmentFileInfo Copy(IAttachmentFileInfo param, string entityId, string entityClassName)
        {
            IAttachmentParentObject parent = new AttachmentParentObject();

            parent.EntityId = entityId;
            parent.EntityClassName = entityClassName;
            parent.AttachmentEntityClassName = KernelContext.ParseObjectType(typeof(AttachmentFileInfo));
            parent.AttachmentFolder = UploadPathHelper.GetAttachmentFolder(param.VirtualPath);

            IAttachmentFileInfo attachment = UploadFileHelper.CreateAttachmentFile(parent, param.AttachmentName, param.FileType, param.FileSize, param.FileData);

            attachment.Save();

            return attachment;
        }
        #endregion
    }
}
