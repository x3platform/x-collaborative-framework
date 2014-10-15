// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IJobService.cs
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

namespace X3Platform.Membership.IBLL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IBLL.IJobService")]
    public interface IJobService
    {
        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IJobInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IJobInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</returns>
        IJobInfo Save(IJobInfo param);
        #endregion

        #region 属性:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        IJobInfo FindOne(string id);
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        IList<IJobInfo> FindAll();
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        IList<IJobInfo> FindAll(string whereClause);
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IJobInfo"/>����ϸ��Ϣ</returns>
        IList<IJobInfo> FindAll(string whereClause, int length);
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
        /// <returns>����һ���б�ʵ��<see cref="IJobInfo"/></returns>
        IList<IJobInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="name">ְλ����</param>
        /// <returns>����ֵ</returns>
        bool IsExistName(string name);
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">ְλ��ʶ</param>
        /// <param name="name">ְλ����</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        int Rename(string id, string name);
        #endregion

        #region 属性:SyncFromPackPage(IJobInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">ְλ��Ϣ</param>
        int SyncFromPackPage(IJobInfo param);
        #endregion

        #region 属性:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>�������ݰ�</summary>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        string CreatePackage(DateTime beginDate, DateTime endDate);
        #endregion
    }
}
