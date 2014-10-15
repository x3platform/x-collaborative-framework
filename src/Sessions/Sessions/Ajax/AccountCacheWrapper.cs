#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AccountCacheWrapper.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

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

    /// <summary>�ʺŻ���</summary>
    public class AccountCacheWrapper : ContextWrapper
    {
        IAccountCacheService service = SessionContext.Instance.AccountCacheService;

        #region 属性:Heartbeat(XmlDocument doc)
        /// <summary>��������</summary>
        /// <returns></returns>
        public string Heartbeat(XmlDocument doc)
        {
            if (KernelContext.Current.User == null)
            {
                // δ��¼�û���Ϣ����
                // return "{\"ajaxStorage\":{\"sessionId\":\"\"},\"message\":{\"returnCode\":1,\"value\":\"����¼ϵͳ��\"}}";
            }
            else
            {
                // �ѵ�¼�û���Ϣ����
                // return "{\"ajaxStorage\":{\"sessionId\":\"" + HttpContext.Current.Session.SessionID + "\"},\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
            }

            return "1";
        }
        #endregion

        #region 属性:GetSessionId(XmlDocument doc)
        /// <summary>��ȡ�Ự��ʶ</summary>
        /// <returns></returns>
        public string GetSessionId(XmlDocument doc)
        {
            if (KernelContext.Current.User == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"����¼ϵͳ��\"}}";
            }
            else
            {
                return "{\"ajaxStorage\":{\"sessionId\":\"" + HttpContext.Current.Session.SessionID + "\"},\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}";
            }
        }
        #endregion

        #region 属性:Find(XmlDocument doc)
        /// <summary>��������</summary>
        /// <returns></returns>
        public string Find(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            string accountIdentity = AjaxStorageConvertor.Fetch("accountIdentity", doc);

            AccountCacheInfo param = this.service.FindByAccountIdentity(accountIdentity);

            outString.Append("{\"ajaxStorage\":");

            if (param != null)
            {
                outString.Append("{");
                outString.Append("\"accountIdentity\":\"" + param.AccountIdentity + "\",");
                outString.Append("\"accountCacheValue\":\"" + param.AccountCacheValue + "\",");
                outString.Append("\"accountObjectType\":\"" + param.AccountObjectType + "\",");
                outString.Append("\"accountObject\":\"" + param.AccountObject + "\",");
                outString.Append("\"updateDate\":\"" + param.UpdateDate + "\" ");
                outString.Append("}");
            }

            outString.Append(",\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}");

            return outString.ToString();
        }
        #endregion

        #region 属性:Dump(XmlDocument doc)
        /// <summary>ת������</summary>
        /// <returns></returns>
        public string Dump(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            IList<AccountCacheInfo> list = service.Dump();

            outString.Append("{\"ajaxStorage\":[");

            foreach (AccountCacheInfo item in list)
            {
                outString.Append("{");
                outString.Append("\"accountIdentity\":\"" + item.AccountIdentity + "\",");
                outString.Append("\"accountCacheValue\":\"" + item.AccountCacheValue + "\",");
                outString.Append("\"updateDate\":\"" + item.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\" ");
                outString.Append("},");
            }

            outString = StringHelper.TrimEnd(outString, ",");

            outString.Append("],\"message\":{\"returnCode\":0,\"value\":\"�����ɹ���\"}}");

            return outString.ToString();
        }
        #endregion
    }
}