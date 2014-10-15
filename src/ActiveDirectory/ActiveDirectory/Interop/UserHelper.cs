
namespace X3Platform.ActiveDirectory.Interop
{
    using System;
    using System.DirectoryServices;

    using Common.Logging;

    using X3Platform.ActiveDirectory.Configuration;

    public sealed class UserHelper
    {
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����ģʽ ON | OFF</summary>
        private string integratedMode = null;

        private DirectoryEntry directoryEntry = null;

        private DirectorySearcher directorySearcher = null;

        private string directoryName = null;

        private string suffixEmailDomain = null;

        public UserHelper()
        {
            Initialize(
                ActiveDirectoryConfigurationView.Instance.IntegratedMode,
                ActiveDirectoryConfigurationView.Instance.LDAPPath,
                ActiveDirectoryConfigurationView.Instance.LoginName,
                ActiveDirectoryConfigurationView.Instance.Password,
                ActiveDirectoryConfigurationView.Instance.SuffixEmailDomain,
                ActiveDirectoryConfigurationView.Instance.CorporationUserFolderRoot);
        }

        public UserHelper(string integratedMode, string path, string username, string password, string suffixEmailDomain, string directoryName)
        {
            Initialize(integratedMode, path, username, password, suffixEmailDomain, directoryName);
        }

        #region ����:Initialize(string integratedMode, string path, string username, string password, string suffixEmailDomain, string directoryName)
        /// <summary>��ʼ�� Active Directory ����</summary>
        public void Initialize(string integratedMode, string path, string username, string password, string suffixEmailDomain, string directoryName)
        {
            this.integratedMode = integratedMode.ToUpper();

            directoryEntry = new DirectoryEntry(path, username, password);

            directorySearcher = new DirectorySearcher();

            this.directoryName = directoryName;

            this.suffixEmailDomain = suffixEmailDomain;
        }
        #endregion

        #region ����:Rename(string loginName, string newName)
        private void CheckDefaultDirectory()
        {
            string name = string.Format("OU={0}", this.directoryName);

            if (Find(this.directoryName, ActiveDirectorySchemaClassType.OrganizationalUnit) == null)
            {
                DirectoryEntry param = directoryEntry.Children.Add(name, ActiveDirectorySchemaClassType.OrganizationalUnit);

                param.CommitChanges();
            }
        }
        #endregion

