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
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace X3Platform.CacheBuffer
{
    /// <summary>缓存队列</summary>
    public sealed class CacheQueue<Key, Value>
        where Key : IComparable<Key>
        where Value : ICacheable
    {
        ///   <summary>默认构造函数</summary>
        public CacheQueue() { }

        ///   <summary>
        ///   Alternative   constructor   allowing   maximum   cache   size   to   be   specified
        ///   at   construction   time.
        ///   </summary>
        ///   <param   name= "maxLength "> Maximum   size   of   the   cache. </param>
        public CacheQueue(int maxLength)
        {
            this.maxLength = maxLength;
        }

        ///   <summary>
        ///   The   <c> maxBytes </c>   variable   stores   the   maximum   size   of
        ///   the   item   cache.     This   is   the   sum   of   all   of   the   items.     Once
        ///   this   size   is   breached.     The   cache   starts   dropping   out   items.
        ///   If   they   are   reconstructable,   then   the   items   are   destroyed   but
        ///   left   in   the   cache   allowing   them   to   come   alive   again.
        ///   </summary>
        private int maxLength = 53;

        /// <summary>
        /// 
        /// </summary>
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        /// <summary></summary>
        private Dictionary<Key, Value> cacheStorage = null;

        /// <summary></summary>
        private Queue<Key> queue = null;

        ///   <summary>
        ///   This   function   is   used   when   it   has   been   determined   that   the   cache   is
        ///   too   full   to   fit   the   new   item.     The   function   is   called   with   the   parameter
        ///   of   the   number   of   bytes   needed.     A   basic   purging   algorthim   is   used   to   make
        ///   space.
        ///   </summary>
        ///   <remarks> This   purge   function   may   be   improved   with   some   hit-count   being maintained. </remarks>
        private void Purge()
        {
            //   Purge   space   using   a   slight   modification   of   the   first-in-first-out   system.
            //   Basically,   the   first   item   added   (the   oldest)   will   be   the   first   removed,   however,
            //   any   item   accessed   is   given   a   "touch "   and   moved   to   the   end   of   the   queue.
            //   This   should   ensure   items   in   use   stay   within   the   cache   (unless   the   cache   is   too
            //   small).
            if (cacheStorage != null && queue != null && cacheStorage.Count != 0)
            {
                List<Key> list = new List<Key>();

                foreach (KeyValuePair<Key, Value> entry in cacheStorage)
                {
                    if (entry.Value.Expires < DateTime.Now)
                    {
                        list.Add(entry.Key);
                    }
                }

                foreach (Key key in list)
                {
                    Remove(key);
                }

                // 
                while (maxLength <= queue.Count + 1)
                {
                    if (queue.Count == 0)
                        break;

                    Key key = queue.Dequeue();

                    cacheStorage.Remove(key);
                }
            }
        }

        /// <summary>The   internal   function   to   store   the   items   within   the   cache.</summary>
        /// <param name= "key"> Identifier   or   key   for   the   item. </param>
        /// <param name= "value"> The   actual item   to   store/cache. </param>
        private void StoreItem(Key key, Value value)
        {
            if (cacheStorage == null)
            {
                // Create the storage.
                cacheStorage = new Dictionary<Key, Value>(maxLength);

                queue = new Queue<Key>();
            }

            queue.Enqueue(key);

            cacheStorage.Add(key, value);
        }

        /// <summary>添加缓存项</summary>
        /// <param name="key">键</param>
        /// <param   name= "value">值</param>
        public void Add(Key key, Value value)
        {
            // Check if we 're using this yet
            if (ContainsKey(key))
            {
                // Simple replacement by removing and adding again, this
                // will ensure we do the   size calculation in only one place.
                Remove(key);
            }

            // Need to get current total size and see   if   this   will   fit.
            // int projectedUsage = value.BytesUsed + this.cou;
            if (this.queue != null && this.queue.Count + 1 > maxLength)
            {
                Purge();
            }

            // 存储
            StoreItem(key, value);
        }

        ///   <summary>
        ///   Remove the specified item from the cache.
        ///   </summary>
        ///   <param   name= "key"> Identifier   for   the   item   to   remove. </param>
        public void Remove(Key key)
        {
            if (ContainsKey(key))
            {
                RemoveKeyFromQueue(key);

                cacheStorage.Remove(key);
            }
        }

        ///   <summary>
        ///   Internal   function   to   dequeue   a   specified   value.
        ///   </summary>
        ///   <param   name= "k "> Identifier   of   item   to   remove. </param>
        ///   <remarks> In   worst   case   senarios,   a   new   queue   needs   to   be   rebuilt.    
        ///   Perhaps   a   List   acting   like   a   queue   would   work   better. </remarks>
        private void RemoveKeyFromQueue(Key key)
        {
            if (queue.Contains(key))
            {
                if (queue.Peek().CompareTo(key) == 0)
                    queue.Dequeue();
                else
                {
                    Queue<Key> tempQueue = new Queue<Key>();
                    int oldQueueSize = queue.Count;
                    while (queue.Count > 0)
                    {
                        Key tempValue = queue.Dequeue();

                        if (tempValue.CompareTo(key) != 0)
                            tempQueue.Enqueue(tempValue);
                    }

                    queue = tempQueue;
                }
            }
        }

        ///   <summary>
        ///   Touch   or   refresh   a   specified   item.     This   allows   the   specified
        ///   item   to   be   moved   to   the   end   of   the   dispose   queue.     E.g.   when   it
        ///   is   known   that   this   item   would   benifit   from   not   being   purged.
        ///   </summary>
        ///   <param   name="key"> Identifier   of   item   to   touch. </param>
        //public void Touch(Key key)
        //{
        //    RemoveKeyFromQueue(key);
        //    queue.Enqueue(key);       //   Put   at   end   of   queue.
        //}

        ///   <summary>
        ///   Returns   the   item   associated   with   the   supplied   identifier.
        ///   </summary>
        ///   <param   name= "key"> Identifier   for   the   value   to   be   returned. </param>
        ///   <returns> Item   value   corresponding   to   Key   supplied. </returns>
        ///   <remarks> Accessing   a   stored   item   in   this   way   automatically
        ///   forces   the   item   to   the   end   of   the   purge   queue. </remarks>
        public Value GetValue(Key key)
        {
            if (cacheStorage != null && cacheStorage.ContainsKey(key))
            {
                // Touch(key);
                return cacheStorage[key];
            }
            else
            {
                return default(Value);
            }
        }

        /// <summary>Determines   whether   the   cache   contains   the   specific   key.</summary>
        /// <param name="key"> Key   to   locate   in   the   cache. </param>
        /// <returns><c>true</c>if the cache contains the specified key; otherwise <c>false</c>.</returns>
        public bool ContainsKey(Key key)
        {
            return (cacheStorage != null && cacheStorage.ContainsKey(key));
        }

        ///   <summary>
        ///   Indexer   into   the   cache   using   the   associated   key   to   specify
        ///   the   value   to   return.
        ///   </summary>
        ///   <param   name= "key"> Key   identifying   value   to   return. </param>
        ///   <returns> The   value   asspciated   to   the   supplied   key. </returns>
        public Value this[Key key]
        {
            get { return GetValue(key); }
            set { Add(key, value); }
        }

        ///   <summary>
        ///   Returns   an   enumerator   that   iterates   through   the   collection   of
        ///   keys.
        ///   </summary>
        /// <returns>枚举的键</returns>
        public IEnumerable<Key> GetKeys()
        {
            foreach (Key key in queue)
            {
                yield return key;
            }
        }

        ///   <summary>
        ///   Returns   an   enumerator   that   iterates   through   the   collection   of
        ///   values   within   the   cache.
        ///   </summary>
        ///   <returns>枚举的值</returns>
        public IEnumerable<Value> GetValues()
        {
            foreach (KeyValuePair<Key, Value> cache in cacheStorage)
            {
                yield return cache.Value;
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
        public IEnumerable<KeyValuePair<Key, Value>> GetItems()
        {
            foreach (KeyValuePair<Key, Value> cache in cacheStorage)
                yield return cache;
        }

        ///   <summary>
        ///   The   default   enumerator   for   the   cache   collection.     Returns   an   enumerator   allowing   the   traversing   of   the   values,   much   like   the   <see   cref= "GetValues "/> .
        ///   </summary>
        ///   <returns> The   enumerator   for   the   values. </returns>
        public IEnumerator<Value> GetEnumerator()
        {
            foreach (KeyValuePair<Key, Value> cache in cacheStorage)
            {
                yield return cache.Value;
            }
        }

        /// <summary>
        /// Gets the number of items stored in   the   cache.
        /// </summary>
        /// <value> The   number   of   items   stored   in   the   cache.   </value>
        public int Count
        {
            get { return (cacheStorage == null) ? 0 : cacheStorage.Count; }
        }

        /// <summary>清除</summary>
        public void Clear()
        {
            queue.Clear();

            cacheStorage.Clear();
        }
    }
}