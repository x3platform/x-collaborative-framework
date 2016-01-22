namespace X3Platform.CacheBuffer
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;
    using X3Platform.CacheBuffer.Configuration;

    /// <summary></summary>
    public class MemoryCacheProvider : ICacheProvider
    {
        ObjectCache cache = MemoryCache.Default;

        /// <summary></summary>
        public MemoryCacheProvider()
        {
        }

        public object this[string name]
        {
            get
            {
                return this.cache.Get(name);
            }
            set
            {
                this.cache.Set(name, value, DateTime.Now.AddHours(6));
            }
        }

        public bool Contains(string name)
        {
            return this.cache.Contains(name);
        }

        public object Get(string name)
        {
            return this.cache.Get(name);
        }

        public void Set(string name, object value)
        {
            this.cache.Set(name, value, DateTime.Now.AddHours(6));
        }

        public void Add(string name, object value)
        {
            this.cache.Add(name, value, DateTime.Now.AddHours(6));
        }

        public void Add(string name, object value, int minutes)
        {
            this.cache.Add(name, value, DateTime.Now.AddHours(6));
        }

        public void Remove(string name)
        {
            this.cache.Remove(name);
        }

        private DateTimeOffset GetAbsoluteExpiration()
        {
            return DateTime.Now.AddMilliseconds(CacheBufferConfigurationView.Instance.CacheDefaultDuration);
        }
    }
}