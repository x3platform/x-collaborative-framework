using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace X3Platform.Services.IServiceContracts
{
    // 注意: 如果更改此处的接口名称“IDebugService”，
    // 也必须更新 App.config 中对“IDebugService”的引用。
    /// <summary>调试服务</summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDebugServiceCallback))]
    public interface IDebugService
    {
        // 任务: 在此处添加服务操作

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        string[] Connect(string applicationName);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Disconnect();

        /// <summary>写入</summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        [OperationContract]
        void Write(string message);

        /// <summary>写入</summary>
        /// <param name="applicationName">应用名称</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        [OperationContract(Name = "WriteWithApplicationName")]
        void WriteWithApplicationName(string applicationName, string message);

        /// <summary>写入</summary>
        /// <param name="message">消息</param>
        [OperationContract]
        void WriteLine(string message);

        /// <summary>写入</summary>
        /// <param name="applicationName">应用名称</param>
        /// <param name="message">消息</param>
        [OperationContract(Name = "WriteLineWithApplicationName")]
        void WriteLineWithApplicationName(string applicationName, string message);

        /// <summary>默认测试方法</summary>
        /// <returns></returns>
        [OperationContract]
        string Hi();
    }
}