#region Copyright & Author
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
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;

    using X3Platform.Util;
    #endregion

    /// <summary>Ӧ�����ӵ�����Ϣ</summary>
    public class ConnectCallInfo : EntityClass
    {
        public ConnectCallInfo()
        {
            this.m_Id = DateTime.Now.ToString("yyyyMMddHHmmssff") + StringHelper.ToRandom("0123456789", 6);
        }

        public ConnectCallInfo(string appKey, string requestUri, string requestData)
            : this()
        {
            this.m_AppKey = appKey;
            this.m_RequestUri = requestUri;
            this.m_RequestData = requestData;
        }

        #region 属性:Id
        private string m_Id;

        /// <summary>��ʶ</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:AppKey
        private string m_AppKey;

        /// <summary>Ӧ�ñ�ʶ</summary>
        public string AppKey
        {
            get { return this.m_AppKey; }
            set { this.m_AppKey = value; }
        }
        #endregion

        #region 属性:RequestUri
        private string m_RequestUri = string.Empty;

        /// <summary>�����ĵ�ַ��Ϣ</summary>
        public string RequestUri
        {
            get { return this.m_RequestUri; }
            set { this.m_RequestUri = value; }
        }
        #endregion

        #region 属性:RequestData
        private string m_RequestData = string.Empty;

        /// <summary>������������Ϣ</summary>
        public string RequestData
        {
            get { return this.m_RequestData; }
            set { this.m_RequestData = value; }
        }
        #endregion

        #region 属性:StartTime
        private DateTime m_StartTime;

        /// <summary>��ʼʱ��</summary>
        public DateTime StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }
        #endregion

        #region 属性:FinishTime
        private DateTime m_FinishTime;

        /// <summary>����ʱ��</summary>
        public DateTime FinishTime
        {
            get { return m_FinishTime; }
            set { m_FinishTime = value; }
        }
        #endregion

        #region 属性:TimeSpan
        private double m_TimeSpan;

        /// <summary>ʱ������</summary>
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
        #region 属性:ReturnCode
        private int m_ReturnCode = 0;

        /// <summary></summary>
        public int ReturnCode
        {
            get { return this.m_ReturnCode; }
            set { this.m_ReturnCode = value; }
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
        // ���� EntityClass ��ʶ
        // -------------------------------------------------------

        #region 属性:EntityId
        /// <summary>ʵ��������ʶ</summary>
        public override string EntityId
        {
            get { return this.Id; }
        }
        #endregion

        // -------------------------------------------------------
        // ����ʱ������ʱ��
        // -------------------------------------------------------

        private TimeSpan timeSpan;

        /// <summary></summary>
        public void Start()
        {
            this.StartTime = DateTime.Now;

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
