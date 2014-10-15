namespace X3Platform.Connect.IServiceContracts
{
    using System.ServiceModel;

    /// <summary>新闻服务接口契约</summary>
    [ServiceContract]
    public interface IConnectWebService
    {
        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string securityToken, string taskCode)
        /// <summary>查询某条记录</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>返回一个 ConnectInfo 实例的Xml元素信息</returns>
        [OperationContract(Name = "FindOne")]
        string FindOne(string securityToken, string taskCode);
        #endregion

        #region 函数:FindAllByLoginName(string securityToken, string loginName, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="loginName">登录帐号</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 ConnectInfo 实例的详细信息</returns>
        [OperationContract(Name = "FindAllByLoginName")]
        string FindAllByLoginName(string securityToken, string loginName, int length);
        #endregion

        #region 函数:FindAllByDate(string securityToken, string beginDate, string endDate, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 ConnectInfo 实例的详细信息</returns>
        [OperationContract(Name = "FindAllByDate")]
        string FindAllByDate(string securityToken, string beginDate, string endDate, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------
    }
}