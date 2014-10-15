#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IStorageStrategy.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Storages
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    /// <summary>应用存储策略接口</summary>
    public interface IStorageStrategy
    {
        /// <summary>获取默认存储节点</summary>
        /// <returns></returns>
        IStorageNode GetStorageNode();

        /// <summary>根据存储节点类型获取存储节点</summary>
        /// <param name="storageNodeType">存储节点类型</param>
        /// <returns></returns>
        IStorageNode GetStorageNode(string storageNodeType);

        /// <summary>根据存储节点类型和主键获取存储节点</summary>
        /// <param name="storageNodeType">存储节点类型</param>
        /// <param name="primarykey">主键数据</param>
        /// <returns></returns>
        IStorageNode GetStorageNode(string storageNodeType, string primarykey);

        /// <summary>根据存储节点类型和多个主键获取存储节点</summary>
        /// <param name="storageNodeType">存储节点类型</param>
        /// <param name="primarykeys"></param>
        /// <returns></returns>
        IStorageNode GetStorageNode(string storageNodeType, string[] primarykeys);
    }
}
