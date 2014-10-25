#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :MessageObject.cs
//
// Description  :��Ϣ����
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Messages
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.CacheBuffer;
    using X3Platform.Util;
    #endregion

    /// <summary>默认消息对象</summary>
    public sealed class MessageObject : IMessageObject, ICacheable, IDisposable
    {
        #region 属性:Value
        private object m_Value = null;

        /// <summary>返回的值</summary>
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        #endregion

        #region 属性:ReturnCode
        private string m_ReturnCode = null;

        /// <summary>返回的代码</summary>
        public string ReturnCode
        {
            get { return m_ReturnCode; }
            set { m_ReturnCode = value; }
        }
        #endregion

        #region 属性:Result
        private object m_Result = null;

        /// <summary>结果</summary>
        public object Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }
        #endregion

        #region 属性:Description
        private string m_Description = string.Empty;

        /// <summary>描述</summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region 属性:Url
        private string m_Url = string.Empty;

        /// <summary>Url</summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        #endregion

        #region 函数:Clear()
        /// <summary>清空信息</summary>
        public void Clear()
        {
            this.Value = null;
        }
        #endregion

        #region 函数:Set(string returnCode, object value, object result, string description, string url)
        /// <summary>设置信息</summary>
        public void Set(string returnCode, object value)
        {
            this.Set(returnCode, value, string.Empty);
        }

        /// <summary>设置信息</summary>
        public void Set(string returnCode, object value, object result)
        {
            this.Set(returnCode, value, result, string.Empty);

        }

        /// <summary>设置信息</summary>
        public void Set(string returnCode, object value, object result, string description)
        {
            this.Set(returnCode, value, result, description, string.Empty);
        }

        /// <summary>设置信息</summary>
        /// <param name="returnCode"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <param name="description"></param>
        /// <param name="url"></param>
        public void Set(string returnCode, object value, object result, string description, string url)
        {
            this.ReturnCode = returnCode;
            this.Value = value;
            this.Result = result;
            this.Description = description;
            this.Url = url;
        }
        #endregion

        #region 函数:ToString()
        /// <summary>转换为JSON格式的字符串对象.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("{");
            outString.Append("\"returnCode\":\"" + this.ReturnCode + "\",");
            outString.Append("\"result\":\"" + this.Result + "\",");
            outString.Append("\"description\":\"" + this.Description + "\",");
            outString.Append("\"value\":\"" + this.Value + "\",");
            outString.Append("\"url\":\"" + this.Url + "\" ");
            outString.Append("}");

            return outString.ToString();
        }
        #endregion

        //
        // 实现 EntityClass 序列化
        // 

        #region 函数:Serializable()
        /// <summary>序列化对象</summary>
        public string Serializable()
        {
            return Serializable(false, false);
        }
        #endregion

        #region 函数:Serializable(bool displayComment, bool displayFriendlyName)
        /// <summary>序列化对象</summary>
        /// <param name="displayComment">显示注释信息</param>
        /// <param name="displayFriendlyName">显示友好名称信息</param>
        /// <returns></returns>
        public string Serializable(bool displayComment, bool displayFriendlyName)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("<response>");
            outString.Append("<value>" + this.Value + "</value>");
            outString.Append("<returnCode>" + this.ReturnCode + "</returnCode>");
            outString.Append("<description>" + this.Description + "</description>");
            outString.Append("<url>" + this.Url + "</url>");
            outString.Append("</response>");

            return outString.ToString();
        }
        #endregion

        #region 函数:Deserialize(XmlElement element)
        /// <summary>反序列化对象</summary>
        /// <param name="element">Xml 元素</param>
        public void Deserialize(XmlElement element)
        {
            this.Value = XmlHelper.Fetch("value", element);
            this.ReturnCode = XmlHelper.Fetch("returnCode", element);
            this.Description = XmlHelper.Fetch("description", element);
            this.Url = XmlHelper.Fetch("url", element);
        }
        #endregion

        //
        // 显式实现 ICacheable
        // 

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(1);

        /// <summary>缓存过期时间</summary>
        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion

        //
        // 显式实现 ICacheable
        // 

        #region 函数:Dispose()
        void IDisposable.Dispose()
        {
        }
        #endregion
    }
}
