namespace X3Platform.CacheBuffer
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;
#if NETCORE10
    using Microsoft.Extensions.Caching.Memory;
#endif
    using X3Platform.CacheBuffer.Configuration;

    /// <summary></summary>
    public class MemoryCacheProvider : ICacheProvider
    {
#if NETCORE10
        IMemoryCache cache = MemoryCache.Default;
#else
        ObjectCache cache = MemoryCache.Default;
#endif
        /// <summary></summary>
        public MemoryCacheProvider()
        {
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                return this.cache.Get(name);
            }
            set
            {
                this.cache.Set(name, value, GetAbsoluteExpiration());
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return this.cache.Contains(name);
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Get(string name)
        {
            return this.cache.Get(name);
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            this.cache.Set(name, value, GetAbsoluteExpiration());
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, object value)
        {
            this.cache.Add(name, value, GetAbsoluteExpiration());
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="minutes"></param>
        public void Add(string name, object value, int minutes)
        {
            this.cache.Add(name, value, DateTime.Now.AddMinutes(minutes));
        }

        /// <summary></summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            this.cache.Remove(name);
        }

        private DateTimeOffset GetAbsoluteExpiration()
        {
            return DateTime.Now.AddMinutes(CacheBufferConfigurationView.Instance.CacheDefaultDuration);
        }
    }
}