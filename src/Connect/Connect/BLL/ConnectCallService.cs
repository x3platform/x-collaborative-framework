namespace X3Platform.Connect.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Messages;
    using X3Platform.Spring;

    using X3Platform.Connect.Configuration;
    using X3Platform.Connect.Model;
    using X3Platform.Connect.IBLL;
    using X3Platform.Connect.IDAL;
    #endregion

    public sealed class ConnectCallService : IConnectCallService
    {
        private IConnectCallProvider provider = null;

        /// <summary>队列</summary>
        private IMessageQueueObject queue = null;

        public ConnectCallService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = ConnectConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(ConnectConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<IConnectCallProvider>(typeof(IConnectCallProvider));

            // 设置队列
            this.queue = objectBuilder.GetObject<IMessageQueueObject>("X3Platform.Connect.Queues.ConnectCallQueue", new object[]{
                ConnectConfigurationView.Instance.MessageQueueHostName, 
                ConnectConfigurationView.Instance.MessageQueuePort,
                ConnectConfigurationView.Instance.MessageQueueUsername, 
                ConnectConfigurationView.Instance.MessageQueuePassword,
                ConnectConfigurationView.Instance.MessageQueueName});
        }

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(ConnectCallInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">AccountInfo 实例详细信息</param>
        /// <param name="message">数据库操作返回的相关信息</param>
        /// <returns>AccountInfo 实例详细信息</returns>
        public ConnectCallInfo Save(ConnectCallInfo param)
        {
            // 如果消息队列可用，则将数据发送到队列。
            if (ConnectConfigurationView.Instance.MessageQueueMode == "ON" && queue.Enabled)
            {
                queue.Send(param);

                return param;
            }
            else
            {
                return this.provider.Save(param);
            }
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AccountInfo Id号</param>
        /// <returns>返回一个 AccountInfo 实例的详细信息</returns>
        public ConnectCallInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
        public IList<ConnectCallInfo> FindAll()
        {
            return FindAll(new DataQuery() { Length = 1000 });
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有 AccountInfo 实例的详细信息</returns>
        public IList<ConnectCallInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        public IList<ConnectCallInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion
    }
}
