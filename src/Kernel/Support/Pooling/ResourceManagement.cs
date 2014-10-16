namespace X3Platform.Pooling
{
    using System;
    using System.Collections.Generic;

    /// <summary>��Դ�أ����������������Դ��Ҳ����ȡ����</summary>
    /// <typeparam name="T"></typeparam>
    public class PoolingManagement<T> : IDisposable where T : class, IDisposable
    {
        private static PoolingManagement<T> pool;

        /// <summary>����һ����Դ�أ����õ���ģʽ��</summary>
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

        /// <summary>˽�й��캯��</summary>
        /// <param name="resourceObject"></param>
        /// <param name="maxResource"></param>
        private PoolingManagement(IResourceObject<T> resourceObject, int maxResource)
        {
            this.IResourceObject = resourceObject;

            PoolingManagement<T>.maxResource = maxResource;

            messages = new Dictionary<long, ResourceWatcher<T>>();
        }

        static object key3 = new object();

        /// <summary>����Դ������ȡ��Դ</summary>
        /// <param name="messageId">����Դ�û������messageId��������Դʱ�����������ض���Դ.</param>
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

                //��������Դ
                T message = IResourceObject.Request();

                //������Դʧ��
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

        /// <summary>�����µ���Դ��</summary>
        /// <returns></returns>
        private long NewResourceId()
        {
            return DateTime.Now.Ticks;
        }

        object key2 = new object();

        /// <summary>������Դ</summary>
        /// <param name="message">ref���͵Ĳ��������ں����ڲ���Ϊnull����ζ�ŷ��غ������á�</param>
        /// <param name="messageId">��ȡ��Դʱ�õ����Ǹ�messageId���������һ������ȷ��id,���׳��쳣��</param>
        public void ReturnResource(ref T message, long messageId)
        {
            if (!messages.ContainsKey(messageId))
            {
                throw new InvalidOperationException("��ͼ�黹һ���Ƿ�����Դ��");
            }

            T toDispose = null;

            lock (key2)
            {
                ResourceWatcher<T> tag = messages[messageId];

                tag.InUse = false;

                //��ǰ��id�����ϣ���������
                messages.Remove(messageId);

                //����ǰ��message�ÿգ���������
                message = null;

                //�ﵽ���ޣ����ͷ���Դ
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

        #region ����:IDisposable
        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool isDisposing)
        {
            foreach (long key in messages.Keys)
            {
                //�ͷ���Դ
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

        #region ��������:~PoolingManagement()
        /// <summary></summary>
        ~PoolingManagement()
        {
            Dispose(false);
        }
        #endregion
    }
}
