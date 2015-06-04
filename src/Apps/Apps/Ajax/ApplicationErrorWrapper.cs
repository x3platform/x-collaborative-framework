namespace X3Platform.Connect.Ajax
{
    #region Using Libraries
    using System;
    using System.Text;
    using System.Xml;
    using System.Web;

    using X3Platform.Ajax;
    using X3Platform.Ajax.Json;
    using X3Platform.Ajax.Net;
    using X3Platform.Configuration;
    using X3Platform.DigitalNumber;
    using X3Platform.Membership;
    using X3Platform.Util;
    using X3Platform.Web;

    using X3Platform.Connect.Model;
    using X3Platform.Connect.Configuration;
    using System.IO;
    #endregion

    /// <summary></summary>
    public class ConnectAuthWrapper
    {
        // -------------------------------------------------------
        // 接口地址:/api/connect.auth.authorize.aspx
        // -------------------------------------------------------

        #region 函数:GetAuthorizeCode(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetAuthorizeCode(XmlDocument doc)
        {
            // 地址示例
            // http://local.x3platform.com/api/connect.auth.authorize.aspx?clientId=52cf89ba-7db5-4e64-9c64-3c868b6e7a99
            // &redirectUri=https://x10.x3platform.com/api/connect.oauth.token.aspx
            // &responseType=code
            // &scope=public,news_read,tasks_read

            // http://local.x3platform.com/api/connect.auth.authorize.aspx?client_id=05ce2febad3eeaab116a8fc307bcc001&redirect_uri=https://x10.x3platform.com/api/connect.connect.oauth.token.aspx
            // &response_type=code
            // &scope=oauth,news,tasks

            StringBuilder outString = new StringBuilder();

            string clientId = XmlHelper.Fetch("clientId", doc);
            string redirectUri = XmlHelper.Fetch("redirectUri", doc);
            string responseType = XmlHelper.Fetch("responseType", doc);
            string scope = XmlHelper.Fetch("scope", doc);

            string style = XmlHelper.Fetch("style", doc);

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
                    else if (responseType == "json")
                    {
                        outString.Append("{\"data\":" + AjaxUtil.Parse<ConnectAccessTokenInfo>(ConnectContext.Instance.ConnectAccessTokenService.FindOneByAccountId(clientId, account.Id)) + ",");

                        outString.Append("\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

                        string callback = XmlHelper.Fetch("callback", doc);

                        return string.IsNullOrEmpty(callback) 
                            ? outString.ToString()
                            : callback + "(" + outString.ToString() + ")";
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect(CombineUrlAndAuthorizationCode(redirectUri, code));
                    }
                }
            }

            outString.Append("{\"message\":{\"returnCode\":0,\"value\":\"执行成功。\"}}");

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
                return redirectUri + "?token=" + token.Id + "&expiresIn=" + token.ExpiresIn + "&refreshToken=" + token.RefreshToken;
            }
            else if (redirectUri.IndexOf("?") > -1 && redirectUri.IndexOf("&") == -1)
            {
                return redirectUri + "&token=" + token.Id + "&expiresIn=" + token.ExpiresIn;
            }
            else
            {
                return redirectUri + "&token=" + token.Id + "&expiresIn=" + token.ExpiresIn;
            }
        }
        #endregion

        #region 函数:GetAccessToken(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string GetAccessToken(XmlDocument doc)
        {
            // http://local.passport.x3platform.com/api/connect.auth.token.aspx?code=28f35bf4743030ae

            string code = XmlHelper.Fetch("code", doc);

            ConnectAuthorizationCodeInfo authorizationCodeInfo = ConnectContext.Instance.ConnectAuthorizationCodeService[code];

            if (authorizationCodeInfo == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"authorization code not find\"}}";
            }

            ConnectAccessTokenInfo accessTokenInfo = ConnectContext.Instance.ConnectAccessTokenService.FindOneByAccountId(authorizationCodeInfo.AppKey, authorizationCodeInfo.AccountId);

            if (accessTokenInfo == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"access token not find\"}}";
            }

            StringBuilder outString = new StringBuilder();

            outString.Append("{\"data\":{");
            outString.Append("accessToken:\"" + accessTokenInfo.Id + "\",");
            outString.Append("expiresIn:\"" + accessTokenInfo.ExpiresIn + "\",");
            outString.Append("refreshToken:\"" + accessTokenInfo.RefreshToken + "\" ");
            outString.Append("},\"message\":{\"returnCode\":0,\"value\":\"query success\"}}");

            return outString.ToString();
        }
        #endregion

        #region 函数:Me(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string Me(XmlDocument doc)
        {
            string accessToken = XmlHelper.Fetch("accessToken", doc);

            StringBuilder outString = new StringBuilder();

            ConnectAccessTokenInfo accessTokenInfo = ConnectContext.Instance.ConnectAccessTokenService[accessToken];

            IMemberInfo member = MembershipManagement.Instance.MemberService[accessTokenInfo.AccountId];

            if (member == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"people not find\"}}";
            }

            return "{\"data\":" + ToPeopleJson(member) + ",\"message\":{\"returnCode\":0,\"value\":\"query success\"}}";
        }
        #endregion

        #region 函数:People(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string People(XmlDocument doc)
        {
            // http://x10.x3platform.com/api/connect.oauth.people.aspx?id=${guid}

            string accessToken = XmlHelper.Fetch("accessToken", doc);

            string id = XmlHelper.Fetch("id", doc);

            string loginName = XmlHelper.Fetch("loginName", doc);

            IMemberInfo member = MembershipManagement.Instance.MemberService[id];

            if (member == null)
            {
                return "{\"message\":{\"returnCode\":1,\"value\":\"people not find, args[id:" + id + "]\"}}";
            }

            return "{\"data\":" + ToPeopleJson(member) + ",\"message\":{\"returnCode\":0,\"value\":\"查询成功\"}}";
        }
        #endregion

        #region 私有函数:ToPeopleJson(IMemberInfo member)
        /// <summary>将人员信息格式化为Json格式</summary>
        /// <param name="member"></param>
        private string ToPeopleJson(IMemberInfo member)
        {
            StringBuilder outString = new StringBuilder();

            IAccountInfo account = member.Account;

            string certifiedAvatar = account.CertifiedAvatar;

            certifiedAvatar = certifiedAvatar
                .Replace("{uploads}", KernelConfigurationView.Instance.FileHostName + "/uploads/")
                .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            outString.Append("{");
            outString.Append("id:\"" + StringHelper.ToSafeJson(account.Id) + "\",");
            outString.Append("name:\"" + StringHelper.ToSafeJson(account.Name) + "\",");
            outString.Append("loginName:\"" + StringHelper.ToSafeJson(account.LoginName) + "\",");
            outString.Append("certifiedAvatar:\"" + StringHelper.ToSafeJson(certifiedAvatar) + "\",");
            outString.Append("status:\"" + account.Status + "\"");
            outString.Append("}");

            return outString.ToString();
        }
        #endregion

        #region 函数:SingleSignOn(XmlDocument doc)
        /// <summary>获取详细信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        public string SingleSignOn(XmlDocument doc)
        {
            StringBuilder outString = new StringBuilder();

            // http://x10.x3platform.com/api/connect.auth.singleSignOn.aspx

            AjaxRequestData requestData = new AjaxRequestData();

            string authType = XmlHelper.Fetch("authType", doc);

            switch (authType)
            {
                case "douban":
                    requestData.ActionUri = new Uri("http://api.douban.com/v2/user/~me");
                    break;
            }

            return AjaxRequest.Request(requestData);

            // outString.Append("{\"message\":{\"returnCode\":0,\"value\":\"查询成功。\"}}");

            // return outString.ToString();
        }
        #endregion

        #region 私有函数:RenameXmlElement(XmlDocument doc, string oldXmlNodeName, string newXmlNodeName)
        /// <summary>重命名Xml节点名称</summary>
        /// <param name="doc"></param>
        /// <param name="oldXmlNodeName"></param>
        /// <param name="newXmlNodeName"></param>
        private void RenameXmlElement(XmlDocument doc, string oldXmlNodeName, string newXmlNodeName)
        {
            XmlNode node = doc.DocumentElement.SelectSingleNode(oldXmlNodeName);

            if (node == null) { return; }

            XmlElement element = doc.CreateElement(newXmlNodeName);

            element.InnerText = node.InnerText;

            doc.DocumentElement.AppendChild(element);

            doc.DocumentElement.RemoveChild(node);
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
            outString.AppendLine();
            outString.Append("<head>");
            outString.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\" />");
            outString.Append("<title>" + connect.Name + " 应用接入验证 - " + KernelConfigurationView.Instance.SystemName + "</title>");
            outString.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            outString.Append("<link rel=\"stylesheet\" media=\"all\" href=\"/views/templates/internal/oauth/login.css\" type=\"text/css\" />");
            outString.Append("<link rel=\"shortcut icon\" href=\"/favorite.ico\" />");
            outString.Append("<script id=\"crypto-sha1-script\" type=\"text/javascript\" src=\"/resources/javascript/crypto/rollups/sha1.js\" ></script>");
            outString.Append("</head>");

            outString.Append("<body>");

            outString.Append("<form id=\"auth-form\" name=\"authForm\" method=\"POST\" action=\"/api/connect.auth.authorize.aspx?clientId=" + clientId
                + (string.IsNullOrEmpty(redirectUri) ? "&redirectUri=" + connect.RedirectUri : "&redirectUri=" + redirectUri)
                + (string.IsNullOrEmpty(responseType) ? "&responseType=code" : "&responseType=" + responseType)
                + (string.IsNullOrEmpty(scope) ? string.Empty : "&scope=" + scope) + "\" >");

            outString.Append("<div class=\"window-login-main-wrapper\" style=\"width:100%;\" >");
            outString.Append("<div class=\"window-login-form-wrapper\" style=\"margin:4px auto 0 auto; float:none;\" >");
            outString.Append("<div class=\"window-login-form-container\" >");
            outString.Append("<div class=\"window-login-form-input\" >");
            outString.Append("<span>帐号</span> ");
            outString.Append("<input id=\"loginName\" name=\"loginName\" maxlength=\"20\" type=\"text\" class=\"window-login-input-style\" value=\"\" />");
            outString.Append("</div>");
            outString.Append("<div class=\"window-login-form-input\" >");
            outString.Append("<span>密码</span>");

            outString.Append("<input id=\"originalPassword\" name=\"originalPassword\" maxlength=\"20\" type=\"password\" class=\"window-login-input-style\" value=\"\" />");
            outString.Append("<input id=\"password\" type=\"hidden\" value=\"\" />");
            outString.Append("</div>");
            
            // outString.Append("<div class=\"window-login-form-remember-me\" >");
            // outString.Append("<a href=\"/public/forgot-password.aspx\" target=\"_blank\" >忘记登录密码？</a>");
            // outString.Append("<input id=\"remember\" name=\"remember\" type=\"checkbox\" > <span>记住登录状态</span>");
            // outString.Append("</div>");
            outString.Append("<div class=\"window-login-button-wrapper\" >");
            outString.Append("<div class=\"window-login-button-submit\" ><a id=\"btnSubmit\" href=\"javascript:loginCheck();\" >登录</a></div>");
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
            outString.AppendLine();
            outString.AppendLine("<script type=\"text/javascript\">");
            outString.AppendLine("function loginCheck() {");
            outString.AppendLine("if (document.getElementById('loginName').value == '' || document.getElementById('originalPassword').value == '') {");
            outString.AppendLine("  alert('必须填写帐号和密码。');");
            outString.AppendLine("  return void(0);");
            outString.AppendLine("} ");
            outString.AppendLine();
            outString.AppendLine("document.getElementById('password').value = CryptoJS.SHA1(document.getElementById('originalPassword').value).toString(); ");
            outString.AppendLine();
            outString.AppendLine("document.getElementById('auth-form').submit();");
            outString.AppendLine("} ");
            outString.Append("</script>");

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

            string clientId = XmlHelper.Fetch("client_id", doc);

            string authType = XmlHelper.Fetch("authType", doc);

            authType = string.IsNullOrEmpty(authType) ? "classic" : authType;

            string code = XmlHelper.Fetch("code", doc);
            string grantType = XmlHelper.Fetch("grant_type", doc);

            string token = XmlHelper.Fetch("token", doc);

            ConnectInfo connect = ConnectContext.Instance.ConnectService.FindOneByAppKey(clientId);

            // ConnectAccessTokenInfo accessTokenInfo = null;

            AjaxRequestData requestData = new AjaxRequestData();

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
            else
            {
                // token
            }

            string responseText = AjaxRequest.Request(requestData);

            JsonObject responseObject = JsonObjectConverter.Deserialize(responseText);

            HttpContext.Current.Response.Cookies["connect$token"].Value = token;
            HttpContext.Current.Response.Cookies["connect$authType"].Value = authType;

            requestData.ActionUri = new Uri(ConnectConfigurationView.Instance.ApiHostName + "/api/connect.auth.me.aspx?token=" + token);

            requestData.Args.Clear();

            return AjaxRequest.Request(requestData);
        }
        #endregion
    }
}