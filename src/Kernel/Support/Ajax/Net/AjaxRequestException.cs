// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AjaxRequestException.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;

namespace X3Platform.Ajax.Net
{
    /// <summary>Ajax�����쳣</summary>
    public class AjaxRequestException : Exception
    {
        #region 属性:RequestData
        private AjaxRequestData requestData;

        /// <summary>����������</summary>
        public AjaxRequestData RequestData
        {
            get { return requestData; }
            set { requestData = value; }
        }
        #endregion

        #region 属性:ResponseData
        private AjaxResponseData responseData;

        /// <summary>��Ӧ������</summary>
        public AjaxResponseData ResponseData
        {
            get { return responseData; }
            set { responseData = value; }
        }
        #endregion

        public AjaxRequestException(string message, AjaxRequestData requestData)
            : base(message)
        {
            this.requestData = requestData;
        }

        public AjaxRequestException(string message, AjaxRequestData requestData, Exception innerException)
            : base(message, innerException)
        {
            this.requestData = requestData;
        }

        public AjaxRequestException(string message, AjaxResponseData responseData)
            : base(message)
        {
            this.responseData = responseData;
        }

        public AjaxRequestException(string message, AjaxResponseData responseData, Exception innerException)
            : base(message, innerException)
        {
            this.responseData = responseData;
        }
    }
}
