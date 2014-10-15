// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityLifeHistoryService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform;
using X3Platform.DigitalNumber;
using X3Platform.Membership;
using X3Platform.Spring;

using X3Platform.Entities.Configuration;
using X3Platform.Entities.IBLL;
using X3Platform.Entities.IDAL;
using X3Platform.Entities.Model;

namespace X3Platform.Entities.BLL
{
    /// <summary></summary>
    public class EntityLifeHistoryService : IEntityLifeHistoryService
    {
        /// <summary>����</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IEntityLifeHistoryProvider provider = null;

        #region ���캯��:EntityLifeHistoryService()
        /// <summary>���캯��</summary>
        public EntityLifeHistoryService()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IEntityLifeHistoryProvider>(typeof(IEntityLifeHistoryProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityLifeHistoryInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(EntityLifeHistoryInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntityLifeHistoryInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntityLifeHistoryInfo"/>��ϸ��Ϣ</returns>
        public EntityLifeHistoryInfo Save(EntityLifeHistoryInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        ///<summary>ɾ����¼</summary>
        ///<param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>����ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        public EntityLifeHistoryInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<returns>��������ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityLifeHistoryInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<returns>��������ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityLifeHistoryInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>��������ʵ��<see cref="EntityLifeHistoryInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityLifeHistoryInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
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
        /// <returns>����һ���б�ʵ��<see cref="EntityLifeHistoryInfo"/></returns>
        public IList<EntityLifeHistoryInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼.</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 属性:Log(string methodName, EntityClass entity, string contextDiffLog)
        /// <summary>������־��Ϣ</summary>
        /// <param name="methodName">��������</param>
        /// <param name="entity">ʵ����</param>
        /// <param name="contextDiffLog">�����Ĳ�����¼</param>
        /// <returns>0 �����ɹ� | 1 ����ʧ��</returns>
        public int Log(string methodName, EntityClass entity, string contextDiffLog)
        {
            return this.Log(methodName, entity.EntityId, entity.EntityClassName, contextDiffLog);
        }
        #endregion

        #region 属性:Log(string methodName, string entityId, string entityClassName, string contextDiffLog)
        /// <summary>������־��Ϣ</summary>
        /// <param name="methodName">��������</param>
        /// <param name="entityId">ʵ����ʶ</param>
        /// <param name="entityClassName">ʵ��������</param>
        /// <param name="contextDiffLog">�����Ĳ�����¼</param>
        /// <returns>0 �����ɹ� | 1 ����ʧ��</returns>
        public int Log(string methodName, string entityId, string entityClassName, string contextDiffLog)
        {
            IAccountInfo account = KernelContext.Current.User;

            // ����ʵ�����ݲ�����¼
            EntityLifeHistoryInfo param = new EntityLifeHistoryInfo();

            param.Id = DigitalNumberContext.Generate("Key_Guid");
            param.AccountId = account == null ? Guid.Empty.ToString() : account.Id;
            param.MethodName = methodName;
            param.EntityId = entityId;
            param.EntityClassName = entityClassName;
            param.ContextDiffLog = contextDiffLog;

            this.Save(param);

            return 0;
        }
        #endregion
    }
}
