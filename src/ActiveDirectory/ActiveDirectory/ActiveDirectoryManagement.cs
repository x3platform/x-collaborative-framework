#region Using Libraries
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.DirectoryServices;

using Common.Logging;

using X3Platform.Plugins;

using X3Platform.ActiveDirectory.Configuration;
using X3Platform.ActiveDirectory.Interop;
#endregion

namespace X3Platform.ActiveDirectory
{
    public sealed class ActiveDirectoryManagement : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 属性:Name
        public override string Name
        {
            get { return "ActiveDirectory 管理"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile ActiveDirectoryManagement instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static ActiveDirectoryManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ActiveDirectoryManagement();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private ActiveDirectoryConfiguration configuration = null;

        /// <summary>配置</summary>
        public ActiveDirectoryConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:Organization
        private OrganizationHelper m_Organization = null;

        /// <summary>组织</summary>
        public OrganizationHelper Organization
        {
            get { return m_Organization; }
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

        #region 构造函数:ActiveDirectoryManagement()
        /// <summary>构造函数</summary>
        private ActiveDirectoryManagement()
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
            configuration = ActiveDirectoryConfigurationView.Instance.Configuration;

            this.integratedMode = ActiveDirectoryConfigurationView.Instance.IntegratedMode;

            directoryEntry = new DirectoryEntry(ActiveDirectoryConfigurationView.Instance.LDAPPath,
                ActiveDirectoryConfigurationView.Instance.LoginName,
                ActiveDirectoryConfigurationView.Instance.Password);

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
                string corporationOrganizationFolderRoot = ActiveDirectoryConfigurationView.Instance.CorporationOrganizationFolderRoot;
                string corporationGroupFolderRoot = ActiveDirectoryConfigurationView.Instance.CorporationGroupFolderRoot;
                string corporationRoleFolderRoot = ActiveDirectoryConfigurationView.Instance.CorporationRoleFolderRoot;
                string corporationUserFolderRoot = ActiveDirectoryConfigurationView.Instance.CorporationUserFolderRoot;

                string[] list = new string[] { corporationOrganizationFolderRoot, corporationGroupFolderRoot, corporationRoleFolderRoot, corporationUserFolderRoot };

                for (int i = 0; i < list.Length; i++)
                {
                    if (Find(list[i], "organizationalUnit") == null)
                    {
                        DirectoryEntry param = directoryEntry.Children.Add(string.Format("OU={0}", list[i]), "OrganizationalUnit");

                        param.CommitChanges();
                    }
                }

                if (Find(corporationOrganizationFolderRoot, "group") == null)
                {
                    string fullName = string.Format("CN={0},OU={0}", corporationOrganizationFolderRoot);

                    DirectoryEntry param = directoryEntry.Children.Add(fullName, "group");

                    param.Properties["name"].Add(corporationOrganizationFolderRoot);
                    param.Properties["samaccountname"].Add(corporationOrganizationFolderRoot);

                    param.Properties["grouptype"].Add("-2147483640");
                    param.CommitChanges();
                }

                string everyoneGroup = "所有人";

                if (Find(everyoneGroup, "group") == null)
                {
                    string fullName = string.Format("CN={1},OU={0}", corporationOrganizationFolderRoot, everyoneGroup);

                    DirectoryEntry param = directoryEntry.Children.Add(fullName, "group");

                    param.Properties["name"].Add(everyoneGroup);
                    param.Properties["samaccountname"].Add(everyoneGroup);

                    param.Properties["grouptype"].Add("-2147483640");
                    param.CommitChanges();
                }
            }

            m_Organization = new OrganizationHelper();

            m_Group = new GroupHelper();

            m_User = new UserHelper();
        }
        #endregion

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry Find(string name, string schemaClassName)
        {
            if (this.integratedMode == "OFF")
                return null;

            directorySearcher.SearchRoot = directoryEntry;

            directorySearcher.Filter = string.Format("(&(objectClass={0})(name={1}))", schemaClassName, name);
            directorySearcher.SearchScope = SearchScope.Subtree;

            directorySearcher.Sort = new SortOption("name", SortDirection.Ascending);

            SearchResultCollection list = directorySearcher.FindAll();

            return (list.Count == 0) ? null : list[0].GetDirectoryEntry();
        }
    }
}