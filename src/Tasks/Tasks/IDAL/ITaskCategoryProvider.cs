#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IDAL.ITaskCategoryProvider")]
    public interface ITaskCategoryProvider
    {
        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region 属性:BeginTransaction()
        /// <summary>��������</summary>
        void BeginTransaction();
        #endregion

        #region 属性:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        void BeginTransaction(IsolationLevel isolationLevel);
        #endregion

        #region 属性:CommitTransaction()
        /// <summary>�ύ����</summary>
        void CommitTransaction();
        #endregion

        #region 属性:RollBackTransaction()
        /// <summary>�ع�����</summary>
        void RollBackTransaction();
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(TaskCategoryInfo param)
        /// <summary>
        /// ������¼
        /// </summary>
        /// <param name="param">ʵ����ϸ��Ϣ</param>
        /// <returns></returns>
        TaskCategoryInfo Save(TaskCategoryInfo param);
        #endregion

        #region 属性:Insert(TaskCategoryInfo param)
        /// <summary>
        /// ���Ӽ�¼
        /// </summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        void Insert(TaskCategoryInfo param);
        #endregion

        #region 属性:Update(TaskCategoryInfo param)
        /// <summary>
        /// �޸ļ�¼
        /// </summary>
        /// <param name="param">ʵ������ϸ��Ϣ</param>
        void Update(TaskCategoryInfo param);
        #endregion

        #region 属性:CanDelete(string id)
        /// <summary>�ж����������Ƿ��ܹ���ɾ��</summary>
        /// <param name="id">����������ʶ</param>
        /// <returns></returns>
        bool CanDelete(string id);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">����������ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>
        /// ��ѯĳ����¼
        /// </summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ������ϸ��Ϣ</returns>
        TaskCategoryInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="categoryIndex">��������</param>
        /// <returns>����ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>
        /// ��ѯ�������ؼ�¼
        /// </summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns></returns>
        IList<TaskCategoryInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllQueryObject(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        IList<TaskCategoryInfo> FindAllQueryObject(string whereClause, int length);
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
        /// <returns></returns>
        IList<TaskCategoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:SetStatus(string id, int status)
        /// <summary>��������״̬(ͣ��/����)</summary>
        /// <param name="id">����������ʶ</param>
        /// <param name="status">1 ��ͣ�õ��������ã�0 �����õ�����ͣ��</param>
        /// <returns></returns>
        bool SetStatus(string id, int status);
        #endregion
    }
}
