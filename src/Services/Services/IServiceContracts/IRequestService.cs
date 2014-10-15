using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace X3Platform.Services.IServiceContracts
{
    // 注意: 如果更改此处的接口名称“IRequestService”，
    // 也必须更新 App.config 中对“IRequestService”的引用。
    [ServiceContract]
    public interface IRequestService
    {
        // 任务: 在此处添加服务操作

        /// <summary>查询</summary>
        /// <param name="securityToken">安全标记</param>
        /// <param name="xml">Xml格式字符串</param>
        /// <returns></returns>
        [OperationContract]
        string Query(string securityToken, string xml);

        /// <summary>执行</summary>
        /// <param name="securityToken">安全标记</param>
        /// <param name="xml">Xml格式字符串</param>
        /// <returns></returns>
        [OperationContract]
        string Execute(string securityToken, string xml);

        /// <summary>默认测试方法</summary>
        /// <returns></returns>
        [OperationContract]
        string Hi();
    }
}