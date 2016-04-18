namespace X3Platform.Messages
{
    #region Using Libraries
    using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using X3Platform.Ajax;
using X3Platform.Ajax.Configuration;
using X3Platform.CacheBuffer;
using X3Platform.Configuration;
using X3Platform.Util;
    #endregion

    /// <summary>消息对象</summary>
    public sealed class MessageObject : IMessageObject
    {
        #region 属性:ReturnCode
        /// <summary>返回的代码</summary>
        public string ReturnCode { get; set; }
        #endregion

        #region 属性:Value
        /// <summary>返回的值</summary>
        public string Value { get; set; }
        #endregion

        #region 函数:Set(string returnCode, string value)
        /// <summary>设置信息</summary>
        /// <param name="returnCode"></param>
        /// <param name="value"></param>
        public void Set(string returnCode, string value)
        {
            this.ReturnCode = returnCode;
            this.Value = value;
        }
        #endregion

        #region 函数:ToString()
        /// <summary>转换为JSON格式的字符串对象.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }
        #endregion

        #region 函数:ToString(bool nobrace)
        /// <summary>转换为字符串</summary>
        /// <param name="nobrace">对象不包含最外面的大括号</param>
        /// <returns></returns>
        public string ToString(bool nobrace)
        {
            StringBuilder outString = new StringBuilder();

            if (!nobrace)
            {
                outString.Append("{");
            }

            outString.Append("\"message\":{");
            if (AjaxConfigurationView.Instance.NamingRule == "underline")
            {
                outString.AppendFormat("\"return_code\":\"{0}\",", StringHelper.ToSafeJson(this.ReturnCode));
            }
            else
            {
                outString.AppendFormat("\"returnCode\":\"{0}\",", StringHelper.ToSafeJson(this.ReturnCode));
            }
            outString.AppendFormat("\"value\":\"{0}\"", StringHelper.ToSafeJson(this.Value));
            outString.Append("},");

            // 是否成功执行
            outString.Append("\"success\":1,\"msg\":\"success\"");

            if (!nobrace)
            {
                outString.Append("}");
            }

            IMessageObjectFormatter formatter = (IMessageObjectFormatter)Activator.CreateInstance(Type.GetType(KernelConfigurationView.Instance.MessageObjectFormatter));

            return !nobrace ? formatter.Format(outString.ToString(), nobrace) : formatter.Format(string.Concat("{", outString.ToString(), "}"), nobrace);
        }
        #endregion

        // -------------------------------------------------------
        // 实现 EntityClass 序列化
        // -------------------------------------------------------

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

            outString.Append("<returnCode>" + this.ReturnCode + "</returnCode>");
            outString.Append("<value>" + this.Value + "</value>");
            //outString.Append("<description>" + this.Description + "</description>");
            //outString.Append("<url>" + this.Url + "</url>");
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
            // this.Description = XmlHelper.Fetch("description", element);
            // this.Url = XmlHelper.Fetch("url", element);
        }
        #endregion

        // -------------------------------------------------------
        // 静态方法
        // -------------------------------------------------------

        /// <summary>格式化为 JOSN 格式字符串</summary>
        /// <param name="returnCode">返回的代码</param>
        /// <param name="value">消息</param>
        /// <returns></returns>
        public static string Stringify(string returnCode, string value)
        {
            MessageObject message = new MessageObject() { ReturnCode = returnCode, Value = value };

            return message.ToString();
        }

        /// <summary>格式化为 JOSN 格式字符串</summary>
        /// <param name="returnCode">返回的代码</param>
        /// <param name="value">消息</param>
        /// <param name="nobrace">对象不包含最外面的大括号</param>
        /// <returns></returns>
        public static string Stringify(string returnCode, string value, bool nobrace)
        {
            MessageObject message = new MessageObject() { ReturnCode = returnCode, Value = value };

            return message.ToString(nobrace);
        }
    }
}
