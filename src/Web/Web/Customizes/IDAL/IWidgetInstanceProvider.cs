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

    using X3Platform.Web.Customizes.Model;

    /// <summary></summary>
    [SpringObject("X3Platform.Web.Customizes.IDAL.IWidgetInstanceProvider")]
    public interface IWidgetInstanceProvider
    {
        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">WidgetInstanceInfo Id��</param>
        /// <returns>����һ��ʵ��<see cref="WidgetInstanceInfo"/>����ϸ��Ϣ</returns>
        WidgetInstanceInfo FindOne(string id);
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="WidgetInstanceInfo"/>����ϸ��Ϣ</returns>
        IList<WidgetInstanceInfo> FindAll(string whereClause, int length);
        #endregion

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(WidgetInstanceInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        /// <returns>WidgetInstanceInfo ʵ����ϸ��Ϣ</returns>
        WidgetInstanceInfo Save(WidgetInstanceInfo param);
        #endregion

        #region 属性:Insert(WidgetInstanceInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ������ϸ��Ϣ</param>
        void Insert(WidgetInstanceInfo param);
        #endregion

        #region 属性:Update(WidgetInstanceInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ������ϸ��Ϣ</param>
        void Update(WidgetInstanceInfo param);
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
        IList<WidgetInstanceInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount);
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="param">WidgetInstanceInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        bool IsExist(string id);
        #endregion

        #region 属性:RemoveUnbound(string pageId, string bindingWidgetInstanceIds)
        /// <summary>ɾ��δ�󶨵Ĳ���ʵ��</summary>
        /// <param name="pageId">ҳ������</param>
        /// <param name="bindingWidgetInstanceIds">�󶨵Ĳ�����ʶ</param>
        /// <returns>����ֵ</returns>
        int RemoveUnbound(string pageId, string bindingWidgetInstanceIds);
        #endregion
    }
}
