namespace X3Platform.SMS.Client.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;
    using Common.Logging;
    using X3Platform.Configuration;
    using X3Platform.SMS.Client.Configuration;
    using X3Platform.Util;

    /// <summary></summary>
    public class AlidayuShortMessageClientProvider : IShortMessageClientProvider
    {
        /// <summary>发送短信</summary>
        /// <param name="message">短信</param>
        public string Send(ShortMessage message)
        {
            //
            //curl -X POST 'http://gw.api.taobao.com/router/rest' \
            //-d 'app_key=12129701' \
            //-d 'format=json' \
            //-d 'method=alibaba.aliqin.fc.sms.num.send' \
            //-d 'partner_id=apidoc' \
            //-d 'sign=1504970E2D634D872A5EED23FF091BC3' \
            //-d 'sign_method=hmac' \
            //-d 'timestamp=2015-12-08+20%3A19%3A29' \
            //-d 'v=2.0' \
            //-d 'extend=123456' \
            //-d 'rec_num=13000000000' \
            //-d 'sms_free_sign_name=%E9%98%BF%E9%87%8C%E5%A4%A7%E9%B1%BC' \
            //-d 'sms_param=%7B%5C%22code%5C%22%3A%5C%221234%5C%22%2C%5C%22product%5C%22%3A%5C%22alidayu%5C%22%7D' \
            //-d 'sms_template_code=SMS_585014' \
            //-d 'sms_type=normal'

            // http://gw.api.taobao.com/router/rest?app_key={0}&amp;sign={1}&amp;sign_method=hmac&amp;timestamp={2}&amp;rec_num={3}&amp;sms_free_sign_name={4}&amp;sms_param={5}&amp;sms_template_code={6}&amp;sms_type=normal&amp;v=2.0&amp;method=alibaba.aliqin.fc.sms.num.send&amp;format=json


            string result = null;

            string url = SMSConfigurationView.Instance.SendUrl;

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("app_key", SMSConfigurationView.Instance.Username);
            parameters.Add("sign_method", "md5");
            parameters.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("rec_num", message.PhoneNumber);
            parameters.Add("sms_free_sign_name", SMSConfigurationView.Instance.SignName);
            parameters.Add("sms_param", message.MessageTemplateParam);
            parameters.Add("sms_template_code", message.MessageTemplateCode);
            parameters.Add("sms_type", "normal");
            parameters.Add("v", "2.0");
            parameters.Add("method", "alibaba.aliqin.fc.sms.num.send");
            parameters.Add("format", "xml");
            // parameters.Add("partner_id", "x3platform");

            string sign = SignTopRequest(parameters, SMSConfigurationView.Instance.Password, true);

            parameters.Add("sign", sign);

            StringBuilder httpParams = new StringBuilder();

            foreach (KeyValuePair<string, string> arg in parameters)
            {
                httpParams.AppendFormat("{0}={1}&", arg.Key, HttpUtility.UrlEncode(arg.Value, Encoding.UTF8));
            }

            httpParams = StringHelper.TrimEnd(httpParams, "&");

            byte[] data = Encoding.UTF8.GetBytes(httpParams.ToString());

            try
            {
                HttpWebRequest request = null;

                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = CertificateManager.GetCallback(true);
                    request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create(url);
                }

                request.ServicePoint.Expect100Continue = false;

                request.Headers["Accept-Encoding"] = "gzip";

                request.Method = "POST";
                request.KeepAlive = true;
                request.UserAgent = "top-sdk-net";
                request.Accept = "text/xml,text/javascript";
                request.Timeout = 20000;
                request.ReadWriteTimeout = 60000;
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            StreamReader reader = null;
                            if (response.ContentEncoding == "gzip")
                            {
                                GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                                reader = new StreamReader(gzipStream, Encoding.UTF8);
                            }
                            else
                            {
                                reader = new StreamReader(stream, Encoding.UTF8);
                            }

                            result = reader.ReadToEnd();
                        }
                    }
                }

                IDictionary<string, string> dict = ToDictionary(result);

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
        private IDictionary<string, string> ToDictionary(string text)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();

            string[] items = text.Split('&');

            foreach (string item in items)
            {
                string[] subitems = item.Split('=');

                dict.Add(subitems[0], subitems[1]);
            }

            return dict;
        }

        /// <summary>
        /// 给TOP请求签名。
        /// </summary>
        /// <param name="parameters">所有字符型的TOP请求参数</param>
        /// <param name="secret">签名密钥</param>
        /// <param name="qhs">是否前后都加密钥进行签名</param>
        /// <returns>签名</returns>
        public static string SignTopRequest(IDictionary<string, string> parameters, string secret, bool qhs)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            if (qhs)
            {
                query.Append(secret);
            }

            // 第三步：使用MD5加密
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }
    }

    public sealed class CertificateManager
    {
        private const string ROOT_CERT_BASE64 = "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";
        private static readonly IDictionary<string, bool> aliDomains = new Dictionary<string, bool>();
        private static X509Certificate2 rootCert;

        static CertificateManager()
        {
            aliDomains.Add("*.taobao.com", true);
            aliDomains.Add("*.alipay.com", true);
            aliDomains.Add("*.aliyuncs.com", true);
            aliDomains.Add("*.alibaba.com", true);
            aliDomains.Add("*.tmall.com", true);

            byte[] rootCertData = Convert.FromBase64String(ROOT_CERT_BASE64);
            rootCert = new X509Certificate2(rootCertData);
        }

        private static bool TrustAllValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; // 忽略SSL证书检查
        }

        private static bool AlibabaValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            foreach (X509ChainElement element in chain.ChainElements)
            {
                if (!element.Certificate.Verify())
                {
                    return false;
                }
            }

            bool hasAliDomain = aliDomains.ContainsKey(GetCertificateCN(certificate));
            if (!hasAliDomain)
            {
                throw new Exception("Access to the non Alibaba Group's HTTPS services are not allowed!");
            }

            X509Chain rootChain = new X509Chain();
            rootChain.ChainPolicy.ExtraStore.Add(rootCert);
            return rootChain.Build((X509Certificate2)certificate);
        }

        private static string GetCertificateCN(X509Certificate cert)
        {
            string subject = cert.Subject;
            string[] entries = subject.Split(',');
            foreach (string entry in entries)
            {
                string[] kv = entry.Trim().Split('=');
                if ("CN".Equals(kv[0]) && kv.Length > 1)
                {
                    return kv[1];
                }
            }
            return subject;
        }

        private static RemoteCertificateValidationCallback allCallback = new RemoteCertificateValidationCallback(TrustAllValidationCallback);
        private static RemoteCertificateValidationCallback aliCallback = new RemoteCertificateValidationCallback(AlibabaValidationCallback);

        public static RemoteCertificateValidationCallback GetCallback(bool ignoreSSLCheck)
        {
            if (ignoreSSLCheck)
            {
                return allCallback;
            }
            else
            {
                return aliCallback;
            }
        }
    }
}
