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
// Date         :2009-05-10
//
// =============================================================================

using System;

namespace X3Platform.CacheBuffer
{
    /// <summary>对象缓存接口</summary>
    public interface ICacheable
    {
        /// <summary>过期时间</summary>
        DateTime Expires { get; set; }
    }
}