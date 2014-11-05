namespace X3Platform.Security.Authority.WebService
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Services;
    using System.Web.Services.Protocols;

    using X3Platform.Security.Authority.IBLL;

    [System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AuthorityWebService : System.Web.Services.WebService
	{
        private IAuthorityService service = AuthorityContext.Instance.AuthorityService;

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="Id">AuthorityInfo Id号</param>
        /// <returns>返回一个 AuthorityInfo 实例的详细信息</returns>
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
