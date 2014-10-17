namespace X3Platform.Storages.StorageAdapters
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary>主从存储适配器</summary>
    public sealed class MasterSlavesStorageAdapter : AbstractStorageAdapter
    {
        /// <summary>构造函数</summary>
        public MasterSlavesStorageAdapter(IStorageSchema storageSchema)
        {
            // 获取存储架构信息
            this.storageSchema = storageSchema;

            // 初始化存储策略和存储节点信息
            this.Initialize();
        }

        /// <summary>执行</summary>
        /// <returns></returns>
        public override object Execute(string commandText, string[] args)
        {
            return null;
        }
    }
}