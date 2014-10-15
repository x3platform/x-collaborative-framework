// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IAccountProvider.cs
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
    using System.Data;

    using X3Platform;
    using X3Platform.Spring;
    using X3Platform.Membership.Scope;

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IAccountProvider")]
    public interface IAccountProvider
    {
        // -------------------------------------------------------
        // ���� ɾ�� ����
        // -------------------------------------------------------

        #region 属性:Save(IAccount param)
        /// <summary>������¼</summary>
        /// <param name="param">IAccount ʵ����ϸ��Ϣ</param>
        /// <returns>IAccount ʵ����ϸ��Ϣ</returns>
        IAccountInfo Save(IAccountInfo param);
        #endregion

        #region 属性:Insert(IAccount param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">IAccount ʵ������ϸ��Ϣ</param>
        void Insert(IAccountInfo param);
        #endregion

        #region 属性:Update(IAccount param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">IAccount ʵ������ϸ��Ϣ</param>
        void Update(IAccountInfo param);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">�ʺű�ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">IAccount id��</param>
        /// <returns>����һ�� IAccount ʵ������ϸ��Ϣ</returns>
        IAccountInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByGlobalName(string globalName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="globalName">�ʺŵ�ȫ������</param>
        /// <returns>����һ��<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        IAccountInfo FindOneByGlobalName(string globalName);
        #endregion

        #region 属性:FindOneByLoginName(string loginName)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="loginName">��¼��</param>
        /// <returns>����һ�� IAccount ʵ������ϸ��Ϣ</returns>
        IAccountInfo FindOneByLoginName(string loginName);
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindAll(string whereClause, int length);
        #endregion

        #region 属性:FindAllByOrganizationId(string organizationId)
        /// <summary>��ѯĳ���û����ڵ�������֯��λ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindAllByOrganizationId(string organizationId);
        #endregion

        #region 属性:FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation)
        /// <summary>��ѯĳ����֯�µ����������ʺ�</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="defaultOrganizationRelation">Ĭ����֯��ϵ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindAllByOrganizationId(string organizationId, bool defaultOrganizationRelation);
        #endregion

        #region 属性:FindAllByRoleId(string roleId)
        /// <summary>��ѯĳ����ɫ�µ����������ʺ�</summary>
        /// <param name="roleId">��֯��ʶ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindAllByRoleId(string roleId);
        #endregion

        #region 属性:FindAllByGroupId(string groupId)
        /// <summary>��ѯĳ��Ⱥ���µ����������ʺ�</summary>
        /// <param name="groupId">Ⱥ����ʶ</param>
        /// <returns>����һ�� IAccountInfo ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindAllByGroupId(string groupId);
        #endregion

        #region 属性:FindAllWithoutMemberInfo(int length)
        /// <summary>��������û�г�Ա��Ϣ���ʺ���Ϣ</summary>
        /// <param name="length">����, 0��ʾȫ��</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindAllWithoutMemberInfo(int length);
        #endregion

        #region 属性:FindForwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>�������������쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindForwardLeaderAccountsByOrganizationId(string organizationId, int level);
        #endregion

        #region 属性:FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level)
        /// <summary>�������з����쵼���ʺ���Ϣ</summary>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="level">����</param>
        /// <returns>��������<see cref="IAccountInfo"/>ʵ������ϸ��Ϣ</returns>
        IList<IAccountInfo> FindBackwardLeaderAccountsByOrganizationId(string organizationId, int level);
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
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns>
        IList<IAccountInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>�����Ƿ��������صļ�¼.</summary>
        /// <param name="id">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExistLoginNameAndGlobalName(string loginName, string name);
        /// <summary>�����Ƿ��������صļ�¼,��¼�����������߶������ظ�.</summary>
        /// <param name="loginName">��¼��</param>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        bool IsExistLoginNameAndGlobalName(string loginName, string name);
        #endregion

        #region 属性:IsExistLoginName(string loginName)
        /// <summary>�����Ƿ��������صļ�¼, �û�����, ��¼�������ظ�. [�����ʺ�]</summary>
        /// <param name="loginName">��¼��</param>
        /// <returns>����ֵ</returns>
        bool IsExistLoginName(string loginName);
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>�����Ƿ��������صļ�¼, ������ͬһ��OU����,�������������ظ�. �޸�����ʱ</summary>
        /// <param name="name">����</param>
        /// <returns>����ֵ</returns>
        bool IsExistName(string name);
        #endregion

        #region 属性:IsExistGlobalName(string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="globalName">��֯��λȫ������</param>
        /// <returns>����ֵ</returns>
        bool IsExistGlobalName(string globalName);
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">�ʺű�ʶ</param>
        /// <param name="name">�ʺ�����</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        int Rename(string id, string name);
        #endregion

        // -------------------------------------------------------
        // ����Ա����
        // -------------------------------------------------------

        #region 属性:SetGlobalName(string accountId, string globalName)
        /// <summary>����ȫ������</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="globalName">ȫ������</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetGlobalName(string accountId, string globalName);
        #endregion

        #region 属性:GetPassword(string loginName)
        /// <summary>��ȡ����(����Ա)</summary>
        /// <param name="loginName">�˺�</param>
        /// <returns>����</returns>
        string GetPassword(string loginName);
        #endregion

        #region 属性:SetPassword(string accountId, string password)
        /// <summary>�����ʺ�����(����Ա)</summary>
        /// <param name="accountId">����</param>
        /// <param name="password">����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �����벻ƥ��, ���� 1.</returns>
        int SetPassword(string accountId, string password);
        #endregion

        #region 属性:SetLoginName(string accountId, string loginName)
        /// <summary>���õ�¼��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="loginName">��¼��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetLoginName(string accountId, string loginName);
        #endregion

        #region 属性:SetCertifiedTelephone(string accountId, string telephone)
        /// <summary>��������֤����ϵ�绰</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="telephone">��ϵ�绰</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetCertifiedTelephone(string accountId, string telephone);
        #endregion

        #region 属性:SetCertifiedEmail(string accountId, string email)
        /// <summary>��������֤������</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="email">����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetCertifiedEmail(string accountId, string email);
        #endregion

        #region 属性:SetCertifiedAvatar(string accountId, string avatarVirtualPath)
        /// <summary>��������֤��ͷ��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="avatarVirtualPath">ͷ��������·��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetCertifiedAvatar(string accountId, string avatarVirtualPath);
        #endregion

        #region 属性:SetExchangeStatus(string accountId, int status)
        /// <summary>��������״̬</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetExchangeStatus(string accountId, int status);
        #endregion

        #region 属性:SetStatus(string accountId, int status)
        /// <summary>����״̬</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="status">״̬��ʶ, 1:����, 0:����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetStatus(string accountId, int status);
        #endregion

        #region 属性:SetIPAndLoginDate(string accountId, string ip, string loginDate)
        /// <summary>���õ�¼��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <param name="ip">��¼��</param>
        /// <param name="loginDate">��¼ʱ��</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        int SetIPAndLoginDate(string accountId, string ip, string loginDate);
        #endregion

        // -------------------------------------------------------
        // ��Ա����
        // -------------------------------------------------------

        #region 属性:ConfirmPassword(string accountId, string passwordType, string password)
        /// <summary>ȷ������</summary>
        /// <param name="accountId">�ʺ�Ψһ��ʶ</param>
        /// <param name="passwordType">����属性: default Ĭ��, query ��ѯ����, trader ��������</param>
        /// <param name="password">����</param>
        /// <returns>����ֵ: 0 �ɹ� | 1 ʧ��</returns>
        int ConfirmPassword(string accountId, string passwordType, string password);
        #endregion

        #region 属性:LoginCheck(string loginName, string password)
        /// <summary>��½����</summary>
        /// <param name="loginName">�ʺ�</param>
        /// <param name="password">����</param>
        /// <returns>IAccount ʵ��</returns>
        IAccountInfo LoginCheck(string loginName, string password);
        #endregion

        #region 属性:ChangeBasicInfo(IAccount param)
        /// <summary>�޸Ļ�����Ϣ</summary>
        /// <param name="param">IAccount ʵ������ϸ��Ϣ</param>
        void ChangeBasicInfo(IAccountInfo param);
        #endregion

        #region 属性:ChangePassword(string loginName, string password, string originalPassword)
        /// <summary>�޸�����</summary>
        /// <param name="loginName">��¼��</param>
        /// <param name="password">������</param>
        /// <param name="originalPassword">ԭʼ����</param>
        /// <returns>�޸ĳɹ�, ���� 0, �����벻ƥ��, ���� 1.</returns>
        int ChangePassword(string loginName, string password, string originalPassword);
        #endregion

        #region 属性:RefreshUpdateDate(string accountId)
        /// <summary>ˢ���ʺŵĸ���ʱ��</summary>
        /// <param name="accountId">�ʻ���ʶ</param>
        /// <returns>0 ���óɹ�, 1 ����ʧ��.</returns>
        int RefreshUpdateDate(string accountId);
        #endregion

        #region 属性:GetAuthorizationScopeObjects(IAccount account)
        /// <summary>��ȡ�ʺ����ص�Ȩ�޶���</summary>
        /// <param name="account">IAccount ʵ������ϸ��Ϣ</param>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(IAccountInfo account);
        #endregion

        #region 属性:SyncFromPackPage(IMemberInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        int SyncFromPackPage(IAccountInfo param);
        #endregion
    }
}
