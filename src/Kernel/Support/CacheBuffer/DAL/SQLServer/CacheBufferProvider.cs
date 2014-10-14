// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :CacheBufferProvider.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.CacheBuffer.DAL.SQLServer
{
    using System;
    using System.Collections.Generic;

    /// <summary>�����ṩ��</summary>
    public class CacheBufferProvider : ICacheBufferProvider
    {
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

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
            return dictionary.ContainsKey(key);
        }

        public object Read(string key)
        {
            object value = null;

            if (dictionary.TryGetValue(key, out value))
                return value;

            return null;
        }

        public void Write(string key, object value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        public void Write(string key, object value, int minutes)
        {
            if (!this.Contains(key))
            {
                //cacheManager.Add(key, value, CacheObjectPriority.Normal, null, new SlidingTime(TimeSpan.FromMinutes(minutes)));
            }
        }

        public void Delete(string key)
        {
            dictionary.Remove(key);
        }

        public void Clear(DateTime date)
        {

        }

        public void Clear()
        {

        }
    }
}