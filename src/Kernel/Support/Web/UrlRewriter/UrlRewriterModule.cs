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
using System.Web;
using System.Text.RegularExpressions;

namespace X3Platform.Web.UrlRewriter
{
    /// <summary>��ַ��дģ��</summary>
    /// <remarks></remarks>
    public abstract class UrlRewriterModule : IHttpModule
    {
        /// <summary>��ʼ��ʱִ�еķ���</summary>
        /// <param name="app"></param>
        /// <remarks></remarks>
        public virtual void Init(HttpApplication application)
        {
            // ע��: ���� AuthorizeRequest �� Windows ��֤��ʽ�²�����������
            // ����ʹ�� Windows ��֤��վ��, ��Ϊ app.BeginRequest

            application.AuthorizeRequest += new EventHandler(this.UrlRewriterModule_AuthorizeRequest);
        }

        public virtual void Dispose() { }

        /// <summary>������֤�¼�</summary>
        /// <remarks></remarks>
        protected virtual void UrlRewriterModule_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            string requestedPath = application.Request.Path;

            Rewrite(requestedPath, application);
        }

        /// <summary>��д��ַ</summary>
        /// <param name="requestedRawUrl">�����ĵ�ַ</param>
        /// <param name="app">HttpApplication ʵ��</param>
        protected abstract void Rewrite(string requestedPath, HttpApplication application);
    }
}
