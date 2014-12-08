namespace X3Platform.Tasks.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Membership;
    using X3Platform.Membership.Scope;
    using X3Platform.Spring;
    using X3Platform.Util;

    using X3Platform.Tasks.Model;
    using X3Platform.Tasks.IBLL;
    using X3Platform.Tasks.IDAL;
    using X3Platform.Tasks.Configuration;
    #endregion

    /// <summary></summary>
    public class TaskCategoryService : ITaskCategoryService
    {
        private TasksConfiguration configuration = null;

        private ITaskCategoryProvider provider = null;

        /// <summary></summary>
        public TaskCategoryService()
        {
            this.configuration = TasksConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(TasksConfiguration.ApplicationName, springObjectFile);

            // 创建数据提供器
            this.provider = objectBuilder.GetObject<ITaskCategoryProvider>(typeof(ITaskCategoryProvider));
        }

        /// <summary></summary>
        public TaskCategoryInfo this[string id]
        {
            get { return this.FindOne(id); }
        }

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(TaskCategoryInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="TaskCategoryInfo"/>详细信息</param>
        /// <returns>实例<see cref="TaskCategoryInfo"/>详细信息</returns>
        public TaskCategoryInfo Save(TaskCategoryInfo param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                throw new Exception("实例标识不能为空。");
            }

            bool isNewObject = !this.IsExist(param.Id);

            string methodName = isNewObject ? "新增" : "编辑";

            IAccountInfo account = KernelContext.Current.User;

            if (methodName == "新增")
            {
                param.AccountId = account.Id;
                param.AccountName = account.Name;
            }

            // 处理XSS特殊字符 
            param = StringHelper.ToSafeXSS<TaskCategoryInfo>(param);

            this.provider.Save(param);

            return param;
        }
        #endregion

        #region 函数:CanDelete(string id)
        /// <summary>检测相关类别能否被删除</summary>
        /// <param name="id">新闻类别标识</param>
        /// <returns></returns>
        public bool CanDelete(string id)
        {
            return this.provider.CanDelete(id);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">新闻类别标识</param>
        public void Delete(string id)
        {
            if (this.CanDelete(id))
            {
                this.provider.Delete(id);
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">标识</param>
        /// <returns>返回实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        public TaskCategoryInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindOneByCategoryIndex(string categoryIndex)
        /// <summary>查询某条记录</summary>
        /// <param name="categoryIndex">类别索引</param>
        /// <returns>返回实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        public TaskCategoryInfo FindOneByCategoryIndex(string categoryIndex)
        {
            return this.provider.FindOneByCategoryIndex(categoryIndex);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        public IList<TaskCategoryInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <returns>返回所有实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        public IList<TaskCategoryInfo> FindAll(string whereClause)
        {
            return FindAll(whereClause, 0);
        }
        #endregion

        #region 函数:FindAll(string whereClause, int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="whereClause">SQL 查询条件</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有实例<see cref="TaskCategoryInfo"/>的详细信息</returns>
        public IList<TaskCategoryInfo> FindAll(string whereClause, int length)
        {
            return this.provider.FindAll(whereClause, length);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="whereClause">WHERE 查询条件</param>
        /// <param name="orderBy">ORDER BY 排序条件</param>
        /// <param name="rowCount">记录行数</param>
        /// <returns>返回一个列表</returns> 
        public IList<TaskCategoryInfo> GetPaging(int startIndex, int pageSize, string whereClause, string orderBy, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, whereClause, orderBy, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">会员标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:SetStatus(int status)
        /// <summary>设置类别状态(停用/启用)</summary>
        /// <param name="id">新闻类别标识</param>
        /// <param name="status">1 将停用的类别启用，0 将启用的类别停用</param>
        /// <returns></returns>
        public bool SetStatus(string id, int status)
        {
            return this.provider.SetStatus(id, status);
        }
        #endregion
    }
}
