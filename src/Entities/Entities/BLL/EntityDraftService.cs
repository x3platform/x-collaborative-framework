#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntityDraftService.cs
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

    using X3Platform.Spring;

    using X3Platform.Entities.Configuration;
    using X3Platform.Entities.IBLL;
    using X3Platform.Entities.IDAL;
    using X3Platform.Entities.Model;
    #endregion

    /// <summary></summary>
    public class EntityDraftService : IEntityDraftService
    {
        /// <summary>����</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IEntityDraftProvider provider = null;

        #region ���캯��:EntityDraftService()
        /// <summary>���캯��</summary>
        public EntityDraftService()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IEntityDraftProvider>(typeof(IEntityDraftProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityDraftInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(EntityDraftInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntityDraftInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntityDraftInfo"/>��ϸ��Ϣ</returns>
        public EntityDraftInfo Save(EntityDraftInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="EntityDraftInfo"/>����ϸ��Ϣ</returns>
        public EntityDraftInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="EntityDraftInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityDraftInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="EntityDraftInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityDraftInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntityDraftInfo"/>����ϸ��Ϣ</returns>
        public IList<EntityDraftInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
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
        /// <returns>����һ���б�ʵ��<see cref="EntityDraftInfo"/></returns>
        public IList<EntityDraftInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion
    }
}
