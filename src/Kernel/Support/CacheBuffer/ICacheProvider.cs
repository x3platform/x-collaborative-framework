
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
        /// <summary></summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string name);
        #endregion

        #region 函数:Get(string name)
        ///<summary>读取缓存记录</summary>
        ///<param name="name">名称</param>
        ///<returns>返回缓存对象的详细信息</returns>
        object Get(string name);
        #endregion

        #region 函数:Add(string name, object value)
        ///<summary>写入缓存记录</summary>
        ///<param name="key">标志</param>
        ///<param name="value">RuanYu.CacheBuffer.Model.TaskInfo Id号</param>
        void Add(string name, object value);
        #endregion

        #region 函数:Add(string name, object value, int minutes)
        void Add(string name, object value, int minutes);
        #endregion

        #region 函数:Remove(string name)
        ///<summary>查询某条记录</summary>
        ///<param name="Id">RuanYu.CacheBuffer.Model.TaskInfo Id号</param>
        ///<returns>返回一个 RuanYu.CacheBuffer.Model.TaskInfo 实例的详细信息</returns>
        void Remove(string name);
        #endregion
    }
}