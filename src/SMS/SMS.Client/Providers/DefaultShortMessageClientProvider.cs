namespace X3Platform.SMS.Client.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    
    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.SMS.Client.Configuration;

    public class DefaultShortMessageClientProvider : IShortMessageClientProvider
    {
        /// <summary>发送短信</summary>
        /// <param name="message">短信</param>
        public string Send(ShortMessage message)
        {
            // url:http://smsapi.ums86.com:8899/sms/Api/Send.do?SpCode={0}&amp;LoginName={1}&amp;Password={2}&amp;MessageContent={5}&amp;UserNumber={4}&amp;SerialNumber={6}&amp;ScheduleTime=&amp;f=1

            string result = null;

            string url = string.Format(SMSConfigurationView.Instance.SendUrl,
                                SMSConfigurationView.Instance.EnterpriseCode,
                                SMSConfigurationView.Instance.Username,
                                SMSConfigurationView.Instance.Password,
                                message.MessageContent,
                                message.PhoneNumber,
                                message.SerialNumber).Trim();


            int markIndex = url.IndexOf("?");

            Uri uri = new Uri(url.Substring(0, markIndex));

            byte[] data = Encoding.GetEncoding("GBK").GetBytes(url.Substring(markIndex + 1, url.Length - markIndex - 1));

            try
            {
                WebRequest request = WebRequest.Create(uri);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            var reader = new StreamReader(stream, Encoding.Default);
                            result = reader.ReadToEnd();
                        }
                    }
                }

                StringDictionary dict = GetResult(result);

                if (dict["result"] != "0")
                {
                    // 返回错误时，记录错误信息
                    KernelContext.Log.Error("request:" + url + "\nresponse:" + result);
                }

                return dict["result"];
            }
            catch (Exception ex)
            {
                KernelContext.Log.Error(ex.ToString());
            }

            return string.Empty;
        }

        /// <summary>将文本信息转换位字典对象信息</summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private StringDictionary GetResult(string result)
        {
            StringDictionary dict = new StringDictionary();

            string[] items = result.Split('&');

            foreach (string item in items)
            {
                string[] subitems = item.Split('=');

                dict.Add(subitems[0], subitems[1]);
            }

            return dict;
        }
    }
}
