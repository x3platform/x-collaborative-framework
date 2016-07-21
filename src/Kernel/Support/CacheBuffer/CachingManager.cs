namespace X3Platform.CacheBuffer
{
    #region Using Libraries
    using System;

    using X3Platform.Logging;

    using X3Platform.CacheBuffer.Configuration;
    using System.Collections.Generic;
    #endregion

    /// <summary></summary>
    public sealed class CachingManager
    {
        #region 静态属性:Instance
        private static volatile CachingManager instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        private static CachingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new CachingManager();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        /// <summary>缓存提供器</summary>
        private ICacheProvider cacheProvider;

        #region 构造函数:CachingManager()
        private CachingManager()
        {
            Reload();
        }
        #endregion

        #region 函数:Reload()
        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            try
            {
                Type objectType = Type.GetType(CacheBufferConfigurationView.Instance.CacheProvider);

                this.cacheProvider = (ICacheProvider)Activator.CreateInstance(objectType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 函数:Contains(string name)
        /// <summary>包含</summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static bool Contains(string name)
        {
            return Instance.cacheProvider.Contains(name);
        }
        #endregion

        #region 函数:Get(string name)
        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object Get(string name)
        {
            return Instance.cacheProvider.Get(name);
        }
        #endregion

        #region 函数:Get(string name)
        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Get<T>(string name)
        {
            return (T)Instance.cacheProvider.Get(name);
        }
        #endregion

        #region 函数:Get(string name)
        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Get<TKey, TValue>(string name)
        {
            return (IDictionary<TKey, TValue>)Instance.cacheProvider.Get(name);
        }
        #endregion

        #region 函数:Set(string name, object value)
        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void Set(string name, object value)
        {
            Instance.cacheProvider.Set(name, value);
        }
        #endregion

        #region 函数:Add(string name, object value)
        /// <summary>写入缓存项</summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public static void Add(string name, object value)
        {
            Instance.cacheProvider.Add(name, value);
        }
        #endregion

        #region 函数:Add(string name, object value, int minutes)
        /// <summary>写入缓存项</summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="minutes">有效分钟</param>
        public static void Add(string name, object value, int minutes)
        {
            Instance.cacheProvider.Add(name, value, minutes);
        }
        #endregion

        #region 函数:Remove(string name)
        /// <summary>删除缓存项</summary>
        /// <param name="name">名称</param>
        public static void Remove(string name)
        {
            Instance.cacheProvider.Remove(name);
        }
        #endregion
    }
}