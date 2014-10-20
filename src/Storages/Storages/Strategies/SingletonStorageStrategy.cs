namespace X3Platform.Storages.Strategies
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>单例存储策略</summary>
    public sealed class SingletonStorageStrategy : AbstractStorageStrategy
    {
        /// <summary></summary>
        public SingletonStorageStrategy(IStorageSchema storageSchema)
        {
            this.storageSchema = storageSchema;
        }

        /// <summary></summary>
        public override IStorageNode GetStorageNode(string storageNodeType, string index)
        {
            this.LazyLoadStorageNodes();

            return this.GetStorageNode();
        }

        /// <summary></summary>
        public override IStorageNode GetStorageNode(string storageNodeType, string[] indexs)
        {
            this.LazyLoadStorageNodes();

            return this.GetStorageNode();
        }
    }
}
