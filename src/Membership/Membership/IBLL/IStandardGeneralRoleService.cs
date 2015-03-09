// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :INonstandardRoleService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.IBLL
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using X3Platform.Spring;

    using X3Platform.Membership.Model;
    using System.Data;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IStandardGeneralRoleService")]
    public interface IStandardGeneralRoleService
    {
        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IStandardGeneralRoleInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IStandardGeneralRoleInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</returns>
        IStandardGeneralRoleInfo Save(IStandardGeneralRoleInfo param);
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
        /// <returns>����ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        IStandardGeneralRoleInfo FindOne(string id);
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        IList<IStandardGeneralRoleInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        IList<IStandardGeneralRoleInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        IList<IStandardGeneralRoleInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="groupTreeNodeId">�����ڵ���ʶ</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        IList<IStandardGeneralRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 属性:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表<see cref="IStandardGeneralRoleInfo"/></returns>
        IList<IStandardGeneralRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="name">��׼ͨ�ý�ɫ����</param>
        /// <returns>����ֵ</returns>
        bool IsExistName(string name);
        #endregion

        #region 属性:GetMappingTable(string standardGeneralRoleId, string organizationId)
        /// <summary>����������֯�µĽ�ɫ�ͱ�׼ͨ�ý�ɫ��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��ʼʱ��</param>
        /// <param name="organizationId">�����ı�׼</param>
        DataTable GetMappingTable(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 属性:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>�������ݰ�</summary>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        string CreatePackage(DateTime beginDate, DateTime endDate);
        #endregion

        // -------------------------------------------------------
        // ���ñ�׼ͨ�ý�ɫ����֯ӳ����ϵ
        // -------------------------------------------------------

        #region 属性:FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>���ұ�׼ͨ�ý�ɫ����֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        IStandardGeneralRoleMappingRelationInfo FindOneMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 属性:GetMappingRelationPaging(int startIndex, int pageSize,  DataQuery query, out int rowCount)
        /// <summary>标准通用角色映射关系分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IStandardGeneralRoleMappingRelationInfo"/></returns>
        IList<IStandardGeneralRoleMappingRelationInfo> GetMappingRelationPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 属性:AddMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>���ӱ�׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        int AddMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 属性:AddMapping(string standardGeneralRoleId, string organizationId, string roleId)
        /// <summary>���ӱ�׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="roleId">��ɫ��ʶ</param>
        int AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId);
        #endregion

        #region 属性:RemoveMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>�Ƴ���׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        int RemoveMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 属性:HasMapping(string standardGeneralRoleId, string organizationId)
        /// <summary>������׼ͨ�ý�ɫ��������֯�Ƿ���ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        bool HasMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion
    }
}
