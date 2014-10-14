namespace X3Platform.Tasks.IServiceContracts
{
    #region Using Libraries
    using System;
    using System.ServiceModel;
    #endregion

    /// <summary>待办服务接口契约</summary>
    [ServiceContract]
    public interface ITaskWebService
    {
        #region 函数:Synchronize(string securityToken, string xml)
        /// <summary>同步待办信息</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="xml">Xml数据</param>
        /// <returns></returns>
        [OperationContract(Name = "Synchronize")]
        string Synchronize(string securityToken, string xml);
        #endregion

        #region 函数:Send(string securityToken, string xml)
        ///<summary>发送任务</summary>
        ///<param name="securityToken">TaskInfo 实例的详细信息</param>
        ///<param name="xml">数据</param>
        [OperationContract(Name = "Send")]
        string Send(string securityToken, string xml);
        #endregion

        #region 函数:Update(string securityToken, string xml)
        ///<summary>更新任务</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="xml">任务编码</param>
        [OperationContract(Name = "Update")]
        string Update(string securityToken, string xml);
        #endregion

        #region 函数:Delete(string securityToken, string taskCode)
        ///<summary>删除记录</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="taskCode">任务编码</param>
        [OperationContract(Name = "Delete")]
        string Delete(string securityToken, string taskCode);
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string securityToken, string taskCode)
        ///<summary>查询某条记录</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="taskCode">任务编码</param>
        ///<returns>返回一个 TaskInfo 实例的Xml元素信息</returns>
        [OperationContract(Name = "FindOne")]
        string FindOne(string securityToken, string taskCode);
        #endregion

        #region 函数:FindAllByLoginName(string securityToken, string loginName, int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="loginName">登录帐号</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有 TaskInfo 实例的详细信息</returns>
        [OperationContract(Name = "FindAllByLoginName")]
        string FindAllByLoginName(string securityToken, string loginName, int length);
        #endregion

        #region 函数:FindAllByDate(string securityToken, string beginDate, string endDate, int length)
        ///<summary>查询所有相关记录</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="beginDate">开始时间</param>
        ///<param name="endDate">结束时间</param>
        ///<param name="length">条数</param>
        ///<returns>返回所有 TaskInfo 实例的详细信息</returns>
        [OperationContract(Name = "FindAllByDate")]
        string FindAllByDate(string securityToken, string beginDate, string endDate, int length);
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:SetTaskFinished(string securityToken, string taskCode)
        ///<summary>设置整个任务完成</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="taskCode">任务编码</param>
        [OperationContract(Name = "SetTaskFinished")]
        string SetTaskFinished(string securityToken, string taskCode);
        #endregion

        #region 函数:SetUsersFinished(string securityToken, string taskCode, string userLoginNames)
        ///<summary>设置用户任务完成</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="taskCode">任务编码</param>
        ///<param name="userLoginNames">用户登录名，以[,]号分开</param>
        [OperationContract(Name = "SetUsersFinished")]
        string SetUsersFinished(string securityToken, string taskCode, string userLoginNames);
        #endregion

        #region 函数:IsExistTaskCode(string securityToken, string taskCode)
        ///<summary>查询是否存在相关的记录</summary>
        ///<param name="securityToken">安全标识</param>
        ///<param name="taskCode">任务编码</param>
        ///<returns>布尔值</returns>
        [OperationContract(Name = "IsExistTaskCode")]
        string IsExistTaskCode(string securityToken, string taskCode);
        #endregion
    }
}