#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :EntitySchemaService.cs
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
    public class EntitySchemaService : IEntitySchemaService
    {
        /// <summary>����</summary>
        private EntitiesConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IEntitySchemaProvider provider = null;

        #region ���캯��:EntitySchemaService()
        /// <summary>���캯��</summary>
        public EntitySchemaService()
        {
            configuration = EntitiesConfigurationView.Instance.Configuration;

            provider = SpringContext.Instance.GetObject<IEntitySchemaProvider>(typeof(IEntitySchemaProvider));
        }
        #endregion

        #region 属性:this[string name]
        /// <summary>����</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EntitySchemaInfo this[string name]
        {
            get { return this.FindOneByName(name); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(EntitySchemaInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntitySchemaInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntitySchemaInfo"/>��ϸ��Ϣ</returns>
        public EntitySchemaInfo Save(EntitySchemaInfo param)
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
        /// <returns>����ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        public EntitySchemaInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">����</param>
        /// <returns>����ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        public EntitySchemaInfo FindOneByName(string name)
        {
            return this.provider.FindOneByName(name);
        }
        #endregion

        #region 属性:FindOneByEntityClassName(string entityClassName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>����ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        public EntitySchemaInfo FindOneByEntityClassName(string entityClassName)
        {
            return this.provider.FindOneByEntityClassName(entityClassName);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        public IList<EntitySchemaInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        public IList<EntitySchemaInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        public IList<EntitySchemaInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByIds(string ids)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        public IList<EntitySchemaInfo> FindAllByIds(string ids)
        {
            return this.provider.FindAllByIds(ids);
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
        /// <returns>����һ���б�ʵ��<see cref="EntitySchemaInfo"/></returns>
        public IList<EntitySchemaInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
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
