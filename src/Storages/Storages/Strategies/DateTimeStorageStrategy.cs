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

    /// <summary>根据时间规则存储的存储策略</summary>
    public sealed class DateTimeStorageStrategy : AbstractStorageStrategy
    {
        public DateTimeStorageStrategy(IStorageSchema storageSchema)
        {
            this.storageSchema = storageSchema;
        }

        public override IStorageNode GetStorageNode(string storageNodeType, string index)
        {
            this.LazyLoadStorageNodes();

            throw new NotImplementedException();
        }

        public override IStorageNode GetStorageNode(string storageNodeType, string[] indexs)
        {
            this.LazyLoadStorageNodes();

            throw new NotImplementedException();
        }
    }
}
