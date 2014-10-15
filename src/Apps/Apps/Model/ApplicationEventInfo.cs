// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================

namespace X3Platform.Apps.Model
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>应用事件记录</summary>
    public class ApplicationEventInfo : EntityClass
    {
        #region 构造函数:ApplicationEventInfo()
        /// <summary>默认构造函数</summary>
        public ApplicationEventInfo()
        {
        }
        #endregion

        #region 属性:Id
        private string m_Id;

        /// <summary></summary>
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        #endregion

        #region 属性:ApplicationId
        private string m_ApplicationId;

        /// <summary></summary>
        public string ApplicationId
        {
            get { return m_ApplicationId; }
            set { m_ApplicationId = value; }
        }
        #endregion

        #region 属性:Tags
        private string m_Tags;

        /// <summary></summary>
        public string Tags
        {
            get { return m_Tags; }
            set { m_Tags = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description;

        /// <summary></summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:StartTime
        private DateTime m_StartTime;

        /// <summary>开始时间</summary>
        public DateTime StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }
        #endregion

        #region 属性:FinishTime
        private DateTime m_FinishTime;

        /// <summary>结束时间</summary>
        public DateTime FinishTime
        {
            get { return m_FinishTime; }
            set { m_FinishTime = value; }
        }
        #endregion

        #region 属性:TimeSpan
        private double m_TimeSpan;

        /// <summary>时间跨度</summary>
        public double TimeSpan
        {
            get { return m_TimeSpan; }
            set { m_TimeSpan = value; }
        }
        #endregion

        #region 属性:IP
        private string m_IP = "0.0.0.0";

        /// <summary></summary>
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        #endregion

        #region 属性:Date
        private DateTime m_Date;

        /// <summary></summary>
        public DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 设置 EntityClass 标识
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>实体对象标识</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion

        // -------------------------------------------------------
        // 计算时间运行时间
        // -------------------------------------------------------

        private TimeSpan timeSpan;

        /// <summary></summary>
        public void Start()
        {
            this.StartTime = DateTime.Now;

            // 结束时间
            this.timeSpan = new TimeSpan(DateTime.Now.Ticks);
        }

        /// <summary></summary>
        public void Finish()
        {
            this.FinishTime = DateTime.Now;

            this.TimeSpan = this.timeSpan.Subtract(new TimeSpan(this.FinishTime.Ticks)).Duration().TotalSeconds;
        }
    }
}
