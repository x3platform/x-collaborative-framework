using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Web;

using Common.Logging;

using X3Platform.Plugins;
using X3Platform.Spring;

using X3Platform.Location.IPQuery.Configuration;

namespace X3Platform.Location.IPQuery
{
    /// <summary>IPQueryContext</summary>
    public class IPQueryContext : CustomPlugin
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 属性:Name
        public override string Name
        {
            get { return "IP查询引擎"; }
        }
        #endregion

        #region 属性:Instance
        private static volatile IPQueryContext instance = new IPQueryContext();

        private static object lockObject = new object();

        public static IPQueryContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new IPQueryContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region 属性:Configuration
        private IPQueryConfiguration configuration = null;

        /// <summary>配置</summary>
        public IPQueryConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        /// <summary>IP分析器提供者</summary>
        private IPAddressParser ipAddressParser;

        private int maxDepthValue = 4; // 最大匹配深度

        #region 构造函数:IPQueryContext()
        /// <summary>构造函数</summary>
        private IPQueryContext()
        {
            this.Restart();
        }
        #endregion

        #region 函数:Restart()
        /// <summary>重启插件</summary>
        /// <returns>返回信息. =0代表重启成功, >0代表重启失败.</returns>
        public override int Restart()
        {
            try
            {
                this.Reload();
            }
            catch
            {
                throw;
            }

            return 0;
        }
        #endregion

        #region 函数:Reload()
        /// <summary>重新加载</summary>
        private void Reload()
        {
            this.configuration = IPQueryConfigurationView.Instance.Configuration;

            this.ipAddressParser = SpringContext.Instance.GetObject<IPAddressParser>(typeof(IPAddressParser));
        }
        #endregion

        #region 函数:ParseAddress()
        /// <summary>分析地址</summary>
        /// <returns>当前IP地址信息</returns>
        public static IPAddressQueryInfo ParseAddress()
        {
            string url = GetClientIP();

            return ParseAddress(url);
        }
        #endregion

        #region 函数:ParseAddress(string url)
        /// <summary>分析地址</summary>
        /// <param name="url">地址</param>
        /// <returns>当前IP地址信息</returns>
        public static IPAddressQueryInfo ParseAddress(string url)
        {
            try
            {
                //
                // 处理多个IP信息的地址.
                // 211.136.28.135,211.136.28.135
                //
                if (url.IndexOf(",") > 0)
                {
                    url = url.Substring(0, url.IndexOf(","));
                }

                //
                // 解析
                //

                IPAddressQueryInfo address = new IPAddressQueryInfo();

                IPAddress ip = Dns.GetHostEntry(url).AddressList[0];

                int depth = instance.maxDepthValue;

                byte[] ipUnit = ip.GetAddressBytes();

                IList<IPAddressQueryInfo> list = instance.ipAddressParser.Parse(ipUnit, out depth);

                address.Domain = url;

                address.Address = ip.ToString();

                foreach (IPAddressQueryInfo item in list)
                {
                    string[] StartIPUnit = item.StartIP.Split('.');

                    string[] EndIPUnit = item.EndIP.Split('.');

                    if (Convert.ToByte(ipUnit[depth]) >= Convert.ToByte(StartIPUnit[depth].ToString()) && Convert.ToByte(ipUnit[depth]) <= Convert.ToByte(EndIPUnit[depth].ToString()))
                    {
                        address.Country = (string.IsNullOrEmpty(item.Country)) ? string.Empty : item.Country;

                        address.Province = (string.IsNullOrEmpty(item.Province)) ? string.Empty : item.Province;

                        address.City = (string.IsNullOrEmpty(item.City)) ? string.Empty : item.City;

                        address.Language = (string.IsNullOrEmpty(item.Language)) ? string.Empty : item.Language;

                        address.Description = (string.IsNullOrEmpty(item.Language)) ? string.Empty : item.Description;

                        address.StartIP = (string.IsNullOrEmpty(item.StartIP)) ? string.Empty : item.StartIP;

                        address.EndIP = (string.IsNullOrEmpty(item.EndIP)) ? string.Empty : item.EndIP;

                        break;
                    }
                }

                return address;
            }
            catch (SocketException socketException)
            {
                logger.Error(string.Format("Failed to get host by name ~ {0}.", url), socketException);

                return null;
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Incorrect address ~ {0}", url), ex);

                return null;
            }
        }
        #endregion

        #region 函数:GetClientIP()
        /// <summary>取得客户端真实IP地址</summary>
        public static string GetClientIP()
        {
            //-------------------------------------------------------
            // 01. 
            // 如果客户端没用代理，则Request.ServerVariables("HTTP_X_FORWARDED_FOR")得到是空值，
            // 应该用Request.ServerVariables("REMOTE_ADDR")方法
            // 
            // 02
            // 如果客户端用了代理服务器，则用Request.ServerVariables("REMOTE_ADDR")方法只能得到空值,
            // 则应该用ServerVariables("HTTP_X_FORWARDED_FOR")方法
            // 
            //-------------------------------------------------------

            string ip;

            HttpContext context = HttpContext.Current;

            if (context == null) { return "127.0.0.1"; }

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                ip = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            return ip;
        }
        #endregion
    }
}
