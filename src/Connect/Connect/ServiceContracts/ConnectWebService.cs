// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
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

namespace X3Platform.Connect.WebService
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.Model;

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConnectWebService : System.Web.Services.WebService
    {
        private IConnectService service = ConnectContext.Instance.ConnectService;

        // -------------------------------------------------------
        // ��ѯ
        // -------------------------------------------------------

        #region 属性:FindOne(int id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="id">ConnectInfo Id��</param>
        /// <returns>����һ�� ConnectInfo ʵ������ϸ��Ϣ</returns>
        [WebMethod]
        public ConnectInfo FindOne(string id)
        {
            return service.FindOne(id);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>��ѯ�������ؼ�¼</summary>
        /// <param name="whereClause">SQL ��ѯ����</param>
        /// <param name="length">����</param>
        /// <returns>�������� ConnectInfo ʵ������ϸ��Ϣ</returns>
        [WebMethod]
        public IList<ConnectInfo> FindAll(string whereClause, int length)
        {
            return service.FindAll(whereClause, length);
        }
        #endregion
    }
}
