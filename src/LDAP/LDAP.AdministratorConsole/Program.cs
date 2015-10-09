
namespace Elane.X.ActiveDirectory.AdministratorConsole
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Elane.X.CommandLine;
    using Elane.X.CommandLine.Attributes;
    using Elane.X.CommandLine.Parser;
    using Elane.X.CommandLine.Text;
    using Elane.X.Configuration;
    using Elane.X.Membership;
    using System.Data.SqlClient;
    using System.Data;
    using System.Configuration;

    static class Program
    {
        private static readonly AssemblyName program = Assembly.GetExecutingAssembly().GetName();

        /// <summary>头部信息</summary>
        private static readonly HeadingInfo headingInfo = new HeadingInfo(program.Name,
            string.Format("{0}.{1}", program.Version.Major, program.Version.Minor));

        #region 类:Options
        private sealed class Options
        {
            #region Standard Option Attribute

            //
            // 创建同步数据更新包
            //

            [Option(null, "check-organizations-and-roles", HelpText = "验证组织结构。")]
            public bool CheckOrganizationsAndRoles = false;

            [Option(null, "check-organizations", HelpText = "验证组织结构(包括角色)。")]
            public bool CheckOrganizations = false;

            [Option(null, "check-roles", HelpText = "验证组织上的角色")]
            public bool CheckRoles = false;

            [Option(null, "sync-organizations", HelpText = "同步组织结构(包括角色)。")]
            public bool SyncOrganizations = false;

            [Option(null, "update-organization-name", HelpText = "更新组织名称(根据映射字段)。")]
            public bool UpdateOrganizationName = false;

            [Option(null, "update-role-name", HelpText = "更新角色名称(根据映射字段)。")]
            public bool UpdateRoleName = false;

            [Option(null, "check-groups", HelpText = "验证常用群组。")]
            public bool CheckGroups = false;

            [Option(null, "sync-groups", HelpText = "初始化常用群组。")]
            public bool SyncGroups = false;

            [Option(null, "check-users", HelpText = "验证用户。")]
            public bool CheckUsers = false;

            [Option(null, "sync-users", HelpText = "同步用户。")]
            public bool SyncUsers = false;

            [Option(null, "change-password", HelpText = "修改密码。")]
            public bool ChangePassword = false;

            [Option(null, "check-password-strategy", HelpText = "检测密码策略类型。")]
            public bool CheckPasswordStrategy = false;

            [Option(null, "loginName", HelpText = "登录名信息，必须和 change-password 等命令配合使用。")]
            public string LoginName = string.Empty;

            [Option(null, "password", HelpText = "密码信息，必须和 change-password 等命令配合使用。")]
            public string Password = string.Empty;

            /// <summary>用法</summary>
            /// <returns></returns>
            [HelpOption(HelpText = "显示帮助信息。")]
            public string GetUsage()
            {
                var help = new HelpText(Program.headingInfo);

                help.AdditionalNewLineAfterOption = true;

                help.Copyright = new CopyrightInfo(KernelConfigurationView.Instance.WebmasterEmail, 2008, DateTime.Now.Year);

                //help.AddPreOptionsLine("人员及权限管理命令行工具");
                //help.AddPreOptionsLine("用于系统的人员及权限的日常维护管理.\r\n");

                help.AddPreOptionsLine("Usage:");
                help.AddPreOptionsLine(string.Format("  {0} --create-full-sync-pack", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --create-sync-pack", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --create-sync-pack --sync-pack-begin 2000-01-01 --sync-pack-end 2010-01-01 ", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --check-password-strategy --password 123456", program.Name));
                help.AddPreOptionsLine(string.Format("  {0} --help", program.Name));
                help.AddOptions(this);

                return help;
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var options = new Options();

                if (args.Length == 0)
                {
                    Console.WriteLine(options.GetUsage());
                    Environment.Exit(0);
                }

                ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(Console.Error));

                if (!parser.ParseArguments(args, options))
                    Environment.Exit(1);

                Command(options);

            }
            catch (Exception ex)
            {
                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                Console.WriteLine(ex);
            }

            Environment.Exit(0);
        }

        #region 静态函数:Command(Options options)
        private static void Command(Options options)
        {
            // 开始时间
            TimeSpan beginTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            if (options.CheckOrganizationsAndRoles || options.CheckOrganizations || options.CheckRoles)
            {
                // 检测组织信息
                CheckOrganizationsAndRoles(options);
            }
            else if (options.SyncOrganizations)
            {
                // 初始化组织信息
                SyncOrganizations(options);
            }
            else if (options.UpdateOrganizationName)
            {
                // 更新组织名称
                UpdateOrganizationName(options);
            }
            else if (options.UpdateRoleName)
            {
                // 更新角色名称
                UpdateRoleName(options);
            }
            else if (options.CheckUsers)
            {
                // 检测用户信息
                CheckUsers(options);
            }
            else if (options.SyncUsers)
            {
                // 同步用户信息
                SyncUsers(options);
            }
            else if (options.CheckPasswordStrategy)
            {
                // 检测密码策略
                CheckPasswordStrategy(options);
            }
            else
            {
                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                Console.WriteLine("未执行任何命令。");
                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
            }

            // 结束时间
            TimeSpan endTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
            Console.WriteLine("\r\n执行结束，共耗时{0}秒。", beginTimeSpan.Subtract(endTimeSpan).Duration().TotalSeconds);
            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);

            Console.WriteLine("Pass any key to continue");

            Console.Read();
        }
        #endregion

        //-------------------------------------------------------
        // 功能函数
        //-------------------------------------------------------

        #region 静态函数:CheckOrganizationsAndRoles(Options options)
        /// <summary>初始化组织结构</summary>
        static void CheckOrganizationsAndRoles(Options options)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LH_BDM"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;

                    CheckOrganizations(options, command, "00000000-0000-0000-0000-000000000001");
                }

                connection.Close();
            }
        }

        static void CheckOrganizations(Options options, SqlCommand command, string parentId)
        {
            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            foreach (IOrganizationInfo item in list)
            {
                if (item.Status == 0)
                    continue;

                // 过滤全局名称为空的信息
                if (string.IsNullOrEmpty(item.GlobalName))
                    continue;

                if (options.CheckOrganizationsAndRoles || options.CheckOrganizations)
                {
                    if (ActiveDirectoryManagement.Instance.Group.IsExistName(item.GlobalName))
                    {
                        //command.CommandText = string.Format("UPDATE Orgs SET MappingToken = '{1}' WHERE OrgGlobalName = '{0}' ",
                        //    item.GlobalName, item.Id);

                        //int rowsAffected = command.ExecuteNonQuery();

                        //if (rowsAffected == 0)
                        //{
                        //    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        //    Console.WriteLine("系统【门户系统】没有组织【{0}】。", item.GlobalName);
                        //    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        //}
                    }
                    else
                    {
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("系统【Active Directory】不存在组织【{0}】{1}。", item.GlobalName,
                            MembershipManagement.Instance.OrganizationService.CombineDistinguishedName(item.GlobalName, item.ParentId));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();

                        MembershipManagement.Instance.OrganizationService.SyncToActiveDirectory(item);

                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
                        Console.WriteLine("系统【Active Directory】成功同步组织【{0}】{1}。", item.GlobalName,
                            MembershipManagement.Instance.OrganizationService.CombineDistinguishedName(item.GlobalName, item.ParentId));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                    }
                }

                if (options.CheckOrganizationsAndRoles || options.CheckRoles)
                {
                    CheckRoles(command, item.Id);
                }

                CheckOrganizations(options, command, item.Id);
            }
        }

        static void CheckRoles(SqlCommand command, string organizationId)
        {
            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAllByOrganizationId(organizationId);

            foreach (IRoleInfo item in list)
            {
                if (item.Status == 0)
                    continue;

                // 过滤全局名称为空的信息
                if (string.IsNullOrEmpty(item.GlobalName))
                    continue;

                if (ActiveDirectoryManagement.Instance.Group.IsExistName(item.GlobalName))
                {
                    //command.CommandText = string.Format("UPDATE Posts SET MappingToken = '{1}' WHERE PostName = '{0}' ",
                    //    item.Name, item.Id);

                    //int rowsAffected = command.ExecuteNonQuery();

                    //if (rowsAffected == 0)
                    //{
                    //    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                    //    Console.WriteLine("系统【门户系统】没有角色【{0}】。", item.Name);
                    //    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                    //}
                }
                else
                {
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                    Console.WriteLine("系统【Active Directory】没有角色【{0}】{1}。",
                        item.Name,
                        MembershipManagement.Instance.RoleService.CombineDistinguishedName(item.GlobalName, organizationId));
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                    Console.ReadLine();

                    MembershipManagement.Instance.RoleService.SyncToActiveDirectory(item);

                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
                    Console.WriteLine("系统【Active Directory】成功同步角色【{0}】{1}。", item.GlobalName,
                        MembershipManagement.Instance.OrganizationService.CombineDistinguishedName(item.GlobalName, organizationId));
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                    Console.ReadLine();
                }
            }
        }
        #endregion

        #region 静态函数:SyncOrganizations(Options options)
        /// <summary>初始化组织结构</summary>
        static void SyncOrganizations(Options options)
        {
            SyncOrganizations("00000000-0000-0000-0000-000000000001");
        }

        static void SyncOrganizations(string parentId)
        {
            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            foreach (IOrganizationInfo item in list)
            {
                // 过滤禁用的
                if (item.Status == 0)
                    continue;

                //if (item.GlobalName.IndexOf("渝地产") == 0)
                //{
                //    item.GlobalName = item.GlobalName.Replace("渝地产", "重庆地产");
                //}

                int result = MembershipManagement.Instance.OrganizationService.SyncToActiveDirectory(item);

                switch (result)
                {
                    case 0:
                        Console.WriteLine("组织【{0}】同步成功。", item.GlobalName);
                        break;
                    case 1:
                        Console.WriteLine("组织【{0}】名称为空，请配置相关信息。",
                        MembershipManagement.Instance.OrganizationService.GetOrganizationPathByOrganizationId(item.Id));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("组织【{0}】全局名称为空，请配置相关信息。",
                        MembershipManagement.Instance.OrganizationService.GetOrganizationPathByOrganizationId(item.Id));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                }

                if (item.Name != "废弃部门")
                {
                    SyncRoles(item.Id);

                    SyncOrganizations(item.Id);
                }
            }
        }

        static void SyncRoles(string organizationId)
        {
            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAllByOrganizationId(organizationId);

            foreach (IRoleInfo item in list)
            {
                // 过滤禁用的
                if (item.Status == 0)
                    continue;
                // 过滤角色名称
                if (string.IsNullOrEmpty(item.GlobalName))
                    continue;

                int result = MembershipManagement.Instance.RoleService.SyncToActiveDirectory(item);

                switch (result)
                {
                    case 0:
                        Console.WriteLine("角色【{0}】同步成功。", item.GlobalName);
                        break;
                    case 1:
                        Console.WriteLine("角色【{0}】全局名称为空，请设置相关配置。",
                        MembershipManagement.Instance.RoleService.CombineDistinguishedName(item.Id, item.OrganizationId));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                }
            }
        }
        #endregion

        #region 静态函数:UpdateOrganizationName(Options options)
        /// <summary>更新组织的名称</summary>
        static void UpdateOrganizationName(Options options)
        {
            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId("00000000-0000-0000-0000-000000000001");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LH_BDM"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;

                    foreach (IOrganizationInfo item in list)
                    {
                        // 过滤名称为空的信息
                        if (string.IsNullOrEmpty(item.Name))
                            continue;

                        // 过滤全局名称为空的信息 
                        if (string.IsNullOrEmpty(item.GlobalName))
                            continue;

                        command.CommandText = string.Format("SELECT OrgName, OrgGlobalName FROM Orgs WHERE MappingToken = '{0}' ", item.Id);

                        string originalName = null;

                        string originalGlobalName = null;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                originalName = reader.GetString(0);

                                originalGlobalName = reader.GetString(1);

                                if (ActiveDirectoryManagement.Instance.Group.IsExistName(originalGlobalName))
                                {
                                    ActiveDirectoryManagement.Instance.Group.Rename(originalGlobalName, item.GlobalName);

                                    ActiveDirectoryManagement.Instance.Organization.Rename(originalName,
                                        MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId("00000000-0000-0000-0000-000000000001"),
                                        item.Name);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(originalGlobalName))
                        {
                            command.CommandText = string.Format("UPDATE Orgs SET OrgName = '{0}', OrgGlobalName = '{1}', ActionDate = GetDate() WHERE MappingToken = '{2}' ", item.Name, item.GlobalName, item.Id);

                            command.ExecuteNonQuery();

                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
                            Console.WriteLine("组织名称【{0}({1})】已更新为【{2}({3})】。", originalName, originalGlobalName, item.Name, item.GlobalName);
                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        }

                        UpdateOrganizationName(command, item.Id);
                    }
                }

                connection.Close();
            }
        }
        #endregion

        #region 静态函数:UpdateOrganizationName(SqlCommand command, string parentId)
        /// <summary>更新组织的名称</summary>
        static void UpdateOrganizationName(SqlCommand command, string parentId)
        {
            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            foreach (IOrganizationInfo item in list)
            {
                // [暂时取消]过滤禁用的
                // if (item.Status == 0)
                //    continue;

                // 过滤名称为空的信息
                if (string.IsNullOrEmpty(item.Name))
                    continue;

                // 过滤全局名称为空的信息 
                if (string.IsNullOrEmpty(item.GlobalName))
                    continue;

                command.CommandText = string.Format("SELECT OrgName, OrgGlobalName FROM Orgs WHERE MappingToken = '{0}' ", item.Id);

                string originalName = null;

                string originalGlobalName = null;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        originalName = reader.GetString(0);

                        originalGlobalName = reader.GetString(1);

                        if (ActiveDirectoryManagement.Instance.Group.IsExistName(originalGlobalName))
                        {
                            ActiveDirectoryManagement.Instance.Group.Rename(originalGlobalName, item.GlobalName);

                            ActiveDirectoryManagement.Instance.Organization.Rename(originalName,
                                MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(parentId),
                                item.Name);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(originalGlobalName))
                {
                    //command.CommandText = string.Format("UPDATE Orgs SET OrgName = '{0}', OrgGlobalName = '{1}', ActionDate = GetDate() WHERE MappingToken = '{2}' ", item.Name, item.GlobalName, item.Id);

                    //command.ExecuteNonQuery();

                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
                    Console.WriteLine("组织名称【{0}({1})】已更新为【{2}({3})】。", originalName, originalGlobalName, item.Name, item.GlobalName);
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                }

                UpdateOrganizationName(command, item.Id);
            }
        }
        #endregion

        #region 静态函数:UpdateRoleName(Options options)
        /// <summary>更新角色的名称</summary>
        static void UpdateRoleName(Options options)
        {
            IList<IRoleInfo> list = MembershipManagement.Instance.RoleService.FindAll();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LH_BDM"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;

                    foreach (IRoleInfo item in list)
                    {
                        // 根据用户中心的的角色
                        // 更具

                        // [暂时取消]过滤禁用的
                        // if (item.Status == 0)
                        //    continue;

                        // 过滤名称为空的信息
                        if (string.IsNullOrEmpty(item.Name))
                            continue;

                        // 过滤全局名称为空的信息 
                        if (string.IsNullOrEmpty(item.GlobalName))
                            continue;

                        command.CommandText = string.Format("SELECT PostName FROM Posts WHERE MappingToken = '{0}' ", item.Id);

                        string originalGlobalName = null;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                originalGlobalName = reader.GetString(0);

                                if (ActiveDirectoryManagement.Instance.Group.IsExistName(originalGlobalName))
                                {
                                    ActiveDirectoryManagement.Instance.Group.Rename(originalGlobalName, item.GlobalName);
                                    ActiveDirectoryManagement.Instance.Group.MoveTo(item.GlobalName,
                                        MembershipManagement.Instance.OrganizationService.GetActiveDirectoryOUPathByOrganizationId(item.OrganizationId));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(originalGlobalName))
                        {
                            //command.CommandText = string.Format("UPDATE Posts SET PostName = '{0}', ActionDate = GetDate() WHERE MappingToken = '{1}' ", item.GlobalName, item.Id);

                            //command.ExecuteNonQuery();

                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
                            Console.WriteLine("角色名称【{0}】已更新为【{1}】。", originalGlobalName, item.GlobalName);
                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        }
                    }
                }

                connection.Close();
            }
        }
        #endregion

        #region 静态函数:CheckUsers(Options options)
        /// <summary>初始化组织结构</summary>
        static void CheckUsers(Options options)
        {
            IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.FindAll();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LH_BDM"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.Connection = connection;

                    foreach (IAccountInfo item in list)
                    {
                        // [暂时取消]过滤禁用的
                        // if (item.Status == 0)
                        //    continue;

                        // 过滤登录名为空的信息
                        if (string.IsNullOrEmpty(item.LoginName))
                            continue;

                        // 过滤全局名称为空的信息 
                        if (string.IsNullOrEmpty(item.GlobalName))
                            continue;

                        if (string.IsNullOrEmpty(item.GlobalName))
                        {
                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                            Console.WriteLine("用户【{0}({1})】的全局名称为空，请设置相关配置。",
                                item.Name,
                                item.LoginName);
                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                            Console.ReadLine();
                        }
                        else if (ActiveDirectoryManagement.Instance.User.IsExistLoginName(item.LoginName))
                        {
                            command.CommandText = string.Format("UPDATE Users SET MappingToken = '{1}' WHERE LoginId = '{0}' ",
                                item.LoginName, item.Id);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                                Console.WriteLine("系统【门户系统】没有用户【{0}({1})】。", item.Name, item.LoginName);
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                            }
                        }
                        else
                        {
                            if (ActiveDirectoryManagement.Instance.User.IsExist(item.LoginName, item.GlobalName))
                            {
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                                Console.WriteLine("用户【{0}({1})】的全局名称已被其他人创建，唯一名称：{2}。",
                                    item.Name,
                                    item.LoginName,
                                    MembershipManagement.Instance.AccountService.CombineDistinguishedName(item.GlobalName));
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                                Console.ReadLine(); // "用户【${Name}(${LoginName})】的全局名称已被其他人创建，请设置相关配置。
                            }
                            else
                            {
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                                Console.WriteLine("用户【{0}({1})】不存在，唯一名称：{2}。",
                                    item.Name,
                                    item.LoginName,
                                    MembershipManagement.Instance.AccountService.CombineDistinguishedName(item.GlobalName));
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                                Console.ReadLine();
                            }
                        }
                    }
                }

                connection.Close();
            }
        }
        #endregion

        #region 静态函数:SyncUsers(Options options)
        /// <summary>初始化组织结构</summary>
        static void SyncUsers(Options options)
        {
            IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.FindAll();

            foreach (IAccountInfo item in list)
            {
                // 过滤禁用的
                if (item.Status == 0)
                    continue;

                // 过滤登录名为空的信息
                if (string.IsNullOrEmpty(item.LoginName))
                    continue;

                // 过滤全局名称为空的信息
                if (string.IsNullOrEmpty(item.GlobalName))
                    continue;

                int result = MembershipManagement.Instance.AccountService.SyncToActiveDirectory(item);

                if (result == 0)
                {
                    string accountId = item.Id;

                    if (!string.IsNullOrEmpty(accountId))
                    {
                        foreach (IRoleInfo role in item.Roles)
                        {
                            MembershipManagement.Instance.RoleService.AddRelation(accountId, role.Id);

                            MembershipManagement.Instance.OrganizationService.AddRelation(accountId, role.OrganizationId);

                            MembershipManagement.Instance.OrganizationService.AddParentRelations(accountId, role.OrganizationId);
                        }
                    }
                }

                switch (result)
                {
                    case 0:
                        Console.WriteLine("用户【{0}({1})】同步成功。", item.Name, item.LoginName);
                        break;
                    case 1:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("用户【{0}({1})】登录名为空，请设置相关配置。",
                            item.Name,
                            item.LoginName,
                            MembershipManagement.Instance.AccountService.CombineDistinguishedName(item.GlobalName));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 2:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("用户【{0}({1})】全局名称为空，请设置相关配置。",
                            item.Name,
                            item.LoginName);
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 3:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("用户【{0}({1})】的全局名称【{2}】已被其他人创建，请设置相关配置。",
                            item.Name,
                            item.LoginName,
                            item.GlobalName);
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 4:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.DarkGray);
                        Console.WriteLine("用户【{0}({1})】的帐号为【禁用】状态，如果需要创建 Active Directory 帐号，请设置相关配置。",
                            item.Name,
                            item.LoginName);
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                }
            }
        }
        #endregion

        #region 静态函数:CheckPasswordStrategy(Options options)
        static void CheckPasswordStrategy(Options options)
        {
            bool result = ActiveDirectoryManagement.Instance.User.CheckPasswordStrategy(options.Password);

            Console.WriteLine("测试密码:【{0}】 \t结果:{1}", options.Password, result);
        }
        #endregion
    }
}