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

    /// <summary>根据整数规则存储的存储策略</summary>
    public sealed class IntegerStorageStrategy : AbstractStorageStrategy
    {
        public IntegerStorageStrategy(IStorageSchema storageSchema)
        {
            this.storageSchema = storageSchema;
        }

        public override IStorageNode GetStorageNode(string storageNodeType, string index)
        {
            this.LazyLoadStorageNodes();

            int hashCode = index.GetHashCode();

            IList<IStorageNode> list = new List<IStorageNode>();

            foreach (IStorageNode storageNode in this.storageNodes)
            {
                if (storageNode.Type == storageNodeType)
                {
                    list.Add(storageNode);
                }
            }

            if (list.Count == 0)
            {
                return this.GetStorageNode();
            }
            else
            {
                return list[hashCode % list.Count];
            }
        }

        public override IStorageNode GetStorageNode(string storageNodeType, string[] indexs)
        {
            this.LazyLoadStorageNodes();

            int hashCode = indexs.GetHashCode();

            IList<IStorageNode> list = new List<IStorageNode>();

            foreach (IStorageNode storageNode in this.storageNodes)
            {
                if (storageNode.Type == storageNodeType)
                {
                    list.Add(storageNode);
                }
            }

            if (list.Count == 0)
            {
                return this.GetStorageNode();
            }
            else
            {
                return list[hashCode % list.Count];
            }
        }
    }
}
