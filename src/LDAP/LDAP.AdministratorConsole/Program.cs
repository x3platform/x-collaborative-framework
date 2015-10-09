
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

        /// <summary>ͷ����Ϣ</summary>
        private static readonly HeadingInfo headingInfo = new HeadingInfo(program.Name,
            string.Format("{0}.{1}", program.Version.Major, program.Version.Minor));

        #region ��:Options
        private sealed class Options
        {
            #region Standard Option Attribute

            //
            // ����ͬ�����ݸ��°�
            //

            [Option(null, "check-organizations-and-roles", HelpText = "��֤��֯�ṹ��")]
            public bool CheckOrganizationsAndRoles = false;

            [Option(null, "check-organizations", HelpText = "��֤��֯�ṹ(������ɫ)��")]
            public bool CheckOrganizations = false;

            [Option(null, "check-roles", HelpText = "��֤��֯�ϵĽ�ɫ")]
            public bool CheckRoles = false;

            [Option(null, "sync-organizations", HelpText = "ͬ����֯�ṹ(������ɫ)��")]
            public bool SyncOrganizations = false;

            [Option(null, "update-organization-name", HelpText = "������֯����(����ӳ���ֶ�)��")]
            public bool UpdateOrganizationName = false;

            [Option(null, "update-role-name", HelpText = "���½�ɫ����(����ӳ���ֶ�)��")]
            public bool UpdateRoleName = false;

            [Option(null, "check-groups", HelpText = "��֤����Ⱥ�顣")]
            public bool CheckGroups = false;

            [Option(null, "sync-groups", HelpText = "��ʼ������Ⱥ�顣")]
            public bool SyncGroups = false;

            [Option(null, "check-users", HelpText = "��֤�û���")]
            public bool CheckUsers = false;

            [Option(null, "sync-users", HelpText = "ͬ���û���")]
            public bool SyncUsers = false;

            [Option(null, "change-password", HelpText = "�޸����롣")]
            public bool ChangePassword = false;

            [Option(null, "check-password-strategy", HelpText = "�������������͡�")]
            public bool CheckPasswordStrategy = false;

            [Option(null, "loginName", HelpText = "��¼����Ϣ������� change-password ���������ʹ�á�")]
            public string LoginName = string.Empty;

            [Option(null, "password", HelpText = "������Ϣ������� change-password ���������ʹ�á�")]
            public string Password = string.Empty;

            /// <summary>�÷�</summary>
            /// <returns></returns>
            [HelpOption(HelpText = "��ʾ������Ϣ��")]
            public string GetUsage()
            {
                var help = new HelpText(Program.headingInfo);

                help.AdditionalNewLineAfterOption = true;

                help.Copyright = new CopyrightInfo(KernelConfigurationView.Instance.WebmasterEmail, 2008, DateTime.Now.Year);

                //help.AddPreOptionsLine("��Ա��Ȩ�޹��������й���");
                //help.AddPreOptionsLine("����ϵͳ����Ա��Ȩ�޵��ճ�ά������.\r\n");

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

        #region ��̬����:Command(Options options)
        private static void Command(Options options)
        {
            // ��ʼʱ��
            TimeSpan beginTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            if (options.CheckOrganizationsAndRoles || options.CheckOrganizations || options.CheckRoles)
            {
                // �����֯��Ϣ
                CheckOrganizationsAndRoles(options);
            }
            else if (options.SyncOrganizations)
            {
                // ��ʼ����֯��Ϣ
                SyncOrganizations(options);
            }
            else if (options.UpdateOrganizationName)
            {
                // ������֯����
                UpdateOrganizationName(options);
            }
            else if (options.UpdateRoleName)
            {
                // ���½�ɫ����
                UpdateRoleName(options);
            }
            else if (options.CheckUsers)
            {
                // ����û���Ϣ
                CheckUsers(options);
            }
            else if (options.SyncUsers)
            {
                // ͬ���û���Ϣ
                SyncUsers(options);
            }
            else if (options.CheckPasswordStrategy)
            {
                // ����������
                CheckPasswordStrategy(options);
            }
            else
            {
                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                Console.WriteLine("δִ���κ����");
                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
            }

            // ����ʱ��
            TimeSpan endTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
            Console.WriteLine("\r\nִ�н���������ʱ{0}�롣", beginTimeSpan.Subtract(endTimeSpan).Duration().TotalSeconds);
            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);

            Console.WriteLine("Pass any key to continue");

            Console.Read();
        }
        #endregion

        //-------------------------------------------------------
        // ���ܺ���
        //-------------------------------------------------------

        #region ��̬����:CheckOrganizationsAndRoles(Options options)
        /// <summary>��ʼ����֯�ṹ</summary>
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

                // ����ȫ������Ϊ�յ���Ϣ
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
                        //    Console.WriteLine("ϵͳ���Ż�ϵͳ��û����֯��{0}����", item.GlobalName);
                        //    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        //}
                    }
                    else
                    {
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("ϵͳ��Active Directory����������֯��{0}��{1}��", item.GlobalName,
                            MembershipManagement.Instance.OrganizationService.CombineDistinguishedName(item.GlobalName, item.ParentId));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();

                        MembershipManagement.Instance.OrganizationService.SyncToActiveDirectory(item);

                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
                        Console.WriteLine("ϵͳ��Active Directory���ɹ�ͬ����֯��{0}��{1}��", item.GlobalName,
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

                // ����ȫ������Ϊ�յ���Ϣ
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
                    //    Console.WriteLine("ϵͳ���Ż�ϵͳ��û�н�ɫ��{0}����", item.Name);
                    //    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                    //}
                }
                else
                {
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                    Console.WriteLine("ϵͳ��Active Directory��û�н�ɫ��{0}��{1}��",
                        item.Name,
                        MembershipManagement.Instance.RoleService.CombineDistinguishedName(item.GlobalName, organizationId));
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                    Console.ReadLine();

                    MembershipManagement.Instance.RoleService.SyncToActiveDirectory(item);

                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
                    Console.WriteLine("ϵͳ��Active Directory���ɹ�ͬ����ɫ��{0}��{1}��", item.GlobalName,
                        MembershipManagement.Instance.OrganizationService.CombineDistinguishedName(item.GlobalName, organizationId));
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                    Console.ReadLine();
                }
            }
        }
        #endregion

        #region ��̬����:SyncOrganizations(Options options)
        /// <summary>��ʼ����֯�ṹ</summary>
        static void SyncOrganizations(Options options)
        {
            SyncOrganizations("00000000-0000-0000-0000-000000000001");
        }

        static void SyncOrganizations(string parentId)
        {
            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            foreach (IOrganizationInfo item in list)
            {
                // ���˽��õ�
                if (item.Status == 0)
                    continue;

                //if (item.GlobalName.IndexOf("��ز�") == 0)
                //{
                //    item.GlobalName = item.GlobalName.Replace("��ز�", "����ز�");
                //}

                int result = MembershipManagement.Instance.OrganizationService.SyncToActiveDirectory(item);

                switch (result)
                {
                    case 0:
                        Console.WriteLine("��֯��{0}��ͬ���ɹ���", item.GlobalName);
                        break;
                    case 1:
                        Console.WriteLine("��֯��{0}������Ϊ�գ������������Ϣ��",
                        MembershipManagement.Instance.OrganizationService.GetOrganizationPathByOrganizationId(item.Id));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("��֯��{0}��ȫ������Ϊ�գ������������Ϣ��",
                        MembershipManagement.Instance.OrganizationService.GetOrganizationPathByOrganizationId(item.Id));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                }

                if (item.Name != "��������")
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
                // ���˽��õ�
                if (item.Status == 0)
                    continue;
                // ���˽�ɫ����
                if (string.IsNullOrEmpty(item.GlobalName))
                    continue;

                int result = MembershipManagement.Instance.RoleService.SyncToActiveDirectory(item);

                switch (result)
                {
                    case 0:
                        Console.WriteLine("��ɫ��{0}��ͬ���ɹ���", item.GlobalName);
                        break;
                    case 1:
                        Console.WriteLine("��ɫ��{0}��ȫ������Ϊ�գ�������������á�",
                        MembershipManagement.Instance.RoleService.CombineDistinguishedName(item.Id, item.OrganizationId));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                }
            }
        }
        #endregion

        #region ��̬����:UpdateOrganizationName(Options options)
        /// <summary>������֯������</summary>
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
                        // ��������Ϊ�յ���Ϣ
                        if (string.IsNullOrEmpty(item.Name))
                            continue;

                        // ����ȫ������Ϊ�յ���Ϣ 
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
                            Console.WriteLine("��֯���ơ�{0}({1})���Ѹ���Ϊ��{2}({3})����", originalName, originalGlobalName, item.Name, item.GlobalName);
                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        }

                        UpdateOrganizationName(command, item.Id);
                    }
                }

                connection.Close();
            }
        }
        #endregion

        #region ��̬����:UpdateOrganizationName(SqlCommand command, string parentId)
        /// <summary>������֯������</summary>
        static void UpdateOrganizationName(SqlCommand command, string parentId)
        {
            IList<IOrganizationInfo> list = MembershipManagement.Instance.OrganizationService.FindAllByParentId(parentId);

            foreach (IOrganizationInfo item in list)
            {
                // [��ʱȡ��]���˽��õ�
                // if (item.Status == 0)
                //    continue;

                // ��������Ϊ�յ���Ϣ
                if (string.IsNullOrEmpty(item.Name))
                    continue;

                // ����ȫ������Ϊ�յ���Ϣ 
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
                    Console.WriteLine("��֯���ơ�{0}({1})���Ѹ���Ϊ��{2}({3})����", originalName, originalGlobalName, item.Name, item.GlobalName);
                    CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                }

                UpdateOrganizationName(command, item.Id);
            }
        }
        #endregion

        #region ��̬����:UpdateRoleName(Options options)
        /// <summary>���½�ɫ������</summary>
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
                        // �����û����ĵĵĽ�ɫ
                        // ����

                        // [��ʱȡ��]���˽��õ�
                        // if (item.Status == 0)
                        //    continue;

                        // ��������Ϊ�յ���Ϣ
                        if (string.IsNullOrEmpty(item.Name))
                            continue;

                        // ����ȫ������Ϊ�յ���Ϣ 
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
                            Console.WriteLine("��ɫ���ơ�{0}���Ѹ���Ϊ��{1}����", originalGlobalName, item.GlobalName);
                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        }
                    }
                }

                connection.Close();
            }
        }
        #endregion

        #region ��̬����:CheckUsers(Options options)
        /// <summary>��ʼ����֯�ṹ</summary>
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
                        // [��ʱȡ��]���˽��õ�
                        // if (item.Status == 0)
                        //    continue;

                        // ���˵�¼��Ϊ�յ���Ϣ
                        if (string.IsNullOrEmpty(item.LoginName))
                            continue;

                        // ����ȫ������Ϊ�յ���Ϣ 
                        if (string.IsNullOrEmpty(item.GlobalName))
                            continue;

                        if (string.IsNullOrEmpty(item.GlobalName))
                        {
                            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                            Console.WriteLine("�û���{0}({1})����ȫ������Ϊ�գ�������������á�",
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
                                Console.WriteLine("ϵͳ���Ż�ϵͳ��û���û���{0}({1})����", item.Name, item.LoginName);
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                            }
                        }
                        else
                        {
                            if (ActiveDirectoryManagement.Instance.User.IsExist(item.LoginName, item.GlobalName))
                            {
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                                Console.WriteLine("�û���{0}({1})����ȫ�������ѱ������˴�����Ψһ���ƣ�{2}��",
                                    item.Name,
                                    item.LoginName,
                                    MembershipManagement.Instance.AccountService.CombineDistinguishedName(item.GlobalName));
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                                Console.ReadLine(); // "�û���${Name}(${LoginName})����ȫ�������ѱ������˴�����������������á�
                            }
                            else
                            {
                                CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                                Console.WriteLine("�û���{0}({1})�������ڣ�Ψһ���ƣ�{2}��",
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

        #region ��̬����:SyncUsers(Options options)
        /// <summary>��ʼ����֯�ṹ</summary>
        static void SyncUsers(Options options)
        {
            IList<IAccountInfo> list = MembershipManagement.Instance.AccountService.FindAll();

            foreach (IAccountInfo item in list)
            {
                // ���˽��õ�
                if (item.Status == 0)
                    continue;

                // ���˵�¼��Ϊ�յ���Ϣ
                if (string.IsNullOrEmpty(item.LoginName))
                    continue;

                // ����ȫ������Ϊ�յ���Ϣ
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
                        Console.WriteLine("�û���{0}({1})��ͬ���ɹ���", item.Name, item.LoginName);
                        break;
                    case 1:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("�û���{0}({1})����¼��Ϊ�գ�������������á�",
                            item.Name,
                            item.LoginName,
                            MembershipManagement.Instance.AccountService.CombineDistinguishedName(item.GlobalName));
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 2:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("�û���{0}({1})��ȫ������Ϊ�գ�������������á�",
                            item.Name,
                            item.LoginName);
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 3:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.LightRed);
                        Console.WriteLine("�û���{0}({1})����ȫ�����ơ�{2}���ѱ������˴�����������������á�",
                            item.Name,
                            item.LoginName,
                            item.GlobalName);
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                    case 4:
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.DarkGray);
                        Console.WriteLine("�û���{0}({1})�����ʺ�Ϊ�����á�״̬�������Ҫ���� Active Directory �ʺţ�������������á�",
                            item.Name,
                            item.LoginName);
                        CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);
                        Console.ReadLine();
                        break;
                }
            }
        }
        #endregion

        #region ��̬����:CheckPasswordStrategy(Options options)
        static void CheckPasswordStrategy(Options options)
        {
            bool result = ActiveDirectoryManagement.Instance.User.CheckPasswordStrategy(options.Password);

            Console.WriteLine("��������:��{0}�� \t���:{1}", options.Password, result);
        }
        #endregion
    }
}