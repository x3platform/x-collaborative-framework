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
            //
            // 创建服务跟踪对象
            //

            BinaryClientFormatterSinkProvider clientSinkProvider = new BinaryClientFormatterSinkProvider();

            BinaryServerFormatterSinkProvider serverSinkProvider = new BinaryServerFormatterSinkProvider();

            serverSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary properties = new Hashtable();

            properties["port"] = ServicesConfigurationView.Instance.TcpPort;

            TcpChannel channel = new TcpChannel(properties, clientSinkProvider, serverSinkProvider);

            ChannelServices.RegisterChannel(channel, false);

            ObjRef serviceTraceRefObject = RemotingServices.Marshal(ServiceTrace.Instance, "X3Platform.Services.ServiceTrace");

            //
            // 创建服务宿主对象
            //

            Topshelf.Host host = HostFactory.New(configure =>
            {
                configure.AfterStoppingServices(callback =>
                {
                    ServiceTrace.Instance.WriteLine("正在停止服务。");
                });

                configure.BeforeStartingServices(callback =>
                {
                    ServiceTrace.Instance.WriteLine("正在启动服务。");
                });

                configure.EnableDashboard();

                configure.Service<ServicesManagement>(callback =>
                {
                    callback.SetServiceName(ServicesConfigurationView.Instance.ServiceName);
                    callback.ConstructUsing(instance => new ServicesManagement());
                    callback.WhenStarted(instance => instance.Start());
                    callback.WhenStopped(instance => instance.Stop());
                });

                configure.RunAsLocalSystem();

                configure.SetServiceName(ServicesConfigurationView.Instance.ServiceName);
                configure.SetDisplayName(ServicesConfigurationView.Instance.ServiceDisplayName);
                configure.SetDescription(ServicesConfigurationView.Instance.ServiceDescription);

                // 设置服务的依赖项
                // configure.DependsOn("Elane X Services");
                // configure.DependsOnMsmq();
                // configure.DependsOnMsSql();
            });
            
            host.Run();
        }

        /*
        static class Program
        {
            /// <summary>
            /// 应用程序的主入口点。
            /// </summary>
            static void Main()
            {
                ServiceBase[] ServicesToRun;

                // 同一进程中可以运行多个用户服务。若要将
                // 另一个服务添加到此进程中，请更改下行以
                // 创建另一个服务对象。例如，
                //
                //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
                //
                ServicesToRun = new ServiceBase[] { new ServicesManagement() };

                ServiceBase.Run(ServicesToRun);
            }*/
    }
}