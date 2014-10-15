namespace X3Platform.Web.Builder
{
    #region Using Libraries
    using System.Web;
    using System.Collections.Generic;

    using X3Platform.Configuration;
    using X3Platform.Membership;
    #endregion

    public abstract class CommonBuilder
    {
        #region 属性:ServerNameView
        /// <summary>客户访问时看到的服务器名称</summary>
        public string ServerNameView
        {
            get { return HttpContext.Current.Request.ServerVariables["SERVER_NAME"]; }
        }
        #endregion

        #region 属性:Domain
        /// <summary>域名</summary>
        public virtual string Domain
        {
            get { return "localhost"; }
        }
        #endregion

        #region 属性:IsActiveDomain
        /// <summary>判断是否是当前活动的域</summary>
        public virtual bool IsActiveDomain
        {
            get
            {
                string serverNameView = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

                string domain = string.Empty;

                int point = 0;

                if (serverNameView.LastIndexOf(".") == -1)
                {
                    // 没有点的情况
                    // => 不做处理, 设置为默认路径
                }
                else
                {
                    point = serverNameView.LastIndexOf(".");

                    if (serverNameView.Substring(0, point).LastIndexOf(".") == -1)
                    {
                        // 只有一个点的情况
                        // => 例如 workspace.com 直接取 workspace.com

                        domain = serverNameView;
                    }
                    else
                    {
                        // 有两个或两个以上的点的情况
                        // => my.workspace.com 取 workspace.com

                        point = serverNameView.Substring(0, point).LastIndexOf(".") + 1;

                        domain = serverNameView.Substring(point, serverNameView.Length - point);
                    }
                }

                return (Domain == domain);
            }
        }
        #endregion

        #region 属性:HostName
        /// <summary>主机名</summary>
        public string HostName
        {
            get { return KernelConfigurationView.Instance.HostName; }
        }
        #endregion

        #region 属性:StaticFileHostName
        /// <summary>存放静态文件的主机名</summary>
        public string StaticFileHostName
        {
            get { return KernelConfigurationView.Instance.StaticFileHostName; }
        }
        #endregion

        protected string GetOptionValue(Dictionary<string, string> options, string name)
        {
            return options.ContainsKey(name) ? options[name] : string.Empty;
        }

        protected string ReplaceOptionValue(Dictionary<string, string> options, string text)
        {
            foreach (KeyValuePair<string, string> option in options)
            {
                if (option.Key.IndexOf("$") == 0)
                {
                    text = text.Replace(option.Key, option.Value);
                }
            }

            return text;
        }

        protected bool HasRole(IAccountInfo account, IRoleInfo Role)
        {
            bool success = false;

            //// if (grade.IndexOf("[" + member.Grade + "]") > -1) success = true;

            //for (int i = 0; i < gradeArray.Length; i++)
            //{
            //    if (gradeArray[i] == member.Grade)
            //    {
            //        success = true;
            //    }
            //}

            return success;
        }

        protected bool HasAuthority(IAccountInfo account, string authority)
        {
            bool success = false;

            //if (!(string.IsNullOrEmpty(authority) || member == null || string.IsNullOrEmpty(member.Authority)))
            //{
            //    if (member.Authority.IndexOf("[" + authority + "]") > -1) success = true;
            //}

            return success;
        }
    }
}
