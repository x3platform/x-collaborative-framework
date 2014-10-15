#region Copyright & Author
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
#endregion

namespace X3Platform.Membership.IDAL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Membership.Model;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IAccountGrantProvider")]
    public interface IAccountGrantProvider
    {
        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IAccountGrantInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</returns>
        IAccountGrantInfo Save(IAccountGrantInfo param);
        #endregion

        #region 属性:Insert(IAccountGrantInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</param>
        void Insert(IAccountGrantInfo param);
        #endregion

        #region 属性:Update(IAccountGrantInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</param>
        void Update(IAccountGrantInfo param);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">ʵ���ı�ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        IAccountGrantInfo FindOne(string id);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        IList<IAccountGrantInfo> FindAll(string whereClause, int length);
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
        /// <returns>����һ���б�ʵ��<see cref="IAccountGrantInfo"/></returns>
        IList<IAccountGrantInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>��ѯ�Ƿ���������ί���˵ļ�¼.</summary>
        /// <param name="grantorId">ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <returns>����ֵ</returns>
        bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo);
        #endregion

        #region 属性:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>��ѯ�Ƿ���������ί���˵ļ�¼.</summary>
        /// <param name="grantorId">ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <param name="ignoreIds">����ί�б�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds);
        #endregion

        #region 属性:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>��ѯ�Ƿ��������ر�ί���˵ļ�¼.</summary>
        /// <param name="granteeId">��ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <returns>����ֵ</returns>
        bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo);
        #endregion

        #region 属性:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>��ѯ�Ƿ��������ر�ί���˵ļ�¼.</summary>
        /// <param name="granteeId">��ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <param name="ignoreIds">����ί�б�ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds);
        #endregion

        #region 属性:Abort(string id)
        /// <summary>��ֹ��ǰί��</summary>
        /// <param name="id">��ʶ</param>
        /// <returns></returns>
        int Abort(string id);
        #endregion

        // -------------------------------------------------------
        // ͬ������
        // -------------------------------------------------------

        #region 属性:SyncFromPackPage(IAccountGrantInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        int SyncFromPackPage(IAccountGrantInfo param);
        #endregion
    }
}
