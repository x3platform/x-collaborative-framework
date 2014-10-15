#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityLifeHistoryService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IBLL.IEntityLifeHistoryService")]
    public interface IEntityLifeHistoryService
    {
        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(EntityLifeHistoryInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntityLifeHistoryInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntityLifeHistoryInfo"/>��ϸ��Ϣ</returns>
        EntityLifeHistoryInfo Save(EntityLifeHistoryInfo param);
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>����ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        EntityLifeHistoryInfo FindOne(string id);
        #endregion

        #region 属性:FindAll()
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<returns>��������ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        IList<EntityLifeHistoryInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>��������ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        IList<EntityLifeHistoryInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>��������ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        IList<EntityLifeHistoryInfo> FindAll(string whereClause, int length);
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
        /// <returns>����һ���б�ʵ��<see cref="EntityLifeHistoryInfo"/></returns>
        IList<EntityLifeHistoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼.</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:Log(string methodName, EntityClass entity, string contextDiffLog)
        /// <summary>������־��Ϣ</summary>
        /// <param name="methodName">��������</param>
        /// <param name="entity">ʵ����</param>
        /// <param name="contextDiffLog">�����Ĳ�����¼</param>
        /// <returns>0 �����ɹ� | 1 ����ʧ��</returns>
        int Log(string methodName, EntityClass entity, string contextDiffLog);
        #endregion

        #region 属性:Log(string methodName, string entityId, string entityClassName, string contextDiffLog)
        /// <summary>������־��Ϣ</summary>
        /// <param name="methodName">��������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="contextDiffLog">�����Ĳ�����¼</param>
        /// <returns>0 �����ɹ� | 1 ����ʧ��</returns>
        int Log(string methodName, string entityId, string entityClassName, string contextDiffLog);
        #endregion
    }
}
