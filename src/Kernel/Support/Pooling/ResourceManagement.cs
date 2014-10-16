namespace X3Platform.Pooling
{
    using System;
    using System.Collections.Generic;

    /// <summary>资源池，可以往里面加入资源，也可以取出来</summary>
    /// <typeparam name="T"></typeparam>
    public class PoolingManagement<T> : IDisposable where T : class, IDisposable
    {
        private static PoolingManagement<T> pool;

        /// <summary>返回一个资源池，采用单件模式。</summary>
        /// <param name="IResourceObject"></param>
        /// <returns></returns>
        public static PoolingManagement<T> Instance(IResourceObject<T> IResourceObject, int maxResource)
        {
            if (pool == null)
            {
                lock (key3)
                {
                    if (pool == null)
                    {
                        pool = new PoolingManagement<T>(IResourceObject, maxResource);
                    }
                }
            }

            return pool;
        }

        private IResourceObject<T> IResourceObject;

        private static int maxResource;

        /// <summary></summary>
        public static int MaxResource
        {
            get { return PoolingManagement<T>.maxResource; }
        }

        private Dictionary<long, ResourceWatcher<T>> messages;

        public int ResourceCount
        {
            get { return messages.Keys.Count; }
        }

        /// <summary>私有构造函数</summary>
        /// <param name="resourceObject"></param>
        /// <param name="maxResource"></param>
        private PoolingManagement(IResourceObject<T> resourceObject, int maxResource)
        {
            this.IResourceObject = resourceObject;

            PoolingManagement<T>.maxResource = maxResource;

            messages = new Dictionary<long, ResourceWatcher<T>>();
        }

        static object key3 = new object();

        /// <summary>从资源池中提取资源</summary>
        /// <param name="messageId">向资源用户输出的messageId，返回资源时用它来返回特定资源.</param>
        /// <returns></returns>
        public T GetResource(out long messageId)
        {
            T result = null;

            result = GetFreeResource(out messageId);

            return result;
        }

        object key1 = new object();

        private T GetFreeResource(out long messageId)
        {
            lock (key1)
            {
                foreach (long key in messages.Keys)
                {
                    if (!messages[key].InUse)
                    {
                        messages[key].InUse = true;
                        messageId = key;
                        return messages[key].Resource;
                    }
                }

                //申请新资源
                T message = IResourceObject.Request();

                //申请资源失败
                if (message == null)
                {
                    messageId = GetNullResourceId();
                    return null;
                }
                else
                {
                    ResourceWatcher<T> tag = new ResourceWatcher<T>(message, true);
                    long id = NewResourceId();
                    messages.Add(id, tag);
                    messageId = id;
                    return message;
                }
            }
        }

        private long GetNullResourceId()
        {
            return -1;
        }

        /// <summary>产生新的资源号</summary>
        /// <returns></returns>
        private long NewResourceId()
        {
            return DateTime.Now.Ticks;
        }

        object key2 = new object();

        /// <summary>返回资源</summary>
        /// <param name="message">ref类型的参数，将在函数内部设为null，意味着返回后不能再用。</param>
        /// <param name="messageId">获取资源时得到的那个messageId。如果返回一个不正确的id,将抛出异常。</param>
        public void ReturnResource(ref T message, long messageId)
        {
            if (!messages.ContainsKey(messageId))
            {
                throw new InvalidOperationException("试图归还一个非法的资源。");
            }

            T toDispose = null;

            lock (key2)
            {
                ResourceWatcher<T> tag = messages[messageId];

                tag.InUse = false;

                //当前的id将作废，不能再用
                messages.Remove(messageId);

                //将当前的message置空，不能再用
                message = null;

                //达到上限，将释放资源
                if (messages.Keys.Count >= maxResource)
                {
                    toDispose = message;
                }
                else
                {
                    messages.Add(NewResourceId(), tag);
                }
            }

            if (toDispose != null)
            {
                toDispose.Dispose();
            }
        }

        #region 函数:IDisposable
        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool isDisposing)
        {
            foreach (long key in messages.Keys)
            {
                //释放资源
                messages[key].Resource.Dispose();
            }

            if (isDisposing)
            {
                key1 = null;
                key2 = null;
                key3 = null;

                IResourceObject = null;
            }
        }
        #endregion

        #region 析构函数:~PoolingManagement()
        /// <summary></summary>
        ~PoolingManagement()
        {
            Dispose(false);
        }
        #endregion
    }
}
