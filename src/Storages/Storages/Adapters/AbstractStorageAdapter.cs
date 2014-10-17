namespace X3Platform.Storages.StorageAdapters
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    #endregion

    /// <summary>抽象存储适配器</summary>
    public abstract class AbstractStorageAdapter : IStorageAdapter
    {
        /// <summary>选项</summary>
        protected Dictionary<string, string> options;

        /// <summary>存储节点</summary>
        protected IStorageSchema storageSchema = null;

        /// <summary>存储节点</summary>
        protected IStorageStrategy storageStrategy = null;

        /// <summary>存储节点</summary>
        protected IList<IStorageNode> storageNodes = null;

        /// <summary>构造函数</summary>
        public AbstractStorageAdapter() { }

        /// <summary>初始化</summary>
        protected void Initialize()
        {
            // 绑定存储策略信息 
            this.storageStrategy = (IStorageStrategy)KernelContext.CreateObject(this.storageSchema.StrategyClassName);

            // 绑定存储节点信息
            this.storageNodes = StorageContext.Instance.StorageNodeService.FindAllBySchemaId(storageSchema.Id);
        }

        /// <summary>根据节点类型获取存储节点信息</summary>
        /// <param name="type">存储节点类型 Master | Search</param>
        public virtual IStorageNode GetStorageNode(string type)
        {
            foreach (IStorageNode storageNode in this.storageNodes)
            {
                if (storageNode.Type == type)
                {
                    return storageNode;
                }
            }

            return null;
        }

        /// <summary>根据存储策略获取存储节点信息</summary>
        /// <param name="strategy">存储节点策略</param>
        public virtual IStorageNode GetStorageNode()
        {
            return this.storageStrategy.GetStorageNode();
        }

        /// <summary>执行</summary>
        /// <returns></returns>
        public abstract object Execute(string commandText, string[] args);
    }
}