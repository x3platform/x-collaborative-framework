namespace X3Platform.LDAP
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using System.DirectoryServices;

    using Common.Logging;

    using X3Platform.Plugins;

    using X3Platform.LDAP.Configuration;
    using X3Platform.LDAP.Interop;
    #endregion

    public sealed class LDAPManagement : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region 属性:Name
        public override string Name
        {
            get { return "LDAP 管理"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile LDAPManagement instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static LDAPManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new LDAPManagement();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private LDAPConfiguration configuration = null;

        /// <summary>配置</summary>
        public LDAPConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:OrganizationUnit
        private OrganizationUnitHelper m_OrganizationUnit = null;

        /// <summary>组织</summary>
        public OrganizationUnitHelper OrganizationUnit
        {
            get { return m_OrganizationUnit; }
        }
        #endregion

        #region 属性:Group
        private GroupHelper m_Group = null;

        /// <summary>群组</summary>
        public GroupHelper Group
        {
            get { return m_Group; }
        }
        #endregion

        #region 属性:User
        private UserHelper m_User;

        /// <summary>用户</summary>
        public UserHelper User
        {
            get { return m_User; }
        }
        #endregion

        /// <summary>集成模式 ON | OFF</summary>
        private string integratedMode = null;

        private DirectoryEntry directoryEntry = null;

        private DirectorySearcher directorySearcher = null;

        #region 构造函数:LDAPManagement()
        /// <summary>构造函数</summary>
        private LDAPManagement()
        {
            Reload();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                Reload();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                return 1;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            configuration = LDAPConfigurationView.Instance.Configuration;

            this.integratedMode = LDAPConfigurationView.Instance.IntegratedMode;

            directoryEntry = new DirectoryEntry(LDAPConfigurationView.Instance.LDAPPath,
                LDAPConfigurationView.Instance.LoginName,
                LDAPConfigurationView.Instance.Password);

            directorySearcher = new DirectorySearcher();

            Initialize();
        }
        #endregion

        #region 函数:Initialize()
        /// <summary>初始化 Active Directory 对象</summary>
        private void Initialize()
        {
            if (this.integratedMode == "ON")
            {
                string corporationOrganizationUnitFolderRoot = LDAPConfigurationView.Instance.CorporationOrganizationUnitFolderRoot;
                string corporationGroupFolderRoot = LDAPConfigurationView.Instance.CorporationGroupFolderRoot;
                string corporationRoleFolderRoot = LDAPConfigurationView.Instance.CorporationRoleFolderRoot;
                string corporationUserFolderRoot = LDAPConfigurationView.Instance.CorporationUserFolderRoot;

                string[] list = new string[] { corporationOrganizationUnitFolderRoot, corporationGroupFolderRoot, corporationRoleFolderRoot, corporationUserFolderRoot };

                for (int i = 0; i < list.Length; i++)
                {
                    if (Find(list[i], "organizationalUnit") == null)
                    {
                        DirectoryEntry param = directoryEntry.Children.Add(string.Format("OU={0}", list[i]), "OrganizationUnitalUnit");

                        param.CommitChanges();
                    }
                }

                if (Find(corporationOrganizationUnitFolderRoot, "group") == null)
                {
                    string fullName = string.Format("CN={0},OU={0}", corporationOrganizationUnitFolderRoot);

                    DirectoryEntry param = directoryEntry.Children.Add(fullName, "group");

                    param.Properties["name"].Add(corporationOrganizationUnitFolderRoot);
                    param.Properties["samaccountname"].Add(corporationOrganizationUnitFolderRoot);

                    param.Properties["grouptype"].Add("-2147483640");
                    param.CommitChanges();
                }

                string everyoneGroup = "所有人";

                if (Find(everyoneGroup, "group") == null)
                {
                    string fullName = string.Format("CN={1},OU={0}", corporationOrganizationUnitFolderRoot, everyoneGroup);

                    DirectoryEntry param = directoryEntry.Children.Add(fullName, "group");

                    param.Properties["name"].Add(everyoneGroup);
                    param.Properties["samaccountname"].Add(everyoneGroup);

                    param.Properties["grouptype"].Add("-2147483640");
                    param.CommitChanges();
                }
            }

            m_OrganizationUnit = new OrganizationUnitHelper();

            m_Group = new GroupHelper();

            m_User = new UserHelper();
        }
        #endregion

        #region 函数:Find(string name, string schemaClassName)
        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry Find(string name, string schemaClassName)
        {
            if (this.integratedMode == "OFF") { return null; }

            directorySearcher.SearchRoot = directoryEntry;

            directorySearcher.Filter = string.Format("(&(objectClass={0})(name={1}))", schemaClassName, name);
            directorySearcher.SearchScope = SearchScope.Subtree;

            directorySearcher.Sort = new SortOption("name", SortDirection.Ascending);

            SearchResultCollection list = directorySearcher.FindAll();

            return (list.Count == 0) ? null : list[0].GetDirectoryEntry();
        }
        #endregion
    }
}