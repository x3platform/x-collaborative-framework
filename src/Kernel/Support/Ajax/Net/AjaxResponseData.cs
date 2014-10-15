// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxResponseData.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Net;

using X3Platform.Messages;

namespace X3Platform.Ajax.Net
{
    /// <summary>Ajax��Ӧ������</summary>
    public class AjaxResponseData
    {
        #region 属性:ResponseText
        private string m_ResponseText;

        /// <summary>
        /// ��Ӧ���ı���Ϣ
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
        /// ������Ϣ
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
        /// ��Ӧ���쳣��Ϣ
        /// </summary>
        public WebException ResponseException
        {
            get { return m_ResponseException; }
            set { m_ResponseException = value; }
        }
        #endregion
    }
}
