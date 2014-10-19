namespace X3Platform.AttachmentStorage.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Spring;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.AttachmentStorage.IBLL.IAttachmentStorageService")]
    public interface IAttachmentStorageService
    {
        #region ����:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IAttachmentFileInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(IAttachmentFileInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</returns>
        IAttachmentFileInfo Save(IAttachmentFileInfo param);
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        void Delete(string id);
        #endregion
        
        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">IAttachmentFileInfo Id��</param>
        /// <returns>����һ�� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        IAttachmentFileInfo FindOne(string id);
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        IList<IAttachmentFileInfo> FindAll();
        #endregion

        #region ����:FindAll(string whereClause,int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        IList<IAttachmentFileInfo> FindAll(DataQuery query);
        #endregion

        #region ����:FindAllByEntityId(string entityClassName, string entityId)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="entityId">ʵ�����ʶ</param>
        /// <returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        IList<IAttachmentFileInfo> FindAllByEntityId(string entityClassName, string entityId);
        #endregion
   
        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        IList<IAttachmentFileInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region ����:Rename(string id, string name)
        /// <summary>������</summary>
        /// <param name="id">������ʶ</param>
        /// <param name="name">�µĸ�������</param>
        void Rename(string id, string name);
        #endregion

        #region ����:Copy(IAttachmentFileInfo param, string entityId, string entityClassName)
        /// <summary>������ȫ��������Ϣ��ʵ����</summary>
        /// <param name="param">ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</param>
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>�µ� ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</returns>
        IAttachmentFileInfo Copy(IAttachmentFileInfo param, string entityId, string entityClassName);
        #endregion
    }
}
