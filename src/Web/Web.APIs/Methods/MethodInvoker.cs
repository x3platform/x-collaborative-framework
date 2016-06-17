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
    using X3Platform.Web.APIs.Configuration;
    using System.Text;
    using X3Platform.Util;
    using Globalization;
    #endregion

    /// <summary>方法执行器</summary>
    public sealed class MethodInvoker
    {
        // -------------------------------------------------------
        // 执行方法
        // -------------------------------------------------------

        #region 属性:Invoke(string methodName, XmlDocument doc)
        /// <summary>执行方法</summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="doc">Xml 文档元素</param>
        /// <param name="logger">日志对象</param>
        public static string Invoke(string methodName, XmlDocument doc, ILog logger)
        {
            IMethod method = null;

            // 应用方法信息
            ApplicationMethodInfo param = AppsContext.Instance.ApplicationMethodService.FindOneByName(methodName);

            if (param == null)
            {
                logger.Warn(string.Format(I18n.Exceptions["text_web_api_method_not_exists"], methodName));

                throw new GenericException(I18n.Exceptions["code_web_api_method_not_exists"],
                   string.Format(I18n.Exceptions["text_web_api_method_not_exists"], methodName));
            }

            // 应用方法所属的应用信息
            ApplicationInfo application = param.Application;

            if (param.Status == 0)
            {
                throw new GenericException(I18n.Exceptions["code_web_api_method_is_disabled"],
                   string.Format(I18n.Exceptions["text_web_api_method_is_disabled"], methodName));
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
                    throw new GenericException(I18n.Exceptions["code_web_api_method_need_elevated_privileges"],
                       string.Format(I18n.Exceptions["text_web_api_method_need_elevated_privileges"], methodName, "登录用户"));
                }
                else if (param.EffectScope == 4 && !(AppsSecurity.IsMember(account, application.ApplicationName) || AppsSecurity.IsReviewer(account, application.ApplicationName) || AppsSecurity.IsAdministrator(account, application.ApplicationName)))
                {
                    // 需要【应用可访问成员】以上级别权限才能调用此方法
                    throw new GenericException(I18n.Exceptions["code_web_api_method_need_elevated_privileges"],
                       string.Format(I18n.Exceptions["text_web_api_method_need_elevated_privileges"], methodName, "应用可访问成员"));
                }
                else if (param.EffectScope == 8 && !(AppsSecurity.IsReviewer(account, application.ApplicationName) || AppsSecurity.IsAdministrator(account, application.ApplicationName)))
                {
                    // 需要【应用审查员】以上级别权限才能调用此方法
                    throw new GenericException(I18n.Exceptions["code_web_api_method_need_elevated_privileges"],
                       string.Format(I18n.Exceptions["text_web_api_method_need_elevated_privileges"], methodName, "应用审查员"));
                }
                else if (param.EffectScope == 16 && !AppsSecurity.IsAdministrator(account, application.ApplicationName))
                {
                    // 需要【应用管理员】以上级别权限才能调用此方法
                    throw new GenericException(I18n.Exceptions["code_web_api_method_need_elevated_privileges"],
                       string.Format(I18n.Exceptions["text_web_api_method_need_elevated_privileges"], methodName, "应用管理员"));
                }
            }

            Dictionary<string, string> options = new Dictionary<string, string>();

            JsonObject optionObjects = JsonObjectConverter.Deserialize(param.Options);

            foreach (string key in optionObjects.Keys)
            {
                if (optionObjects[key] is JsonPrimary)
                {
                    options.Add(key, ((JsonPrimary)optionObjects[key]).Value.ToString());
                }
                else if (optionObjects[key] is JsonObject)
                {
                    StringBuilder outString = new StringBuilder();

                    JsonObject obj = (JsonObject)optionObjects[key];

                    outString.Append("{");

                    foreach (string objkey in obj.Keys)
                    {
                        outString.AppendFormat("\"{0}\":\"{1}\",", objkey, ((JsonPrimary)obj[objkey]).Value.ToString());
                    }

                    outString = StringHelper.TrimEnd(outString, ",");

                    outString.Append("}");

                    options.Add(key, outString.ToString());
                }
                else if (optionObjects[key] is JsonArray)
                {
                    StringBuilder outString = new StringBuilder();

                    JsonArray list = (JsonArray)optionObjects[key];

                    outString.Append("[");

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] is String || list[i] is DateTime)
                        {
                            outString.AppendFormat("\"{0}\",", list[i].ToString());
                        }
                        else
                        {
                            outString.AppendFormat("{0},", list[i].ToString());
                        }
                    }

                    outString = StringHelper.TrimEnd(outString, ",");

                    outString.Append("]");

                    options.Add(key, outString.ToString());
                }
            }

            IDictionary<string, string> types = APIsConfigurationView.Instance.Configuration.APIMethodTypes;

            if (param.Type == "generic")
            {
                method = new GenericMethod(options, doc);
            }
            else if (types.ContainsKey(param.Type))
            {
                method = (IMethod)KernelContext.CreateObject(types[param.Type], options, doc); ;
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