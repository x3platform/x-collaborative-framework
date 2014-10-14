namespace X3Platform.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>只读的表示键和值（字典）的集合。</summary>
    /// <typeparam name="TKey">字典中的键的类型。</typeparam>
    /// <typeparam name="TValue">字典中的值的类型。</typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>用于保存被包装的普通字典实例。</summary>
        private IDictionary<TKey, TValue> innerDictionary = null;

        /// <summary>初始化只读字典类的新实例，该实例包装了一个普通的字典实例。</summary>
        /// <param name="innerDictionary">要包装的普通字典实例。</param>
        /// <remarks>
        /// <para>只读字典不会从包装的普通字典中复制元素，而是直接暴露普通字典中具有只读特征的成员。
        /// 具有只写特征的成员将被隐藏，如果强制调用，则会抛出<see cref="NotSupportedException"/>异常。</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <see cref="P:innerDictionary"/>为null。
        /// </exception>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> innerDictionary)
        {
            if (innerDictionary == null) { throw new ArgumentNullException(); }

            this.innerDictionary = innerDictionary;
        }

        #region IDictionary<TKey, TValue>
        /// <summary>
        /// 在字典中添加一个带有所提供的键和值的元素。
        /// 此实现总是引发<see cref="NotSupportedException"/>异常。
        /// </summary>
        /// <param name="key">要添加的元素的键。</param>
        /// <param name="value">要添加的元素的值。</param>
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException("对象为只读字典(ReadOnlyDictionary)，只允许查看，不允许增加、删除、修改等操作。");
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
            get { return this.innerDictionary.Keys; }
        }

        /// <summary>
        /// 从字典中移除所指定的键的值。
        /// 此实现总是引发<see cref="NotSupportedException"/>异常。
        /// </summary>
        /// <param name="key">要移除的元素的键。</param>
        /// <returns>如果成功找到并移除该元素，则为 true；否则为 false。
        /// 如果在字典中没有找到 key，此方法则返回 false。</returns>
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException("对象为只读字典(ReadOnlyDictionary)，只允许查看，不允许增加、删除、修改等操作。");
        }

        /// <summary>获取与指定的键相关联的值。</summary>
        /// <param name="key">要获取的值的键。</param>
        /// <param name="value">当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。该参数未经初始化即被传递。</param>
        /// <returns>如果字典包含具有指定键的元素，则为 true；否则为 false。</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.innerDictionary.TryGetValue(key, out value);
        }

        /// <summary>获取包含字典中的值的集合。</summary>
        public ICollection<TValue> Values
        {
            get { return this.innerDictionary.Values; }
        }

        /// <summary>获取与指定的键相关联的值。</summary>
        /// <param name="key">要获取的值的键。</param>
        /// <returns>与指定的键相关联的值。如果找不到指定的键，get操作便会引发<see cref="KeyNotFoundException"/>异常。</returns>
        public TValue this[TKey key]
        {
            get { return this.innerDictionary[key]; }
        }

        /// <summary>
        /// 获取与指定的键相关联的值。
        /// 调用set操作总是会引发<see cref="NotSupportedException"/>异常。
        /// </summary>
        /// <param name="key">要获取的值的键。</param>
        /// <returns>与指定的键相关联的值。如果找不到指定的键，
        /// get操作便会引发<see cref="KeyNotFoundException"/>异常。
        /// 调用set操作总是会引发<see cref="NotSupportedException"/>异常。</returns>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return this[key]; }
            set { throw new NotSupportedException("对象为只读字典(ReadOnlyDictionary)，只允许查看，不允许增加、删除、修改等操作。"); }
        }
        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>>
        /// <summary>
        /// 在字典中添加一个带有所提供的键和值的元素。
        /// 此实现总是引发<see cref="NotSupportedException"/>异常。
        /// </summary>
        /// <param name="item">要添加的元素的键和值。</param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("对象为只读字典(ReadOnlyDictionary)，只允许查看，不允许增加、删除、修改等操作。");
        }

        /// <summary>
        /// 从字典中移除所有项。
        /// 此实现总是引发<see cref="NotSupportedException"/>异常。
        /// </summary>
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException("对象为只读字典(ReadOnlyDictionary)，只允许查看，不允许增加、删除、修改等操作。");
        }

        /// <summary>确定集合中是否包含特定值。</summary>
        /// <param name="item">要在集合中定位的对象。</param>
        /// <returns>如果在集合中找到<see cref="P:item"/>，则为true；否则为false。</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.innerDictionary.Contains(item);
        }

        /// <summary>从特定的数组索引开始，将集合中的元素复制到一个数组中。</summary>
        /// <param name="array">作为从集合复制的元素的目标位置的一维数组。
        /// 该数组必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><see cref="P:array"/>中从零开始的索引，从此处开始复制。</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.innerDictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>获取集合中包含的元素数。</summary>
        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        /// <summary>获取一个值，该值指示集合是否为只读。此实现总是返回true。</summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// 从集合中移除特定对象的第一个匹配项。
        /// 此实现总是引发<see cref="NotSupportedException"/>异常。
        /// </summary>
        /// <param name="item">要从集合中移除的对象。</param>
        /// <returns>如果已从集合中成功移除<see cref="P:item"/>，则为true；否则为false。如果在原始集合中没有找到<see cref="P:item"/>，该方法也会返回 false。</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("对象为只读字典(ReadOnlyDictionary)，只允许查看，不允许增加、删除、修改等操作。");
        }
        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>>
        /// <summary>返回一个循环访问集合的枚举数。</summary>
        /// <returns>可用于循环访问集合的枚举数。</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.innerDictionary.GetEnumerator();
        }
        #endregion

        #region IEnumerable
        /// <summary>返回一个循环访问集合的枚举数。</summary>
        /// <returns>可用于循环访问集合的枚举数。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.innerDictionary).GetEnumerator();
        }
        #endregion
    }
}
