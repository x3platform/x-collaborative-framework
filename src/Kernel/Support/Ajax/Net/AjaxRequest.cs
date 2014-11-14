namespace X3Platform.Ajax.Net
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    #endregion

    /// <summary>Ajax请求</summary>
    public class AjaxRequest
    {
        /// <summary>异步回调函数</summary>
        /// <param name="response"></param>
        public delegate void InvokeAsyncCallbackDelegate(string response);

        /// <summary></summary>
        private AjaxRequestData requestData = null;

        /// <summary></summary>
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
        /// <param name="uri">请求的数据</param>
        /// <param name="httpMethod">请求的方式( GET | POST )</param>
        /// <returns></returns>
        private AjaxResponseData Request(Uri uri, string httpMethod)
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

            request.ContentType = "application/x-www-form-urlencoded";

            if (request.Method == "POST")
            {
                StringBuilder httpParams = new StringBuilder();

                foreach (KeyValuePair<string, string> arg in requestData.Args)
                {
                    httpParams.AppendFormat("{0}={1}&", arg.Key, HttpUtility.UrlEncode(arg.Value));
                }

                byte[] buffer = Encoding.UTF8.GetBytes(httpParams.ToString());

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
                    // Get the stream associated with the response.
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Pipes the stream to a higher level stream reader with the required encoding format. 

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
                // LoggingContext.Instance.Write("Response Data:" + this.responseData.ResponseText);

                throw new AjaxRequestException(ex.Message, requestData, ex);
            }

            return this.responseData;
        }
        #endregion

        // -------------------------------------------------------
        // 异步
        // -------------------------------------------------------

        #region 函数:RequestAsync(Uri uri, string httpMethod)
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

        #region 函数:RequestReady(IAsyncResult asyncResult)
        private void ResponseReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;

            using (WebResponse response = request.EndGetResponse(asyncResult))
            {
                using (Stream stream = response.GetResponseStream())
                {
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
        public static string Request(AjaxRequestData requestData)
        {
            AjaxRequest request = new AjaxRequest(requestData);

            AjaxResponseData response = request.Request(requestData.ActionUri, "POST");

            return response.ResponseText;
        }
        #endregion

        #region 静态函数:Request(AjaxRequestData requestData, string httpMethod)
        /// <summary>发送同步请求</summary>
        public static string Request(AjaxRequestData requestData, string httpMethod)
        {
            AjaxRequest request = new AjaxRequest(requestData);

            AjaxResponseData response = request.Request(requestData.ActionUri, httpMethod);

            return response.ResponseText;
        }
        #endregion

        #region 静态函数:RequestAsync(AjaxRequestData requestData, InvokeAsyncCallbackDelegate callback)
        /// <summary>发送异步请求</summary>
        /// <param name="requestData"></param>
        /// <param name="callback"></param>
        public static void RequestAsync(AjaxRequestData requestData, InvokeAsyncCallbackDelegate callback)
        {
            AjaxRequest request = new AjaxRequest(requestData, callback);

            request.RequestAsync(requestData.ActionUri, "POST");
        }
        #endregion
    }
}
