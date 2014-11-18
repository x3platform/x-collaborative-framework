namespace X3Platform.Ajax.Net
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using X3Platform.Util;
    #endregion

    /// <summary>Ajax请求</summary>
    public class AjaxRequest
    {
        /// <summary>异步回调函数</summary>
        /// <param name="response"></param>
        public delegate void InvokeAsyncCallbackDelegate(string response);

        /// <summary>请求的数据信息</summary>
        private AjaxRequestData requestData = null;

        /// <summary>响应的数据信息</summary>
        private AjaxResponseData responseData = null;

        private InvokeAsyncCallbackDelegate callback;

        #region 构造函数:AjaxRequest(AjaxRequestData requestData)
        private AjaxRequest(AjaxRequestData requestData)
        {
            this.requestData = requestData;
        }
        #endregion

        #region 构造函数:AjaxRequest(AjaxRequestData requestData, InvokeAsyncCallbackDelegate callback)
        private AjaxRequest(AjaxRequestData requestData, InvokeAsyncCallbackDelegate callback)
        {
            this.requestData = requestData;

            this.callback = callback;
        }
        #endregion

        // -------------------------------------------------------
        // 同步
        // -------------------------------------------------------

        #region 函数:Request(AjaxRequestData requestData, string httpMethod)
        /// <summary>发送请求</summary>
        /// <param name="uri">请求的地址</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <returns>响应的数据信息</returns>
        private AjaxResponseData Request(Uri uri, string httpMethod)
        {
            StringBuilder httpParams = new StringBuilder();

            foreach (KeyValuePair<string, string> arg in requestData.Args)
            {
                httpParams.AppendFormat("{0}={1}&", arg.Key, HttpUtility.UrlEncode(arg.Value));
            }

            httpParams = StringHelper.TrimEnd(httpParams, "&");

            return Request(uri, httpMethod, "application/x-www-form-urlencoded", httpParams.ToString());
        }
        #endregion

        #region 函数:Request(AjaxRequestData requestData, string httpMethod, string contentType, string content)
        /// <summary>发送请求</summary>
        /// <param name="uri">请求的地址</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="content">内容</param>
        /// <returns>响应的数据信息</returns>
        private AjaxResponseData Request(Uri uri, string httpMethod, string contentType, string content)
        {
            // -------------------------------------------------------
            // 1.发送请求
            // -------------------------------------------------------

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            if (!string.IsNullOrEmpty(requestData.LoginName))
            {
                // 设置身份验证信息
                request.Credentials = new NetworkCredential(requestData.LoginName, requestData.Password);
            }

            request.Method = httpMethod;

            request.ContentType = contentType;

            if (request.Method == "POST")
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);

                request.ContentLength = buffer.Length;

                if (buffer.Length > 0)
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            else
            {
                // Some limitations
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                request.ContentLength = 0;
            }

            // -------------------------------------------------------
            // 2.返回响应信息
            // -------------------------------------------------------

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // 获取相关的响应流
                    using (Stream stream = response.GetResponseStream())
                    {
                        // 设置流格式的编码格式

                        Encoding encoding = Encoding.UTF8;

                        if (!string.IsNullOrEmpty(response.CharacterSet))
                        {
                            encoding = Encoding.GetEncoding(response.CharacterSet);
                        }

                        using (StreamReader streamReader = new StreamReader(stream, encoding))
                        {
                            this.responseData = ParseResponseData(streamReader.ReadToEnd());

                            streamReader.Close();
                        }

                        stream.Close();
                    }

                    response.Close();
                }
            }
            catch (Exception ex)
            {
                throw new AjaxRequestException(ex.Message, requestData, ex);
            }

            return this.responseData;
        }
        #endregion

        // -------------------------------------------------------
        // 异步
        // -------------------------------------------------------

        #region 函数:RequestAsync(Uri uri, string httpMethod)
        /// <summary>发送异步请求</summary>
        /// <param name="uri">请求的地址</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <returns>响应的数据信息</returns>
        private void RequestAsync(Uri uri, string httpMethod)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            if (!string.IsNullOrEmpty(requestData.LoginName))
            {
                // 设置身份验证信息
                request.Credentials = new NetworkCredential(requestData.LoginName, requestData.Password);
            }

            request.Method = httpMethod;

            request.ContentType = "application/x-www-form-urlencoded";

            request.BeginGetRequestStream(new AsyncCallback(RequestReady), request);
        }
        #endregion

        #region 函数:RequestAsync(Uri uri, string httpMethod, string contentType, string content)
        /// <summary>发送异步请求</summary>
        /// <param name="uri">请求的地址</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="content">内容</param>
        /// <returns>响应的数据信息</returns>
        private void RequestAsync(Uri uri, string httpMethod, string contentType, string content)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            if (!string.IsNullOrEmpty(requestData.LoginName))
            {
                // 设置身份验证信息
                request.Credentials = new NetworkCredential(requestData.LoginName, requestData.Password);
            }

            request.Method = httpMethod;

            request.ContentType = contentType;

            if (request.Method == "POST")
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);

                request.ContentLength = buffer.Length;

                if (buffer.Length > 0)
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            else
            {
                // Some limitations
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                request.ContentLength = 0;
            }

            request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
        }
        #endregion

        #region 函数:RequestReady(IAsyncResult asyncResult)
        private void RequestReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;

            using (StreamWriter streamWriter = new StreamWriter(request.EndGetRequestStream(asyncResult)))
            {
                StringBuilder httpParams = new StringBuilder();

                foreach (KeyValuePair<string, string> arg in requestData.Args)
                {
                    httpParams.AppendFormat("{0}={1}&", arg.Key, HttpUtility.UrlEncode(arg.Value));
                }

                streamWriter.Write(httpParams.ToString().TrimEnd(new char[] { '&' }));
            }

            request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
        }
        #endregion

        #region 函数:ResponseReady(IAsyncResult asyncResult)
        private void ResponseReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;

            using (HttpWebResponse response =  (HttpWebResponse)request.EndGetResponse(asyncResult))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // 设置流格式的编码格式

                    Encoding encoding = Encoding.UTF8;

                    if (!string.IsNullOrEmpty(response.CharacterSet))
                    {
                        encoding = Encoding.GetEncoding(response.CharacterSet);
                    }

                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        this.responseData = ParseResponseData(streamReader.ReadToEnd());

                        if (this.callback != null)
                        {
                            this.callback(this.responseData.ResponseText);
                        }
                    }
                }
            }
        }
        #endregion

        // -------------------------------------------------------
        // 解析返回结果
        // -------------------------------------------------------

        #region 函数:ParseResponseData(AjaxResponseData responseData)
        /// <summary>解析相应的数据</summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        private AjaxResponseData ParseResponseData(string responseText)
        {
            if (this.responseData == null)
            {
                this.responseData = new AjaxResponseData();
            }

            responseData.ResponseText = responseText;

            if (string.IsNullOrEmpty(responseText))
            {
                this.responseData.Result.Set("1", "没有返回任何信息。");
                return this.responseData;
            }

            return this.responseData;
        }
        #endregion

        // -------------------------------------------------------
        // 静态方法
        // -------------------------------------------------------

        #region 静态函数:Request(AjaxRequestData requestData)
        /// <summary>发送同步请求</summary>
        /// <param name="requestData">请求的数据信息</param>
        public static string Request(AjaxRequestData requestData)
        {
            AjaxRequest request = new AjaxRequest(requestData);

            AjaxResponseData response = request.Request(requestData.ActionUri, "POST");

            return response.ResponseText;
        }
        #endregion

        #region 静态函数:Request(AjaxRequestData requestData, string httpMethod)
        /// <summary>发送同步请求</summary>
        /// <param name="requestData">请求的数据信息</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        public static string Request(AjaxRequestData requestData, string httpMethod)
        {
            AjaxRequest request = new AjaxRequest(requestData);

            AjaxResponseData response = request.Request(requestData.ActionUri, httpMethod);

            return response.ResponseText;
        }
        #endregion

        #region 静态函数:Request(AjaxRequestData requestData, string httpMethod, string contentType, string content)
        /// <summary>发送同步请求</summary>
        /// <param name="requestData">请求的数据</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="content">内容</param>
        public static string Request(AjaxRequestData requestData, string httpMethod, string contentType, string content)
        {
            AjaxRequest request = new AjaxRequest(requestData);

            AjaxResponseData response = request.Request(requestData.ActionUri, httpMethod, contentType, content);

            return response.ResponseText;
        }
        #endregion

        #region 静态函数:RequestAsync(AjaxRequestData requestData, InvokeAsyncCallbackDelegate callback)
        /// <summary>发送异步请求</summary>
        /// <param name="requestData">请求的数据</param>
        /// <param name="callback">回调函数</param>
        public static void RequestAsync(AjaxRequestData requestData, InvokeAsyncCallbackDelegate callback)
        {
            AjaxRequest request = new AjaxRequest(requestData, callback);

            request.RequestAsync(requestData.ActionUri, "POST");
        }
        #endregion

        #region 静态函数:RequestAsync(AjaxRequestData requestData, string httpMethod, InvokeAsyncCallbackDelegate callback)
        /// <summary>发送异步请求</summary>
        /// <param name="requestData">请求的数据</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <param name="callback">回调函数</param>
        public static void RequestAsync(AjaxRequestData requestData, string httpMethod, InvokeAsyncCallbackDelegate callback)
        {
            AjaxRequest request = new AjaxRequest(requestData, callback);

            request.RequestAsync(requestData.ActionUri, httpMethod);
        }
        #endregion

        #region 静态函数:RequestAsync(AjaxRequestData requestData, string httpMethod , string contentType, string content, InvokeAsyncCallbackDelegate callback)
        /// <summary>发送异步请求</summary>
        /// <param name="requestData">请求的数据</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="content">内容</param>
        /// <param name="callback">回调函数</param>
        public static void RequestAsync(AjaxRequestData requestData, string httpMethod, string contentType, string content, InvokeAsyncCallbackDelegate callback)
        {
            AjaxRequest request = new AjaxRequest(requestData, callback);

            request.RequestAsync(requestData.ActionUri, httpMethod, contentType, content);
        }
        #endregion
    }
}
