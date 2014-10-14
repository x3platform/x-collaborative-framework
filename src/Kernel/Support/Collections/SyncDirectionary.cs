namespace X3Platform.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading;

    /// <summary>同步的集合。</summary>
    public class SyncDictionary : IDictionary
    {
        /// <summary>用于保存被包装的普通字典实例。</summary>
        private IDictionary innerDictionary = null;


        public SyncDictionary()
            : this(new HybridDictionary())
        {

        }

        public SyncDictionary(IDictionary dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            this.innerDictionary = dictionary;
        }

        #region IDictionary 成员
        public void Add(object key, object value)
        {
            if (!this.innerDictionary.Contains(key))
            {
                lock (this.innerDictionary.SyncRoot)
                {
                    if (!this.innerDictionary.Contains(key))
                    {
                        this.innerDictionary.Add(key, value);
                    }
                }
            }
        }

        public void Clear()
        {
            if (this.innerDictionary.Count > 0)
            {
                lock (this.innerDictionary.SyncRoot)
                {
                    if (this.innerDictionary.Count > 0)
                    {
                        this.innerDictionary.Clear();
                    }
                }
            }
        }

        public bool Contains(object key)
        {
            return this.innerDictionary.Contains(key);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return this.innerDictionary.GetEnumerator();
        }

        bool IDictionary.IsFixedSize
        {
            get { return this.innerDictionary.IsFixedSize; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return this.innerDictionary.IsReadOnly; }
        }

        public ICollection Keys
        {
            get
            {
                lock (this.innerDictionary.SyncRoot)
                {
                    return this.innerDictionary.Keys;
                }
            }
        }

        public void Remove(object key)
        {
            if (this.innerDictionary.Contains(key))
            {
                lock (this.innerDictionary.SyncRoot)
                {
                    if (this.innerDictionary.Contains(key))
                    {
                        this.innerDictionary.Remove(key);
                    }
                }
            }
        }

        public ICollection Values
        {
            get
            {
                lock (this.innerDictionary.SyncRoot)
                {
                    return this.innerDictionary.Values;
                }
            }
        }

        public object this[object key]
        {
            set
            {
                lock (this.innerDictionary.SyncRoot)
                {
                    this.innerDictionary[key] = value;
                }
            }
            get
            {
                return this.innerDictionary[key];
            }
        }
        #endregion

        #region ICollection 成员
        public void CopyTo(Array array, int index)
        {
            lock (this.innerDictionary.SyncRoot)
            {
                this.innerDictionary.CopyTo(array, index);
            }
        }

        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return this.innerDictionary.SyncRoot; }
        }
        #endregion

        #region IEnumerable 成员
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }

    /// <summary>同步的表示键和值（字典）的集合。</summary>
    /// <typeparam name="TKey">字典中的键的类型。</typeparam>
    /// <typeparam name="TValue">字典中的值的类型。</typeparam>
    public class SyncDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private object m_SyncRoot;

        /// <summary>用于保存被包装的普通字典实例。</summary>
        private IDictionary<TKey, TValue> innerDictionary = null;

        private object SyncRoot
        {
            get
            {
                if (this.m_SyncRoot == null)
                {
                    Interlocked.CompareExchange(ref this.m_SyncRoot, new object(), null);
                }

                return this.m_SyncRoot;
            }
        }

        /// <summary></summary>
        public SyncDictionary()
            : this(new Dictionary<TKey, TValue>())
        {

        }

        /// <summary></summary>
        public SyncDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) { throw new ArgumentNullException("dictionary"); }
            this.innerDictionary = dictionary;
        }

        #region IDictionary<TKey,TValue> 成员
        public void Add(TKey key, TValue value)
        {
            if (!this.innerDictionary.ContainsKey(key))
            {
                lock (this.SyncRoot)
                {
                    if (!this.innerDictionary.ContainsKey(key))
                    {
                        this.innerDictionary.Add(key, value);
                    }
                }
            }
        }

        /// <summary>确定字典中是否包含指定的键。</summary>
        /// <param name="key">要在字典中定位的键。</param>
        /// <returns>如果字典中包含具有指定键的元素，则为true；否则为false。</returns>
        public bool ContainsKey(TKey key)
        {
            return this.innerDictionary.ContainsKey(key);
        }

        /// <summary>获取包含字典中的键的集合。</summary>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this.innerDictionary.Keys;
                }
            }
        }

        /// <summary>从字典中移除所指定的键的值。</summary>
        /// <param name="key">要移除的元素的键。</param>
        /// <returns>如果成功找到并移除该元素，则为 true；否则为 false。如果在字典中没有找到 key，此方法则返回 false。</returns>
        public bool Remove(TKey key)
        {
            if (this.innerDictionary.ContainsKey(key))
            {
                lock (this.SyncRoot)
                {
                    if (this.innerDictionary.ContainsKey(key))
                    {
                        return this.innerDictionary.Remove(key);
                    }
                }
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (this.SyncRoot)
            {
                return this.innerDictionary.TryGetValue(key, out value);
            }
        }

        /// <summary>获取包含字典中的值的集合。</summary>
        public ICollection<TValue> Values
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this.innerDictionary.Values;
                }
            }
        }

        /// <summary>获取与指定的键相关联的值。</summary>
        /// <param name="key">要获取的值的键。</param>
        /// <returns>与指定的键相关联的值。如果找不到指定的键，get操作便会引发<see cref="KeyNotFoundException"/>异常。</returns>
        public TValue this[TKey key]
        {
            set
            {
                lock (this.SyncRoot)
                {
                    this.innerDictionary[key] = value;
                }
            }
            get
            {
                TValue value;
                this.innerDictionary.TryGetValue(key, out value);
                return value;
            }
        }
        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> 成员
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            lock (this.SyncRoot)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)this.innerDictionary).Add(item);
            }
        }

        public void Clear()
        {
            if (this.innerDictionary.Count > 0)
            {
                lock (this.SyncRoot)
                {
                    if (this.innerDictionary.Count > 0)
                    {
                        this.innerDictionary.Clear();
                    }
                }
            }
        }

        /// <summary>确定集合中是否包含特定值。</summary>
        /// <param name="item">要在集合中定位的对象。</param>
        /// <returns>如果在集合中找到<see cref="P:item"/>，则为true；否则为false。</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.innerDictionary.Contains(item);
        }

        /// <summary>从特定的数组索引开始，将集合中的元素复制到一个数组中。</summary>
        /// <param name="array">作为从集合复制的元素的目标位置的一维数组。
        /// 该数组必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><see cref="P:array"/>中从零开始的索引，从此处开始复制。</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (this.SyncRoot)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)this.innerDictionary).CopyTo(array, arrayIndex);
            }
        }

        /// <summary>获取集合中包含的元素数。</summary>
        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        /// <summary>获取一个值，该值指示集合是否为只读。此实现总是返回true。</summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>从集合中移除特定对象的第一个匹配项。</summary>
        /// <param name="item">要从集合中移除的对象。</param>
        /// <returns>如果已从集合中成功移除<see cref="P:item"/>，则为true；否则为false。如果在原始集合中没有找到<see cref="P:item"/>，该方法也会返回 false。</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (this.SyncRoot)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)this.innerDictionary).Remove(item);
            }
        }
        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>>
        /// <summary>返回一个循环访问集合的枚举数。</summary>
        /// <returns>可用于循环访问集合的枚举数。</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.innerDictionary).GetEnumerator();
        }
        #endregion

        #region IEnumerable
        /// <summary>返回一个循环访问集合的枚举数。</summary>
        /// <returns>可用于循环访问集合的枚举数。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}