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
    using X3Platform.AttachmentStorage.Util;
    #endregion

    public sealed class AttachmentStorageService : IAttachmentStorageService
    {
        private IAttachmentStorageProvider provider = null;

        private AttachmentStorageConfiguration configuration = null;

        public AttachmentStorageService()
        {
            // 读取配置信息
            this.configuration = AttachmentStorageConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.provider = objectBuilder.GetObject<IAttachmentStorageProvider>(typeof(IAttachmentStorageProvider));
        }

        #region 索引:this[string id]
        /// <summary>索引</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IAttachmentFileInfo this[string id]
        {
            get { return this.FindOne(id); }
        }
        #endregion

        // -------------------------------------------------------
        // 保存 删除
        // -------------------------------------------------------

        #region 函数:Save(AccountInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param">实例<see cref="IAttachmentFileInfo"/>详细信息</param>
        /// <returns>实例<see cref="IAttachmentFileInfo"/>详细信息</returns>
        public IAttachmentFileInfo Save(IAttachmentFileInfo param)
        {
            if (string.IsNullOrEmpty(param.AttachmentName)) { throw new ArgumentNullException("附件名称必填。"); }

            param.FileType = param.FileType.ToLower();

            return provider.Save(param);
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            provider.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(int id)
        /// <summary>查询某条记录</summary>
        /// <param name="id">AccountInfo Id号</param>
        /// <returns>返回一个 实例<see cref="IAttachmentFileInfo"/>的详细信息</returns>
        public IAttachmentFileInfo FindOne(string id)
        {
            AttachmentFileInfo param = (AttachmentFileInfo)provider.FindOne(id);

            IAttachmentParentObject parent = new AttachmentParentObject();


            parent.EntityId = param.EntityId;
            parent.EntityClassName = param.EntityClassName;
            parent.AttachmentEntityClassName = KernelContext.ParseObjectType(typeof(AttachmentFileInfo));
            parent.AttachmentFolder = UploadPathHelper.GetAttachmentFolder(param.VirtualPath, param.FolderRule);

            param.Parent = parent;

            return param;
        }
        #endregion

        #region 函数:FindAll()
        /// <summary>查询所有相关记录</summary>
        /// <returns>返回所有 实例<see cref="IAttachmentFileInfo"/>的详细信息</returns>
        public IList<IAttachmentFileInfo> FindAll()
        {
            return FindAll(string.Empty);
        }
        #endregion

        #region 函数:FindAll(string whereClause)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有 实例<see cref="IAttachmentFileInfo"/>的详细信息</returns>
        public IList<IAttachmentFileInfo> FindAll(string whereClause)
        {
            return this.FindAll(new DataQuery() { Length = 1000 });
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有 实例<see cref="IAttachmentFileInfo"/>的详细信息</returns>
        public IList<IAttachmentFileInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
        }
        #endregion

        #region 函数:FindAllByEntityId(string entityClassName, string entityId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <returns>返回所有<see cref="IAttachmentFileInfo" />实例的详细信息</returns>
        public IList<IAttachmentFileInfo> FindAllByEntityId(string entityClassName, string entityId)
        {
            return provider.FindAllByEntityId(entityClassName, entityId);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 属性:GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个列表实例</returns> 
        public IList<IAttachmentFileInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id">会员标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return provider.IsExist(id);
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>重命名</summary>
        /// <param name="id">附件标识</param>
        /// <param name="name">新的附件名称</param>
        public void Rename(string id, string name)
        {
            provider.Rename(id, name);
        }
        #endregion

        #region 函数:Copy(IAttachmentFileInfo param, string entityId, string entityClassName)
        /// <summary>物理复制全部附件信息到实体类</summary>
        /// <param name="param"><see cref="IAttachmentFileInfo" />实例详细信息</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>新的<see cref="IAttachmentFileInfo" />实例详细信息</returns>
        public IAttachmentFileInfo Copy(IAttachmentFileInfo param, string entityId, string entityClassName)
        {
            IAttachmentParentObject parent = new AttachmentParentObject();

            parent.EntityId = entityId;
            parent.EntityClassName = entityClassName;
            parent.AttachmentEntityClassName = KernelContext.ParseObjectType(typeof(AttachmentFileInfo));
            parent.AttachmentFolder = UploadPathHelper.GetAttachmentFolder(param.VirtualPath, param.FolderRule);

            IAttachmentFileInfo attachment = UploadFileHelper.CreateAttachmentFile(parent, param.AttachmentName, param.FileType, param.FileSize, param.FileData);

            attachment.Save();

            return attachment;
        }
        #endregion
    }
}
