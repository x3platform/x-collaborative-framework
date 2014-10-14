// =============================================================================
//
// Copyright (c) 2011, ruany@live.com
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

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using X3Platform.Security.Authority.IDAL;
using X3Platform.Security.Authority.IBLL;

namespace X3Platform.Security.Authority.WebService
{
    [System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AuthorityWebService : System.Web.Services.WebService
	{
        private IAuthorityService service = AuthorityContext.Instance.AuthorityService;

        //-------------------------------------------------------
        // ��ѯ
        //-------------------------------------------------------

        #region ����:FindOne(string id)
        /// <summary>��ѯĳ����¼</summary>
        /// <param name="Id">AuthorityInfo Id��</param>
        /// <returns>����һ�� AuthorityInfo ʵ������ϸ��Ϣ</returns>
        [WebMethod]
        public AuthorityInfo FindOne(string id)
        {
            try
            {
                return service.FindOne(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
	}
}
