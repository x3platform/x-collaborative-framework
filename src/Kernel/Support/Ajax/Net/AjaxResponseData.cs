using System;
using System.Net;

using X3Platform.Messages;

namespace X3Platform.Ajax.Net
{
    /// <summary>Ajax响应的数据</summary>
    public class AjaxResponseData
    {
        #region 属性:ResponseText
        private string m_ResponseText;

        /// <summary>
        /// 响应的文本信息
        /// </summary>
        public string ResponseText
        {
            get { return m_ResponseText; }
            set { m_ResponseText = value; }
        }
        #endregion

        #region 属性:Result
        private MessageObject m_Result;

        /// <summary>
        /// 结果信息
        /// </summary>
        public MessageObject Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }
        #endregion

        #region 属性:ResponseException
        private WebException m_ResponseException;

        /// <summary>
        /// 响应的异常信息
        /// </summary>
        public WebException ResponseException
        {
            get { return m_ResponseException; }
            set { m_ResponseException = value; }
        }
        #endregion
    }
}
