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

namespace X3Platform.Tasks.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Tasks.IBLL.ITaskCategoryService")]
    public interface ITaskCategoryService
    {
        #region ����:this[string id]
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TaskCategoryInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(FavoriteInfo param)
        /// <summary>
        /// ������¼
        /// </summary>
        /// <param name="param">ʵ����ϸ��Ϣ</param>
        /// <returns></returns>
        TaskCategoryInfo Save(TaskCategoryInfo param);
        #endregion

        #region ����:CanDelete(string id)
        /// <summary>�ж����������Ƿ��ܹ���ɾ��</summary>
        /// <param name="id">����������ʶ</param>
        /// <returns></returns>
        bool CanDelete(string id);
        #endregion

        #region ����:Delete(string id)
        /// <summary>
        /// ɾ����¼
        /// </summary>
        /// <param name="id">����������ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>
        /// ��ѯĳ����¼
        /// </summary>
        /// <param name="id">��ʶ</param>
        /// <returns></returns>
        TaskCategoryInfo FindOne(string id);
        #endregion

        #region ����:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="categoryIndex">��������</param>
        /// <returns>����ʵ��<see cref="TaskCategoryInfo"/>����ϸ��Ϣ</returns>
        TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex);
        #endregion

        #region ����:FindAll()
        /// <summary>
        /// ��ѯ�������ؼ�¼
        /// </summary>
        /// <returns></returns>
        IList<TaskCategoryInfo> FindAll();
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>
        /// ��ѯ�������ؼ�¼
        /// </summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns></returns>
        IList<TaskCategoryInfo> FindAll(string whereClause);
        #endregion

        #region ����:FindAll(string whereClause, int length)
        /// <summary>
        /// ��ѯ�������ؼ�¼
        /// </summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns></returns>
        IList<TaskCategoryInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��.</param>
        /// <param name="pageSize">ÿҳ��ʾ����������</param>
        /// <param name="whereClause">WHERE ��ѯ����.</param>
        /// <param name="orderBy">ORDER BY ��������.</param>
        /// <param name="rowCount">��������������������</param>
        /// <returns>����һ���б�ʵ��<see cref="TaskCategoryInfo"/></returns>
        IList<TaskCategoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>
        /// ��ѯ�Ƿ��������صļ�¼
        /// </summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region ����:SetStatus(string id, int status)
        /// <summary>
        /// ͣ��/��������
        /// </summary>
        /// <param name="id">����������ʶ</param>
        /// <param name="status">1��ͣ�õ��������ã�0�����õ�����ͣ��</param>
        /// <returns></returns>
        bool SetStatus(string id, int status);
        #endregion
    }
}
