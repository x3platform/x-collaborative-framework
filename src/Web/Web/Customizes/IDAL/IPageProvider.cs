// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
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

namespace X3Platform.Web.Customizes.IDAL
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;
    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customizes.Model;
    using X3Platform.Messages;

    /// <summary></summary>
    [SpringObject("X3Platform.Web.Customizes.IDAL.IPageProvider")]
    public interface IPageProvider
    {
        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">PageInfo Id��</param>
        /// <returns>����һ��ʵ��<see cref="PageInfo"/>����ϸ��Ϣ</returns>
        PageInfo FindOne(string id);
        #endregion

        #region 属性:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ��ʵ��<see cref="PageInfo"/>����ϸ��Ϣ</returns>
        PageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name);
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="PageInfo"/>����ϸ��Ϣ</returns>
        IList<PageInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(PageInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">PageInfo ʵ����ϸ��Ϣ</param>
        /// <returns>PageInfo ʵ����ϸ��Ϣ</returns>
        PageInfo Save(PageInfo param);
        #endregion

        #region 属性:Insert(PageInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">PageInfo ʵ������ϸ��Ϣ</param>
        void Insert(PageInfo param);
        #endregion

        #region 属性:Update(PageInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">PageInfo ʵ������ϸ��Ϣ</param>
        void Update(PageInfo param);
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���</param>
        void Delete(string ids);
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
        /// <returns>����һ���б�ʵ��</returns> 
        IList<PageInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="param">PageInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����ֵ</returns>
        bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name);
        #endregion

        #region 属性:TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ��ʵ��<see cref="PageInfo"/>����ϸ��Ϣ</returns>
        string TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name);
        #endregion
    }
}
