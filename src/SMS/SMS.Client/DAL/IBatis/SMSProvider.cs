namespace X3Platform.SMS.Client.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Common.Logging;

    using X3Platform.IBatis.DataMapper;
    using X3Platform.Storages;
    using X3Platform.Util;

    using X3Platform.SMS.Client.Configuration;
    using X3Platform.SMS.Client.IDAL;
    #endregion

    /// <summary>帐号缓存数据提供者</summary>
    [DataObject]
    public class SMSProvider : ISMSProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_SMS";

        #region 构造函数:SMSProvider()
        /// <summary>构造函数</summary>
        public SMSProvider()
        {
            this.ibatisMapping = SMSConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }
        #endregion

        #region 函数:Send(string serialNumber, string accountId, string phoneNumber, string messageContent, string returnCode)
        /// <summary>发送信息</summary>
        /// <param name="serialNumber">流水号</param>
        /// <param name="accountId">帐号唯一标识</param>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="messageContent">消息内容</param>
        /// <param name="returnCode">返回编码</param>
        public int Send(string serialNumber, string accountId, string phoneNumber, string messageContent, string returnCode)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("SerialNumber", StringHelper.ToSafeSQL(serialNumber));
            args.Add("AccountId", StringHelper.ToSafeSQL(accountId));
            args.Add("PhoneNumber", StringHelper.ToSafeSQL(phoneNumber));
            args.Add("MessageContent", StringHelper.ToSafeSQL(messageContent));
            args.Add("ReturnCode", StringHelper.ToSafeSQL(returnCode));

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Send", this.tableName)), args);

            return 0;
        }
        #endregion
    }
}
