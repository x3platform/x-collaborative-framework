
namespace X3Platform.CacheBuffer
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICacheProvider
    {
        #region 属性:索引
        /// <summary>索引</summary>
        object this[string name] { get; set; }
        #endregion

        #region 函数:Contains(string name)
        /// <summary>是否包含缓存记录</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Contains(string name);
        #endregion

        #region 函数:Set(string name, object value)
        ///<summary>设置缓存记录</summary>
        ///<param name="name">名称</param>
        ///<param name="value">值</param>
        ///<returns>返回缓存对象的详细信息</returns>
        void Set(string name, object value);
        #endregion

        #region 函数:Get(string name)
        ///<summary>获取缓存记录</summary>
        ///<param name="name">名称</param>
        ///<returns>返回缓存对象的详细信息</returns>
        object Get(string name);
        #endregion

        #region 函数:Add(string name, object value)
        ///<summary>添加缓存记录</summary>
        ///<param name="name">标志</param>
        ///<param name="value">值</param>
        void Add(string name, object value);
        #endregion

        #region 函数:Add(string name, object value, int minutes)
        ///<summary>添加缓存记录</summary>
        ///<param name="name">标志</param>
        ///<param name="value">值</param>
        ///<param name="minutes">有效分钟数</param>
        void Add(string name, object value, int minutes);
        #endregion

        #region 函数:Remove(string name)
        ///<summary>移除缓存记录</summary>
        ///<param name="name">名称</param>
        void Remove(string name);
        #endregion
    }
}