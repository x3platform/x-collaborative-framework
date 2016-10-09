using System;
using Topshelf;
using X3Platform.Services.Configuration;
using System.Runtime.Remoting.Channels;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;

namespace X3Platform.Services.Host
{
    static class Program
    {
        static void Main(string[] args)
        {
            // -------------------------------------------------------
            // 创建服务宿主对象
            // -------------------------------------------------------

            HostFactory.Run(configure =>
            {
                configure.Service<ServicesManagement>(callback =>
                {
                    callback.ConstructUsing(instance => new ServicesManagement());
                    callback.WhenStarted(instance => instance.Start());
                    callback.WhenStopped(instance => instance.Stop());
                });

                // Support Linux
                configure.UseLinuxIfAvailable();

                configure.RunAsLocalSystem();

                configure.SetServiceName(ServicesConfigurationView.Instance.ServiceName);
                configure.SetDisplayName(ServicesConfigurationView.Instance.ServiceDisplayName);
                configure.SetDescription(ServicesConfigurationView.Instance.ServiceDescription);
            });
        }
    }
}