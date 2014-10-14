#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
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
#endregion

#region Using Libraries
using System;
using System.Collections.Generic;

using X3Platform.Spring;
using X3Platform.Security.Authority.Configuration;
using X3Platform.Data;
#endregion

namespace X3Platform.Security.Authority.IDAL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Security.Authority.IDAL.IAuthorityProvider")]
    public interface IAuthorityProvider
    {
        //-------------------------------------------------------
        // ���� ���� �޸� ɾ��
        //-------------------------------------------------------

        #region ����:Save(AuthorityInfo param)
        /// <summary>������¼</summary>
        /// <param name="param"> ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</returns>
        AuthorityInfo Save(AuthorityInfo param);
        #endregion

        #region ����:Insert(AuthorityInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">AuthorityInfo ʵ������ϸ��Ϣ</param>
        void Insert(AuthorityInfo param);
        #endregion

        #region ����:Update(AuthorityInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">AuthorityInfo ʵ������ϸ��Ϣ</param>
        void Update(AuthorityInfo param);
        #endregion

        #region ����:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ��Ϣ,�����Զ��ŷֿ�.</param>
        void Delete(string ids);
        #endregion

        //-------------------------------------------------------
        // ��ѯ
        //-------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">AuthorityInfo id��</param>
        /// <returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        AuthorityInfo FindOne(string id);
        #endregion

        #region ����:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">Ȩ������</param>
        /// <returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        AuthorityInfo FindOneByName(string name);
        #endregion

        #region ����:FindAll(DataQuery query)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <returns>��������ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        IList<AuthorityInfo> FindAll(DataQuery query);
        #endregion

        //-------------------------------------------------------
        // �Զ��幦��
        //-------------------------------------------------------

        #region ����:Query(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">��¼����</param>
        /// <returns>����һ���б�</returns>
        IList<AuthorityInfo> Query(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
