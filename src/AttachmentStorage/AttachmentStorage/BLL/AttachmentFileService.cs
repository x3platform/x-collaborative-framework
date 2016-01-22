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
    using X3Platform.Membership;
    using X3Platform.Apps;
    #endregion

    public sealed class AttachmentFileService : IAttachmentFileService
    {
        private IAttachmentFileProvider provider = null;

        public AttachmentFileService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = AttachmentStorageConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(AttachmentStorageConfiguration.ApplicationName, springObjectFile);

            // 创建数据服务对象
            this.provider = objectBuilder.GetObject<IAttachmentFileProvider>(typeof(IAttachmentFileProvider));
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
            IAccountInfo account = KernelContext.Current.User;

            if (AppsSecurity.IsAdministrator(account, AttachmentStorageConfiguration.ApplicationName))
            {
                this.provider.Delete(id);
            }
            else
            {
                IAttachmentFileInfo file = this.FindOne(id);
                
                if (file.CreatedBy == account.Id)
                {
                    this.provider.Delete(id);
                }
            }
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
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

        #region 函数:FindAll(DataQuery query)
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

        #region 函数:SetValid(string entityClassName, string entityId, string attachmentFileIds, bool append = false)
        /// <summary>设置有效的文件信息</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="attachmentFileIds">附件唯一标识，多个附件以逗号隔开</param>
        /// <param name="append">附加文件</param>
        public void SetValid(string entityClassName, string entityId, string attachmentFileIds, bool append = false)
        {
            this.provider.SetValid(entityClassName, entityId, attachmentFileIds, append);
        }
        #endregion

        #region 函数:Copy(IAttachmentFileInfo param, string entityClassName, string entityId)
        /// <summary>物理复制全部附件信息到实体类</summary>
        /// <param name="param"><see cref="IAttachmentFileInfo" />实例详细信息</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>新的<see cref="IAttachmentFileInfo" />实例详细信息</returns>
        public IAttachmentFileInfo Copy(IAttachmentFileInfo param, string entityClassName, string entityId)
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

        #region 函数:Move(IAttachmentFileInfo param, string path)
        /// <summary>物理移动附件路径</summary>
        /// <param name="param">实例<see cref="IAttachmentFileInfo"/>详细信息</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="entityClassName">实体类名称</param>
        /// <returns>新的 实例<see cref="IAttachmentFileInfo"/>详细信息</returns>
        public IAttachmentFileInfo Move(IAttachmentFileInfo param, string path)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
