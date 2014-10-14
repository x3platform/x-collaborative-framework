// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :MessageStorage.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;

using X3Platform.Messages;

namespace X3Platform.CacheBuffer
{
    /// <summary>消息存储器</summary>
    public sealed class MessageStorage : ICacheStorage, ICacheable
    {
        /// <summary>默认构造函数</summary>
        public MessageStorage() { }

        /// <summary>索引</summary>
        /// <param name="key" >键</param>
        /// <returns>值</returns>
        public MessageObject this[string key]
        {
            get { return GetValue(key); }
            set { Add(key, value); }
        }

        /// <summary></summary>
        private Dictionary<string, MessageObject> messageStorage = null;

        /// <summary>抓取消息</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MessageObject Fetch(object key)
        {
            return this[key.ToString()];
        }

        /// <summary>添加存储项</summary>
        /// <param name="key">键</param>
        /// <param name= "value">值</param>
        public void Add(string key, MessageObject value)
        {
            if (messageStorage == null)
            {
                // Create the storage.
                messageStorage = new Dictionary<string, MessageObject>();
            }

            if (ContainsKey(key))
            {
                messageStorage[key] = value;
            }
            else
            {
                messageStorage.Add(key, value);
            }
        }

        /// <summary>移除存储项</summary>
        /// <param name= "key">键</param>
        public void Remove(string key)
        {
            if (ContainsKey(key))
            {
                messageStorage.Remove(key);
            }
        }

        /// <summary>
        /// Returns   the   item   associated   with   the   supplied   identifier.
        /// </summary>
        ///   <param   name= "key"> Identifier   for   the   value   to   be   returned. </param>
        ///   <returns> Item   value   corresponding   to   Key   supplied. </returns>
        ///   <remarks> Accessing   a   stored   item   in   this   way   automatically
        ///   forces   the   item   to   the   end   of   the   purge   queue. </remarks>
        public MessageObject GetValue(string key)
        {
            if (messageStorage != null && messageStorage.ContainsKey(key))
            {
                return messageStorage[key];
            }
            else
            {
                MessageObject message = new MessageObject();

                Add(key, message);

                return message;
            }
        }

        /// <summary>Determines   whether   the   cache   contains   the   specific   key.</summary>
        /// <param name="key"> Key   to   locate   in   the   cache. </param>
        /// <returns><c>true</c>if the cache contains the specified key; otherwise <c>false</c>.</returns>
        public bool ContainsKey(string key)
        {
            return (messageStorage != null && messageStorage.ContainsKey(key));
        }

        ///   <summary>
        ///   Returns   an   enumerator   that   iterates   through   the   collection   of
        ///   keys.
        ///   </summary>
        /// <returns>枚举的键</returns>
        public IEnumerable<string> GetKeys()
        {
            foreach (string key in messageStorage.Keys)
            {
                yield return key;
            }
        }

        ///   <summary>
        ///   Returns   an   enumerator   that   iterates   through   the   collection   of
        ///   values   within   the   cache.
        ///   </summary>
        /// <returns>枚举的值</returns>
        public IEnumerable<MessageObject> GetValues()
        {
            foreach (KeyValuePair<string, MessageObject> message in messageStorage)
            {
                yield return message.Value;
            }
        }

        ///   <summary>
        ///   Returns   the   <c> KeyValuePair&lt;Key,Value&gt; </c>   for   the   cache  
        ///   collection.
        ///   </summary>
        ///   <returns> The   enumerator   for   the   cache,   returning   both   the
        ///   key   and   the   value   as   a   pair. </returns>
        ///   <remarks> The   return   value   from   this   function   can   be  
        ///   thought   of   as   being   like   the   C++   Standard   Template  
        ///   Library 's   std::pair   template. </remarks>
        public IEnumerable<KeyValuePair<string, MessageObject>> GetItems()
        {
            foreach (KeyValuePair<string, MessageObject> message in messageStorage)
                yield return message;
        }

        /// <summary>
        /// The   default   enumerator   for   the   cache   collection.     Returns   an   enumerator   allowing   the   traversing   of   the   values,   much   like   the   <see   cref= "GetValues "/> .
        /// </summary>
        /// <returns> The   enumerator   for   the   values. </returns>
        public IEnumerator<MessageObject> GetEnumerator()
        {
            foreach (KeyValuePair<string, MessageObject> message in messageStorage)
            {
                yield return message.Value;
            }
        }

        /// <summary>
        /// Gets the number of items   stored   in   the   cache.
        /// </summary>
        /// <value> The   number   of   items   stored   in   the   cache.   </value>
        public int Count
        {
            get { return (messageStorage == null) ? 0 : messageStorage.Count; }
        }

        /// <summary>全部清除</summary>
        public void Clear()
        {
            messageStorage.Clear();
        }

        //
        // 显式实现 ICacheable
        // 

        #region 属性:Expires
        private DateTime m_Expires = DateTime.Now.AddHours(1);

        DateTime ICacheable.Expires
        {
            get { return m_Expires; }
            set { m_Expires = value; }
        }
        #endregion
    }
}