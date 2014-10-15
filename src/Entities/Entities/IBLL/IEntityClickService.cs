#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntityClickService.cs
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

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Entities.Model;
    using X3Platform.Entities;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Entities.IBLL.IEntityClickService")]
    public interface IEntityClickService
    {
        // -------------------------------------------------------
        // ����
        // -------------------------------------------------------

        #region 属性:Save(IEntityClickInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</returns>
        IEntityClickInfo Save(IEntityClickInfo param);
        #endregion

        #region 属性:Save(string customTableName, IEntityClickInfo param)
        /// <summary>������¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</returns>
        IEntityClickInfo Save(string customTableName, IEntityClickInfo param);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityClickInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAll(string customTableName, string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityClickInfo> FindAll(string customTableName, string whereClause, int length);
        #endregion

        #region 属性:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityClickInfo> FindAllByEntityId(string entityId, string entityClassName);
        #endregion

        #region 属性:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName);
        #endregion

        #region 属性:FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="mapper">���ݽ���ӳ����</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:IsExist(string entityId, string entityClassName, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string entityId, string entityClassName, string accountId);
        #endregion

        #region 属性:IsExist(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string customTableName, string entityId, string entityClassName, string accountId);
        #endregion

        #region 属性:Increment(string entityId, string entityClassName, string accountId)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        int Increment(string entityId, string entityClassName, string accountId);
        #endregion

        #region 属性:Increment(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        int Increment(string customTableName, string entityId, string entityClassName, string accountId);
        #endregion

        #region 属性:CalculateClickCount(string entityId, string entityClassName)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>������</returns>
        int CalculateClickCount(string entityId, string entityClassName);
        #endregion

        #region 属性:CalculateClickCount(string tableName, string entityId, string entityClassName)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>������</returns>
        int CalculateClickCount(string customTableName, string entityId, string entityClassName);
        #endregion
    }
}
