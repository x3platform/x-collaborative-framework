// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;

namespace X3Platform.CategoryIndexes
{
    /// <summary>对象缓存接口</summary>
    public interface ICategoryIndex
    {
        string Text { get; set; }

        string Value { get; set; }

        ICategoryIndex Parent { get; }
        
        IList<ICategoryIndex> ChildNodes { get; }
        
        bool HasChildren { get; }

        void LoadChildNode(ICategoryIndex node);

        void LoadChildNodes(IList<ICategoryIndex> nodes);
    }
}