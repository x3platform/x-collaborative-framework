namespace X3Platform.Membership.ServiceObservers
{
    using System;

    using X3Platform.Services;
    using X3Platform.Services.Configuration;

    /// <summary></summary>
    public sealed class CleanUpExpiredRelationshipServiceObserver : IServiceObserver
    {
        #region 属性:Name
        private string m_Name;

        /// <summary></summary>
        public string Name
        {
            get { return m_Name; }
        }
        #endregion

        #region 属性:IsRunning
        private bool running = false;

        /// <summary>是否正在运行</summary>
        public bool IsRunning
        {
            get { return running; }
        }
        #endregion

        #region 属性:Sleeping
        /// <summary>预计下一次的运行时间</summary>
        private DateTime nextRunTime = new DateTime(2000, 1, 1);

        /// <summary></summary>
        public bool Sleeping
        {
            get { return (nextRunTime > DateTime.Now) ? true : false; }
        }
        #endregion

        /// <summary></summary>
        public CleanUpExpiredRelationshipServiceObserver()
            : this("CleanUpExpiredRelationship", string.Empty)
        {
        }

        /// <summary></summary>
        public CleanUpExpiredRelationshipServiceObserver(string name, string args)
        {
            this.m_Name = name;

            EventLogHelper.Write(string.Format("{0} 监听服务初始化。", this.Name));

            Start();
        }

        /// <summary>启动</summary>
        public void Start()
        {
            this.nextRunTime = ServicesConfigurationView.Instance.Configuration.Observers.Get(this.Name).NextRunTime;
        }

        /// <summary>关闭</summary>
        public void Close()
        {
            ServicesConfigurationView.Instance.Configuration.Observers.Get(this.Name).NextRunTime = this.nextRunTime;
            ServicesConfigurationView.Instance.Save();
        }

        /// <summary>运行</summary>
        public void Run()
        {
            if (running)
                return;

            try
            {
                running = true;

                // 开始时间
                TimeSpan beginTimeSpan = new TimeSpan(DateTime.Now.Ticks);

                ServiceTrace.Instance.WriteLine("正在清理帐号与组织的过期关系信息。");

                // MembershipManagement.Instance.OrganizationService.ClearupAllRelation();

                ServiceTrace.Instance.WriteLine("正在清理人员与角色的过期关系信息。");

                // MembershipManagement.Instance.RoleService.ClearupAllRelation();

                ServiceTrace.Instance.WriteLine("正在清理人员与群组的过期关系信息。");

                // MembershipManagement.Instance.GroupService.ClearupAllRelation();

                ServiceTrace.Instance.WriteLine("已成功清理人员的全部过期关系信息。");

                // 结束时间
                TimeSpan endTimeSpan = new TimeSpan(DateTime.Now.Ticks);

                ServiceTrace.Instance.WriteLine(string.Format("共耗时{0}秒。",
                    beginTimeSpan.Subtract(endTimeSpan).Duration().TotalSeconds));

                // 3.设置下一次运行的时间(每隔一天)
                this.nextRunTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + this.nextRunTime.ToString("HH:mm:00")).AddDays(1);

                running = false;
            }
            catch (Exception ex)
            {
                // 发生异常是, 记录异常信息  并把运行标识为false.

                ServiceTrace.Instance.WriteLine(ex.ToString());

                EventLogHelper.Write(string.Format("{0} 监听器发生异常信息\r\n{1}。", this.Name, ex));

                running = false;
            }
        }
    }
}
