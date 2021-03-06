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
    using System.Text;
    #endregion

    [DataObject]
    public class AttachmentFileProvider : IAttachmentFileProvider
    {
        /// <summary>IBatis映射文件</summary>
        private string ibatisMapping = null;

        /// <summary>IBatis映射对象</summary>
        private ISqlMapper ibatisMapper = null;

        /// <summary>数据表名</summary>
        private string tableName = "tb_Attachment_File";

        /// <summary></summary>
        public AttachmentFileProvider()
        {
            this.ibatisMapping = AttachmentStorageConfigurationView.Instance.Configuration.Keys["IBatisMapping"].Value;

            this.ibatisMapper = ISqlMapHelper.CreateSqlMapper(this.ibatisMapping, true);
        }

        public IAttachmentFileInfo this[string index]
        {
            get { return this.FindOne(index); }
        }

        // -------------------------------------------------------
        // 保存 添加 修改 删除
        // -------------------------------------------------------

        #region 函数:Save(IAttachmentFileInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"><see cref="IAttachmentFileInfo" />实例详细信息</param>
        /// <returns><see cref="IAttachmentFileInfo" />实例详细信息</returns>
        public IAttachmentFileInfo Save(IAttachmentFileInfo param)
        {
            if (this.IsExist(param.Id)) { new Exception("The same entity's id already exists. |-_-||"); }

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

            // 01.删除物理文件信息

            IAttachmentFileInfo file = FindOne(id);

            if (file != null)
            {
                string path = file.VirtualPath.Replace("{uploads}", AttachmentStorageConfigurationView.Instance.PhysicalUploadFolder);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            // 02.删除数据库信息

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id IN ('{0}') ", StringHelper.ToSafeSQL(id).Replace(",", "','")));

            this.ibatisMapper.Delete(StringHelper.ToProcedurePrefix(string.Format("{0}_Delete", this.tableName)), args);

            // 03.删除分布式文件数据

            DistributedFileStorage.Delete(id);
        }
        #endregion

        // -------------------------------------------------------
        // 查询
        // -------------------------------------------------------

        #region 函数:FindOne(string id)
        /// <summary>查询某条记录</summary>
        /// <param name="param">IAttachmentFileInfo Id号</param>
        /// <returns>返回一个<see cref="IAttachmentFileInfo" />实例的详细信息</returns>
        public IAttachmentFileInfo FindOne(string id)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("Id", id);

            return this.ibatisMapper.QueryForObject<IAttachmentFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindOne", this.tableName)), args);
        }
        #endregion

        #region 属性:FindAll(string whereClause,int length)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <param name="length">条数</param>
        /// <returns>返回所有<see cref="IAttachmentFileInfo" />实例的详细信息</returns>
        public IList<IAttachmentFileInfo> FindAll(DataQuery query)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            //if (whereClause.IndexOf(" ORDER BY ") == -1)
            //{
            //    whereClause += " ORDER BY OrderId, AttachmentName ";
            //}

            args.Add("WhereClause", query.GetWhereSql());
            args.Add("OrderBy", query.GetOrderBySql());
            args.Add("Length", query.Length);

            return this.ibatisMapper.QueryForList<IAttachmentFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_FindAll", this.tableName)), args);
        }
        #endregion

        #region 函数:FindAllByEntityId(string entityClassName, string entityId)
        /// <summary>查询所有相关记录</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="entityId">实体类标识</param>
        /// <returns>返回所有<see cref="IAttachmentFileInfo" />实例的详细信息</returns>
        public IList<IAttachmentFileInfo> FindAllByEntityId(string entityClassName, string entityId)
        {
            DataQuery query = new DataQuery();

            query.Where.Add("EntityClassName", entityClassName);
            query.Where.Add("EntityId", entityId);

            return this.FindAll(query);
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
            Dictionary<string, object> args = new Dictionary<string, object>();

            string whereClause = BuildWhereClause(query);
            string orderBy = query.GetOrderBySql(" CreatedDate DESC ");

            args.Add("StartIndex", startIndex);
            args.Add("PageSize", pageSize);
            args.Add("WhereClause", whereClause);
            args.Add("OrderBy", orderBy);

            args.Add("RowCount", 0);

            IList<IAttachmentFileInfo> list = this.ibatisMapper.QueryForList<IAttachmentFileInfo>(StringHelper.ToProcedurePrefix(string.Format("{0}_GetPaging", this.tableName)), args);

            rowCount = Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_GetRowCount", this.tableName)), args));

            return list;
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录</summary>
        /// <param name="id"><see cref="IAttachmentFileInfo" />实例详细信息</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            if (string.IsNullOrEmpty(id)) { throw new Exception("实例标识不能为空。"); }

            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Format(" Id = '{0}' ", StringHelper.ToSafeSQL(id)));

            return (Convert.ToInt32(this.ibatisMapper.QueryForObject(StringHelper.ToProcedurePrefix(string.Format("{0}_IsExist", tableName)), args)) == 0) ? false : true;
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

        #region 函数:SetValid(string entityClassName, string entityId, string attachmentFileIds, bool append)
        /// <summary>设置有效的文件信息</summary>
        /// <param name="entityClassName">实体类名称</param>
        /// <param name="entityId">实体标识</param>
        /// <param name="attachmentFileIds">附件唯一标识，多个附件以逗号隔开</param>
        /// <param name="append">附加文件</param>
        public void SetValid(string entityClassName, string entityId, string attachmentFileIds, bool append)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            args.Add("WhereClause", string.Empty);
            args.Add("FileStatus", 0);
            
            if (!append)
            {
                args["WhereClause"] = string.Format(" EntityClassName = '{0}' AND EntityId = '{1}' ", 
                    StringHelper.ToSafeSQL(entityClassName), 
                    StringHelper.ToSafeSQL(entityId));

                this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetFileStatus", this.tableName)), args);
            }

            args["WhereClause"] = string.Format(" EntityClassName = '{0}' AND EntityId = '{1}' AND Id IN ('{2}') ", 
                StringHelper.ToSafeSQL(entityClassName), 
                StringHelper.ToSafeSQL(entityId),
                StringHelper.ToSafeSQL(attachmentFileIds).Replace(",", "','"));

            args["FileStatus"] = 1;

            this.ibatisMapper.Update(StringHelper.ToProcedurePrefix(string.Format("{0}_SetFileStatus", this.tableName)), args);
        }
        #endregion

        // -------------------------------------------------------
        // 自定义查询条件
        // -------------------------------------------------------

        #region 属性:BuildWhereClause(DataQuery query)
        /// <summary>根据场景名称构建查询条件</summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string BuildWhereClause(DataQuery query)
        {
            string whereClause = string.Empty;

            string scence = query.Variables["scence"];

            if (string.IsNullOrEmpty(scence))
            {
                // 默认查询方式
                whereClause = query.GetWhereSql(new Dictionary<string, string>() { { "AttachmentName", "LIKE" } });
            }
            else
            {
                StringBuilder outString = new StringBuilder();

                if (scence == "Query")
                {
                    DataQueryBuilder.Equal(query.Where, "id", outString);
                    DataQueryBuilder.Equal(query.Where, "attachmentName", outString);
                    DataQueryBuilder.Equal(query.Where, "entityId", outString);

                    DataQueryBuilder.Between(query.Where, "createdDate", "beginDate", "endDate", outString);

                    return outString.ToString();
                }
            }

            return whereClause;
        }
        #endregion
    }
}
