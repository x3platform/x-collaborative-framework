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
    using System.Web;

	/// <summary>�ṩ���õĵ�ַ��д������</summary>
	/// <remarks>This class is marked as internal, meaning only classes in the same assembly will be able to access its methods.</remarks>
	internal class RewriterUtils
    {
        #region ��̬属性:RewriteUrl(HttpContext context, string sendToUrl)
        /// <summary>
		/// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
		/// </summary>
		/// <param name="context">The HttpContext object to rewrite the URL to.</param>
		/// <param name="sendToUrl">The URL to rewrite to.</param>
		internal static void RewriteUrl(HttpContext context, string sendToUrl)
		{
			string x, y;

			RewriteUrl(context, sendToUrl, out x, out y);
		}
        #endregion

        #region ��̬属性:RewriteUrl(HttpContext context, string sendToUrl, out string sendToUrlLessQueryString, out string filePath)
        /// <summary>
		/// Rewrite's a URL using <b>HttpContext.RewriteUrl()</b>.
		/// </summary>
		/// <param name="context">The HttpContext object to rewrite the URL to.</param>
		/// <param name="sendToUrl">The URL to rewrite to.</param>
        /// <param name="sendToUrlLessQueryString">Returns the value of sendToUrl stripped of the querystring.</param>
		/// <param name="filePath">Returns the physical file path to the requested page.</param>
        internal static void RewriteUrl(HttpContext context, string sendToUrl, out string sendToUrlLessQueryString, out string filePath)
		{
			// see if we need to add any extra querystring information
			if (context.Request.QueryString.Count > 0)
			{
				if (sendToUrl.IndexOf('?') != -1)
					sendToUrl += "&" + context.Request.QueryString.ToString();
				else
					sendToUrl += "?" + context.Request.QueryString.ToString();
			}

			// first strip the querystring, if any
			string queryString = String.Empty;

            sendToUrlLessQueryString = sendToUrl;

			if (sendToUrl.IndexOf('?') > 0)
			{
                sendToUrlLessQueryString = sendToUrl.Substring(0, sendToUrl.IndexOf('?'));
				queryString = sendToUrl.Substring(sendToUrl.IndexOf('?') + 1);
			}

			// grab the file's physical path
			filePath = string.Empty;

            filePath = context.Server.MapPath(sendToUrlLessQueryString);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                filePath = filePath.Replace("\\", "/");
            }

			// rewrite the path...
            context.RewritePath(sendToUrlLessQueryString, string.Empty, queryString);

			// context.RewritePath(sendToUrl);
		}
		#endregion

        #region ��̬属性:ResolveUrl(string appPath, string url)
        /// <summary>����Url</summary>
		/// <remarks>Converts ~ to the requesting application path.  Mimics the behavior of the 
		/// <b>Control.ResolveUrl()</b> method, which is often used by control developers.</remarks>
		/// <param name="appPath">The application path.</param>
		/// <param name="url">The URL, which might contain ~.</param>
		/// <returns>A resolved URL.  If the input parameter <b>url</b> contains ~, it is replaced with the
		/// value of the <b>appPath</b> parameter.</returns>
		internal static string ResolveUrl(string appPath, string url)
		{
            if (url.Length == 0 || url[0] != '~')
            {
                return url;		// there is no ~ in the first character position, just return the url
            }
            else
            {
                if (url.Length == 1)
                    return appPath;  // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }
        #endregion
    }
}
