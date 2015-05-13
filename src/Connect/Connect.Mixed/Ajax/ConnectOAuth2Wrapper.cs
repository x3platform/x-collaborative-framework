namespace X3Platform.Connect.Ajax
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Web;

    using X3Platform.Ajax;
    using X3Platform.Ajax.Json;
    using X3Platform.Ajax.Net;
    using X3Platform.Configuration;
    using X3Platform.DigitalNumber;
    using X3Platform.Membership;
    using X3Platform.Sessions;
    using X3Platform.Util;
    using X3Platform.Web;

    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.Model;
    using X3Platform.Connect.Configuration;
    #endregion

    /// <summary></summary>
    public class ConnectOAuth2Wrapper
    {
        // -------------------------------------------------------
        // OAuth 2.0 验证
        // -------------------------------------------------------

        // http://tools.ietf.org/html/draft-ietf-oauth-v2-31

        // -------------------------------------------------------
        // 接口地址:/api/connect.oauth2.authorize.aspx
        // -------------------------------------------------------

        #region 函数:GetAuthorizeCode(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetAuthorizeCode(XmlDocument doc)
        {
            // 地址示例
            // http://x10.x3platform.com/api/connect.connect.oauth.authorize.aspx
            // ?client_id=05ce2febad3eeaab116a8fc307bcc001
            // &redirect_uri=https://x10.x3platform.com/api/connect.connect.oauth.token.aspx
            // &response_type=code
            // &scope=public,news_read,tasks_read

            // http://x10.x3platform.com/api/connect.connect.auth.authorize.aspx?client_id=05ce2febad3eeaab116a8fc307bcc001&redirect_uri=https://x10.x3platform.com/api/connect.connect.oauth.token.aspx
            // &response_type=code
            // &scope=oauth,news,tasks

            StringBuilder outString = new StringBuilder();

            string clientId = XmlHelper.Fetch("clientId", doc);
            string redirectUri = XmlHelper.Fetch("redirect_uri", doc);
            string responseType = XmlHelper.Fetch("response_type", doc);
            string scope = XmlHelper.Fetch("scope", doc);

            string display = XmlHelper.Fetch("display", doc);

            string loginName = XmlHelper.Fetch("loginName", doc);
            string password = XmlHelper.Fetch("password", doc);

            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                HttpContentTypeHelper.SetValue("html");

                return CreateLoginPage(clientId, redirectUri, responseType, scope);
            }
            else
            {
                // 当前用户信息
                IAccountInfo account = MembershipManagement.Instance.AccountService.LoginCheck(loginName, password);

                if (account == null)
                {
                    // 输出登录页面
                    // 设置输出的内容类型，默认为 html 格式。
                    HttpContentTypeHelper.SetValue("html");

                    return CreateLoginPage(clientId, redirectUri, responseType, scope);
                }
                else
                {
                    // 检验是否有授权码
                    if (!ConnectContext.Instance.ConnectAuthorizationCodeService.IsExist(clientId, account.Id))
                    {
                        ConnectAuthorizationCodeInfo authorizationCode = new ConnectAuthorizationCodeInfo();

                        authorizationCode.Id = DigitalNumberContext.Generate("Key_32DigitGuid");
                        authorizationCode.AppKey = clientId;
                        authorizationCode.AccountId = account.Id;

                        authorizationCode.AuthorizationScope = string.IsNullOrEmpty(scope) ? "public" : scope;

                        ConnectContext.Instance.ConnectAuthorizationCodeService.Save(authorizationCode);
                    }

                    // 设置访问令牌
                    ConnectContext.Instance.ConnectAccessTokenService.Write(clientId, account.Id);

                    string code = ConnectContext.Instance.ConnectAuthorizationCodeService.GetAuthorizationCode(clientId, account);

                    if (responseType == "code")
                    {
                        HttpContext.Current.Response.Redirect(CombineUrlAndAuthorizationCode(redirectUri, code));
                    }
                    else if (responseType == "token")
                    {
                        ConnectAccessTokenInfo token = ConnectContext.Instance.ConnectAccessTokenService.FindOneByAccountId(clientId, account.Id);

                        HttpContext.Current.Response.Redirect(CombineUrlAndAccessToken(redirectUri, token));
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(CombineUrlAndAuthorizationCode(redirectUri, code));
                    }
                }
            }

            outString.Append("{\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            return outString.ToString();
        }
        #endregion

        #region 私有函数:CombineUrlAndAuthorizationCode(string redirectUri, string code)
        /// <summary>合并Url地址和授权码</summary>
        private string CombineUrlAndAuthorizationCode(string redirectUri, string code)
        {
            if (redirectUri.IndexOf("?") == -1 && redirectUri.IndexOf("&") == -1)
            {
                return redirectUri + "?code=" + code;
            }
            else if (redirectUri.IndexOf("?") > -1 && redirectUri.IndexOf("&") == -1)
            {
                return redirectUri + "&code=" + code;
            }
            else
            {
                return redirectUri + "&code=" + code;
            }
        }
        #endregion

        #region 私有函数:CombineUrlAndAccessToken(string redirectUri, ConnectAccessTokenInfo token)
        /// <summary>合并Url地址和访问令牌</summary>
        private string CombineUrlAndAccessToken(string redirectUri, ConnectAccessTokenInfo token)
        {
            if (redirectUri == null) { redirectUri = string.Empty; }

            if (redirectUri.IndexOf("?") == -1 && redirectUri.IndexOf("&") == -1)
            {
                return redirectUri + "?token=" + token.Id + "&expires_in=" + token.ExpiresIn + "&refresh_token=" + token.RefreshToken;
            }
            else if (redirectUri.IndexOf("?") > -1 && redirectUri.IndexOf("&") == -1)
            {
                return redirectUri + "&token=" + token.Id + "&expires_in=" + token.ExpiresIn;
            }
            else
            {
                return redirectUri + "&token=" + token.Id + "&expires_in=" + token.ExpiresIn;
            }
        }
        #endregion

        // -------------------------------------------------------
        // 接口地址:/api/connect.oauth2.token.aspx
        // -------------------------------------------------------

        #region 函数:GetAccessToken(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetAccessToken(XmlDocument doc)
        {
            // http://x10.x3platform.com/api/connect.oauth2.token.aspx?code=28f35bf4743030ae

            string code = XmlHelper.Fetch("code", doc);

            ConnectAuthorizationCodeInfo authorizationCodeInfo = ConnectContext.Instance.ConnectAuthorizationCodeService[code];

            if (authorizationCodeInfo == null)
            {
                return "{error:1,descriptiopn:\"not find\"}";
            }

            ConnectAccessTokenInfo accessTokenInfo = ConnectContext.Instance.ConnectAccessTokenService.FindOneByAccountId(authorizationCodeInfo.AppKey, authorizationCodeInfo.AccountId);

            if (accessTokenInfo == null)
            {
                return "{error:1,descriptiopn:\"not find\"}";
            }

            StringBuilder outString = new StringBuilder();

            outString.Append("{");
            outString.Append("access_token:\"" + accessTokenInfo.Id + "\",");
            outString.Append("expires_in:\"" + accessTokenInfo.ExpiresIn + "\",");
            outString.Append("refresh_token:\"" + accessTokenInfo.RefreshToken + "\" ");
            outString.Append("}");

            return outString.ToString();
        }
        #endregion

        #region 私有函数:CreateLoginPage(string clientId, string redirectUri, string responseType, string scope)
        /// <summary>获取登录页面信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        private string CreateLoginPage(string clientId, string redirectUri, string responseType, string scope)
        {
            // 测试地址
            // http://x10.x3platform.com/api/connect.oauth2.authorize.aspx?client_id=a70633f6-b37a-4e91-97a0-597d708fdcef&redirect_uri=https://x10.x3platform.com/api/connect.auth.back.aspx%3fclient_id%3da70633f6-b37a-4e91-97a0-597d708fdcef&response_type=code
            // http://x10.x3platform.com/api/connect.oauth2.authorize.aspx?client_id=a70633f6-b37a-4e91-97a0-597d708fdcef&redirect_uri=https://x10.x3platform.com/api/connect.auth.back.aspx%3fclient_id%3da70633f6-b37a-4e91-97a0-597d708fdcef&response_type=token

            StringBuilder outString = new StringBuilder();

            ConnectInfo connect = ConnectContext.Instance.ConnectService[clientId];

            outString.AppendLine("<!DOCTYPE HTML>");

            outString.Append("<html>");
            outString.Append("<head>");
            outString.Append("<title>" + connect.Name + "需要接入验证 - " + KernelConfigurationView.Instance.SystemName + "</title>");
            outString.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            outString.Append("<link rel=\"stylesheet\" media=\"all\" href=\"/resources/styles/default/login.css\" type=\"text/css\" />");
            outString.Append("<link rel=\"shortcut icon\" href=\"/favorite.ico\" />");
            outString.Append("</head>");

            outString.Append("<body>");

            outString.Append("<form id=\"authForm\" name=\"authForm\" method=\"POST\" action=\"/api/connect.auth.authorize.aspx?clientId=" + clientId
                + (string.IsNullOrEmpty(redirectUri) ? "&redirectUri=" + connect.RedirectUri : "&redirectUri=" + redirectUri)
                + (string.IsNullOrEmpty(responseType) ? "&responseType=code" : "&responseType=" + responseType)
                + (string.IsNullOrEmpty(scope) ? string.Empty : "&scope=" + scope) + "\" >");

            outString.Append("<input id=\"xml\" name=\"xml\" type=\"hidden\" value=\"\" />");

            outString.Append("<div class=\"window-login-main-wrapper\" style=\"width:100%;\" >");
            outString.Append("<div class=\"window-login-form-wrapper\" style=\"margin:4px auto 0 auto; float:none;\" >");
            outString.Append("<div class=\"window-login-form-container\" >");
            outString.Append("<div class=\"window-login-form-input\" >");
            outString.Append("<span>帐号</span> ");
            outString.Append("<input id=\"loginName\" maxlength=\"20\" type=\"text\" class=\"window-login-input-style\" value=\"\" />");
            outString.Append("</div>");
            outString.Append("<div class=\"window-login-form-input\" >");
            outString.Append("<span>密码</span>");
            outString.Append("<input id=\"password\" maxlength=\"20\" type=\"password\" class=\"window-login-input-style\" value=\"\" />");
            outString.Append("</div>");

            // outString.Append("<div class=\"window-login-form-remember-me\" >");
            // outString.Append("<a href=\"/public/forgot-password.aspx\" target=\"_blank\" >忘记登录密码？</a>");
            // outString.Append("<input id=\"remember\" name=\"remember\" type=\"checkbox\" > <span>记住登录状态</span>");
            // outString.Append("</div>");
            outString.Append("<div class=\"window-login-button-wrapper\" >");
            outString.Append("<div class=\"window-login-button-submit\" ><a id=\"btnSubmit\" href=\"javascript:if (document.getElementById('loginName').value == '' || document.getElementById('password').value == '') {alert('必须填写帐号和密码。'); return;}; document.getElementById('xml').value = '<?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?><root><loginName><![CDATA[' + document.getElementById('loginName').value + ']]></loginName><password><![CDATA[' + document.getElementById('password').value + ']]></password></root>'; document.getElementById('authForm').submit();\" >登录</a></div>");
            // outString.Append("<div class=\"window-login-loading\" style=\"display:none;\" ><img src=\"/resources/images/loading.gif\" alt=\"登录中\" /></div>");
            outString.Append("</div>");
            // outString.Append("<div class=\"window-login-form-bottom\" >");
            // outString.Append("<a href=\"#\" >注册新帐号</a>");
            // outString.Append("</div>");
            ;

            outString.Append("</div>");
            outString.Append("</div>");
            outString.Append("</div>");

            outString.Append("</form>");
            outString.Append("</body>");
            outString.Append("</html>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Callback(XmlDocument doc)
        /// <summary>验证后的回调页面</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Callback(XmlDocument doc)
        {
            // https://x10.x3platform.com/api/connect.auth.callback.aspx?clientId=a70633f6-b37a-4e91-97a0-597d708fdcef&code=75266c29f9e3497480e5ddc6cfa38b8c;
            // https://x10.x3platform.com/api/connect.auth.callback.aspx?clientId=a70633f6-b37a-4e91-97a0-597d708fdcef&code=75266c29f9e3497480e5ddc6cfa38b8c;

            string clientId = XmlHelper.Fetch("clientId", doc);

            string code = XmlHelper.Fetch("code", doc);
            string grantType = XmlHelper.Fetch("grant_type", doc);

            string token = XmlHelper.Fetch("token", doc);

            ConnectInfo connect = ConnectContext.Instance.ConnectService.FindOneByAppKey(clientId);

            AjaxRequestData requestData = new AjaxRequestData();

            if (string.IsNullOrEmpty(token))
            {
                if (!string.IsNullOrEmpty(code) && grantType == "authorization_code")
                {
                    // code => token
                    requestData.ActionUri = new Uri(ConnectConfigurationView.Instance.ApiHostName + "/api/connect.oauth2.token.aspx?code=" + code);

                    requestData.Args.Add("client_id", clientId);
                    requestData.Args.Add("client_secret", connect.AppSecret);
                    requestData.Args.Add("grant_type", "authorization_code");
                }
                else if (!string.IsNullOrEmpty(code) && grantType == "refresh_token")
                {
                    requestData.ActionUri = new Uri(ConnectConfigurationView.Instance.ApiHostName + "/api/connect.oauth2.token.aspx?code=" + code);

                    requestData.Args.Add("client_id", clientId);
                    requestData.Args.Add("client_secret", connect.AppSecret);
                    requestData.Args.Add("grant_type", "refresh_token");
                }

                string responseText = AjaxRequest.Request(requestData);

                JsonObject responseObject = JsonObjectConverter.Deserialize(responseText);

                token = ((JsonPrimary)responseObject["access_token"]).Value.ToString();
            }

            HttpContext.Current.Response.Cookies["connect$token"].Value = token;
            HttpContext.Current.Response.Cookies["connect$authType"].Value = "oauth2";

            requestData.ActionUri = new Uri(ConnectConfigurationView.Instance.ApiHostName + "/api/connect.auth.me.aspx?access_token=" + token);

            requestData.Args.Clear();

            return AjaxRequest.Request(requestData);
        }
        #endregion
    }
}