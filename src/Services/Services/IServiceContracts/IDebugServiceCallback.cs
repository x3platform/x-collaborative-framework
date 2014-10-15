using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace X3Platform.Services.IServiceContracts
{
    /// <summary>调试服务回调接口</summary>
    public interface IDebugServiceCallback
    {
        /// <summary>接收调试信息</summary>
        /// <param name="text"></param>
        [OperationContract(IsOneWay = true)]
        void Response(string text);
    }
}