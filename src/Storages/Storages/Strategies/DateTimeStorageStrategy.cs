namespace X3Platform.Storages.Strategies
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>根据时间规则存储的存储策略</summary>
    public sealed class DateTimeStorageStrategy : AbstractStorageStrategy
    {
        /// <summary></summary>
        /// <param name="storageSchema"></param>
        public DateTimeStorageStrategy(IStorageSchema storageSchema)
        {
            this.storageSchema = storageSchema;
        }

        /// <summary></summary>
        public override IStorageNode GetStorageNode(string storageNodeType, string index)
        {
            this.LazyLoadStorageNodes();

            throw new NotImplementedException();
        }

        /// <summary></summary>
        public override IStorageNode GetStorageNode(string storageNodeType, string[] indexs)
        {
            this.LazyLoadStorageNodes();

            throw new NotImplementedException();
        }
    }
}
