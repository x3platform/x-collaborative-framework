// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :StandardRoleService.cs
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
    public class StandardRoleService : IStandardRoleService
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>�����ṩ��</summary>
        private IStandardRoleProvider provider = null;

        #region ���캯��:StandardRoleService()
        /// <summary>���캯��</summary>
        public StandardRoleService()
        {
            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IStandardRoleProvider>(typeof(IStandardRoleProvider));
        }
        #endregion

        #region 属性:this[string id]
        /// <summary>����</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStandardRoleInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(IStandardRoleInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardRoleInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IStandardRoleInfo"/>��ϸ��Ϣ</returns>
        public IStandardRoleInfo Save(IStandardRoleInfo param)
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
        /// <returns>����ʵ��<see cref="IStandardRoleInfo"/>����ϸ��Ϣ</returns>
        public IStandardRoleInfo FindOne(string id)
        {
            return provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="IStandardRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardRoleInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="IStandardRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardRoleInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause, int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IStandardRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardRoleInfo> FindAll(string whereClause, int length)
        {
            return provider.FindAll(whereClause, length);
        }
        #endregion

        #region 属性:FindAllByParentId(string parentId)
        /// <summary>��ѯĳ�򸸽ڵ��µ�������֯��λ</summary>
        /// <param name="parentId">���ڱ�ʶ</param>
        /// <returns>����һ�� IOrganizationInfo ʵ������ϸ��Ϣ</returns>
        public IList<IStandardRoleInfo> FindAllByParentId(string parentId)
        {
            return provider.FindAllByParentId(parentId);
        }
        #endregion

        #region 属性:FindAllByStandardOrganizationId(string standardOrganizationId)
        /// <summary>�ݹ���ѯĳ����׼��֯�������еı�׼��ɫ</summary>
        /// <param name="standardOrganizationId">��֯��ʶ</param>
        /// <returns>��������<see cref="IRoleInfo"/>ʵ������ϸ��Ϣ</returns>
        public IList<IStandardRoleInfo> FindAllByStandardOrganizationId(string standardOrganizationId)
        {
            return provider.FindAllByStandardOrganizationId(standardOrganizationId);
        }
        #endregion

        #region 属性:FindAllByType(int standardRoleType)
        /// <summary>���ݱ�׼��ɫ���Ͳ�ѯ�������ؼ�¼</summary>
        /// <param name="standardRoleType">��׼��ɫ����</param>
        /// <returns>��������ʵ��<see cref="IStandardRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardRoleInfo> FindAllByType(int standardRoleType)
        {
            return provider.FindAllByType(standardRoleType);
        }
        #endregion

        #region 属性:FindAllByGroupTreeNodeId(string groupTreeNodeId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="groupTreeNodeId">�����ڵ���ʶ</param>
        /// <returns>��������ʵ��<see cref="IStandardRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardRoleInfo> FindAllByGroupTreeNodeId(string groupTreeNodeId)
        {
            return provider.FindAllByGroupTreeNodeId(groupTreeNodeId);
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
        /// <returns>返回一个列表实例<see cref="IStandardRoleInfo"/></returns>
        public IList<IStandardRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
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
        /// <param name="name">��׼��ɫ����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            return provider.IsExistName(name);
        }
        #endregion

        #region 属性:Rename(string id, string name)
        /// <summary>�����Ƿ��������صļ�¼</summary>
        /// <param name="id">��׼��ɫ��ʶ</param>
        /// <param name="name">��׼��ɫ����</param>
        /// <returns>0:�����ɹ� 1:�����Ѵ�����ͬ����</returns>
        public int Rename(string id, string name)
        {
            return provider.Rename(id, name);
        }
        #endregion

        #region 属性:SyncFromPackPage(IStandardRoleInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">��λ��Ϣ</param>
        public int SyncFromPackPage(IStandardRoleInfo param)
        {
            return provider.SyncFromPackPage(param);
        }
        #endregion

        #region 属性:GetKeyStandardRoles()
        /// <summary>��ȡ���йؼ���׼��ɫ</summary>
        /// <returns>����һ���б�ʵ��<see cref="IStandardRoleInfo"/></returns>
        public IList<IStandardRoleInfo> GetKeyStandardRoles()
        {
            return provider.GetKeyStandardRoles();
        }
        #endregion

        #region 属性:GetKeyStandardRoles(int standardRoleType)
        /// <summary>��ȡ���йؼ���׼��ɫ</summary>
        /// <param name="standardRoleType">��׼��ɫ����</param>
        /// <returns>����һ���б�ʵ��<see cref="IStandardRoleInfo"/></returns>
        public IList<IStandardRoleInfo> GetKeyStandardRoles(int standardRoleType)
        {
            return provider.GetKeyStandardRoles(standardRoleType);
        }
        #endregion

        #region 属性:CreatePackage(DateTime beginDate, DateTime endDate)
        /// <summary>�������ݰ�</summary>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        public string CreatePackage(DateTime beginDate, DateTime endDate)
        {
            StringBuilder outString = new StringBuilder();

            string whereClause = string.Format(" UpdateDate BETWEEN ##{0}## AND ##{1}## ", beginDate, endDate);

            IList<IStandardRoleInfo> list = MembershipManagement.Instance.StandardRoleService.FindAll(whereClause);

            outString.AppendFormat("<package packageType=\"standardRole\" beginDate=\"{0}\" endDate=\"{1}\">", beginDate, endDate);

            outString.Append("<standardRoles>");

            foreach (IStandardRoleInfo item in list)
            {
                outString.Append(item.Serializable());
            }

            outString.Append("</standardRoles>");

            outString.Append("</package>");

            return outString.ToString();
        }
        #endregion
    }
}
