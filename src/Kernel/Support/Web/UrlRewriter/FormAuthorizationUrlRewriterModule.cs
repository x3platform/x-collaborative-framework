// =============================================================================
//
// Copyright (c) x3platfrom.com
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
using System.Text.RegularExpressions;
using System.Configuration;
using X3Platform.Web.UrlRewriter.Configuration;

namespace X3Platform.Web.UrlRewriter
{
    /// <summary></summary>
    public class FormAuthorizationUrlRewriterModule : UrlRewriterModule
    {
        /// <summary>��д��ַ</summary>
        /// <param name="requestedRawUrl">�����ĵ�ַ</param>
        /// <param name="app">HttpApplication ʵ��</param>
        protected override void Rewrite(string requestedPath, System.Web.HttpApplication application)
        {
            // ��ȡ��д����������Ϣ
            RewriterRuleCollection rules = RewriterConfigurationView.Instance.Configuration.Rules;

            // iterate through each rule...
            for (int i = 0; i < rules.Count; i++)
            {
                // get the pattern to look for, and Resolve the Url (convert ~ into the appropriate directory)
                string lookfor = "^" + RewriterUtils.ResolveUrl(application.Context.Request.ApplicationPath, rules[i].Lookfor) + "$";

                // Create a regex (note that IgnoreCase is set...)
                Regex re = new Regex(lookfor, RegexOptions.IgnoreCase);

                // See if a match is found
                if (re.IsMatch(requestedPath))
                {
                    // match found - do any replacement needed
                    string sendToUrl = RewriterUtils.ResolveUrl(application.Context.Request.ApplicationPath, re.Replace(requestedPath, rules[i].Sendto));

                    // Rewrite the URL
                    RewriterUtils.RewriteUrl(application.Context, sendToUrl);

                    // exit the for loop
                    break;
                }
            }
        }
    }
}
