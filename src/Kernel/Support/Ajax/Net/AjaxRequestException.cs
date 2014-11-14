using System;

namespace X3Platform.Ajax.Net
{
    /// <summary>Ajax请求异常</summary>
    public class AjaxRequestException : Exception
    {
        #region 属性:RequestData
        private AjaxRequestData requestData;

        /// <summary>请求的数据</summary>
        public AjaxRequestData RequestData
        {
            get { return requestData; }
            set { requestData = value; }
        }
        #endregion

        #region 属性:ResponseData
        private AjaxResponseData responseData;

        /// <summary>响应的数据</summary>
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
