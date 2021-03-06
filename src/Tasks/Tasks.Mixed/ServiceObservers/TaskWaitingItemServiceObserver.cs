﻿namespace X3Platform.Tasks.ServiceObservers
{
    #region Using Libraries
    using System;

    using X3Platform.Apps;
    using X3Platform.Messages;
    using X3Platform.Services;
    using X3Platform.Services.Configuration;

    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.MSMQ;
    using System.Collections.Generic;
    using X3Platform.Tasks.Configuration;
    #endregion

    /// <summary></summary>
    public sealed class TaskWaitingItemServiceObserver : IServiceObserver
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

        /// <summary>执行时间间隔(单位:秒)</summary>
        private int interval = TasksConfigurationView.Instance.WaitingItemSendingInterval;

        /// <summary></summary>
        public TaskWaitingItemServiceObserver()
            : this("TaskWaitingItem", string.Empty)
        {
        }

        /// <summary></summary>
        public TaskWaitingItemServiceObserver(string name, string args)
        {
            this.m_Name = name;

            EventLogHelper.Write(string.Format("{0} 监听服务初始化。", this.Name));

            this.Start();
        }

        /// <summary>启动</summary>
        public void Start()
        {
            this.nextRunTime = ServicesConfigurationView.Instance.Configuration.Observers[this.Name].NextRunTime;
        }

        /// <summary>关闭</summary>
        public void Close()
        {
            ServicesConfigurationView.Instance.Configuration.Observers[this.Name].NextRunTime = this.nextRunTime;
            ServicesConfigurationView.Instance.Save();
        }

        /// <summary>运行</summary>
        public void Run()
        {
            if (running) { return; }

            try
            {
                running = true;

                // 开始时间
                TimeSpan beginTimeSpan = new TimeSpan(DateTime.Now.Ticks);

                // 发送待办
                ServiceTrace.Instance.WriteLine("正在检测是否有定时任务需要发送。");

                // 接收定时待办信息
                IList<TaskWaitingItemInfo> list = TasksContext.Instance.TaskWaitingService.FindAllUnsentItems(1000);

                if (list.Count == 0)
                {
                    ServiceTrace.Instance.WriteLine("没有需要发送的定时任务。");
                }
                else
                {
                    // 发送待办
                    ServiceTrace.Instance.WriteLine("正在发送定时任务信息。");

                    int count = 0;

                    foreach (TaskWaitingItemInfo item in list)
                    {
                        TasksContext.Instance.TaskWaitingService.MoveToWorkItem(item);

                        count++;
                    }

                    // 成功发送定时待办
                    ServiceTrace.Instance.WriteLine("已成功发送【" + count + "】条定时任务信息。");
                }

                this.nextRunTime = DateTime.Now.AddSeconds(this.interval);

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
