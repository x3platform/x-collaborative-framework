#region Copyright & Author
// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :AsyncMethod.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform
{
    /// <summary></summary>
    public class AsyncMethod
    {
        /// <summary>异步刷新缓存委托</summary>  
        /// <typeparam name="T"></typeparam>  
        /// <returns></returns>  
        public delegate object AsyncRefreshCacheDelegate();

        /// <summary>异步刷新缓存委托</summary>  
        /// <typeparam name="T"></typeparam>  
        /// <returns></returns>  
        public delegate T AsyncRefreshCacheDelegate<T>();
    }
}