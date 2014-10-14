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

//
// 变更记录
//
// == 2009-05-10 == 
//
// 新建
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace X3Platform.CacheBuffer
{
    /// <summary>缓存存储器</summary>
    public sealed class CacheStorage<Key, Value> : ICacheStorage
        where Key : IComparable<Key>
        where Value : ICacheable
    {
        private object lockObject = new object();

        private object lockPurgeObject = new object();

        /// <summary>默认构造函数</summary>
        public CacheStorage() { }

        /// <summary></summary>
        private Dictionary<Key, Value> cacheStorage = null;

        private List<Key> expireKeys = new List<Key>();

        /// <summary>清理过期数据</summary>
        private void Purge()
        {
            if (cacheStorage != null && cacheStorage.Count != 0)
            {
                lock (lockPurgeObject)
                {
                    expireKeys.Clear();

                    Key[] keys = new Key[cacheStorage.Count + 100];
                    
                    cacheStorage.Keys.CopyTo(keys, 0);
    
                    try
                    {
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (keys[i] != null)
                            {
                                Value value = cacheStorage[keys[i]];

                                if (value != null && value.Expires < DateTime.Now)
                                {
                                    expireKeys.Add(keys[i]);
                                }
                            }
                        }

                        foreach (Key key in expireKeys)
                        {
                            Remove(key);
                        }
                    }
                    catch (KeyNotFoundException)
                    {

                    }
                }
            }
        }

        #region 函数:StoreItem(Key key, Value value)
        /// <summary>存储缓存项</summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        private void StoreItem(Key key, Value value)
        {
            if (cacheStorage == null)
            {
                // 创建缓存字典
                cacheStorage = new Dictionary<Key, Value>();
            }

            if (!cacheStorage.ContainsKey(key) && value != null)
            {
                lock (lockObject)
                {
                    if (!cacheStorage.ContainsKey(key))
                    {
                        cacheStorage.Add(key, value);
                    }
                }
            }
        }
        #endregion

        /// <summary>添加缓存项</summary>
        /// <param name="key">键</param>
        /// <param   name= "value">值</param>
        public void Add(Key key, Value value)
        {
            Purge();

            if (ContainsKey(key))
            {
                Remove(key);
            }

            // 存储
            StoreItem(key, value);
        }

        /// <summary>移除缓存项</summary>
        /// <param name="key">键</param>
        public void Remove(Key key)
        {
            if ((cacheStorage != null && cacheStorage.ContainsKey(key)))
            {
                lock (lockObject)
                {
                    cacheStorage.Remove(key);
                }
            }
        }

        /// <summary>获取相关的值</summary>
        /// <param name="key" >键</param>
        /// <returns>值</returns>
        public Value GetValue(Key key)
        {
            Purge();

            if (cacheStorage != null && cacheStorage.ContainsKey(key))
            {
                return cacheStorage[key];
            }
            else
            {
                return default(Value);
            }
        }

        /// <summary>是否存在相关的键</summary>
        /// <param name="key">键</param>
        /// <returns>返回 <c>true</c> 存在相关的键; 否则返回 <c>false</c>.</returns>
        public bool ContainsKey(Key key)
        {
            Purge();

            return (cacheStorage != null && cacheStorage.ContainsKey(key));
        }

        /// <summary>索引</summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public Value this[Key key]
        {
            get { return GetValue(key); }
            set { Add(key, value); }
        }

        /// <summary>获取键的枚举</summary>
        /// <returns>枚举的键</returns>
        public IEnumerable<Key> GetKeys()
        {
            Purge();

            foreach (Key key in cacheStorage.Keys)
            {
                yield return key;
            }
        }

        /// <summary>获取值的枚举</summary>
        /// <returns>枚举的值</returns>
        public IEnumerable<Value> GetValues()
        {
            foreach (KeyValuePair<Key, Value> cache in cacheStorage)
            {
                yield return cache.Value;
            }
        }

        /// <summary>获取<c> KeyValuePair&lt;Key,Value&gt; </c>的枚举.</summary>
        public IEnumerable<KeyValuePair<Key, Value>> GetItems()
        {
            foreach (KeyValuePair<Key, Value> cache in cacheStorage)
                yield return cache;
        }

        /// <summary>枚举</summary>
        /// <returns>枚举值</returns>
        public IEnumerator<Value> GetEnumerator()
        {
            foreach (KeyValuePair<Key, Value> cache in cacheStorage)
            {
                yield return cache.Value;
            }
        }

        /// <summary>缓存项数目</summary>
        public int Count
        {
            get { return (cacheStorage == null) ? 0 : cacheStorage.Count; }
        }

        /// <summary>全部清除</summary>
        public void Clear()
        {
            cacheStorage.Clear();
        }
    }
}