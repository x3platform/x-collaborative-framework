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

namespace X3Platform.Web.UrlRewriter
{
    using System;
    using System.IO;
    using System.Web.UI;
    using System.Web;
    using System.Text.RegularExpressions;

    using X3Platform.Web.UrlRewriter.Configuration;
    using Common.Logging;

    /// <summary>
    /// Provides an HttpHandler that performs redirection.
    /// </summary>
    /// <remarks>The RewriterFactoryHandler checks the rewriting rules, rewrites the path if needed, and then
    /// delegates the responsibility of processing the ASP.NET page to the <b>PageParser</b> class (the same one
    /// used by the <b>PageHandlerFactory</b> class).</remarks>
    public class RewriterFactoryHandler : IHttpHandlerFactory
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetHandler is executed by the ASP.NET pipeline after the associated HttpModules have run.  The job of
        /// GetHandler is to return an instance of an HttpHandler that can process the page.
        /// </summary>
        /// <param name="context">The HttpContext for this request.</param>
        /// <param name="requestType">The HTTP data transfer method (<b>GET</b> or <b>POST</b>)</param>
        /// <param name="url">The RawUrl of the requested resource.</param>
        /// <param name="pathTranslated">The physical path to the requested resource.</param>
        /// <returns>An instance that implements IHttpHandler; specifically, an HttpHandler instance returned
        /// by the <b>PageParser</b> class, which is the same class that the default ASP.NET PageHandlerFactory delegates
        /// to.</returns>
        public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            // log info to the Trace object...
            // context.Trace.Write("RewriterFactoryHandler", "Entering RewriterFactoryHandler");
            // context.Response.Write("RewriterFactoryHandler" + " / " + "Entering RewriterFactoryHandler");

            string sendToUrl = url;

            string filePath = pathTranslated;

            // get the configuration rules
            RewriterRuleCollection rules = RewriterConfigurationView.Instance.Configuration.Rules;

            // iterate through the rules
            for (int i = 0; i < rules.Count; i++)
            {
                // Get the pattern to look for (and resolve its URL)
                string lookfor = string.Format("^{0}$", RewriterUtils.ResolveUrl(context.Request.ApplicationPath, rules[i].Lookfor));

                // Create a regular expression object that ignores case...
                Regex re = new Regex(lookfor, RegexOptions.IgnoreCase);

                // Check to see if we've found a match
                if (re.IsMatch(url))
                {

                    // do any replacement needed
                    sendToUrl = RewriterUtils.ResolveUrl(context.Request.ApplicationPath, re.Replace(url, rules[i].Sendto));

                    //
                    // ��ǿ��ͨ��CustomServicesֱ�Ӷ�̬����Page����֧��.
                    //
                    if (sendToUrl.ToLower().IndexOf("/services/default.aspx") == -1
                        && sendToUrl.ToLower().IndexOf("/services/") == 0)
                    {
                        return GetHandler(context, requestType, sendToUrl, pathTranslated);
                    }

                    // log info to the Trace object...
                    // context.Trace.Write("RewriterFactoryHandler", "Found match, rewriting to " + sendToUrl);

                    // Rewrite the path, getting the querystring-less url and the physical file path
                    string sendToUrlLessQueryString;

                    RewriterUtils.RewriteUrl(context, sendToUrl, out sendToUrlLessQueryString, out filePath);

                    // return a compiled version of the page
                    // context.Trace.Write("RewriterFactoryHandler", "Exiting RewriterFactoryHandler");	// log info to the Trace object...
                    logger.Trace("sendToUrlLessQueryString:" + sendToUrlLessQueryString + ",filePath:" + filePath);

                    return PageParser.GetCompiledPageInstance(sendToUrlLessQueryString, filePath, context);
                }
            }

            // if we reached this point, we didn't find a rewrite match
            // context.Trace.Write("RewriterFactoryHandler", "Exiting RewriterFactoryHandler");	// log info to the Trace object...
            return PageParser.GetCompiledPageInstance(url, filePath, context);
        }

        public virtual void ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
