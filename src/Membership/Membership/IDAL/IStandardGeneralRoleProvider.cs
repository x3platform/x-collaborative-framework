// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IStandardGeneralRoleProvider.cs
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

using X3Platform.Spring;

using X3Platform.Membership.Model;

namespace X3Platform.Membership.IDAL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IStandardGeneralRoleProvider")]
    public interface IStandardGeneralRoleProvider
    {
        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IStandardGeneralRoleInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</returns>
        IStandardGeneralRoleInfo Save(IStandardGeneralRoleInfo param);
        #endregion

        #region 属性:Insert(IStandardGeneralRoleInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        void Insert(IStandardGeneralRoleInfo param);
        #endregion

        #region 属性:Update(IStandardGeneralRoleInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        void Update(IStandardGeneralRoleInfo param);
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

        #region 属性:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="IStandardGeneralRoleInfo"/></returns>
        IList<IStandardGeneralRoleInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
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

        // -------------------------------------------------------
        // ���ñ�׼ͨ�ý�ɫ����֯ӳ����ϵ
        // -------------------------------------------------------

        #region 属性:FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>���ұ�׼ͨ�ý�ɫ����֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        IStandardGeneralRoleMappingRelationInfo FindOneMappingRelation(string standardGeneralRoleId, string organizationId);
        #endregion

        #region 属性:GetMappingRelationPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��׼ͨ�ý�ɫӳ����ϵ��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="IStandardGeneralRoleMappingRelationInfo"/></returns>
        IList<IStandardGeneralRoleMappingRelationInfo> GetMappingRelationPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId, string standardRoleId)
        /// <summary>���ӱ�׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="roleId">��֯��ʶ</param>
        /// <param name="standardRoleId">��֯��ʶ</param>
        int AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId, string standardRoleId);
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
