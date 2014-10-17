namespace X3Platform.Web.APIs.Configuration
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Reflection;
    using Common.Logging;

    using X3Platform.Configuration;
    using X3Platform.Util;

    /// <summary>��ַ��д������ͼ</summary>
    public sealed class APIsConfigurationView : XmlConfigurationView<APIsConfiguration>
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>�����ļ���Ĭ��·��</summary>
        private const string configFile = "config\\X3Platform.Web.APIs.config";

        #region 静态属性:Instance
        private static volatile APIsConfigurationView instance = null;

        private static object lockObject = new object();

        public static APIsConfigurationView Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new APIsConfigurationView();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        #region ���캯��:APIsConfigurationView()
        /// <summary>���캯��</summary>
        private APIsConfigurationView()
            : base(Path.Combine(KernelConfigurationView.Instance.ApplicationPathRoot, configFile))
        {
        }
        #endregion
    }
}
