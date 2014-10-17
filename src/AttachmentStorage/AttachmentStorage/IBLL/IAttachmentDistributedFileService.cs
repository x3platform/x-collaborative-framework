// =============================================================================
//
// Copyright (c) 2007 X3Platform
//
// FileName     :
//
// Description  :
//
// Author       :X3Platform
//
// Date         :2007-07-08
//
// =============================================================================

namespace X3Platform.AttachmentStorage.IBLL
{
    using System.Collections.Generic;

    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.AttachmentStorage.IBLL.IAttachmentDistributedFileService")]
    public interface IAttachmentDistributedFileService
    {
        #region ����:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DistributedFileInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(DistributedFileInfo param)
        ///<summary>�����¼</summary>
        ///<param name="param">DistributedFileInfo ʵ����ϸ��Ϣ</param>
        ///<returns>DistributedFileInfo ʵ����ϸ��Ϣ</returns>
        DistributedFileInfo Save(DistributedFileInfo param);
        #endregion

        #region ����:Delete(string id)
        ///<summary>ɾ����¼</summary>
        ///<param name="id">��ʶ</param>
        void Delete(string id);
        #endregion
        
        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">DistributedFileInfo Id��</param>
        ///<returns>����һ�� DistributedFileInfo ʵ������ϸ��Ϣ</returns>
        DistributedFileInfo FindOne(string id);
        #endregion

        #region ����:FindAll()
        ///<summary>��ѯ������ؼ�¼</summary>
        ///<returns>�������� DistributedFileInfo ʵ������ϸ��Ϣ</returns>
        IList<DistributedFileInfo> FindAll();
        #endregion

        #region ����:FindAll(string whereClause)
        ///<summary>��ѯ������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>�������� DistributedFileInfo ʵ������ϸ��Ϣ</returns>
        IList<DistributedFileInfo> FindAll(string whereClause);
        #endregion

        #region ����:FindAll(string whereClause,int length)
        ///<summary>��ѯ������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>�������� DistributedFileInfo ʵ������ϸ��Ϣ</returns>
        IList<DistributedFileInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        IList<DistributedFileInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        ///<summary>��ѯ�Ƿ������صļ�¼</summary>
        ///<param name="id">DistributedFileInfo ʵ����ϸ��Ϣ</param>
        ///<returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
