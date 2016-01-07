using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Caching;

namespace X3Platform.CacheBuffer
{
    /// <summary>缓存字典</summary>
    public sealed class CachingDictionary
    {
        private const string prefix = "Dictionary#";

        /// <summary></summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Create<T>(string name)
        {
            T obj = (T)CachingManager.Get(name);
            
            if (obj == null) 
            {
                obj = default(T);
            }

            return obj;
        }
    }
}