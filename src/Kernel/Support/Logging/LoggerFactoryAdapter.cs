using System;
using System.Configuration;
using System.IO;

using Common.Logging;
using Common.Logging.Configuration;

using X3Platform.Logging.Config;
using X3Platform.Configuration;

namespace X3Platform.Logging
{
    /// <summary>
    /// Concrete subclass of ILoggerFactoryAdapter specific to log4net.
    /// </summary>
    public class LoggerFactoryAdapter : ILoggerFactoryAdapter
    { 
        /// <summary></summary>
        /// <param name="properties"></param>
        public LoggerFactoryAdapter(NameValueCollection properties)
        {
            string configType = string.Empty;

            if (properties["configType"] != null)
            {
                configType = properties["configType"].ToUpper();
            }

            string configFile = string.Empty;

            if (properties["configFile"] != null)
            {
                configFile = properties["configFile"];

                if (!string.IsNullOrEmpty(configFile))
                {
                    configFile = KernelConfigurationView.Instance.ReplaceKeyValue(configFile);
                }
            }

            if (configType == "FILE" || configType == "FILE-WATCH")
            {
                if (configFile == string.Empty)
                {
                    throw new ConfigurationErrorsException("Configration property 'configFile' must be set for logging configuration of type 'FILE'.");
                }

                if (!File.Exists(configFile))
                {
                    throw new ConfigurationErrorsException("logging configuration file '" + configFile + "' does not exists");
                }
            }

            switch (configType)
            {
                case "INLINE":
                    XmlConfigurator.Configure();
                    break;
                case "FILE":
                    XmlConfigurator.Configure(new FileInfo(configFile));
                    break;
                case "FILE-WATCH":
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
                    break;
                case "EXTERNAL":
                    // Log4net will be configured outside of IBatisNet
                    break;
                default:
                    BasicConfigurator.Configure();
                    break;
            }
        }

        /// <summary>
        /// Get a ILog instance by type name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Common.Logging.ILog GetLogger(string name)
        {
            X3Platform.Logging.IInternalLog log = X3Platform.Logging.LogManager.GetLogger(name);

            return new Logger(log);
        }

        /// <summary>
        /// Get a ILog instance by type 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Common.Logging.ILog GetLogger(Type type)
        {
            X3Platform.Logging.IInternalLog log = X3Platform.Logging.LogManager.GetLogger(type);
          
            return new Logger(log);
        }
    }
}
