#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
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
#endregion

namespace X3Platform.Membership.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;

    using X3Platform.DigitalNumber;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.Membership.IDAL;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Model;
    #endregion

    [DataObject]
    public class AccountGrantProvider : IAccountGrantProvider
    {
        /// <summary>����</summary>
        private MembershipConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        /// <summary>IBatisӳ������</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_Account_Grant";

        #region ���캯��:AccountGrantProvider()
        /// <summary>���캯��</summary>
        public AccountGrantProvider()
        {
            configuration = MembershipConfigurationView.Instance.Configuration;

            ibatisMapping = configuration.Keys["IBatisMapping"].Value;

            ibatisMapper = ISqlMapHelper.CreateSqlMapper(ibatisMapping, true);
        }
        #endregion

        // -------------------------------------------------------
        // ����֧��
        // -------------------------------------------------------

        #region 属性:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        public void BeginTransaction()
        {
            ibatisMapper.BeginTransaction();
        }
        #endregion

        #region 属性:BeginTransaction(IsolationLevel isolationLevel)
        /// <summary>��������</summary>
        /// <param name="isolationLevel">�������뼶��</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            ibatisMapper.BeginTransaction(isolationLevel);
        }
        #endregion

        #region 属性:CommitTransaction()
        /// <summary>�ύ����</summary>
        public void CommitTransaction()
        {
            ibatisMapper.CommitTransaction();
        }
        #endregion

        #region 属性:RollBackTransaction()
        /// <summary>�ع�����</summary>
        public void RollBackTransaction()
        {
            ibatisMapper.RollBackTransaction();
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ�� �޸�
        // -------------------------------------------------------

        #region 属性:Save(IAccountGrantInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</param>
        /// <returns>ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</returns>
        public IAccountGrantInfo Save(IAccountGrantInfo param)
        {
            if (string.IsNullOrEmpty(param.Id) || !IsExist(param.Id))
            {
                Insert(param);
            }
            else
            {
                Update(param);
            }

            return (IAccountGrantInfo)param;
        }
        #endregion

        #region 属性:Insert(IAccountGrantInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</param>
        public void Insert(IAccountGrantInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                param.Id = DigitalNumberContext.Generate("Key_Guid");
            }

            if (string.IsNullOrEmpty(param.Code))
            {
                param.Code = DigitalNumberContext.Generate("Table_Account_Grant_Key_Code");
            }

            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 属性:Update(IAccountGrantInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="IAccountGrantInfo"/>��ϸ��Ϣ</param>
        public void Update(IAccountGrantInfo param)
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
            {
                return;
            }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Ids", string.Format("'{0}'", ids.Replace(",", "','")));

            ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        public IAccountGrantInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            return ibatisMapper.QueryForObject<IAccountGrantInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", tableName)), args);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="IAccountGrantInfo"/>����ϸ��Ϣ</returns>
        public IList<IAccountGrantInfo> FindAll(string whereClause, int length)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("Length", length);

            return ibatisMapper.QueryForList<IAccountGrantInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", tableName)), args);
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
        /// <returns>����һ���б�ʵ��<see cref="IAccountGrantInfo"/></returns>
        public IList<IAccountGrantInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            orderBy = string.IsNullOrEmpty(orderBy) ? " UpdateDate DESC " : orderBy;

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", StringHelper.ToSafeSQL(whereClause));
            args.Add("OrderBy", StringHelper.ToSafeSQL(orderBy));

            args.Add("RowCount", 0);

            IList<IAccountGrantInfo> list = ibatisMapper.QueryForList<IAccountGrantInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPages", tableName)), args);

            rowCount = (int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", tableName)), args);

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

            args.Add("WhereClause", string.Format(" Id='{0}' ", StringHelper.ToSafeSQL(id)));

            isExist = ((int)ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 属性:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>��ѯ�Ƿ���������ί���˵ļ�¼.</summary>
        /// <param name="grantorId">ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        {
            return this.IsExistGrantor(grantorId, grantedTimeFrom, grantedTimeTo, string.Empty);
        }
        #endregion

        #region 属性:IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>��ѯ�Ƿ���������ί���˵ļ�¼.</summary>
        /// <param name="grantorId">ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <param name="ignoreIds">����ί�б�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantor(string grantorId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        {
            if (string.IsNullOrEmpty(grantorId)) { throw new Exception("ί���˱�ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(ignoreIds))
            {
                args.Add("WhereClause", string.Format(@" GrantorId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' ))
AND IsAborted = 0 ", StringHelper.ToSafeSQL(grantorId), grantedTimeFrom, grantedTimeTo));
            }
            else
            {
                args.Add("WhereClause", string.Format(@" GrantorId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' )) 
AND IsAborted = 0 AND Id NOT IN ('{3}') ", StringHelper.ToSafeSQL(grantorId), grantedTimeFrom, grantedTimeTo, ignoreIds.Replace(",", "','")));
            }

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        /// <summary>��ѯ�Ƿ��������ر�ί���˵ļ�¼.</summary>
        /// <param name="granteeId">��ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo)
        {
            return this.IsExistGrantee(granteeId, grantedTimeFrom, grantedTimeTo, string.Empty);
        }
        #endregion

        #region 属性:IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        /// <summary>��ѯ�Ƿ��������ر�ί���˵ļ�¼.</summary>
        /// <param name="granteeId">��ί���˱�ʶ</param>
        /// <param name="grantedTimeFrom">ί�п�ʼʱ��</param>
        /// <param name="grantedTimeTo">ί�н���ʱ��</param>
        /// <param name="ignoreIds">����ί�б�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExistGrantee(string granteeId, DateTime grantedTimeFrom, DateTime grantedTimeTo, string ignoreIds)
        {
            if (string.IsNullOrEmpty(granteeId)) { throw new Exception("��ί���˱�ʶ����Ϊ�ա�"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(ignoreIds))
            {
                args.Add("WhereClause", string.Format(@" GranteeId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' )) 
AND IsAborted = 0 ", StringHelper.ToSafeSQL(granteeId), grantedTimeFrom, grantedTimeTo));
            }
            else
            {
                args.Add("WhereClause", string.Format(@" GranteeId = '{0}' 
AND (( GrantedTimeFrom <= '{1}' AND GrantedTimeTo >= '{1}' ) OR ( GrantedTimeFrom <= '{2}' AND GrantedTimeTo >= '{2}' )) 
AND IsAborted = 0 AND Id NOT IN ('{3}') ", StringHelper.ToSafeSQL(granteeId), grantedTimeFrom, grantedTimeTo, ignoreIds.Replace(",", "','")));
            }

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:Abort(string id)
        /// <summary>��ֹ��ǰί��</summary>
        /// <param name="id">��ʶ</param>
        /// <returns></returns>
        public int Abort(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));

            ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Abort", tableName)), args);

            return 0;
        }
        #endregion

        // -------------------------------------------------------
        // ͬ������
        // -------------------------------------------------------

        #region 属性:SyncFromPackPage(IAccountGrantInfo param)
        /// <summary>ͬ����Ϣ</summary>
        /// <param name="param">�ʺ���Ϣ</param>
        public int SyncFromPackPage(IAccountGrantInfo param)
        {
            ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_SyncFromPackPage", tableName)), param);

            return 0;
        }
        #endregion
    }
}