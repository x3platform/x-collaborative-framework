// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :IStandardGeneralRoleProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Membership.DAL.IBatis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Model;
    using X3Platform.Data;

    /// <summary></summary>
    [DataObject]
    public class StandardGeneralRoleProvider : IStandardGeneralRoleProvider
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_StandardGeneralRole";

        #region ���캯��:StandardGeneralRoleProvider()
        /// <summary>���캯��</summary>
        public StandardGeneralRoleProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region 属性:Save(IStandardGeneralRoleInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</returns>
        public IStandardGeneralRoleInfo Save(IStandardGeneralRoleInfo param)
        {
            if (!IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (IStandardGeneralRoleInfo)param;
        }
        #endregion

        #region 属性:Insert(IStandardGeneralRoleInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        public void Insert(IStandardGeneralRoleInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_StandardGeneralRole_Key_Code");
            }

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 属性:Update(IStandardGeneralRoleInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="IStandardGeneralRoleInfo"/>��ϸ��Ϣ</param>
        public void Update(IStandardGeneralRoleInfo param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(ids).Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IStandardGeneralRoleInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            IStandardGeneralRoleInfo param = ibatisMapper.QueryForObject<IStandardGeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);

            return param;
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardGeneralRoleInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<IStandardGeneralRoleInfo> list = ibatisMapper.QueryForList<IStandardGeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:FindAllByCatalogItemId(string CatalogItemId)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="CatalogItemId">�����ڵ���ʶ</param>
        /// <returns>��������ʵ��<see cref="IStandardGeneralRoleInfo"/>����ϸ��Ϣ</returns>
        public IList<IStandardGeneralRoleInfo> FindAllByCatalogItemId(string CatalogItemId)
        {
            string whereClause = string.Format(" CatalogItemId = ##{0}## ORDER BY OrderId ", CatalogItemId);

            return this.FindAll(whereClause, 0);
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
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IStandardGeneralRoleInfo"/></returns>
        public IList<IStandardGeneralRoleInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" OrderId, ModifiedDate DESC "));

            args.Add("RowCount", 0);

            IList<IStandardGeneralRoleInfo> list = ibatisMapper.QueryForList<IStandardGeneralRoleInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", tableName)), args);

            rowCount = Convert.ToInt32(ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("ʵ����ʶ����Ϊ�ա�");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 属性:IsExistName(string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼.</summary>
        /// <param name="name">��׼ͨ�ý�ɫ����</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("ʵ�����Ʋ���Ϊ�ա�");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Name = '{0}' ", StringHelper.ToSafeSQL(name)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        // -------------------------------------------------------
        // ���ñ�׼ͨ�ý�ɫ����֯ӳ����ϵ
        // -------------------------------------------------------

        #region 属性:FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>���ұ�׼ͨ�ý�ɫ����֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public IStandardGeneralRoleMappingRelationInfo FindOneMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            return this.ibatisMapper.QueryForObject<IStandardGeneralRoleMappingRelationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneMappingRelation", tableName)), args);
        }
        #endregion

        #region 属性:GetMappingRelationPaging(int startIndex, int pageSize,  DataQuery query, out int rowCount)
        /// <summary>标准通用角色映射关系分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例<see cref="IStandardGeneralRoleMappingRelationInfo"/></returns>
        public IList<IStandardGeneralRoleMappingRelationInfo> GetMappingRelationPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", query.GetWhereSql(new Dictionary<string, string>() { { "Name", "LIKE" } }));
            args.Add("OrderBy", query.GetOrderBySql(" RoleName "));

            args.Add("RowCount", 0);

            IList<IStandardGeneralRoleMappingRelationInfo> list = ibatisMapper.QueryForList<IStandardGeneralRoleMappingRelationInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMappingRelationPaging", tableName)), args);

            rowCount = Convert.ToInt32(ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetMappingRelationRowCount", tableName)), args));

            return list;
        }
        #endregion

        #region 属性:AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId, string standardRoleId)
        /// <summary>���ӱ�׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        /// <param name="roleId">��֯��ʶ</param>
        /// <param name="standardRoleId">��֯��ʶ</param>
        public int AddMappingRelation(string standardGeneralRoleId, string organizationId, string roleId, string standardRoleId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));
            args.Add("RoleId", StringHelper.ToSafeSQL(roleId));
            args.Add("StandardRoleId", StringHelper.ToSafeSQL(standardRoleId));

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_AddMappingRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 属性:RemoveMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>�Ƴ���׼ͨ�ý�ɫ��������֯��ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public int RemoveMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_RemoveMappingRelation", tableName)), args);

            return 0;
        }
        #endregion

        #region 属性:HasMappingRelation(string standardGeneralRoleId, string organizationId)
        /// <summary>������׼ͨ�ý�ɫ��������֯�Ƿ���ӳ����ϵ</summary>
        /// <param name="standardGeneralRoleId">��׼ͨ�ý�ɫ��ʶ</param>
        /// <param name="organizationId">��֯��ʶ</param>
        public bool HasMappingRelation(string standardGeneralRoleId, string organizationId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("StandardGeneralRoleId", StringHelper.ToSafeSQL(standardGeneralRoleId));
            args.Add("OrganizationUnitId", StringHelper.ToSafeSQL(organizationId));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_HasMappingRelation", tableName)), args) == 0) ? false : true;
        }
        #endregion
    }
}
