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
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Web.APIs.Methods
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;

    using Common.Logging;
    
    using X3Platform.Apps.Model;
    using X3Platform.Ajax.Json;
    using X3Platform.Membership;
    using X3Platform.Apps;
    #endregion

    /// <summary>����������</summary>
    public sealed class MethodInvoker
    {
        // -------------------------------------------------------
        // ִ�з���
        // -------------------------------------------------------

        #region 属性:Invoke(string methodName, XmlDocument doc)
        /// <summary>ִ�з���</summary>
        /// <param name="methodName">��������</param>
        /// <param name="doc">Xml �ĵ�Ԫ��</param>
        public static string Invoke(string methodName, XmlDocument doc, ILog logger)
        {
            IMethod method = null;

            // Ӧ�÷�����Ϣ
            ApplicationMethodInfo param = AppsContext.Instance.ApplicationMethodService.FindOneByName(methodName);

            if (param == null)
            {
                logger.Warn("unkown methodName:" + methodName + ", please contact the administrator.");

                return "{\"message\":{\"returnCode\":1,\"value\":\"��" + methodName + "�����������ڣ�����ϵ����Ա����������Ϣ��\"}}";
            }

            // Ӧ�÷���������Ӧ����Ϣ
            ApplicationInfo application = param.Application;

            if (param.Status == 0)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"��" + methodName + "������ �ѱ����á�\"}}";
            }

            if (param.EffectScope == 1)
            {
                // ���������˿��Է���
            }
            else
            {
                // ��ǰ�û���Ϣ
                IAccountInfo account = KernelContext.Current == null ? null : KernelContext.Current.User;

                if (param.EffectScope == 2 && account == null)
                {
                    // ��Ҫ����¼�û������ϼ���Ȩ�޲��ܵ��ô˷���
                    return "{\"message\":{\"returnCode\":2,\"value\":\"��" + methodName + "���˷�����Ҫ����¼�û����������ܵ��ô˷�����\"}}";
                }
                else if (param.EffectScope == 4 && !(AppsSecurity.IsMember(account, application.ApplicationName) || AppsSecurity.IsReviewer(account, application.ApplicationName) || AppsSecurity.IsAdministrator(account, application.ApplicationName)))
                {
                    // ��Ҫ��Ӧ�ÿɷ��ʳ�Ա�����ϼ���Ȩ�޲��ܵ��ô˷���
                    return "{\"message\":{\"returnCode\":2,\"value\":\"��" + methodName + "���˷�����Ҫ��Ӧ�ÿɷ��ʳ�Ա�����ϼ���Ȩ�޲��ܵ��ô˷�����\"}}";
                }
                else if (param.EffectScope == 8 && !(AppsSecurity.IsReviewer(account, application.ApplicationName) || AppsSecurity.IsAdministrator(account, application.ApplicationName)))
                {
                    // ��Ҫ��Ӧ������Ա�����ϼ���Ȩ�޲��ܵ��ô˷���
                    return "{\"message\":{\"returnCode\":2,\"value\":\"��" + methodName + "���˷�����Ҫ��Ӧ������Ա�����ϼ���Ȩ�޲��ܵ��ô˷�����\"}}";
                }
                else if (param.EffectScope == 16 && !AppsSecurity.IsAdministrator(account, application.ApplicationName))
                {
                    // ��Ҫ��Ӧ�ù���Ա�����ϼ���Ȩ�޲��ܵ��ô˷���
                    return "{\"message\":{\"returnCode\":2,\"value\":\"��" + methodName + "���˷�����Ҫ��Ӧ�ù���Ա�����ϼ���Ȩ�޲��ܵ��ô˷�����\"}}";
                }
            }

            Dictionary<string, string> options = new Dictionary<string, string>();

            JsonObject optionObjects = JsonObjectConverter.Deserialize(param.Options);

            foreach (string key in optionObjects.Keys)
            {
                options.Add(key, ((JsonPrimary)optionObjects[key]).Value.ToString());
            }

            switch (param.Type)
            {
                case "generic":
                    method = new GenericMethod(options, doc);
                    break;

                case "ajax":
                    method = new AjaxMethod(options, doc);
                    break;

                default:
                    break;
            };

            if (method != null)
            {
                return method.Execute().ToString();
            }

            return string.Empty;
        }
        #endregion
    }
}