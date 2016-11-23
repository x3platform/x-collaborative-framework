using System;
using System.Collections;

using Topshelf;

using X3Platform.Services.Configuration;

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