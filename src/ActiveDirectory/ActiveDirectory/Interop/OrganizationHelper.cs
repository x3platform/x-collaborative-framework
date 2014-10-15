using System;
using System.DirectoryServices;

using Common.Logging;

using X3Platform.ActiveDirectory.Configuration;

namespace X3Platform.ActiveDirectory.Interop
{
    public sealed class OrganizationHelper
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>集成模式 ON | OFF</summary>
        private string integratedMode = null;

        private DirectoryEntry directoryEntry = null;

        private DirectorySearcher directorySearcher = null;

        private string directoryName = null;

        public OrganizationHelper()
        {
            Initialize(
                ActiveDirectoryConfigurationView.Instance.IntegratedMode,
                ActiveDirectoryConfigurationView.Instance.LDAPPath,
                ActiveDirectoryConfigurationView.Instance.LoginName,
                ActiveDirectoryConfigurationView.Instance.Password,
                ActiveDirectoryConfigurationView.Instance.CorporationOrganizationFolderRoot);
        }

        public OrganizationHelper(string integratedMode, string path, string username, string password, string directoryName)
        {
            Initialize(integratedMode, path, username, password, directoryName);
        }

        #region 函数:Initialize(string path, string username, string password, string domain, string directoryName, string mode)
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
        /// <summary>添加组织信息</summary>
        public void Add(string name, string parentPath)
        {
            if (this.integratedMode == "OFF")
                return;

            try
            {
                string fullName = string.Format("OU={0}", name + (string.Format(",{0}", (string.IsNullOrEmpty(parentPath) ? string.Format("OU={0}", directoryName) : parentPath))));

                if (FindChildren(fullName) == null)
                {
                    DirectoryEntry param = directoryEntry.Children.Add(fullName, ActiveDirectorySchemaClassType.OrganizationalUnit);

                    param.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}", ex.Message, ex.ToString()));
            }
        }
        #endregion

        public void Remove(string fullName)
        {
            if (this.integratedMode == "OFF")
                return;

            try
            {
                DirectoryEntry param = directoryEntry.Children.Find(fullName);

                param.DeleteTree();

                param.CommitChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}", ex.Message, ex.ToString()));
            }
        }

        public bool IsExistName(string name, string parentPath)
        {
            try
            {
                string fullName = string.Format("OU={0}", name + (string.Format(",{0}", (string.IsNullOrEmpty(parentPath) ? string.Format("OU={0}", directoryName) : parentPath))));

                DirectoryEntry param = directoryEntry.Children.Find(fullName, ActiveDirectorySchemaClassType.OrganizationalUnit);

                return (param == null) ? false : true;
            }
            catch
            {
                return false;
            }
        }

        public void Rename(string name, string parentPath, string newName)
        {
            if (this.integratedMode == "OFF") { return; }

            // 新的名称和原始的名称相同，忽略此操作。
            if (name == newName)
                return;

            try
            {
                string fullName = string.Format("OU={0}", name + (string.Format(",{0}", (string.IsNullOrEmpty(parentPath) ? string.Format("OU={0}", directoryName) : parentPath))));

                DirectoryEntry param = directoryEntry.Children.Find(fullName, ActiveDirectorySchemaClassType.OrganizationalUnit);

                if (param != null)
                {
                    param.Rename(string.Format("OU={0}", newName));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}", e.Message, e.ToString()));
            }
        }

        /// <summary>查找子节点</summary>
        /// <param name="name"></param>
        /// <param name="schemaClassName"></param>
        /// <returns></returns>
        public DirectoryEntry FindChildren(string name)
        {
            if (this.integratedMode == "OFF")
                return null;

            try
            {
                DirectoryEntry result = directoryEntry.Children.Find(name, ActiveDirectorySchemaClassType.OrganizationalUnit);

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0}\r\n{1}\r\n{2}", name, ex.Message, ex.ToString()));

                return null;
            }
        }

        #region 函数:MoveTo(string parentPath, string fullName)
        /// <summary>移动组织单位</summary>
        /// <param name="fullName">组织单位的显名,例如:OU=微软技术中心,OU=合作伙伴,OU=组织结构</param>
        /// <param name="parentPath">例如:OU=微软技术中心,OU=合作伙伴,OU=组织结构</param>
        public int MoveTo(string fullName, string parentPath)
        {
            try
            {
                DirectoryEntry parent = directoryEntry.Children.Find(parentPath);

                DirectoryEntry child = directoryEntry.Children.Find(fullName);

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
    }
}