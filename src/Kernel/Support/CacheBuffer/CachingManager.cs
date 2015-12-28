namespace X3Platform.CacheBuffer
{
    #region Using Libraries
    using System;

    using X3Platform.Logging;

    using X3Platform.CacheBuffer.Configuration;
    #endregion

    /// <summary></summary>
    public sealed class CacheBufferContext
    {
        #region 静态属性:Instance
        private static volatile CacheBufferContext instance = null;

        private static object lockObject = new object();

        /// <summary>实例</summary>
        public static CacheBufferContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new CacheBufferContext();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        /// <summary>缓存提供器</summary>
        private ICacheProvider cacheProvider;

        #region 构造函数:CacheBufferContext()
        private CacheBufferContext()
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
                Type objectType = Type.GetType(CacheBufferConfigurationView.Instance.CacheBufferProvider);

                this.cacheProvider = (ICacheProvider)Activator.CreateInstance(objectType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 函数:Contains(string key)
        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return this.cacheProvider.Contains(key);
        }
        #endregion

        #region 函数:Write(string key, object value, int minutes)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Read(string key)
        {
            return this.cacheProvider.Read(key);
        }
        #endregion

        #region 函数:Write(string key, object value)
        /// <summary>写入缓存项</summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Write(string key, object value)
        {
            this.cacheProvider.Write(key, value);
        }
        #endregion

        #region 函数:Write(string key, object value, int minutes)
        /// <summary>写入缓存项</summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="minutes">有效分钟</param>
        public void Write(string key, object value, int minutes)
        {
            this.cacheProvider.Write(key, value, minutes);
        }
        #endregion

        #region 函数:Delete(string key)
        /// <summary>
        /// 删除缓存项
        /// </summary>
        /// <param name="key">键</param>
        public void Delete(string key)
        {
            this.cacheProvider.Delete(key);
        }
        #endregion
    }
}