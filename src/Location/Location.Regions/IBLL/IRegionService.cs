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

namespace X3Platform.Location.Regions.IBLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Location.Regions.Model;
    using X3Platform.Data;
    using X3Platform.CategoryIndexes;
    #endregion

    /// <summary></summary>
    [SpringObject("X3Platform.Location.Regions.IBLL.IRegionService")]
    public interface IRegionService
    {
        #region ����:this[string id]
        /// <summary>����</summary>
        /// <param name="id">��ʶ</param>
        /// <returns></returns>
        RegionInfo this[string id] { get; }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region ����:Save(RegionInfo param)
        /// <summary>�����¼</summary>
        /// <param name="param">ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</returns>
        RegionInfo Save(RegionInfo param);
        #endregion

        #region ����:Delete(string id)
        /// <summary>ɾ����¼</summary>
        /// <param name="id">��ʶ</param>
        void Delete(string id);
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����һ�� ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</returns>
        RegionInfo FindOne(string id);
        #endregion

        #region ����:FindAll()
        /// <summary>��ѯ������ؼ�¼</summary>
        /// <returns>�������� ʵ��<see cref="RegionInfo"/>����ϸ��Ϣ</returns>
        IList<RegionInfo> FindAll();
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
        /// <param name="id">ʵ��<see cref="RegionInfo"/>��ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region ����:GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick)
        /// <summary>��ȡ�첽���ɵ���</summary>
        /// <param name="treeName">��</param>
        /// <param name="parentId">�����ڵ��ʶ</param>
        /// <param name="url">���ӵ�ַ</param>
        /// <param name="enabledLeafClick">ֻ������Ҷ�ӽڵ�</param>
        /// <returns>��</returns>
        DynamicTreeView GetDynamicTreeView(string treeName, string parentId, string url, bool enabledLeafClick);
        #endregion
    }
}
