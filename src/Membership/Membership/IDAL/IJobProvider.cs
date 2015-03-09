// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IJobProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;

using X3Platform.Spring;

using X3Platform.Membership.Model;
using X3Platform.Data;

namespace X3Platform.Membership.IDAL
{
    /// <summary></summary>
    [SpringObject("X3Platform.Membership.IDAL.IJobProvider")]
    public interface IJobProvider
    {
        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IJobInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</returns>
        IJobInfo Save(IJobInfo param);
        #endregion

        #region 属性:Insert(IJobInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</param>
        void Insert(IJobInfo param);
        #endregion

        #region 属性:Update(IJobInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="IJobInfo"/>��ϸ��Ϣ</param>
        void Update(IJobInfo param);
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

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IJobInfo"/></returns>
        IList<IJobInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
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
        /// <param name="param">��λ��Ϣ</param>
        int SyncFromPackPage(IJobInfo param);
        #endregion
    }
}