        #region ����:Rename(string loginName, string newName)
        /// <summary>������,(*) loginName �� samAccountName �ǲ����, name �ǿ����޸�.</summary>
        /// <param name="name">��¼��</param>
        /// <param name="newName">�µ�����</param>
        public void Rename(string loginName, string newName)
        {
            if (this.integratedMode == "OFF")
                return;

            try
            {
                DirectoryEntry param = Find(loginName);

                if (param != null)
                {
                    param.Rename(string.Format("CN={0}", newName));

                    param.Properties["givenName"].Value = newName;
                    param.Properties["displayname"].Value = newName;

                    param.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        #endregion

        #region ����:Find(string loginName)
        public DirectoryEntry Find(string loginName)
        {
            return Find(loginName, string.Empty);
        }
        #endregion

        #region ����:Find(string longinName, string name)
        /// <summary>����</summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry Find(string longinName, string name)
        {
            //
            // Notice
            //
            // Active Directory �� �û���Name�����ظ�,ֻҪ������ͬ��OU��.
            //
            // ����ֻ�в��õ�¼���ķ�ʽ���Ψһ.
            //

            if (this.integratedMode == "OFF")
                return null;

            string filter = null;

            if (string.IsNullOrEmpty(name))
            {
                filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", longinName);
            }
            else
            {
                filter = string.Format("(&(objectClass=user)(sAMAccountName={0})(name={1}))", longinName, name);
            }

            directorySearcher.Filter = filter;

            directorySearcher.SearchRoot = directoryEntry;
            directorySearcher.SearchScope = SearchScope.Subtree;
            directorySearcher.Sort = new SortOption("sAMAccountName", SortDirection.Ascending);

            SearchResultCollection list = directorySearcher.FindAll();

            return (list.Count == 0) ? null : list[0].GetDirectoryEntry();
        }
        #endregion

        /// <summary>����Ĭ��OU�����������Ա.</summary>
        /// <returns></returns>
        public SearchResultCollection FindAll()
        {
            return FindAll(string.Format("OU={0}", directoryName));
        }

        /// <summary>����ĳ����֯��λ�����������Ա.</summary>
        /// <param name="fullName">��֯��λ������,����:OU=΢��������,OU=�������,OU=��֯�ṹ</param>
        /// <returns></returns>
        public SearchResultCollection FindAll(string fullName)
        {
            if (this.integratedMode == "OFF")
                return null;

            DirectoryEntries nodes = this.directoryEntry.Children;

            directorySearcher.SearchRoot = nodes.Find(fullName);
            directorySearcher.PageSize = int.MaxValue; // �˲��������������ã������ܲ����ã��粻���ö�ȡAD����Ϊ0-999�����ݣ����ú���Զ�ȡ����1000������.
            //directorySearcher.SizeLimit = 10000;
            directorySearcher.Filter = "(objectClass=user)";
            directorySearcher.SearchScope = SearchScope.Subtree;

            directorySearcher.Sort = new SortOption("sAMAccountName", SortDirection.Ascending);

            return directorySearcher.FindAll();
        }

        #region ����:MoveTo(string parentPath, string loginName)
        /// <summary>�ƶ��û�</summary>
        /// <param name="parentPath">��֯��λ������,����:OU=΢��������,OU=�������,OU=��֯�ṹ</param>
        /// <param name="fullName">�û�������,����:CN=Administrator,OU=΢��������,OU=�������,OU=��֯�ṹ</param>
        public void MoveTo(string parentPath, string loginName)
        {
            try
            {
                DirectoryEntry parent = directoryEntry.Children.Find(parentPath);

                DirectoryEntry child = Find(loginName);

                if (child == null || parent == null)
                {
                    return;
                }

                child.MoveTo(parent);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        #endregion

        #region ����:Add(string loginName, string name, string telephone, string email)
        /// <summary></summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="telephone"></param>
        /// <param name="email"></param>
        public void Add(string loginName, string name, string telephone, string email)
        {
            string password = ActiveDirectoryConfigurationView.Instance.NewlyCreatedAccountPassword;

            this.Add(loginName, password, name, telephone, email);
        }
        #endregion

        #region ����:Add(string loginName, string password, string name, string telephone, string email)
        /// <summary></summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="telephone"></param>
        /// <param name="email"></param>
        public void Add(string loginName, string password, string name, string telephone, string email)
        {
            if (this.integratedMode == "OFF") { return; }

            try
            {
                if (!IsExist(loginName, name))
                {
                    DirectoryEntries directoryEntries = directoryEntry.Children;

                    DirectoryEntry param = directoryEntries.Add(string.Format("CN={0},OU={1}", name, this.directoryName), ActiveDirectorySchemaClassType.User);

                    param.Properties["samAccountName"].Add(loginName);

                    param.Properties["name"].Add(name);
                    param.Properties["givenName"].Add(name);
                    param.Properties["displayname"].Add(name);
                    param.Properties["userPrincipalName"].Add(string.Format("{0}{1}", loginName, this.suffixEmailDomain));

                    // �����δ������������AD��������, ���׳��쳣..
                    //
                    //item.Properties["telephoneNumber"].Add(telephone);
                    //item.Properties["mail"].Add(email);

                    param.Properties["accountexpires"].Add("0");

                    param.CommitChanges();

                    //�����ʺ�״̬
                    param.Properties["userAccountControl"].Value = 66048; //66048 ����, 546 ����, �����������ڱ�־Ϊ 0x10000

                    param.CommitChanges();

                    this.SetPassword(loginName, password);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw;
            }
        }
        #endregion

        #region ����:Update(string loginName, string name, string telephone, string email)
        public void Update(string loginName, string name, string telephone, string email)
        {
            if (this.integratedMode == "OFF")
                return;

            try
            {
                DirectoryEntry item = Find(loginName, name);

                if (item != null)
                {
                    item.Properties["name"].Value = name;
                    item.Properties["givenName"].Value = name;
                    item.Properties["displayname"].Value = name;
                    item.Properties["userPrincipalName"].Value = string.Format("{0}{1}", loginName, this.suffixEmailDomain);
                    item.Properties["samAccountName"].Value = name;
                    //item.Properties["telephoneNumber"].Value = telephone;
                    //item.Properties["mail"].Value = email;

                    item.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        #endregion

        #region ����:Remove(string loginName)
        /// <summary>ɾ��</summary>
        /// <param name="loginName">��¼��</param>
        public void Remove(string loginName)
        {
            if (this.integratedMode == "OFF")
                return;

            try
            {
                DirectoryEntry param = Find(loginName);

                if (param == null)
                {
                    return;
                }
                else
                {
                    param.Parent.Children.Remove(param);

                    param.Parent.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        #endregion

        #region ����:IsExistLoginName(string loginName)
        /// <summary>�Ƿ�����û�[user]</summary>
        /// <param name="loginName">��¼����</param>
        /// <returns></returns>
        public bool IsExistLoginName(string loginName)
        {
            SearchResultCollection list = null;

            directorySearcher.SearchRoot = directoryEntry;

            directorySearcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", loginName);
            directorySearcher.SearchScope = SearchScope.Subtree;

            list = directorySearcher.FindAll();

            return (list.Count == 0) ? false : true;
        }
        #endregion

        #region ����:IsExist(string loginName, string name)
        /// <summary>�Ƿ���ڶ���(������[Group]���û�[User])</summary>
        /// <param name="name">����</param>
        /// <returns></returns>
        public bool IsExist(string loginName, string name)
        {
            // �Ȳ����û�, ��������������Ⱥ����Ϣ.

            bool result = false;

            SearchResultCollection list = null;

            directorySearcher.SearchRoot = directoryEntry;

            directorySearcher.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", loginName);
            directorySearcher.SearchScope = SearchScope.Subtree;

            list = directorySearcher.FindAll();

            result = (list.Count == 0) ? false : true;

            if (!result)
            {

                //directorySearcher.SearchRoot = directoryEntry;
                directorySearcher.Filter = string.Format("(&(objectClass=user)(name={0}))", name);
                directorySearcher.SearchScope = SearchScope.Subtree;

                list = directorySearcher.FindAll();

                result = (list.Count == 0) ? false : true;
            }

            if (!result)
            {
                directorySearcher.Filter = string.Format("(&(objectClass=group)(name={0}))", name);
                directorySearcher.SearchScope = SearchScope.Subtree;

                list = directorySearcher.FindAll();

                result = (list.Count == 0) ? false : true;
            }

            return result;
        }
        #endregion

        #region ����:SetPassword(string longinName, string password)
        /// <summary>��������</summary>
        /// <param name="longinName">����</param>
        /// <param name="password">����</param>
        /// <returns></returns>
        public bool SetPassword(string longinName, string password)
        {
            try
            {
                if (this.integratedMode == "OFF")
                    return true;


                DirectoryEntry param = Find(longinName);

                if (param != null)
                {
                    param.AuthenticationType = AuthenticationTypes.Secure;

                    param.Invoke("SetPassword", new object[] { password });

                    param.CommitChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);

                return false;
            }
        }
        #endregion

        #region ����:ChangePassword(string longinName, string password, string originalPassword)
        /// <summary>
        /// ��������Ƿ�����������
        /// </summary>
        /// <param name="password">����</param>
        /// <param name="password">����</param>
        /// <returns></returns>
        public bool ChangePassword(string longinName, string password, string originalPassword)
        {
            try
            {
                if (this.integratedMode == "OFF")
                    return true;

                DirectoryEntry param = Find(longinName);

                if (param != null)
                {
                    param.AuthenticationType = AuthenticationTypes.Secure;

                    param.Invoke("ChangePassword", new object[] { originalPassword, password });

                    param.CommitChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);

                return false;
            }
        }
        #endregion

        #region ����:SetStatus(string longinName, bool disabled)
        /// <summary>
        /// �޸��˻�״̬
        /// </summary>
        /// <param name="loginName">��¼�ʺ�</param>
        /// <param name="enabled">true : ���� | false : ����</param>
        /// <returns></returns>
        public void SetStatus(string longinName, bool enabled)
        {
            try
            {
                if (this.integratedMode == "OFF")
                    return;

                DirectoryEntry param = Find(longinName);

                if (param != null)
                {
                    //�����ʺ�״̬
                    param.Properties["userAccountControl"].Value = enabled ? 66048 : 546; //66048 ����, 546 ����, �����������ڱ�־Ϊ 0x10000

                    param.CommitChanges();

                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw;
            }
        }
        #endregion

        #region ����:CheckPasswordStrategy(string password)
        /// <summary>
        /// ��������Ƿ�����������
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckPasswordStrategy(string password)
        {
            try
            {
                if (this.integratedMode == "OFF")
                    return true;

                //
                // CN=PasswordStrategy
                //
                // (*)�� ActiveDirectory �н�һ��PasswordStrategy�ʺţ�ר��������������Ƿ���ϵ�ǰ��������ԡ�
                //

                DirectoryEntry param = Find("passwordstrategy");

                string defaultPassword = "PasswordStrategy@2008";

                if (param == null)
                {
                    DirectoryEntries directoryEntries = directoryEntry.Children;

                    param = directoryEntries.Add("CN=PasswordStrategy,CN=Users", ActiveDirectorySchemaClassType.User);

                    param.Properties["name"].Add("PasswordStrategy");
                    param.Properties["givenName"].Add("PasswordStrategy");
                    param.Properties["displayname"].Add("PasswordStrategy");
                    param.Properties["userPrincipalName"].Add(string.Format("{0}{1}", "passwordstrategy", this.suffixEmailDomain));
                    param.Properties["samAccountName"].Add("PasswordStrategy");
                    param.Properties["accountexpires"].Add("0");

                    param.CommitChanges();

                    param.Invoke("SetPassword", new object[] { defaultPassword });

                    param.CommitChanges();
                }

                param.Invoke("SetPassword", new object[] { password });

                param.CommitChanges();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(this.directoryEntry.Path + "|" + this.directoryEntry.Username, ex);

                return false;
            }
        }
        #endregion

        /// <summary>����Ⱥ�����ƻ�ȡ�û���Ϣ</summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public string[] GetUsersByGroupName(string groupName)
        {
            directorySearcher.Filter = "(&(objectClass=group)(cn=" + groupName + "))";

            directorySearcher.PropertiesToLoad.Add("member");

            SearchResult searchResult = directorySearcher.FindOne();

            if (searchResult.Properties["member"] == null)
            {
                return null;
            }

            string[] results = new string[searchResult.Properties["member"].Count];

            for (int i = 0; i < searchResult.Properties["member"].Count; i++)
            {
                string groupPath = searchResult.Properties["member"][i].ToString();

                results[i] = groupPath.Substring(3, groupPath.IndexOf(",") - 3);
            }

            return results;
        }
    }
}