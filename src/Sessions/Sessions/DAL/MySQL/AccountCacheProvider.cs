// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :AccountCacheProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Sessions.DAL.MySQL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Storages;
    using X3Platform.Util;

    using X3Platform.Sessions;
    using X3Platform.Sessions.Configuration;
    using X3Platform.Sessions.IDAL;
    using Common.Logging;
    #endregion

    /// <summary>�ʺŻ��������ṩ��</summary>
    [DataObject]
    public class AccountCacheProvider : IAccountCacheProvider
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SessionsConfiguration configuration = null;

        /// <summary>IBatisӳ���ļ�</summary>
        private string ibatisMapping = null;

        private Dictionary<string, ISqlMapper> ibatisMappers = null;

        /// <summary>���ݱ���</summary>
        private string tableName = "tb_AccountCache";

        /// <summary>���ݴ洢�ܹ���ʶ</summary>
        private string storageSchemaId = "01-14-Sessions";

        /// <summary>���ݴ洢����</summary>
        private IStorageStrategy storageStrategy = null;

        #region ���캯��:AccountCacheProvider()
        /// <summary>���캯��</summary>
        public AccountCacheProvider()
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("X3Platform.Sessions.DAL.MySQL.AccountCacheProvider constructor() begin");
            }
            this.configuration = SessionsConfigurationView.Instance.Configuration;

            this.ibatisMapping = this.configuration.Keys["IBatisMapping"].Value;

            this.ibatisMappers = StorageContext.Instance.CreateSqlMappers(this.storageSchemaId, this.ibatisMapping);
   
            this.storageStrategy = StorageContext.Instance.StorageSchemaService[this.storageSchemaId].GetStrategyClass();

            if (logger.IsDebugEnabled)
            {
                logger.Debug("X3Platform.Sessions.DAL.MySQL.AccountCacheProvider constructor() end");
            }
        }
        #endregion

        #region 属性:FindByAccountIdentity(string accountIdentity)
        /// <summary>���ݲ���ĳ����¼</summary>
        /// <param name="accountIdentity">�ʺŻ���Ψһ��ʶ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        public AccountCacheInfo FindByAccountIdentity(string accountIdentity)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountIdentity", StringHelper.ToSafeSQL(accountIdentity, true));

            return this.ibatisMappers[storageNode.Name].QueryForObject<AccountCacheInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindByAccountIdentity", tableName)), args);
        }
        #endregion

        #region 属性:FindByAccountCacheValue(string accountCacheValue)
        /// <summary>����ĳ����¼</summary>
        /// <param name="accountCacheValue">�ʺŻ�����ֵ</param>
        /// <returns>����һ��ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</returns>
        public AccountCacheInfo FindByAccountCacheValue(string accountCacheValue)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("AccountCacheValue", StringHelper.ToSafeSQL(accountCacheValue, true));

            return this.ibatisMappers[storageNode.Name].QueryForObject<AccountCacheInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindByAccountCacheValue", tableName)), args);
        }
        #endregion

        #region 属性:Dump()
        /// <summary>ת�����м�¼��Ϣ</summary>
        /// <returns>����һ��<see cref="AccountCacheInfo"/>�б�</returns>
        public IList<AccountCacheInfo> Dump()
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            return this.ibatisMappers[storageNode.Name].QueryForList<AccountCacheInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_Dump", tableName)), args);
        }
        #endregion

        #region 属性:Insert(AccountCacheInfo param)
        /// <summary>���Ӽ�¼</summary>
        /// <param name="param">ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</param>
        public void Insert(AccountCacheInfo param)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            this.ibatisMappers[storageNode.Name].Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Insert", tableName)), param);
        }
        #endregion

        #region 属性:Update(AccountCacheInfo param)
        /// <summary>�޸ļ�¼</summary>
        /// <param name="param">ʵ��<see cref="AccountCacheInfo"/>����ϸ��Ϣ</param>
        public void Update(AccountCacheInfo param)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            this.ibatisMappers[storageNode.Name].Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Update", tableName)), param);
        }
        #endregion

        #region 属性:Delete(string accountIdentity)
        /// <summary>ɾ����¼</summary>
        /// <param name="accountIdentity">�ʺŻ���Ψһ��ʶ</param>
        public int Delete(string accountIdentity)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Delete", accountIdentity);

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountIdentity < '{0}' ", StringHelper.ToSafeSQL(accountIdentity, true)));

            this.ibatisMappers[storageNode.Name].Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            return 0;
        }
        #endregion

        #region 属性:IsExist(string accountIdentity)
        /// <summary>������¼�Ƿ�����</summary>
        /// <param name="accountIdentity">�ʺŻ���Ψһ��ʶ</param>
        public bool IsExist(string accountIdentity)
        {
            if (string.IsNullOrEmpty(accountIdentity)) { throw new Exception("�ʺŻ���Ψһ��ʶ����Ϊ�ա�"); }

            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountIdentity = '{0}' ", StringHelper.ToSafeSQL(accountIdentity, true)));

            return (Convert.ToInt32(this.ibatisMappers[storageNode.Name].QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
        }
        #endregion

        #region 属性:IsExistAccountCacheValue(string accountCacheValue)
        /// <summary>������¼�Ƿ�����</summary>
        /// <param name="accountCacheValue">�ʺŻ���ֵ</param>
        public bool IsExistAccountCacheValue(string accountCacheValue)
        {
            if (string.IsNullOrEmpty(accountCacheValue)) { throw new Exception("�ʺŻ���ֵ����Ϊ�ա�"); }

            IStorageNode storageNode = storageStrategy.GetStorageNode("Query");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" AccountCacheValue = '{0}' ", StringHelper.ToSafeSQL(accountCacheValue, true)));

            return ((int)this.ibatisMappers[storageNode.Name].QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args) == 0) ? false : true;
        }
        #endregion

        #region 属性:Clear(DateTime expiryTime)
        /// <summary>��������ʱ��֮ǰ�Ļ�����¼</summary>
        /// <param name="expiryTime">����ʱ��</param>
        public int Clear(DateTime expiryTime)
        {
            IStorageNode storageNode = storageStrategy.GetStorageNode("Delete");

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" EndDate < '{0}' ", expiryTime.ToString("yyyy-MM-dd HH:mm:ss")));

            this.ibatisMappers[storageNode.Name].Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", tableName)), args);

            return 0;
        }
        #endregion
    }
}
