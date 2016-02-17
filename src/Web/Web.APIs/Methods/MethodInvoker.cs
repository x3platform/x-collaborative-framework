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

    /// <summary>方法执行器</summary>
    public sealed class MethodInvoker
    {
        // -------------------------------------------------------
        // 执行方法
        // -------------------------------------------------------

        #region 属性:Invoke(string methodName, XmlDocument doc)
        /// <summary>执行方法</summary>
        /// <param name="name">方法名称</param>
        /// <param name="doc">Xml 文档元素</param>
        public static string Invoke(string methodName, XmlDocument doc, ILog logger)
        {
            IMethod method = null;

            // 应用方法信息
            ApplicationMethodInfo param = AppsContext.Instance.ApplicationMethodService.FindOneByName(methodName);

            if (param == null)
            {
                logger.Warn("unkown methodName:" + methodName + ", please contact the administrator.");

                throw new GenericException("1", "【" + methodName + "】方法不存在，请联系管理员检查配置信息。");

                // return "{\"message\":{\"returnCode\":1,\"value\":\"【" + methodName + "】方法不存在，请联系管理员检查配置信息。\"}}";
            }

            // 应用方法所属的应用信息
            ApplicationInfo application = param.Application;

            if (param.Status == 0)
            {
                return "{\"message\":{\"returnCode\":1001,\"value\":\"【" + methodName + "】方法 已被禁用。\"}}";
            }

            if (param.EffectScope == 1)
            {
                // 允许所有人可以访问
            }
            else
            {
                // 当前用户信息
                IAccountInfo account = KernelContext.Current == null ? null : KernelContext.Current.User;

                if (param.EffectScope == 2 && account == null)
                {
                    // 需要【登录用户】以上级别权限才能调用此方法
                    return "{\"message\":{\"returnCode\":1002,\"value\":\"【" + methodName + "】此方法需要【登录用户】级别才能调用此方法。\"}}";
                }
                else if (param.EffectScope == 4 && !(AppsSecurity.IsMember(account, application.ApplicationName) || AppsSecurity.IsReviewer(account, application.ApplicationName) || AppsSecurity.IsAdministrator(account, application.ApplicationName)))
                {
                    // 需要【应用可访问成员】以上级别权限才能调用此方法
                    return "{\"message\":{\"returnCode\":1003,\"value\":\"【" + methodName + "】此方法需要【应用可访问成员】以上级别权限才能调用此方法。\"}}";
                }
                else if (param.EffectScope == 8 && !(AppsSecurity.IsReviewer(account, application.ApplicationName) || AppsSecurity.IsAdministrator(account, application.ApplicationName)))
                {
                    // 需要【应用审查员】以上级别权限才能调用此方法
                    return "{\"message\":{\"returnCode\":1004,\"value\":\"【" + methodName + "】此方法需要【应用审查员】以上级别权限才能调用此方法。\"}}";
                }
                else if (param.EffectScope == 16 && !AppsSecurity.IsAdministrator(account, application.ApplicationName))
                {
                    // 需要【应用管理员】以上级别权限才能调用此方法
                    return "{\"message\":{\"returnCode\":1005,\"value\":\"【" + methodName + "】此方法需要【应用管理员】以上级别权限才能调用此方法。\"}}";
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