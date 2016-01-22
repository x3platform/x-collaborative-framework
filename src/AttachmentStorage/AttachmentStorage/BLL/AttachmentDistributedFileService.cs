namespace X3Platform.AttachmentStorage.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.IBLL;
    using X3Platform.AttachmentStorage.IDAL;
    #endregion

    public sealed class AttachmentDistributedFileService : IAttachmentDistributedFileService
    {
        private IAttachmentDistributedFileProvider provider = null;

        public AttachmentDistributedFileService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = AttachmentStorageConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.provider = objectBuilder.GetObject<IAttachmentDistributedFileProvider>(typeof(IAttachmentDistributedFileProvider));
        }

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DistributedFileInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(DistributedFileInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="DistributedFileInfo"/>实例详细信息</param>
        /// <param name="message">数据库操作返回的相关信息</param>
        /// <returns><see cref="DistributedFileInfo"/>实例详细信息</returns>
        public DistributedFileInfo Save(DistributedFileInfo param)
        {
            return this.provider.Save(param);
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

        #region 函数:FindOne(int id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AccountInfo Id号</param>
        /// <returns>返回一个<see cref="DistributedFileInfo"/>实例的详细信息</returns>
        public DistributedFileInfo FindOne(string id)
        {
            return this.provider.FindOne(id);
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有<see cref="DistributedFileInfo"/>实例的详细信息</returns>
        public IList<DistributedFileInfo> FindAll()
        {
            return this.FindAll(new DataQuery() { Length = 1000 });
        }
        #endregion

        #region 函数:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="DistributedFileInfo"/>实例的详细信息</returns>
        public IList<DistributedFileInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">会员标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion
    }
}
