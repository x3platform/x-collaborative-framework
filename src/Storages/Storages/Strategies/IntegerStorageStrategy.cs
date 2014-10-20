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
        /// <summary></summary>
        /// <param name="storageSchema"></param>
        public IntegerStorageStrategy(IStorageSchema storageSchema)
        {
            this.storageSchema = storageSchema;
        }

        /// <summary></summary>
        /// <param name="storageNodeType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary></summary>
        /// <param name="storageNodeType"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
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
