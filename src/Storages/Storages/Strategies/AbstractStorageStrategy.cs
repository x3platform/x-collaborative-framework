#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :StorageNodeInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Storages.Strategies
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>抽象存储策略</summary>
    public abstract class AbstractStorageStrategy : IStorageStrategy
    {
        /// <summary>存储架构</summary>
        protected IStorageSchema storageSchema = null;

        /// <summary>存储节点列表</summary>
        protected IList<IStorageNode> storageNodes = null;

        protected void LazyLoadStorageNodes()
        {
            if (storageNodes == null)
            {
                this.storageNodes = StorageContext.Instance.StorageNodeService.FindAllBySchemaId(this.storageSchema.Id);
            }
        }
        /// <summary>获取默认存储节点</summary>
        /// <returns></returns>
        public virtual IStorageNode GetStorageNode()
        {
            this.LazyLoadStorageNodes();

            foreach (IStorageNode storageNode in storageNodes)
            {

            }

            return (this.storageNodes.Count == 0) ? null : this.storageNodes[0];
        }

        /// <summary>获取存储节点</summary>
        public virtual IStorageNode GetStorageNode(string storageNodeType)
        {
            this.LazyLoadStorageNodes();

            foreach (IStorageNode storageNode in storageNodes)
            {
                if (storageNode.Type == storageNodeType)
                {
                    return storageNode;
                }
            }

            return this.GetStorageNode();
        }

        public abstract IStorageNode GetStorageNode(string storageNodeType, string index);

        public abstract IStorageNode GetStorageNode(string storageNodeType, string[] indexs);
    }
}
