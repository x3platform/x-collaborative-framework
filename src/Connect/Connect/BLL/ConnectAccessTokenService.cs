#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :ConnectAccessTokenService.cs
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
    using X3Platform.Location.IPQuery;
    #endregion

    public sealed class ConnectAccessTokenService : IConnectAccessTokenService
    {
        private ConnectConfiguration configuration = null;

        private IConnectAccessTokenProvider provider = null;

        public ConnectAccessTokenService()
        {
            this.configuration = ConnectConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IConnectAccessTokenProvider>(typeof(IConnectAccessTokenProvider));
        }

        public ConnectAccessTokenInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(ConnectAccessTokenInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">ConnectAccessTokenInfo ʵ����ϸ��Ϣ</param>
        /// <param name="message">���ݿ����󷵻ص�������Ϣ</param>
        /// <returns>ConnectAccessTokenInfo ʵ����ϸ��Ϣ</returns>
        public ConnectAccessTokenInfo Save(ConnectAccessTokenInfo param)
        {
            if (string.IsNullOrEmpty(param.Id)) { throw new NullReferenceException("ʵ����ʶ����Ϊ�ա�"); }

            // ���� Cross Site Script
            param = StringHelper.ToSafeXSS<ConnectAccessTokenInfo>(param);

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
        /// <returns>����һ��ʵ��<see cref="ConnectAccessTokenInfo"/>����ϸ��Ϣ</returns>
        public ConnectAccessTokenInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindOneByAccountId(string appKey, string accountId)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����һ��ʵ��<see cref="ConnectAccessTokenInfo"/>����ϸ��Ϣ</returns>
        public ConnectAccessTokenInfo FindOneByAccountId(string appKey, string accountId)
        {
            return this.provider.FindOneByAccountId(appKey, accountId);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>��������ʵ��<see cref="ConnectAccessTokenInfo"/>����ϸ��Ϣ</returns>
        public IList<ConnectAccessTokenInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>��������ʵ��<see cref="ConnectAccessTokenInfo"/>����ϸ��Ϣ</returns>
        public IList<ConnectAccessTokenInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>��������ʵ��<see cref="ConnectAccessTokenInfo"/>����ϸ��Ϣ</returns>
        public IList<ConnectAccessTokenInfo> FindAll(string whereClause, int length)
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
        public IList<ConnectAccessTokenInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPages(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 属性:IsExist(string id)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="id">��Ա��ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 属性:IsExist(string appKey, string accountId)
        /// <summary>��ѯ�Ƿ��������صļ�¼</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns>����ֵ</returns>
        public bool IsExist(string appKey, string accountId)
        {
            return this.provider.IsExist(appKey, accountId);
        }
        #endregion

        #region 属性:Write(string appKey, string accountId)
        /// <summary>д�����ʺŵķ���������Ϣ</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="accountId">�ʺű�ʶ</param>
        /// <returns></returns>
        public int Write(string appKey, string accountId)
        {
            ConnectAccessTokenInfo param = this.FindOneByAccountId(appKey, accountId);

            if (param == null)
            {
                param = new ConnectAccessTokenInfo();

                param.Id = DigitalNumberContext.Generate("Key_32DigitGuid");
                param.AppKey = appKey;
                param.AccountId = accountId;
                param.ExpireDate = DateTime.Now.AddSeconds(ConnectConfigurationView.Instance.SessionTimeLimit);
                param.RefreshToken = DigitalNumberContext.Generate("Key_32DigitGuid");

                this.Save(param);
            }
            else
            {
                this.Refesh(appKey, param.RefreshToken, DateTime.Now.AddSeconds(ConnectConfigurationView.Instance.SessionTimeLimit));
            }

            return 0;
        }
        #endregion

        #region 属性:Refesh(string appKey, string refreshToken, DateTime expireDate)
        /// <summary>ˢ���ʺŵķ�������</summary>
        /// <param name="appKey">Ӧ�ñ�ʶ</param>
        /// <param name="refreshToken">ˢ������</param>
        /// <param name="expireDate">����ʱ��</param>
        /// <returns></returns>
        public int Refesh(string appKey, string refreshToken, DateTime expireDate)
        {
            return this.provider.Refesh(appKey, refreshToken, expireDate);
        }
        #endregion
    }
}
