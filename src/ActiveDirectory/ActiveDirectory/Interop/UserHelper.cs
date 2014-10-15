
namespace X3Platform.ActiveDirectory.Interop
{
    using System;
    using System.DirectoryServices;

    using Common.Logging;

    using X3Platform.ActiveDirectory.Configuration;

    public sealed class UserHelper
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>集成模式 ON | OFF</summary>
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

        #region 函数:Initialize(string integratedMode, string path, string username, string password, string suffixEmailDomain, string directoryName)
        /// <summary>初始化 Active Directory 对象</summary>
        public void Initialize(string integratedMode, string path, string username, string password, string suffixEmailDomain, string directoryName)
        {
            this.integratedMode = integratedMode.ToUpper();

            directoryEntry = new DirectoryEntry(path, username, password);

            directorySearcher = new DirectorySearcher();

            this.directoryName = directoryName;

            this.suffixEmailDomain = suffixEmailDomain;
        }
        #endregion

        #region 函数:Rename(string loginName, string newName)
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

        #region 函数:Rename(string loginName, string newName)
        /// <summary>重命名,(*) loginName 即 samAccountName 是不变得, name 是可以修改.</summary>
        /// <param name="name">登录名</param>
        /// <param name="newName">新的名字</param>
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

        #region 函数:Find(string loginName)
        public DirectoryEntry Find(string loginName)
        {
            return Find(loginName, string.Empty);
        }
        #endregion

        #region 函数:Find(string longinName, string name)
        /// <summary>查找</summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry Find(string longinName, string name)
        {
            //
            // Notice
            //
            // Active Directory 中 用户的Name可以重复,只要不在相同的OU下.
            //
            // 所有只有采用登录名的方式辨别唯一.
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

        /// <summary>查找默认OU下面的所有人员.</summary>
        /// <returns></returns>
        public SearchResultCollection FindAll()
        {
            return FindAll(string.Format("OU={0}", directoryName));
        }

        /// <summary>查找某个组织单位下面的所有人员.</summary>
        /// <param name="fullName">组织单位的显名,例如:OU=微软技术中心,OU=合作伙伴,OU=组织结构</param>
        /// <returns></returns>
        public SearchResultCollection FindAll(string fullName)
        {
            if (this.integratedMode == "OFF")
                return null;

            DirectoryEntries nodes = this.directoryEntry.Children;

            directorySearcher.SearchRoot = nodes.Find(fullName);
            directorySearcher.PageSize = int.MaxValue; // 此参数可以任意设置，但不能不设置，如不设置读取AD数据为0-999条数据，设置后可以读取大于1000条数据.
            //directorySearcher.SizeLimit = 10000;
            directorySearcher.Filter = "(objectClass=user)";
            directorySearcher.SearchScope = SearchScope.Subtree;

            directorySearcher.Sort = new SortOption("sAMAccountName", SortDirection.Ascending);

            return directorySearcher.FindAll();
        }

        #region 函数:MoveTo(string parentPath, string loginName)
        /// <summary>移动用户</summary>
        /// <param name="parentPath">组织单位的显名,例如:OU=微软技术中心,OU=合作伙伴,OU=组织结构</param>
        /// <param name="fullName">用户的显名,例如:CN=Administrator,OU=微软技术中心,OU=合作伙伴,OU=组织结构</param>
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

        #region 函数:Add(string loginName, string name, string telephone, string email)
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

        #region 函数:Add(string loginName, string password, string name, string telephone, string email)
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

                    // 这两段代码放在龙湖的AD服务器上, 会抛出异常..
                    //
                    //item.Properties["telephoneNumber"].Add(telephone);
                    //item.Properties["mail"].Add(email);

                    param.Properties["accountexpires"].Add("0");

                    param.CommitChanges();

                    //设置帐号状态
                    param.Properties["userAccountControl"].Value = 66048; //66048 启用, 546 禁用, 密码永不过期标志为 0x10000

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

        #region 函数:Update(string loginName, string name, string telephone, string email)
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

        #region 函数:Remove(string loginName)
        /// <summary>删除</summary>
        /// <param name="loginName">登录名</param>
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

        #region 函数:IsExistLoginName(string loginName)
        /// <summary>是否存在用户[user]</summary>
        /// <param name="loginName">登录名称</param>
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

        #region 函数:IsExist(string loginName, string name)
        /// <summary>是否存在对象(包括组[Group]和用户[User])</summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public bool IsExist(string loginName, string name)
        {
            // 先查找用户, 如果不存在则查找群组信息.

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

        #region 函数:SetPassword(string longinName, string password)
        /// <summary>设置密码</summary>
        /// <param name="longinName">密码</param>
        /// <param name="password">密码</param>
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

        #region 函数:ChangePassword(string longinName, string password, string originalPassword)
        /// <summary>
        /// 检测密码是否符合密码策略
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="password">密码</param>
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

        #region 函数:SetStatus(string longinName, bool disabled)
        /// <summary>
        /// 修改账户状态
        /// </summary>
        /// <param name="loginName">登录帐号</param>
        /// <param name="enabled">true : 激活 | false : 禁用</param>
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
                    //设置帐号状态
                    param.Properties["userAccountControl"].Value = enabled ? 66048 : 546; //66048 启用, 546 禁用, 密码永不过期标志为 0x10000

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

        #region 函数:CheckPasswordStrategy(string password)
        /// <summary>
        /// 检测密码是否符合密码策略
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
                // (*)在 ActiveDirectory 中建一个PasswordStrategy帐号，专门用来检测密码是否符合当前的密码策略。
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

        /// <summary>根据群组名称获取用户信息</summary>
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