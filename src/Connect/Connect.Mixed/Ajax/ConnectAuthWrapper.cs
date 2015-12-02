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
    using X3Platform.Membership.Authentication;
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
                    if (responseType == "json")
                    {
                        outString.Append("{\"message\":{\"returnCode\":1,\"value\":\"帐号或者密码错误。\"}}");

                        return outString.ToString();
                    }
                    else
                    {
                        // 输出登录页面
                        // 设置输出的内容类型，默认为 html 格式。
                        HttpContentTypeHelper.SetValue("html");

                        return CreateLoginPage(clientId, redirectUri, responseType, scope);
                    }
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

                    // 设置会话信息
                    ConnectAccessTokenInfo token = ConnectContext.Instance.ConnectAccessTokenService.FindOneByAccountId(clientId, account.Id);

                    // 记录日志
                    MembershipManagement.Instance.AccountLogService.Log(account.Id, "connect.auth.authorize", string.Format("【{0}】在 {1} 登录了系统。【IP:{2}】", account.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), account.IP));

                    string sessionId = token.AccountId + "-" + token.Id;

                    KernelContext.Current.AuthenticationManagement.AddSession(clientId, sessionId, account);

                    HttpAuthenticationCookieSetter.SetUserCookies(sessionId);

                    string code = ConnectContext.Instance.ConnectAuthorizationCodeService.GetAuthorizationCode(clientId, account);

                    if (responseType == "code")
                    {
                        HttpContext.Current.Response.Redirect(CombineUrlAndAuthorizationCode(redirectUri, code));
                    }
                    else if (responseType == "token")
                    {
                        HttpContext.Current.Response.Redirect(CombineUrlAndAccessToken(redirectUri, token));
                    }
                    else if (responseType == "json")
                    {
                        outString.Append("{\"data\":" + AjaxUtil.Parse<ConnectAccessTokenInfo>(token) + ",");

                        outString.Append("\"message\":{\"returnCode\":0,\"value\":\"验证成功。\"}}");

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
            outString.Append("<input id=\"originalPassword\" maxlength=\"20\" type=\"password\" class=\"window-login-input-style\" value=\"\" />");
            outString.Append("<input id=\"password\" name=\"password\" type=\"hidden\" value=\"\" />");
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

        /*
        // -------------------------------------------------------
        // 登录页面
        // -------------------------------------------------------

        #region 函数:GetLoginPage(XmlDocument doc)
        /// <summary>获取登录页面信息</summary>
        /// <param name="doc">Xml 文档对象</param>
        /// <returns>返回操作结果</returns>
        private string CreateClassicLoginPage(XmlDocument doc)
        {
            //
            // http://x10.x3platform.com/api/connect.oauth2.login.aspx?client_id=a70633f6-b37a-4e91-97a0-597d708fdcef&client_secret=dab4dc97&redirect_uri=https://www.x3platform.com/back&response_type=code
            // http://x10.x3platform.com/api/connect.oauth2.authorize.aspx?client_id=a70633f6-b37a-4e91-97a0-597d708fdcef&client_secret=dab4dc97&redirect_uri=https://www.x3platform.com/back&response_type=code
            //

            StringBuilder outString = new StringBuilder();

            outString.AppendLine("<!DOCTYPE HTML>");

            outString.Append("<html>");
            outString.Append("<head>");
            outString.Append("<title>应用接入验证 - " + KernelConfigurationView.Instance.SystemName + "</title>");
            outString.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            outString.Append("<link rel=\"stylesheet\" media=\"all\" href=\"/resources/styles/default/login.css\" type=\"text/css\" />");
            outString.Append("<link rel=\"shortcut icon\" href=\"/favorite.ico\" />");
            outString.Append("</head>");

            outString.Append("<body>");

            outString.Append("<form id=\"form1\" action=\"/api/connect.oauth2.authorize.aspx\" >");

            outString.Append("<input id=\"clientId\" name=\"clientId\" type=\"hidden\" value=\"\" />");
            outString.Append("<input id=\"redirectUri\" name=\"redirectUri\" type=\"hidden\" value=\"\" />");

            outString.Append("<div class=\"window-login-main-wrapper\" style=\"width:100%;\" >");
            outString.Append("<div class=\"window-login-form-wrapper\" style=\"margin:4px auto 0 auto; float:none;\" >");
            outString.Append("<div class=\"window-login-form-container\" >");
            outString.Append("<div class=\"window-login-form-input\" >");
            outString.Append("<span>帐号</span> ");
            outString.Append("<input id=\"loginName\" maxlength=\"20\" name=\"loginName\" type=\"text\" class=\"window-login-input-style\" value=\"\" />");
            outString.Append("</div>");
            outString.Append("<div class=\"window-login-form-input\" >");
            outString.Append("<span>密码</span>");
            outString.Append("<input id=\"password\" maxlength=\"20\" name=\"password\" type=\"password\" class=\"window-login-input-style\" value=\"\" />");
            outString.Append("</div>");

            // outString.Append("<div class=\"window-login-form-remember-me\" >");
            // outString.Append("<a href=\"/public/forgot-password.aspx\" target=\"_blank\" >忘记登录密码？</a>");
            // outString.Append("<input id=\"remember\" name=\"remember\" type=\"checkbox\" > <span>记住登录状态</span>");
            // outString.Append("</div>");
            outString.Append("<div class=\"window-login-button-wrapper\" >");
            outString.Append("<div class=\"window-login-button-submit\" ><a id=\"btnSubmit\" href=\"javascript:if (document.getElementById('loginName').value == '' || document.getElementById('password').value == '') {alert('必须填写帐号和密码。'); return;}; document.getElementById('form1').submit();\" >登录</a></div>");
            // outString.Append("<div class=\"window-login-loading\" style=\"display:none;\" ><img src=\"/resources/images/loading.gif\" alt=\"登录中\" /></div>");
            outString.Append("</div>");
            // outString.Append("<div class=\"window-login-form-bottom\" >");
            // outString.Append("<a href=\"#\" >注册新帐号</a>");
            // outString.Append("</div>");

            outString.Append("</div>");
            outString.Append("</div>");
            outString.Append("</div>");

            outString.Append("</form>");
            outString.Append("</body>");
            outString.Append("</html>");

            return outString.ToString();
        }
        #endregion

        /// <summary></summary>
        private static string CreateDefaultLoginPage(string clientId, string redirectUri, string responseType, string scope)
        {
            // 示例: /api/connect.oauth.authorize.aspx?clientId=CLIENT_ID&redirectUri=REDIRECT_URI&responseType=RESPONSE_TYPE&scope=SCOPE

            StringBuilder outString = new StringBuilder();

            outString.AppendLine("<!DOCTYPE html>");
            outString.AppendLine("<html>");

            outString.AppendLine("<head>");
            outString.AppendLine("<meta charset=\"utf-8\" >");
            outString.AppendLine("<title>登录 - " + KernelConfigurationView.Instance.SystemName + "</title>");
            outString.AppendLine("<meta name=\"viewport\" content=\"width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1\" >");
            outString.AppendLine("<style type=\"text/css\" >");
            //      body,h1,p { padding:0;margin:0; }
            //      body { font:normal 12px/1.4 arial,sans-serif;color:#333;background:#eaeff3; }
            //      em { font-style:normal; }
            //      p em { color:#188414; }
            //      a { text-decoration:none; }
            //      a:link { color:#369; }
            //      a:visited { color:#669; }
            //      a:hover { color:#fff;background:#039; }
            //      a:active { color:#fff;background:#f93; }
            //      .window-authorization-form-header { padding:20px;border-top:30px solid #ebf5ea;background:#fff; }
            //      .window-authorization-form-content { background:#fff; }
            //      .intro { padding:0 20px;margin-bottom:20px; }
            //      .item { padding-left:20px;margin-bottom:12px; }
            //      .item input { padding:5px;width:12em;font-size:14px;border:1px solid #ccc;-webkit-appearance:none;-webkit-border-radius:3px;border-radius:3px; }
            //      .item label { float:left;width:40px;line-height:2.8; }
            //      .item:after { content:'\0020';display:block;clear:both; } 
            //      .submit { padding-left:60px;margin-top:20px;background:#eaeff3;border-top:1px solid #d9e2e9;padding-top:20px;  }
            //      .submit input { padding:5px 20px;margin-right:20px;-webkit-appearance:none;font-size:13px;-webkit-border-radius:3px;border-radius:3px; }
            //      .submit input[type=submit] { background:#3da247;border:1px solid #538730;color:#fff;font-weight:800;  }
            //      .submit input[type=button] {
            //        border:1px solid #b0b0b0;
            //        background: -moz-linear-gradient(top, #fff 0%, #e2e2e2 100%); /* firefox */
        //        background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#fff), color-stop(100%,#e2e2e2)); /* webkit */
        //        background: -o-linear-gradient(top, #fff 0%,#e2e2e2 100%); /* opera */
        //      }
        //        .window-authorization-form-logo { width:129px;height:25px;overflow:hidden;line-height:10em;background:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIEAAAAZCAMAAAAhS71MAAAAwFBMVEVuxuP/1rLq9vaFxpeSzKI0qFn/yJTn8+r/wYhErWRbtnVlun3U6tr/5c+Q0+n/0qqk2u4qpVL9/v1Svd95wo01tdvD48wZoUgApNO53cLE5/TK5dFuvYS24fHz+fWl1LEAqdUio06x2rz/3cDc7uGo1rRKr2gTrdes2Lj6/Pvw+f0Amjif0q35/P7g8/oLn0LR7PX/v4NSs28LrNf/u3vA4cn4+/n/+vcAoNH/zJ4jsdn/7uA+rGAMrdj////////Oz0I9AAAAQHRSTlP///////////////////////////////////////////////////////////////////////////////////8AwnuxRAAABF9JREFUeNq1lotyqkgQhtFhQAY0DqJCIAgKCBGNkouXsPT7v9XORY6anKpkU5vfitPKXL783dOJAn+Th9oghLSEX5WiE/NWOj/4qQF7kjr1dA7zGD5If/XhCyWv1rcJkjq7VQIhQtgp1RhXyExURJFawLXUVW1/sS1aZc13CeB2Jv+YMFuI46gTp2taxAyIGcK1ZpR8tb1KTfg2ARR6txt22Q976eKsfAKTSa76QVJBroM/g98lcCidnhXLhaYDDvIQI8gaogIyf5kATVPvrLlcmFdQ+XPkmzbzgEA9+W2Cp0td0QCYrCrETS4IWJwYBdxo/v8TXE6IOUFi1CRGM0bQkLzCFc4skCo8h8msJUEzVEajQRe4+scFwDnqtASJGqB2KdgPykjoERbHPvv83rlb7CVB7OuJrif8JTwIVadGXskIgMxnCKuqDkIpZpUS09oQBMPocHDdgyua2uLlyAcRaZKgCMRsFYSGSzab6dndwN3LAt61l95u1+uLSpxiTIUwnhLg8jEA84CA4YNF2yShmKKEASLMCQauq3ShYRwjfm7vriXoHTmBk2TTmd0gTHMAMXt0D7AeRFEX7nb99xetD02np+0ZwessLfMZU1qm+Zy7W5Q1KhEnIHPwaReEcorn54ARbF13AFz3EQs+EWBSUZGAAPPe9OAKq3jACbS73tHmJ2m7vvjecUCoNMNzzzOMNFAtAkglmWFU3PSwxujqLozccVtK7tK+JtgJgoy+ApdH+VUauyNoCTaMQNOEscyNhSI4V2J2UVNBYL2aJgDxLJKqTo5qNZXn1m8XgsdnaYE0YcjO/eABPuc/qXHKLbiHKw962jtwsagjvckq8bvH7cU3VLCxbZlqmUESy6tHsAMXArbTPUg1/0TK5yww34TCjJbMppN9Q9CBC4E/y3MvoGqep3WW53npA6RPFnjYDzD2cGjFwhjbwOqFAIbu8wbOOn0muPSDsKIzkYQbgsUVgbOaxjE2aDyldU3jeOWAzZebqKw8VTV160kQJAbNQSoVBMs/BONo9FeCNgueYLzUwQeCIizeQqY3+R52C/F/CVgWX+7b4OOWYA5S6pdZkARtJer28g/B8LMH8E11DZqefTVq0mx4+UltbyrR1rQbAoQzaE6RzAKLnqPNDwmarL2MATZIc5XagRs9Sg/knjcECebgo2jc3tzTjwnAwfIvFKKEEfCEPkhznnmzee9povg7veM1gU3EpVDOORscxssfZwESioMG9GBV5qIrK4doyIbt6XBiw77Hd90fXxZ9WQeiufmE1gkb11E0foRGOSjbKNr+lABySjMSxyWkcdUIRw+n8fhwGK2Bqd/baccd21r2RGoYhFTxtErkFYjcaBwdFMbi3vNO2BLs/gsBWI5RlSHADJvC8e1guVwqDyD1znrtYt/eBaOqssxwvKYtZGV5UjbMjXE0hM4PPJBqikIMbwVI2fa6uTy1bTHs2WC/2UxFc7V2bYtxzcZm3z5g0b9PYFa2psIX4gAAAABJRU5ErkJggg==) no-repeat; }
        //        .accounts { color:#666;margin:0 0 60px 20px; }
        //        .error { padding-left:60px;margin-bottom:10px;color:#d92616; }
        /*    
        outString.AppendLine("</style>");
            outString.AppendLine("</head>");

            outString.AppendLine("<body>");
            // window-authorization-form-wrapper
            outString.AppendLine("<div class=\"window-authorization-form-wrapper\" >");

            outString.AppendLine("<div class=\"window-authorization-form-header\" >");
            outString.AppendLine("<div class=\"window-authorization-form-logo\">" + KernelConfigurationView.Instance.SystemName + "</div>");
            outString.AppendLine("</div>");

            outString.AppendLine("<div class=\"window-authorization-form-content\" >");
            outString.AppendLine("<p class=\"intro\" >");
            outString.AppendLine("第三方应用 <b>x3platform.com</b> 请求你的授权。");

            outString.AppendLine("<ul>");
            outString.AppendLine("<em><li>获得你的个人信息;</li></em>");
            outString.AppendLine("<em><li>" + KernelConfigurationView.Instance.SystemName + "公共API;</li></em>");
            outString.AppendLine("</ul>");
            outString.AppendLine("</p>");

            outString.AppendLine("<p class=\"intro\"><a href=\"" + KernelConfigurationView.Instance.HostName + "\">" + KernelConfigurationView.Instance.SystemName + "开放平台服务条款</a></p>");

            //      <div class="error"></div>
            // 拒绝表单
            //    <form id="refuse_form" method="post" action="?client_id=06263a9a55201ac11272afceff605043&amp;redirect_uri=http://x10.x3platform.com/api/connect.oauth2.token.aspx\xxdd=dd?&amp;response_type=code">
            //        <input type="hidden" name="refuse" value="1">
            //    </form>

            // 授权表单
            //    <form method="post" action="?client_id=06263a9a55201ac11272afceff605043&amp;redirect_uri=http://x10.x3platform.com/api/connect.oauth2.token.aspx\xxdd=dd?&amp;response_type=code">
            outString.AppendLine("<div class=\"item\">");
            //       
            outString.AppendLine("<label for=\"inp-email\">邮箱</label>");
            outString.AppendLine("<input id=\"inp-email\" name=\"user_email\" type=\"email\" size=\"24\" maxlength=\"60\" value=\"\" >");
            outString.AppendLine("</div>");
            outString.AppendLine("<div class=\"item\">");
            outString.AppendLine("<label for=\"inp-pwd\">密码</label>");
            outString.AppendLine("<input id=\"inp-pwd\" name=\"user_passwd\" type=\"password\" size=\"10\" maxlength=\"20\" value=\"\" >");
            outString.AppendLine("</div>");
            outString.AppendLine("<div class=\"item\">没有帐号？<a href=\"http://www.douban.com/accounts/register\" >注册新的帐号</a></div>");
            outString.AppendLine("<div class=\"submit\">");
            outString.AppendLine("<input type=\"submit\" name=\"confirm\" value=\"授权\" >");
            outString.AppendLine("<input type=\"button\" value=\"拒绝\" onclick=\"javascript:document.forms['refuse_form'].submit();\" >");
            outString.AppendLine("</div>");
            outString.AppendLine("</form>");
            outString.AppendLine("</div>");

            outString.AppendLine("</div>");

            //<script>var _check_hijack = function () {
            //            var _sig = "8JtthuGS", _login = false, bid = get_cookie('bid');
            //            if (location.protocol != "file:" && (typeof(bid) != "string" && _login || typeof(bid) == "string" && bid.substring(0,8) != _sig)) {
            //                location.href+=(/\?/.test(location.href)?"&":"?") + "_r=" + Math.random().toString(16).substring(2);
            //            }};
            //            if (typeof(Do) != 'undefined') Do(_check_hijack);
            //            else if (typeof(get_cookie) != 'undefined') _check_hijack();
            //            </script>
            outString.AppendLine("</body>");
            outString.AppendLine("</html>");

            return outString.ToString();
        }
        */

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