namespace X3Platform.Sessions.Interop
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Web;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Sessions.IBLL;
    #endregion

    /// <summary>帐号缓存</summary>
    public class AccountCacheWrapper : ContextWrapper
    {
        IAccountCacheService service = SessionContext.Instance.AccountCacheService;

        #region 函数:Heartbeat(XmlDocument doc)
        /// <summary>保持心跳</summary>
        /// <returns></returns>
        public string Heartbeat(XmlDocument doc)
        {
            return "1";
        }
        #endregion

        #region 函数:GetSessionId(XmlDocument doc)
        /// <summary>获取会话标识</summary>
        /// <returns></returns>
        public string GetSessionId(XmlDocument doc)
        {
            if (KernelContext.Current.User == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"请登录系统。\"}}";
            }
            else
            {
                return "{\"data\":{\"sessionId\":\"" + HttpContext.Current.Session.SessionID + "\"},\"message\":{\"returnCode\":0,\"value\":\"操作成功。\"}}";
            }
        }
        #endregion

        #region 函数:Find(XmlDocument doc)
        /// <summary>查找数据</summary>
        /// <returns></returns>
        public string Find(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountIdentity = XmlHelper.Fetch("accountIdentity", doc);

            AccountCacheInfo param = this.service.FindByAccountIdentity(accountIdentity);

            outString.Append("{\"data\":");

            if (param != null)
            {
                outString.Append("{");
                outString.Append("\"accountIdentity\":\"" + param.AccountIdentity + "\",");
                outString.Append("\"accountCacheValue\":\"" + param.AccountCacheValue + "\",");
                outString.Append("\"accountObjectType\":\"" + param.AccountObjectType + "\",");
                outString.Append("\"accountObject\":\"" + param.AccountObject + "\",");
                outString.Append("\"date\":\"" + param.Date + "\" ");
                outString.Append("}");
            }

            outString.Append(",\"message\":{\"returnCode\":0,\"value\":\"操作成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Dump(XmlDocument doc)
        /// <summary>转储数据</summary>
        /// <returns></returns>
        public string Dump(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            IList<AccountCacheInfo> list = service.Dump();

            outString.Append("{\"data\":[");

            foreach (AccountCacheInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"accountIdentity\":\"" + item.AccountIdentity + "\",");
                outString.Append("\"accountCacheValue\":\"" + item.AccountCacheValue + "\",");
                outString.Append("\"date\":\"" + item.Date.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\" ");
                outString.Append("},");
            }

            outString = StringHelper.TrimEnd(outString, ",");

            outString.Append("],\"message\":{\"returnCode\":0,\"value\":\"操作成功。\"}}");

            return outString.ToString();
        }
        #endregion
    }
}