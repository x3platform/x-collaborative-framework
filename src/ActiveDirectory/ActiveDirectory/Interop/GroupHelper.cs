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
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>����ģʽ ON | OFF</summary>
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

        #region ����:Initialize(string integratedMode, string path, string username, string password, string directoryName)
        /// <summary>��ʼ�� Active Directory ����</summary>
        private void Initialize(string integratedMode, string path, string username, string password, string directoryName)
        {
            this.integratedMode = integratedMode.ToUpper();

            directoryEntry = new DirectoryEntry(path, username, password);

            directorySearcher = new DirectorySearcher();

            this.directoryName = directoryName;
        }
        #endregion

        #region ����:Add(string name, string parentPath)
        /// <summary>�����</summary>
        /// <param name="name">����</param>
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

                    // 2:ȫ��-ͨѶ�� | 8:ͨ��-ͨѶ�� | -2147483640:ͨ��-��ȫ��

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

        #region ����:Find(string name)
        /// <summary>��ѯ</summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry Find(string name)
        {
            return Find(name, ActiveDirectorySchemaClassType.Group);
        }

        /// <summary>��ѯ</summary>
        /// <param name="name">��schemaClassName��group, ��name��ʾgroupName, ��schemaClassName��user, ��name��ʾloginName.</param>
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

        #region ����:IsExistName(string name)
        /// <summary>
        /// �Ƿ���ڶ���(������[Group]���û�[User])
        /// </summary>
        /// <param name="name">����</param>
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

            // �Ȳ���Ⱥ��, ���������������û���Ϣ.

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

        #region ����:Rename(string name, string newName)
        /// <summary>������</summary>
        /// <param name="name">����</param>
        /// <param name="newName">�µ�����</param>
        public void Rename(string name, string newName)
        {
            if (this.integratedMode == "OFF")
                return;

            // �µ����ƺ�ԭʼ��������ͬ�����Դ˲�����
            if (name == newName)
                return;

            try
            {
                DirectoryEntry param = Find(name, "Group");

                if (param != null)
                {
                    //window 2000���ʺ�
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

        #region ����:MoveTo(string groupName, string parentPath)
        /// <summary>�ƶ�Ⱥ��</summary>
        /// <param name="groupName">�����ܲ���Ϣ����</param>
        /// <param name="parentPath">��֯��λ������,����:OU=΢��������,OU=�������,OU=��֯�ṹ</param>
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

        #region ����:AddRelation(string name, string schemaClassName, string groupName)
        /// <summary>����û�����Ĺ�ϵ���������Ĺ�ϵ</summary>
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

        #region ����:RemoveRelation(string name, string schemaClassName, string groupName)
        /// <summary>ɾ���û�����Ĺ�ϵ���������Ĺ�ϵ</summary>
        /// <param name="name">�ӽڵ������</param>
        /// <param name="schemaClassName">�ܹ�����: user (�û�) | group (��)</param>
        /// <param name="groupName">������</param>
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