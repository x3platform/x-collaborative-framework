#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IEntitySchemaService.cs
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
    [SpringObject("X3Platform.Entities.IBLL.IEntitySchemaService")]
    public interface IEntitySchemaService
    {
        #region 属性:this[string name]
        /// <summary>����</summary>
        /// <param name="name">����</param>
        /// <returns></returns>
        EntitySchemaInfo this[string name] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(EntitySchemaInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="EntitySchemaInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="EntitySchemaInfo"/>��ϸ��Ϣ</returns>
        EntitySchemaInfo Save(EntitySchemaInfo param);
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
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        EntitySchemaInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">����</param>
        /// <returns>����ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        EntitySchemaInfo FindOneByName(string name);
        #endregion

        #region 属性:FindOneByName(string entityClassName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="entityClassName">ʵ��������</param>
        /// <returns>����ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        EntitySchemaInfo FindOneByEntityClassName(string entityClassName);
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        IList<EntitySchemaInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        IList<EntitySchemaInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        IList<EntitySchemaInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllByIds(string ids)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        /// <returns>��������ʵ��<see cref="EntitySchemaInfo"/>����ϸ��Ϣ</returns>
        IList<EntitySchemaInfo> FindAllByIds(string ids);
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
        IList<EntitySchemaInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
