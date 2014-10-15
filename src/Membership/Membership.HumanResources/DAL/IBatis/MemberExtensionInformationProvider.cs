//=============================================================================
//
// Copyright (c) 2007 RuanYu
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2007-06-03
//
//=============================================================================

namespace X3Platform.Membership.HumanResources.DAL.IBatis
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Xml;
    using System.Text;

    using X3Platform;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;

    using X3Platform.Membership.HumanResources.Configuration;
    using X3Platform.Membership.HumanResources.Model;
    using X3Platform.Membership.HumanResources.IDAL;

    [DataObject]
    public class MemberExtensionInformationProvider : IMemberExtensionInformationProvider
    {
        /// <summary>����</summary>
        private HumanResourcesConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Member_ExtensionInformation";

        #region ���캯��:MemberExtensionInformationProvider()
        /// <summary>���캯��</summary>
        public MemberExtensionInformationProvider()
        {
            configuration = HumanResourcesConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        //-------------------------------------------------------
        // ���� ɾ�� �޸�
        //-------------------------------------------------------

        #region ����:Save(MemberExtensionInformation param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="MemberExtensionInformation"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="MemberExtensionInformation"/>��ϸ��Ϣ</returns>
        public MemberExtensionInformation Save(MemberExtensionInformation param)
        {
            if (!IsExist(param.AccountId))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (MemberExtensionInformation)param;
        }
        #endregion

        #region ����:Insert(MemberExtensionInformation param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="MemberExtensionInformation"/>��ϸ��Ϣ</param>
        public void Insert(MemberExtensionInformation param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region ����:Update(MemberExtensionInformation param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="MemberExtensionInformation"/>��ϸ��Ϣ</param>
        public void Update(MemberExtensionInformation param)
        {
            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region ����:Delete(string ids)
        ///<summary>ɾ����¼</summary>
        ///<param name="ids">��ʶ,�����Զ��Ÿ���.</param>
        public void Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        //-------------------------------------------------------
        // ��ѯ
        //-------------------------------------------------------

        #region ����:FindOne(string id)
        ///<summary>��ѯĳ����¼</summary>
        ///<param name="accountId">��ʶ</param>
        ///<returns>����ʵ��<see cref="MemberExtensionInformation"/>����ϸ��Ϣ</returns>
        public MemberExtensionInformation FindOneByAccountId(string accountId)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));

            MemberExtensionInformation param = ibatisMapper.QueryForObject<MemberExtensionInformation>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOneByAccountId", tableName)), args);

            return param;
        }
        #endregion

        #region ����:FindAll(string whereClause,int length)
        ///<summary>��ѯ�������ؼ�¼</summary>
        ///<param name="whereClause">SQL ��ѯ����</param>
        ///<param name="length">����</param>
        ///<returns>��������ʵ��<see cref="MemberExtensionInformation"/>����ϸ��Ϣ</returns>
        public IList<MemberExtensionInformation> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            IList<MemberExtensionInformation> list = ibatisMapper.QueryForList<MemberExtensionInformation>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);

            return list;
        }
        #endregion

        //-------------------------------------------------------
        // �Զ��幦��
        //-------------------------------------------------------

        #region ����:GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��<see cref="MemberExtensionInformation"/></returns>
        public IList<MemberExtensionInformation> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<MemberExtensionInformation> list = ibatisMapper.QueryForList<MemberExtensionInformation>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

            return list;
        }
        #endregion

        #region ����:IsExist(string id)
        ///<summary>��ѯ�Ƿ��������صļ�¼.</summary>
        ///<param name="id">��ʶ</param>
        ///<returns>����ֵ</returns>
        public bool IsExist(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
                throw new Exception("ʵ����ʶ����Ϊ��.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountId='{0}' ", StringHelper.ToSafeSQL(accountId)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion
    }
}