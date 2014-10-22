namespace X3Platform.Tasks.ServiceContracts
{
    #region Using Libraries
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Services;
    using System.Xml;

    using X3Platform.Apps;
    using X3Platform.Membership;
    
    using X3Platform.Tasks.IServiceContracts;
    using X3Platform.Tasks.Model;
    #endregion

    /// <summary></summary>
    public class TaskWebService : ITaskWebService
    {
        // -------------------------------------------------------
        // 发送 删除 更新
        // -------------------------------------------------------

        #region 函数:Synchronize(string securityToken, string xml)
        /// <summary>同步待办信息</summary>
        /// <param name="securityToken">安全标记</param>
        /// <param name="xml">Xml数据</param>
        /// <returns></returns>
        public string Synchronize(string securityToken, string xml)
        {
            return Process(securityToken, xml, "同步");
        }
        #endregion

        #region 函数:Send(string securityToken, string xml)
        /// <summary>发送任务</summary>
        /// <param name="securityToken">TaskInfo 实例的详细信息</param>
        /// <param name="xml">数据</param>
        public string Send(string securityToken, string xml)
        {
            return Process(securityToken, xml, "发送");
        }
        #endregion

        #region 函数:Update(string securityToken, string xml)
        /// <summary>更新一条数据记录</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="xml">任务编码</param>
        public string Update(string securityToken, string xml)
        {
            return Process(securityToken, xml, "更新");
        }
        #endregion

        #region 函数:Delete(string securityToken, string taskCode)
        /// <summary>删除记录</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="taskCode">任务编码</param>
        public string Delete(string securityToken, string taskCode)
        {
            try
            {
                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                TasksContext.Instance.TaskService.DeleteByTaskCode(applicationId, taskCode);

                return string.Format("删除任务【{0}】成功。", taskCode);
            }
            catch (Exception ex)
            {
                return string.Format("SecurityToken:{0}\r\nTaskCode:{1}\r\nException:{2}", securityToken, taskCode, ex.ToString());
            }
        }
        #endregion

        #region 私有函数:Process(string securityToken, string xml, string actionType)
        private string Process(string securityToken, string xml, string actionType)
        {
            try
            {
                /*
                * 
                * Xml数据示例

                   <?xml version="1.0" encoding="utf-8"?>

                   <root>
                       <task>
                           <taskCode>T00001</taskCode>
                           <title>Some Title</title>
                           <url>http://www.google.com</url>
                           <category>Document</category>
                           <senderId>administrator</senderId>
                           <receiver>0#test1,1#test2#2008-1-1 08:08:08</receiver>
                           <createDate>2008-01-01</createDate>
                       </task>
                       <task>
                           <taskCode>T00002</taskCode>
                           <title>Some Title</title>
                           <url>http://www.google.com</url>
                           <category>Document</category>
                           <senderId>administrator</senderId>
                           <receiver>0#test1,1#test2#2008-1-1 08:08:08</receiver>
                           <createDate>2008-01-01</createDate>
                       </task>
                   </root>
             
                   (*) 接收者信息说明 格式:{0}#{1}#{2} , {0}代表代办状态 1 完成 0 未完成, {1}相关的帐号, {2}{任务完成时间}
                
                * 
                * 
                */

                string result = null;

                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                TaskInfo param = null;

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xml);

                XmlNodeList list = doc.GetElementsByTagName("ajaxStorage")[0].ChildNodes;

                foreach (XmlNode item in list)
                {
                    param = new TaskInfo(applicationId);

                    param.Deserialize((XmlElement)item);

                    if (param.ReceiverGroup.Count == 0)
                    {
                        result += string.Format("任务【{0}】没有指派给任何用户请重新设置，", param.TaskCode);
                    }
                    else
                    {
                        if (actionType == "发送"
                            && TasksContext.Instance.TaskService.IsExistTaskCode(param.ApplicationId, param.TaskCode))
                        {
                            result += string.Format("发送失败(任务【{0}】已存在)，", param.TaskCode);
                        }
                        else if (actionType == "更新"
                            && !TasksContext.Instance.TaskService.IsExistTaskCode(param.ApplicationId, param.TaskCode))
                        {
                            result += string.Format("更新失败(任务【{0}】不存在)，", param.TaskCode);
                        }
                        else
                        {
                            param = TasksContext.Instance.TaskService.Save(param);

                            if (param == null)
                            {
                                result += string.Format("任务【{0}】" + actionType + "失败请重新" + actionType + "，", param.TaskCode);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(result))
                {
                    return "任务全部" + actionType + "成功。";
                }
                else
                {
                    return result.TrimEnd(new char[] { '，' }) + "。";
                }
            }
            catch (Exception ex)
            {
                return string.Format("SecurityToken:{0}\r\nXml:{1}\r\nException:{2}", securityToken, xml, ex.ToString());
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string securityToken, string taskCode)
        /// <summary>查询某条记录</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>返回一个 TaskInfo 实例的Xml元素信息</returns>
        [WebMethod]
        public string FindOne(string securityToken, string taskCode)
        {
            try
            {
                StringBuilder outString = new StringBuilder();

                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                TaskInfo param = TasksContext.Instance.TaskService.FindOneByTaskCode(applicationId, taskCode);

                outString.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

                outString.Append("<root>");

                outString.Append(param.Serializable());

                outString.Append("</root>");

                return outString.ToString();
            }
            catch (Exception ex)
            {
                return string.Format("SecurityToken:{0}\r\nTaskCode:{1}\r\nException:{2}", securityToken, taskCode, ex.ToString());
            }
        }
        #endregion

        #region 函数:FindAllByLoginName(string securityToken, string loginName, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="securityToken">安全标记</param>
        /// <param name="loginName">发送者账号</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 TaskInfo 实例的详细信息</returns>
        [WebMethod]
        public string FindAllByLoginName(string securityToken, string loginName, int length)
        {
            try
            {
                StringBuilder outString = new StringBuilder();

                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                IAccountInfo account = Membership.MembershipManagement.Instance.AccountService.FindOneByLoginName(loginName);

                string whereClause = string.Format(" ApplicationId = ##{0}## AND ( SenderId = ##{1}## OR Id IN ( SELECT TaskId FROM TaskReceiver WHERE ReceiverId = ##{1}## ))",
                    applicationId,
                    account.Id);

                IList<TaskWorkItemInfo> list = TasksContext.Instance.TaskService.FindAll(whereClause, length);

                outString.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

                outString.Append("<root>");

                foreach (TaskWorkItemInfo item in list)
                {
                    // outString.Append(item.Serializable());
                }

                outString.Append("</root>");

                return outString.ToString();
            }
            catch (Exception ex)
            {
                return string.Format("SecurityToken:{0}\r\nloginName:{1}\r\nLength:{2}\r\nException:{3}",
                    securityToken,
                    loginName,
                    length,
                    ex.ToString());
            }
        }
        #endregion

        #region 函数:FindAllByDate(string securityToken, string beginDate, string endDate, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="securityToken">安全标记</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 TaskInfo 实例的详细信息</returns>
        [WebMethod]
        public string FindAllByDate(string securityToken, string beginDate, string endDate, int length)
        {
            try
            {
                StringBuilder outString = new StringBuilder();

                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                string whereClause = string.Format(" ApplicationId = ##{0}## AND ( CreateDate > ##{1}## AND CreateDate < ##{2}## ) ",
                    applicationId,
                    beginDate,
                    endDate);

                IList<TaskWorkItemInfo> list = TasksContext.Instance.TaskService.FindAll(whereClause, length);

                outString.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

                outString.Append("<root>");

                foreach (TaskWorkItemInfo item in list)
                {
                    // outString.Append(item.Serializable());
                }

                outString.Append("</root>");

                return outString.ToString();
            }
            catch (Exception ex)
            {
                return string.Format("SecurityToken:{0}\r\nBeginDate:{1}\r\nEndDate:{2}\r\nLength:{3}\r\nException:{4}",
                    securityToken,
                    beginDate,
                    endDate,
                    length,
                    ex.ToString());
            }
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:SetTaskFinished(string securityToken, string taskCode)
        /// <summary>设置整个任务完成</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="taskCode">任务编码</param>
        public string SetTaskFinished(string securityToken, string taskCode)
        {
            try
            {
                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                TasksContext.Instance.TaskService.SetFinished(securityToken, taskCode);

                return string.Format("设置任务【{0}】已完成.", taskCode);
            }
            catch (Exception ex)
            {
                return string.Format("ApplicationId:{0}\r\nTaskCode:{1}\r\nException:{2}", securityToken, taskCode, ex.ToString());
            }
        }
        #endregion

        #region 函数:SetUsersFinished(string securityToken, string taskCode, string userIds)
        /// <summary>设置用户任务完成</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <param name="userIds">用户编号,以逗号分开</param>
        public string SetUsersFinished(string securityToken, string taskCode, string userIds)
        {
            try
            {
                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                TasksContext.Instance.TaskReceiverService.SetFinishedByTaskCode(applicationId, taskCode, userIds);

                return string.Format("设置任务【{0}】接收者【{1}】已完成.", taskCode, userIds);
            }
            catch (Exception ex)
            {
                return string.Format("ApplicationId:{0}\r\nTaskCode:{1}\r\nusers:{1}\r\nException:{2}", securityToken, taskCode, userIds, ex.ToString());
            }
        }
        #endregion

        #region 函数:IsExistTaskCode(string securityToken, string taskCode)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="securityToken">安全标识</param>
        /// <param name="taskCode">任务编码</param>
        /// <returns>布尔值</returns>
        public string IsExistTaskCode(string securityToken, string taskCode)
        {
            try
            {
                string applicationId = SecurityTokenManager.Authenticate(securityToken);

                return TasksContext.Instance.TaskService.IsExistTaskCode(securityToken, taskCode).ToString();
            }
            catch (Exception ex)
            {
                return string.Format("SecurityToken:{0}\r\nXml:{1}\r\nException:{2}", securityToken, taskCode, ex.ToString());
            }
        }
        #endregion
    }
}
