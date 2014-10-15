using System.IO;

using X3Platform.Services.IServiceContracts;
using System;
using System.Xml;

namespace X3Platform.Services.ServiceContracts
{
    /// <summary></summary>
    public class RequestService : IRequestService
    {
        /// <summary>查询</summary>
        /// <param name="securityToken">安全标记</param>
        /// <param name="xml">Xml格式字符串</param>
        /// <returns></returns>
        public string Query(string securityToken, string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xml);

                return "没有找到相关的查询方法.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>执行</summary>
        /// <param name="securityToken">安全标记</param>
        /// <param name="xml">Xml格式字符串</param>
        /// <returns></returns>
        public string Execute(string securityToken, string xml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xml);

                return "没有找到相关的执行方法.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>默认测试方法</summary>
        /// <returns></returns>
        public string Hi()
        {
            return string.Format("当前时间:{0} 服务器({1})这是[RequestService]一个测试方法, 说明你已经成功连接到此服务.", DateTime.Now, System.Environment.MachineName);
        }
    }
}
