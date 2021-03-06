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
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.IDAL
{
    using System;
    using System.Collections.Generic;
    using X3Platform.Data;
    using X3Platform.Spring;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IMemberProvider")]
    public interface IMemberProvider
    {
        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IAccount param)
        /// <summary>������¼</summary>
        /// <param name="param">IAccount ʵ����ϸ��Ϣ</param>
        /// <returns>IAccount ʵ����ϸ��Ϣ</returns>
        IMemberInfo Save(IMemberInfo param);
        #endregion

        #region 属性:Insert(IAccount param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">IAccount ʵ������ϸ��Ϣ</param>
        void Insert(IMemberInfo param);
        #endregion

        #region 属性:Update(IAccount param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">IAccount ʵ������ϸ��Ϣ</param>
        void Update(IMemberInfo param);
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">IAccount ʵ���� Id ��Ϣ</param>
        void Delete(string ids);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">IAccount id��</param>
        /// <returns>����һ�� IAccount ʵ������ϸ��Ϣ</returns>
        IMemberInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByAccountId(string accountId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����һ�� MemberInfo ʵ������ϸ��Ϣ</returns>
        IMemberInfo FindOneByAccountId(string accountId);
        #endregion

        #region 属性:FindOneByLoginName(string loginName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="loginName">��¼��</param>
        /// <returns>����һ�� IAccount ʵ������ϸ��Ϣ</returns>
        IMemberInfo FindOneByLoginName(string loginName);
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        IList<IMemberInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllWithoutDefaultOrganizationUnit(int length)
        /// <summary>��������û��Ĭ����֯�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        IList<IMemberInfo> FindAllWithoutDefaultOrganizationUnit(int length);
        #endregion

        #region 属性:FindAllWithoutDefaultJob(int length)
        /// <summary>��������û��ְλ��Ϣ�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        IList<IMemberInfo> FindAllWithoutDefaultJob(int length);
        #endregion

        #region 属性:FindAllWithoutDefaultAssignedJob(int length)
        /// <summary>��������û��Ĭ�ϸ�λ�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        IList<IMemberInfo> FindAllWithoutDefaultAssignedJob(int length);
        #endregion

        #region 属性:FindAllWithoutDefaultRole(int length)
        /// <summary>��������û��Ĭ�Ͻ�ɫ�ĳ�Ա��Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IMemberInfo" />ʵ������ϸ��Ϣ</returns>
        IList<IMemberInfo> FindAllWithoutDefaultRole(int length);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表实例<see cref="IMemberInfo"/></returns>
        IList<IMemberInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>�����Ƿ��������صļ�¼.</summary>
        /// <param name="id">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:SetContactCard(string accountId, Dictionary<string,string> contactItems);
        /// <summary>������ϵ����Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="contactItems">��ϵ���ֵ�</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        int SetContactCard(string accountId, Dictionary<string, string> contactItems);
        #endregion

        #region 属性:SetDefaultCorporationAndDepartments(string accountId, string organizationIds)
        /// <summary>����Ĭ����֯��λ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationIds">��֯��λ��ʶ��[0]��˾��ʶ��[1]һ�����ű�ʶ��[2]�������ű�ʶ��[3]�������ű�ʶ��</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        int SetDefaultCorporationAndDepartments(string accountId, string organizationIds);
        #endregion

        #region 属性:SetDefaultOrganizationUnit(string accountId, string organizationId)
        /// <summary>����Ĭ����֯��λ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="organizationId">��֯��λ��ʶ</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        int SetDefaultOrganizationUnit(string accountId, string organizationId);
        #endregion

        #region 属性:SetDefaultRole(string accountId, string roleId)
        /// <summary>����Ĭ�Ͻ�ɫ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="roleId">��ɫ��ʶ</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetDefaultRole(string accountId, string roleId);
        #endregion

        #region 属性:SetDefaultJob(string accountId, string jobId)
        /// <summary>����Ĭ��ְλ��Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="jobId">ְλ��Ϣ</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        int SetDefaultJob(string accountId, string jobId);
        #endregion

        #region 属性:SetDefaultAssignedJob(string accountId, string assignedJobId)
        /// <summary>����Ĭ�ϸ�λ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="assignedJobId">��λ��ʶ</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetDefaultAssignedJob(string accountId, string assignedJobId);
        #endregion

        #region 属性:SetDefaultJobGrade(string accountId, string jobGradeId)
        /// <summary>����Ĭ��ְ����Ϣ</summary>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <param name="jobGradeId">ְ����ʶ</param>
        /// <returns>�޸ĳɹ�,���� 0, �޸�ʧ��,���� 1.</returns>
        int SetDefaultJobGrade(string accountId, string jobGradeId);
        #endregion

        #region 属性:SyncFromPackPage(IMemberInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��Ա��Ϣ</param>
        int SyncFromPackPage(IMemberInfo param);
        #endregion
    }
}
