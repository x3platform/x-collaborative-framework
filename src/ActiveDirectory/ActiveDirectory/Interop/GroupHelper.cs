namespace X3Platform.ActiveDirectory.Interop
{
    #region Using Libraries
    using System;
    using System.DirectoryServices;

    using Common.Logging;

    using X3Platform.ActiveDirectory.Configuration;
    #endregion

    public sealed class GroupHelper
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>集成模式 ON | OFF</summary>
        private string integratedMode = null;

        private DirectoryEntry directoryEntry = null;

        private DirectorySearcher directorySearcher = null;

        private string directoryName = null;

        public GroupHelper()
        {
            Initialize(
                ActiveDirectoryConfigurationView.Instance.IntegratedMode,
                ActiveDirectoryConfigurationView.Instance.LDAPPath,
                ActiveDirectoryConfigurationView.Instance.LoginName,
                ActiveDirectoryConfigurationView.Instance.Password,
                ActiveDirectoryConfigurationView.Instance.CorporationGroupFolderRoot);
        }

        public GroupHelper(string integratedMode, string path, string username, string password, string directoryName)
        {
            Initialize(integratedMode, path, username, password, directoryName);
        }

        #region 函数:Initialize(string integratedMode, string path, string username, string password, string directoryName)
        /// <summary>初始化 Active Directory 对象</summary>
        private void Initialize(string integratedMode, string path, string username, string password, string directoryName)
        {
            this.integratedMode = integratedMode.ToUpper();

            directoryEntry = new DirectoryEntry(path, username, password);

            directorySearcher = new DirectorySearcher();

            this.directoryName = directoryName;
        }
        #endregion

        #region 函数:Add(string name, string parentPath)
        /// <summary>添加组</summary>
        /// <param name="name">名称</param>
        /// <param name="parentPath">OrganizationalUnit Name</param>
        public void Add(string name, string parentPath)
        {
            if (this.integratedMode == "OFF")
            {
                return;
            }

            try
            {
                if (!IsExistName(name))
                {
                    string fullName = string.Format("CN={0}", name + (string.Format(",{0}", (string.IsNullOrEmpty(parentPath) ? string.Format("OU={0}", directoryName) : parentPath))));

                    DirectoryEntry param = directoryEntry.Children.Add(fullName, ActiveDirectorySchemaClassType.Group);

                    // 2:全局-通讯组 | 8:通用-通讯组 | -2147483640:通用-安全组

                    param.Properties["name"].Add(name);
                    param.Properties["samaccountname"].Add(name);

                    param.Properties["grouptype"].Add("-2147483640");
                    param.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}", ex.Message, ex.ToString()));
            }
        }
        #endregion

        #region 函数:Find(string name)
        /// <summary>查询</summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry Find(string name)
        {
            return Find(name, ActiveDirectorySchemaClassType.Group);
        }

        /// <summary>查询</summary>
        /// <param name="name">当schemaClassName是group, 则name表示groupName, 当schemaClassName是user, 则name表示loginName.</param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry Find(string name, string schemaClassName)
        {
            if (this.integratedMode == "OFF")
                return null;

            string filter = null;

            if (schemaClassName == "user")
            {
                filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", name);
            }
            else
            {
                filter = string.Format("(&(objectClass={0})(name={1}))", schemaClassName, name);
            }

            directorySearcher.SearchRoot = directoryEntry;

            directorySearcher.Filter = filter;
            directorySearcher.SearchScope = SearchScope.Subtree;

            directorySearcher.Sort = new SortOption("name", SortDirection.Ascending);

            SearchResultCollection list = directorySearcher.FindAll();

            return (list.Count == 0) ? null : list[0].GetDirectoryEntry();
        }
        #endregion

        #region 函数:IsExistName(string name)
        /// <summary>
        /// 是否存在对象(包括组[Group]和用户[User])
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public bool IsExistName(string name)
        {
            bool result = false;

            SearchResultCollection list = null;

            directorySearcher.SearchRoot = directoryEntry;

            //directorySearcher.SearchRoot = directoryEntry;
            directorySearcher.Filter = string.Format("(&(objectClass=group)(name={0}))", name);
            directorySearcher.SearchScope = SearchScope.Subtree;

            list = directorySearcher.FindAll();

            result = (list.Count == 0) ? false : true;

            // 先查找群组, 如果不存在则查找用户信息.

            if (!result)
            {
                directorySearcher.Filter = string.Format("(&(objectClass=user)(name={0}))", name);
                directorySearcher.SearchScope = SearchScope.Subtree;

                list = directorySearcher.FindAll();

                result = (list.Count == 0) ? false : true;
            }

            return result;
        }
        #endregion

        #region 函数:Rename(string name, string newName)
        /// <summary>重命名</summary>
        /// <param name="name">名字</param>
        /// <param name="newName">新的名字</param>
        public void Rename(string name, string newName)
        {
            if (this.integratedMode == "OFF")
                return;

            // 新的名称和原始的名称相同，忽略此操作。
            if (name == newName)
                return;

            try
            {
                DirectoryEntry param = Find(name, "Group");

                if (param != null)
                {
                    //window 2000的帐号
                    param.Properties["samaccountname"][0] = newName;
                    param.CommitChanges();

                    param.Rename(string.Format("CN={0}", newName));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}", ex.Message, ex.ToString()));
            }
        }
        #endregion

        #region 函数:MoveTo(string groupName, string parentPath)
        /// <summary>移动群组</summary>
        /// <param name="groupName">集团总部信息中心</param>
        /// <param name="parentPath">组织单位的显名,例如:OU=微软创新中心,OU=合作伙伴,OU=组织结构</param>
        public int MoveTo(string groupName, string parentPath)
        {
            try
            {
                DirectoryEntry parent = directoryEntry.Children.Find(parentPath);

                DirectoryEntry child = Find(groupName);

                if (child == null)
                {
                    return 1;
                }

                if (parent == null)
                {
                    return 2;
                }

                child.MoveTo(parent);

                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}", ex.Message, ex.ToString()));

                return int.MaxValue;
            }
        }
        #endregion

        #region 函数:AddRelation(string name, string schemaClassName, string groupName)
        /// <summary>添加用户和组的关系或者组和组的关系</summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public void AddRelation(string name, string schemaClassName, string groupName)
        {
            if (this.integratedMode == "OFF")
                return;

            DirectoryEntry child = Find(name, schemaClassName);
            DirectoryEntry group = Find(groupName, ActiveDirectorySchemaClassType.Group);

            if (child == null || group == null)
                return;

            if (!group.Properties["member"].Contains(child.Properties["distinguishedName"].Value))
            {
                try
                {
                    group.Properties["member"].Add(child.Properties["distinguishedName"].Value);
                    group.CommitChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}", ex.Message, ex.ToString()));
                }
            }
        }
        #endregion

        #region 函数:RemoveRelation(string name, string schemaClassName, string groupName)
        /// <summary>删除用户和组的关系或者组和组的关系</summary>
        /// <param name="name">子节点的名称</param>
        /// <param name="schemaClassName">架构名称: user (用户) | group (组)</param>
        /// <param name="groupName">组名称</param>
        /// <returns></returns>
        public void RemoveRelation(string name, string schemaClassName, string groupName)
        {
            if (this.integratedMode == "OFF")
                return;

            DirectoryEntry child = Find(name, schemaClassName);
            DirectoryEntry group = Find(groupName, ActiveDirectorySchemaClassType.Group);

            if (child == null || group == null)
                return;

            if (group.Properties["member"].Contains(child.Properties["distinguishedName"].Value))
            {
                group.Properties["member"].Remove(child.Properties["distinguishedName"].Value);
                group.CommitChanges();
            }
        }
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string[] GetGroupsByLoginName(string loginName)
        {
            try
            {
                directorySearcher.Filter = string.Format("(sAMAccountName={0})", loginName);

                directorySearcher.PropertiesToLoad.Add("memberof");

                SearchResult searchResult = directorySearcher.FindOne();

                if (searchResult == null || searchResult.Properties["memberof"] == null)
                {
                    return null;
                }

                string[] results = new string[searchResult.Properties["memberof"].Count];

                for (int i = 0; i < searchResult.Properties["memberof"].Count; i++)
                {
                    string groupPath = searchResult.Properties["memberof"][i].ToString();

                    results[i] = groupPath.Substring(3, groupPath.IndexOf(",") - 3);
                }

                return results;
            }
            catch (Exception ex)
            {
                logger.Error(loginName, ex);

                return null;
            }
        }
    }
}