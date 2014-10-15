using System;
using System.Collections.Generic;
using System.ServiceModel;

using X3Platform.Services.Configuration;
using X3Platform.Configuration;

namespace X3Platform.Services
{
    public static class ServiceHostManagement
    {
        /// <summary>服务列表</summary>
        private static IList<ServiceHost> hosts = new List<ServiceHost>();

        /// <summary>启动</summary>
        public static void Start()
        {
            try
            { 
                ServicesConfiguration configuration = ServicesConfigurationView.Instance.Configuration;
             
                foreach (NameTypeConfigurationElement service in configuration.Services)
                {
                    ServiceTrace.Instance.WriteLine("正在创建服务【" + service.Name + "】。");

                    if (service.Type == null)
                    {
                        EventLogHelper.Write(string.Format("服务【{0}】初始化类型【{1}】失败，请确认配置是否正确。", service.Name, service.TypeName));
                    }
                    else
                    {
                        OpenHost(service.Type);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogHelper.Error(ex.ToString());

                throw ex;
            }
        }

        /// <summary>启用服务的宿主</summary>
        /// <param name="type"></param>
        private static void OpenHost(Type type)
        {
            ServiceHost host = new ServiceHost(type);

            host.Open();

            hosts.Add(host);
        }

        /// <summary>关闭</summary>
        public static void Close()
        {
            foreach (ServiceHost host in hosts)
            {
                host.Close();
            }
        }
    }
}