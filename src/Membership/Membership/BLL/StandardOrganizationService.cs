// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardOrganizationService.cs
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

using X3Platform.Membership.Configuration;
using X3Platform.Membership.IBLL;
using X3Platform.Membership.IDAL;
using X3Platform.Membership.Model;
using X3Platform.Configuration;
using X3Platform.Data;

namespace X3Platform.Membership.BLL
{
    /// <summary></summary>
    public class StandardOrganizationService : IStandardOrganizationService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IStandardOrganizationProvider provider = null;

        #region ���캯��:StandardOrganizationService()
        /// <summary>���캯��</summary>
        public StandardOrganizationService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IStandardOrganizationProvider>(typeof(IStandardOrganizationProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStandardOrganizationInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IStandardOrganizationInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardOrganizationInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IStandardOrganizationInfo"/>��ϸ��Ϣ</returns>
        public IStandardOrganizationInfo Save(IStandardOrganizationInfo param)
        {
            return provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">ʵ���ı�ʶ,������¼�Զ��ŷֿ�</param>
        public void Delete(string ids)
        {
            provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IStandardOrganizationInfo"/>����ϸ��Ϣ</returns>
        public IStandardOrganizationInfo FindOne(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IStandardOrganizationInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardOrganizationInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IStandardOrganizationInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardOrganizationInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IStandardOrganizationInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardOrganizationInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByParentId(string parentId)
        /// <summary>��ѯĳ�򸸽ڵ��µ�������֯��λ</summary>
        /// <param name="parentId">���ڱ�ʶ</param>
        /// <returns>����һ�� IOrganizationInfo ʵ������ϸ��Ϣ</returns>
        public IList<IStandardOrganizationInfo> FindAllByParentId(string parentId)
        {
            return provider.FindAllByParentId(parentId);
        }
        #endregion

        // -------------------------------------------------------
        // �Զ��幦��
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表<see cref="IOrganizationInfo"/></returns>
        public IList<IStandardOrganizationInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="name">��׼��֯����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 属性:IsExistGlobalName(string globalName)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="globalName">��׼��֯��λȫ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGlobalName(string globalName)
        {
            return provider.IsExistGlobalName(globalName);
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��׼��֯��ʶ</param>
        /// <param name="name">��׼��֯����</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        public int Rename(string id, string name)
        {
            return provider.Rename(id, name);
        }
        #endregion

        #region 属性:SetGlobalName(string id, string globalName)
        /// <summary>����ȫ������</summary>
        /// <param name="id">�ʻ���ʶ</param>
        /// <param name="globalName">ȫ������</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetGlobalName(string id, string globalName)
        {
            if (string.IsNullOrEmpty(globalName))
            {
                // ������${Id}��ȫ�����Ʋ���Ϊ�ա�
                return 1;
            }

            if (IsExistGlobalName(globalName))
            {
                return 2;
            }

            // �����Ƿ����ڶ���
            if (!IsExist(id))
            {
                // ������${Id}�������ڡ�
                return 3;
            }

            return provider.SetGlobalName(id, globalName);
        }
        #endregion

        #region 属性:SetParentId(string id, string parentId)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��֯��ʶ</param>
        /// <param name="parentId">������֯��ʶ</param>
        /// <returns>�޸ĳɹ�, ���� 0, �޸�ʧ��, ���� 1.</returns>
        public int SetParentId(string id, string parentId)
        {
            // �����Ƿ����ڶ���
            if (!IsExist(id))
            {
                // ������${Id}�������ڡ�
                return 1;
            }

            // �����Ƿ����ڶ���
            if (!IsExist(parentId))
            {
                // ����������${ParentId}�������ڡ�
                return 2;
            }

            return provider.SetGlobalName(id, parentId);
        }
        #endregion

        #region 属性:SyncFromPackPage(IStandardOrganizationInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��λ��Ϣ</param>
        public int SyncFromPackPage(IStandardOrganizationInfo param)
        {
            provider.SyncFromPackPage(param);

            return 0;
        }
        #endregion
    }
}
