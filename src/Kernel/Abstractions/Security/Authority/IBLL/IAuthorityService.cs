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
using System.Collections.Generic;

using X3Platform.Data;
using X3Platform.Spring;
#endregion

namespace X3Platform.Security.Authority.IBLL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Security.Authority.IBLL.IAuthorityService")]
    public interface IAuthorityService
    {
        #region ����:this[string name]
        /// <summary>����</summary>
        /// <param name="name">Ȩ������</param>
        /// <returns>ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</returns>
        AuthorityInfo this[string name] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(AuthorityInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="AuthorityInfo"/>��ϸ��Ϣ</returns>
        AuthorityInfo Save(AuthorityInfo param);
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        /// <returns>����һ��ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        AuthorityInfo FindOne(string id);
        #endregion

        #region ����:FindOneByName(string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="name">Ȩ������</param>
        /// <returns>����һ��ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        AuthorityInfo FindOneByName(string name);
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        IList<AuthorityInfo> FindAll();
        #endregion

        #region ����:FindAll(DataQuery query)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <returns>��������ʵ��<see cref="AuthorityInfo"/>����ϸ��Ϣ</returns>
        IList<AuthorityInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="AuthorityInfo"/></returns> 
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
