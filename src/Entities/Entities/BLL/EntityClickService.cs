#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityClickService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Entities.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    public class EntityClickService : IEntityClickService
    {
        /// <summary>����</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IEntityClickProvider provider = null;

        #region ���캯��:EntityClickService()
        /// <summary>���캯��</summary>
        public EntityClickService()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IEntityClickProvider>(typeof(IEntityClickProvider));
        }
        #endregion

        // -------------------------------------------------------
        // ����
        // -------------------------------------------------------

        #region 属性:Save(IEntityClickInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</returns>
        public IEntityClickInfo Save(IEntityClickInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Save(string customTableName, IEntityClickInfo param)
        /// <summary>������¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="param">ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IEntityClickInfo"/>��ϸ��Ϣ</returns>
        public IEntityClickInfo Save(string customTableName, IEntityClickInfo param)
        {
            return this.provider.Save(customTableName, param);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAll(string customTableName, string whereClause, int length)
        {
            return this.provider.FindAll(customTableName, whereClause, length);
        }
        #endregion

        #region 属性:FindAllByEntityId(string entityId, string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string entityId, string entityClassName)
        {
            return this.provider.FindAllByEntityId(entityId, entityClassName);
        }
        #endregion

        #region 属性:FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName)
        {
            return this.provider.FindAllByEntityId(customTableName, entityId, entityClassName);
        }
        #endregion

        #region 属性:FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="mapper">���ݽ���ӳ����</param>
        /// <returns>��������ʵ��<see cref="IEntityClickInfo"/>����ϸ��Ϣ</returns>
        public IList<IEntityClickInfo> FindAllByEntityId(string customTableName, string entityId, string entityClassName, DataResultMapper mapper)
         {
             return this.provider.FindAllByEntityId(customTableName, entityId, entityClassName, mapper);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:IsExist(string entityId, string entityClassName, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string entityId, string entityClassName, string accountId)
        {
            return this.provider.IsExist(entityId, entityClassName, accountId);
        }
        #endregion

        #region 属性:IsExist(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string customTableName, string entityId, string entityClassName, string accountId)
        {
            return this.provider.IsExist(customTableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 属性:Increment(string entityId, string entityClassName, string accountId)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public int Increment(string entityId, string entityClassName, string accountId)
        {
            return this.provider.Increment(entityId, entityClassName, accountId);
        }
        #endregion

        #region 属性:Increment(string tableName, string entityId, string entityClassName, string accountId)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public int Increment(string customTableName, string entityId, string entityClassName, string accountId)
        {
            return this.provider.Increment(customTableName, entityId, entityClassName, accountId);
        }
        #endregion

        #region 属性:CalculateClickCount(string entityId, string entityClassName)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>������</returns>
        public int CalculateClickCount(string entityId, string entityClassName)
        {
            return this.provider.CalculateClickCount(entityId, entityClassName);
        }
        #endregion

        #region 属性:CalculateClickCount(string tableName, string entityId, string entityClassName)
        /// <summary>����ʵ�����ݵĵ�����</summary>
        /// <param name="customTableName">�Զ������ݱ�����</param>
        /// <param name="entityId">ʵ������ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>������</returns>
        public int CalculateClickCount(string customTableName, string entityId, string entityClassName)
        {
            return this.provider.CalculateClickCount(customTableName, entityId, entityClassName);
        }
        #endregion
    }
}
