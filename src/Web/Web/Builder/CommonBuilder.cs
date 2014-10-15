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
        #region ����:ServerNameView
        /// <summary>�ͻ�����ʱ�����ķ���������</summary>
        public string ServerNameView
        {
            get { return HttpContext.Current.Request.ServerVariables["SERVER_NAME"]; }
        }
        #endregion

        #region ����:Domain
        /// <summary>����</summary>
        public virtual string Domain
        {
            get { return "localhost"; }
        }
        #endregion

        #region ����:IsActiveDomain
        /// <summary>�ж��Ƿ��ǵ�ǰ�����</summary>
        public virtual bool IsActiveDomain
        {
            get
            {
                string serverNameView = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

                string domain = string.Empty;

                int point = 0;

                if (serverNameView.LastIndexOf(".") == -1)
                {
                    // û�е�����
                    // => ��������, ����ΪĬ��·��
                }
                else
                {
                    point = serverNameView.LastIndexOf(".");

                    if (serverNameView.Substring(0, point).LastIndexOf(".") == -1)
                    {
                        // ֻ��һ��������
                        // => ���� workspace.com ֱ��ȡ workspace.com

                        domain = serverNameView;
                    }
                    else
                    {
                        // ���������������ϵĵ�����
                        // => my.workspace.com ȡ workspace.com

                        point = serverNameView.Substring(0, point).LastIndexOf(".") + 1;

                        domain = serverNameView.Substring(point, serverNameView.Length - point);
                    }
                }

                return (Domain == domain);
            }
        }
        #endregion

        #region ����:HostName
        /// <summary>������</summary>
        public string HostName
        {
            get { return KernelConfigurationView.Instance.HostName; }
        }
        #endregion

        #region ����:StaticFileHostName
        /// <summary>��ž�̬�ļ���������</summary>
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
