#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IAttachmentStorageProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.AttachmentStorage.IDAL
{
    using System.Collections.Generic;

    using X3Platform.Spring;

    /// <summary>�����洢</summary>
    [SpringObject("X3Platform.AttachmentStorage.IDAL.IAttachmentStorageProvider")]     
    public interface IAttachmentStorageProvider
    {
        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IAttachmentFileInfo param)
        ///<summary>������¼</summary>
        ///<param name="param">ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</param>
        ///<returns>ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</returns>
        IAttachmentFileInfo Save(IAttachmentFileInfo param);
        #endregion

        #region 属性:Delete(string id)
        ///<summary>ɾ����¼</summary>
        ///<param name="id">��ʶ</param>
        void Delete(string id);
        #endregion
        
		// -------------------------------------------------------
		// ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">IAttachmentFileInfo Id��</param>
		///<returns>����һ�� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        IAttachmentFileInfo FindOne(string id);
		#endregion

		#region 属性:FindAll(string whereClause,int length)
		///<summary>��ѯ�������ؼ�¼</summary>
		///<param name="whereClause">SQL ��ѯ����</param>
		///<param name="length">����</param>
		///<returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        IList<IAttachmentFileInfo> FindAll(string whereClause, int length);
		#endregion

        #region 属性:FindAllByEntityId(string entityClassName, string entityId)
        ///<summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="entityId">ʵ������ʶ</param>
        ///<returns>�������� ʵ��<see cref="IAttachmentFileInfo"/>����ϸ��Ϣ</returns>
        IList<IAttachmentFileInfo> FindAllByEntityId(string entityClassName, string entityId);
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
        IList<IAttachmentFileInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼</summary>
		///<param name="param">ʵ��<see cref="IAttachmentFileInfo"/>��ϸ��Ϣ</param>
		///<returns>����ֵ</returns>
		bool IsExist(string id);
		#endregion

        #region 属性:Rename(string id, string name)
        ///<summary>������</summary>
        ///<param name="id">������ʶ</param>
        ///<param name="name">�µĸ�������</param>
        void Rename(string id, string name);
        #endregion
	}
}
