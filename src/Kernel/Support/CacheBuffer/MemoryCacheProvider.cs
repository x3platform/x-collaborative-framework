namespace X3Platform.CacheBuffer
{
    using System;
    using System.Collections.Generic;

    /// <summary></summary>
    public class MemoryCacheProvider : ICacheProvider
    {
        private  CacheStorage<string, CacheItem> cacheStorage = new CacheStorage<string, CacheItem>();

        /// <summary>���캯��</summary>
        public CacheBufferProvider()
        {
        }

        public object this[string key]
        {
            get { return this.Read(key); }
            set { this.Write(key, value); }
        }

        public bool Contains(string key)
        {
            return cacheStorage.ContainsKey(key);
        }

        public object Read(string key)
        {
            return cacheStorage[key].Value;
        }

        public void Write(string key, object value)
        {
            if (!this.Contains(key))
            {
                cacheStorage.Add(key, new CacheItem(value));
            }
        }

        public void Write(string key, object value, int minutes)
        {
            if (!this.Contains(key))
            {
                cacheStorage.Add(key, new CacheItem(value, DateTime.Now.AddMinutes(minutes)));
            }
        }

        public void Delete(string key)
        {
            cacheStorage.Remove(key);
        }

        public void Clear()
        {
            cacheStorage.Clear();
        }
    }
}