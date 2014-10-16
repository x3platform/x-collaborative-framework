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