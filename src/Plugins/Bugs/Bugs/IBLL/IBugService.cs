#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Plugins.Bugs.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership.Scope;
    using X3Platform.Spring;

    using X3Platform.Plugins.Bugs.Model;
  using X3Platform.Data;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Plugins.Bugs.IBLL.IBugService")]
    public interface IBugService
    {
        #region ����:this[string index]
        /// <summary>����</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        BugInfo this[string index] { get; }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">BugInfo Id��</param>
        /// <returns>����һ�� BugInfo ʵ������ϸ��Ϣ</returns>
        BugInfo FindOne(string id);
        #endregion

        #region ����:FindOneByCode(string code)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="code">������</param>
        /// <returns>����һ�� BugInfo ʵ������ϸ��Ϣ</returns>
        BugInfo FindOneByCode(string code);
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <returns>�������� BugInfo ʵ������ϸ��Ϣ</returns>
        IList<BugInfo> FindAll();
        #endregion

        #region ����:FindAll(string whereClause)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� BugInfo ʵ������ϸ��Ϣ</returns>
        IList<BugInfo> FindAll(string whereClause);
        #endregion

        #region ����:FindAll(string whereClause,int length)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� BugInfo ʵ������ϸ��Ϣ</returns>
        IList<BugInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(BugInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">BugInfo ʵ����ϸ��Ϣ</param>
        /// <returns>BugInfo ʵ����ϸ��Ϣ</returns>
        BugInfo Save(BugInfo param);
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        int Delete(string id);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        IList<BugInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="CostInfo"/></returns>
        IList<BugQueryInfo> GetQueryObjectPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="id">BugInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region ����:GetAuthorizationScopeObjects(string entityId, string authorityName)
        /// <summary>��ѯʵ������Ȩ����Ϣ</summary> 
        /// <param name="entityId">ʵ���ʶ</param>
        /// <param name="authorityName">Ȩ������</param>
        /// <returns></returns>
        IList<MembershipAuthorizationScopeObject> GetAuthorizationScopeObjects(string entityId, string authorityName);
        #endregion
    }
}
