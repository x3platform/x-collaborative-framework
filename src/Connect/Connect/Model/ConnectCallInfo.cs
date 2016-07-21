namespace X3Platform.Connect.Model
{
    #region Using Libraries
    using System;
    using System.Text;
    using System.Web;
    using System.Xml;
    using X3Platform.Messages;
    using X3Platform.Util;
    #endregion

    /// <summary>应用连接调用信息</summary>
    public class ConnectCallInfo : IMessageObject, ISerializedObject
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

        /// <summary>标识</summary>
        public string Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; }
        }
        #endregion

        #region 属性:AccessToken
        private string m_AccessToken;

        /// <summary>访问令牌</summary>
        public string AccessToken
        {
            get { return this.m_AccessToken; }
            set { this.m_AccessToken = value; }
        }
        #endregion

        #region 属性:AppKey
        private string m_AppKey;

        /// <summary>应用标识</summary>
        public string AppKey
        {
            get { return this.m_AppKey; }
            set { this.m_AppKey = value; }
        }
        #endregion

        #region 属性:RequestUri
        private string m_RequestUri = string.Empty;

        /// <summary>请求的地址信息</summary>
        public string RequestUri
        {
            get { return this.m_RequestUri; }
            set { this.m_RequestUri = value; }
        }
        #endregion

        #region 属性:RequestData
        private string m_RequestData = string.Empty;

        /// <summary>请求的数据信息</summary>
        public string RequestData
        {
            get { return this.m_RequestData; }
            set { this.m_RequestData = value; }
        }
        #endregion

        #region 属性:ResponseData
        private string m_ResponseData = string.Empty;

        /// <summary>响应的数据信息</summary>
        public string ResponseData
        {
            get { return this.m_ResponseData; }
            set { this.m_ResponseData = value; }
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

        #region 属性:ReturnCode
        private int m_ReturnCode = 0;

        /// <summary></summary>
        public int ReturnCode
        {
            get { return this.m_ReturnCode; }
            set { this.m_ReturnCode = value; }
        }
        #endregion

        #region 属性:CreatedDate
        private DateTime m_CreatedDate;

        /// <summary></summary>
        public DateTime CreatedDate
        {
            get { return m_CreatedDate; }
            set { m_CreatedDate = value; }
        }
        #endregion

        // -------------------------------------------------------
        // 计算时间运行时间
        // -------------------------------------------------------

        /// <summary></summary>
        public void Start()
        {
            this.StartTime = DateTime.Now;
        }

        /// <summary></summary>
        public void Finish()
        {
            this.FinishTime = DateTime.Now;

            // this.TimeSpan = this.timeSpan.Subtract(new TimeSpan(this.FinishTime.Ticks)).Duration().TotalSeconds;
            this.TimeSpan = DateHelper.GetTimeSpan(StartTime, FinishTime).TotalSeconds;
        }

        // -------------------------------------------------------
        // Xml 元素的导入和导出 
        // -------------------------------------------------------

        #region 函数:Deserialize(XmlElement element)
        /// <summary>根据Xml元素加载对象</summary>
        /// <param name="element">Xml元素</param>
        public void Deserialize(XmlElement element)
        {
            StringBuilder outString = new StringBuilder();

            this.Id = element.SelectSingleNode("id").InnerText;
            this.AppKey = element.SelectSingleNode("appKey").InnerText;
            this.RequestUri = element.SelectSingleNode("requestUri").InnerText;
            this.RequestData = StringHelper.FromBase64(element.SelectSingleNode("requestData").InnerText);
            this.StartTime = Convert.ToDateTime(element.SelectSingleNode("startTime").InnerText);
            this.FinishTime = Convert.ToDateTime(element.SelectSingleNode("finishTime").InnerText);
            this.TimeSpan = Convert.ToDouble(element.SelectSingleNode("timeSpan").InnerText);
            this.IP = element.SelectSingleNode("ip").InnerText;
            this.ReturnCode = Convert.ToInt32(element.SelectSingleNode("returnCode").InnerText);
            this.CreatedDate = Convert.ToDateTime(element.SelectSingleNode("createdDate").InnerText);
        }
        #endregion

        #region 函数:Serializable()
        /// <summary>根据对象导出Xml元素</summary>
        /// <returns></returns>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>根据对象导出Xml元素</summary>
        /// <param name="displayComment">显示注释</param>
        /// <param name="displayFriendlyName">显示友好名称</param>
        /// <returns></returns>
        public virtual string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("<connectCall>");
            outString.AppendFormat("<id><![CDATA[{0}]]></id>", this.Id);
            outString.AppendFormat("<appKey><![CDATA[{0}]]></appKey>", this.AppKey);
            outString.AppendFormat("<requestUri><![CDATA[{0}]]></requestUri>", this.RequestUri);
            outString.AppendFormat("<requestData><![CDATA[{0}]]></requestData>", StringHelper.ToBase64(this.RequestData));
            outString.AppendFormat("<startTime><![CDATA[{0}]]></startTime>", this.StartTime);
            outString.AppendFormat("<finishTime><![CDATA[{0}]]></finishTime>", this.FinishTime);
            outString.AppendFormat("<timeSpan><![CDATA[{0}]]></timeSpan>", this.TimeSpan);
            outString.AppendFormat("<ip><![CDATA[{0}]]></ip>", this.IP);
            outString.AppendFormat("<returnCode><![CDATA[{0}]]></returnCode>", this.ReturnCode);
            outString.AppendFormat("<createdDate><![CDATA[{0}]]></createdDate>", this.CreatedDate);
            outString.Append("</connectCall>");

            return outString.ToString();
        }
        #endregion
    }
}
