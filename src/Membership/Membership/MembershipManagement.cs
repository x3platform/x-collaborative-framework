namespace X3Platform.Membership
{
    using System;

    using Common.Logging;

    using X3Platform.Plugins;
    using X3Platform.Security.Authentication;
    using X3Platform.Spring;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Configuration;
    using X3Platform.Membership.Passwords;

    /// <summary>人员关系管理</summary>
    public sealed class MembershipManagement : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 属性:Name
        /// <summary></summary>
        public override string Name
        {
            get { return "人员及权限管理"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile MembershipManagement instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static MembershipManagement Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new MembershipManagement();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private MembershipConfiguration configuration = null;

        /// <summary>配置</summary>
        public MembershipConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        #region 属性:PasswordEncryptionManagement
        private IPasswordEncryptionManagement m_PasswordEncryptionManagement;

        /// <summary>密码加密管理</summary>
        public IPasswordEncryptionManagement PasswordEncryptionManagement
        {
            get { return this.m_PasswordEncryptionManagement; }
        }
        #endregion

        #region 属性:AuthorizationObjectService
        private IAuthorizationObjectService m_AuthorizationObjectService;

        /// <summary>授权对象</summary>
        public IAuthorizationObjectService AuthorizationObjectService
        {
            get { return this.m_AuthorizationObjectService; }
        }
        #endregion

        #region 属性:AccountService
        private IAccountService m_AccountService;

        /// <summary>帐号</summary>
        public IAccountService AccountService
        {
            get { return this.m_AccountService; }
        }
        #endregion

        #region 属性:AccountLogService
        private IAccountLogService m_AccountLogService;

        /// <summary>帐号日志</summary>
        public IAccountLogService AccountLogService
        {
            get { return this.m_AccountLogService; }
        }
        #endregion

        #region 属性:AccountGrantService
        private IAccountGrantService m_AccountGrantService;

        /// <summary>帐号委托</summary>
        public IAccountGrantService AccountGrantService
        {
            get { return this.m_AccountGrantService; }
        }
        #endregion

        #region 属性:MemberService
        private IMemberService m_MemberService = null;

        /// <summary>成员</summary>
        public IMemberService MemberService
        {
            get { return this.m_MemberService; }
        }
        #endregion

        #region 属性:OrganizationService
        private IOrganizationService m_OrganizationService = null;

        /// <summary>组织结构</summary>
        public IOrganizationService OrganizationService
        {
            get { return this.m_OrganizationService; }
        }
        #endregion

        #region 属性:RoleService
        private IRoleService m_RoleService = null;

        /// <summary>角色信息</summary>
        public IRoleService RoleService
        {
            get { return this.m_RoleService; }
        }
        #endregion

        #region 属性:GeneralRoleService
        private IGeneralRoleService m_GeneralRoleService = null;

        /// <summary>通用角色信息</summary>
        public IGeneralRoleService GeneralRoleService
        {
            get { return this.m_GeneralRoleService; }
        }
        #endregion

        #region 属性:StandardOrganizationService
        private IStandardOrganizationService m_StandardOrganizationService = null;

        /// <summary>标准组织信息</summary>
        public IStandardOrganizationService StandardOrganizationService
        {
            get { return this.m_StandardOrganizationService; }
        }
        #endregion

        #region 属性:StandardGeneralRoleService
        private IStandardGeneralRoleService m_StandardGeneralRoleService = null;

        /// <summary>标准通用角色信息</summary>
        public IStandardGeneralRoleService StandardGeneralRoleService
        {
            get { return this.m_StandardGeneralRoleService; }
        }
        #endregion

        #region 属性:StandardRoleService
        private IStandardRoleService m_StandardRoleService = null;

        /// <summary>标准角色信息</summary>
        public IStandardRoleService StandardRoleService
        {
            get { return this.m_StandardRoleService; }
        }
        #endregion

        #region 属性:ContactService
        private IContactService m_ContactService = null;

        /// <summary>群组分类树形信息</summary>
        public IContactService ContactService
        {
            get { return this.m_ContactService; }
        }
        #endregion

        #region 属性:GroupService
        private IGroupService m_GroupService = null;

        /// <summary>群组信息</summary>
        public IGroupService GroupService
        {
            get { return this.m_GroupService; }
        }
        #endregion

        #region 属性:GroupTreeService
        private IGroupTreeService m_GroupTreeService = null;

        /// <summary>群组分类树形信息</summary>
        public IGroupTreeService GroupTreeService
        {
            get { return this.m_GroupTreeService; }
        }
        #endregion

        #region 属性:GroupTreeNodeService
        private IGroupTreeNodeService m_GroupTreeNodeService = null;

        /// <summary>群组分类树形节点信息</summary>
        public IGroupTreeNodeService GroupTreeNodeService
        {
            get { return this.m_GroupTreeNodeService; }
        }
        #endregion

        #region 属性:AssignedJobService
        private IAssignedJobService m_AssignedJobService = null;

        /// <summary>岗位信息</summary>
        public IAssignedJobService AssignedJobService
        {
            get { return this.m_AssignedJobService; }
        }
        #endregion

        #region 属性:JobFamilyService
        private IJobFamilyService m_JobFamilyService = null;

        /// <summary>职级信息</summary>
        public IJobFamilyService JobFamilyService
        {
            get { return this.m_JobFamilyService; }
        }
        #endregion

        #region 属性:JobGradeService
        private IJobGradeService m_JobGradeService = null;

        /// <summary>职级信息</summary>
        public IJobGradeService JobGradeService
        {
            get { return this.m_JobGradeService; }
        }
        #endregion

        #region 属性:JobService
        private IJobService m_JobService = null;

        /// <summary>职位信息</summary>
        public IJobService JobService
        {
            get { return this.m_JobService; }
        }
        #endregion

        #region 属性:SettingService
        private ISettingService m_SettingService = null;

        /// <summary>参数信息</summary>
        public ISettingService SettingService
        {
            get { return this.m_SettingService; }
        }
        #endregion

        #region 构造函数:MembershipManagement()
        /// <summary>构造函数</summary>
        private MembershipManagement()
        {
            this.Restart();
        }
        #endregion

        /// <summary>重启次数计数器</summary>
        private int restartCount = 0;

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();

                // 自增重启次数计数器
                this.restartCount++;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw ex;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            if (this.restartCount > 0)
            {
                // 重新加载配置信息
                MembershipConfigurationView.Instance.Reload();
            }

            this.configuration = MembershipConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(MembershipConfiguration.ApplicationName, springObjectFile);

            this.m_PasswordEncryptionManagement = objectBuilder.GetObject<IPasswordEncryptionManagement>(typeof(IPasswordEncryptionManagement));

            this.m_AuthorizationObjectService = objectBuilder.GetObject<IAuthorizationObjectService>(typeof(IAuthorizationObjectService));
            this.m_AccountService = objectBuilder.GetObject<IAccountService>(typeof(IAccountService));
            this.m_AccountLogService = objectBuilder.GetObject<IAccountLogService>(typeof(IAccountLogService));
            this.m_AccountGrantService = objectBuilder.GetObject<IAccountGrantService>(typeof(IAccountGrantService));
            this.m_MemberService = objectBuilder.GetObject<IMemberService>(typeof(IMemberService));
            this.m_OrganizationService = objectBuilder.GetObject<IOrganizationService>(typeof(IOrganizationService));
            this.m_RoleService = objectBuilder.GetObject<IRoleService>(typeof(IRoleService));
            this.m_GeneralRoleService = objectBuilder.GetObject<IGeneralRoleService>(typeof(IGeneralRoleService));
            this.m_StandardGeneralRoleService = objectBuilder.GetObject<IStandardGeneralRoleService>(typeof(IStandardGeneralRoleService));
            this.m_StandardRoleService = objectBuilder.GetObject<IStandardRoleService>(typeof(IStandardRoleService));
            this.m_StandardOrganizationService = objectBuilder.GetObject<IStandardOrganizationService>(typeof(IStandardOrganizationService));
            this.m_GroupService = objectBuilder.GetObject<IGroupService>(typeof(IGroupService));
            this.m_GroupTreeService = objectBuilder.GetObject<IGroupTreeService>(typeof(IGroupTreeService));
            this.m_GroupTreeNodeService = objectBuilder.GetObject<IGroupTreeNodeService>(typeof(IGroupTreeNodeService));
            this.m_ContactService = objectBuilder.GetObject<IContactService>(typeof(IContactService));
            this.m_AssignedJobService = objectBuilder.GetObject<IAssignedJobService>(typeof(IAssignedJobService));
            this.m_JobFamilyService = objectBuilder.GetObject<IJobFamilyService>(typeof(IJobFamilyService));
            this.m_JobGradeService = objectBuilder.GetObject<IJobGradeService>(typeof(IJobGradeService));
            this.m_JobService = objectBuilder.GetObject<IJobService>(typeof(IJobService));
            this.m_SettingService = objectBuilder.GetObject<ISettingService>(typeof(ISettingService));
            this.m_GroupTreeService = objectBuilder.GetObject<IGroupTreeService>(typeof(IGroupTreeService));
            this.m_GroupTreeNodeService = objectBuilder.GetObject<IGroupTreeNodeService>(typeof(IGroupTreeNodeService));
        }
        #endregion
    }
}
