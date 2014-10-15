#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ConnectService.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Connect.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.DigitalNumber;
    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.IDAL;
    using X3Platform.Connect.Model;
    using X3Platform.CacheBuffer;
    #endregion

    public sealed class ConnectService : IConnectService
    {
        private ConnectConfiguration configuration = null;

        private IConnectProvider provider = null;

        /// <summary>�����洢</summary>
        private Dictionary<string, ConnectInfo> Dictionary = new Dictionary<string, ConnectInfo>();

        public ConnectService()
        {
            this.configuration = ConnectConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IConnectProvider>(typeof(IConnectProvider));
        }

        #region 属性:this[string appKey]
        /// <summary>����</summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public ConnectInfo this[string appKey]
        {
            get { return this.FindOneByAppKey(appKey); }
        }
        #endregion

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(ConnectInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ConnectInfo ʵ����ϸ��Ϣ</param>
        /// <param name="message">���ݿ����󷵻ص�������Ϣ</param>
        /// <returns>ConnectInfo ʵ����ϸ��Ϣ</returns>
        public ConnectInfo Save(ConnectInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new Exception("ʵ����ʶ����Ϊ�ա�"); }

            bool isNewObject = !this.IsExist(param.Id);

            string methodName = isNewObject ? "����" : "�༭";

            if (isNewObject)
            {
                IAccountInfo account = KernelContext.Current.User;

                param.AccountId = account.Id;
                param.AccountName = account.Name;
            }

            // ���� Cross Site Script
            param = StringHelper.ToSafeXSS<ConnectInfo>(param);

            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="keys">��ʶ,�����Զ��Ÿ���</param>
        public int Delete(string ids)
        {
            return this.provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">��������ʶ</param>
        /// <returns>����һ��ʵ��<see cref="ConnectInfo"/>����ϸ��Ϣ</returns>
        public ConnectInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindOneByAppKey(string appKey)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>����һ��ʵ��<see cref="ConnectInfo"/>����ϸ��Ϣ</returns>
        public ConnectInfo FindOneByAppKey(string appKey)
        {
            ConnectInfo param = null;

            // ��ʼ������
            if (this.Dictionary.Count == 0)
            {
                IList<ConnectInfo> list = this.FindAll();

                foreach (ConnectInfo item in list)
                {
                    this.Dictionary.Add(item.AppKey, item);
                }
            }

            // ���һ�������
            if (this.Dictionary.ContainsKey(appKey))
            {
                param = this.Dictionary[appKey];
            }

            // ����������δ�ҵ��������ݣ����������ݿ�����
            return param == null ? this.provider.FindOneByAppKey(appKey) : param;
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="ConnectInfo"/>����ϸ��Ϣ</returns>
        public IList<ConnectInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="ConnectInfo"/>����ϸ��Ϣ</returns>
        public IList<ConnectInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="ConnectInfo"/>����ϸ��Ϣ</returns>
        public IList<ConnectInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
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
        public IList<ConnectInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>��ҳ����</summary>
        /// <param name="startIndex">��ʼ��������,��0��ʼͳ��</param>
        /// <param name="pageSize">ҳ����С</param>
        /// <param name="whereClause">WHERE ��ѯ����</param>
        /// <param name="orderBy">ORDER BY ��������</param>
        /// <param name="rowCount">����</param>
        /// <returns>����һ���б�ʵ��</returns>
        public IList<ConnectQueryInfo> GetQueryObjectPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetQueryObjectPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExistAppKey(string appKey)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns>����ֵ</returns>
        public bool IsExistAppKey(string appKey)
        {
            return (this.FindOneByAppKey(appKey) == null) ? false : true;
        }
        #endregion

        #region 属性:ResetAppSecret(string appKey)
        /// <summary>����Ӧ������������Կ</summary>
        /// <param name="appKey">AppKey</param>
        /// <returns></returns>
        public int ResetAppSecret(string appKey)
        {
            string appSecret = Guid.NewGuid().ToString().Substring(0, 8);

            return this.ResetAppSecret(appKey, appSecret);
        }
        #endregion

        #region 属性:ResetAppSecret(string appKey, string appSecret)
        /// <summary>����Ӧ������������Կ</summary>
        /// <param name="appKey">App Key</param>
        /// <param name="appSecret">App Secret</param>
        /// <returns></returns>
        public int ResetAppSecret(string appKey, string appSecret)
        {
            // ���»�����Ϣ
            ConnectInfo param = this.FindOneByAppKey(appKey);

            param.AppSecret = appSecret;

            // �������ݿ���Ϣ
            return this.provider.ResetAppSecret(appKey, appSecret);
        }
        #endregion
    }
}
