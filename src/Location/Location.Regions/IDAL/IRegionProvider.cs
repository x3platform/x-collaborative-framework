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

namespace X3Platform.Location.Regions.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Location.Regions.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [SpringObject("X3Platform.Location.Regions.IDAL.IRegionProvider")]
    public interface IRegionProvider
    {
        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">RegionInfo Id��</param>
        /// <returns>����һ�� ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</returns>
        RegionInfo FindOne(string id);
        #endregion

        #region ����:FindAll(DataQuery query)
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <returns>�������� ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</returns>
        IList<RegionInfo> FindAll(DataQuery query);
        #endregion

        // -------------------------------------------------------
        // ���� ��� �޸� ɾ��
        // -------------------------------------------------------

        #region ����:Save(RegionInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</returns>
        RegionInfo Save(RegionInfo param);
        #endregion

        #region ����:Insert(RegionInfo param)
        /// <summary>��Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</param>
        void Insert(RegionInfo param);
        #endregion

        #region ����:Update(RegionInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</param>
        void Update(RegionInfo param);
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region ����:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ���С</param>
        /// <param name="query">���ݲ�ѯ����</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns> 
        IList<RegionInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount);
        #endregion

        #region ����:IsExist(string id)
        /// <summary>��ѯ�Ƿ������صļ�¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion
    }
}
