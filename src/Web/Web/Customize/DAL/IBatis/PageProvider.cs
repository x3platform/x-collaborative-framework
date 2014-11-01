#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :PageProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Web.Customize.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Web.Configuration;
    using X3Platform.Web.Customize.Model;
    using X3Platform.Web.Customize.IDAL;
    #endregion

    [DataObject]
    public class PageProvider : IPageProvider
    {
        /// <summary>����</summary>
        private WebConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Customize_Page";

        public PageProvider()
        {
            this.configuration = WebConfigurationView.Instance.Configuration;

            this.ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping);
        }

        public PageInfo this[string index]
        {
            get { return this.FindOne(index); }
        }

        // -------------------------------------------------------
        // ���� ���� �޸� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(PageInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">PageInfo ʵ����ϸ��Ϣ</param>
        /// <returns>PageInfo ʵ����ϸ��Ϣ</returns>
        public PageInfo Save(PageInfo param)
        {
            if (!IsExistName(param.AuthorizationObjectType, param.AuthorizationObjectId, param.Name))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return param;
        }
        #endregion

        #region 属性:Insert(PageInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">PageInfo ʵ������ϸ��Ϣ</param>
        public void Insert(PageInfo param)
        {
            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 属性:Update(PageInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">PageInfo ʵ������ϸ��Ϣ</param>
        public void Update(PageInfo param)
        {
            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="ids">��ʶ,�����Զ��Ÿ���</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="param">PageInfo Id��</param>
        /// <returns>����һ�� PageInfo ʵ������ϸ��Ϣ</returns>
        public PageInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 属性:FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ�� PageInfo ʵ������ϸ��Ϣ</returns>
        public PageInfo FindOneByName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AuthorizationObjectType", StringHelper.ToSafeSQL(authorizationObjectType));
            args.Add("AuthorizationObjectId", StringHelper.ToSafeSQL(authorizationObjectId));
            args.Add("Name", name);

            PageInfo param = this.ibatisMapper.QueryForObject<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByName", tableName)), args);

            return param;
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� PageInfo ʵ������ϸ��Ϣ</returns>
        public IList<PageInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return this.ibatisMapper.QueryForList<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
        }
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
        public IList<PageInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<PageInfo> list = this.ibatisMapper.QueryForList<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">PageInfo ʵ����ϸ��Ϣ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("ʵ����ʶ����Ϊ�ա�");
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", id));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����ֵ</returns>
        public bool IsExistName(string authorizationObjectType, string authorizationObjectId, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AuthorizationObjectType = '{0}' AND AuthorizationObjectId = '{1}' AND Name = '{2}' ", StringHelper.ToSafeSQL(authorizationObjectType), StringHelper.ToSafeSQL(authorizationObjectId), StringHelper.ToSafeSQL(name)));

            return ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 属性:TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="authorizationObjectType">��Ȩ��������</param>
        /// <param name="authorizationObjectId">��Ȩ������ʶ</param>
        /// <param name="name">ҳ������</param>
        /// <returns>����һ�� PageInfo ʵ������ϸ��Ϣ</returns>
        public string TryParseHtml(string authorizationObjectType, string authorizationObjectId, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AuthorizationObjectType", StringHelper.ToSafeSQL(authorizationObjectType));
            args.Add("AuthorizationObjectId", StringHelper.ToSafeSQL(authorizationObjectId));
            args.Add("Name", name);

            PageInfo param = this.ibatisMapper.QueryForObject<PageInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_TryParseHtml", tableName)), args);

            return param == null ? string.Empty : param.Html;
        }
        #endregion
    }
}
