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
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Connect.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Spring;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.IDAL;
    #endregion

    public sealed class ConnectCallService : IConnectCallService
    {
        private ConnectConfiguration configuration = null;

        private IConnectCallProvider provider = null;

        public ConnectCallService()
        {
            configuration = ConnectConfigurationView.Instance.Configuration;

            // �������󹹽���(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // ���������ṩ��
            this.provider = objectBuilder.GetObject<IConnectCallProvider>(typeof(IConnectCallProvider));
        }

        public ConnectCallInfo this[string index]
        {
            get { return this.FindOne(index); }
        }

        // -------------------------------------------------------
        // ���� ɾ��
        // -------------------------------------------------------

        #region 属性:Save(AccountInfo param)
        /// <summary>������¼</summary>
        /// <param name="param">AccountInfo ʵ����ϸ��Ϣ</param>
        /// <param name="message">���ݿ����󷵻ص�������Ϣ</param>
        /// <returns>AccountInfo ʵ����ϸ��Ϣ</returns>
        public ConnectCallInfo Save(ConnectCallInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string ids)
        /// <summary>ɾ����¼</summary>
        /// <param name="keys">��ʶ,�����Զ��Ÿ���</param>
        public void Delete(string ids)
        {
            this.provider.Delete(ids);
        }
        #endregion

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(int id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">AccountInfo Id��</param>
        /// <returns>����һ�� AccountInfo ʵ������ϸ��Ϣ</returns>
        public ConnectCallInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 属性:FindAll()
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<ConnectCallInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 属性:FindAll(string whereClause)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<ConnectCallInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� AccountInfo ʵ������ϸ��Ϣ</returns>
        public IList<ConnectCallInfo> FindAll(string whereClause, int length)
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
        public IList<ConnectCallInfo> GetPages(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
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

        // -------------------------------------------------------
        // Ȩ��
        // -------------------------------------------------------

        #region 属性:GetAuthorizationObject(ConnectCallInfo param)
        /// <summary>��֤������Ȩ��</summary>
        /// <param name="param">����֤�Ķ���</param>
        /// <returns>����</returns>
        private ConnectCallInfo GetAuthorizationObject(ConnectCallInfo param)
        {
            return param;
        }
        #endregion
    }
}
