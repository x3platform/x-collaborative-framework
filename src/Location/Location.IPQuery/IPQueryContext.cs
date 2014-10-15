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
        /// <summary>��־��¼��</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ����:Name
        public override string Name
        {
            get { return "IP��ѯ����"; }
        }
        #endregion

        #region ����:Instance
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

        #region ����:Configuration
        private IPQueryConfiguration configuration = null;

        /// <summary>����</summary>
        public IPQueryConfiguration Configuration
        {
            get { return configuration; }
        }
        #endregion

        /// <summary>IP�������ṩ��</summary>
        private IPAddressParser ipAddressParser;

        private int maxDepthValue = 4; // ���ƥ�����

        #region ���캯��:IPQueryContext()
        /// <summary>���캯��</summary>
        private IPQueryContext()
        {
            this.Restart();
        }
        #endregion

        #region ����:Restart()
        /// <summary>�������</summary>
        /// <returns>������Ϣ. =0���������ɹ�, >0��������ʧ��.</returns>
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

        #region ����:Reload()
        /// <summary>���¼���</summary>
        private void Reload()
        {
            this.configuration = IPQueryConfigurationView.Instance.Configuration;

            this.ipAddressParser = SpringContext.Instance.GetObject<IPAddressParser>(typeof(IPAddressParser));
        }
        #endregion

        #region ����:ParseAddress()
        /// <summary>������ַ</summary>
        /// <returns>��ǰIP��ַ��Ϣ</returns>
        public static IPAddressQueryInfo ParseAddress()
        {
            string url = GetClientIP();

            return ParseAddress(url);
        }
        #endregion

        #region ����:ParseAddress(string url)
        /// <summary>������ַ</summary>
        /// <param name="url">��ַ</param>
        /// <returns>��ǰIP��ַ��Ϣ</returns>
        public static IPAddressQueryInfo ParseAddress(string url)
        {
            try
            {
                //
                // ������IP��Ϣ�ĵ�ַ.
                // 211.136.28.135,211.136.28.135
                //
                if (url.IndexOf(",") > 0)
                {
                    url = url.Substring(0, url.IndexOf(","));
                }

                //
                // ����
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

        #region ����:GetClientIP()
        /// <summary>ȡ�ÿͻ�����ʵIP��ַ</summary>
        public static string GetClientIP()
        {
            //-------------------------------------------------------
            // 01. 
            // ����ͻ���û�ô�����Request.ServerVariables("HTTP_X_FORWARDED_FOR")�õ��ǿ�ֵ��
            // Ӧ����Request.ServerVariables("REMOTE_ADDR")����
            // 
            // 02
            // ����ͻ������˴��������������Request.ServerVariables("REMOTE_ADDR")����ֻ�ܵõ���ֵ,
            // ��Ӧ����ServerVariables("HTTP_X_FORWARDED_FOR")����
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
