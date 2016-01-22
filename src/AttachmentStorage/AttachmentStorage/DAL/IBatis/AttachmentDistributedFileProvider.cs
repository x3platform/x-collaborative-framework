namespace X3Platform.AttachmentStorage.DAL.IBatis
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    using X3Platform.Data;
    using X3Platform.IBatis.DataMapper;
    using X3Platform.Util;

    using X3Platform.AttachmentStorage.Configuration;
    using X3Platform.AttachmentStorage.IDAL;
    #endregion

    [DataObject]
    public class AttachmentDistributedFileProvider : IAttachmentDistributedFileProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Attachment_DistributedFile";

        /// <summary></summary>
        public AttachmentDistributedFileProvider()
        {
            this.ibatisMapping = AttachmentStorageConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(DistributedFileInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="DistributedFileInfo"/>实例详细信息</param>
        /// <returns><see cref="DistributedFileInfo"/>实例详细信息</returns>
        public DistributedFileInfo Save(DistributedFileInfo param)
        {
            if (IsExist(param.Id))
            {
                new Exception("The same entity's id already exists. |-_-||");
            }

            this.ibatisMapper.Insert(StringHelper.ToProcedurePrefix(string.Format("{0}_Save", this.tableName)), param);

            return param;
        }
        #endregion

        #region 函数:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) { return; }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="param">DistributedFileInfo Id号</param>
        /// <returns>返回一个<see cref="DistributedFileInfo"/>实例的详细信息</returns>
        public DistributedFileInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<DistributedFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="DistributedFileInfo"/>实例的详细信息</returns>
        public IList<DistributedFileInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", query.GetWhereSql());
            args.Add("OrderBy", query.GetOrderBySql());
            args.Add("Length", query.Length);

            IList<DistributedFileInfo> list = this.ibatisMapper.QueryForList<DistributedFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);

            return list;
        }
        #endregion

        // -------------------------------------------------------
        // 自定义功能
        // -------------------------------------------------------

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id"><see cref="DistributedFileInfo"/>实例详细信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("实例标识不能为空.");

            bool isExist = true;

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            isExist = ((int)this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", this.tableName)), args) == 0) ? false : true;

            return isExist;
        }
        #endregion

        #region 函数:Rename(string id, string name)
        /// <summary>重命名</summary>
        /// <param name="id">附件标识</param>
        /// <param name="name">新的附件名称</param>
        public void Rename(string id, string name)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", StringHelper.ToSafeSQL(id));
            args.Add("AttachmentName", StringHelper.ToSafeSQL(name));

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_Rename", this.tableName)), args);
        }
        #endregion
    }
}
